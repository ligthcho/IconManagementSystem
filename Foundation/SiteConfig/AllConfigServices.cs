using Foundation.SiteConfig.Models;

namespace Foundation.SiteConfig
{
    /// <summary>
    /// 所有配置文件服务类（默认使用缓存，缓存一小时）
    /// </summary>
    public class AllConfigServices
    {
        /// <summary>
        /// 网站配置
        /// </summary>
        public static SettingsConfig SettingsConfig
        {
            get
            {
                return SiteConfig<SettingsConfig>.Instance;
            }
        }
        /// <summary>
        /// 图片上传配置
        /// </summary>
        public static ImageUploadBaseConfig ImageUploadConfig
        {
            get
            {
                return SiteConfig<ImageUploadBaseConfig>.Instance;
            }
        }
        ///// <summary>
        ///// 新闻图片上传配置
        ///// </summary>
        //public static NewsImageUploadConfig NewsImageUploadConfig
        //{
        //    get
        //    {
        //        return SiteConfig<NewsImageUploadConfig>.Instance;
        //    }
        //}
        /// <summary>
        /// 文件上传配置
        /// </summary>
        public static FileUploadConfig FileUploadConfig
        {
            get
            {
                return SiteConfig<FileUploadConfig>.Instance;
            }
        }
    }
}