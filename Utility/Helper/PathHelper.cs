using System;
using System.Web;

namespace Utility
{
	public class PathHelper
	{
		/// <summary>
		/// 网站根目录
		/// </summary>
		public static string BasePath
		{
			get
			{
				if(HttpContext.Current != null)
				{
					return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath);
				}
				return HttpRuntime.AppDomainAppVirtualPath != null && HttpRuntime.AppDomainAppVirtualPath.EndsWith("/")
					? HttpRuntime.AppDomainAppVirtualPath
					: HttpRuntime.AppDomainAppVirtualPath + "/";
			}
		}
		/// <summary>
		/// 获得当前绝对路径
		/// </summary>
		/// <param name="strPath">指定的路径</param>
		/// <returns>绝对路径</returns>
		public static string GetMapPath(string strPath)
		{
			if(strPath.ToLower().StartsWith("http://"))
			{
				return strPath;
			}
			if(HttpContext.Current != null)
			{
				return HttpContext.Current.Server.MapPath(strPath);
			}
			else //非web程序引用
			{
				strPath = strPath.Replace("/","\\");
				if(strPath.StartsWith("\\"))
				{
					strPath = strPath.TrimStart('\\');
				}
				else if(strPath.StartsWith("~"))
				{
					strPath = strPath.Substring(1).TrimStart('\\');
				}
				return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,strPath);
			}
		}

	}
}