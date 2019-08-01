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
	public class ImageDownloadService
	{
		/// <summary>
		/// 图片下载处理（单个图片直接返回，多个图片打包成zip,成功返回压缩包地址）
		/// </summary>
		/// <param name="postFile"></param>
		/// <param name="imageUploadConfig"></param>
		/// <param name="stringUserName"></param>
		/// <param name="dataModel"></param>
		/// <param name="actionType"></param>
		/// <returns></returns>
		public CommonResult ImageHandler(ImageUploadBaseConfig imageUploadConfig,List<DataModel> listDataModel)
		{
			//验证目标目录是否存在
			if(!Directory.Exists(PathHelper.GetMapPath(imageUploadConfig.SavePath)))
			{
				return CommonResult.Instance("找不到指定的上传文件目录");
			}
			if(!Directory.Exists(PathHelper.GetMapPath(imageUploadConfig.SaveCompressPath)))
			{
				return CommonResult.Instance("找不到指定的存放压缩文件目录");
			}
			if(!Directory.Exists(PathHelper.GetMapPath(imageUploadConfig.SaveTempPath)))
			{
				return CommonResult.Instance("找不到指定的存放打包文件临时目录");
			}


			if(listDataModel.Count() == 1)//单个文件下载，不需要打包
			{
				DataModel dataModel = listDataModel[0];
				string strUrl = dataModel["url"].ToStr();
				if(string.IsNullOrEmpty(strUrl))//设置图片存放目录(物理路径)
				{
					return CommonResult.Instance("下载链接为空，请检查");
				}
				string strSourceFileName = PathHelper.GetMapPath(strUrl);
				return CommonResult.Instance(2,null,strSourceFileName);
			}
			//要压缩的文件夹，把需要打包的文件存放在此文件夹
			string strPath = PathHelper.GetMapPath(imageUploadConfig.SaveTempPath);
			FileHelper.DelectDir(strPath);
			try
			{
				//压缩后的文件存放路径
				string strDestFile = PathHelper.GetMapPath(imageUploadConfig.SaveCompressPath);
				string strName = "icon.zip";
				string strLastDestFile = string.Format("{0}/{1}",strDestFile,FileHelper.ResetFileName(strName));

				foreach(DataModel dataModel in listDataModel)
				{
					string strUrl = dataModel["url"].ToStr();
					string strSourceFileName = PathHelper.GetMapPath(strUrl);
					string strFileName = Path.GetFileName(strSourceFileName);
					string strNewPath = string.Format("{0}/{1}",strPath,strFileName);
					System.IO.File.Copy(strSourceFileName,strNewPath,true);//拷贝文件到压缩文件夹  //true为覆盖同名文件
				}
				ZipUtil.CreateZipFile(strPath,strLastDestFile);
				//单个文件下载，不需要打包
				//foreach(string fileName in list)
				//{
				//	string sourceFileName = System.IO.Path.Combine(filePath,fileName) + ".xls";
				//	string destFileName = System.IO.Path.Combine(dPath,fileName) + ".xls";
				//	System.IO.File.Copy(sourceFileName,destFileName,true);
				//}
				//参数为文件存放路径，下载的文件格式，文件名字
				//return File(destFile,"application/image",System.IO.Path.GetFileName(destFile));

				return CommonResult.Instance(1,null,strLastDestFile);
			}
			catch(Exception ex)
			{
				FileHelper.DelectDir(strPath);
				return CommonResult.Instance(ex.ToString());
			}
		}
	}
}
