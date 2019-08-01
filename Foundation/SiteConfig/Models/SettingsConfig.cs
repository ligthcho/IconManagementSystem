
namespace Foundation.SiteConfig.Models
{
    /// <summary>
    /// SettingsConfig
    /// </summary>
    public class SettingsConfig
    {
        /// <summary>
        /// 后台管理员登录时保存的cookie的key
        /// </summary>
        public string AdminLoginCookieKey;
        /// <summary>
        /// 前台用户登录时保存的cookie的key
        /// </summary>
        public string WebLoginCookieKey;
		/// <summary>
		/// 后台管理员登录时保存的session的key
		/// </summary>
		public string LoginSessionKey;
		/// <summary>
		/// 站点名称
		/// </summary>
		public string SiteName;
        /// <summary>
        /// 超级管理员账号（此账号拥有后台管理的所有权限）
        /// </summary>
        public string SuperAdminAccount;
        /// <summary>
        /// 超级管理员密码
        /// </summary>
        public string SuperAdminPassword;
        /// <summary>
        /// 默认页
        /// </summary>
        public string DefaultPage;
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version;
    }
}
