
namespace Foundation.SiteConfig
{
    internal class SiteConfig<T> : BaseSiteConfig<T> where T : class, new()
    {
        /// <summary>
        /// 指定类型的配置文件实例
        /// </summary>
        public static T Instance
        {
            get
            {
                return GetConfig() as T;
            }
        }
    }
}