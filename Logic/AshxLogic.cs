using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Collections.Specialized;
using System.Data;
using Model;

using System.Collections;

namespace Logic
{
	/// <summary>
	/// asp.net 页面输入输出
	/// </summary>
	public class AshxLogic
	{
		#region 读取配置
		/*
		<!--┏页面配置┓-->
		<!--页面输出的数据是否回车换行-->
		<add key="PageDataFormat" value="json"/>
		<!--页面输出的数据是否回车换行-->
		<add key="PageDataIsEnter" value="true"/>
		<!--页面输出的数据类型-->
		<add key="PageDataType" value="text/html"/>
		<!--页面输出的数据编码-->
		<add key="PageDataCharset" value="UTF-8"/>
		<!--页面的Post从Get获取数据-->
		<add key="PagePostByGet" value="true"/>
		<!--是否输出PageLog到Log-->
		<add key="PageLogWriteLog" value="true"/>
		<!--输出PageLog到Log的目录-->
		<add key="PageLogWriteLogPath" value="/NCD/Log/"/>
		<!--输出PageLog到Log的文件扩展名-->
		<add key="PageLogWriteLogExtension" value=".URL.LOG"/>
		<!--输出PageLog到的Log中不输出的RequestForm参数-->
		<add key="PageLogWriteLogRequestFormClear" value="*.LogInfo,*.Content"/>
		<!--是否启用Post/Get的Data数据加解密-->
		<add key="PageDataEncryptDecryptEnabled" value="false" />
		<!--是否启用Post/Get的Data数据校验-->
		<add key="PageDataMD5LicenseEnabled" value="false" />
		<!--允许Post/Get的Data数据校验 Licenses，MD5(DATA+License)校验比对，不多于5个-->
		<add key="PageDataMD5Licenses" value="00000000,11111111,22222222,44444444" />
		<!--┗页面配置┛-->
		*/
		public static string PAGE_DATA_FORMAT = ConfigLogic.GetString("PageDataFormat").ToLower();
		public static bool PAGE_DATA_IS_ENTER = ConfigLogic.GetBool("PageDataIsEnter");
		public static string PAGE_DATA_TYPE = ConfigLogic.GetString("PageDataType");
		public static string PAGE_DATA_CHARSET = ConfigLogic.GetString("PageDataCharset");
		public static bool PAGE_POST_BY_GET = ConfigLogic.GetBool("PagePostByGet");
		public static bool PAGE_LOG_WRITE_LOG = ConfigLogic.GetBool("PageLogWriteLog");
		public static string PAGE_LOG_WRITE_LOG_PATH = ConfigLogic.GetString("PageLogWriteLogPath");
		public static string PAGE_LOG_WRITE_LOG_EXTENSION = ConfigLogic.GetString("PageLogWriteLogExtension");
		public static string PAGE_LOG_WRITE_LOG_REQUEST_FORM_CLEAR = "," + ConfigLogic.GetString("PageLogWriteLogRequestFormClear") + ",";
		public static bool PAGE_DATA_ENCRYPT_DECRYPT_ENABLED = ConfigLogic.GetBool("PageDataEncryptDecryptEnabled");
		public static bool PAGE_DATA_MD5_LICENSE_ENABLED = ConfigLogic.GetBool("PageDataMD5LicenseEnabled");
		public static string[] PAGE_DATA_MD5_LICENSES = ConfigLogic.GetStrings("PageDataMD5Licenses");
		#endregion 读取配置

