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
using System.Data.Objects.SqlClient;

namespace Services
{
	/// <summary>
	/// 文件夹服务类
	/// </summary>
	public class FolderService
	{
		public readonly Models.UIDBEntities db = new Models.UIDBEntities();
		/// <summary>
		/// 根据FolderNo获取文件夹信息
		/// </summary>
		/// <param name="intFolderID"></param>
		/// <returns></returns>
		public Folder GetByID(int intFolderUID)
		{
			return db.Folder.Where(folder => folder.UID == intFolderUID).FirstOrDefault();
		}
		/// <summary>
		/// 通过FolderUID判断是否存在
		/// </summary>
		/// <param name="intFolderID"></param>
		/// <returns></returns>
		public bool BoolByID(int intFolderUID)
		{
			return db.Folder.Where(folder => folder.UID == intFolderUID).FirstOrDefault() != null? true:false;
		}
		/// <summary>
		/// 根据多个FolderNo获取文件夹信息
		/// </summary>
		/// <param name="intFolderID"></param>
		/// <returns></returns>
		public List<Folder> GetByIDs(int[] intFolderUIDs)
		{
			var listFilter = new List<Utility.Filter>();
			foreach(int i in intFolderUIDs)
			{
				//动态查询表达式
				listFilter.Add(Utility.Filter.Add("UID",Op.Equals,i,true));
			}

			var expTaskList = LambdaExpressionBuilder.GetExpressionByAndAlso<Document>(listFilter);
			Expression<Func<Folder,bool>> whereLambda = null;
			List<Folder> list = db.Folder.Where(whereLambda).ToList();
			return list;
		}

		/// <summary>
		/// 加载分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Folder> GetByPage(int intPageIndex,int intPageSize,Expression<Func<Folder,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null)
		{
			if(whereLambda == null)
			{
				whereLambda = u => 1 == 1;
			}
			var q = db.Folder.Where(whereLambda).OrderBy(dicOrderBy);
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Folder>(intPageIndex,intPageSize,q.Count(),list);
		}
		/// <summary>
		/// 新增文件夹
		/// </summary>
		/// <param name="folderModel"></param>
		/// <returns></returns>
		public CommonResult Add(Folder folderModel)
		{
			if(IsRepeatFolderName(folderModel.UID ,folderModel.FolderName))
			{
				return CommonResult.Instance("已存在此文件名，请换一个再试");
			}
			folderModel.FolderNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('Folder')) ").FirstOrDefault();
			folderModel.CreateTime = DateTime.Now;
			folderModel.EditTime = DateTime.Now;
			db.Folder.Add(folderModel);
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance(0,"新建失败",folderModel.FolderName);
			}
			//AllServices.ActionLogService.AddLog("新增文件夹",folderModel.ToJson(),Enums.ActionCategory.Add);
			Log4NetHelper.Info("新增文件夹",folderModel.ToJson());
			return CommonResult.Instance();
		}
		/// <summary>
		/// 更新文件夹
		/// </summary>
		/// <param name="folderModel"></param>
		/// <returns></returns>
		public CommonResult Update(Folder folderModel)
		{
			if(IsRepeatFolderName(folderModel.UID,folderModel.FolderName))
			{
				return CommonResult.Instance("已存在此项目名，请换一个再试");
			}
			db.Folder.Where(folder => folder.UID == folderModel.UID).Update(u => new Folder()
			{
				FolderName = folderModel.FolderName,
				FatherID = folderModel.FatherID,
				Editor = folderModel.Editor,
				EditTime = DateTime.Now
			});
			Log4NetHelper.Info("更新文件夹信息",folderModel.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		///  更新文件夹名字
		/// </summary>
		/// <param name="folderModel"></param>
		/// <returns></returns>
		public CommonResult UpdateFolderName(Folder folderModel)
		{
			if(IsRepeatFolderName(folderModel.UID,folderModel.FolderName))
			{
				return CommonResult.Instance("已存在此项目名，请换一个再试");
			}
			db.Folder.Where(folder => folder.UID == folderModel.UID).Update(u => new Folder()
			{
				FolderName = folderModel.FolderName,
				Editor = folderModel.Editor,
				EditTime = DateTime.Now
			});
			Log4NetHelper.Info("更新文件夹信息",folderModel.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 删除多个文件夹
		/// </summary>
		/// <param name="intFolderIDs"></param>
		/// <returns></returns>
		public CommonResult Delete(int[] intFolderIDs)
		{
			//删除文件夹
			db.Folder.Where(folder => intFolderIDs.Contains(folder.UID)).Delete();
			Log4NetHelper.Info("删除文件信息","IDs:"+intFolderIDs.ToJson());
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return CommonResult.Instance();
		}

		/// <summary>
		/// 查询所有文件夹信息
		/// </summary>
		/// <returns></returns>
		public List<Folder> Show()
		{
			//查询文件夹
			List<Folder> listFolder = db.Folder.ToList();
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return listFolder;
		}

		/// <summary>
		/// 检查是否已经有重复的文件夹名
		/// </summary>
		/// <param name="intFolderID"></param>
		/// <param name="strFolderName"></param>
		/// <returns></returns>
		public bool IsRepeatFolderName(int intFolderUID,string strFolderName)
		{
			var instance = db.Folder.Where(folder => folder.FolderName.ToLower() == strFolderName.ToLower()).FirstOrDefault();
			return !(instance == null || instance.UID == intFolderUID);
		}
		/// <summary>
		/// 检查是否已经有重复的UID
		/// </summary>
		/// <param name="intFolderID"></param>
		/// <param name="strFolderName"></param>
		/// <returns></returns>
		public bool IsRepeatFolderUID(int intFolderUID)
		{
			   var instance = db.Folder.Where(folder => folder.UID == intFolderUID).FirstOrDefault();
			return !(instance == null || instance.UID == intFolderUID);
		}
	}
}
