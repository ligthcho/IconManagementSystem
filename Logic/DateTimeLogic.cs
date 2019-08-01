using System;
using System.Globalization;

namespace Logic
{
	public class DateTimeLogic
	{
		#region 读取配置
		/*
		<!--┏其他┓-->
		<!--日期控件手动输入的格式分割符-->
		<add key="DateFormats" value="yyyy-MM-dd,yyyy-M-d,yyyyMMdd,yyyy/MM/dd,yyyy/M/d,yyyy.MM.dd,yyyy.M.d,yyyy MM dd,yyyy M d" />
		<!--时间控件手动输入的格式分割符-->
		<add key="TimeFormats" value="HH:mm:ss,H:mm:ss,HH:mm,H:mm" />
		<!--┗其他┛-->
		<!--┗错误日志配置┛-->
		*/

		public static string[] DATE_FORMATS = ConfigLogic.GetStrings("DateFormats");
		public static string[] TIME_FORMATS = ConfigLogic.GetStrings("TimeFormats");
		public static string[] DATETIME_FORMATS
		{
			get
			{
				string strDateTimeFormats = string.Empty;
				foreach(string strDateFormat in DATE_FORMATS)
				{
					foreach(string strTimeFormat in TIME_FORMATS)
					{
						strDateTimeFormats += strDateFormat + " " + strTimeFormat + ",";
					}
				}
				if(strDateTimeFormats.EndsWith(","))
				{
					strDateTimeFormats = strDateTimeFormats.Substring(0,strDateTimeFormats.Length - 1);
				}
				return strDateTimeFormats.Split(',');
			}
		}
		#endregion 读取配置

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static DateTime DiscardDayTime(DateTime d)
		{
			return new DateTime(d.Year,d.Month,1,0,0,0);
		}

		/// <summary>
		/// 月份与月份的差
		/// </summary>
		/// <param name="dt1"></param>
		/// <param name="dt2"></param>
		/// <returns></returns>
		public static int CompareYearMonth(DateTime dt1,DateTime dt2)
		{
			return (dt1.Year-dt2.Year)*12+(dt1.Month-dt2.Month);
		}

		/// <summary>
		/// 将字符串按照日期时间格式化配置，格式化为包含年月日时分秒DateTime类型
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static DateTime ParseExactDateTime(string strValue)
		{
			System.DateTime dateTime=new System.DateTime(0);
			if(strValue==null)
			{
				return dateTime;
			}
			if(strValue.Trim()==string.Empty)
			{
				return dateTime;
			}
			bool isFormatted=false;
			foreach(string strDateTimeFormats in DATETIME_FORMATS)
			{
				if(isFormatted)
				{
					break;
				}
				if(strDateTimeFormats.Trim()==string.Empty)
				{
					continue;
				}
				try
				{
					dateTime=System.DateTime.ParseExact(strValue.Trim(),strDateTimeFormats.Trim(),CultureInfo.CurrentCulture);

					isFormatted=true;
				}
				catch
				{
				}
			}
			return dateTime;
		}

		/// <summary>
		/// 将字符串按照日期格式化配置，格式化为只有年月日的DateTime类型（00时:00分:00秒）
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static DateTime ParseExactDate(string strValue)
		{
			System.DateTime dateTime=new System.DateTime(0);
			if(strValue==null)
			{
				return dateTime;
			}
			if(strValue.Trim()==string.Empty)
			{
				return dateTime;
			}
			bool isFormatted=false;
			foreach(string strDateFormats in DATE_FORMATS)
			{
				if(isFormatted)
				{
					break;
				}
				if(strDateFormats.Trim()==string.Empty)
				{
					continue;
				}
				try
				{
					dateTime = System.DateTime.ParseExact(Convert.ToDateTime(strValue).ToString(strDateFormats.Trim()).Trim(),strDateFormats.Trim(),CultureInfo.CurrentCulture);
					isFormatted=true;
				}
				catch
				{
				}
			}
			return dateTime;
		}

		/// <summary>
		/// 将字符串按照时间格式化配置，格式化为只有时分秒的DateTime类型（0001年-01月-01日）
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static DateTime ParseExactTime(string strValue)
		{
			System.DateTime dateTime=new System.DateTime(0);
			if(strValue==null)
			{
				return dateTime;
			}
			if(strValue.Trim()==string.Empty)
			{
				return dateTime;
			}
			bool isFormatted=false;
			foreach(string strTimeFormats in TIME_FORMATS)
			{
				if(isFormatted)
				{
					break;
				}
				if(strTimeFormats.Trim()==string.Empty)
				{
					continue;
				}
				try
				{
					dateTime = System.DateTime.ParseExact(Convert.ToDateTime(strValue).ToString(strTimeFormats.Trim()).Trim(),strTimeFormats.Trim(),CultureInfo.CurrentCulture);
					isFormatted=true;
				}
				catch
				{
				}
			}

			return System.DateTime.ParseExact("0001-01-01 "+dateTime.ToString(TIME_FORMATS[0]),"yyyy-MM-dd "+TIME_FORMATS[0],CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// 当前时间的JavaScript时间戳
		/// </summary>
		/// <returns></returns>
		public static long DateTimeNowJSTimeStamp()
		{
			return DateTimeToJSTimeStamp(DateTime.Now);
        }

		/// <summary>
		/// DateTime转换为JavaScript时间戳
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static long DateTimeToJSTimeStamp(DateTime dateTime)
		{
			System.DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1)); // 当地时区
			return (long)(dateTime - dateTimeStart).TotalMilliseconds; // 相差毫秒数
		}

		/// <summary>
		/// JavaScript时间戳转换为DateTime
		/// </summary>
		/// <param name="longJSTimeStamp"></param>
		/// <returns></returns>
		public static DateTime JSTimeStampToDateTime(long longJSTimeStamp)
		{
			System.DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1)); // 当地时区
			return dateTimeStart.AddMilliseconds(longJSTimeStamp);
		}

		/// <summary>
		/// 当前时间的Unix时间戳
		/// </summary>
		/// <returns></returns>
		public static long DateTimeNowUnixTimeStamp()
		{
			return DateTimeToUnixTimeStamp(DateTime.Now);
		}

		/// <summary>
		/// DateTime转换为Unix时间戳
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static long DateTimeToUnixTimeStamp(DateTime dateTime)
		{
			System.DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1)); // 当地时区
			return (long)(dateTime - dateTimeStart).TotalSeconds; // 相差秒数
		}

		/// <summary>
		/// Unix时间戳转换为DateTime
		/// </summary>
		/// <param name="longUnixTimeStamp"></param>
		/// <returns></returns>
		public static DateTime UnixTimeStampToDateTime(long longUnixTimeStamp)
		{
			System.DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970,1,1)); // 当地时区
			return dateTimeStart.AddSeconds(longUnixTimeStamp);
		}
	}
}
