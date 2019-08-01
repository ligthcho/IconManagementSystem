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
	public class ProjectService
	{
		public readonly UIDBEntities db = new Models.UIDBEntities();
		/// <summary>
		/// 根据ProjectNo获取项目信息
		/// </summary>
		/// <param name="strProjectNo"></param>
		/// <returns></returns>
		public Models.Project GetByID(string strProjectNo)
		{
			return db.Project.Where(project => project.ProjectNo == strProjectNo).FirstOrDefault();
		}
		/// <summary>
		/// 加载分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Models.Project> GetByPage(int intPageIndex,int intPageSize,Expression<Func<Models.Project,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null)
		{
			if(whereLambda == null)
			{
				whereLambda = u => 1 == 1;
			}
			var q = db.Project.Where(whereLambda).OrderBy(dicOrderBy);
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Models.Project>(intPageIndex,intPageSize,q.Count(),list);
		}
		/// <summary>
		/// 新增项目
		/// </summary>
		/// <param name="projectModel"></param>
		/// <returns></returns>
		public CommonResult Add(Models.Project projectModel)
		{
			if(IsRepeatProjectName(projectModel.ProjectNo,projectModel.ProjectName))
			{
				return CommonResult.Instance("已存在此任务名，请换一个再试");
			}

			projectModel.ProjectNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('Project')) ").FirstOrDefault();
			projectModel.ProjectName = projectModel.ProjectName;
			projectModel.Creater = projectModel.Creater;
			projectModel.Editor = projectModel.Editor;
			projectModel.CreateTime = projectModel.CreateTime;
			projectModel.EditTime = projectModel.EditTime;
			projectModel.Remark = projectModel.Remark;
			projectModel.State = Convert.ToInt32(ItemState.Enable);

			db.Project.Add(projectModel);
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance(0,"新建失败",projectModel.ProjectName);
			}
			return CommonResult.Instance();
			//AllServices.ActionLogService.AddLog("新增文件",folderModel.ToJson(),Enums.ActionCategory.Add);
		}

		/// <summary>
		/// 更新项目
		/// </summary>
		/// <param name="projectModel"></param>
		/// <returns></returns>
		public CommonResult Update(Models.Project projectModel)
		{
			if(IsRepeatProjectName(projectModel.ProjectNo,projectModel.ProjectName))
			{
				return CommonResult.Instance("已存在此项目名，请换一个再试");
			}
			db.Project.Where(project => project.ProjectNo == projectModel.ProjectNo).Update(u => new Models.Project()
			{
				ProjectName = projectModel.ProjectName,
				Creater = projectModel.Creater,
				CreateTime = projectModel.CreateTime,
				Editor = projectModel.Editor,
				EditTime = DateTime.Now,
				Remark = projectModel.Remark,
				State = projectModel.State
			});

			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 删除多个项目
		/// </summary>
		/// <param name="strProdectIDs"></param>
		/// <returns></returns>
		public CommonResult Delete(string[] strProdectIDs)
		{
			//删除项目
			db.Project.Where(project => strProdectIDs.Contains(project.ProjectNo)).Delete();
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return CommonResult.Instance();
		}

		/// <summary>
		/// 检查是否已经有重复的项目名
		/// </summary>
		/// <param name="strProjectNo"></param>
		/// <param name="strProjectName"></param>
		/// <returns></returns>
		public bool IsRepeatProjectName(string strProjectNo,string strProjectName)
		{
			var instance = db.Project.Where(project => project.ProjectName.ToLower() == strProjectName.ToLower()).FirstOrDefault();
			return !(instance == null || instance.ProjectNo == strProjectNo);
		}

		/// <summary>
		/// 根据ProjectUID查询相关项目信息
		/// </summary>
		/// <returns></returns>
		public List<Project> GetProjectByProjectUID(int intProjectUID)
		{
			var instance = db.Project.Where(project => project.UID == intProjectUID).ToList();
			return instance;
		}

		/// <summary>
		/// 查询所有项目信息
		/// </summary>
		/// <returns></returns>
		public List<Project> Show()
		{
			//查询项目
			List<Project> listProject = db.Project.ToList();
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return listProject;
		}
	}
}
