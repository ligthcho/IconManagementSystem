using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Management;
using System.Net;

namespace Model
{
	public class ClientModel
	{
		#region 基本信息
		/// <summary>
		/// 客户端信息索引ID
		/// </summary>
		private string m_ID = string.Empty;

		/// <summary>
		/// 客户端信息索引ID
		/// </summary>
		public string ID
		{
			get
			{
				return m_ID;
			}
			set
			{
				m_ID = value;
			}
		}

		/// <summary>
		/// 时间
		/// </summary>
		private DateTime m_DateTime = DateTime.Now;

		/// <summary>
		/// 时间
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				return m_DateTime;
			}
			set
			{
				m_DateTime = value;
			}
		}

		/// <summary>
		/// 类型
		/// </summary>
		private ClientType m_ClientType = ClientType.UNKNOWN;

		/// <summary>
		/// 类型
		/// </summary>
		public ClientType ClientType
		{
			get
			{
				return m_ClientType;
			}
			set
			{
				m_ClientType = value;
			}
		}
		#endregion 基本信息

		#region 请求信息
		/// <summary>
		/// 客户端请求的地址
		/// </summary>
		private string m_Url = GetUrl();

		/// <summary>
		/// IP地址
		/// </summary>
		public string Url
		{
			get
			{
				return m_Url;
			}
			set
			{
				m_Url = value;
			}
		}
		#endregion 请求信息

		#region 特征信息
		/// <summary>
		/// IP地址
		/// </summary>
		private string m_IPAddress = GetIPAdderss();

		/// <summary>
		/// IP地址
		/// </summary>
		public string IPAddress
		{
			get
			{
				return m_IPAddress;
			}
			set
			{
				m_IPAddress = value;
			}
		}

		/// <summary>
		/// 设备名
		/// </summary>
		private string m_HostName = GetHostName();

		/// <summary>
		/// 设备名
		/// </summary>
		public string HostName
		{
			get
			{
				return m_HostName;
			}
			set
			{
				m_HostName = value;
			}
		}

		/// <summary>
		/// MAC地址
		/// </summary>
		private string m_MAC = GetMac();

		/// <summary>
		/// MAC地址
		/// </summary>
		public string MAC
		{
			get
			{
				return m_MAC;
			}
			set
			{
				m_MAC = value;
			}
		}

		/// <summary>
		/// 其他特征信息Json
		/// </summary>
		private string m_OrderTraitsJson = string.Empty;

		/// <summary>
		/// 其他特征信息Json
		/// </summary>
		public string OrderTraitsJson
		{
			get
			{
				return m_OrderTraitsJson;
			}
			set
			{
				m_OrderTraitsJson = value;
			}
		}
		#endregion 特征信息

		#region 许可
		/// <summary>
		/// 客户端许可
		/// </summary>
		private string m_License = string.Empty;

		/// <summary>
		/// 客户端许可
		/// </summary>
		public string License
		{
			get
			{
				return m_License;
			}
			set
			{
				m_License = value;
			}
		}

		/// <summary>
		/// 预留信息1
		/// </summary>
		private string m_OtherAudit1 = string.Empty;

		/// <summary>
		/// 预留信息1
		/// </summary>
		public string OtherAudit1
		{
			get
			{
				return m_OtherAudit1;
			}
			set
			{
				m_OtherAudit1 = value;
			}
		}

		/// <summary>
		/// 预留信息2
		/// </summary>
		private string _otherAudit2 = string.Empty;

		/// <summary>
		/// 预留信息2
		/// </summary>
		public string OtherAudit2
		{
			get
			{
				return _otherAudit2;
			}
			set
			{
				_otherAudit2 = value;
			}
		}

		/// <summary>
		/// 预留信息S
		/// </summary>
		private string m_Others = string.Empty;

		/// <summary>
		/// 预留信息S
		/// </summary>
		public string Others
		{
			get
			{
				return m_Others;
			}
			set
			{
				m_Others = value;
			}
		}
		#endregion 许可

		/// <summary>
		/// 得到当前Http请求的Url地址
		/// </summary>
		public static string GetUrl()
		{
			string strUrl = HttpContext.Current.Request.Path;
			try
			{
				strUrl = HttpContext.Current.Request.Url.AbsoluteUri;
			}
			catch
			{
			}
			return strUrl;
		}

		/// <summary>
		/// 得到当前Http请求IP地址
		/// </summary>
		public static string GetIPAdderss()
		{
			string strUserHostAddress = string.Empty;
			try
			{
				strUserHostAddress = HttpContext.Current.Request.UserHostAddress;
			}
			catch
			{
			}


			string strHTTP_X_FORWARDED_FOR = string.Empty;
			try
			{
				strHTTP_X_FORWARDED_FOR = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			}
			catch
			{
			}

			string strREMOTE_ADDR = string.Empty;
			try
			{
				strREMOTE_ADDR = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}
			catch
			{
			}

			string strIPAdderss = string.Empty;
            if(!string.IsNullOrEmpty(strUserHostAddress))
			{
				strIPAdderss+= strUserHostAddress+" ";
			}
			if(string.IsNullOrEmpty(strHTTP_X_FORWARDED_FOR))
			{
				strIPAdderss += strHTTP_X_FORWARDED_FOR + " ";
			}
			if(string.IsNullOrEmpty(strREMOTE_ADDR))
			{
				strIPAdderss += strREMOTE_ADDR + " ";
			}
			if(string.IsNullOrEmpty(strIPAdderss))
			{
				strIPAdderss = "0.0.0.0";
			}
			if(strIPAdderss.EndsWith(" "))
			{
				strIPAdderss = strIPAdderss.Substring(0,strIPAdderss.Length - 1);
            }

			return strIPAdderss;
		}

		/// <summary>
		/// 得到当前请求设备的MAC地址
		/// </summary>
		/// <returns></returns>
		public static string GetMac()
		{
			string strMac = string.Empty;
			try
			{
				ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
				ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
				foreach(ManagementObject managementObject in managementObjectCollection)
				{
					if(managementObject["IPEnabled"].ToString() == "True")
					{
						strMac = managementObject["MacAddress"].ToString();
					}
				}
				strMac = strMac.Replace(":","-");
			}
			catch
			{
			}
			return strMac;
		}

		/// <summary>
		/// 得到客户端设备的名称
		/// </summary>
		/// <returns></returns>
		public static string GetHostName()
		{
			string strHostName = string.Empty;
			try
			{
				strHostName = Dns.GetHostName();
			}
			catch
			{
			}
			return strHostName;
		}
	}

	/// <summary>
	/// 功能分组状态
	/// </summary>
	public enum ClientType
	{
		UNKNOWN,
		WINDOWS,
        WINDOWS_WPF,
		WINDOWS_WINFORM,
		WINDOWS_MFC,
		WEB,
        WEB_ASPNET,
		WEB_SERVER,
		WEB_HTML5,
		WEB_ASP,
		WEB_JSP,
		IOS,
		MAC,
		ANDROID,
		LINUX,

		OTHER
	}
}
