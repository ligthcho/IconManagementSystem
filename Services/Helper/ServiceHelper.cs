using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Utility;

namespace Services.Helper
{
	public static class ServiceHelper
	{
		/// <summary>
		/// 取得可作为主键值的字符串(16位GUID)
		/// </summary>
		/// <returns></returns>
		public static string GetKeyNum()
		{
			return StringHelper.GuidTo16String();
		}

		/// <summary>
		/// 取得可作为随机数值
		/// </summary>
		/// <returns></returns>
		public static string GetRandom()
		{
			return StringHelper.GetRandom();//+DateTime.Now.ToString("yyyyMMddHHmmssfff");
		}
		/// <summary>
		/// 取得客户端IP
		/// </summary>
		/// <returns></returns>
		public static string GetClientIp()
		{
			string ip = string.Empty;
			if(HttpContext.Current != null)
			{
				ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if(string.IsNullOrEmpty(ip))
				{
					ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}
				if(string.IsNullOrEmpty(ip))
				{
					string strHostName = Dns.GetHostName();
					ip = Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
				}
				if(string.IsNullOrEmpty(ip))
				{
					ip = HttpContext.Current.Request.UserHostAddress;
				}
			}
			return ip;
		}

	}
}