		/// <summary>
		/// 页面输出
		/// </summary>
		/// <param name="page"></param>
		/// <param name="strOutput"></param>
		public static void PageOut(HttpContext page,string strOutput)
		{
			try
			{
				#region 记录相应日志
				if(PAGE_LOG_WRITE_LOG)
				{
					//记录相应日志到HTTPLog （ip，hostName，页面地址，参数）
					string strForm = String.Empty;
					for(int i = 0;i < HttpContext.Current.Request.Form.Count;i++)
					{
						try
						{
							string strName = HttpContext.Current.Request.Form.Keys[i].ToString().ToLower();
							string strNames = strName;
							string strValue = HttpContext.Current.Request.Form[i].ToString();

							if(PAGE_LOG_WRITE_LOG_REQUEST_FORM_CLEAR.ToLower().IndexOf("," + strName + ",") > -1 || PAGE_LOG_WRITE_LOG_REQUEST_FORM_CLEAR.ToLower().IndexOf("," + strNames + ",") > -1)//清空数据
							{
								strValue = "*";
							}
							strForm += "&" + HttpContext.Current.Request.Form.Keys[i].ToString() + "=" + strValue;
						}
						catch
						{
						}
					}
					if(strForm != String.Empty)
					{
						strForm += "?" + strForm;
					}
					string strLog = DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ClientModel.GetIPAdderss().PadLeft(15,' ') + "\t" + page.Request.RawUrl + strForm;//+"\t"+strOutput.Replace("\r",string.Empty).Replace("\n",string.Empty).Replace("\t",string.Empty);
					LogLogic.Write(strLog,PAGE_LOG_WRITE_LOG_PATH,PAGE_LOG_WRITE_LOG_EXTENSION);
				}
				#endregion 记录相应日志

				page.Response.Clear();
				page.Response.ContentType = PAGE_DATA_TYPE;
				page.Response.Charset = PAGE_DATA_CHARSET;//设置输出流的字符集-中文
				page.Response.ContentEncoding = System.Text.Encoding.GetEncoding(PAGE_DATA_CHARSET);//设置输出流的字符集
				page.Response.Write(strOutput);
				HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		#region 公用属性
		public static DataModel OutData
		{
			get
			{
				DataModel dataModel = new DataModel();
				dataModel.Add("ResultID",0);
				dataModel.Add("ResultMessage",string.Empty);
				dataModel.Add("ResultData",null);
				return dataModel;
			}
		}

		public static DataModel PostData
		{
			get
			{
				string strDATA = DATA;//Post上来的DATA
				bool isValidMD5 = false;//是否通过MD5校验
				string strMD5 = MD5;//Post上来的MD5
				string strPostLicense = string.Empty;//Post上来的MD5使用的License
				if(strMD5 != string.Empty)//Post上来的MD5校验不能为空
				{
					if(PAGE_DATA_MD5_LICENSE_ENABLED)//判断是否启用校验
					{
						foreach(string strLicense in PAGE_DATA_MD5_LICENSES)//循环多个License
						{
							string strServerMD5 = CodeLogic.EncryptMD5(strDATA + strLicense);
							if(strServerMD5.Equals(strMD5))//判断Post的MD5和后台是否一致
							{
								isValidMD5 = true;//通过MD5校验
								strPostLicense = strLicense;
								break;
							}
							else
							{
								continue;//下一个License
							}
						}
					}
					else
					{
						isValidMD5 = true;//如果校验不启用，通过MD5校验
					}
				}

				DataModel postData = null;
				if(isValidMD5)//如果通过了MD5校验，即MD5是有效的
				{
					string strJson = string.Empty;
					if(PAGE_DATA_ENCRYPT_DECRYPT_ENABLED)//判断是否启用加解密
					{
						try
						{
							strJson = CodeLogic.DecryptAES(strDATA);//用AES解密Post上来的DATA得到Json字符串
						}
						catch
						{
						}
					}
					else
					{
						strJson = strDATA;//如果不需要解密，直接将Post上来的DATA等于Json字符串
					}
					postData = JsonLogic.ToDataModel(strJson);//将Json字符串拆为DataModel（Hashtable）
					if(postData == null)//当Post上来的参数转DataModel后为null，应new
					{
						postData = new DataModel();
					}
					#region 补充一些参数到Post的DataModel中，如客户端或两端加解密使用的License、MD5、Key、IV等
					if(!postData.ContainsKey("_License"))
					{
						postData.Add("_License",strPostLicense);
					}
					if(!postData.ContainsKey("_MD5"))
					{
						postData.Add("_MD5",strMD5);
					}
					if(!postData.ContainsKey("_Key"))
					{
						postData.Add("_AESKey",CodeLogic.CODE_AES_KEY);
					}
					if(!postData.ContainsKey("_IV"))
					{
						postData.Add("_AESIV",CodeLogic.CODE_AES_IV);
					}
					#endregion 补充一些参数到Post的DataModel中，如客户端或两端加解密使用的License、MD5、Key、IV等
				}
				else
				{
					postData = null;
				}

				return postData;
			}
		}

		/// <summary>
		/// 数据包
		/// </summary>
		public static string DATA
		{
			get
			{
				return PageLogic.RequestFormString("DATA");
			}
		}

		/// <summary>
		/// 校验
		/// </summary>
		public static string MD5
		{
			get
			{
				return PageLogic.RequestFormString("MD5");
			}
		}
		#endregion 公用属性

		#region Post/Get
		/// <summary>
		/// 获取页面Post上来的参数Int值
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static int RequestFormInt(string strName)
		{
			if(HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName] == String.Empty)
			{
				if(PAGE_POST_BY_GET)
				{
					return RequestQueryInt(strName);
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return Convert.ToInt32(HttpContext.Current.Request.Form[strName].Trim());
			}
		}

		/// <summary>
		/// 获取页面Post上来的参数Decimal值
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static decimal RequestFormDecimal(string strName)
		{
			if(HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName] == String.Empty)
			{
				if(PAGE_POST_BY_GET)
				{
					return RequestQueryDecimal(strName);
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return Convert.ToDecimal(HttpContext.Current.Request.Form[strName].Trim());
			}
		}

		/// <summary>
		/// 获取页面Post上来的参数String值
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static string RequestFormString(string strName)
		{
			if(HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName] == String.Empty)
			{
				if(PAGE_POST_BY_GET)
				{
					return RequestQueryString(strName);
				}
				else
				{
					return string.Empty;
				}
			}
			else
			{
				return Convert.ToString(HttpContext.Current.Request.Form[strName].Trim());
			}
		}

		/// <summary>
		/// 获取页面Post上来的参数Object值
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static object RequestFormObject(string strName)
		{
			if(HttpContext.Current.Request.Form[strName] == null)
			{
				return null;
			}
			else
			{
				return HttpContext.Current.Request.Form[strName];
			}
		}

		/// <summary>
		/// 获取页面Get上来的参数Int值
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static int RequestQueryInt(string strName)
		{
			if(HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName] == String.Empty)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(HttpContext.Current.Request.QueryString[strName].Trim());
			}
		}

