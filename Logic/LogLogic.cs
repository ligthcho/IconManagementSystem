using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Logic
{
	/// <summary>
	/// 日志操作类
	/// </summary>
	public class LogLogic
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="strValue"></param>
		/// <param name="strPath"></param>
		/// <param name="strExtension"></param>
		/// <returns></returns>
		public static string Write(string strValue,string strPath,string strExtension)
		{
			return Write(strValue,strPath,strExtension,"yyyyMMdd");
        }

		/// <summary>
		/// 写日志
		/// </summary>
		/// <param name="strValue"></param>
		/// <param name="strPath"></param>
		/// <param name="strExtension"></param>
		/// <param name="strFileFormat"></param>
		/// <returns></returns>
		public static string Write(string strValue,string strPath,string strExtension,string strFileFormat)
		{
			string strFilePath=string.Empty;
			try
			{
				DirectoryInfo directoryInfo=null;
                if(HttpContext.Current != null)
				{
					directoryInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(strPath));//虚拟目录
				}
				else
				{
					directoryInfo=new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory+strPath);//应用程序
				}

				if(directoryInfo != null)
				{
					if(!directoryInfo.Exists)
					{
						directoryInfo.Create();
					}
					strFilePath = directoryInfo.FullName + "\\" + System.DateTime.Now.ToString(strFileFormat) + strExtension;
				}
				else
				{
					return String.Empty;
				}
			}
			catch
			{
				return String.Empty;
			}

			if(strValue.Equals(string.Empty))
			{
				return String.Empty;
			}
			try
			{
				if(File.Exists(strFilePath))
				{
					StreamWriter streamWriter = File.AppendText(strFilePath);
					streamWriter.WriteLine(strValue);
					streamWriter.Close();
				}
				else
				{
					StreamWriter streamWriter = File.CreateText(strFilePath);
					streamWriter.WriteLine(strValue);
					streamWriter.Close();
				}
			}
			catch
			{
				return String.Empty;
			}

			return strFilePath;
		}
	}
}
