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
	[Description("项目控制器")]
	public class ProjectController : Controller
    {	
		[Description("查看项目控制器数据页面")]
		public ActionResult Index()
		{
			return View();
		}
		[HttpPost]
		[Description("新增项目信息")]
		public string Add()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strProject = Request["Project"].ToStr();
			var res = CommonResult.Instance();

			Project project = JsonHelper.FromJson<Project>(strProject);
			if(project == null)
			{
				return CommonResult.ToJsonStr("数据为空");
			}

			Project NewProject = new Project();
			NewProject.ProjectNo = ServiceHelper.GetKeyNum();
			NewProject.ProjectName = project.ProjectName;
			NewProject.Creater = userModel.UserName;
			NewProject.Editor = userModel.UserName;
			NewProject.CreateTime = DateTime.Now;
			NewProject.EditTime = DateTime.Now;
			NewProject.Remark = project.Remark;
			NewProject.State = Convert.ToInt32(ItemState.Enable);
			res = (CommonResult)AllServices.ProjectService.Add(NewProject);

			if(res.code < 1)
			{
				return res.ToJson();
			}

			return res.ToJson();
		}

		[HttpPost]
		[Description("查询所有项目信息")]
		public string Show()
		{
			var userModel = AllServices.UserService.GetUserBySession();

			List<Project> listProject = AllServices.ProjectService.Show();
			if(listProject != null)
			{
				return CommonResult.Instance(1,"新建成功",listProject).ToJson();
			}
			return CommonResult.Instance(0,"新建失败",null).ToJson();			
		}
		[HttpPost]
		[Description("更新项目信息")]
		public string Update()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strProject = Request["Project"].ToStr();
			Project project = JsonHelper.FromJson<Project>(strProject);

			return AllServices.ProjectService.Update(project).ToJson();
		}
	}
}