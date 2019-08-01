using Foundation.SiteConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Handler
{
	/// <summary>
	/// 上传文件夹的文件处理
	/// </summary>
	public class UploadFilesHandler:IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
		public void ProcessRequest(HttpContext context)
		{
			Process(context);
		}

		public static void Process(HttpContext context)
		{
			//是否开启登录验证（是否登录(前台或后台)后才能下载文件(~/UploadFiles)）
			bool LoginValidation = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["LoginValidation"]);
			//是否开启防盗链
			bool AntitheftLink = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AntitheftLink"]);

			if(LoginValidation)
			{
				string FormsCookieNameAdmin = AllConfigServices.SettingsConfig.AdminLoginCookieKey;
				string FormsCookieNameWeb = AllConfigServices.SettingsConfig.WebLoginCookieKey;
				var cookieAdmin = HttpContext.Current.Request.Cookies[FormsCookieNameAdmin];
				var cookieWeb = HttpContext.Current.Request.Cookies[FormsCookieNameWeb];
				if(cookieAdmin == null && cookieWeb == null)
				{
					ReturnErr(context);
					return;
				}
			}

			if(AntitheftLink)
			{
				string host = System.Configuration.ConfigurationManager.AppSettings["Host"];
				//如果不是本地引用，则是盗链本站图片
				if(context.Request.UrlReferrer == null || (context.Request.UrlReferrer.Host != "localhost" && context.Request.UrlReferrer.Host != host))
				{
					ReturnErr(context);
					return;
				}
			}

			ReturnOk(context);
			return;
		}

		private static void ReturnOk(HttpContext context)
		{
			string extension = System.IO.Path.GetExtension(context.Request.Url.ToString()).ToLower();
			string[] imgExtension = new string[] { ".jpg",".jpeg",".png",".bmp",".gif" };

			//设置客户端缓冲时间过期时间为0，即立即过期
			context.Response.Expires = 0;
			//清空服务器端为此会话开启的输出缓存
			context.Response.Clear();
			//设置输出文件类型
			if(imgExtension.Contains(extension))
				context.Response.ContentType = "image/jpg";
			else
				context.Response.ContentType = "application/octet-stream";
			//将请求文件写入到输出缓存中
			context.Response.WriteFile(context.Server.MapPath(HttpUtility.UrlDecode(context.Request.Url.PathAndQuery)));
			//将输出缓存中的信息传送到客户端
			context.Response.End();
		}

		private static void ReturnErr(HttpContext context)
		{
			//设置客户端缓冲时间过期时间为0，即立即过期
			context.Response.Expires = 0;
			//清空服务器端为此会话开启的输出缓存
			context.Response.Clear();
			//设置输出文件类型
			context.Response.ContentType = "image/jpeg";
			//将请求文件写入到输出缓存中
			context.Response.WriteFile(context.Request.PhysicalApplicationPath + "/Content/images/403.png");
			//将输出缓存中的信息传送到客户端
			context.Response.End();
		}
	}
}