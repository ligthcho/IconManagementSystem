using Foundation.SiteConfig;
using Foundation.SiteConfig.Models;
using Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Models;
using Enums;

namespace Services
{
	public class ImageUploadService
	{
		/// <summary>
		/// 上传图片处理
		/// </summary>
		/// <param name="file"></param>
		/// <param name="imageUploadConfig">图片上传配置基类或其派生类</param>
		/// <returns></returns>
		public CommonResult ImageHandler(HttpPostedFileBase postFile,ImageUploadBaseConfig imageUploadConfig,string strUserName,DataModel dataModel,ActionType actionType)
		{
			try
			{
				#region 验证并保存原图
				//验证目标目录是否存在
				if(!Directory.Exists(PathHelper.GetMapPath(imageUploadConfig.SavePath)))
				{
					return CommonResult.Instance("找不到指定的上传文件目录");
				}
				//获取文件扩展名
				string fileExtension = Path.GetExtension(postFile.FileName);
				//检查文件是否为可上传的文件格式
				bool isAllowExtension = imageUploadConfig.AllowExtension.ToStr().Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries).Contains(fileExtension);
				if(!isAllowExtension)
				{
					return CommonResult.Instance("不允许上传的图片类型");
				}
				//上传文件大小限制
				if(postFile.ContentLength > imageUploadConfig.MaxSize * 1024)
				{
					return CommonResult.Instance(string.Format("上传的图片不能大于{0}K",imageUploadConfig.MaxSize));
				}
				//设置图片名称
				var fileName = dataModel["DefinedName"].ToStr();//postFile.FileName;
				//取得扩展名
				string strExt = dataModel["Ext"].ToStr();
				//取得图片名称（去扩展名）												
				string strFileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
				//设置图片存放目录(虚拟路径)
				string strFileSaveVirtualPath = string.Format("{0}/{1}{2}",imageUploadConfig.SavePath,fileName,strExt);
				//设置图片存放目录(物理路径)
				string strFileSavePath = PathHelper.GetMapPath(strFileSaveVirtualPath);

				//是否处理相同路径图片
				this.OldImageHandler(strFileSaveVirtualPath,imageUploadConfig,strUserName);
				//if(FileHelper.Exists(strFileSavePath))
				//{
				//	#region 处理同路径旧图片
				//	//File.Delete(strFileSavePath);
				//	string strNewFileName = FileHelper.ResetFileMinName(strFileSavePath);
				//	if(!FileHelper.RenameFile(strFileSavePath,strNewFileName))//重命名旧文件
				//	{
				//		return CommonResult.Instance("旧文件重命名失败");
				//	}
				//	//更新旧文件的信息
				//	Document oldDocumentModel = new Document();
				//	oldDocumentModel.UID = AllServices.DocumentService.GetUID(strFileSaveVirtualPath);
				//	oldDocumentModel.DocumentName = strNewFileName;
				//	oldDocumentModel.DocumentPath = string.Format("{0}/{1}",imageUploadConfig.SavePath,strNewFileName);
				//	oldDocumentModel.Editor = stringUserName;
				//	oldDocumentModel.EditTime = DateTime.Now;
				//	oldDocumentModel.State = Convert.ToInt32(ItemState.Disable);
				//	AllServices.DocumentService.Update(oldDocumentModel);
				//	#endregion

				//}
				//保存图片
				postFile.SaveAs(strFileSavePath);
				#endregion

				#region 处理图片
				string mode = imageUploadConfig.Mode;
				string strJson = string.Empty;

				if(!mode.Contains("None"))
				{
					//处理后的图片保存目录
					string imgHandlerDir = imageUploadConfig.SavePath + "/" + "m";
					if(!Directory.Exists(PathHelper.GetMapPath(imgHandlerDir)))
					{
						Directory.CreateDirectory(PathHelper.GetMapPath(imgHandlerDir));
					}
					//目标图片名称
					string fileNameByTarget = Path.GetFileName(strFileSavePath);
					//目标图片的虚拟路径
					string fileSaveVirtualPathByTarget = string.Format("{0}/{1}",imgHandlerDir,fileNameByTarget);
					//目标图片的物理路径
					string fileSavePathByTarget = PathHelper.GetMapPath(fileSaveVirtualPathByTarget);
					//处理图片
					ImageHelper.CompressType cType = ImageHelper.CompressType.Zoom;
					switch(mode.ToLower())
					{
						case "cut":
							cType = ImageHelper.CompressType.WidthAndHeightCut;
							break;
						case "zoom":
							cType = ImageHelper.CompressType.Zoom;
							break;
						case "stretch":
							cType = ImageHelper.CompressType.WidthAndHeight;
							break;
					}
					ImageHelper.Thumb(strFileSavePath,fileSavePathByTarget,imageUploadConfig.Width,imageUploadConfig.Height,cType,imageUploadConfig.Quality);
					//添加上传文件记录（处理后的图片）
					AllServices.DocumentService.Add(new Models.Document()
					{
						//DocumentName = fileNameByTarget,
						////File_Extension = fileExtension.ToLower(),
						//DocumentPath = fileSaveVirtualPathByTarget,
						//PictureSize = new System.IO.FileInfo(fileSavePathByTarget).Length.ToStr(),
						////IsSystemCreate = true
					});
					//删除原图
					if(imageUploadConfig.DelOriginalPhoto)
					{
						File.Delete(strFileSavePath);
					}
					else
					{
						//添加上传文件记录（原图）
						AllServices.DocumentService.Add(new Models.Document()
						{
							//DocumentName = fileName,
							//File_Extension = fileExtension.ToLower(),
							//DocumentPath = strFileSaveVirtualPath,
							//PictureSize = postFile.ContentLength.ToStr(),
							//IsSystemCreate = false
						});
					}
					strFileSaveVirtualPath = fileSaveVirtualPathByTarget;
				}
				#endregion

				//释放图片资源
				postFile.InputStream.Close();
				postFile.InputStream.Dispose();

				CommonResult commonResult = this.AddImageHandler(0,fileName,strFileSaveVirtualPath,dataModel["Ext"].ToStr(),dataModel["Size"].ToStr(),dataModel["DpiID"].ToInt32(),actionType,strUserName);
				return commonResult;
			}
			catch(Exception ex)
			{
				return CommonResult.Instance(ex.ToString());
			}
		}


