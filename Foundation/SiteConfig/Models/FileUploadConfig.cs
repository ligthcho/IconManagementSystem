
namespace Foundation.SiteConfig.Models
{
    /// <summary>
    /// 文件上传配置
    /// </summary>
    public class FileUploadConfig
    {
        /// <summary>
        /// 可上传文件的扩展名
        /// </summary>
        public string AllowExtension;
        /// <summary>
        /// 可上传文件的大小（K）
        /// </summary>
        public int MaxSize;
        /// <summary>
        /// 文件保存目录
        /// </summary>
        public string SavePath;
    }
}