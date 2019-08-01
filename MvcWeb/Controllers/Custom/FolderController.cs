using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Utility;
using Models;
using Services;
using System.Web.UI;
using Enums;
using Services.Helper;

namespace MvcWeb.Controllers.Custom
{
	[Description("文件控制器")]
	public class FolderController : Controller
    {
		[Description("查看文件控制器数据页面")]
		public ActionResult Index()
        {
            return View();
        }
		[HttpPost]
		[Description("添加文件夹信息")]
		public string Add()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strFolder = Request["Folder"].ToStr();
			var res = CommonResult.Instance();

			Folder folder = JsonHelper.FromJson<Folder>(strFolder);
			if(folder == null)
			{
				return CommonResult.ToJsonStr("数据为空");
			}

			Folder newFolder = new Folder();
			newFolder.FolderName = folder.FolderName;
			newFolder.FatherID = folder.FatherID;
			newFolder.Creater = userModel.UserName;
			newFolder.Editor = userModel.UserName;
			newFolder.CreateTime = DateTime.Now;
			newFolder.EditTime = DateTime.Now;
			newFolder.Remark = folder.Remark;
			newFolder.State = Convert.ToInt32(ItemState.Enable);
			res = (CommonResult)AllServices.FolderService.Add(newFolder);

			if(res.code < 1)
			{
				return res.ToJson();
			}

			return res.ToJson();
		}

		[HttpPost]
		[Description("查询所有文件夹信息")]
		public string Show()
		{
			var userModel = AllServices.UserService.GetUserBySession();

			CommonResult.Instance();

			List<Folder> listFolder = AllServices.FolderService.Show();

			if(listFolder.Count > 0)
			{
				return CommonResult.Instance(1,null,listFolder).ToJson();
			}
			else
			{
				return CommonResult.Instance(0,"数据为空",null).ToJson();
			}
		}

		[HttpPost]
		[Description("根据ID删除文件夹信息")]
		public string Delete()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			var FID = Request["FID"];
			int[] arr =ArrayHelper.ToIntArray(FID);
			return AllServices.FolderService.Delete(arr).ToJson();
		}
		[HttpPost]
		[Description("根据ID修改文件夹信息")]
		public string Modify()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			var FID = Request["FID"];
			var FName = Request["FName"];

			Folder newFolder = new Folder();
			newFolder.UID = FID.ToInt32();
			newFolder.FolderName = FName;
			newFolder.Creater = userModel.UserName;
			newFolder.Editor = userModel.UserName;
			newFolder.State = Convert.ToInt32(ItemState.Enable);

			return AllServices.FolderService.UpdateFolderName(newFolder).ToJson();
		}
		[HttpPost]
		[Description("根据图标UID查询文件夹信息")]
		public string QueryByDocumentFolderUID()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			int intSysID = Request["UID"].ToInt32();

			List<DocumentFolder> listDocumentFolder = AllServices.DocumentFolderService.GetByDocumentUID(intSysID);
			if(listDocumentFolder.Count > 0)
			{
				List<Folder> listFolder = new List<Folder>();
				foreach(DocumentFolder documentFolder in listDocumentFolder)
				{
					listFolder.Add(AllServices.FolderService.GetByID(documentFolder.FolderUID));
				}
				//拿到所属系统id
				if(listFolder != null)
				{
					return CommonResult.Instance(1,null,listFolder).ToJson();
				}
			}
			return CommonResult.Instance("操作失败，请联系管理员").ToJson();
		}
	}
}