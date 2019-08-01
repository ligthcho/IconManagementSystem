using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Web.Services.Description; //WS的描述 
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Logic
{
	public class ESBLogic
	{
		#region 读取配置
		/*
		<!--┏错误日志配置┓-->
		<!--是否输出Error到Log-->
		<add key="ErrorWriteLog" value="true" />
		<!--输出Error到Log的目录-->
		<add key="ErrorWriteLogPath" value="/Log/" />
		<!--输出Error到Log的文件扩展名-->
		<add key="ErrorWriteLogExtension" value=".ERR.LOG" />
		<!--┗错误日志配置┛-->
		*/

		public static bool ERROR_WRITE_LOG = ConfigLogic.GetBool("ErrorWriteLog");
		public static string ERROR_WRITE_LOG_PATH = ConfigLogic.GetString("ErrorWriteLogPath");
		public static string ERROR_WRITE_LOG_EXTENSION = ConfigLogic.GetString("ErrorWriteLogExtension");
		#endregion 读取配置

		/// <summary> 
		/// 根据指定的信息，调用远程WebService方法  
		/// </summary>  
		/// <param name="strUrl">WebService的http形式的地址</param>  
		/// <param name="strNameSpace">欲调用的WebService的命名空间</param>  
		/// <param name="strClassName">欲调用的WebService的类名（不包括命名空间前缀）</param>  
		/// <param name="strMethodName">欲调用的WebService的方法名</param>  
		/// <param name="args">参数列表</param>  
		/// <returns>WebService的执行结果</returns>  
		public static object WebServiceInvoke(string strUrl,string strNameSpace,string strClassName,string strMethodName,object[] args)
		{
			try
			{
				//1.使用WebClient 下载WSDL信息
				WebClient webClient=new WebClient();
				Stream stream=webClient.OpenRead(strUrl+"?wsdl");

				//2.创建和格式化WSDL文档
				ServiceDescription serviceDescription=ServiceDescription.Read(stream);

				//3. 创建客户端代理代理类
				ServiceDescriptionImporter serviceDescriptionImporter=new ServiceDescriptionImporter();
				serviceDescriptionImporter.ProtocolName="Soap";//指定访问协议
				serviceDescriptionImporter.Style=ServiceDescriptionImportStyle.Client;//生成客户端代理，默认。
				serviceDescriptionImporter.AddServiceDescription(serviceDescription,string.Empty,string.Empty); //添加WSDL文档。

				//4 .使用 CodeDom 编译客户端代理类。
				CodeNamespace codeNamespce=new CodeNamespace(strNameSpace);
				CodeCompileUnit codeCompileUnit=new CodeCompileUnit();
				codeCompileUnit.Namespaces.Add(codeNamespce);
				serviceDescriptionImporter.Import(codeNamespce,codeCompileUnit);

				CodeDomProvider codeDomProvider=CodeDomProvider.CreateProvider("CSharp");

				//表示用于调用编译器的参数。
				System.CodeDom.Compiler.CompilerParameters compilerParameters=new System.CodeDom.Compiler.CompilerParameters();
				compilerParameters.GenerateExecutable=false;   //设置是否生成可执行文件。
				compilerParameters.GenerateInMemory=true;      //设置是否在内存中生成输出。
				compilerParameters.ReferencedAssemblies.Add("System.dll");   //ReferencedAssemblies  获取当前项目所引用的程序集。
				compilerParameters.ReferencedAssemblies.Add("System.XML.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Data.dll");

				//获取从编译器返回的编译结果。
				System.CodeDom.Compiler.CompilerResults compilerResults=codeDomProvider.CompileAssemblyFromDom(compilerParameters,codeCompileUnit);
				if(true==compilerResults.Errors.HasErrors)
				{
					System.Text.StringBuilder stringBuilder=new System.Text.StringBuilder();
					foreach(System.CodeDom.Compiler.CompilerError compilerError in compilerResults.Errors)
					{
						stringBuilder.Append(compilerError.ToString());
						stringBuilder.Append(System.Environment.NewLine);
					}
					throw new Exception(stringBuilder.ToString());
				}

				//获取已编译的程序集，然后通过反射进行动态调用。
				System.Reflection.Assembly assembly=compilerResults.CompiledAssembly;
				Type type=assembly.GetType(strNameSpace+"."+strClassName,true,true); // 如果在前面为代理类添加了命名空间，此处需要将命名空间添加到类型前面。
				object obj=Activator.CreateInstance(type);
				System.Reflection.MethodInfo methodInfo=type.GetMethod(strMethodName);
				return methodInfo.Invoke(obj,args);
			}
			catch(Exception ex)
			{
				string strMessage=strUrl+"\t"+strNameSpace+"\t"+strClassName+"\t"+strMethodName+"\t"+ex.Message.Replace("\r",string.Empty).Replace("\n",string.Empty);
				LogLogic.Write(strMessage+"\r\n",ERROR_WRITE_LOG_PATH,ERROR_WRITE_LOG_EXTENSION);
				//throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
				return string.Empty;
			}
		}

		/// <summary>
		/// 向http地址Post数据，并取得返回值
		/// </summary>
		/// <param name="strUrl"></param>
		/// <param name="strPostData"></param>
		/// <returns></returns>
		public static string PostHttpInvoke(string strUrl,string strPostData)
		{
			HttpLogic httpLogic = new HttpLogic();
			return httpLogic.PostGetHttpHtml(strUrl,strPostData);
		}
	}
}