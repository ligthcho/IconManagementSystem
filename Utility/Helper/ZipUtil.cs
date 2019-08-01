using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace Utility
{
	/// <summary>
	/// zip文件压缩
	/// </summary>
	public class ZipUtil
	{
		/// <summary>
		/// 压缩文件 调用 ZipUtil.CreateZip(@"D:\Temp\forzip", @"D:\Temp2\forzip.zip") 即可。
		/// </summary>
		/// <param name="sourceFilePath"></param>
		/// <param name="destinationZipFilePath"></param>   /有bug/！！！！！！！ 只能压一个文件不知道为什么 ，请用下面那个
		public static bool CreateZip(string sourceFilePath,string destinationZipFilePath)
		{
			try
			{
				if(sourceFilePath[sourceFilePath.Length - 1] != Path.DirectorySeparatorChar)
				{
					sourceFilePath += Path.DirectorySeparatorChar;
				}
				var zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
				zipStream.SetLevel(6); // 压缩级别 0-9
				CreateZipFiles(sourceFilePath,zipStream,sourceFilePath);
				zipStream.Finish();
				zipStream.Close();
				return true;
			}
			catch(Exception ex)
			{
				//LogManager.Log("文件压缩异常：" + ex.Message + ex.StackTrace);
				return false;
			}
		}

		/// <summary>
		/// 递归压缩文件
		/// </summary>
		/// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
		/// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
		/// <param name="staticFile"></param>
		private static void CreateZipFiles(string sourceFilePath,ZipOutputStream zipStream,string staticFile)
		{
			var crc = new Crc32();
			string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
			foreach(string file in filesArray)
			{
				if(Directory.Exists(file)) //如果当前是文件夹，递归
				{
					CreateZipFiles(file,zipStream,staticFile);
				}

				else //如果是文件，开始压缩
				{
					FileStream fileStream = File.OpenRead(file);

					var buffer = new byte[fileStream.Length];
					fileStream.Read(buffer,0,buffer.Length);
					string tempFile = file.Substring(staticFile.LastIndexOf("\\",StringComparison.Ordinal) + 1);
					var entry = new ZipEntry(tempFile) { DateTime = DateTime.Now,Size = fileStream.Length };

					fileStream.Close();
					crc.Reset();
					crc.Update(buffer);
					entry.Crc = crc.Value;
					zipStream.PutNextEntry(entry);

					zipStream.Write(buffer,0,buffer.Length);
				}
			}
		}

		/// <summary>
		/// 压缩文件
		/// </summary>
		/// <param name="filesPath">需要压缩文件夹的绝对路径</param>
		/// <param name="zipFilePath">压缩文件的绝对路径</param>
		public static void CreateZipFile(string filesPath,string zipFilePath)
		{

			if(!Directory.Exists(filesPath))
			{
				return;
			}

			try
			{
				string[] filenames = Directory.GetFiles(filesPath);
				using(ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
				{

					s.SetLevel(9); // 压缩级别 0-9
								   //s.Password = "123"; //Zip压缩文件密码
					byte[] buffer = new byte[4096]; //缓冲区大小
					foreach(string file in filenames)
					{
						ZipEntry entry = new ZipEntry(Path.GetFileName(file));
						entry.DateTime = DateTime.Now;
						s.PutNextEntry(entry);
						using(FileStream fs = File.OpenRead(file))
						{
							int sourceBytes;
							do
							{
								sourceBytes = fs.Read(buffer,0,buffer.Length);
								s.Write(buffer,0,sourceBytes);
							} while(sourceBytes > 0);
						}
					}
					s.Finish();
					s.Close();
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("Exception during processing {0}",ex);
			}
		}

		/// <summary>
		/// Decompression the zip file 
		/// </summary>
		/// <param name="zipFilePath">the source path of the zip file</param>
		/// <param name="savePath">What path you want to save the directory that was decompressioned</param>
		private static void UnZipFile(string zipFilePath,string savePath)
		{
			if(!File.Exists(zipFilePath))
			{
				Console.WriteLine("Cannot find file '{0}'",zipFilePath);
				return;
			}

			using(ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
			{

				ZipEntry theEntry;
				while((theEntry = s.GetNextEntry()) != null)
				{

					string fullPath = JudgeFullPath(savePath) + theEntry.Name;
					Console.WriteLine(fullPath);

					string directoryName = Path.GetDirectoryName(fullPath);
					string fileName = Path.GetFileName(fullPath);

					//if (String.IsNullOrEmpty(savePath))
					//{
					//    directoryName = savePath + "/"+directoryName;
					//}
					// create directory
					if(directoryName.Length > 0)
					{
						Directory.CreateDirectory(directoryName);
					}

					if(fileName != String.Empty)
					{
						using(FileStream streamWriter = File.Create(fullPath))
						{

							int size = 2048;
							byte[] data = new byte[2048];
							while(true)
							{
								size = s.Read(data,0,data.Length);
								if(size > 0)
								{
									streamWriter.Write(data,0,size);
								}
								else
								{
									break;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Judge the last symbol of the full path wether is "/" or not.
		/// </summary>
		/// <param name="_path"></param>
		/// <returns></returns>
		private static string JudgeFullPath(string _path)
		{
			if(!string.IsNullOrEmpty(_path) && _path.Length > 4)
			{
				string lastSymbol = _path.Substring(_path.Length - 1);
				if(lastSymbol == "\\")
				{
					return _path;
				}
				return _path + "\\";
			}
			return _path;
		
	}
}
}