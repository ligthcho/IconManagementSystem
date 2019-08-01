
namespace Foundation.SiteConfig.Models
{
    /// <summary>
    /// 文件上传配置
    /// </summary>
    public class UploadConfig
    {
        /// <summary>
        /// 可上传文件的扩展名
        /// </summary>
        public string FileExtension;
        /// <summary>
        /// 可上传文件的大小(单位：KB)
        /// </summary>
        public int FileSize;
        /// <summary>
        /// 可上传图片的扩展名
        /// </summary>
        public string ImageExtension;
        /// <summary>
        /// 可上传图片的大小(单位：KB)
        /// </summary>
        public int ImageSize;
        /// <summary>
        /// 可上传语音的扩展名
        /// </summary>
        public string VoiceExtension;
        /// <summary>
        /// 可上传语音的大小(单位：KB)
        /// </summary>
        public int VoiceSize;
        /// <summary>
        /// 可上传视频的扩展名
        /// </summary>
        public string VideoExtension;
        /// <summary>
        /// 可上传视频的大小(单位：KB)
        /// </summary>
        public int VideoSize;
    }
}