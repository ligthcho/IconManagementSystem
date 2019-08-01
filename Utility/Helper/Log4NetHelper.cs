using log4net;
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility.Models;

namespace Utility
{
	public class Log4NetHelper
	{
		public static void Error(string strMsg)
		{
			ILog log = log4net.LogManager.GetLogger(strMsg);
			log.Error(strMsg);
		}
		/// <summary>
		/// 记录信息类日志
		/// </summary>
		/// <param name="strMsgTitle"></param>
		/// <param name="strMsg"></param>
		public static void Info(string strMsgTitle,string strMsg)
		{
			ILog log = log4net.LogManager.GetLogger(strMsgTitle);	
			log.Info(new LogModel(DateTime.Now.ToString("HH:mm:ss.fff"),string.Empty,0,strMsg,ClientModel.GetIPAdderss(),ClientModel.GetMac(),string.Empty,strMsgTitle,"",""));
		}

		public static void Warning(string strMsg)
		{
			ILog log = log4net.LogManager.GetLogger(strMsg);
			log.Warn(strMsg);
		}

		public static void Error(string strMsg,Exception ex)
		{
			ILog log = log4net.LogManager.GetLogger(strMsg);
			log.Error("Error",ex);
		}

		public static void Info(string strMsg,Exception ex)
		{
			ILog log = log4net.LogManager.GetLogger(strMsg);
			log.Info("Info",ex);
		}

		public static void Warning(string strMsg,Exception ex)
		{
			ILog log = log4net.LogManager.GetLogger(strMsg);
			log.Warn("Warning",ex);
		}
	}


	public class LogMessagePatternConverter:PatternLayoutConverter
	{
		protected override void Convert(System.IO.TextWriter writer,log4net.Core.LoggingEvent loggingEvent)
		{
			if(Option != null)
			{
				// Write the value for the specified key
				WriteObject(writer,loggingEvent.Repository,LookupProperty(Option,loggingEvent));
			}
			else
			{
				// Write all the key value pairs
				WriteDictionary(writer,loggingEvent.Repository,loggingEvent.GetProperties());
			}
			//if (Option != null)
			//{
			//    // Write the value for the specified key
			//    WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty(Option));
			//}
			//else
			//{
			//    // Write all the key value pairs
			//    WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
			//}
		}

		/// <summary>
		/// 通过反射获取传入的日志对象的某个属性的值
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		private object LookupProperty(string property,log4net.Core.LoggingEvent loggingEvent)
		{
			object propertyValue = string.Empty;

			PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
			if(propertyInfo != null)
				propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject,null);

			return propertyValue;
		}

	}


	public class LogLayout:PatternLayout
	{
		public LogLayout()
		{
			this.AddConverter("property",typeof(LogMessagePatternConverter));
		}
	}
}