		/// <summary>
		/// 上传图片合并处理
		/// </summary>
		/// <param name="file"></param>
		/// <param name="imageUploadConfig">图片上传配置基类或其派生类</param>
		/// <returns></returns>
		public CommonResult ChunkImageHandler(ImageUploadBaseConfig imageUploadConfig,string strUserName,DataModel dataModel,ActionType actionType)
		{
			try
			{
				//验证目标目录是否存在
				if(!Directory.Exists(PathHelper.GetMapPath(imageUploadConfig.SavePath)))
				{
					return CommonResult.Instance("找不到指定的上传文件目录");
				}
				string strFileName = dataModel["DefinedName"].ToStr();
				string strExt = dataModel["Ext"].ToStr();
				//设置图片存放目录(虚拟路径)
				string strFileSaveVirtualPath = string.Format("{0}/{1}{2}",imageUploadConfig.SavePath,strFileName,strExt);
				//设置图片存放目录(物理路径)
				string strFileSavePath = PathHelper.GetMapPath(strFileSaveVirtualPath);

				//处理相同路径图片
				this.OldImageHandler(strFileSaveVirtualPath,imageUploadConfig,strUserName);

				#region 合并文件
				string strGuid = dataModel["Guid"].ToStr();
				//设置图片存放分片目录(虚拟路径)
				string strFileSaveChunkPath = string.Format("{0}/{1}",imageUploadConfig.SavePath,"chunk\\" + strGuid + "\\");
				string strSourcePath = PathHelper.GetMapPath(strFileSaveChunkPath);//临时文件夹
				string strTargetPath = strFileSavePath;//合并后的文件

				DirectoryInfo dicInfo = new DirectoryInfo(strSourcePath);
				if(Directory.Exists(Path.GetDirectoryName(strSourcePath)))
				{
					FileInfo[] files = dicInfo.GetFiles();
					foreach(FileInfo file in files.OrderBy(f => int.Parse(f.Name)))
					{
						FileStream addFile = new FileStream(strTargetPath,FileMode.Append,FileAccess.Write);
						BinaryWriter AddWriter = new BinaryWriter(addFile);

						//获得上传的分片数据流 
						Stream stream = file.Open(FileMode.Open);
						BinaryReader TempReader = new BinaryReader(stream);
						//将上传的分片追加到临时文件末尾
						AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
						//关闭BinaryReader文件阅读器
						TempReader.Close();
						stream.Close();
						AddWriter.Close();
						addFile.Close();

						TempReader.Dispose();
						stream.Dispose();
						AddWriter.Dispose();
						addFile.Dispose();
					}
					FileHelper.DelectDir(strSourcePath);
					CommonResult commonResult = this.AddImageHandler(0,strFileName,strFileSaveVirtualPath,dataModel["Ext"].ToStr(),dataModel["Size"].ToStr(),dataModel["DpiID"].ToInt32(),actionType,strUserName);
					return CommonResult.Instance();
				}
				return CommonResult.Instance();
			}
			catch(Exception ex)
			{
				return CommonResult.Instance(ex.ToString());
			}
			#endregion
		}
		/// <summary>
		/// 处理旧图片
		/// </summary>
		/// <param name="strFileSavePath">图片虚拟路径</param>
		/// <returns></returns>
		public CommonResult OldImageHandler(string strFileSaveVirtualPath,ImageUploadBaseConfig imageUploadConfig,string strUserName)
		{
			try
			{
				string strFileSavePath = PathHelper.GetMapPath(strFileSaveVirtualPath);
				//是否存在图片文件
				if(FileHelper.Exists(strFileSavePath))
				{
					#region 处理同路径旧图片
					//File.Delete(strFileSavePath);
					string strNewFileName = FileHelper.ResetFileMinName(strFileSavePath);
					if(!FileHelper.RenameFile(strFileSavePath,strNewFileName))//重命名旧文件
					{
						return CommonResult.Instance("旧文件重命名失败");
					}
					//更新旧文件的信息
					Document oldDocumentModel = new Document();
					oldDocumentModel.UID = AllServices.DocumentService.GetUID(strFileSaveVirtualPath);
					oldDocumentModel.DocumentName = strNewFileName;
					oldDocumentModel.DocumentPath = string.Format("{0}/{1}",imageUploadConfig.SavePath,strNewFileName);
					oldDocumentModel.Editor = strUserName;
					oldDocumentModel.EditTime = DateTime.Now;
					oldDocumentModel.State = Convert.ToInt32(ItemState.Disable);

					#endregion
					return AllServices.DocumentService.Update(oldDocumentModel);
				}
				else
				{
					return CommonResult.Instance("处理旧图片失败");
				}
			}
			catch(Exception ex)
			{
				return CommonResult.Instance(ex.ToString());
			}
		}
		/// <summary>
		/// 新增图片处理
		/// </summary>
		/// <param name="intUID"></param>
		/// <param name="strFileNameWithoutExtension"></param>
		/// <param name="strFileSaveVirtualPath"></param>
		/// <param name="strFileExtension"></param>
		/// <param name="strSize"></param>
		/// <param name="intDpiID"></param>
		/// <param name="actionType"></param>
		/// <param name="strUserName"></param>
		/// <returns></returns>
		public CommonResult AddImageHandler(int intUID,string strFileNameWithoutExtension,string strFileSaveVirtualPath,string strFileExtension,string strSize,int intDpiID,ActionType actionType,string strUserName)
		{
			CommonResult commonResult = new CommonResult();
			if(actionType == ActionType.Add)
			{
				//添加上传文件记录（原图）
				commonResult = AllServices.DocumentService.Add(new Models.Document()
				{
					DocumentName = strFileNameWithoutExtension,
					DocumentType = strFileExtension.ToLower(),
					DocumentPath = strFileSaveVirtualPath,
					PictureSize = strSize,
					PictureResolution = intDpiID.ToInt() == 0 ? "96dpi" : "72dpi",
					Creater = strUserName,
					Editor = strUserName,
					CreateTime = DateTime.Now,
					EditTime = DateTime.Now,
					State = Convert.ToInt32(ItemState.Enable)
					//IsSystemCreate = false
				});
			}
			else if(actionType == ActionType.Modify)
			{
				//更新图片记录
				Document documentModel = new Document();
				documentModel.UID = intUID;
				documentModel.DocumentName = strFileNameWithoutExtension;
				documentModel.DocumentPath = strFileSaveVirtualPath;
				documentModel.PictureSize = strSize;
				documentModel.PictureResolution = intDpiID.ToInt() == 0 ? "96dpi" : "72dpi";
				documentModel.Editor = strUserName;
				documentModel.EditTime = DateTime.Now;
				documentModel.State = Convert.ToInt32(ItemState.Disable);
				commonResult = AllServices.DocumentService.Update(documentModel);
			}
			return commonResult;
		}
	}
}
