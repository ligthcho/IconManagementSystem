using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Xml;
using Logic;
using Model;
using System.Diagnostics;

namespace Base
{
	/// <summary>
	/// The SqlHelper class is intended to encapsulate high performance, scalable best practices for 
	/// common uses of SqlClient.
	/// </summary>
	public sealed class SqlHelper
	{
		#region 读取配置
		/*
		<!--┏SQL数据库配置┓-->
		<add key="SQLConnections" value="SQLConnection1" />
		<add key="SQLConnection1" value="Data Source=[SQLHostName];User Id=[SQLUserName];Password=[SQLPassword];Initial Catalog=[SQLDateBase];" />
		<!--是否输出SQLScript到Log-->
		<add key="SQLScriptWriteLog" value="true"/>
		<!--输出SQLScript到Log的目录-->
		<add key="SQLScriptWriteLogPath" value="/Log/"/>
		<!--输出SQLScript到Log的文件扩展名-->
		<add key="SQLScriptWriteLogExtension" value=".SQL.LOG"/>
		<!--┗SQL数据库配置┛-->

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
		/// Data Source=[SQLHostName];User Id=[SQLUserName];Password=[SQLPassword];Initial Catalog=[SQLDateBase];
		/// </summary>
		public static string SQL_CONNECTION
		{
			get
			{
				string[] strSQLConnections = ConfigLogic.GetStrings("SQLConnections");
				Random random = new Random();
				string strSQLConnectionKey = strSQLConnections[random.Next(0,strSQLConnections.Length)].ToString();
				return ConfigLogic.GetString(strSQLConnectionKey);
			}
		}

		public static bool SQL_SCRIPT_WRITE_LOG = ConfigLogic.GetBool("SQLScriptWriteLog");
		public static string SQL_SCRIPT_WRITE_LOG_PATH = ConfigLogic.GetString("SQLScriptWriteLogPath");
		public static string SQL_SCRIPT_WRITE_LOG_EXTENSION = ConfigLogic.GetString("SQLScriptWriteLogExtension");
		public static bool ERROR_WRITE_LOG = ConfigLogic.GetBool("ErrorWriteLog");
		public static string ERROR_WRITE_LOG_PATH = ConfigLogic.GetString("ErrorWriteLogPath");
		public static string ERROR_WRITE_LOG_EXTENSION = ConfigLogic.GetString("ErrorWriteLogExtension");
		#endregion 读取配置

		#region 运行SP
		public static DataSet RunStoredProcedureToDataSet(string commandText,Hashtable hashtableParameter)
		{
			commandText=commandText.ToLower();
			DataSet dataSet=new DataSet();
			SqlParameter[] commandParameters=new SqlParameter[0];
			try
			{
				List<SqlHelperParameter> sqlHelperParameterList = new List<SqlHelperParameter>();
				sqlHelperParameterList = GetParameterList(commandText);
				if(sqlHelperParameterList.Count > 0)
				{
					commandParameters = new SqlParameter[sqlHelperParameterList.Count];
					for(int i = 0;i < sqlHelperParameterList.Count;i++)
					{
						SqlHelperParameter sqlHelperParameter = sqlHelperParameterList[i];
						commandParameters[i] = sqlHelperParameter.SqlParameter;

						string strKey = sqlHelperParameter.Name.Replace("@",string.Empty);
						string strValue=string.Empty;
						if(hashtableParameter.ContainsKey(strKey))
						{
							if(hashtableParameter[strKey]!=null)
							{
								strValue=hashtableParameter[strKey].ToString();
							}
						}
						if(
						commandParameters[i].SqlDbType==SqlDbType.Int||
						commandParameters[i].SqlDbType==SqlDbType.Decimal||
						commandParameters[i].SqlDbType==SqlDbType.BigInt||
						commandParameters[i].SqlDbType==SqlDbType.Float||
						commandParameters[i].SqlDbType==SqlDbType.Real||
						commandParameters[i].SqlDbType==SqlDbType.SmallInt||
						commandParameters[i].SqlDbType==SqlDbType.SmallMoney||
						commandParameters[i].SqlDbType==SqlDbType.TinyInt
						)
						{
							if(strValue.Trim()==string.Empty)
							{
								commandParameters[i].Value=0;
							}
							else
							{
								commandParameters[i].Value=strValue;
							}
						}
						else
						{
							commandParameters[i].Value=strValue;
						}
					}
				}
				using(SqlConnection sqlConnection=new SqlConnection(SQL_CONNECTION))
				{
					try
					{
						sqlConnection.Open();
						dataSet=SqlHelper.ExecuteDataset(sqlConnection,CommandType.StoredProcedure,commandText,commandParameters);
					}
					catch(Exception ex)
					{
						string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,CommandType.StoredProcedure,commandText,commandParameters).Replace("\r\n",String.Empty);
						LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
						throw ex;
					}
				}
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,CommandType.StoredProcedure,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
				throw ex;
			}
			return dataSet;
		}

		public static string RunStoredProcedureToXML(string commandText,Hashtable hashtableParameter)
		{
			string strReturn=string.Empty;
			DataSet dataSet=RunStoredProcedureToDataSet(commandText,hashtableParameter);
			strReturn=dataSet.GetXml();
			dataSet.Dispose();
			return strReturn;			
		}

		public static string RunStoredProcedureToJson(string commandText,Hashtable hashtableParameter)
		{
			string strReturn = string.Empty;
			DataSet dataSet = RunStoredProcedureToDataSet(commandText,hashtableParameter);
			strReturn = JsonLogic.ToJson(dataSet);
			dataSet.Dispose();
			return strReturn;
		}

		public static List<SqlHelperParameter> GetParameterList(string commandText)
		{
			List<SqlHelperParameter> sqlHelperParameterList = new List<SqlHelperParameter>();
			string sql="select p.name as Name,t.name as Type,p.length as Length from sysobjects sp, syscolumns p, systypes t where sp.name='"+commandText+ "' and sp.xtype='p' and sp.id=p.id and p.xtype=t.xtype and t.name<>'sysname' order by p.colorder";
			using(SqlConnection sqlConnection=new SqlConnection(SQL_CONNECTION))
			{
				sqlConnection.Open();
				DataSet dataSet=SqlExecuteDataset(sqlConnection,CommandType.Text,sql);

				if(dataSet.Tables.Count>0&&dataSet.Tables[0].Rows.Count>0)
				{
					for(int i=0;i<dataSet.Tables[0].Rows.Count;i++)
					{
						string strName=string.Empty;
						if(!Convert.IsDBNull(dataSet.Tables[0].Rows[i]["Name"]))
						{
							strName=dataSet.Tables[0].Rows[i]["Name"].ToString().Trim();
						}
						string strType=string.Empty;
						if(!Convert.IsDBNull(dataSet.Tables[0].Rows[i]["Type"]))
						{
							strType=dataSet.Tables[0].Rows[i]["Type"].ToString().Trim();
						}
						int intLength=0;
						if(!Convert.IsDBNull(dataSet.Tables[0].Rows[i]["Length"]))
						{
							intLength=Convert.ToInt32(dataSet.Tables[0].Rows[i]["Length"].ToString().Trim());
						}
						SqlHelperParameter sqlHelperParameter = new SqlHelperParameter(strName,strType,intLength);
						if(sqlHelperParameter != null)
						{
							sqlHelperParameterList.Add(sqlHelperParameter);
						}
					}
				}
			}
			return sqlHelperParameterList;
		}
		#endregion 运行SP

		#region private utility methods & constructors

		/// <summary>
		/// Since this class provides only static methods, make the default constructor private to prevent 
		/// </summary>
		public SqlHelper()
		{
		}


		/// <summary>
		/// This method is used to attach array of SqlParameters to a SqlCommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">an array of SqlParameters tho be added to command</param>
		private static void AttachParameters(SqlCommand command,SqlParameter[] commandParameters)
		{
			foreach(SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if((p.Direction==ParameterDirection.InputOutput)&&(p.Value==null))
				{
					p.Value=DBNull.Value;
				}

				command.Parameters.Add(p);
			}
		}

		/// <summary>
		/// This method assigns an array of values to an array of SqlParameters.
		/// </summary>
		/// <param name="commandParameters">array of SqlParameters to be assigned values</param>
		/// <param name="parameterValues">array of objects holding the values to be assigned</param>
		private static void AssignParameterValues(SqlParameter[] commandParameters,object[] parameterValues)
		{
			if((commandParameters==null)||(parameterValues==null))
			{
				//do nothing if we get no data
				return;
			}

			// we must have the same number of values as we pave parameters to put them in
			if(commandParameters.Length!=parameterValues.Length)
			{
				throw new ArgumentException("Parameter count does not match Parameter Value count.");
			}

			//iterate through the SqlParameters, assigning the values from the corresponding position in the 
			//value array
			for(int i=0,j=commandParameters.Length;i<j;i++)
			{
				commandParameters[i].Value=parameterValues[i];
			}
		}

		/// <summary>
		/// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
		/// to the provided command.
		/// </summary>
		/// <param name="command">the SqlCommand to be prepared</param>
		/// <param name="connection">a valid SqlConnection, on which to execute this command</param>
		/// <param name="transaction">a valid SqlTransaction, or 'null'</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
		private static void PrepareCommand(SqlCommand command,SqlConnection connection,SqlTransaction transaction,CommandType commandType,string commandText,SqlParameter[] commandParameters)
		{
			//if the provided connection is not open, we will open it
			if(connection.State!=ConnectionState.Open)
			{
				connection.Open();
			}

			//associate the connection with the command
			command.Connection=connection;

			//set command timeout value
			command.CommandTimeout=3600;

			//set the command text (stored procedure name or SQL statement)
			command.CommandText=commandText;

			//if we were provided a transaction, assign it.
			if(transaction!=null)
			{
				command.Transaction=transaction;
			}

			//set the command type
			command.CommandType=commandType;

			//attach the command parameters if they are provided
			if(commandParameters!=null)
			{
				AttachParameters(command,commandParameters);
			}

			return;
		}


		#endregion private utility methods & constructors

		#region ExecuteNonQuery

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(connectionString,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using(SqlConnection cn=new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteNonQuery(cn,commandType,commandText,commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored prcedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteNonQuery(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteNonQuery(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(connection,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection connection,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,connection,(SqlTransaction)null,commandType,commandText,commandParameters);

			int retval=0;
			try
			{
				//finally, execute the command.
				retval=cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteNonQuery(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteNonQuery(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(transaction,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlTransaction transaction,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,transaction.Connection,transaction,commandType,commandText,commandParameters);

			int retval=0;
			try
			{
				//finally, execute the command.
				retval=cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified 
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteNonQuery(transaction,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteNonQuery(transaction,CommandType.StoredProcedure,spName);
			}
		}


		#endregion ExecuteNonQuery

		#region ExecuteDataSet

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(connectionString,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using(SqlConnection cn=new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteDataset(cn,commandType,commandText,commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteDataset(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteDataset(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(connection,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,connection,(SqlTransaction)null,commandType,commandText,commandParameters);

			//create the DataAdapter & DataSet
			SqlDataAdapter da=new SqlDataAdapter(cmd);
			DataSet ds=new DataSet();

			try
			{
				//fill the DataSet using default values for DataTable names, etc.
				da.Fill(ds);
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.			
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			//return the dataset
			return ds;
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet SqlExecuteDataset(SqlConnection connection,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,connection,(SqlTransaction)null,commandType,commandText,commandParameters);

			//create the DataAdapter & DataSet
			SqlDataAdapter da=new SqlDataAdapter(cmd);
			DataSet ds=new DataSet();

			try
			{
				//fill the DataSet using default values for DataTable names, etc.
				da.Fill(ds);
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.			
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			//return the dataset
			return ds;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteDataset(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteDataset(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(transaction,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlTransaction transaction,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,transaction.Connection,transaction,commandType,commandText,commandParameters);

			//create the DataAdapter & DataSet
			SqlDataAdapter da=new SqlDataAdapter(cmd);
			DataSet ds=new DataSet();

			try
			{
				//fill the DataSet using default values for DataTable names, etc.
				da.Fill(ds);
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			//return the dataset
			return ds;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteDataset(transaction,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteDataset(transaction,CommandType.StoredProcedure,spName);
			}
		}

		#endregion ExecuteDataSet

		#region ExecuteReader

		/// <summary>
		/// this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
		/// we can set the appropriate CommandBehavior when calling ExecuteReader()
		/// </summary>
		private enum SqlConnectionOwnership
		{
			/// <summary>Connection is owned and managed by SqlHelper</summary>
			Internal,
			/// <summary>Connection is owned and managed by the caller</summary>
			External
		}

		/// <summary>
		/// Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
		/// </summary>
		/// <remarks>
		/// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
		/// 
		/// If the caller provided the connection, we want to leave it to them to manage.
		/// </remarks>
		/// <param name="connection">a valid SqlConnection, on which to execute this command</param>
		/// <param name="transaction">a valid SqlTransaction, or 'null'</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
		/// <param name="connectionOwnership">indicates whether the connection parameter was provided by the caller, or created by SqlHelper</param>
		/// <returns>SqlDataReader containing the results of the command</returns>
		private static SqlDataReader ExecuteReader(SqlConnection connection,SqlTransaction transaction,CommandType commandType,string commandText,SqlParameter[] commandParameters,SqlConnectionOwnership connectionOwnership)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,connection,transaction,commandType,commandText,commandParameters);

			//create a reader
			SqlDataReader dr=null;

			try
			{
				// call ExecuteReader with the appropriate CommandBehavior
				if(connectionOwnership==SqlConnectionOwnership.External)
				{
					dr=cmd.ExecuteReader();
				}
				else
				{
                    
					dr=cmd.ExecuteReader(CommandBehavior.CloseConnection);
				}
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
            stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript        

			if(SQL_SCRIPT_WRITE_LOG)
			{
               
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript
          
			return dr;
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteReader(connectionString,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(string connectionString,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection
			SqlConnection cn=new SqlConnection(connectionString);
			cn.Open();

			try
			{
				//call the private overload that takes an internally owned connection in place of the connection string
				return ExecuteReader(cn,null,commandType,commandText,commandParameters,SqlConnectionOwnership.Internal);
			}
			catch(Exception es)
			{
				//if we fail to return the SqlDatReader, we need to close the connection ourselves
				es.ToString();
				cn.Close();
				throw;
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteReader(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteReader(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(SqlConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteReader(connection,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(SqlConnection connection,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			//pass through the call to the private overload using a null transaction value and an externally owned connection
			return ExecuteReader(connection,(SqlTransaction)null,commandType,commandText,commandParameters,SqlConnectionOwnership.External);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(SqlConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				AssignParameterValues(commandParameters,parameterValues);

				return ExecuteReader(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteReader(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(SqlTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteReader(transaction,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(SqlTransaction transaction,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			//pass through to private overload, indicating that the connection is owned by the caller
			return ExecuteReader(transaction.Connection,transaction,commandType,commandText,commandParameters,SqlConnectionOwnership.External);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
		public static SqlDataReader ExecuteReader(SqlTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				AssignParameterValues(commandParameters,parameterValues);

				return ExecuteReader(transaction,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteReader(transaction,CommandType.StoredProcedure,spName);
			}
		}

		#endregion ExecuteReader

		#region ExecuteScalar

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(connectionString,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using(SqlConnection cn=new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteScalar(cn,commandType,commandText,commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteScalar(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteScalar(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(connection,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlConnection connection,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,connection,(SqlTransaction)null,commandType,commandText,commandParameters);

			object retval=new object();
			try
			{
				//execute the command & return the results
				retval=cmd.ExecuteScalar();
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;

		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteScalar(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteScalar(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(transaction,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlTransaction transaction,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,transaction.Connection,transaction,commandType,commandText,commandParameters);

			object retval=new object();
			try
			{
				//execute the command & return the results
				retval=cmd.ExecuteScalar();
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteScalar(transaction,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteScalar(transaction,CommandType.StoredProcedure,spName);
			}
		}

		#endregion ExecuteScalar

		#region ExecuteXmlReader

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
		/// <returns>an XmlReader containing the resultset generated by the command</returns>
		public static XmlReader ExecuteXmlReader(SqlConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteXmlReader(connection,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an XmlReader containing the resultset generated by the command</returns>
		public static XmlReader ExecuteXmlReader(SqlConnection connection,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,connection,(SqlTransaction)null,commandType,commandText,commandParameters);

			XmlReader retval=null;
			try
			{
				//create the DataAdapter & DataSet
				retval=cmd.ExecuteXmlReader();
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;

		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure using "FOR XML AUTO"</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an XmlReader containing the resultset generated by the command</returns>
		public static XmlReader ExecuteXmlReader(SqlConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteXmlReader(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteXmlReader(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
		/// <returns>an XmlReader containing the resultset generated by the command</returns>
		public static XmlReader ExecuteXmlReader(SqlTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteXmlReader(transaction,commandType,commandText,(SqlParameter[])null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command using "FOR XML AUTO"</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an XmlReader containing the resultset generated by the command</returns>
		public static XmlReader ExecuteXmlReader(SqlTransaction transaction,CommandType commandType,string commandText,params SqlParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			SqlCommand cmd=new SqlCommand();
			PrepareCommand(cmd,transaction.Connection,transaction,commandType,commandText,commandParameters);

			XmlReader retval=null;

			try
			{
				//create the DataAdapter & DataSet
				retval=cmd.ExecuteXmlReader();
			}
			catch(Exception ex)
			{
				string strLog="/*"+DateTime.Now.ToString("HH:mm:ss.fff")+"\t"+ex.Message.Replace("\r\n",String.Empty)+"*/ "+SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(SQL_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=SqlHelperParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
		/// SqlTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static XmlReader ExecuteXmlReader(SqlTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				SqlParameter[] commandParameters=SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of SqlParameters
				return ExecuteXmlReader(transaction,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteXmlReader(transaction,CommandType.StoredProcedure,spName);
			}
		}


