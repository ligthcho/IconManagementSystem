using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utility
{
	/// <summary>
	/// 字符串扩展类
	/// </summary>
	public static class StringExtension
	{

		/// <summary>
		/// 指示指定的字符串是 Empty 字符串
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsEmpty(this string s)
		{
			return s == string.Empty;
		}

		/// <summary>
		/// 指示指定的字符串不是 Empty 字符串
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsNotEmpty(this string s)
		{
			return !s.IsEmpty();
		}

		/// <summary>
		/// 指示指定的字符串是 null 还是 Empty 字符串
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

		/// <summary>
		/// 指示指定的字符串是否不为空
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsNotNullAndEmpty(this string s)
		{
			return !string.IsNullOrEmpty(s);
		}

		/// <summary>
		/// 合并拼接路径
		/// </summary>
		/// <param name="path"></param>
		/// <param name="append"></param>
		/// <returns></returns>
		public static string Combine(this string path,string append)
		{
			return Path.Combine(path,append);
		}

		/// <summary>
		/// 判断是不是图片(根据其扩展名)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsImage(this string input)
		{
			var extesion = new[] { "jpg","jpeg","png","tiff","gif","bmp" };
			return extesion.Any(s => input.ToLower().EndsWith("." + s));
		}

		#region 正则相关

		/// <summary>
		/// 从指定的字符串中移除Html
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string RemoveHtml(this string input)
		{
			var stripTags = new Regex("</?[a-z][^<>]*>",RegexOptions.IgnoreCase);
			return stripTags.Replace(input,string.Empty);
		}

		/// <summary>
		/// 指示正则表达式在输入字符串中是否找到匹配项
		/// </summary>
		/// <param name="s"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static bool IsMatch(this string s,string pattern)
		{
			if(s == null)
				return false;
			else
				return Regex.IsMatch(s,pattern);
		}

		/// <summary>
		/// 在输入字符串中搜索匹配正则表达式模式的子字符串，并将第一个匹配项作为单个 Match 对象的值返回
		/// </summary>
		/// <param name="s"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static string Match(this string s,string pattern)
		{
			if(s == null)
				return "";
			return Regex.Match(s,pattern).Value;
		}

		/// <summary>
		/// 检测字符串是否为数字
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为数字，则为 true；否则为 false。</returns>
		public static bool IsNumber(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.NumberRegex.IsMatch(input);
			}
		}

		/// <summary>
		/// 检测字符串是否为数字，可带正负号
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为数字，则为 true；否则为 false。</returns>
		public static bool IsNumberSign(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.NumberSignRegex.IsMatch(input);
			}
		}

		/// <summary>
		/// 检测字符串是否为浮点数
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为浮点数，则为 true；否则为 false。</returns>
		public static bool IsDecimal(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.DecimalRegex.IsMatch(input);
			}
		}

		/// <summary>
		/// 判断字符串是否是有效的IP地址
		/// </summary>
		/// <param name="input">IP地址字符串</param>
		/// <returns>有效IP地址返回true ；否则返回false</returns>
		public static bool IsIP(this string input)
		{
			if(!string.IsNullOrEmpty(input))
			{
				return RegexExpression.IPRegex.IsMatch(input);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 检测字符串是否为有效的URL地址
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为有效的URL地址，则为 true；否则为 false。</returns>
		public static bool IsUrl(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.UrlRegex.IsMatch(input);
			}
		}

		/// <summary>
		/// 检测字符串是否为有效的邮件地址
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为有效的邮件地址，则为 true；否则为 false。</returns>
		public static bool IsEmail(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.EmailRegex.IsMatch(input);
			}
		}

		/// <summary>
		/// 检测字符串是否为有效的邮政编码
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为有效的邮政编码，则为 true；否则为 false。</returns>
		public static bool IsPostCode(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.PostCodeRegex.IsMatch(input);
			}
		}
		/// <summary>
		/// 检测字符串是否为有效的日期
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为有效的日期，则为 true；否则为 false。</returns>
		public static bool IsDate(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.DateRegex.IsMatch(input);
			}
		}

		/// <summary>
		/// 检测字符串是否为有效的时间
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为有效的时间，则为 true；否则为 false。</returns>
		public static bool IsTime(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.TimeRegex.IsMatch(input);
			}
		}

		/// <summary>
		/// 检测字符串是否为有效的日期时间
		/// </summary>
		/// <param name="input">需要检查的字符串</param>
		/// <returns>如果字符串为有效的日期时间，则为 true；否则为 false。</returns>
		public static bool IsDateTime(this string input)
		{
			if(string.IsNullOrEmpty(input))
			{
				return false;
			}
			else
			{
				return RegexExpression.DateTimeRegex.IsMatch(input);
			}
		}
		#endregion
	}
}
