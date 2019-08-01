using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using Logic;
using Model;
using System.Diagnostics;

namespace Base
{
	public sealed class OleLogic
	{
		#region 读取配置
		/*
		<!--┏OLE数据库配置┓-->
		<add key="OLEConnections" value="OLEConnection1" />
		<add key="OLEConnection1" value="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=[ApplicationPath]\\Access.mdb" />
		<!--是否输出OLEScript到Log-->
		<add key="OLEScriptWriteLog" value="true" />
		<!--输出OLEScript到Log的目录-->
		<add key="OLEScriptWriteLogPath" value="/Log/" />
		<!--输出OLEScript到Log的文件扩展名-->
		<add key="OLEScriptWriteLogExtension" value=".OLE.LOG" />
		<!--┗OLE数据库配置┛-->

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
		/// Provider=Microsoft.Jet.OLEDB.4.0;Data Source=[ApplicationPath]+\\+[FileName];
		/// </summary>
		public static string OLE_CONNECTION
		{
			get
			{
				string[] strOLEConnections = ConfigLogic.GetStrings("OLEConnections");
				Random random = new Random();
				string strOLEConnectionKey = strOLEConnections[random.Next(0,strOLEConnections.Length)].ToString();
				return ConfigLogic.GetString(strOLEConnectionKey).Replace("[ApplicationPath]",AppDomain.CurrentDomain.BaseDirectory);
			}
		}

		public static bool OLE_SCRIPT_WRITE_LOG = ConfigLogic.GetBool("OLEScriptWriteLog");
		public static string OLE_SCRIPT_WRITE_LOG_PATH = ConfigLogic.GetString("OLEScriptWriteLogPath");
		public static string OLE_SCRIPT_WRITE_LOG_EXTENSION = ConfigLogic.GetString("OLEScriptWriteLogExtension");
		public static bool ERROR_WRITE_LOG = ConfigLogic.GetBool("ErrorWriteLog");
		public static string ERROR_WRITE_LOG_PATH = ConfigLogic.GetString("ErrorWriteLogPath");
		public static string ERROR_WRITE_LOG_EXTENSION = ConfigLogic.GetString("ErrorWriteLogExtension");
		#endregion 读取配置

		#region private utility methods & constructors
		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new OleDbHelper()".
		private OleLogic()
		{
		}

		/// <summary>
		/// This method is used to attach array of OleDbParameters to a OleDbCommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">an array of OleDbParameters tho be added to command</param>
		private static void AttachParameters(OleDbCommand command,OleDbParameter[] commandParameters)
		{
			foreach(OleDbParameter p in commandParameters)
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
		/// This method assigns an array of values to an array of OleDbParameters.
		/// </summary>
		/// <param name="commandParameters">array of OleDbParameters to be assigned values</param>
		/// <param name="parameterValues">array of objects holding the values to be assigned</param>
		private static void AssignParameterValues(OleDbParameter[] commandParameters,object[] parameterValues)
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

			//iterate through the OleDbParameters, assigning the values from the corresponding position in the 
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
		/// <param name="command">the OleDbCommand to be prepared</param>
		/// <param name="connection">a valid OleDbConnection, on which to execute this command</param>
		/// <param name="transaction">a valid OleDbTransaction, or 'null'</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParameters to be associated with the command or 'null' if no parameters are required</param>
		private static void PrepareCommand(OleDbCommand command,OleDbConnection connection,OleDbTransaction transaction,CommandType commandType,string commandText,OleDbParameter[] commandParameters)
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

			//set the command text (stored procedure name or OleDb statement)
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
		/// Execute a OleDbCommand (that returns no resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteNonQuery(connectionString,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns no resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			//create & open a OleDbConnection, and dispose of it after we are done.
			using(OleDbConnection cn=new OleDbConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteNonQuery(cn,commandType,commandText,commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns no resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="spName">the name of the stored prcedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteNonQuery(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteNonQuery(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns no resultset and takes no parameters) against the provided OleDbConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(OleDbConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteNonQuery(connection,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns no resultset) against the specified OleDbConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(OleDbConnection connection,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			OleDbCommand cmd=new OleDbCommand();
			PrepareCommand(cmd,connection,(OleDbTransaction)null,commandType,commandText,commandParameters);

			int retval = 0;
			try
			{
				//finally, execute the command.
				retval = cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the OleDbParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(OLE_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns no resultset) against the specified OleDbConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(OleDbConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteNonQuery(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteNonQuery(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns no resultset and takes no parameters) against the provided OleDbTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(OleDbTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteNonQuery(transaction,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns no resultset) against the specified OleDbTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(OleDbTransaction transaction,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			OleDbCommand cmd=new OleDbCommand();
			PrepareCommand(cmd,transaction.Connection,transaction,commandType,commandText,commandParameters);

			int retval = 0;
			try
			{
				//finally, execute the command.
				retval = cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the OleDbParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(OLE_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns no resultset) against the specified 
		/// OleDbTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(OleDbTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
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
		/// Execute a OleDbCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteDataset(connectionString,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			//create & open a OleDbConnection, and dispose of it after we are done.
			using(OleDbConnection cn=new OleDbConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteDataset(cn,commandType,commandText,commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteDataset(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteDataset(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(OleDbConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteDataset(connection,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset) against the specified OleDbConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(OleDbConnection connection,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			OleDbCommand cmd=new OleDbCommand();
			PrepareCommand(cmd,connection,(OleDbTransaction)null,commandType,commandText,commandParameters);

			//create the DataAdapter & DataSet
			OleDbDataAdapter da=new OleDbDataAdapter(cmd);
			DataSet ds=new DataSet();
			try
			{
				//fill the DataSet using default values for DataTable names, etc.
				da.Fill(ds);
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the OleDbParameters from the command object, so they can be used again.			
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(OLE_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			//return the dataset
			return ds;
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a resultset) against the specified OleDbConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(OleDbConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteDataset(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteDataset(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(OleDbTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteDataset(transaction,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset) against the specified OleDbTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(OleDbTransaction transaction,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			OleDbCommand cmd=new OleDbCommand();
			PrepareCommand(cmd,transaction.Connection,transaction,commandType,commandText,commandParameters);

			//create the DataAdapter & DataSet
			OleDbDataAdapter da=new OleDbDataAdapter(cmd);
			DataSet ds=new DataSet();
			try
			{
				//fill the DataSet using default values for DataTable names, etc.
				da.Fill(ds);
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the OleDbParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(OLE_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			//return the dataset
			return ds;
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a resultset) against the specified 
		/// OleDbTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(OleDbTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
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
		/// this enum is used to indicate whether the connection was provided by the caller, or created by OleDbHelper, so that
		/// we can set the appropriate CommandBehavior when calling ExecuteReader()
		/// </summary>
		private enum OleDbConnectionOwnership
		{
			/// <summary>Connection is owned and managed by OleDbHelper</summary>
			Internal,
			/// <summary>	</summary>
			External
		}

		/// <summary>
		/// Create and prepare a OleDbCommand, and call ExecuteReader with the appropriate CommandBehavior.
		/// </summary>
		/// <remarks>
		/// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
		/// 
		/// If the caller provided the connection, we want to leave it to them to manage.
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection, on which to execute this command</param>
		/// <param name="transaction">a valid OleDbTransaction, or 'null'</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParameters to be associated with the command or 'null' if no parameters are required</param>
		/// <param name="connectionOwnership">indicates whether the connection parameter was provided by the caller, or created by OleDbHelper</param>
		/// <returns>OleDbDataReader containing the results of the command</returns>
		private static OleDbDataReader ExecuteReader(OleDbConnection connection,OleDbTransaction transaction,CommandType commandType,string commandText,OleDbParameter[] commandParameters,OleDbConnectionOwnership connectionOwnership)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			OleDbCommand cmd=new OleDbCommand();
			PrepareCommand(cmd,connection,transaction,commandType,commandText,commandParameters);

			//create a reader
			OleDbDataReader dr = null;

			try
			{
				// call ExecuteReader with the appropriate CommandBehavior
				if(connectionOwnership == OleDbConnectionOwnership.External)
				{
					dr = cmd.ExecuteReader();
				}
				else
				{
					dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				}
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the OleDbParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(OLE_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return dr;
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteReader(connectionString,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(string connectionString,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			//create & open a OleDbConnection
			OleDbConnection cn=new OleDbConnection(connectionString);
			cn.Open();

			try
			{
				//call the private overload that takes an internally owned connection in place of the connection string
				return ExecuteReader(cn,null,commandType,commandText,commandParameters,OleDbConnectionOwnership.Internal);
			}
			catch
			{
				//if we fail to return the OleDbDatReader, we need to close the connection ourselves
				cn.Close();
				throw;
			}
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteReader(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteReader(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(OleDbConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteReader(connection,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset) against the specified OleDbConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(OleDbConnection connection,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			//pass through the call to the private overload using a null transaction value and an externally owned connection
			return ExecuteReader(connection,(OleDbTransaction)null,commandType,commandText,commandParameters,OleDbConnectionOwnership.External);
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a resultset) against the specified OleDbConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(OleDbConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

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
		/// Execute a OleDbCommand (that returns a resultset and takes no parameters) against the provided OleDbTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(OleDbTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteReader(transaction,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a resultset) against the specified OleDbTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///   OleDbDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(OleDbTransaction transaction,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			//pass through to private overload, indicating that the connection is owned by the caller
			return ExecuteReader(transaction.Connection,transaction,commandType,commandText,commandParameters,OleDbConnectionOwnership.External);
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a resultset) against the specified
		/// OleDbTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  OleDbDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a OleDbDataReader containing the resultset generated by the command</returns>
		public static OleDbDataReader ExecuteReader(OleDbTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

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
		/// Execute a OleDbCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteScalar(connectionString,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a 1x1 resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			//create & open a OleDbConnection, and dispose of it after we are done.
			using(OleDbConnection cn=new OleDbConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteScalar(cn,commandType,commandText,commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a 1x1 resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteScalar(connectionString,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteScalar(connectionString,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a 1x1 resultset and takes no parameters) against the provided OleDbConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(OleDbConnection connection,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteScalar(connection,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a 1x1 resultset) against the specified OleDbConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(OleDbConnection connection,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			OleDbCommand cmd=new OleDbCommand();
			PrepareCommand(cmd,connection,(OleDbTransaction)null,commandType,commandText,commandParameters);

			object retval = new object();
			try
			{
				//execute the command & return the results
				retval = cmd.ExecuteScalar();
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the OleDbParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(OLE_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;

		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a 1x1 resultset) against the specified OleDbConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="connection">a valid OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(OleDbConnection connection,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteScalar(connection,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteScalar(connection,CommandType.StoredProcedure,spName);
			}
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a 1x1 resultset and takes no parameters) against the provided OleDbTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(OleDbTransaction transaction,CommandType commandType,string commandText)
		{
			//pass through the call providing null for the set of OleDbParameters
			return ExecuteScalar(transaction,commandType,commandText,(OleDbParameter[])null);
		}

		/// <summary>
		/// Execute a OleDbCommand (that returns a 1x1 resultset) against the specified OleDbTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new OleDbParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(OleDbTransaction transaction,CommandType commandType,string commandText,params OleDbParameter[] commandParameters)
		{
			#region 开始时间
			Stopwatch stopwatch = Stopwatch.StartNew();
			#endregion 开始时间

			//create a command and prepare it for execution
			OleDbCommand cmd=new OleDbCommand();
			PrepareCommand(cmd,transaction.Connection,transaction,commandType,commandText,commandParameters);

			object retval = new object();
			try
			{
				//execute the command & return the results
				retval=cmd.ExecuteScalar();
			}
			catch(Exception ex)
			{
				string strLog = "/*" + DateTime.Now.ToString("HH:mm:ss.fff") + "\t" + ex.Message.Replace("\r\n",String.Empty) + "*/ " + OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters).Replace("\r\n",String.Empty);
				LogLogic.Write(strLog,ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
			}