		/// <summary>
		/// 获取页面Get上来的参数Decimal值
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static decimal RequestQueryDecimal(string strName)
		{
			if(HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName] == String.Empty)
			{
				return 0;
			}
			else
			{
				return Convert.ToDecimal(HttpContext.Current.Request.QueryString[strName].Trim());
			}
		}

		/// <summary>
		/// 获取页面Get上来的参数String值
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static string RequestQueryString(string strName)
		{
			if(HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName] == String.Empty)
			{
				return String.Empty;
			}
			else
			{
				string strString = Convert.ToString(HttpContext.Current.Request.QueryString[strName].Trim());
				strString = strString.Replace("%22","\"").Replace("%22","\"");
				strString = strString.Replace("%2F","/").Replace("%2f","/");
				strString = strString.Replace("%5C",@"\").Replace("%5c",@"\");
				strString = strString.Replace("%3F","?").Replace("%3f","?");
				strString = strString.Replace("%26","&").Replace("%26","&");
				strString = strString.Replace("%20"," ").Replace("%20"," ");
				strString = strString.Replace("%23","#").Replace("%23","#");
				strString = strString.Replace("%2B","+").Replace("%2b","+");
				strString = strString.Replace("%3D","=").Replace("%3d","=");
				strString = strString.Replace("%2E",".").Replace("%2e",".");
				strString = strString.Replace("%3A",":").Replace("%3a",":");
				strString = strString.Replace("%25","%").Replace("%25","%");
				return strString;
			}
		}
		#endregion Post/Get
	}
}