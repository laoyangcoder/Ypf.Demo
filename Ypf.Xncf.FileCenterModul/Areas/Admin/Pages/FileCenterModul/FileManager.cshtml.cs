using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Ncf.Service;
using Senparc.Ncf.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ypf.Xncf.FileCenterModul.Domain.Services;

namespace Ypf.Xncf.FileCenterModul.Areas.FileCenterModul.Pages
{
    public class FileManager : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        private readonly FileCenterService fileCenterService;

        public FileManager(Lazy<XncfModuleService> xncfModuleService, FileCenterService fileCenterService) : base(xncfModuleService)
        {
            this.fileCenterService = fileCenterService;
        }

        public void OnGet()
        {
        }

        /// <summary>
        /// 文件列表
        /// </summary>
        /// <param name="fileFullName">文件名</param>
        /// <param name="fileId">文件id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetList(string fileFullName, string fileId, int pageIndex, int pageSize)
        {
            var seh = new SenparcExpressionHelper<FileCenterEntity>();
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(fileFullName), _ => _.FileName.Contains(fileFullName));
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(fileId), _ => _.GuidName == fileId);
            var where = seh.BuildWhereExpression();
            var reslut = await this.fileCenterService.GetObjectListAsync(pageIndex, pageSize, where, "AddTime Desc");
            return Ok(new
            {
                reslut.TotalCount,
                reslut.PageIndex,
                List = reslut.Select(f => new { FileName = f.FileName, FileId = f.GuidName, Size = f.Size, EXpandedName = f.EXpandedName }).ToList()
            });
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDownLoad(string fileId)
        {
            var reslut = await this.fileCenterService.GetFile(fileId);
            if (reslut.res)
            {
                return Ok(new
                {
                    res = true,
                    msg = reslut.msg,
                    filenName = reslut.fileName,
                    fileSize = reslut.fileSize,
                    fileContent = reslut.FileContent,
                    extName = Path.GetExtension(reslut.fileName)
                });
            }
            else
            {
                return Ok(new { res = false, msg = reslut.msg });
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDelete(string fileId)
        {
            var result = await this.fileCenterService.Remove(fileId);
            return Ok(new
            {
                res = result.res,
                msg = result.msg
            });
        }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUpload()
        {
            var reslut = await this.fileCenterService.Upload(UploadFiles);
            if (reslut.res)
            {
                return Ok(new
                {
                    res = true,
                    msg = reslut.msg,
                    fileId = reslut.fileId
                });
            }
            else
            {
                return Ok(new
                {
                    res = true,
                    msg = reslut.msg
                });
            }
        }
    }
}
