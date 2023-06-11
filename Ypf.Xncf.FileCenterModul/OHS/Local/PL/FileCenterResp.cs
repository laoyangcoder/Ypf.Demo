namespace Ypf.Xncf.FileCenterModul.OHS.Local.PL
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileCenterResp
    {
        public long FileSize { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