			// detach the OleDbParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			#region 结束时间
			stopwatch.Stop();
			#endregion 结束时间

			#region 输出SQLScript
			if(OLE_SCRIPT_WRITE_LOG)
			{
				string strTestSQL=OleParameterCache.FormatSQLScript(String.Empty,commandType,commandText,commandParameters);
				SQLScriptWriteLog(stopwatch.ElapsedMilliseconds,strTestSQL.Replace("\r\n",String.Empty));
			}
			#endregion  输出SQLScript

			return retval;
		}

		/// <summary>
		/// Execute a stored procedure via a OleDbCommand (that returns a 1x1 resultset) against the specified
		/// OleDbTransaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.@  
		///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
		/// </remarks>
		/// <param name="transaction">a valid OleDbTransaction</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(OleDbTransaction transaction,string spName,params object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if((parameterValues!=null)&&(parameterValues.Length>0))
			{
				//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
				OleDbParameter[] commandParameters=OleParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString,spName);

				//assign the provided values to these parameters based on parameter order
				AssignParameterValues(commandParameters,parameterValues);

				//call the overload that takes an array of OleDbParameters
				return ExecuteScalar(transaction,CommandType.StoredProcedure,spName,commandParameters);
			}
			//otherwise we can just call the SP without params
			else
			{
				return ExecuteScalar(transaction,CommandType.StoredProcedure,spName);
			}
		}

		#endregion ExecuteScalar

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
				LogLogic.Write(strLog,OLE_SCRIPT_WRITE_LOG_PATH,OLE_SCRIPT_WRITE_LOG_EXTENSION);
			}
			catch
			{

			}
		}

		/// <summary>
		/// 测试数据库连接
		/// </summary>
		/// <param name="strOleConnection"></param>
		/// <param name="strOleState"></param>
		/// <returns></returns>
		public static bool TestOleConnection(string strOleConnection,ref string strOleState)
		{
			bool isOleServerOpen=false;
			OleDbConnection oleDbConnection=new OleDbConnection(strOleConnection);
			OleDbCommand oleDbCommand=new OleDbCommand();
			try
			{
				oleDbCommand.Connection=oleDbConnection;
				oleDbConnection.Open();
				strOleState=oleDbConnection.State.ToString();
				isOleServerOpen=true;
			}
			catch(Exception ex)
			{
				strOleState=ex.Message;
				isOleServerOpen=false;
			}
			oleDbCommand.Dispose();
			oleDbConnection.Close();
			oleDbConnection.Dispose();
			oleDbConnection=null;
			return isOleServerOpen;
		}
	}

	/// <summary>
	/// OleParameterCache provides functions to leverage a static cache of procedure parameters, and the
	/// ability to discover parameters for stored procedures at run-time.
	/// </summary>
	public sealed class OleParameterCache
	{
		#region private methods, variables, and constructors

		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new OleParameterCache()".
		private OleParameterCache()
		{
		}

		private static Hashtable paramCache=Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// resolve at run time the appropriate set of OleDbParameters for a stored procedure
		/// </summary>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
		private static OleDbParameter[] DiscoverSpParameterSet(string connectionString,string spName,bool includeReturnValueParameter)
		{
			using(OleDbConnection cn=new OleDbConnection(connectionString))
			using(OleDbCommand cmd=new OleDbCommand(spName,cn))
			{
				cn.Open();
				cmd.CommandType=CommandType.StoredProcedure;

				OleDbCommandBuilder.DeriveParameters(cmd);

				if(!includeReturnValueParameter)
				{
					cmd.Parameters.RemoveAt(0);
				}

				OleDbParameter[] discoveredParameters=new OleDbParameter[cmd.Parameters.Count];
				;

				cmd.Parameters.CopyTo(discoveredParameters,0);

				return discoveredParameters;
			}
		}

		//deep copy of cached OleDbParameter array
		private static OleDbParameter[] CloneParameters(OleDbParameter[] originalParameters)
		{
			OleDbParameter[] clonedParameters=new OleDbParameter[originalParameters.Length];

			for(int i=0,j=originalParameters.Length;i<j;i++)
			{
				clonedParameters[i]=(OleDbParameter)((ICloneable)originalParameters[i]).Clone();
			}

			return clonedParameters;
		}

		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// add parameter array to the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <param name="commandParameters">an array of OleDbParamters to be cached</param>
		public static void CacheParameterSet(string hashKey,params OleDbParameter[] commandParameters)
		{
			//string hashKey = connectionString + "@" + commandText;

			paramCache[hashKey]=commandParameters;
		}

		/// <summary>
		/// retrieve a parameter array from the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="commandText">the stored procedure name or T-OleDb command</param>
		/// <returns>an array of OleDbParamters</returns>
		public static OleDbParameter[] GetCachedParameterSet(string hashKey)//connectionString, string commandText)
		{
			//string hashKey = connectionString + "@" + commandText;

			OleDbParameter[] cachedParameters=(OleDbParameter[])paramCache[hashKey];

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
		/// Retrieves the set of OleDbParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an array of OleDbParameters</returns>
		public static OleDbParameter[] GetSpParameterSet(string connectionString,string spName)
		{
			return GetSpParameterSet(connectionString,spName,false);
		}

		/// <summary>
		/// Retrieves the set of OleDbParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a OleDbConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
		/// <returns>an array of OleDbParameters</returns>
		public static OleDbParameter[] GetSpParameterSet(string connectionString,string spName,bool includeReturnValueParameter)
		{
			string hashKey=connectionString+"@"+spName+(includeReturnValueParameter?"@include ReturnValue Parameter":String.Empty);

			OleDbParameter[] cachedParameters;

			cachedParameters=(OleDbParameter[])paramCache[hashKey];

			if(cachedParameters==null)
			{
				cachedParameters=(OleDbParameter[])(paramCache[hashKey]=DiscoverSpParameterSet(connectionString,spName,includeReturnValueParameter));
			}

			return CloneParameters(cachedParameters);
		}

		#endregion Parameter Discovery Functions

		/// <summary>
		/// 将传入的SQL参数过滤数据库无法执行的字符
		/// </summary>
		/// <param name="parms">返回传入的SQL参数，已经过滤了数据库无法执行的字符</param>
		/// <returns>是否发生过滤</returns>
		public static bool FilterParameter(ref OleDbParameter[] parms)
		{
			int intFilter=0;
			for(int i=0;i<parms.Length;i++)
			{
				if(parms[i].OleDbType==OleDbType.VarChar||parms[i].OleDbType==OleDbType.Char)
				{
					parms[i].Value=Convert.ToString(parms[i].Value).Replace("'","’");
					intFilter++;
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
		/// <param name="parms">SQL参数</param>
		/// <returns>返回可以在查询分析器中执行字符串</returns>
		public static string FormatSQLScript(string strUseDataBaseName,CommandType commandType,string commandText,OleDbParameter[] parms)
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
								parms[i].OleDbType==OleDbType.Char||
								parms[i].OleDbType==OleDbType.VarChar||
								parms[i].OleDbType==OleDbType.VarWChar||
								parms[i].OleDbType==OleDbType.LongVarChar||
								parms[i].OleDbType==OleDbType.LongVarWChar||
								parms[i].OleDbType==OleDbType.Date
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
								parms[i].OleDbType==OleDbType.Char||
								parms[i].OleDbType==OleDbType.VarChar||
								parms[i].OleDbType==OleDbType.VarWChar||
								parms[i].OleDbType==OleDbType.LongVarChar||
								parms[i].OleDbType==OleDbType.LongVarWChar||
								parms[i].OleDbType==OleDbType.Date
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


//访问类型名称	数据库数据类型	OLE DB 类型	.NET 框架类型	成员名
//文本	VarWChar	DBTYPE_WSTR	System.String	OleDbType.VarWChar
//备注	LongVarWCha r	DBTYPE_WSTR	System.String	OleDbType.LongVarWChar
//数量： 字节	UnsignedTinyInt	dbtype_ui1	System.Byte	OleDbType.UnsignedTinyInt
//是/否	布尔	DBTYPE_BOOL	System.Boolean	OleDbType.Boolean
//日期/时间	日期时间	DBTYPE_DATE	System.DateTime	OleDbType.Date
//货币	十进制	DBTYPE_NUMERIC	System.Decimal	OleDbType.Numeric
//数量： 十进制	十进制	DBTYPE_NUMERIC	System.Decimal	OleDbType.Numeric
//数量： 双	双精度	dbtype_r8	System.Double	OleDbType.Double
//自动编号 （同步复制 ID）	GUID	DBTYPE_GUID	System.Guid	OleDbType.Guid
//号码： (同步复制 ID）	GUID	DBTYPE_GUID	System.Guid	OleDbType.Guid
//自动编号 （长整型）	整数	dbtype_i4	System.Int32	OleDbType.Integer
//号码： (长整型）	整数	dbtype_i4	System.Int32	OleDbType.Integer
//OLE 对象	LongVarBinary	DBTYPE_BYTES	System.Byte 的数组	OleDbType.LongVarBinary
//数量： 单	单个	dbtype_r4	System.Single	OleDbType.Single
//编号: 整型	SmallInt	dbtype_i2	System.Int16	OleDbType.SmallInt
//二进制	VarBinary *	DBTYPE_BYTES	System.Byte 的数组	OleDbType.Binary
//超链接	VarWChar	DBTYPE_WSTR	System.String	OleDbType.VarWChar