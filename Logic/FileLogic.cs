using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Logic
{
    public class FileLogic
    {
        /// <summary>
        /// 写入Text文件
        /// </summary>
        /// <param name="strPathFileName"></param>
        /// <param name="strText"></param>
        public static void WriteText(string strPathFileName, string strText)
        {
            strPathFileName = strPathFileName.Replace("/", "\\");
			string strPath = strPathFileName.Substring(0, strPathFileName.LastIndexOf("\\") + 1);
			string strFileName = strPathFileName.Substring(strPathFileName.LastIndexOf("\\") + 1);
			DirectoryInfo directoryInfo = null;
            if (HttpContext.Current != null)
            {
                directoryInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(strPath));//虚拟目录
            }
            else
            {
                directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + strPath);//应用程序
            }

            if (directoryInfo != null)
            {
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }

			if(File.Exists(directoryInfo.FullName + strFileName))
			{
                File.Delete(directoryInfo.FullName + strFileName);
            }
            StreamWriter streamWriter = File.CreateText(directoryInfo.FullName + strFileName);
            streamWriter.WriteLine(strText);
            streamWriter.Close();
        }

        /// <summary>
        /// 读取Text文件
        /// </summary>
        /// <param name="strPathFileName"></param>
        /// <returns></returns>
        public static string ReadText(string strPathFileName)
        {
            strPathFileName = strPathFileName.Replace("/", "\\");

            DirectoryInfo directoryInfo = null;
            if (HttpContext.Current != null)
            {
                directoryInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(strPathFileName));//虚拟目录
            }
            else
            {
                directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + strPathFileName);//应用程序
            }

            string strText = string.Empty;
            if (File.Exists(directoryInfo.FullName))
            {
                strText = File.ReadAllText(directoryInfo.FullName,Encoding.UTF8);
            }
            return strText;
        }
    }
}
