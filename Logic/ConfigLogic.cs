using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Logic
{
	/// <summary>
	/// 从web.config、app.config配置文件读取<appSettings>...</appSettings>配置信息
	/// </summary>
	public class ConfigLogic
	{
		#region 读取配置
		/*
		<!--┏错误日志配置┓-->
		<!--是否输出Error到Log-->
		<add key="ErrorWriteLog" value="true" />
		<!--输出Error到Log的目录-->
		<add key="ErrorWriteLogPath" value="/Log/" />
		<!--输出Error到Log的文件扩展名-->
		<add key="ErrorWriteLogExtension" value=".ERR.LOG" />
		<!--输出Error到的Log中不输出的RequestForm参数-->
		<add key="ErrorWriteLogRequestFormClear" value="*.LogInfo,*.Content"/>
		<!--┗错误日志配置┛-->
		*/
		public static bool ERROR_WRITE_LOG = ConfigLogic.GetBool("ErrorWriteLog");
		public static string ERROR_WRITE_LOG_PATH = ConfigLogic.GetString("ErrorWriteLogPath");
		public static string ERROR_WRITE_LOG_EXTENSION = ConfigLogic.GetString("ErrorWriteLogExtension");
		public static string ERROR_WRITE_LOG_REQUEST_FORM_CLEAR = "," + ConfigLogic.GetString("ErrorWriteLogRequestFormClear") + ",";

		#endregion 读取配置

		/// <summary>
		/// 通过Key读取配置信息，返回字符串
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static string GetString(string strKey)
		{
			string strValue=string.Empty;
			try
			{
				strValue = System.Configuration.ConfigurationManager.AppSettings[strKey].Replace("%26","&");
			}
			catch
			{
			}
			return strValue;
		}

		/// <summary>
		/// 通过Key读取配置信息，经过“,”分割，返回字符串数组
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static string[] GetStrings(string strKey)
		{
			List<string> listString = new List<string>();
            try
			{
				foreach(string strValue in GetString(strKey).Split(','))
				{
					if(!string.IsNullOrEmpty(strValue.Trim()))
					{
						listString.Add(strValue.Trim());
                    }
                }
			}
			catch
			{
			}
			return listString.ToArray();
		}

		/// <summary>
		/// 通过Key读取配置信息，经过与“true”字符串比较，返回布尔值
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static bool GetBool(string strKey)
		{
			bool boolValue = false;
			try
			{
				boolValue = GetString(strKey).ToLower() == "true" ? true : false;
			}
			catch
			{
			}
			return boolValue;
		}

		/// <summary>
		/// 通过Key读取配置信息，返回整型
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static int GetInt(string strKey)
		{
			int intValue = 0;
			try
			{
				intValue = Convert.ToInt32(GetString(strKey));
			}
			catch
			{
			}
			return intValue;
		}

		/// <summary>
		/// 通过Key读取配置信息，返回Decimal
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static decimal GetDecimal(string strKey)
		{
			decimal decValue = 0;
			try
			{
				decValue = Convert.ToDecimal(GetString(strKey));
			}
			catch
			{
			}
			return decValue;
		}

		/// <summary>
		/// 通过Key读取配置信息，返回Double
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static double GetDouble(string strKey)
		{
			double doubleValue = 0;
			try
			{
				doubleValue = Convert.ToDouble(GetString(strKey));
			}
			catch
			{
			}
			return doubleValue;
		}

		/// <summary>
		/// 通过Key读取配置信息，返回日期时间
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static DateTime GetDateTime(string strKey)
		{
			DateTime dataTimeValue = DateTime.MinValue;
			try
			{
				dataTimeValue = Convert.ToDateTime(GetString(strKey));
			}
			catch
			{
			}
			return dataTimeValue;
		}

		/// <summary>
		/// 通过Key读取配置信息，返回时间间隔
		/// </summary>
		/// <param name="strKey"></param>
		/// <returns></returns>
		public static TimeSpan GetTimeSpan(string strKey)
		{
			TimeSpan timeSpanValue =new TimeSpan(0);
			try
			{
				timeSpanValue = new TimeSpan(long.Parse(GetString(strKey)));
			}
			catch
			{
			}
			return timeSpanValue;
		}
	}
}
