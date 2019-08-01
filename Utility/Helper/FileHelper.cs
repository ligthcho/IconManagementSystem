using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility
{
	/// <summary>
	///     辅助类
	/// </summary>
	public class FileHelper
	{
		/// <summary>
		///     生成树形文件Html
		/// </summary>
		/// <param name="builder">用于存放拼接的Html，由于是递归拼接，调用方法时，传入空的StringBuilder即可</param>
		/// <param name="path">要显示的服务器端文件夹路径（物理路径）</param>
		/// <param name="replacePath">要替换掉的路径部分</param>
		/// <returns></returns>
		public static string GetGuideTree(StringBuilder builder,string path,string replacePath)
		{
			var currentDir = new DirectoryInfo(path);
			DirectoryInfo[] subDirs = currentDir.GetDirectories();
			if(subDirs.Length > 0)
			{
				builder.AppendFormat("<li><span class='folder' path='{0}'>{1}</span>" + Environment.NewLine,
									 currentDir.FullName.Replace(replacePath,""),currentDir.Name);
				builder.Append("    <ul>" + Environment.NewLine);
				foreach(DirectoryInfo dir in subDirs)
				{
					GetGuideTree(builder,dir.FullName,replacePath);
				}

				#region 文件夹下文件

				FileInfo[] files = currentDir.GetFiles();
				if(files.Length > 0)
				{
					foreach(FileInfo file in files)
					{
						string previewUrl = file.FullName.IsImage()
												? GetFileWebUrl(
													file.FullName.Replace(HttpContext.Current.Server.MapPath("~/"),""))
												: string.Empty;
						builder.AppendFormat(
							"<li><span class='file' name='{0}' img='{1}' path='{2}'>{0}</span>" + Environment.NewLine,
							file.Name,
							previewUrl,file.FullName.Replace(replacePath,""));
					}
				}

				builder.Append("    </ul>" + Environment.NewLine);

				#endregion

				builder.Append("</li>" + Environment.NewLine);
			}
			else
			{
				builder.AppendFormat(
					"<li  class='closed'><span class='folder' path='{0}'>{1}</span>" + Environment.NewLine,
					currentDir.FullName.Replace(replacePath,""),currentDir.Name);

				#region 文件夹下文件

				FileInfo[] files = currentDir.GetFiles();
				if(files.Length > 0)
				{
					builder.Append("    <ul>" + Environment.NewLine);
					foreach(FileInfo file in files)
					{
						string previewUrl = file.FullName.IsImage()
												? GetFileWebUrl(
													file.FullName.Replace(HttpContext.Current.Server.MapPath("~/"),""))
												: string.Empty;
						builder.AppendFormat(
							"<li><span class='file' name='{0}' img='{1}' path='{2}'>{0}</span>" + Environment.NewLine,
							file.Name,
							previewUrl,file.FullName.Replace(replacePath,""));
					}
					builder.Append("    </ul>" + Environment.NewLine);
				}

				#endregion
			}
			return builder.ToString();
		}

		public static string GetFileWebUrl(string filePath)
		{
			if(filePath.IsEmpty())
			{
				return string.Empty;
			}
			filePath = filePath.Replace("\\","/");
			if(filePath.StartsWith("/"))
			{
				filePath = filePath.TrimStart('/');
			}
			return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath) + filePath;
		}

		/// <summary>
		/// 读取文件
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string ReadFile(string filePath)
		{
			if(File.Exists(filePath))
			{
				var fs = new FileStream(filePath,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
				using(var sr = new StreamReader(fs,Encoding.UTF8))
				{
					return sr.ReadToEnd();
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// 取得文件编码
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Encoding GetFileEncoding(string path)
		{
			FileStream fileStream = File.Open(path,FileMode.Open,FileAccess.ReadWrite);
			var buffer = new byte[fileStream.Length];
			fileStream.Read(buffer,0,buffer.Length);
			fileStream.Close();
			fileStream.Dispose();
			var fileEncoding = GetEncode(buffer);
			return fileEncoding;
		}

		/// <summary>
		/// 自适应编码读取文本
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string GetTxt(string path)
		{
			FileStream fileStream = File.Open(path,FileMode.Open,FileAccess.ReadWrite);
			var buffer = new byte[fileStream.Length];
			fileStream.Read(buffer,0,buffer.Length);
			fileStream.Close();
			fileStream.Dispose();
			return GetTxt(buffer,GetEncode(buffer));
		}

		/// <summary>
		/// 按指定编码方式读取文本
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string GetTxt(byte[] buffer,Encoding encoding)
		{
			if(Equals(encoding,Encoding.UTF8))
				return encoding.GetString(buffer,3,buffer.Length - 3);
			if(Equals(encoding,Encoding.BigEndianUnicode) || Equals(encoding,Encoding.Unicode))
				return encoding.GetString(buffer,2,buffer.Length - 2);
			return encoding.GetString(buffer);
		}

		/// <summary>
		/// 取得文件编码方式
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static Encoding GetEncode(byte[] buffer)
		{
			if(buffer.Length <= 0 || buffer[0] < 239)
				return Encoding.Default;
			if(buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
				return Encoding.UTF8;
			if(buffer[0] == 254 && buffer[1] == byte.MaxValue)
				return Encoding.BigEndianUnicode;
			if(buffer[0] == byte.MaxValue && buffer[1] == 254)
				return Encoding.Unicode;
			return Encoding.Default;
		}

		/// <summary>
		/// 写入文本
		/// </summary>
		/// <param name="filepath">写入文件</param>
		/// <param name="body">写入内容</param>
		/// <param name="encoding">编码方式</param>
		public static void WriteTxt(string filepath,string body,Encoding encoding)
		{
			if(File.Exists(filepath))
				File.Delete(filepath);
			byte[] bytes = encoding.GetBytes(body);
			FileStream fileStream = File.Open(filepath,FileMode.CreateNew,FileAccess.Write);
			if(Equals(encoding,Encoding.UTF8))
			{
				fileStream.WriteByte(239);
				fileStream.WriteByte(187);
				fileStream.WriteByte(191);
			}
			else if(Equals(encoding,Encoding.BigEndianUnicode))
			{
				fileStream.WriteByte(254);
				fileStream.WriteByte(byte.MaxValue);
			}
			else if(Equals(encoding,Encoding.Unicode))
			{
				fileStream.WriteByte(byte.MaxValue);
				fileStream.WriteByte(254);
			}
			fileStream.Write(bytes,0,bytes.Length);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
		}

		/// <summary>
		/// 逐行读取文件
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static List<string> ReadFileForLines(string filePath)
		{
			var lines = new List<string>();
			using(var sr = new StreamReader(filePath,Encoding.UTF8))
			{
				String input;
				while((input = sr.ReadLine()) != null)
				{
					lines.Add(input);
				}
			}
			return lines;
		}

		/// <summary>
		/// 写入文件
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="content"></param>
		public static void WriteFile(string filePath,string content)
		{
			try
			{
				if(File.Exists(filePath))
				{
					File.Delete(filePath);
				}
				using(var stream = new FileStream(filePath,FileMode.OpenOrCreate))
				{
					Encoding encode = Encoding.UTF8;
					//获得字节数组
					byte[] data = encode.GetBytes(content);
					//开始写入
					stream.Write(data,0,data.Length);
					//清空缓冲区、关闭流
					stream.Flush();
					stream.Close();
				}
			}
			catch(Exception ex)
			{
			}
		}

		public static void GetFiles(string dir,List<string> list)
		{
			//添加文件
			list.AddRange(Directory.GetFiles(dir));

			//如果是目录，则递归
			DirectoryInfo[] directories = new DirectoryInfo(dir).GetDirectories();
			foreach(DirectoryInfo item in directories)
			{
				GetFiles(item.FullName,list);
			}
		}

		/// <summary>
		/// 重新设置文件名并返回
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>yyyyMMddHHmmssfff{4位随机数}_{原来的文件名，最长12位}{文件扩展名}</returns>
		public static string ResetFileName(string fileName)
		{
			string f = Path.GetFileNameWithoutExtension(fileName);
			string extension = Path.GetExtension(fileName);
			if(f.Length > 12)
			{
				f = f.Substring(0,12);
			}
			return string.Format("{0}_{1}{2}",DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000,9999),f,extension);
		}
		/// <summary>
		/// 重新设置文件名并返回
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>yyyyMMdd_{原来的文件名，最长12位}{文件扩展名}</returns>
		public static string ResetFileMinName(string fileName)
		{
			string f = Path.GetFileNameWithoutExtension(fileName);
			string extension = Path.GetExtension(fileName);
			if(f.Length > 12)
			{
				f = f.Substring(0,12);
			}
			return string.Format("{0}_{1}{2}",f,DateTime.Now.ToString("yyyyMMddHHmmssfff"),extension);
		}

		/// <summary>
		/// 复制文件（如果目标文件已存在将覆盖）。
		/// </summary>
		/// <param name="sourceFile">源文件。</param>
		/// <param name="destFile">目标文件。</param>
		public static bool CopyFile(string sourceFile,string destFile)
		{
			bool copyResult = false;
			try
			{
				string directory = Path.GetDirectoryName(destFile);
				if(!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				File.Copy(sourceFile,destFile,true);
			}
			catch(Exception)
			{
				copyResult = false;
			}
			return copyResult;
		}

		/// <summary>
		/// 获取指定文件夹下所有子文件夹路径。
		/// </summary>
		/// <param name="sourceDirectory">源文件夹。</param>
		/// <returns></returns>
		public static List<string> GetAllDirectory(string sourceDirectory)
		{
			List<string> directorys = new List<string>();
			if(Directory.Exists(sourceDirectory))
			{
				string[] folders = Directory.GetDirectories(sourceDirectory);
				if(folders.Length > 0)
				{
					foreach(string folder in folders)
					{
						directorys.Add(folder);
						directorys.AddRange(GetAllDirectory(folder));
					}
				}
			}
			return directorys;
		}

		/// <summary>
		/// 将指定文件夹（包括文件夹所有子目录和文件）拷贝到目标文件夹。
		/// </summary>
		/// <param name="sourceDirectory">需要拷贝的文件夹。</param>
		/// <param name="destDirectory">目标文件夹。</param>
		public static void CopyDirectory(string sourceDirectory,string destDirectory)
		{
			// 如果源文件夹存在。
			if(Directory.Exists(sourceDirectory))
			{
				// 如果目标文件夹不存在
				if(!Directory.Exists(destDirectory))
				{
					Directory.CreateDirectory(destDirectory);
				}

				// 拷贝根目录文件
				string[] files = Directory.GetFiles(sourceDirectory);
				foreach(string f in files)
				{
					CopyFile(f,Path.Combine(destDirectory,f.Substring(sourceDirectory.Length + 1)));
				}

				// 子目录拷贝以及文件拷贝
				List<string> directorys = GetAllDirectory(sourceDirectory);
				directorys.ForEach(p =>
				{
					string path = Path.Combine(destDirectory + "\\",p.Substring(sourceDirectory.Length + 1));
					if(!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					string[] folderFiles = Directory.GetFiles(p);
					foreach(string f in folderFiles)
					{
						CopyFile(f,Path.Combine(path,f.Substring(p.Length + 1)));
					}
				});
			}
		}

		/// <summary>
		///     删除文件夹（及文件夹下所有子文件夹和文件）
		/// </summary>
		/// <param name="directoryPath"></param>
		public static void DeleteFolder(string directoryPath)
		{
			try
			{
				foreach(string d in Directory.GetFileSystemEntries(directoryPath))
				{
					if(File.Exists(d))
					{
						var fi = new FileInfo(d);
						if(fi.Attributes.ToString().IndexOf("ReadOnly",StringComparison.Ordinal) != -1)
							fi.Attributes = FileAttributes.Normal;
						File.Delete(d); //删除文件   
					}
					else
						DeleteFolder(d); //删除文件夹
				}
				Directory.Delete(directoryPath); //删除空文件夹
			}
			catch(Exception ex)
			{
			}
		}

		 /// <summary>
         /// 判断文件是否存在
         /// </summary>
         /// <param name="filePath">文件全路径</param>
         /// <returns></returns>
         public static bool Exists(string strFilePath)
         {
             if (strFilePath == null || strFilePath.Trim() == "")
             {
                 return false;
             }
 
             if (File.Exists(strFilePath))
             {
                 return true;
             }
 
             return false;
         }
		/// <summary>
		/// 对文件进行重命名
		/// </summary>
		/// <param name="strFilePath"></param>
		/// <param name="strNewName"></param>
		/// <returns></returns>
		public static bool RenameFile(string strFilePath,string strNewName)
		{
			if(strFilePath == null || strFilePath.Trim() == ""|| strNewName == null || strNewName.Trim() == "")
			{
				return false;
			}
			if(Path.GetFileName(strFilePath)== strNewName)
			{
				return true;
			}
			if(File.Exists(strFilePath))
			{
				try
				{
					Computer computer = new Computer();
					computer.FileSystem.RenameFile(strFilePath,strNewName);
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}
			return false;
		}
		/// <summary>
		/// 去文件扩展名
		/// </summary>
		/// <param name="strFileName"></param>
		/// <returns></returns>
		public static string GetFileNameWithoutExtension(string strFileName)
		{
			return Path.GetFileNameWithoutExtension(strFileName);
		}


		/// <summary>  
		/// 递归(删除文件夹下的所有文件)  
		/// </summary>  
		/// <param name="strPath">目录地址</param>  
		public static void DelectDir(string strPath)
		{
			try
			{
				DirectoryInfo dir = new DirectoryInfo(strPath);
				FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
				foreach(FileSystemInfo i in fileinfo)
				{
					i.Attributes = FileAttributes.Normal;
					if(i is DirectoryInfo)            //判断是否文件夹
					{
						DirectoryInfo subdir = new DirectoryInfo(i.FullName);
						subdir.Delete(true);          //删除子目录和文件
					}
					else
					{
						File.Delete(i.FullName);      //删除指定文件
					}
				}
			}
			catch(Exception e)
			{
				throw;
			}
		}
	}
}