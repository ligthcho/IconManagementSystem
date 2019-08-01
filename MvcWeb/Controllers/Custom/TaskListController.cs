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
	[Description("子任务控制器")]
	public class TaskListController:Controller
	{
		[Description("查看子任务控制器数据页面")]
		public ActionResult Index()
		{
			return View();
		}
		[HttpPost]
		[Description("批量新增子任务信息")]
		public string AddList()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strTaskList = Request["TaskList"].ToStr();
			var res = CommonResult.Instance();

			List<TaskList> listTaskList = JsonHelper.FromJsonList<TaskList>(strTaskList);
			if(listTaskList == null)
			{
				return CommonResult.ToJsonStr("数据为空");
			}
			foreach(TaskList tasklistModel in listTaskList)
			{
				TaskList tasklist = new TaskList();
				tasklist.TaskListNo = ServiceHelper.GetRandom();
				tasklist.TaskListName = tasklistModel.TaskListName;
				tasklist.TaskUID = tasklistModel.TaskUID;
				tasklist.DocumentUID = 0; //tasklistModel.DocumentUID;
				tasklist.PictureSize = tasklistModel.PictureSize;
				tasklist.PictureResolution = tasklistModel.PictureResolution;
				tasklist.PictureBackground =tasklistModel.PictureBackground;
				tasklist.DocumentType = tasklistModel.DocumentType;
				tasklist.Creater = userModel.UserName;
				tasklist.Editor = userModel.UserName;
				tasklist.CreateTime = DateTime.Now;
				tasklist.EditTime = DateTime.Now;
				tasklist.Remark = tasklistModel.Remark;
				tasklist.State = Convert.ToInt32(ItemState.Enable);
				tasklist.ProjectId = 0;//Convert.ToInt32(ItemState.Enable);
				res = (CommonResult)AllServices.TaskListService.Add(tasklist);
				if(res.code < 1)
				{
					return res.ToJson();
				}
			}
			return res.ToJson();
		}
		[HttpPost]
		[Description("更新任务信息")]
		public string Update()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strTaskList = Request["TaskList"].ToStr();
			int intUID = Request["UID"].ToInt();
			List<TaskList> listTaskList = JsonHelper.FromJsonList<TaskList>(strTaskList);
			foreach(TaskList tasklist in listTaskList)
			{
				tasklist.Editor = userModel.UserName;
				tasklist.EditTime = DateTime.Now;
				if(tasklist.UID == 0)
				{
					AllServices.TaskListService.Add(tasklist).ToJson();
				}
				else
				{
					AllServices.TaskListService.Update(tasklist).ToJson();
				}
			}
			return CommonResult.Instance().ToJson();
		}

		[HttpPost]
		[Description("分页展示任务信息")]
		public string Show()
		{
			int intPageIndex = Request["PageIndex"].ToInt32();
			int intPageSize = Request["PageSize"].ToInt32();
			string strField = Request["Field"].ToStr();
			string strFieldValue = Request["FieldValue"].ToStr();
			var listFilter = new List<Utility.Filter>();

			//动态查询表达式
			listFilter.Add(Utility.Filter.Add(strField,Op.Equals,strFieldValue,true));
			var expTaskList = LambdaExpressionBuilder.GetExpressionByAndAlso<TaskList>(listFilter);
			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add("UID","desc");
			//分页获取数据
			Page<TaskList> listTaskList = AllServices.TaskListService.GetByPage(intPageIndex,intPageSize,expTaskList,dicOrderBy);

			return listTaskList.ToJson();
		}


		[HttpPost]
		[Description("分页展示未完成任务信息")]
		public string UnfinishedTaskList()
		{
			int intPageIndex = Request["PageIndex"].ToInt32();
			int intPageSize = Request["PageSize"].ToInt32();
			string strField = Request["Field"].ToStr();
			string strFieldValue = Request["FieldValue"].ToStr();
			int intState = Request["State"].ToInt32();
			var listFilter = new List<Utility.Filter>();
			//动态查询表达式
			listFilter.Add(Utility.Filter.Add(strField,Op.Equals,strFieldValue,true));
			listFilter.Add(Utility.Filter.Add("DocumentUID",Op.Equals,"0",true));
			if(intState <= 10)
			{
				listFilter.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Enable),true));
			}
			else
			{
				listFilter.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Disable),true));
			}
			var expTaskList = LambdaExpressionBuilder.GetExpressionByAndAlso<TaskList>(listFilter);
			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add("UID","desc");
			//分页全部获取数据
			Page<TaskList> listTaskList = AllServices.TaskListService.GetByPage(intPageIndex,intPageSize,expTaskList,dicOrderBy);
			//AllServices.TaskService.GetByPage(1,500,null,null)
			return listTaskList.ToJson();
		}

		[HttpPost]
		[Description("分页展示已完成任务信息")]
		public string FinishedTaskList()
		{
			int intPageIndex = Request["PageIndex"].ToInt32();
			int intPageSize = Request["PageSize"].ToInt32();
			string strField = Request["Field"].ToStr();
			int intFieldValue = Request["FieldValue"].ToInt32();
			string strOrder = Request["Order"].ToStr() == string.Empty ? "asc" : Request["Order"].ToStr();
			string strCond = Request["Cond"].ToStr() == string.Empty ? "TaskListName" : Request["Cond"].ToStr();//默认排序 姓名 a-z递增

			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add(strCond,strOrder);
			//分页全部获取数据
			Page<DataModel> listTaskList = AllServices.TaskListService.GetFinishedByPage(intPageIndex,intPageSize,intFieldValue,dicOrderBy);

			// AllServices.TaskService.GetByPage(1,500,null,null)
			return listTaskList.ToJson();
		}

		/// <summary>
		/// 修改子任务为退返状态（修改子任务状态）
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public string ReturnTaskList()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strUIDs = Request["UIDs"].ToString();
			string strRemark = Request["Remark"].ToString();
			
			int[] intUIDs = ArrayHelper.ToIntArray(strUIDs);
			//关联表状态改退返
			return AllServices.TaskListService.Return(intUIDs,strRemark).ToJson();
		}
	}
}