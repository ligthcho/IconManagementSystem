using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Logic;
using Model;
using System.Diagnostics;

namespace Base
{
	/// <summary>
	/// MySqlHelper操作类
	/// </summary>
	public sealed partial class MySqlHelper
	{
		#region 读取配置
		/*
		<!--┏MySQL数据库配置┓-->
		<add key="MYSQLConnections" value="MYSQLConnection1" />
		<add key="MYSQLConnection1" value="Server=192.168.0.102;Port=3306;Database=database;Uid=root;Pwd=123456;charset=utf8" />
		<!--是否输出OLEScript到Log-->
		<add key="MYSQLScriptWriteLog" value="true" />
		<!--输出OLEScript到Log的目录-->
		<add key="MySQLScriptWriteLogPath" value="/Log/" />
		<!--输出OLEScript到Log的文件扩展名-->
		<add key="MySQLScriptWriteLogExtension" value=".MYSQL.LOG" />
		<!--┗MySQL数据库配置┛--> 

		<!--┏错误日志配置┓-->
		<!--是否输出Error到Log-->
		<add key="ErrorWriteLog" value="true" />
		<!--输出Error到Log的目录-->
		<add key="ErrorWriteLogPath" value="/Log/" />
		<!--输出Error到Log的文件扩展名-->
		<add key="ErrorWriteLogExtension" value=".ERR.LOG" />
		<!--输出Error到的Log中不输出的RequestForm参数-->
		<add key="ErrorWriteLogRequestFormClear" value="*.LogInfo,*.Content"/>
		<!--┗错误日志配置┛-->
		*/
		/// <summary>
		/// 数据库链接字符串属性
		/// Server=[Server&IP];Port=[Port3306];Database=[Database];Uid=[Uid];Pwd=[Pwd];charset=utf8
		/// </summary>
		public static string MYSQL_CONNECTION
		{
			get
			{
				string[] strMYSQLConnections = ConfigLogic.GetStrings("MYSQLConnections");
				Random random = new Random();
				string strMYSQLConnectionKey = strMYSQLConnections[random.Next(0,strMYSQLConnections.Length)].ToString();
				return ConfigLogic.GetString(strMYSQLConnectionKey);
			}
		}

		public static bool MYSQL_SCRIPT_WRITE_LOG = ConfigLogic.GetBool("MYSQLScriptWriteLog");
		public static string MYSQL_SCRIPT_WRITE_LOG_PATH = ConfigLogic.GetString("MYSQLScriptWriteLogPath");
		public static string MYSQL_SCRIPT_WRITE_LOG_EXTENSION = ConfigLogic.GetString("MYSQLScriptWriteLogExtension");
		public static bool ERROR_WRITE_LOG = ConfigLogic.GetBool("ErrorWriteLog");
		public static string ERROR_WRITE_LOG_PATH = ConfigLogic.GetString("ErrorWriteLogPath");
		public static string ERROR_WRITE_LOG_EXTENSION = ConfigLogic.GetString("ErrorWriteLogExtension");
		#endregion 读取配置

		/// <summary>
		/// 批量操作每批次记录数
		/// </summary>
		public static int BatchSize=2000;

		/// <summary>
		/// 超时时间
		/// </summary>
		public static int CommandTimeOut=600;

		///// <summary>
		/////初始化MySqlHelper实例
		///// </summary>
		///// <param name="connectionString">数据库连接字符串</param>
		//public MySqlHelper(string connectionString)
		//{
		//    this.MYSQL_CONNECTION=connectionString;
		//}

		///// <summary>
		///// 数据库连接字符串
		///// </summary>
		//public string MYSQL_CONNECTION
		//{
		//    get;
		//    set;
		//}

		#region 实例方法

		#region ExecuteNonQuery

