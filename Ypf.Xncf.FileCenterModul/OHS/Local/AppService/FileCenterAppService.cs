using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET;
using Senparc.CO2NET.WebApi;
using Senparc.Ncf.Core.AppServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ypf.Xncf.FileCenterModul.Domain.Services;
using Ypf.Xncf.FileCenterModul.OHS.Local.PL;

namespace Ypf.Xncf.FileCenterModul.OHS.Local.AppService
{
    public class FileCenterAppService : AppServiceBase
    {
        private readonly FileCenterService _fileCenterService;

        public FileCenterAppService(IServiceProvider serviceProvider, FileCenterService fileCenterService) : base(serviceProvider)
        {
            _fileCenterService = fileCenterService;
        }


        /// <summary>
        /// 上传文件并返回文件ID
        /// </summary>
        /// <returns></returns>
        [ApiBind(ApiRequestMethod = ApiRequestMethod.Post)]
        public async Task<AppResponseBase> Upload([FromForm] List<IFormFile> files)
        {
            return await this.GetResponseAsync<AppResponseBase<string>, string>(async (resp, logger) =>
            {
                var (res, msg, fileId) = await _fileCenterService.Upload(files);
                resp.ErrorMessage = msg;
                if (res)
                {
                    resp.Success = true;
                }
                else
                {
                    resp.Success = false;
                }
                return fileId;
            });
        }

        /// <summary>
        /// 根据文件ID获取文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        [ApiBind(ApiRequestMethod = ApiRequestMethod.Get)]
        public async Task<AppResponseBase> GetFile(string fileId)
        {
            return await this.GetResponseAsync<AppResponseBase<FileCenterResp>, FileCenterResp>(async (resp, logger) =>
                {
                    var (res, msg, fileName, fileSize, filecenter) = await _fileCenterService.GetFile(fileId);
                    resp.ErrorMessage = msg;
                    if (res)
                    {
                        resp.Success = true;
                        return new FileCenterResp { FileContent = filecenter, FileName = fileName, FileSize = fileSize };
                    }
                    else
                    {
                        resp.Success = false;
                        return new FileCenterResp { FileContent = null, FileName = fileName, FileSize = fileSize };
                    }
                });
        }

        /// <summary>
        /// 根据文件ID删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        [ApiBind(ApiRequestMethod = ApiRequestMethod.Get)]
        public async Task<AppResponseBase> Remove(string fileId)
        {
            return await this.GetResponseAsync<AppResponseBase<string>, string>(async (resp, logger) =>
            {
                var (res, msg) = await _fileCenterService.Remove(fileId);
                if (res)
                {
                    resp.Success = true;
                }
                else
                {
                    resp.Success = false;
                }
                return msg;
            });
        }
    }
}
