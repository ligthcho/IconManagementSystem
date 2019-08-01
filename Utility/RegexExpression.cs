using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility
{
	/// <summary>
	/// 正则表达式
	/// </summary>
	public static class RegexExpression
	{

		/// <summary>
		/// 数据验证类使用的正则表述式选项
		/// </summary>
		public const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.Compiled;

		/// <summary>
		/// 检测字符串是否为数字捕获正则
		/// </summary>
		public static readonly Regex NumberRegex = new Regex(@"^[0-9]+$",Options);

		/// <summary>
		/// 检测字符串是否为数字（可带正负号）捕获正则
		/// </summary>
		public static readonly Regex NumberSignRegex = new Regex(@"^[+-]?[0-9]+$",Options);

		/// <summary>
		/// 检测字符串是否为浮点数捕获正则
		/// </summary>
		public static readonly Regex DecimalRegex = new Regex(@"^[0-9]+(\.[0-9]+)?$",Options);

		/// <summary>
		/// 检测字符串是否是有效的IP地址捕获正则
		/// </summary>
		public static readonly Regex IPRegex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$",Options);

		/// <summary>
		/// 检测字符串是否为有效的URL地址捕获正则
		/// </summary>
		public static readonly Regex UrlRegex = new Regex(@"^http(s)?://(([\w-]+\.)+[\w-]+|localhost)(:\d{1,5})?(/[\w- ./?%&=]*)?",Options);

		/// <summary>
		/// 检测字符串是否为有效的邮件地址捕获正则
		/// </summary>
		public static readonly Regex EmailRegex = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",Options);

		/// <summary>
		/// 检测字符串是否为有效的邮政编码捕获正则
		/// </summary>
		public static readonly Regex PostCodeRegex = new Regex(@"^\d{6}$",Options);

		/// <summary>
		/// 检测字符串是否为有效的日期捕获正则
		/// </summary>
		public static readonly Regex DateRegex = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$",Options);

		/// <summary>
		/// 检测字符串是否为有效的时间捕获正则
		/// </summary>
		public static readonly Regex TimeRegex = new Regex(@"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$",Options);

		/// <summary>
		/// 检测字符串是否为有效的日期时间捕获正则
		/// </summary>
		public static readonly Regex DateTimeRegex = new Regex(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$",Options);

	}
}
