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
using Services.Helper;

namespace Services
{
	public class DocumentFolderService
	{
		public readonly UIDBEntities db = new Models.UIDBEntities();
		/// <summary>
		/// 根据DocumentUID获取文件信息
		/// </summary>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public List<DocumentFolder> GetByDocumentUID(int intDocumentUID)
		{
			Log4NetHelper.Info("查询关联文件记录","ID:"+ intDocumentUID);
			return db.DocumentFolder.Where(documentFolder => documentFolder.DocumentUID == intDocumentUID).ToList();
		}

		///// <summary>
		///// 加载分页数据
		///// </summary>
		///// <param name="intPageIndex"></param>
		///// <param name="intPageSize"></param>
		///// <param name="whereLambda"></param>
		///// <param name="dicOrderBy"></param>
		///// <returns></returns>
		//public Page<Folder> GetByPage(int intPageIndex,int intPageSize,Expression<Func<Folder,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null)
		//{
		//	if(whereLambda == null)
		//	{
		//		whereLambda = u => 1 == 1;
		//	}
		//	var q = db.Folder.Where(whereLambda).OrderBy(dicOrderBy);
		//	var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
		//	return new Page<Folder>(intPageIndex,intPageSize,q.Count(),list);
		//}
		/// <summary>
		/// 新增文件关联
		/// </summary>
		/// <param name="folderModel"></param>
		/// <returns></returns>
		public CommonResult Add(DocumentFolder documentfolderModel)
		{
			if(IsRepeatFolderUIDwithDocumentUID(documentfolderModel.UID,documentfolderModel.FolderUID))
			{
				return CommonResult.Instance("已存在此项目名，请换一个再试");
			}
			documentfolderModel.DocumentFolderNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('DocumentFolder'))").FirstOrDefault();

			db.DocumentFolder.Add(documentfolderModel);
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance("新建失败");
			}
			//AllServices.ActionLogService.AddLog("新增文件",folderModel.ToJson(),Enums.ActionCategory.Add);
			Log4NetHelper.Info("新增关联文件记录",documentfolderModel.ToJson());
			return CommonResult.Instance(documentfolderModel.UID);
		}
		/// <summary>
		/// 新增文件关联
		/// </summary>
		/// <param name="folderModel"></param>
		/// <returns></returns>
		public CommonResult Add(List<DocumentFolder> listDocumentFolderModel)
		{
			foreach(DocumentFolder documentFolder in listDocumentFolderModel)
			{
				documentFolder.DocumentFolderNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('DocumentFolder'))").FirstOrDefault();
			}			
			db.DocumentFolder.AddRange(listDocumentFolderModel);
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance("新建失败");
			}
			Log4NetHelper.Info("新增关联文件记录",listDocumentFolderModel.ToJson());
			//AllServices.ActionLogService.AddLog("新增文件",folderModel.ToJson(),Enums.ActionCategory.Add);
			return CommonResult.Instance();
		}

		/// <summary>
		/// 更新文件
		/// </summary>
		/// <param name="folderModel"></param>
		/// <returns></returns>
		public CommonResult Update(DocumentFolder DocumentFolderModel)
		{
			db.DocumentFolder.Where(documentFolder => documentFolder.UID == DocumentFolderModel.UID).Update(u => DocumentFolderModel);
			if(db.SaveChanges() < 0)
			{
				return CommonResult.Instance("新建失败");
			}
			Log4NetHelper.Info("更新关联文件记录",DocumentFolderModel.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 删除文件关联
		/// </summary>
		/// <param name="intFolderIDs"></param>
		/// <returns></returns>
		public CommonResult Delete(int intDocumentUID)
		{
			//删除关联
			db.DocumentFolder.Where(documentFolder => intDocumentUID == documentFolder.DocumentUID).Delete();
			Log4NetHelper.Info("删除关联文件记录","ID:"+ intDocumentUID);
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 删除文件关联
		/// </summary>
		/// <param name="intFolderIDs"></param>
		/// <returns></returns>
		public CommonResult Delete(int[] intDocumentUID)
		{
			//删除关联
			db.DocumentFolder.Where(documentFolder => intDocumentUID.Contains(documentFolder.DocumentUID)).Delete();
			Log4NetHelper.Info("删除关联文件记录","IDs:" + intDocumentUID.ToJson());
			//AllServices.ActionLogService.AddLog("删除项目",string.Join(",",projectIds),Enums.ActionCategory.Del);
			return CommonResult.Instance();
		}

		///// <summary>
		///// 检查是否已经有重复的文件名与文件夹名 存在返回flase 
		///// </summary>
		///// <param name="intFolderID"></param>
		///// <param name="strFolderName"></param>
		///// <returns></returns>
		public bool IsRepeatFolderUIDwithDocumentUID(int intFolderID,int intDocumentUID)
		{
			var instance = db.DocumentFolder.Where(documentfolder => documentfolder.DocumentUID == intDocumentUID && documentfolder.FolderUID == intFolderID).FirstOrDefault();
			Log4NetHelper.Info("检查是否已经有重复的文件名与文件夹名",string.Empty);
			return !(instance == null);
		}
		/// <summary>
		/// 更新文件状态
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult UpdateState(int intDocumentUID,ItemState ItemState)
		{
			db.DocumentFolder.Where(documentFolder => documentFolder.DocumentUID == intDocumentUID).Update(documentFolder => new DocumentFolder { State = (int)ItemState });
			db.SaveChanges();
			Log4NetHelper.Info("更新关联文件记录","ID:" + intDocumentUID);
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 批量更新文件状态
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult UpdateState(int[] intDocumentUIDs,ItemState ItemState)
		{
			db.DocumentFolder.Where(documentFolder => intDocumentUIDs.Contains(documentFolder.DocumentUID)).Update(documentFolder => new DocumentFolder { State = (int)ItemState });
			db.SaveChanges();
			Log4NetHelper.Info("批量更新关联文件记录","IDs:" + intDocumentUIDs.ToJson());
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			return CommonResult.Instance();
		}
	}
}
