using Foundation.SiteConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Utility;

namespace Services
{
	public class SysFormsAuthenticationHelper<T> where T:class
	{
		private readonly static string FormsCookieName = AllConfigServices.SettingsConfig.AdminLoginCookieKey;
		private readonly static string LOGIN_SESSION_KEY = AllConfigServices.SettingsConfig.AdminLoginCookieKey;//AllConfigServices.SettingsConfig.LoginSessionKey;
		/// <summary>
		/// 创建身份验证票据
		/// </summary>
		/// <param name="userId">用户id</param>
		/// <param name="t">用户信息实例</param>
		public static bool SetAuthCookie(string userId,T t)
		{
			try
			{
				string userData = Serializer.JsonSerialize(t);
				userData = userData.Length > 800 ? string.Empty : userData; //防止最终存储的cookie的值大小超过浏览器限制
				FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
						 (1,
							 userId,
							 DateTime.Now,
							 DateTime.Now.AddHours(12),
							 true,
							 userData,
							 "/"
						 );
				var cookie = new HttpCookie(FormsCookieName,FormsAuthentication.Encrypt(ticket));
				cookie.HttpOnly = true;
				cookie.Expires = DateTime.Now.AddHours(12);
				HttpContext.Current.Response.Cookies.Add(cookie);

				HttpContext.Current.Session[LOGIN_SESSION_KEY] = t;
				return true;
			}
			catch(Exception ex)
			{
				return false;
			}
		}

		/// <summary>
		/// 创建身份验证票据
		/// </summary>
		/// <param name="userId">用户id</param>
		/// <param name="t">用户信息实例</param>
		public static bool SetAuthSession(string userId,T t)
		{
			try
			{
				string userData = Serializer.JsonSerialize(t);
				userData = userData.Length > 800 ? string.Empty : userData; //防止最终存储的cookie的值大小超过浏览器限制
				HttpContext.Current.Session[LOGIN_SESSION_KEY] = userData;
				return true;
			}
			catch(Exception ex)
			{
				return false;
			}
		}

		/// <summary>
		/// 获取与 Forms 身份验证票相关联的用户Id
		/// </summary>
		/// <returns></returns>
		public static string GetUserId()
		{
			var ticket = GetTiket();
			return ticket == null ? null : ticket.Name;
		}

		/// <summary>
		/// 获取一个存储在票证中的用户信息的实例
		/// </summary>
		/// <returns></returns>
		public static T GetUserInstance()
		{
			var ticket = GetTiket();
			if(ticket == null)
				return null;
			else if(string.IsNullOrEmpty(ticket.UserData))
			{
				string userId = GetUserId();
				return AllServices.UserService.GetByUserID(userId) as T;
			}
			return Serializer.JsonDeserialize<T>(ticket.UserData);
		}
		/// <summary>
		/// 获取一个存储在Session中的用户信息的实例
		/// </summary>
		/// <returns></returns>
		public static T GetUserSession()
		{
			var ticket = HttpContext.Current.Session[LOGIN_SESSION_KEY];
			if(ticket == null)
				return null;
			return Serializer.JsonDeserialize<T>(ticket.ToStr());
		}
		private static FormsAuthenticationTicket GetTiket()
		{
			var cookie = HttpContext.Current.Request.Cookies[FormsCookieName];
			return cookie == null ? null : FormsAuthentication.Decrypt(cookie.Value);//网络不好报错 
		}

		/// <summary>
		/// 登出
		/// </summary>
		public static void SignOut()
		{
			var cookie = HttpContext.Current.Response.Cookies.Get(FormsCookieName);
			cookie.Expires = DateTime.Now.AddDays(-1);
			HttpContext.Current.Session[LOGIN_SESSION_KEY] = null;
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

	}
}
