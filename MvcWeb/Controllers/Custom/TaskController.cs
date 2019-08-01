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

namespace MvcWeb.Controllers.Custom
{
	[Description("任务控制器")]
	public class TaskController : Controller
    {
		[Description("查看任务控制器数据页面")]
		public ActionResult Index()
        {
            return View();
        }

		[Description("搜索任务信息数据")]
		public string GetListByPage()
		{
			int intPageIndex = Request["PageIndex"].ToInt(1);
			int intPageSize = Request["PageSize"].ToInt(10);
			string strTaskName = Request["TaskName"].ToStr();
			string strSort = Request["Sort"].ToStr(" ");
			string strOrder = Request["Order"].ToStr("desc");

			var listFilter = new List<Utility.Filter>();

			//动态查询表达式
			listFilter.Add(Utility.Filter.Add("TaskName",Op.Contains,strTaskName,true));
			var exp = LambdaExpressionBuilder.GetExpressionByAndAlso<Task>(listFilter);

			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add(strSort,strOrder);

			//分页获取数据
			Page<Models.Task> listTaskModel = AllServices.TaskService.GetByPage(intPageIndex,intPageSize,exp,dicOrderBy);

			return listTaskModel.ToJson();
		}
		[HttpPost]
		[Description("新增主任务信息")]
		public string Add()
		{
			string strTaskName = Request["TaskName"].ToStr();
			string strRemark = Request["Remark"].ToStr();

			if(strTaskName.IsNullOrEmpty())
			{
				return CommonResult.ToJsonStr("不能为空");
			}
			var res = AllServices.TaskService.Add(new Task()
			{
				TaskName = strTaskName,
				Remark = strRemark,
				Creater = string.Empty,
				Editor = string.Empty,

			});
			return res.ToJson();
		}
		[HttpPost]
		[Description("删除主任务信息")]
		public string Delete()
		{//20 停用
			int intUID = Request["UID"].ToInt32();

			if(intUID == 0)
			{
				return CommonResult.ToJsonStr("不能为空");
			}
			var res = AllServices.TaskService.ModifyStatu(intUID,Enums.ItemState.Disable);
			return res.ToJson();
		}
		[HttpPost]
		[Description("主任务展示任务信息")]
		public string Show()
		{
			int intPageIndex = Request["PageIndex"].ToInt32();
			int intPageSize = Request["PageSize"].ToInt32();
			var listFilter = new List<Utility.Filter>();
			//动态查询表达式
			listFilter.Add(Utility.Filter.Add(string.Empty,Op.Contains,string.Empty,true));
			var expTask = LambdaExpressionBuilder.GetExpressionByAndAlso<Task>(listFilter);
			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add("UID","desc");
			//分页获取数据
			Page<Task> listTask = AllServices.TaskService.GetByPage(intPageIndex,intPageSize,expTask,dicOrderBy);

			return CommonResult.Instance(1,null,listTask).ToJson();
		}
		[HttpPost]
		[Description("主任务展示任务信息(未完成)")]
		public string ShowUnfinished()
		{
			int intPageIndex = Request["PageIndex"].ToInt32();
			int intPageSize = Request["PageSize"].ToInt32();
			int intState = Request["State"].ToInt32();
			var listFilter = new List<Utility.Filter>();
			//动态查询表达式
			if(intState <= 10 && intState >= 0)
			{
				listFilter.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Enable),true));
			}
			else
			{
				listFilter.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Disable),true));
			}
			//动态查询表达式
			listFilter.Add(Utility.Filter.Add(string.Empty,Op.Contains,string.Empty,true));
			var expTask = LambdaExpressionBuilder.GetExpressionByAndAlso<Task>(listFilter);
			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add("UID","desc");
			//分页获取数据
			Page<Temp> listTaskTemp = AllServices.TaskService.GetUnfinishedByPage(intPageIndex,intPageSize);

			return CommonResult.Instance(1,null,listTaskTemp).ToJson();
		}
		[HttpPost]
		[Description("主任务展示任务信息(已完成)")]
		public string ShowFinished()
		{
			int intPageIndex = Request["PageIndex"].ToInt32();
			int intPageSize = Request["PageSize"].ToInt32();
			int intState = Request["State"].ToInt();
			var listFilter = new List<Utility.Filter>();
			//动态查询表达式
			
			if(intState <= 10 && intState >= 0)
			{
				listFilter.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Enable),true));
			}
			else
			{
				listFilter.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Disable),true));
			}

			//动态查询表达式
			listFilter.Add(Utility.Filter.Add(string.Empty,Op.Contains,string.Empty,true));
			var expTask = LambdaExpressionBuilder.GetExpressionByAndAlso<Task>(listFilter);
			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add("UID","desc");
			//分页获取数据
			Page<Temp> listTaskTemp = AllServices.TaskService.GetFinishedByPage(intPageIndex,intPageSize);
			return CommonResult.Instance(1,null,listTaskTemp).ToJson();
		}
		[HttpGet]
		[Description("主任务详细信息")]
		public string GetTask()
		{
			int intUID = Request["UID"].ToInt32();
			var listFilter = new List<Utility.Filter>();
			//动态查询表达式
			listFilter.Add(Utility.Filter.Add(string.Empty,Op.Contains,string.Empty,true));
			var expTask = LambdaExpressionBuilder.GetExpressionByAndAlso<Task>(listFilter);
			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add("UID","desc");
			//分页获取数据
			Task task = AllServices.TaskService.GetByID(intUID);

			return CommonResult.Instance(1,null,task).ToJson();
		}
		public ActionResult PublishingTask()
		{
			return View("PublishingTasks");
		}
		public ActionResult ManagementTask()
		{
			return View("ManagementTasks");
		}
		[HttpGet]
		[Description("打开主任务修改页")]
		public ActionResult ModifyTasks()
		{
			int intUID = Request["UID"].ToInt32();

			//分页获取数据
			Task task = AllServices.TaskService.GetByID(intUID);

			if(task != null)
			{
				List<TaskList> listTaskList = AllServices.TaskListService.GetByTaskUID(task.UID);
				DataModel dataModel = new DataModel();
				dataModel.Add("Task",task);
				dataModel.Add("ListTaskList",listTaskList);
				ViewBag.CommonResult = CommonResult.Instance(1,null,dataModel).ToJson();
			}
			else
			{
				ViewBag.CommonResult = CommonResult.Instance().ToJson();
			}
			return View("ModifyTasks");
		}
		[HttpPost]
		[Description("修改主任务信息")]
		public string Update()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			int intUID = Request["UID"].ToInt();
			string strTaskName = Request["TaskName"].ToStr();
			string strRemark = Request["Remark"].ToStr();

			if(strTaskName.IsNullOrEmpty())
			{
				return CommonResult.ToJsonStr("不能为空");
			}
			var res = AllServices.TaskService.Update(new Task()
			{
				UID = intUID,
				TaskName = strTaskName,
				Remark = strRemark,
				Editor = userModel.UserName,
				EditTime = DateTime.Now
			});
			return res.ToJson();
		}
	}
}