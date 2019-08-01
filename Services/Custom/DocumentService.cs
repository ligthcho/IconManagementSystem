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
using Services.Helper;
using EntityFramework.Extensions;

namespace Services
{
	public class DocumentService
	{
		public readonly UIDBEntities db = new Models.UIDBEntities();

		/// <summary>
		/// 根据UID获取上传文件信息
		/// </summary>
		/// <param name="intUID"></param>
		/// <returns></returns>
		public Models.Document GetByUID(int intUID)
		{
			return db.Document.Where(d => d.UID == intUID).FirstOrDefault();
		}
		/// <summary>
		/// 新增上传文件记录
		/// </summary>
		/// <param name="userIds"></param>
		/// <returns></returns>
		public CommonResult Add(Models.Document documentModel)
		{
			documentModel.DocumentNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('Document')) ").FirstOrDefault();
			db.Document.Add(documentModel);
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance("新建失败");
			}
			//AllServices.ActionLogService.AddLog("新增上传记录",model.ToJson(),Enums.ActionCategory.Add);
			Log4NetHelper.Info("新增上传记录",documentModel.ToJson());
			return CommonResult.Instance(documentModel.UID);
		}
		/// <summary>
		/// 加载分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Models.Document> GetByPage(int intPageIndex,int intPageSize,Expression<Func<Models.Document,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null,Expression<Func<Models.DocumentTag,bool>> whereTagLambda = null,Expression<Func<Models.DocumentFolder,bool>> whereDocFolderLambda = null)
		{
			if(whereLambda == null)
			{
				whereLambda = u => 1 == 1;
			}
			var q = db.Document.Where(whereLambda).OrderBy(dicOrderBy);
			if(whereTagLambda != null || whereDocFolderLambda != null)
			{
				if(whereTagLambda != null)
				{
					q = db.Document.Where(
					d => db.DocumentTag.Where(whereTagLambda).Select(s => s.DocumentUID).Contains(d.UID)
					).Where(whereLambda).OrderBy(dicOrderBy);
				}
				if(whereDocFolderLambda != null)
				{
					q = db.Document.Where(
					d => db.DocumentFolder.Where(whereDocFolderLambda).Select(s => s.DocumentUID).Contains(d.UID)
					).Where(whereLambda).OrderBy(dicOrderBy);
				}
				if(whereDocFolderLambda != null && whereTagLambda != null)
				{
					q = db.Document.Where(
					d => db.DocumentFolder.Where(whereDocFolderLambda).Select(s => s.DocumentUID).Contains(d.UID) &&
					db.DocumentTag.Where(whereTagLambda).Select(s => s.DocumentUID).Contains(d.UID)
					).Where(whereLambda).OrderBy(dicOrderBy);
				}


			}
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Models.Document>(intPageIndex,intPageSize,q.Count(),list);
		}
		/// <summary>
		/// 加载分页数据(高级查询)
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		//public Page<Models.Document> GetSearchByPage(int intPageIndex,int intPageSize,int intFolderUID,string strTag,DateTime dateBeginTime,DateTime dateEndTime,Dictionary<string,string> dicOrderBy = null)
		//{
		//	if(whereLambda == null)
		//	{
		//		whereLambda = u => 1 == 1;
		//	}
		//	int intFolderUID = 71;//所属系统ID
		//	string strTag = "2";//标签
		//	DateTime dateBeginTime = Convert.ToDateTime("2018-01-12");
		//	DateTime dateEndTime = Convert.ToDateTime("2018-01-13");

		//	var q = db.Document.Where(
		//		d => db.DocumentFolder.Where(df => df.FolderUID == intFolderUID).Select(s => s.DocumentUID).Contains(d.UID) &&
		//		db.DocumentTag.Where(dt => dt.Tag.Contains(strTag)).Select(s => s.DocumentUID).Contains(d.UID) &&
		//		d.CreateTime <= dateEndTime && d.CreateTime >= dateBeginTime
		//		);

		//	var q = db.Document.Where(whereLambda).OrderBy(dicOrderBy);
		//	var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
		//	return new Page<Models.Document>(intPageIndex,intPageSize,q.Count(),list);
		//}

		/// <summary>
		/// 更新项目
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult Update(Models.Document documentModel)
		{
			if(IsRepeatDocumentName(documentModel.DocumentName,documentModel.DocumentPath))
			{
				return CommonResult.Instance("已存在此文件名，请换一个再试");
			}
			db.Document.Where(document => document.UID == documentModel.UID).Update(u => new Models.Document()
			{
				DocumentName = documentModel.DocumentName,
				DocumentPath = documentModel.DocumentPath,
				Editor = documentModel.Editor,
				EditTime = documentModel.EditTime,
				State = documentModel.State
			});
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			Log4NetHelper.Info("更新上传记录",documentModel.ToJson());
			return CommonResult.Instance();
		}
		/// <summary>
		/// 检查是否已经有重复的文件名&&相同的路径
		/// </summary>
		/// <param name="intTaskID"></param>
		/// <param name="strTaskName"></param>
		/// <returns></returns>
		public bool IsRepeatDocumentName(string strDocumentName,string strDocumentPath)
		{
			var instance = db.Document.Where(document => document.DocumentPath == strDocumentPath).FirstOrDefault();
			return !(instance == null || instance.DocumentName == strDocumentName);
		}
		/// <summary>
		/// 更新文件状态
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult UpdateState(int intUID,ItemState ItemState)
		{
			Document document = db.Document.Where(d => d.UID == intUID).FirstOrDefault();
			document.State = (int)ItemState;
			db.SaveChanges();
			Log4NetHelper.Info("更新文件状态","ID:"+ intUID);
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 更新文件状态
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult UpdateState(int[] intUIDs,ItemState ItemState)
		{
			db.Document.Where(d => intUIDs.Contains(d.UID)).Update(document => new Document { State = (int)ItemState });
			db.SaveChanges();
			Log4NetHelper.Info("更新文件状态","IDs:" + intUIDs.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 根据相对路径返回UID
		/// </summary>
		/// <param name="strTaskName"></param>
		/// <returns></returns>
		public int GetUID(string strDocumentPath)
		{
			var instance = db.Document.Where(document => document.DocumentPath == strDocumentPath).FirstOrDefault();
			return instance.UID;
		}

	}
}
