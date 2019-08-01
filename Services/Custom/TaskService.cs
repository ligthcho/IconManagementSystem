using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using Utility;
using Enums;
using Foundation.SiteConfig;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using System.Linq.Dynamic;
using Services.Helper;

namespace Services
{
	public class TaskService
	{
		public readonly UIDBEntities db = new Models.UIDBEntities();
		/// <summary>
		/// 根据TaskNo获取任务信息
		/// </summary>
		/// <param name="strTaskID"></param>
		/// <returns></returns>
		public Models.Task GetByID(int intTaskUID)
		{
			return db.Task.Where(task => task.UID == intTaskUID).FirstOrDefault();
		}
		/// <summary>
		/// 加载分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Models.Task> GetByPage(int intPageIndex,int intPageSize,Expression<Func<Models.Task,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null)
		{
			if(whereLambda == null)
			{
				whereLambda = u => 1 == 1;
			}
			var q = db.Task.Where(whereLambda).OrderBy(dicOrderBy);
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Models.Task>(intPageIndex,intPageSize,q.Count(),list);
		}

		/// <summary>
		/// 加载未完成主任务分页view数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Temp> GetUnfinishedByPage(int intPageIndex,int intPageSize,Expression<Func<Models.Task,bool>> whereLambda = null)
		{
			List<Temp> listTemp = db.Database.SqlQuery<Temp>("exec GetUnfinishTaskPer").ToList();
			return new Page<Temp>(intPageIndex,intPageSize,listTemp.Count(),listTemp);
		}
		/// <summary>
		/// 加载已完成主任务分页view数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Temp> GetFinishedByPage(int intPageIndex,int intPageSize)
		{
			//List<Temp> listTemp = db.Database.SqlQuery<Temp>("exec GetFinishTaskPer").ToList();
			var listTemp = db.Database.SqlQuery<Temp>("exec GetFinishTaskPer");
			List<Temp> list = listTemp.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Temp>(intPageIndex,intPageSize,listTemp.Count(),list);
		}
		/// <summary>
		/// 新增任务
		/// </summary>
		/// <param name="taskModel"></param>
		/// <returns></returns>
		public CommonResult Add(Models.Task taskModel)
		{
			if(IsRepeatTaskName(taskModel.UID,taskModel.TaskName))
			{
				return CommonResult.Instance("已存在此任务名，请换一个再试");
			}

			taskModel.TaskNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('Task')) ").FirstOrDefault();
			taskModel.TaskName = taskModel.TaskName;
			taskModel.Creater = taskModel.Creater;
			taskModel.Editor = taskModel.Editor;
			taskModel.CreateTime = DateTime.Now;
			taskModel.EditTime = DateTime.Now;
			taskModel.Remark = taskModel.Remark;
			taskModel.State = Convert.ToInt32(ItemState.Enable);

			db.Task.Add(taskModel);
			db.SaveChanges();
			Log4NetHelper.Info("新增任务",taskModel.ToJson());
			//AllServices.ActionLogService.AddLog("新增文件",folderModel.ToJson(),Enums.ActionCategory.Add);
			return CommonResult.Instance(1,null,taskModel.UID);
		}
		/// <summary>
		/// 更新任务
		/// </summary>
		/// <param name="taskModel"></param>
		/// <returns></returns>
		public CommonResult Update(Models.Task taskModel)
		{
			if(IsRepeatTaskName(taskModel.UID,taskModel.TaskName))
			{
				return CommonResult.Instance("已存在此任务名，请换一个再试");
			}
			int i=db.Task.Where(task => task.UID == taskModel.UID).Update(u => new Models.Task()
			{
				TaskName = taskModel.TaskName,
				Editor = taskModel.Editor,
				EditTime = taskModel.EditTime,
				Remark = taskModel.Remark,
				State = Convert.ToInt32(ItemState.Enable)
			});
			Log4NetHelper.Info("更新任务",taskModel.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 删除多个任务
		/// </summary>
		/// <param name="strTaskIDs"></param>
		/// <returns></returns>
		public CommonResult Delete(int[] intTaskUIDs)
		{
			//删除文件
			foreach(int uid in intTaskUIDs)
			{
				if(!IsReleTaskList(uid))
				{
					return CommonResult.Instance(2,"有未完成的子任务",null);
				}
			}
			db.Task.Where(task => intTaskUIDs.Contains(task.UID)).Delete();
			Log4NetHelper.Info("删除多个任务","ID:"+intTaskUIDs.ToJson());
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return CommonResult.Instance();
		}

		/// <summary>
		/// 修改任务状态
		/// </summary>
		/// <param name="intTaskID"></param>
		/// <returns></returns>
		public CommonResult ModifyStatu(int intTaskID,ItemState itemState)
		{
			var instance = db.Task.Where(task =>task.UID == intTaskID).FirstOrDefault();
			//删除文件
			if(IsReleTaskList(intTaskID))
			{
				return CommonResult.Instance(2,"有未完成的子任务",null);
			}
			if(instance != null)
			{
				instance.State = (int)itemState;
			}
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance(0,"删除失败",null);
			}
			Log4NetHelper.Info("删除单个任务","ID:" + intTaskID);
			return CommonResult.Instance();
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
		}

		/// <summary>
		/// 检查是否已经有重复的任务名
		/// </summary>
		/// <param name="intTaskID"></param>
		/// <param name="strTaskName"></param>
		/// <returns></returns>
		public bool IsRepeatTaskName(int intTaskUID,string strTaskName)
		{
			var instance = db.Task.Where(task => task.TaskName.ToLower() == strTaskName.ToLower()).FirstOrDefault();
			return !(instance == null || instance.UID  == intTaskUID);
		}

		/// <summary>
		/// 检查是否已经有关联的子任务
		/// </summary>
		/// <param name="intTaskID"></param>
		/// <returns></returns>
		public bool IsReleTaskList(int intTaskUID)
		{
			var instance = db.TaskList.Where(task => task.TaskUID == intTaskUID).FirstOrDefault();
			return !(instance == null);
		}
	}

	/// <summary>
	/// 临时表
	/// </summary>
	public class Temp
	{
		public int UID
		{
			get; set;
		}
		public string TaskName
		{
			get; set;
		}
		public string Per
		{
			get; set;
		}
		public int TotalCount
		{
			get; set;
		}
		public DateTime CreateTime
		{
			get; set;
		}
		public int State
		{
			get; set;
		}
	}
}
