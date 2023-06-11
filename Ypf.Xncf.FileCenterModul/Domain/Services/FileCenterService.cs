using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.Exceptions;
using Senparc.CO2NET;
using Senparc.CO2NET.WebApi;
using Senparc.Ncf.Repository;
using Senparc.Ncf.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ypf.Xncf.FileCenterModul.Domain.Services
{
    public class FileCenterService : ServiceBase<FileCenterEntity>
    {
        private readonly IConfiguration _configuration;
        private readonly MinioClient _minioClient;
        private readonly string bucketName = "defalut";

        public FileCenterService(IRepositoryBase<FileCenterEntity> repo, IServiceProvider serviceProvider, IConfiguration configuration)
            : base(repo, serviceProvider)
        {
            _configuration = configuration;

            #region 1、 实例化minioclient客户端
            bucketName = _configuration.GetValue<string>("Minio:bucketName");
            var endpoint = _configuration.GetValue<string>("Minio:endpoint");
            var accessKey = _configuration.GetValue<string>("Minio:accessKey");
            var secretKey = _configuration.GetValue<string>("Minio:secretKey");
            var secure = _configuration.GetValue<bool>("Minio:secure");
            _minioClient = new MinioClient().WithEndpoint(endpoint).WithCredentials(accessKey, secretKey).WithSSL(secure).Build();
            #endregion
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<(bool res, string msg,string fileId)> Upload(List<IFormFile> files)
        {
            if (files.Count > 0)
            {
                foreach (var item in files)
                {
                    try
                    {
                        //2、创建文件桶
                        // Make a bucket on the server, if not already present.
                        var beArgs = new BucketExistsArgs().WithBucket(bucketName);
                        bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
                        if (!found)
                        {
                            var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                            await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                        }

                        //3、上传文件

                        var newFileGuid = Guid.NewGuid().ToString("N");// 文件ID
                        var expandedName = Path.GetExtension(item.FileName);// 文件扩展名

                        // 方式1 【已废弃】：PutObjectAsync(桶名,文件名,文件流,文件大小)
                        //await minioClient.PutObjectAsync(bucketName, item.FileName, item.OpenReadStream(), item.Length);

                        // 方式2：
                        PutObjectArgs putObjectArgs = new PutObjectArgs()
                                      .WithBucket(bucketName)// 桶名
                                      .WithObject($"{newFileGuid}{expandedName}")// 文件名
                                      .WithObjectSize(item.Length)// 文件大小
                                      .WithStreamData(item.OpenReadStream())// 文件流
                                      .WithContentType("application/octet-stream");// 设置ContentType为流
                        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

                        var file = new FileCenterEntity { BucketName = bucketName, FileName = item.FileName, GuidName = newFileGuid, Size = item.Length, EXpandedName = expandedName };
                        await base.SaveObjectAsync(file).ConfigureAwait(false);
                        return (true, $"文件[{item.FileName}]上传成功",newFileGuid);
                    }
                    catch (MinioException e)
                    {
                        return (false, e.Message,"");
                    }
                }

            }
            return (false, "无上传文件","");
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        public async Task<(bool res, string msg)> Remove(string fileId)
        {

            if (!string.IsNullOrEmpty(fileId))
            {
                // 1、数据库中是否有文件ID
                var fileDb = await base.GetObjectAsync(x => x.GuidName == fileId);
                if (fileDb == null)
                {
                    return (false, $"文件ID[{fileId}]无数据");
                }
                try
                {

                    //2、删除文件
                    var args = new RemoveObjectArgs()
                            .WithBucket(bucketName)
                            .WithObject($"{fileDb.GuidName}{fileDb.EXpandedName}");
                    await _minioClient.RemoveObjectAsync(args).ConfigureAwait(false);
                    await base.DeleteObjectAsync(fileDb).ConfigureAwait(false);
                    return (true, $"文件[{fileId}]删除成功");
                }
                catch (MinioException e)
                {
                    return (false, e.Message);
                }


            }
            return (false, "文件ID不能为空");
        }

        /// <summary>
        /// 根据文件ID获取文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        public async Task<(bool res, string msg,string fileName,long fileSize, byte[] FileContent)> GetFile(string fileId)
        {

            if (!string.IsNullOrEmpty(fileId))
            {
                // 1、数据库中是否有文件ID
                var fileDb = await base.GetObjectAsync(x => x.GuidName == fileId);
                if (fileDb == null)
                {
                    return (false, $"文件ID[{fileId}]无数据","",0, null);
                }
                try
                {
                    //2、文件是否存在检查
                    try
                    {
                        StatObjectArgs statObjectArgs = new StatObjectArgs()
                                                   .WithBucket(bucketName)
                                                   .WithObject($"{fileDb.GuidName}{fileDb.EXpandedName}");
                        await _minioClient.StatObjectAsync(statObjectArgs);
                    }
                    catch (System.Exception e)
                    {
                        return (false, $"文件ID[{fileId}]数据不存在!异常信息：: {e.Message}","",0, null);
                    }

                    //3、创建内存流
                    using (var fileStream = new MemoryStream())
                    {
                        //3、获取文件
                        var args = new GetObjectArgs()
                                .WithBucket(bucketName)
                                .WithObject($"{fileDb.GuidName}{fileDb.EXpandedName}")
                               // 文件操作委托：将文件拷贝到内存流中
                               .WithCallbackStream((stream) =>
                               {
                                   stream.CopyTo(fileStream);
                               });

                        var r = await _minioClient.GetObjectAsync(args).ConfigureAwait(false);

                        fileStream.Position = 0L;
                        var fileContent = fileStream.ToArray();
                        fileStream.Flush();

                        return (true, $"文件[{fileId}]获取成功", fileDb.FileName,fileDb.Size,fileContent);
                    }
                }
                catch (MinioException e)
                {
                    return (false, e.Message,"",0, null);
                }


            }
            return (false, "文件ID不能为空", "",0,null);
        }
    }
}
