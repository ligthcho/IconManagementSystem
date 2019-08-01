using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Net;
using Model;

namespace Logic
{
	public class HttpLogic
	{
		/// <summary>
		/// 标有无用的并影响分析效率的标记，如：
		/// "VIEWSTATE"
		/// </summary>
		public static string[] ContainUselessTag = new string[]
		{
			@"__VIEWSTATE.[^>]*.",
		};

		/// <summary>
		/// 常见PC的User-Agent，用Tillage机器人User-Agent 或者 用伪造其他浏览器数据进行伪装访问
		/// </summary>
		public static string[] HttpWebRequestUserAgent = new string[]
		{
			////zs-hc
			//@"zs-hc/1.0 (CLOUD; http://www.zs-hc.com; )",//zs-hc

			//IE
			@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; MyIE2; SV1;)",//Internet Explorer 6,MYIE2内核
			@"Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.0)",//Internet Explorer 5.5 on Windows 2000
			@"Mozilla/4.0 (compatible; MSIE 6.0; MSN 2.5; Windows 98)",//Internet Explorer 6.0 in MSN on Windows 98
			@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)",//Internet Explorer 6
			@"Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 6.0)",//Internet Explorer 7
			@"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)",//Internet Explorer 8
			@"Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//Internet Explorer 9
			@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322)",//Internet Explorer 6,.net 1.1

			//傲游
			@"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)",//Maxthon 2
			@"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/533.9 (KHTML, like Gecko) Maxthon/3.0 Safari/533.9",//Maxthon 3

			//火狐
			@"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.7.5) Gecko/20041210 Firefox/1.0",//Mozilla Firefox 1.0 on Windows XP
			@"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9b5pre) Gecko/2008030706 Firefox/3.0b5pre",//Firefox 3
			@"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3",//Firefox 3

			//谷歌
			@"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/531.0 (KHTML, like Gecko) Chrome/5.0.195.0 Safari/531.0",//Chrome

			//苹果
			@"Mozilla/5.0 (Macintosh; U; PPC Mac OS X; en) AppleWebKit/124 (KHTML, like Gecko) Safari/125",//Safari
			@"Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/533.17.8 (KHTML, like Gecko) Version/5.0.1 Safari/533.17.8",//Safari

			//Opera
			@"Mozilla/4.0 (compatible; MSIE 5.0; Windows 2000) Opera 6.03 [en]",//Opera
			@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 6.0; en) Opera 9.50",//Opera
			@"Opera/7.23 (Windows 98; U) [en]",//Opera 7.23 on Windows 98
			@"Opera/8.00 (Windows NT 5.1; U; en)",//Opera 8.00 on Windows XP
			@"Opera/9.99 (Windows NT 5.1; U; zh-CN) Presto/9.9.9",//Opera 9.99 on Windows XP

			//其他
			@"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.7.5) Gecko/20070321 Netscape/9.0",//NETSCAPE
			@"Mozilla/5.0 (Windows; U; Windows NT 5.1; es-AR; rv:1.7.5) Gecko/20060912 Netscape/8.1.2",//NETSCAPE
			@"Mozilla/5.0 (compatible; Konqueror/3.1; Linux 2.4.22-10mdk; X11; i686; fr, fr_FR)",//Konqueror 3.1 (French)
			@"Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.6) Gecko/20040113",//Mozilla 1.6 on Linux
			@"Mozilla/4.8 [en] (Windows NT 5.0; U)",//Netscape 4.8 on Windows XP
			@"Mozilla/5.0 (X11; U; SunOS sun4u; en-US; rv:1.0.1) Gecko/20020920 Netscape/7.0"//Netscape 7 on Sun Solaris 8
		};

		private static int HTTP_TIME_OUT=30000;//请求超时时间

		/// <summary>
		/// Cookie容器的实体对象
		/// </summary>
		private CookieContainer m_CookieContainer;

		/// <summary>
		/// Cookie容器的实体对象属性
		/// </summary>
		public CookieContainer CookieContainer
		{
			get
			{
				return m_CookieContainer;
			}
			set
			{
				m_CookieContainer=value;
			}
		}

		#region 网站响应信息和编码

		/// <summary>
		/// 提交Method信息
		/// </summary>
		/// <param name="proxyModel">代理服务器设置</param>
		/// <param name="methodModel">Method验证信息</param>
		/// <returns></returns>
		public bool SetMethod(ProxyModel proxyModel,MethodModel methodModel)
		{
			bool isPostOk=false;
			try
			{
				if(!String.IsNullOrEmpty(methodModel.Post)&&!String.IsNullOrEmpty(methodModel.PostUrl))
				{
					HttpWebRequest httpWebRequest=(HttpWebRequest)HttpWebRequest.Create(methodModel.PostUrl);

					#region 代理服务器
					if(proxyModel.ProxyAddress!=String.Empty)
					{
						WebProxy webProxy=new WebProxy(proxyModel.ProxyAddress+":"+proxyModel.ProxyPort,false);
						webProxy.Credentials=new NetworkCredential(proxyModel.ProxyUserName,proxyModel.ProxyPassword,proxyModel.ProxyDomain);
						httpWebRequest.Proxy=webProxy;
					}
					#endregion 代理服务器

					#region 伪造浏览器数据进行伪装访问，避免被防采集程序过滤，并避开ASP常用的POST检查
					System.Random random=new System.Random();
					int intRandom=random.Next(0,HttpWebRequestUserAgent.Length-1);//随机一个UserAgent
					httpWebRequest.UserAgent=HttpWebRequestUserAgent[intRandom];
					try
					{
						httpWebRequest.Referer=this.ConvertUrlRoot(methodModel.PostUrl);//指明来源网页，可以将这里替换成要采集页面的主页
					}
					catch
					{
					}
					httpWebRequest.ContentType="application/x-www-form-urlencoded";
					httpWebRequest.Accept="text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
					#endregion 伪造浏览器数据进行伪装访问，避免被防采集程序过滤，并避开ASP常用的POST检查

					#region 包装HttpWebRequest
					httpWebRequest.Credentials=CredentialCache.DefaultCredentials;
					httpWebRequest.Timeout=HTTP_TIME_OUT;
					httpWebRequest.AllowAutoRedirect=true;
					#endregion 包装HttpWebRequest

					#region 提交Method.Post信息
					httpWebRequest.CookieContainer=this.m_CookieContainer;
					byte[] byteRequest=Encoding.Default.GetBytes(methodModel.Post);
					httpWebRequest.Method=Method.Post.ToString();
					httpWebRequest.ContentLength=byteRequest.Length;
					Stream stream=httpWebRequest.GetRequestStream();
					stream.Write(byteRequest,0,byteRequest.Length);
					stream.Close();
					#endregion 提交Method.Post信息

					isPostOk=true;

					HttpWebResponse httpWebResponse;
					try
					{
						httpWebResponse=(HttpWebResponse)httpWebRequest.GetResponse();
					}
					catch(WebException ex)
					{
						httpWebResponse=(HttpWebResponse)ex.Response;
					}

					httpWebRequest.Abort();
					httpWebResponse.Close();
				}
				else
				{
					isPostOk=false;
				}
			}
			catch
			{
				isPostOk=false;
			}
			return isPostOk;
		}

		/// <summary>
		/// 取得指定网站网页的响应信息
		/// </summary>
		/// <param name="strUrl">网站网页的URL</param>
		/// <param name="proxyModel">代理服务器设置</param>
		/// <param name="methodModel">Method验证信息</param>
		/// <param name="encoding">返回编码</param>
		/// <param name="pageLastModified">返回网站网页时间</param>
		/// <returns>返回网站网页的响应信息字符串</returns>
		public string GetHttpHtml(string strUrl,ProxyModel proxyModel,MethodModel methodModel,ref Encoding encoding,ref DateTime pageLastModified)
		{
			#region 开始时间
			DateTime dateTimeBegin=DateTime.Now;
			#endregion 开始时间

			encoding=Encoding.Default;
			string strResult=String.Empty;
			try
			{
				HttpWebRequest httpWebRequest=(HttpWebRequest)HttpWebRequest.Create(strUrl);

				#region 代理服务器
				if(proxyModel.ProxyAddress!=String.Empty)
				{
					WebProxy webProxy=new WebProxy(proxyModel.ProxyAddress+":"+proxyModel.ProxyPort,false);
					webProxy.Credentials=new NetworkCredential(proxyModel.ProxyUserName,proxyModel.ProxyPassword,proxyModel.ProxyDomain);
					httpWebRequest.Proxy=webProxy;
				}
				#endregion 代理服务器

				#region 伪造浏览器数据进行伪装访问，避免被防采集程序过滤，并避开ASP常用的POST检查
				System.Random random=new System.Random();
				int intRandom=random.Next(0,HttpWebRequestUserAgent.Length-1);//随机一个UserAgent
				httpWebRequest.UserAgent=HttpWebRequestUserAgent[intRandom];
				try
				{
					httpWebRequest.Referer=strUrl;//指明来源网页，可以将这里替换成要采集页面的主页
				}
				catch
				{
				}
				httpWebRequest.ContentType="application/x-www-form-urlencoded";
				httpWebRequest.Accept="text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
				#endregion 伪造浏览器数据进行伪装访问，避免被防采集程序过滤，并避开ASP常用的POST检查

				#region 包装HttpWebRequest
				httpWebRequest.Credentials=CredentialCache.DefaultCredentials;
				httpWebRequest.Timeout=HTTP_TIME_OUT;
				httpWebRequest.AllowAutoRedirect=true;
				#endregion 包装HttpWebRequest

				#region 调用Cookie
				this.SetMethod(proxyModel,methodModel);//提交Method信息
				httpWebRequest.CookieContainer=this.m_CookieContainer;
				#endregion 调用Cookie

				HttpWebResponse httpWebResponse;
				try
				{
					httpWebResponse=(HttpWebResponse)httpWebRequest.GetResponse();
				}
				catch(WebException ex)
				{
					httpWebResponse=(HttpWebResponse)ex.Response;
				}

				#region 获取最后一次修改响应内容的日期和时间
				try
				{
					pageLastModified=httpWebResponse.LastModified;
				}
				catch(Exception ex)
				{
					string strTemp="StackTrace："+ex.StackTrace+"\r\nMessage："+ex.Message+"\r\nURL："+strUrl;
				}
				#endregion 获取最后一次修改响应内容的日期和时间

				#region 自动识别网页编码，并读取内容

				strResult=this.GetHttpHtmlDecode(httpWebResponse,ref encoding);

				#endregion 自动识别网页编码，并读取内容

				#region 自动识别失败原因
				if(strResult==String.Empty)
				{
					try
					{
						strResult=new StreamReader(httpWebResponse.GetResponseStream(),Encoding.Default).ReadToEnd();
					}
					catch(Exception ex)
					{
						strResult=ex.Message.Replace("\r",String.Empty).Replace("\n",String.Empty);
					}
				}
				#endregion 自动识别失败原因

				httpWebRequest.Abort();
				httpWebResponse.Close();
			}
			catch(Exception ex)
			{
				string strTemp="StackTrace："+ex.StackTrace+"\r\nMessage："+ex.Message+"\r\nURL："+strUrl;
			}

			#region 结束时间
			DateTime dateTimeEnd=DateTime.Now;
			#endregion 结束时间

			#region 输出URLResult

			#endregion  输出URLResult

			#region 滤去无用的并影响分析效率的字符串 如：VIEWSTATE
			if(strResult.Trim()!=String.Empty)
			{
				foreach(string oldValue in ContainUselessTag)
				{
					if(oldValue!=String.Empty)
					{
						Regex r=new Regex(oldValue,RegexOptions.IgnoreCase|RegexOptions.Compiled);
						MatchCollection mc=r.Matches(strResult);
						for(int i=0;i<mc.Count;i++)
						{
							if(mc[i].Value.Length>512)
							{
								strResult=strResult.Replace(mc[i].Value,String.Empty).Trim();
							}
						}
						r=null;
						mc=null;
					}
				}
			}

			#endregion 滤去无用的并影响分析效率的字符串  如：VIEWSTATE

			return strResult;
		}

		/// <summary>
		/// 取得指定网站网页的响应信息
		/// </summary>
		/// <param name="strUrl">网站网页的URL</param>
		/// <param name="proxyModel">代理服务器设置</param>
		/// <param name="methodModel">Method验证信息</param>
		/// <returns>返回网站网页的响应信息字符串</returns>
		public string GetHttpHtml(string strUrl,ProxyModel proxyModel,MethodModel methodModel)
		{
			Encoding encoding=Encoding.Default;
			DateTime pageLastModified=DateTime.Now;
			return this.GetHttpHtml(strUrl,proxyModel,methodModel,ref encoding,ref pageLastModified);
		}

		/// <summary>
		/// 取得指定网站网页的响应信息
		/// </summary>
		/// <param name="strUrl">网站网页的URL</param>
		/// <param name="encoding">返回编码</param>
		/// <param name="pageLastModified">返回网站网页时间</param>
		/// <returns>返回网站网页的响应信息字符串</returns>
		public string GetHttpHtml(string strUrl,ref Encoding encoding,ref DateTime pageLastModified)
		{
			MethodModel methodModel=new MethodModel();
			ProxyModel proxyModel=new ProxyModel();
			return this.GetHttpHtml(strUrl,proxyModel,methodModel,ref encoding,ref pageLastModified);
		}

		/// <summary>
		/// 取得指定网站网页的响应信息
		/// </summary>
		/// <param name="strUrl">网站网页的URL</param>
		/// <returns>返回网站网页的响应信息字符串</returns>
		public string GetHttpHtml(string strUrl)
		{
			Encoding encoding=Encoding.Default;
			DateTime pageLastModified=DateTime.Now;
			return this.GetHttpHtml(strUrl,ref encoding,ref pageLastModified);
		}

		///<summary>
		///Post取得指定网站网页的响应信息
		///</summary>
		///<param name="URL">url地址</param>
		///<param name="strPostdata">发送的数据</param>
		///<param name="encoding">编码</param>
		///<returns></returns>
		public string PostGetHttpHtml(string strUrl,string strPostData,ref Encoding encoding)
		{
			try
			{
				HttpWebRequest httpWebRequest=(HttpWebRequest)WebRequest.Create(strUrl);
				httpWebRequest.Method=Method.Post.ToString();
				//httpWebRequest.Accept="text/html, application/xhtml+xml, */*";//连MuleESB，有此设置后读不到POST
				//httpWebRequest.ContentType="application/x-www-form-urlencoded";//连MuleESB，有此设置后读不到POST
				byte[] buffer=encoding.GetBytes(strPostData);
				httpWebRequest.ContentLength=buffer.Length;
				httpWebRequest.GetRequestStream().Write(buffer,0,buffer.Length);

				HttpWebResponse httpWebResponse=(HttpWebResponse)httpWebRequest.GetResponse();
				using(StreamReader reader=new StreamReader(httpWebResponse.GetResponseStream(),encoding))
				{
					return reader.ReadToEnd();
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		///<summary>
		///Post取得指定网站网页的响应信息
		///</summary>
		///<param name="URL">url地址</param>
		///<param name="strPostdata">发送的数据</param>
		///<param name="encoding">编码</param>
		///<returns></returns>
		public string PostGetHttpHtml(string strUrl,string strPostData)
		{
			Encoding encoding=Encoding.UTF8;
			return this.PostGetHttpHtml(strUrl,strPostData,ref encoding);
		}

		/// <summary>
		/// 自动识别网页编码，并读取内容
		/// </summary>
		/// <param name="httpWebResponse">网站网页相应信息</param>
		/// <param name="encoding">返回编码</param>
		/// <returns>返回网站网页的响应信息字符串</returns>
		private string GetHttpHtmlDecode(HttpWebResponse httpWebResponse,ref Encoding encoding)
		{
			// 读取数据到 MemoryStream 
			MemoryStream inMemoryStream=new MemoryStream();
			byte[] byteBuffer=new byte[2048];
			Stream stream=httpWebResponse.GetResponseStream();

			#region 通过HttpWebResponse Headers的Content-Encoding判断是否是压缩流，并对应解压缩
			string strContentEncoding=httpWebResponse.Headers["Content-Encoding"];
			if(strContentEncoding!=null)
			{
				if(strContentEncoding.ToLower().IndexOf("gzip",StringComparison.CurrentCultureIgnoreCase)>-1)
				{
					stream=new GZipStream(stream,CompressionMode.Decompress);
				}
				if(strContentEncoding.ToLower().IndexOf("deflate",StringComparison.CurrentCultureIgnoreCase)>-1)
				{
					stream=new DeflateStream(stream,CompressionMode.Decompress);
				}
			}
			#endregion 通过HttpWebResponse Headers的Content-Encoding判断是否是压缩流，并对应解压缩

			int intReadNum=stream.Read(byteBuffer,0,byteBuffer.Length);
			while(intReadNum>0)
			{
				inMemoryStream.Write(byteBuffer,0,intReadNum);
				intReadNum=stream.Read(byteBuffer,0,byteBuffer.Length);
			}
			stream.Close();

			// 如果内容开始表头有 charset=calue
			string strCharset=null;

			#region 通过HttpWebResponse Headers提取编码
			string strContentType=httpWebResponse.Headers["content-type"];
			if(strContentType!=null)
			{
				int intStartIndex=strContentType.IndexOf("charset=",StringComparison.CurrentCultureIgnoreCase);
				if(intStartIndex!=-1)
				{
					strCharset=strContentType.Substring(intStartIndex+8);
				}
			}
			#endregion 通过HttpWebResponse Headers提取编码

			// 如果 ContentType 是null, 或没有包含 charset, 在本文中搜寻 charset
			if(strCharset==null)
			{
				MemoryStream outMemoryStream=inMemoryStream;
				outMemoryStream.Seek(0,SeekOrigin.Begin);

				StreamReader outStreamReader=new StreamReader(outMemoryStream,Encoding.Default);
				string strMeta=outStreamReader.ReadToEnd();

				if(strMeta!=null)
				{
					int intStartIndex=strMeta.IndexOf("charset=",StringComparison.CurrentCultureIgnoreCase);
					int intEndIndex=-1;
					if(intStartIndex!=-1)
					{
						intEndIndex=strMeta.IndexOf("\"",intStartIndex);
						if(intEndIndex!=-1)
						{
							int intStart=intStartIndex+8;
							strCharset=strMeta.Substring(intStart,intEndIndex-intStart+1);
							strCharset=strCharset.TrimEnd(new Char[] { '>','"' });
						}
					}
				}
				strMeta=null;
			}

			if(strCharset==null)
			{
				encoding=Encoding.Default; //默认的编码 
			}
			else
			{
				try
				{
					if(strCharset.ToLower()=="gbk")
					{
						encoding=Encoding.GetEncoding("GB18030"); //GBK 
					}
					else
					{
						encoding=Encoding.GetEncoding(strCharset);
					}
				}
				catch
				{
					string strCharsetTemp=String.Empty;
					MemoryStream outMemoryStream=inMemoryStream;
					outMemoryStream.Seek(0,SeekOrigin.Begin);
					StreamReader outStreamReader=new StreamReader(outMemoryStream,Encoding.Default);
					string strMeta=outStreamReader.ReadToEnd();
					if(strMeta!=null)
					{
						int intStartIndex=strMeta.IndexOf("charset=",StringComparison.CurrentCultureIgnoreCase);
						int intEndIndex=-1;
						if(intStartIndex!=-1)
						{
							intEndIndex=strMeta.IndexOf("\"",intStartIndex);
							if(intEndIndex!=-1)
							{
								int intStart=intStartIndex+8;
								strCharsetTemp=strMeta.Substring(intStart,intEndIndex-intStart+1);
								strCharsetTemp=strCharsetTemp.TrimEnd(new Char[] { '>','"' });
							}
						}
					}
					strMeta=null;
					if(strCharsetTemp!=String.Empty)
					{
						try
						{
							encoding=Encoding.GetEncoding(strCharsetTemp);
						}
						catch
						{
							encoding=Encoding.Default;
						}
					}
					else
					{
						encoding=Encoding.Default;
					}
				}
			}
			inMemoryStream.Seek(0,SeekOrigin.Begin);
			StreamReader streamReader=new StreamReader(inMemoryStream,encoding);
			string strHtml=streamReader.ReadToEnd();
			httpWebResponse.Close();
			inMemoryStream.Close();
			byteBuffer=null;

			return strHtml;
		}
		#endregion 网站响应信息和编码

		#region 下载
		/// <summary>
		/// 将二进制流保存到磁盘Save a binary file to disk.
		/// </summary>
		/// <param name="httpWebResponse">The response used to save the file</param>
		/// <param name="strSavePath">保存到本地的路径</param>
		/// <returns></returns>
		private bool SaveBinary(HttpWebResponse httpWebResponse,string strSavePath)
		{
			bool isSave=true;
			try
			{
				byte[] byteBuffer=new byte[2048];
				if(File.Exists(strSavePath))
				{
					File.Delete(strSavePath);
				}
				Stream inStream=httpWebResponse.GetResponseStream();
				Stream outStream=System.IO.File.Create(strSavePath);
				int intInOutNum;
				do
				{
					intInOutNum=inStream.Read(byteBuffer,0,byteBuffer.Length);
					if(intInOutNum>0)
					{
						outStream.Write(byteBuffer,0,intInOutNum);
					}
				}
				while(intInOutNum>0);
				outStream.Close();
				inStream.Close();
			}
			catch(Exception ex)
			{
				string strTemp="StackTrace："+ex.StackTrace+"\r\nMessage："+ex.Message;
				isSave=false;
			}
			return isSave;
		}

		#endregion 下载

		#region URL地址分析
		/// <summary>
		/// 根据当前路径得到上一级地址
		/// 根据传入的URL地址得到当前地址的上一级地址
		/// </summary>
		/// <param name="strUrl">URL地址</param>
		/// <returns>返回上一级地址</returns>
		public string ConvertUrlHigher(string strUrl)
		{
			strUrl=strUrl.Replace("://","：／／");
			strUrl=strUrl.Replace(@"\","/");
			int intLastSolidus=strUrl.LastIndexOf('/');
			if(intLastSolidus>1)
			{
				strUrl=strUrl.Substring(0,intLastSolidus);
			}
			strUrl=strUrl.Replace("：／／","://")+"/";
			return strUrl;
		}

		/// <summary>
		/// 根据当前路径得到根级地址
		/// 根据传入的URL地址得到当前地址的根级地址
		/// </summary>
		/// <param name="strUrl">URL地址</param>
		/// <returns>返回根级地址</returns>
		public string ConvertUrlRoot(string strUrl)
		{
			strUrl=strUrl.Replace("://","：／／");
			strUrl=strUrl.Replace(@"\","/");
			int intSolidus=strUrl.IndexOf('/');
			if(intSolidus>1)
			{
				strUrl=strUrl.Substring(0,intSolidus);
			}
			strUrl=strUrl.Replace("：／／","://");
			return strUrl;
		}

		/// <summary>
		/// 根据当前路径得到标准地址（去参数）
		/// </summary>
		/// <param name="strUrl">URL地址</param>
		/// <returns>返回标准地址（去参数）</returns>
		public string ConvertUrlNonce(string strUrl)
		{
			string strReturnUrl=strUrl;
			try
			{
				strReturnUrl=this.GetUri(strUrl).ToString();
			}
			catch
			{
				strReturnUrl=strUrl;
			}
			return strReturnUrl;
		}

		/// <summary>
		/// 根据URL地址得到Uri
		/// </summary>
		/// <param name="path">URL地址</param>
		/// <returns>Uri</returns>
		public Uri GetUri(string path)
		{
			Uri uriPath;
			try
			{
				uriPath=new Uri(path);
				StringBuilder builder1=new StringBuilder();
				UriBuilder builder2=new UriBuilder(uriPath);
				builder2.Query=builder1.ToString();
				uriPath=builder2.Uri;
			}
			catch(UriFormatException)
			{
				uriPath=new Uri(Path.GetFullPath(path));
			}
			return uriPath;
		}
		#endregion URL地址分析
	}

}