		#endregion ExecuteXmlReader

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
				string strLog="/*"+ longMilliseconds.ToString().PadLeft(8,' ')+"ms "+DateTime.Now.ToString("HH:mm:ss.fff")+" "+strUserHostAddress.PadLeft(15,' ')+"*/ "+strSQLScript+" /*"+strUrl+"*/";
				LogLogic.Write(strLog,SQL_SCRIPT_WRITE_LOG_PATH,SQL_SCRIPT_WRITE_LOG_EXTENSION);
			}
			catch
			{

			}
		}
	}

	/// <summary>
	/// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
	/// ability to discover parameters for stored procedures at run-time.
	/// </summary>
	public sealed class SqlHelperParameterCache
	{
		#region private methods, variables, and constructors

		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new SqlHelperParameterCache()".
		private SqlHelperParameterCache()
		{
		}

		private static Hashtable paramCache=Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// resolve at run time the appropriate set of SqlParameters for a stored procedure
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
		private static SqlParameter[] DiscoverSpParameterSet(string connectionString,string spName,bool includeReturnValueParameter)
		{
			using(SqlConnection cn=new SqlConnection(connectionString))
			using(SqlCommand cmd=new SqlCommand(spName,cn))
			{
				cn.Open();
				cmd.CommandType=CommandType.StoredProcedure;

				SqlCommandBuilder.DeriveParameters(cmd);

				if(!includeReturnValueParameter)
				{
					cmd.Parameters.RemoveAt(0);
				}

				SqlParameter[] discoveredParameters=new SqlParameter[cmd.Parameters.Count];
				;

				cmd.Parameters.CopyTo(discoveredParameters,0);

				return discoveredParameters;
			}
		}

		//deep copy of cached SqlParameter array
		private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
		{
			SqlParameter[] clonedParameters=new SqlParameter[originalParameters.Length];

			for(int i=0,j=originalParameters.Length;i<j;i++)
			{
				clonedParameters[i]=(SqlParameter)((ICloneable)originalParameters[i]).Clone();
			}

			return clonedParameters;
		}

		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// add parameter array to the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters to be cached</param>
		public static void CacheParameterSet(string hashKey,params SqlParameter[] commandParameters)
		{
			//string hashKey = connectionString + "@" + commandText;

			paramCache[hashKey]=commandParameters;
		}

		/// <summary>
		/// retrieve a parameter array from the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an array of SqlParamters</returns>
		public static SqlParameter[] GetCachedParameterSet(string hashKey)//connectionString, string commandText)
		{
			//string hashKey = connectionString + "@" + commandText;

			SqlParameter[] cachedParameters=(SqlParameter[])paramCache[hashKey];

			if(cachedParameters==null)
			{
				return null;
			}
			else
			{
				return CloneParameters(cachedParameters);
			}
		}

		#endregion caching functions

		#region Parameter Discovery Functions

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an array of SqlParameters</returns>
		public static SqlParameter[] GetSpParameterSet(string connectionString,string spName)
		{
			return GetSpParameterSet(connectionString,spName,false);
		}

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
		/// <returns>an array of SqlParameters</returns>
		public static SqlParameter[] GetSpParameterSet(string connectionString,string spName,bool includeReturnValueParameter)
		{
			string hashKey = connectionString + "@" + spName + (includeReturnValueParameter ? "@include ReturnValue Parameter" : string.Empty);

			SqlParameter[] cachedParameters;

			cachedParameters=(SqlParameter[])paramCache[hashKey];

			if(cachedParameters==null)
			{
				cachedParameters=(SqlParameter[])(paramCache[hashKey]=DiscoverSpParameterSet(connectionString,spName,includeReturnValueParameter));
			}

			return CloneParameters(cachedParameters);
		}

		#endregion Parameter Discovery Functions

		/// <summary>
		/// 将传入的SQL参数过滤数据库无法执行的字符
		/// </summary>
		/// <param name="commandParameters">返回传入的SQL参数，已经过滤了数据库无法执行的字符</param>
		/// <returns>是否发生过滤</returns>
		public static bool FilterParameter(ref SqlParameter[] commandParameters)
		{
			int intFilter=0;
			if(commandParameters!=null)
			{
				for(int i=0;i<commandParameters.Length;i++)
				{
					if(commandParameters[i].SqlDbType==SqlDbType.VarChar||commandParameters[i].SqlDbType==SqlDbType.Text)
					{
						commandParameters[i].Value=Convert.ToString(commandParameters[i].Value).Replace("'","’");
						intFilter++;
					}
				}
			}
			if(intFilter!=0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 生成可以在查询分析器中执行的存储过程或SQL语句
		/// </summary>
		/// <param name="strUseDataBaseName">数据库名称，“use dbname”</param>
		/// <param name="commandType">指定如何解释命令字符串：存储过程或SQL语句</param>
		/// <param name="commandText">存储过程或SQL语句的字符串</param>
		/// <param name="commandParameters">SQL参数</param>
		/// <returns>返回可以在查询分析器中执行字符串</returns>
		public static string FormatSQLScript(string strUseDataBaseName,CommandType commandType,string commandText,SqlParameter[] commandParameters)
		{
			string strSQLScript=String.Empty;
			if(commandType==CommandType.StoredProcedure)
			{
				if(commandParameters!=null)
				{
					for(int i=0;i<commandParameters.Length;i++)
					{
						if(
								commandParameters[i].SqlDbType==SqlDbType.Char||
								commandParameters[i].SqlDbType==SqlDbType.VarChar||
								commandParameters[i].SqlDbType==SqlDbType.Text||
								commandParameters[i].SqlDbType==SqlDbType.NChar||
								commandParameters[i].SqlDbType==SqlDbType.NVarChar||
								commandParameters[i].SqlDbType==SqlDbType.NText||
								commandParameters[i].SqlDbType==SqlDbType.DateTime
							)
						{
							strSQLScript+="'"+Convert.ToString(commandParameters[i].Value).Replace("'","''")+"',\r\n";
						}
						else
						{
							strSQLScript+=Convert.ToString(commandParameters[i].Value)+",\r\n";
						}
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
				if(commandParameters!=null)
				{
					for(int i=0;i<commandParameters.Length;i++)
					{
						if(
							commandParameters[i].SqlDbType==SqlDbType.Char||
							commandParameters[i].SqlDbType==SqlDbType.VarChar||
							commandParameters[i].SqlDbType==SqlDbType.Text||
							commandParameters[i].SqlDbType==SqlDbType.NChar||
							commandParameters[i].SqlDbType==SqlDbType.NVarChar||
							commandParameters[i].SqlDbType==SqlDbType.NText||
							commandParameters[i].SqlDbType==SqlDbType.DateTime
							)
						{
							commandText=commandText.Replace(commandParameters[i].ParameterName+" ","'"+Convert.ToString(commandParameters[i].Value).Replace("'","''")+"'"+" ");
							commandText=commandText.Replace(commandParameters[i].ParameterName+",","'"+Convert.ToString(commandParameters[i].Value).Replace("'","''")+"'"+",");
							commandText=commandText.Replace(commandParameters[i].ParameterName+")","'"+Convert.ToString(commandParameters[i].Value).Replace("'","''")+"'"+")");
						}
						else
						{
							commandText=commandText.Replace(commandParameters[i].ParameterName+" ",Convert.ToString(commandParameters[i].Value)+" ");
							commandText=commandText.Replace(commandParameters[i].ParameterName+",",Convert.ToString(commandParameters[i].Value)+",");
							commandText=commandText.Replace(commandParameters[i].ParameterName+")",Convert.ToString(commandParameters[i].Value)+")");
						}
					}
				}
				strSQLScript=commandText;
			}
			if(strUseDataBaseName!=String.Empty&&strSQLScript!=String.Empty)
			{
				strSQLScript="USE "+strUseDataBaseName+"\r\n "+strSQLScript;
			}

			return strSQLScript;
		}

	}

	public class SqlHelperParameter
	{
		public SqlHelperParameter(string strName,string strType,int intLength)
		{
			this.Name = strName;
			this.Type = strType;
			this.Length = intLength;
		}

		public SqlParameter SqlParameter
		{
			get
			{
				if(this.Name.Equals(string.Empty) || this.Type.Equals(string.Empty) || this.Length == 0)
				{
					return null;
				}
				else
				{
					return new SqlParameter(this.Name,this.TypeSqlDb,this.Length);
				}
			}
		}

		private string m_Name = string.Empty;
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		private string m_Type = string.Empty;
		public string Type
		{
			get
			{
				return m_Type.ToLower();
			}
			set
			{
				m_Type = value.ToLower();
			}
		}

		public SqlDbType TypeSqlDb
		{
			get
			{
				switch(m_Type)
				{
					case "image":
						return SqlDbType.Image;
					case "text":
						return SqlDbType.Text;
					case "uniqueidentifier":
						return SqlDbType.UniqueIdentifier;
					case "date":
						return SqlDbType.Date;
					case "time":
						return SqlDbType.Time;
					case "datetime2":
						return SqlDbType.DateTime2;
					case "datetimeoffset":
						return SqlDbType.DateTimeOffset;
					case "tinyint":
						return SqlDbType.TinyInt;
					case "smallint":
						return SqlDbType.SmallInt;
					case "int":
						return SqlDbType.Int;
					case "smalldatetime":
						return SqlDbType.SmallDateTime;
					case "real":
						return SqlDbType.Real;
					case "money":
						return SqlDbType.Money;
					case "datetime":
						return SqlDbType.DateTime;
					case "float":
						return SqlDbType.Float;
					case "sql_variant":
						return SqlDbType.Variant;
					case "ntext":
						return SqlDbType.NText;
					case "bit":
						return SqlDbType.Bit;
					case "decimal":
						return SqlDbType.Decimal;
					//case "numeric":return SqlDbType.numeric;
					case "smallmoney":
						return SqlDbType.SmallMoney;
					case "bigint":
						return SqlDbType.BigInt;
					//case "hierarchyid":return SqlDbType.hierarchyid;
					//case "geometry":return SqlDbType.geometry;
					//case "geography":return SqlDbType.geography;
					case "varbinary":
						return SqlDbType.VarBinary;
					case "varchar":
						return SqlDbType.VarChar;
					case "binary":
						return SqlDbType.Binary;
					case "char":
						return SqlDbType.Char;
					case "timestamp":
						return SqlDbType.Timestamp;
					case "nvarchar":
						return SqlDbType.NVarChar;
					case "nchar":
						return SqlDbType.NChar;
					case "xml":
						return SqlDbType.Xml;
					//case "sysname":return SqlDbType.sysName;
					default:
						return SqlDbType.VarChar;
				}
			}
		}

		private int m_Length = 0;
		public int Length
		{
			get
			{
				return m_Length;
			}
			set
			{
				m_Length = value;
			}
		}
	}


}
