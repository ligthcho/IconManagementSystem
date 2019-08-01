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
	public class TaskListService
	{
		public readonly UIDBEntities db = new Models.UIDBEntities();
		/// <summary>
		/// 根据TaskListID获取子任务信息
		/// </summary>
		/// <param name="intTaskListID"></param>
		/// <returns></returns>
		public Models.TaskList GetByID(int intUID)
		{
			return db.TaskList.Where(taskList => taskList.UID == intUID).FirstOrDefault();
		}
		/// <summary>
		/// 根据TaskiD获取子任务集合信息
		/// </summary>
		/// <param name="intTaskListID"></param>
		/// <returns></returns>
		public List<TaskList> GetByTaskUID(int intUID)
		{
			return db.TaskList.Where(taskList => taskList.TaskUID == intUID).ToList();
		}
		/// <summary>
		/// 加载分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Models.TaskList> GetByPage(int intPageIndex,int intPageSize,Expression<Func<Models.TaskList,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null)
		{
			if(whereLambda == null)
			{
				whereLambda = u => 1 == 1;
			}
			var q = db.TaskList.Where(whereLambda).OrderBy(dicOrderBy);
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Models.TaskList>(intPageIndex,intPageSize,q.Count(),list);
		}
		/// <summary>
		/// 加载未完成子任务分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Models.TaskList> GetUnfinishedByPage(int intPageIndex,int intPageSize,Expression<Func<Models.TaskList,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null)
		{
			if(whereLambda == null)
			{
				whereLambda = u => 1 == 1;
			}
			var q = db.TaskList.Where(whereLambda).OrderBy(dicOrderBy);
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Models.TaskList>(intPageIndex,intPageSize,q.Count(),list);
		}
		/// <summary>
		/// 加载已完成子任务分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<DataModel> GetFinishedByPage(int intPageIndex,int intPageSize,int intTaskUID,Dictionary<string,string> dicOrderBy = null)
		{
			var q = from u in db.TaskList
					join o in db.Document on u.DocumentUID equals o.UID
					where o.State == 10
					select new
					{
						UID = u.UID,
						TaskListName = u.TaskListName,
						PictureSize = u.PictureSize,
						Remark = u.Remark,
						EditTime = u.EditTime,
						TaskUID = u.TaskUID,
						DocumentPath = o.DocumentPath,
						State = u.State
					};
			q = q.Where(u => u.TaskUID == intTaskUID && u.State == 10).OrderBy(dicOrderBy);
			//var q = db.TaskList.Join(db.Document,d => db.Document.Select(s => s.DocumentName).Contains(d.TaskListName)).Where(whereLambda).OrderBy(dicOrderBy);
			//var q = db.TaskList.Join(db.Document,t=>t.DocumentUID,d=>d.UID,(t,d) => new {t,d}).Where(whereLambda).ToList();//
			var list = q.ToList();

			List<DataModel> listDataModel = new List<DataModel>();
			foreach(var model in list)
			{
				DataModel dataModel = new DataModel();
				dataModel["UID"] = model.UID;
				dataModel["TaskUID"] = model.TaskUID;
				dataModel["TaskListName"] = model.TaskListName;
				dataModel["PictureSize"] = model.PictureSize;
				dataModel["Remark"] = model.Remark;
				dataModel["EditTime"] = model.EditTime;
				dataModel["DocumentPath"] = model.DocumentPath;
				listDataModel.Add(dataModel);
			}
			return new Page<DataModel>(intPageIndex,intPageSize,q.Count(),listDataModel);
		}
		/// <summary>
		/// 新增子任务
		/// </summary>
		/// <param name="tasklistModel"></param>
		/// <returns></returns>
		public CommonResult Add(Models.TaskList tasklistModel)
		{
			//if(IsRepeatTaskListName(tasklistModel.TaskListNo,tasklistModel.TaskListName))
			//{
			//	return CommonResult.Instance("已存在此任务名，请换一个再试");
			//}
			tasklistModel.TaskListNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('TaskList')) ").FirstOrDefault();
			db.TaskList.Add(tasklistModel);
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance(0,"新建失败",tasklistModel.TaskListName);
			}
			Log4NetHelper.Info("新增子任务",tasklistModel.ToJson());
			return CommonResult.Instance();
			//AllServices.ActionLogService.AddLog("新增文件",folderModel.ToJson(),Enums.ActionCategory.Add);
		}

		/// <summary>
		/// 批量新增子任务
		/// </summary>
		/// <param name="tasklistModel"></param>
		/// <returns></returns>
		public CommonResult AddList(List<Models.TaskList> listTaskListModel)
		{
			foreach(TaskList tasklistModel in listTaskListModel)
			{
				tasklistModel.TaskListNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('TaskList'))").FirstOrDefault();
				tasklistModel.TaskListName = tasklistModel.TaskListName;
				tasklistModel.TaskUID = tasklistModel.TaskUID;
				tasklistModel.DocumentUID = 0;//tasklistModel.FolderUID;
				tasklistModel.PictureSize = "1";//string.Empty;//tasklistModel.PictureSize;
				tasklistModel.PictureResolution = "1";//string.Empty;//tasklistModel.PictureResolution;
				tasklistModel.PictureBackground = "1";//string.Empty;//tasklistModel.PictureBackground;
				tasklistModel.DocumentType = "1";//string.Empty;//tasklistModel.DocumentType;
				tasklistModel.Creater = tasklistModel.Creater;
				tasklistModel.Editor = tasklistModel.Editor;
				tasklistModel.CreateTime = tasklistModel.CreateTime;
				tasklistModel.EditTime = tasklistModel.EditTime;
				tasklistModel.Remark = tasklistModel.Remark;
				tasklistModel.State = Convert.ToInt32(ItemState.Enable);
				tasklistModel.ProjectId = 0;//Convert.ToInt32(ItemState.Enable);

				if(IsRepeatTaskListName(tasklistModel.TaskListNo,tasklistModel.TaskListName))
				{
					return CommonResult.Instance(0,"已存在此任务名，请换一个再试",tasklistModel.TaskListName);
				}
				db.TaskList.Add(tasklistModel);
				if(db.SaveChanges() < 0)
				{
					return CommonResult.Instance(0,"新建失败",tasklistModel.TaskListName);
				}
			}
			Log4NetHelper.Info("新增子任务",listTaskListModel.ToJson());
			return CommonResult.Instance();
			//AllServices.ActionLogService.AddLog("新增文件",folderModel.ToJson(),Enums.ActionCategory.Add);
		}

		/// <summary>
		/// 更新任务
		/// </summary>
		/// <param name="tasklistModel"></param>
		/// <returns></returns>
		public CommonResult Update(Models.TaskList tasklistModel)
		{
			if(IsRepeatTaskListName(tasklistModel.TaskListNo,tasklistModel.TaskListName))
			{
				return CommonResult.Instance("已存在此任务名，请换一个再试");
			}
			db.TaskList.Where(taskList => taskList.UID == tasklistModel.UID).Update(u => new Models.TaskList()
			{
				TaskListName = tasklistModel.TaskListName,
				Editor = tasklistModel.Editor,
				EditTime = tasklistModel.EditTime,
				Remark = tasklistModel.Remark,
				State = tasklistModel.State
			});
			Log4NetHelper.Info("更新任务",tasklistModel.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary> 
		/// 更新任务
		/// </summary>
		/// <param name="tasklistModel"></param>
		/// <returns></returns>
		public CommonResult UpdateByDocumentUID(int intDocumentUID,string strDocumentName)
		{
			db.TaskList.Where(taskList => taskList.TaskListName == strDocumentName).Update(tasklist => new TaskList { DocumentUID = intDocumentUID });
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			Log4NetHelper.Info("更新任务","ID:"+intDocumentUID);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 退返
		/// </summary>
		/// <param name="intDocumentUID"></param>
		/// <param name="strDocumentName"></param>
		/// <returns></returns>
		public CommonResult Return(int[] intUIDs,string strRemark)
		{ 
			int i = db.TaskList.Where(taskList => intUIDs.Contains(taskList.UID)).Update(tasklist => new TaskList() { DocumentUID = 0,Remark = strRemark });
			if(i < 0)
			{
				return CommonResult.Instance("退返失败");
			}
			Log4NetHelper.Info("退返任务","IDs:" + intUIDs.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}

		/// <summary>
		/// 删除多个任务
		/// </summary>
		/// <param name="strTaskIDs"></param>
		/// <returns></returns>
		public CommonResult Delete(string[] strTaskIDs)
		{
			//删除文件
			db.TaskList.Where(taskList => strTaskIDs.Contains(taskList.TaskListNo)).Delete();
			Log4NetHelper.Info("删除任务","IDs:" + strTaskIDs.ToJson());

			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return CommonResult.Instance();
		}

		/// <summary>
		/// 检查是否已经有重复的子任务名
		/// </summary>
		/// <param name="strTaskListID"></param>
		/// <param name="strTaskListName"></param>
		/// <returns></returns>
		public bool IsRepeatTaskListName(string strTaskListID,string strTaskListName)
		{
			var instance = db.TaskList.Where(taskList => taskList.TaskListName.ToLower() == strTaskListName.ToLower()).FirstOrDefault();
			return !(instance == null || instance.TaskListNo == strTaskListID);
		}
		/// <summary>
		/// 根据TaskUID查询相关子任务信息
		/// </summary>
		/// <returns></returns>
		public List<TaskList> GetTaskListByTaskNo(int intTaskUID)
		{
			var instance = db.TaskList.Where(taskList => taskList.TaskUID == intTaskUID).ToList();
			return instance;
		}
		
	}

}