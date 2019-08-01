using System;
using System;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace Models
{
	public partial class UIDBEntities:DbContext
	{
		public string ConnectionString
		{
			get
			{
				return string.Format("data source={0};initial catalog={1};user id={2};password={3};",
									this.data_source,
									this.initial_catalog,
									this.user_id,
									this.password);
			}
		}
		public string data_source
		{
			get
			{
				var val = new Regex(@".*data\ source=(?<value>.*?);",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				).Match(getConn()).Groups["value"].Value;
				return val;
			}
		}
		public string initial_catalog
		{
			get
			{
				var val = new Regex(@".*initial\ catalog=(?<value>.*?);",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				).Match(getConn()).Groups["value"].Value;
				return val;
			}
		}
		public string user_id
		{
			get
			{
				var val = new Regex(@".*user\ id=(?<value>.*?);",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				).Match(getConn()).Groups["value"].Value;
				return val;
			}
		}
		public string password
		{
			get
			{
				var val = new Regex(@".*password=(?<value>.*?);",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				).Match(getConn()).Groups["value"].Value;
				return val;
			}
		}
		private string getConn()
		{
			string conn = System.Configuration.ConfigurationManager.ConnectionStrings["UIDBEntities"].ToString();
			if(string.IsNullOrEmpty(conn))
			{
				throw new Exception("没有配置连接字符串:UIDBEntities");
			}
			return conn;
		}
	}
}
