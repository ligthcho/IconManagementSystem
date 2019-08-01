using System;

namespace Foundation.SiteConfig
{
    /// <summary>
    /// 配置文件基类
    /// </summary>
    /// <typeparam name="T">配置文件实体类</typeparam>
    internal class BaseSiteConfig<T> where T : class, new()
    {
        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="config">配置信息类</param>
        internal static void UpdateConfig(T config)
        {
            Utility.ConfigHelper.UpdateConfig<T>(config);
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        internal static T GetConfig()
        {
            bool cacheConfig = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CacheConfig"]);
            if (!cacheConfig)
            {
                return Utility.ConfigHelper.GetConfig<T>();
            }
            else
            {
            Type configClassType = typeof(T);
            string configCacheKey = "CK_SiteConfigCode_" + configClassType.Name;
            object configObject = Utility.CacheHelper.Get(configCacheKey);

            if (configObject == null)
            {
                configObject = Utility.ConfigHelper.GetConfig<T>();
                Utility.CacheHelper.Insert(configCacheKey, configObject);
            }

            var config = configObject as T;
            if (config == null)
            {
                return new T();
            }
            return config;
            }
        }
    }
}