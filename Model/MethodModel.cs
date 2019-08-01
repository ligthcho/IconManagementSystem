using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Model
{
	public class MethodModel
	{
		Method method=Method.Get;

		public Method Method
		{
			get
			{
				string strPostUserName=String.Empty;
				string strPostPassword=String.Empty;
				string strPostAction=String.Empty;

				if(this.PostUserNameParameter.Trim()!=String.Empty)
				{
					strPostUserName="&"+this.PostUserNameParameter.Trim()+"="+this.PostUserNameValue.Trim();
				}
				if(this.PostPasswordParameter.Trim()!=String.Empty)
				{
					strPostPassword="&"+this.PostPasswordParameter.Trim()+"="+this.PostPasswordValue.Trim();
				}
				if(this.PostActionParameter.Trim()!=String.Empty)
				{
					strPostAction="&"+this.PostActionParameter.Trim()+"="+this.PostActionValue.Trim();
				}

				string strPost=strPostUserName+strPostPassword+strPostAction;

				if(this.PostUrl!=String.Empty&&strPost!=String.Empty)
				{
					return Method.Post;
				}
				else
				{
					return Method.Get;
				}
			}
			set
			{
				method=value;
			}
		}

		#region Post
		/// <summary>
		/// 标准的POST格式
		/// PostUserNameParameter=PostUserNameValue＆PostPasswordParameter=PostPasswordValue＆PostActionParameter=PostActionValue
		/// </summary>
		public string Post
		{
			get
			{
				if(this.PostUrl!=String.Empty&&this.Method==Method.Post)
				{
					string strPostUserName=String.Empty;
					string strPostPassword=String.Empty;
					string strPostAction=String.Empty;

					if(this.PostUserNameParameter.Trim()!=String.Empty)
					{
						strPostUserName="&"+System.Web.HttpUtility.UrlEncode(this.PostUserNameParameter.Trim(),Encoding.Default)+"="+System.Web.HttpUtility.UrlEncode(this.PostUserNameValue.Trim(),Encoding.Default);
					}
					if(this.PostPasswordParameter.Trim()!=String.Empty)
					{
						strPostPassword="&"+System.Web.HttpUtility.UrlEncode(this.PostPasswordParameter.Trim(),Encoding.Default)+"="+System.Web.HttpUtility.UrlEncode(this.PostPasswordValue.Trim(),Encoding.Default);
					}
					if(this.PostActionParameter.Trim()!=String.Empty)
					{
						strPostAction="&"+System.Web.HttpUtility.UrlEncode(this.PostActionParameter.Trim(),Encoding.Default)+"="+System.Web.HttpUtility.UrlEncode(this.PostActionValue.Trim(),Encoding.Default);
					}

					string strPost=strPostUserName+strPostPassword+strPostAction;

					//strPost = "password=3nssss&CookieDate=2&userhidden=2&comeurl=index.asp&submit=%u7ACB%u5373%u767B%u5F55&ajaxPost=1&username=3nss";
					//strPost += String.Empty;
					//strPost += String.Empty;

					return strPost;
				}
				else
				{
					return String.Empty;
				}
			}
		}

		/// <summary>
		/// 提交验证信息的URL地址
		/// </summary>
		private string postUrl=String.Empty;

		/// <summary>
		/// 提交验证信息的URL地址属性
		/// </summary>
		public string PostUrl
		{
			get
			{
				return postUrl.Trim();
			}
			set
			{
				postUrl=value.Trim();
			}
		}

		/// <summary>
		/// 需要Post的帐号参数名
		/// </summary>
		private string postUserNameParameter=String.Empty;

		/// <summary>
		/// 需要Post的帐号参数名属性
		/// </summary>
		public string PostUserNameParameter
		{
			get
			{
				return postUserNameParameter.Trim();
			}
			set
			{
				postUserNameParameter=value.Trim();
			}
		}

		/// <summary>
		/// 需要Post的帐号参数值
		/// </summary>
		private string postUserNameValue=String.Empty;

		/// <summary>
		/// 需要Post的帐号参数值属性
		/// </summary>
		public string PostUserNameValue
		{
			get
			{
				return postUserNameValue.Trim();
			}
			set
			{
				postUserNameValue=value.Trim();
			}
		}

		/// <summary>
		/// 需要Post的密码参数名
		/// </summary>
		private string postPasswordParameter=String.Empty;

		/// <summary>
		/// 需要Post的密码参数名属性
		/// </summary>
		public string PostPasswordParameter
		{
			get
			{
				return postPasswordParameter.Trim();
			}
			set
			{
				postPasswordParameter=value.Trim();
			}
		}

		/// <summary>
		/// 需要Post的密码参数值
		/// </summary>
		private string postPasswordValue=String.Empty;

		/// <summary>
		/// 需要Post的密码参数值属性
		/// </summary>
		public string PostPasswordValue
		{
			get
			{
				return postPasswordValue.Trim();
			}
			set
			{
				postPasswordValue=value.Trim();
			}
		}

		/// <summary>
		/// 需要Post的动作参数名
		/// </summary>
		private string postActionParameter=String.Empty;

		/// <summary>
		/// 需要Post的动作参数名属性
		/// </summary>
		public string PostActionParameter
		{
			get
			{
				return postActionParameter.Trim();
			}
			set
			{
				postActionParameter=value.Trim();
			}
		}

		/// <summary>
		/// 需要Post的动作参数值
		/// </summary>
		private string postActionValue=String.Empty;

		/// <summary>
		/// 需要Post的动作参数值属性
		/// </summary>
		public string PostActionValue
		{
			get
			{
				return postActionValue.Trim();
			}
			set
			{
				postActionValue=value.Trim();
			}
		}
		#endregion Post

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.method.ToString();
		}
	}

	/// <summary>
	/// 网页操作方式Method
	/// </summary>
	public enum Method
	{
		Post = 0,
		Get = 1
	}
}
