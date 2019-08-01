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
	public class DocumentTagService
	{
		public readonly UIDBEntities db = new Models.UIDBEntities();

		/// <summary>
		/// 根据UID获取上传文件信息
		/// </summary>
		/// <param name="intUID"></param>
		/// <returns></returns>
		public Models.DocumentTag GetByUID(int intUID)
		{
			return db.DocumentTag.Where(d => d.UID == intUID).FirstOrDefault();
		}
		/// <summary>
		/// 根据UID获取上传文件信息
		/// </summary>
		/// <param name="intUID"></param>
		/// <returns></returns>
		public Models.DocumentTag GetByDocumentUID(int intDocumentUID)
		{
			return db.DocumentTag.Where(d => d.DocumentUID == intDocumentUID).FirstOrDefault();
		}
		/// <summary>
		/// 新增关联文件标签记录
		/// </summary>
		/// <param name="userIds"></param>
		/// <returns></returns>
		public CommonResult Add(Models.DocumentTag documenttagModel)
		{
			documenttagModel.DocumentTagNo = db.Database.SqlQuery<string>("select ([dbo].[GetNextTN]('DocumentTag'))").FirstOrDefault();
			db.DocumentTag.Add(documenttagModel);
			db.SaveChanges();
			Log4NetHelper.Info("新增关联文件标签记录",documenttagModel.ToJson());
			//AllServices.ActionLogService.AddLog("新增上传记录",model.ToJson(),Enums.ActionCategory.Add);
			return CommonResult.Instance(documenttagModel.UID);
		}
		/// <summary>
		/// 加载分页数据
		/// </summary>
		/// <param name="intPageIndex"></param>
		/// <param name="intPageSize"></param>
		/// <param name="whereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<Models.DocumentTag> GetByPage(int intPageIndex,int intPageSize,Expression<Func<Models.DocumentTag,bool>> whereLambda = null,Dictionary<string,string> dicOrderBy = null)
		{
			if(whereLambda == null)
			{
				whereLambda = u => 1 == 1;
			}
			var q = db.DocumentTag.Where(whereLambda).OrderBy(dicOrderBy);
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();
			return new Page<Models.DocumentTag>(intPageIndex,intPageSize,q.Count(),list);
		}
		/// <summary>
		/// 更新标签
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult Update(Models.DocumentTag documenttagModel)
		{
			db.DocumentTag.Where(documentTag => documentTag.UID == documenttagModel.UID).Update(u => new Models.DocumentTag()
			{
				Tag = documenttagModel.Tag,
				Editor = documenttagModel.Editor,
				EditTime = documenttagModel.EditTime,
				State = documenttagModel.State
			});
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			Log4NetHelper.Info("更新关联文件标签记录",documenttagModel.ToJson());
			return CommonResult.Instance();
		}
		/// <summary>
		/// 更新标签状态
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult UpdateState(int intDocumentUID,ItemState ItemState)
		{
			DocumentTag documentTag = db.DocumentTag.Where(d => d.UID == intDocumentUID).FirstOrDefault();
			documentTag.State = (int)ItemState;
			db.SaveChanges();
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			Log4NetHelper.Info("更新关联文件标签记录","ID:"+ intDocumentUID);
			return CommonResult.Instance();
		}
		/// <summary>
		/// 更新标签状态
		/// </summary>
		/// <param name="documentModel"></param>
		/// <returns></returns>
		public CommonResult UpdateState(int[] intDocumentUIDs,ItemState ItemState)
		{
			db.DocumentTag.Where(d => intDocumentUIDs.Contains(d.UID)).Update(documentFolder => new DocumentTag { State = (int)ItemState });
			db.SaveChanges();
			//AllServices.ActionLogService.AddLog("更新项目信息",model.ToJson(),Enums.ActionCategory.Update);
			Log4NetHelper.Info("更新关联文件标签记录","IDs:" + intDocumentUIDs.ToJson());
			return CommonResult.Instance();
		}
		/// <summary>
		/// 检查是否已经有文件号
		/// </summary>
		/// <param name="intTaskID"></param>
		/// <param name="strTaskName"></param>
		/// <returns></returns>
		public bool IsRepeatNo(int intUID,int intDocumentUID)
		{
			var instance = db.DocumentTag.Where(documentTag => documentTag.UID == intDocumentUID).FirstOrDefault();
			return !(instance == null || instance.UID == intUID);
		}
	}
}