		/// <summary>
		/// 执行SQL语句,返回影响的行数
		/// </summary>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回影响的行数</returns>
		public int ExecuteNonQuery(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteNonQuery(MYSQL_CONNECTION,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回影响的行数
		/// </summary>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回影响的行数</returns>
		public int ExecuteNonQuery(CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteNonQuery(MYSQL_CONNECTION,commandType,commandText,parms);
		}

		#endregion ExecuteNonQuery

		#region ExecuteScalar

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <typeparam name="T">返回对象类型</typeparam>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public T ExecuteScalar<T>(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteScalar<T>(MYSQL_CONNECTION,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public object ExecuteScalar(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteScalar(MYSQL_CONNECTION,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public object ExecuteScalar(CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteScalar(MYSQL_CONNECTION,commandType,commandText,parms);
		}

		#endregion ExecuteScalar

		#region ExecuteDataReader

		/// <summary>
		/// 执行SQL语句,返回只读数据集
		/// </summary>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回只读数据集</returns>
		private MySqlDataReader ExecuteDataReader(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataReader(MYSQL_CONNECTION,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回只读数据集
		/// </summary>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回只读数据集</returns>
		private MySqlDataReader ExecuteDataReader(CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataReader(MYSQL_CONNECTION,commandType,commandText,parms);
		}
		#endregion

		#region ExecuteDataRow

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行
		/// </summary>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行</returns>
		public DataRow ExecuteDataRow(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataRow(MYSQL_CONNECTION,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行
		/// </summary>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行</returns>
		public DataRow ExecuteDataRow(CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataRow(MYSQL_CONNECTION,commandType,commandText,parms);
		}

		#endregion ExecuteDataRow

		#region ExecuteDataTable

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一个数据表
		/// </summary>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一个数据表</returns>
		public DataTable ExecuteDataTable(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataTable(MYSQL_CONNECTION,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一个数据表
		/// </summary>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一个数据表</returns>
		public DataTable ExecuteDataTable(CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(MYSQL_CONNECTION,commandType,commandText,parms).Tables[0];
		}

		#endregion ExecuteDataTable

		#region ExecuteDataSet

		/// <summary>
		/// 执行SQL语句,返回结果集
		/// </summary>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集</returns>
		public DataSet ExecuteDataSet(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(MYSQL_CONNECTION,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集
		/// </summary>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集</returns>
		public DataSet ExecuteDataSet(CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(MYSQL_CONNECTION,commandType,commandText,parms);
		}

		#endregion ExecuteDataSet

		#region 批量操作

		/// <summary>
		/// 使用MySqlDataAdapter批量更新数据
		/// </summary>
		/// <param name="table">数据表</param>
		public void BatchUpdate(DataTable table)
		{
			BatchUpdate(MYSQL_CONNECTION,table);
		}

		/// <summary>
		///大批量数据插入,返回成功插入行数
		/// </summary>
		/// <param name="table">数据表</param>
		/// <returns>返回成功插入行数</returns>
		public int BulkInsert(DataTable table)
		{
			return BulkInsert(MYSQL_CONNECTION,table);
		}

		#endregion 批量操作

		#endregion 实例方法

		/// <summary>
		/// 输出SQL脚本到Log
		/// </summary>
		/// <param name="dateTimeBegin">开始时间</param>
		/// <param name="dateTimeEnd">结束时间</param>
		/// <param name="strSQLScript">输出SQL脚本</param>
		public static void SQLScriptWriteLog(long longMilliseconds,string strSQLScript)
		{
			try
			{
				string strUrl = ClientModel.GetUrl();
				string strUserHostAddress = ClientModel.GetIPAdderss();
				string strLog = "/*" + longMilliseconds.ToString().PadLeft(8,' ') + "ms " + DateTime.Now.ToString("HH:mm:ss.fff") + " " + strUserHostAddress.PadLeft(15,' ') + "*/ " + strSQLScript + " /*" + strUrl + "*/";
				LogLogic.Write(strLog,MYSQL_SCRIPT_WRITE_LOG_PATH,MYSQL_SCRIPT_WRITE_LOG_EXTENSION);
			}
			catch
			{

			}
		}

		/// <summary>
		/// 测试数据库连接
		/// </summary>
		/// <param name="strMySqlConnection"></param>
		/// <param name="strMySqlState"></param>
		/// <returns></returns>
		public static bool TestMySqlConnection(string strMySqlConnection,ref string strMySqlState)
		{
			bool isMySqlServerOpen=false;
			MySqlConnection mySqlDbConnection=new MySqlConnection(strMySqlConnection);
			MySqlCommand mySqlDbCommand=new MySqlCommand();
			try
			{
				mySqlDbCommand.Connection=mySqlDbConnection;
				mySqlDbConnection.Open();
				strMySqlState=mySqlDbConnection.State.ToString();
				isMySqlServerOpen=true;
			}
			catch(Exception ex)
			{
				strMySqlState=ex.Message;
				isMySqlServerOpen=false;
			}
			mySqlDbCommand.Dispose();
			mySqlDbConnection.Close();
			mySqlDbConnection.Dispose();
			mySqlDbConnection=null;
			return isMySqlServerOpen;
		}
		#region 静态方法

		private static void PrepareCommand(MySqlCommand command,MySqlConnection connection,MySqlTransaction transaction,CommandType commandType,string commandText,MySqlParameter[] parms)
		{
			if(connection.State!=ConnectionState.Open)
				connection.Open();

			command.Connection=connection;
			command.CommandTimeout=CommandTimeOut;
			// 设置命令文本(存储过程名或SQL语句)
			command.CommandText=commandText;
			// 分配事务
			if(transaction!=null)
			{
				command.Transaction=transaction;
			}
			// 设置命令类型.
			command.CommandType=commandType;
			if(parms!=null&&parms.Length>0)
			{
				//预处理MySqlParameter参数数组，将为NULL的参数赋值为DBNull.Value;
				foreach(MySqlParameter parameter in parms)
				{
					if((parameter.Direction==ParameterDirection.InputOutput||parameter.Direction==ParameterDirection.Input)&&(parameter.Value==null))
					{
						parameter.Value=DBNull.Value;
					}
				}
				command.Parameters.AddRange(parms);
			}
		}

		#region ExecuteNonQuery

		/// <summary>
		/// 执行SQL语句,返回影响的行数
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回影响的行数</returns>
		public static int ExecuteNonQuery(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			using(MySqlConnection connection=new MySqlConnection(connectionString))
			{
				return ExecuteNonQuery(connection,CommandType.Text,commandText,parms);
			}
		}

		/// <summary>
		/// 执行SQL语句,返回影响的行数
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回影响的行数</returns>
		public static int ExecuteNonQuery(string connectionString,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			using(MySqlConnection connection=new MySqlConnection(connectionString))
			{
				return ExecuteNonQuery(connection,commandType,commandText,parms);
			}
		}

		/// <summary>
		/// 执行SQL语句,返回影响的行数
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回影响的行数</returns>
		public static int ExecuteNonQuery(MySqlConnection connection,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteNonQuery(connection,null,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回影响的行数
		/// </summary>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回影响的行数</returns>
		public static int ExecuteNonQuery(MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteNonQuery(transaction.Connection,transaction,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回影响的行数
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回影响的行数</returns>
		private static int ExecuteNonQuery(MySqlConnection connection,MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			MySqlCommand command=new MySqlCommand();
			PrepareCommand(command,connection,transaction,commandType,commandText,parms);
			int retval=command.ExecuteNonQuery();
			command.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(MYSQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=MySqlParameterCache.FormatSQLScript(String.Empty,commandType,commandText,parms);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		#endregion ExecuteNonQuery

		#region ExecuteScalar

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <typeparam name="T">返回对象类型</typeparam>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public static T ExecuteScalar<T>(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			object result=ExecuteScalar(connectionString,commandText,parms);
			if(result!=null)
			{
				return (T)Convert.ChangeType(result,typeof(T));
				;
			}
			return default(T);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public static object ExecuteScalar(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			using(MySqlConnection connection=new MySqlConnection(connectionString))
			{
				return ExecuteScalar(connection,CommandType.Text,commandText,parms);
			}
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public static object ExecuteScalar(string connectionString,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			using(MySqlConnection connection=new MySqlConnection(connectionString))
			{
				return ExecuteScalar(connection,commandType,commandText,parms);
			}
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public static object ExecuteScalar(MySqlConnection connection,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteScalar(connection,null,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		public static object ExecuteScalar(MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteScalar(transaction.Connection,transaction,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行第一列
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一行第一列</returns>
		private static object ExecuteScalar(MySqlConnection connection,MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			MySqlCommand command=new MySqlCommand();
			PrepareCommand(command,connection,transaction,commandType,commandText,parms);
			object retval=command.ExecuteScalar();
			command.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(MYSQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=MySqlParameterCache.FormatSQLScript(String.Empty,commandType,commandText,parms);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		#endregion ExecuteScalar

		#region ExecuteDataReader

		/// <summary>
		/// 执行SQL语句,返回只读数据集
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回只读数据集</returns>
		private static MySqlDataReader ExecuteDataReader(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			MySqlConnection connection=new MySqlConnection(connectionString);
			return ExecuteDataReader(connection,null,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回只读数据集
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回只读数据集</returns>
		private static MySqlDataReader ExecuteDataReader(string connectionString,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			MySqlConnection connection=new MySqlConnection(connectionString);
			return ExecuteDataReader(connection,null,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回只读数据集
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回只读数据集</returns>
		private static MySqlDataReader ExecuteDataReader(MySqlConnection connection,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataReader(connection,null,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回只读数据集
		/// </summary>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回只读数据集</returns>
		private static MySqlDataReader ExecuteDataReader(MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataReader(transaction.Connection,transaction,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回只读数据集
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回只读数据集</returns>
		private static MySqlDataReader ExecuteDataReader(MySqlConnection connection,MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			MySqlCommand command=new MySqlCommand();
			PrepareCommand(command,connection,transaction,commandType,commandText,parms);
			MySqlDataReader dr = null;

			try
			{
				dr = command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + MySqlParameterCache.FormatSQLScript(String.Empty,commandType,commandText,parms).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(MYSQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=MySqlParameterCache.FormatSQLScript(String.Empty,commandType,commandText,parms);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return dr;
		}

		#endregion

		#region ExecuteDataRow

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>,返回结果集中的第一行</returns>
		public static DataRow ExecuteDataRow(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			DataTable dt=ExecuteDataTable(connectionString,CommandType.Text,commandText,parms);
			return dt.Rows.Count>0?dt.Rows[0]:null;
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>,返回结果集中的第一行</returns>
		public static DataRow ExecuteDataRow(string connectionString,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			DataTable dt=ExecuteDataTable(connectionString,commandType,commandText,parms);
			return dt.Rows.Count>0?dt.Rows[0]:null;
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>,返回结果集中的第一行</returns>
		public static DataRow ExecuteDataRow(MySqlConnection connection,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			DataTable dt=ExecuteDataTable(connection,commandType,commandText,parms);
			return dt.Rows.Count>0?dt.Rows[0]:null;
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一行
		/// </summary>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>,返回结果集中的第一行</returns>
		public static DataRow ExecuteDataRow(MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			DataTable dt=ExecuteDataTable(transaction,commandType,commandText,parms);
			return dt.Rows.Count>0?dt.Rows[0]:null;
		}

		#endregion ExecuteDataRow

		#region ExecuteDataTable

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一个数据表
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一个数据表</returns>
		public static DataTable ExecuteDataTable(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(connectionString,CommandType.Text,commandText,parms).Tables[0];
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一个数据表
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一个数据表</returns>
		public static DataTable ExecuteDataTable(string connectionString,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(connectionString,commandType,commandText,parms).Tables[0];
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一个数据表
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一个数据表</returns>
		public static DataTable ExecuteDataTable(MySqlConnection connection,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(connection,commandType,commandText,parms).Tables[0];
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一个数据表
		/// </summary>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集中的第一个数据表</returns>
		public static DataTable ExecuteDataTable(MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(transaction,commandType,commandText,parms).Tables[0];
		}

		/// <summary>
		/// 执行SQL语句,返回结果集中的第一个数据表
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="tableName">数据表名称</param>
		/// <returns>返回结果集中的第一个数据表</returns>
		public static DataTable ExecuteEmptyDataTable(string connectionString,string tableName)
		{
			return ExecuteDataSet(connectionString,CommandType.Text,string.Format("select * from {0} where 1=-1",tableName)).Tables[0];
		}

		#endregion ExecuteDataTable

		#region ExecuteDataSet

		/// <summary>
		/// 执行SQL语句,返回结果集
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandText">SQL语句</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集</returns>
		public static DataSet ExecuteDataSet(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(connectionString,CommandType.Text,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集</returns>
		public static DataSet ExecuteDataSet(string connectionString,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			using(MySqlConnection connection=new MySqlConnection(connectionString))
			{
				return ExecuteDataSet(connection,commandType,commandText,parms);
			}
		}

		/// <summary>
		/// 执行SQL语句,返回结果集
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集</returns>
		public static DataSet ExecuteDataSet(MySqlConnection connection,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(connection,null,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集
		/// </summary>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集</returns>
		public static DataSet ExecuteDataSet(MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			return ExecuteDataSet(transaction.Connection,transaction,commandType,commandText,parms);
		}

		/// <summary>
		/// 执行SQL语句,返回结果集
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="transaction">事务</param>
		/// <param name="commandType">命令类型(存储过程,命令文本, 其它.)</param>
		/// <param name="commandText">SQL语句或存储过程名称</param>
		/// <param name="parms">查询参数</param>
		/// <returns>返回结果集</returns>
		private static DataSet ExecuteDataSet(MySqlConnection connection,MySqlTransaction transaction,CommandType commandType,string commandText,params MySqlParameter[] parms)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			MySqlCommand command=new MySqlCommand();

			PrepareCommand(command,connection,transaction,commandType,commandText,parms);
			MySqlDataAdapter adapter=new MySqlDataAdapter(command);

			DataSet ds=new DataSet();

			try
			{
				adapter.Fill(ds);
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + MySqlParameterCache.FormatSQLScript(String.Empty,commandType,commandText,parms).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			if(commandText.IndexOf("@")>0)
			{
				string strCommandText=commandText.ToLower();
				int index=strCommandText.IndexOf("where ");
				if(index<0)
				{
					index=strCommandText.IndexOf("\nwhere");
				}
				if(index>0)
				{
					ds.ExtendedProperties.Add("SQL",strCommandText.Substring(0,index-1));  //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
				}
				else
				{
					ds.ExtendedProperties.Add("SQL",strCommandText);  //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
				}
			}
			else
			{
				ds.ExtendedProperties.Add("SQL",commandText);  //将获取的语句保存在表的一个附属数组里，方便更新时生成CommandBuilder
			}

			foreach(DataTable dt in ds.Tables)
			{
				dt.ExtendedProperties.Add("SQL",ds.ExtendedProperties["SQL"]);
			}

			command.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(MYSQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=MySqlParameterCache.FormatSQLScript(String.Empty,commandType,commandText,parms);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return ds;
		}

		#endregion ExecuteDataSet

		#region 批量操作

		/// <summary>
		///使用MySqlDataAdapter批量更新数据
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="table">数据表</param>
		public static void BatchUpdate(string connectionString,DataTable table)
		{
			MySqlConnection connection=new MySqlConnection(connectionString);

			MySqlCommand command=connection.CreateCommand();
			command.CommandTimeout=CommandTimeOut;
			command.CommandType=CommandType.Text;
			MySqlDataAdapter adapter=new MySqlDataAdapter(command);
			MySqlCommandBuilder commandBulider=new MySqlCommandBuilder(adapter);
			commandBulider.ConflictOption=ConflictOption.OverwriteChanges;

			MySqlTransaction transaction=null;
			try
			{
				connection.Open();
				transaction=connection.BeginTransaction();
				//设置批量更新的每次处理条数
				adapter.UpdateBatchSize=BatchSize;
				//设置事物
				adapter.SelectCommand.Transaction=transaction;

				if(table.ExtendedProperties["SQL"]!=null)
				{
					adapter.SelectCommand.CommandText=table.ExtendedProperties["SQL"].ToString();
				}
				adapter.Update(table);
				transaction.Commit();/////提交事务
			}
			catch(MySqlException ex)
			{
				if(transaction!=null)
					transaction.Rollback();
				throw ex;
			}
			finally
			{
				connection.Close();
				connection.Dispose();
			}
		}

		/// <summary>
		///大批量数据插入,返回成功插入行数
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="table">数据表</param>
		/// <returns>返回成功插入行数</returns>
		public static int BulkInsert(string connectionString,DataTable table)
		{
			if(string.IsNullOrEmpty(table.TableName))
				throw new Exception("请给DataTable的TableName属性附上表名称");
			if(table.Rows.Count==0)
				return 0;
			int insertCount=0;
			string tmpPath=Path.GetTempFileName();
			string csv=DataTableToCsv(table);
			File.WriteAllText(tmpPath,csv);
			using(MySqlConnection conn=new MySqlConnection(connectionString))
			{
				MySqlTransaction tran=null;
				try
				{
					conn.Open();
					tran=conn.BeginTransaction();
					MySqlBulkLoader bulk=new MySqlBulkLoader(conn)
					{
						FieldTerminator=",",
						FieldQuotationCharacter='"',
						EscapeCharacter='"',
						LineTerminator="\r\n",
						FileName=tmpPath,
						NumberOfLinesToSkip=0,
						TableName=table.TableName,
					};
					bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
					insertCount=bulk.Load();
					tran.Commit();
				}
				catch(MySqlException ex)
				{
					if(tran!=null)
						tran.Rollback();
					throw ex;
				}
			}
			File.Delete(tmpPath);
			return insertCount;
		}

		/// <summary>
		///将DataTable转换为标准的CSV
		/// </summary>
		/// <param name="table">数据表</param>
		/// <returns>返回标准的CSV</returns>
		private static string DataTableToCsv(DataTable table)
		{
			//以半角逗号（即,）作分隔符，列为空也要表达其存在。
			//列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
			//列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
			StringBuilder sb=new StringBuilder();
			DataColumn colum;
			foreach(DataRow row in table.Rows)
			{
				for(int i=0;i<table.Columns.Count;i++)
				{
					colum=table.Columns[i];
					if(i!=0)
						sb.Append(",");
					if(colum.DataType==typeof(string)&&row[colum].ToString().Contains(","))
					{
						sb.Append("\""+row[colum].ToString().Replace("\"","\"\"")+"\"");
					}
					else
						sb.Append(row[colum].ToString());
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		#endregion 批量操作

		#endregion 静态方法


	}

	/// <summary>
	/// MySqlHelper扩展(依赖AutoMapper.dll)
	/// </summary>
	public sealed partial class MySqlHelper
	{
		#region 实例方法

		public T ExecuteObject<T>(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteObject<T>(MYSQL_CONNECTION,commandText,parms);
		}

		public List<T> ExecuteObjects<T>(string commandText,params MySqlParameter[] parms)
		{
			return ExecuteObjects<T>(MYSQL_CONNECTION,commandText,parms);
		}

		#endregion

		#region 静态方法
		public static T ExecuteObject<T>(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			//DataTable dt = ExecuteDataTable(connectionString, commandText, parms);
			//return AutoMapper.Mapper.DynamicMap<List<T>>(dt.CreateDataReader()).FirstOrDefault();
			using(MySqlDataReader reader = ExecuteDataReader(connectionString,commandText,parms))
			{
				return AutoMapper.Mapper.DynamicMap<List<T>>(reader).FirstOrDefault();
			}
		}

		public static List<T> ExecuteObjects<T>(string connectionString,string commandText,params MySqlParameter[] parms)
		{
			//DataTable dt = ExecuteDataTable(connectionString, commandText, parms);
			//return AutoMapper.Mapper.DynamicMap<List<T>>(dt.CreateDataReader());
			using(MySqlDataReader reader = ExecuteDataReader(connectionString,commandText,parms))
			{
				return AutoMapper.Mapper.DynamicMap<List<T>>(reader);
			}
		}
		#endregion 静态方法
	}

	public sealed class MySqlParameterCache
	{
		/// <summary>
		/// 生成可以在查询分析器中执行的存储过程或SQL语句
		/// </summary>
		/// <param name="strUseDataBaseName">数据库名称，“use dbname”</param>
		/// <param name="commandType">指定如何解释命令字符串：存储过程或SQL语句</param>
		/// <param name="commandText">存储过程或SQL语句的字符串</param>
		/// <param name="parms">SQL参数</param>
		/// <returns>返回可以在查询分析器中执行字符串</returns>
		public static string FormatSQLScript(string strUseDataBaseName,CommandType commandType,string commandText,MySqlParameter[] parms)
		{
			string strSQLScript=String.Empty;
			if(parms==null)
			{
				strSQLScript=commandText;
			}
			else
			{
				if(commandType==CommandType.StoredProcedure)
				{
					for(int i=0;i<parms.Length;i++)
					{
						if(
								parms[i].MySqlDbType==MySqlDbType.VarChar||
								parms[i].MySqlDbType==MySqlDbType.VarString||
								parms[i].MySqlDbType==MySqlDbType.Date
							)
						{
							strSQLScript+="'"+Convert.ToString(parms[i].Value).Replace("'","''")+"',\r\n";
						}
						else
						{
							strSQLScript+=Convert.ToString(parms[i].Value)+",\r\n";
						}
					}
					if(strSQLScript.EndsWith(",\r\n"))
					{
						strSQLScript=strSQLScript.Substring(0,strSQLScript.Length-3);
					}
					strSQLScript="EXEC "+commandText+" "+strSQLScript;
				}
				else if(commandType==CommandType.Text)
				{
					for(int i=0;i<parms.Length;i++)
					{
						if(
								parms[i].MySqlDbType==MySqlDbType.VarChar||
								parms[i].MySqlDbType==MySqlDbType.VarString||
								parms[i].MySqlDbType==MySqlDbType.Date
							)
						{
							commandText=commandText.Replace(parms[i].ParameterName,"'"+Convert.ToString(parms[i].Value).Replace("'","''")+"'");
						}
						else
						{
							commandText=commandText.Replace(parms[i].ParameterName,Convert.ToString(parms[i].Value));
						}
					}
					strSQLScript=commandText;
				}
			}
			if(strUseDataBaseName!=String.Empty&&strSQLScript!=String.Empty)
			{
				strSQLScript="USE "+strUseDataBaseName+"\r\n "+strSQLScript;
			}

			return strSQLScript;
		}
	}
}