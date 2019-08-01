using Foundation.SiteConfig;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace MvcWeb.Controllers
{
	[Description("首页，登录、登出等")]
	public class HomeController : Controller
    {
		// GET: Home
		[AllowAnonymous]
		public ActionResult Index()
        {
            return Default();
        }
		[Description("前台默认页")]
		[AllowAnonymous]
		public ActionResult Default()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			Session["User"] = null;
			if(userModel != null)
			{
				//ViewBag.LoginName = userModel.UserName;
				Session["User"] = userModel;
			}
			//ViewBag.LoginName = userModel.UserName;
			//ViewBag.DisableModifyPwd = userModel.UserName == AllConfigServices.SettingsConfig.SuperAdminAccount;

			//List<Cat.Models.Sys_Menu_Permission> userMenuPermissionList;
			//List<Cat.Models.Sys_Action_Permission> userActionPermissionList;
			//if(userModel.UserName.ToLower() == AllConfigServices.SettingsConfig.SuperAdminAccount &&
			//	userModel.Password == AllConfigServices.SettingsConfig.SuperAdminPassword)
			//{
			//系统预留的超级管理员拥有全部权限
			//userMenuPermissionList = AllServices.SysMenuPermissionService.GetAllByCache();
			//userActionPermissionList = AllServices.SysActionPermissionService.GetAllByCache();
			//}
			//else
			//{
			//userMenuPermissionList = AllServices.SysMenuPermissionService.GetAllByUserId(userInstance.User_Id).ToList();
			//userActionPermissionList = AllServices.SysActionPermissionService.GetAllByUserId(userInstance.User_Id).ToList();
			//}

			//ViewBag.MenuPermissions = userMenuPermissionList.ToJson();
			//ViewBag.ActionPermissions = userActionPermissionList.ToJson();

			//获取上次登录(上次登录，即过滤最新的一次登录)
			//ViewBag.LastLogin = AllServices.SysLoginLogService.GetLastLogin();
			return View();
		}

		[Description("确定登录")]
		//[AuthorizeFilter(IsNoAuthorize = true)]
		[AllowAnonymous]
		public string LoginConfirm()
		{
			string strLoginName = Request["LoginName"].ToStr();
			string strPassword = Request["Password"].ToStr();
			 
			var res = AllServices.UserService.Login(strLoginName,strPassword);

			if(res.code < 0)
			{
				return res.ToJson();
			}
			//新增登录日志
			//AllServices.SysLoginLogService.AddLog();

			//string callbackURL = Request["callbackurl"];
			//if(string.IsNullOrEmpty(callbackURL))
			//{
			//	return CommonResult.ToJsonStr(0,string.Empty,"/Admin/Home/Default");
			//}
			//else
			//{
			//	return CommonResult.ToJsonStr(0,string.Empty,callbackURL);
			//}
			return res.ToJson();
		}

		[Description("注销")]
		//[AuthorizeFilter(IsNoAuthorize = true)]
		public ActionResult LoginOut()
		{
			AllServices.UserService.Logout();
			return RedirectToRoute("Default");
		}
		[Description("任务列表")]
		public ActionResult GetTaskList()
		{
			//return PartialView("/Views/Home/_PartialPage2.cshtml", datastest1);//要绝对路径
			return PartialView("/Views/Shared/TestView.cshtml");//要绝对路径.datastest1--要传到分部视图中的数据.
		}
	}
}