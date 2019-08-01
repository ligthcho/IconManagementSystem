using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models
{
	public class LogModel
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="intEventType"></param>
		/// <param name="strEventCategory"></param>
		/// <param name="intEventID"></param>
		/// <param name="strIP"></param>
		/// <param name="strMacAddress"></param>
		/// <param name="strUserName">登录ID</param>
		/// <param name="strSourceType"></param>
		/// <param name="strSource"></param>
		/// <param name="strDescription">描述</param>
		public LogModel(string strRecordingTime,string strEventCategory,int intEventID,string strMessage,string strIP,string strMacAddress,string strUserName,string strSourceType,string strSource,string strDescription)
		{
			m_Message = strMessage;
			m_RecordingTime = strRecordingTime;
			m_EventCategory = strEventCategory;
			m_EventID = EventID;
			m_IP = strIP;
			m_MacAddress = strMacAddress;
			m_UserName = strUserName;
			m_SourceType = strSourceType;
			m_Source = strSource;
			m_Description = strDescription;
		}

		string m_Message;
		/// <summary>
		/// 日志消息
		/// </summary>
		public string Message
		{
			get
			{
				return m_Message;
			}
			set
			{
				m_Message = value;
			}
		}
		string m_RecordingTime;
		/// <summary>
		/// 时间
		/// </summary>
		public string RecordingTime
		{
			get
			{
				return m_RecordingTime;
			}
			set
			{
				m_RecordingTime = value;
			}
		}
		string m_EventCategory;
		/// <summary>
		/// 日志分类描述，自定义
		/// </summary>
		public string EventCategory
		{
			get
			{
				return m_EventCategory;
			}
			set
			{
				m_EventCategory = value;
			}
		}
		int m_EventID;
		/// <summary>
		/// 日志分类号
		/// </summary>
		public int EventID
		{
			get
			{
				return m_EventID;
			}
			set
			{
				m_EventID = value;
			}
		}
		string m_IP;
		/// <summary>
		/// 计算机IP
		/// </summary>
		public string IP
		{
			get
			{
				return m_IP;
			}
			set
			{
				m_IP = value;
			}
		}
		string m_MacAddress;
		/// <summary>
		/// 计算机Mac地址
		/// </summary>
		public string MacAddress
		{
			get
			{
				return m_MacAddress;
			}
			set
			{
				m_MacAddress = value;
			}
		}
		string m_UserName;
		/// <summary>
		/// 系统登陆用户
		/// </summary>
		public string UserName
		{
			get
			{
				return m_UserName;
			}
			set
			{
				m_UserName = value;
			}
		}
		string m_SourceType;
		/// <summary>
		/// Rier
		/// </summary>
		public string SourceType
		{
			get
			{
				return m_SourceType;
			}
			set
			{
				m_SourceType = value;
			}
		}
		string m_Source;
		/// <summary>
		/// Rier Recorder audit
		/// </summary>
		public string Source
		{
			get
			{
				return m_Source;
			}
			set
			{
				m_Source = value;
			}
		}
		string m_Description;
		/// <summary>
		/// 日志描述信息
		/// </summary>
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
			}
		}
	}
}
