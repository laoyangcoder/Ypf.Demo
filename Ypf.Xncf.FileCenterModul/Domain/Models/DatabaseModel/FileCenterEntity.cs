using Senparc.Ncf.Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ypf.Xncf.FileCenterModul
{
    /// <summary>
    /// 文件中心 实体类
    /// </summary>
    [Table(Register.DATABASE_PREFIX + "FileCenter")]//必须添加前缀，防止全系统中发生冲突
    [Serializable]
    public class FileCenterEntity : EntityBase<int>
    {
        /// <summary>
        /// 桶名
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get;  set; }

        /// <summary>
        /// 文件GUID
        /// </summary>
        public string GuidName { get;  set; }=Guid.NewGuid().ToString("N");

        /// <summary>
        /// 文件长度
        /// </summary>
        public long Size { get;  set; }
        
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string EXpandedName { get; set; }
    }
}
