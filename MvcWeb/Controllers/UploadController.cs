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
using System.IO;

using Foundation.SiteConfig.Models;

namespace MvcWeb.Controllers
{
	[Description("上传文件管理控制器")]
	public class UploadController:Controller
	{
		public readonly string strKEY = "fileuploadkey";

		[Description("上传文件管理页面")]
		public ActionResult Index()
		{
			return View("UploadFile");
		}
		//UploadFile
		[Description("上传文件修改页面")]
		public ActionResult UploadDetial()
		{
			int intUID = Request["UID"].ToInt32();
			Document document = AllServices.DocumentService.GetByUID(intUID);
			if(document != null)
			{
				DocumentTag documentTag = AllServices.DocumentTagService.GetByDocumentUID(document.UID);
				List<DocumentFolder> listDocumentFolder = AllServices.DocumentFolderService.GetByDocumentUID(document.UID);

				DataModel dataModel = new DataModel();
				dataModel.Add("Document",document);
				dataModel.Add("DocumentTag",documentTag);
				dataModel.Add("ListDocumentFolder",listDocumentFolder);
				ViewBag.CommonResult = CommonResult.Instance(1,null,dataModel).ToJson();
			}
			else
			{
				ViewBag.CommonResult = CommonResult.Instance().ToJson();
			}
			return View("UploadDetial");
		}
		/// <summary>
		/// 上传图片,根据config的参数值确定对应的配置文件
		/// </summary>
		/// <param name="fileData"></param>
		/// <returns></returns>
		public string UploadImage()
		{
			var userModel = AllServices.UserService.GetUserBySession();

			string strkey = Request["key"].ToStr();
			string strTag = Request["tag"].ToStr();
			string strSize = Request["size"].ToStr();
			string strSysID = Request["sysID"].ToStr();
			int intDpiID = Request["dpi"].ToInt32();
			if(strkey != this.strKEY)
			{
				return new CommonResult("上传文件失败,key值对应不上").ToJson();
			}
			string strConfig = Request["config"].ToStr("ImageUploadBaseConfig");
			CommonResult commonResult = new CommonResult();
			HttpPostedFileBase postFile = Request.Files[0];
			var configInstance = ConfigHelper.GetConfig<Foundation.SiteConfig.Models.ImageUploadBaseConfig>(strConfig);
			DataModel dataModel = new DataModel();
			dataModel.Add("DpiID",intDpiID);
			dataModel.Add("Size",strSize);
			commonResult = AllServices.ImageUploadService.ImageHandler(postFile,configInstance,userModel.UserName,dataModel,ActionType.Add);//保存文件
			if(commonResult.code < 1)
			{
				return commonResult.ToJson();
			}
			if(string.IsNullOrEmpty(strSysID))
			{
				return new CommonResult("请选择所属系统").ToJson();
			}
			int[] intSysIDs = ArrayHelper.ToIntArray(strSysID);
			CommonResult commonResult1 = AddDocumentFolder(intSysIDs,(int)commonResult.data);
			CommonResult commonResult2 = AddTag(strTag,(int)commonResult.data);
			if(commonResult1.code < 1)
			{
				commonResult = new CommonResult("文件上传成功,标签新增失败");
			}
			else if(commonResult2.code < 1)
			{
				commonResult = new CommonResult("文件上传成功,所属系统失败");
			}
			else if(commonResult1.code < 1 && commonResult2.code < 1)
			{
				commonResult = new CommonResult("文件上传成功,标签新增失败，所属系统失败");
			}
			return commonResult.ToJson();//上传成功返回图片信息
		}
		/// <summary>
		/// 修改图片,根据config的参数值确定对应的配置文件
		/// </summary>
		/// <param name="fileData"></param>
		/// <returns></returns>
		public string UpdateImage()
		{
			var userModel = AllServices.UserService.GetUserBySession();

			int intDpiID = Request["dpi"].ToInt32();
			string strkey = Request["key"].ToStr();
			string strTag = Request["tag"].ToStr();
			string strSize = Request["size"].ToStr();
			string strSysID = Request["sysID"].ToStr();
			int intUID = Request["uid"].ToInt32();

			if(strkey != this.strKEY)
			{
				return new CommonResult("上传文件失败,key值对应不上").ToJson();
			}
			string strConfig = Request["config"].ToStr("ImageUploadBaseConfig");
			DataModel dataModel = new DataModel();
			dataModel.Add("UID",intUID);
			dataModel.Add("DpiID",intDpiID);
			dataModel.Add("Size",strSize);
			CommonResult commonResult = new CommonResult();

			HttpPostedFileBase postFile = Request.Files[0];
			var configInstance = ConfigHelper.GetConfig<Foundation.SiteConfig.Models.ImageUploadBaseConfig>(strConfig);
			commonResult = AllServices.ImageUploadService.ImageHandler(postFile,configInstance,userModel.UserName,dataModel,ActionType.Modify);//修改文件

			if(commonResult.code < 1)
			{
				return commonResult.ToJson();
			}
			if(string.IsNullOrEmpty(strSysID))
			{
				return new CommonResult("请选择所属系统").ToJson();
			}
			int[] intSysIDs = ArrayHelper.ToIntArray(strSysID);
			CommonResult commonResult1 = AddDocumentFolder(intSysIDs,(int)commonResult.data);
			CommonResult commonResult2 = UpdateTag(strTag,intUID);
			if(commonResult1.code < 1)
			{
				commonResult = new CommonResult("文件修改成功,标签修改失败");
			}
			else if(commonResult2.code < 1)
			{
				commonResult = new CommonResult("文件修改成功,所属系统修改失败");
			}
			else if(commonResult1.code < 1 && commonResult2.code < 1)
			{
				commonResult = new CommonResult("文件修改成功,标签修改失败，所属修改失败");
			}
			return commonResult.ToJson();//上传成功返回图片信息
										 //var res = AllServices.ImageUploadService.ImageHandler(fileData, AllConfigServices.NewsImageUploadConfig);
		}
		/// <summary>
		/// 修改图片信息
		/// </summary>
		/// <param name="fileData"></param>
		/// <returns></returns>
		public string UpdateImageInfo()
		{
			var userModel = AllServices.UserService.GetUserBySession();

			int intDpiID = Request["dpi"].ToInt32();
			string strTag = Request["tag"].ToStr();
			string strSize = Request["size"].ToStr();
			string strSysID = Request["sysID"].ToStr();
			string strName = Request["name"].ToStr();
			string strPath = Request["path"].ToStr();
			string strType = Request["type"].ToStr();
			int intUID = Request["uid"].ToInt32();
			string strConfig = Request["config"].ToStr("ImageUploadBaseConfig");
			DataModel dataModel = new DataModel();
			dataModel.Add("UID",intUID);
			dataModel.Add("DpiID",intDpiID);
			dataModel.Add("Size",strSize);
			CommonResult commonResult;
			var configInstance = ConfigHelper.GetConfig<Foundation.SiteConfig.Models.ImageUploadBaseConfig>(strConfig);

			if(!FileHelper.RenameFile(PathHelper.GetMapPath(strPath),strName + strType))
			{
				commonResult = new CommonResult("重命名失败,修改终止,请联系管理员");
				return commonResult.ToJson();
			}
			//更新图片信息记录
			Document documentModel = new Document();
			documentModel.UID = dataModel["UID"].ToInt();
			documentModel.DocumentName = strName;
			documentModel.DocumentPath = string.Format("{0}/{1}",configInstance.SavePath,strName + strType);
			documentModel.PictureSize = dataModel["Size"].ToStr();
			documentModel.PictureResolution = dataModel["DpiID"].ToInt() == 0 ? "96dpi" : "72dpi";
			documentModel.Editor = userModel.UserName;
			documentModel.EditTime = DateTime.Now;
			documentModel.State = Convert.ToInt32(ItemState.Enable);

			commonResult = AllServices.DocumentService.Update(documentModel);

			if(commonResult.code < 1)
			{
				return commonResult.ToJson();
			}
			if(string.IsNullOrEmpty(strSysID))
			{
				return new CommonResult("请选择所属系统").ToJson();
			}
			int[] intSysIDs = ArrayHelper.ToIntArray(strSysID);
			CommonResult commonResult1 = AddDocumentFolder(intSysIDs,intUID);
			CommonResult commonResult2 = UpdateTag(strTag,intUID);
			if(commonResult1.code < 1)
			{
				commonResult = new CommonResult("文件修改成功,标签修改失败");
			}
			else if(commonResult2.code < 1)
			{
				commonResult = new CommonResult("文件修改成功,所属系统修改失败");
			}
			else if(commonResult1.code < 1 && commonResult2.code < 1)
			{
				commonResult = new CommonResult("文件修改成功,标签修改失败，所属修改失败");
			}
			return commonResult.ToJson();
		}
		/// <summary>
		/// 分页展示图片
		/// </summary>
		/// <returns></returns>
		public string Show()
		{
			int intPageIndex = Request["PageIndex"].ToInt32();
			int intPageSize = Request["PageSize"].ToInt32();
			int intState = Request["State"].ToInt32();
			string strQueryList = Request["QueryList"].ToStr();
			string strOrder = Request["Order"].ToStr() == string.Empty ? "asc" : Request["Order"].ToStr();
			string strCond = Request["Cond"].ToStr() == string.Empty ? "DocumentName" : Request["Cond"].ToStr();//默认排序 姓名 a-z递增
			List<DataModel> listDataModel = JsonHelper.ToListDataModel(strQueryList);

			var listFilter1 = new List<Utility.Filter>();
			var listFilter2 = new List<Utility.Filter>();
			var listFilter3 = new List<Utility.Filter>();

			foreach(DataModel dataModel in listDataModel)
			{
				string strField = dataModel["Field"].ToStr();
				string strOperation = dataModel["Operation"].ToStr();
				string strFieldValue = dataModel["FieldValue"].ToStr();

				if(!Enum.IsDefined(typeof(Op),strOperation))//第二个参数也可以传入intValue
				{
					strOperation = Op.Equals.ToString();//没有
				}
				Op op = (Op)Enum.Parse(typeof(Op),strOperation);
				if(strField == "Tag")
				{
					listFilter2.Add(Utility.Filter.Add(strField,op,strFieldValue,true));
				}
				else if(strField == "FolderUID")
				{
					listFilter3.Add(Utility.Filter.Add(strField,op,strFieldValue,true));
				}
				else if(strField == "CreateTime")
				{
					listFilter1.Add(Utility.Filter.Add(strField,op,Convert.ToDateTime(strFieldValue),true));
				}
				else
				{
					listFilter1.Add(Utility.Filter.Add(strField,op,strFieldValue,true));
				}

			}
			//,string strTag,DateTime dateBeginTime,DateTime dateEndTime
			//动态查询表达式
			if(intState <= 10)
			{
				listFilter1.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Enable),true));
			}
			else
			{
				listFilter1.Add(Utility.Filter.Add("State",Op.Equals,Convert.ToString((int)ItemState.Disable),true));
			}

			var expTaskList = LambdaExpressionBuilder.GetExpressionByAndAlso<Document>(listFilter1);
			var expTagTaskList = LambdaExpressionBuilder.GetExpressionByAndAlso<DocumentTag>(listFilter2);
			var expDocumentFolderTaskList = LambdaExpressionBuilder.GetExpressionByAndAlso<DocumentFolder>(listFilter3);

			//排序所需字典
			Dictionary<string,string> dicOrderBy = new Dictionary<string,string>();
			dicOrderBy.Add(strCond,strOrder);
			//分页获取数据
			Page<Document> listDocumentList = AllServices.DocumentService.GetByPage(intPageIndex,intPageSize,expTaskList,dicOrderBy,expTagTaskList,expDocumentFolderTaskList);

			return CommonResult.Instance(1,null,listDocumentList).ToJson();
		}
		/// <summary>
		/// 还原图片状态
		/// </summary>
		/// <returns></returns>
		public string RestoreImages()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strSysID = Request["SysID"].ToStr();
			string strUIDs = Request["UIDs"].ToStr();
			int[] intUIDs = ArrayHelper.ToIntArray(strUIDs);
			int[] intSysIDs = ArrayHelper.ToIntArray(strSysID);
			//状态改变
			AllServices.DocumentService.UpdateState(intUIDs,ItemState.Enable);
			//关联表
			AddDocumentFolder(intSysIDs,intUIDs);
			return CommonResult.Instance().ToJson();
		}
		/// <summary>
		/// 添加图片标签
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public CommonResult AddTag(string strTag,int intDocumentUID)
		{
			var userModel = AllServices.UserService.GetUserBySession();

			DocumentTag documentTag = new DocumentTag();
			documentTag.DocumentUID = intDocumentUID;
			documentTag.Tag = strTag;
			documentTag.Creater = userModel.UserName;
			documentTag.CreateTime = DateTime.Now;
			documentTag.Editor = userModel.UserName;
			documentTag.EditTime = DateTime.Now;
			documentTag.State = (int)ItemState.Enable;
			return AllServices.DocumentTagService.Add(documentTag);
		}

		/// <summary>
		/// 更新图片标签
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public CommonResult UpdateTag(string strTag,int intDocumentUID)
		{
			var userModel = AllServices.UserService.GetUserBySession();

			DocumentTag documentTag = new DocumentTag();
			documentTag.UID = AllServices.DocumentTagService.GetByDocumentUID(intDocumentUID).UID;
			documentTag.DocumentUID = intDocumentUID;
			documentTag.Tag = strTag;
			documentTag.Editor = userModel.UserName;
			documentTag.EditTime = DateTime.Now;
			documentTag.State = (int)ItemState.Enable;
			return AllServices.DocumentTagService.Update(documentTag);
		}

		/// <summary>
		/// 新增关联图片系统
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intFolderUID"></param>
		/// <returns></returns>
		public CommonResult AddDocumentFolder(int[] intSysIDs,int intDocumentUID)
		{
			var userModel = AllServices.UserService.GetUserBySession();
			Folder folder = new Folder();
			bool boolRes = false;
			CommonResult commonResult = new CommonResult();
			foreach(int i in intSysIDs)
			{
				if(!AllServices.FolderService.BoolByID(i)) //判断所属系统id是否存在 
				{
					boolRes = true;
				}
			}
			if(!boolRes)
			{
				AllServices.DocumentFolderService.Delete(intDocumentUID);//取消关联
				foreach(int UID in intSysIDs)
				{
					DocumentFolder documentfolderModel = new DocumentFolder();
					documentfolderModel.FolderUID = UID;
					documentfolderModel.DocumentUID = intDocumentUID;
					documentfolderModel.Creater = userModel.UserName;
					documentfolderModel.CreateTime = DateTime.Now;
					documentfolderModel.Editor = userModel.UserName;
					documentfolderModel.EditTime = DateTime.Now;
					documentfolderModel.State = Convert.ToInt32(ItemState.Enable);
					commonResult = AllServices.DocumentFolderService.Add(documentfolderModel);
					if(commonResult.code < 0)
					{
						break;
					}
				}
			}
			return commonResult;
		}

		/// <summary>
		/// 新增关联图片系统
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intFolderUID"></param>
		/// <returns></returns>
		public CommonResult AddDocumentFolder(int[] intSysIDs,int[] intDocumentUIDs)
		{
			var userModel = AllServices.UserService.GetUserBySession();
			Folder folder = new Folder();
			bool boolRes = false;
			CommonResult commonResult = new CommonResult();
			foreach(int i in intSysIDs)
			{
				if(!AllServices.FolderService.BoolByID(i)) //判断所属系统id是否存在 
				{
					boolRes = true;
				}
			}
			if(!boolRes)
			{
				AllServices.DocumentFolderService.Delete(intDocumentUIDs);//取消关联
				List<DocumentFolder> listDocumentFolder = new List<DocumentFolder>();
				foreach(int UID in intDocumentUIDs)
				{
					foreach(int SysID in intSysIDs)
					{
						DocumentFolder documentfolderModel = new DocumentFolder();
						documentfolderModel.FolderUID = SysID;
						documentfolderModel.DocumentUID = UID;
						documentfolderModel.DocumentFolderNo = Convert.ToString(Services.Helper.ServiceHelper.GetRandom());
						documentfolderModel.Creater = userModel.UserName;
						documentfolderModel.CreateTime = DateTime.Now;
						documentfolderModel.Editor = userModel.UserName;
						documentfolderModel.EditTime = DateTime.Now;
						documentfolderModel.State = Convert.ToInt32(ItemState.Enable);
						listDocumentFolder.Add(documentfolderModel);
					}
				}
				AllServices.DocumentFolderService.Add(listDocumentFolder);
			}
			return commonResult;
		}

		/// <summary>
		/// 新增关联任务子列表清单
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intFolderUID"></param>
		/// <returns></returns>
		public CommonResult AddTaskList(int intDocumentUID)
		{
			var userModel = AllServices.UserService.GetUserBySession();
			CommonResult commonResult = new CommonResult();
			Document document =AllServices.DocumentService.GetByUID(intDocumentUID);
			commonResult=AllServices.TaskListService.UpdateByDocumentUID(document.UID,document.DocumentName);
			return commonResult;
		}

		/// <summary>
		/// 删除图片（修改图片状态）
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public string DeleteImage()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			int intUID = Request["UID"].ToInt32();
			//关联表状态改禁用
			AllServices.DocumentTagService.UpdateState(intUID,ItemState.Disable);
			AllServices.DocumentFolderService.UpdateState(intUID,ItemState.Disable);
			AllServices.DocumentService.UpdateState(intUID,ItemState.Disable);
			return CommonResult.Instance().ToJson();
		}
		/// <summary>
		/// 还原图片状态（修改图片状态）
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public string RestoreImage()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			int intUID = Request["UID"].ToInt32();
			//关联表状态改禁用
			AllServices.DocumentTagService.UpdateState(intUID,ItemState.Enable);
			AllServices.DocumentFolderService.UpdateState(intUID,ItemState.Enable);
			AllServices.DocumentService.UpdateState(intUID,ItemState.Enable);
			return CommonResult.Instance().ToJson();
		}
		/// <summary>
		/// 删除图片集合（修改图片状态）
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public string DeleteImages()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strUIDs = Request["UIDs"].ToStr();
			int[] intUIDs = ArrayHelper.ToIntArray(strUIDs);
			//关联表状态改禁用
			AllServices.DocumentTagService.UpdateState(intUIDs,ItemState.Disable);
			AllServices.DocumentFolderService.UpdateState(intUIDs,ItemState.Disable);
			AllServices.DocumentService.UpdateState(intUIDs,ItemState.Disable);
			return CommonResult.Instance().ToJson();
		}
		/// <summary>
		/// 重新关联所属系统
		/// </summary>
		/// <param name="strTag"></param>
		/// <param name="intDocumentUID"></param>
		/// <returns></returns>
		public string MoveImages()
		{
			var userModel = AllServices.UserService.GetUserBySession();
			string strSysID = Request["SysID"].ToStr();
			string strUIDs = Request["UIDs"].ToStr();
			int[] intUIDs = ArrayHelper.ToIntArray(strUIDs);
			int[] intSysIDs = ArrayHelper.ToIntArray(strSysID);
			//关联表
			AddDocumentFolder(intSysIDs,intUIDs);
			return CommonResult.Instance().ToJson();
		}
		/// <summary>
		/// 下载文件
		/// </summary>
		/// <returns></returns>
		public FilePathResult GetImage()
		{
			string strUID = Request["UID"].ToStr();
			string filePath = Server.MapPath("~/UploadFiles/images/病历检查申请模板_S.png");//路径
			return File(filePath,"text/plain","病历检查申请模板_S.png"); //welcome.txt是客户端保存的名字
		}

		public FileStreamResult GetImageFiles()
		{
			////获取服务器中的文件路径
			string strConfig = Request["config"].ToStr("ImageUploadBaseConfig");
			var configInstance = ConfigHelper.GetConfig<Foundation.SiteConfig.Models.ImageUploadBaseConfig>(strConfig);

			string listImagePath = Request["ListImagePath"].ToStr();
			List<DataModel> listDataModel = JsonHelper.ToListDataModel(listImagePath);
			CommonResult commonResult = AllServices.ImageDownloadService.ImageHandler(configInstance,listDataModel);
			if(commonResult.code == 1)
			{
				string strDestFile = commonResult.data.ToStr();
				return File(new FileStream(strDestFile,FileMode.Open),"application/octet-stream",System.IO.Path.GetFileName(strDestFile));
			}
			else if(commonResult.code == 2)
			{
				string strDestFile = commonResult.data.ToStr();
				//只读文件 +FileAccess.Read
				return File(new FileStream(strDestFile,FileMode.Open,FileAccess.Read),"application/image",System.IO.Path.GetFileName(strDestFile));
			}
			return File(new FileStream(commonResult.data.ToStr(),FileMode.Open,FileAccess.Read),"application/image",System.IO.Path.GetFileName(commonResult.data.ToStr()));
		}
		/////////////////////////////////WebUploader
		/// <summary>
		/// 是否调试状态（若true，上传的文件不会实际保存）
		/// </summary>
		readonly bool IsDeBug = false;//Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsDeBug"].ToString());

		/// <summary>
		/// 文件分块上传
		/// </summary>
		/// <param name="fileData"></param>
		/// <returns></returns>
		public string ChunkUpload(HttpPostedFileBase file)
		{
			var userModel = AllServices.UserService.GetUserBySession();

			//if(IsDeBug)
			//{
			//	return CommonResult.Instance(0,string.Empty,"{\"chunked\" : true, \"ext\" : \"" + Path.GetExtension(file.FileName) + "\"}").ToJson();
			//}

			if(file == null) //当用户在上传文件时突然刷新页面，可能造成这种情况
			{
				return CommonResult.Instance(-1,"HttpPostedFileBase对象为null","{\"chunked\" : " + Request.Form.AllKeys.Any(m => m == "chunk") + "}").ToJson();
			}
			string strConfig = Request["config"].ToStr("ImageUploadBaseConfig");
			var configInstance = ConfigHelper.GetConfig<Foundation.SiteConfig.Models.ImageUploadBaseConfig>(strConfig);

			string strRoot = PathHelper.GetMapPath(configInstance.SavePath);//拿到保存图片绝对路径

			string strTag = Request["tag"].ToStr();
			string strSize = Request["size"].ToStr();
			string strSysID = Request["sysID"].ToStr();
			int intDpiID = Request["dpi"].ToInt32();
			string definedName = Request["definedName"].ToStr();
			string strExt = Request["ext"].ToStr();

			DataModel dataModel = new DataModel();
			dataModel.Add("DpiID",intDpiID);
			dataModel.Add("Size",strSize);
			dataModel.Add("DpiID",intDpiID);
			dataModel.Add("Size",strSize);
			dataModel.Add("Ext",strExt);
			dataModel.Add("DefinedName",definedName);

			//如果进行了分片
			if(Request.Form.AllKeys.Any(m => m == "chunk"))
			{
				//取得chunk和chunks
				int intChunk = Convert.ToInt32(Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
				int intChunks = Convert.ToInt32(Request.Form["chunks"]);//总分片数
																		//根据GUID创建用该GUID命名的临时文件夹
																		//string folder = Server.MapPath("~/Upload/" + Request["md5"] + "/");
				string strFolder = string.Format("{0}/{1}",strRoot,"chunk\\" + Request["md5"] + "\\");//临时文件夹
				string strPath = strFolder + intChunk;

				//建立临时传输文件夹
				if(!Directory.Exists(Path.GetDirectoryName(strFolder)))
				{
					Directory.CreateDirectory(strFolder);
				}

				FileStream addFile = null;
				BinaryWriter AddWriter = null;
				Stream stream = null;
				BinaryReader TempReader = null;

				try
				{
					//addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
					addFile = new FileStream(strPath,FileMode.Create,FileAccess.Write);
					AddWriter = new BinaryWriter(addFile);
					//获得上传的分片数据流
					stream = file.InputStream;
					TempReader = new BinaryReader(stream);
					//将上传的分片追加到临时文件末尾
					AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
				}
				finally
				{
					if(addFile != null)
					{
						addFile.Close();
						addFile.Dispose();
					}
					if(AddWriter != null)
					{
						AddWriter.Close();
						AddWriter.Dispose();
					}
					if(stream != null)
					{
						stream.Close();
						stream.Dispose();
					}
					if(TempReader != null)
					{
						TempReader.Close();
						TempReader.Dispose();
					}
				}

				//context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"f_ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
				//return CommonResult.ToJsonStr(0, string.Empty, "{\"chunked\" : true, \"ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
				return CommonResult.Instance(0,string.Empty,new ChunkTemp()
				{
					chunked = true,
					ext = Path.GetExtension(file.FileName),
				}).ToJson();
			}
			else//没有分片直接保存
			{
				CommonResult commonResult = AllServices.ImageUploadService.ImageHandler(Request.Files[0],configInstance,userModel.UserName,dataModel,ActionType.Add);
				//string path = string.Format("{0}/{1}",strRoot,Request["md5"] + Path.GetExtension(Request.Files[0].FileName));
				//Request.Files[0].SaveAs(path);

				if(commonResult.code < 0)
				{
					//context.Response.Write("{\"chunked\" : false, \"hasError\" : false}");
					return CommonResult.Instance(0,string.Empty,"{\"chunked\" : false}").ToJson();
				}
				CommonResult commonResult1 = new CommonResult("请选择所属系统");
				if(!string.IsNullOrEmpty(strSysID))
				{
					int[] intSysIDs = ArrayHelper.ToIntArray(strSysID);
					commonResult1 = AddDocumentFolder(intSysIDs,(int)commonResult.data);
				}
				CommonResult commonResult2 = AddTag(strTag,(int)commonResult.data);
				CommonResult commonResult3 = AddTaskList((int)commonResult.data);
				if(commonResult1.code < 1)
				{
					commonResult = new CommonResult("文件上传成功,标签新增失败");
				}
				else if(commonResult2.code < 1)
				{
					commonResult = new CommonResult("文件上传成功,所属系统失败");
				}
				else if(commonResult3.code < 1)
				{
					commonResult = new CommonResult("文件上传成功,所属系统成功，任务列表清单关联失败");
				}
				else if(commonResult1.code < 1 && commonResult2.code < 1 && commonResult3.code < 1)
				{
					commonResult = new CommonResult("文件上传成功,标签新增失败，所属系统失败");
				}

				return commonResult.ToJson();
			}
		}

		private class ChunkTemp
		{
			public bool chunked
			{
				get; set;
			}
			public string ext
			{
				get; set;
			}
		}

		#region 获取指定文件的已上传的文件块
		/// <summary>
		/// 获取指定文件的已上传的文件块
		/// </summary>
		/// <returns></returns>
		public string GetMaxChunk()
		{
			if(IsDeBug)
			{
				return CommonResult.Instance(0,string.Empty,0).ToJson();
			}
			string strConfig = Request["config"].ToStr("ImageUploadBaseConfig");
			var configInstance = ConfigHelper.GetConfig<Foundation.SiteConfig.Models.ImageUploadBaseConfig>(strConfig);
			string strRoot = Server.MapPath(configInstance.SavePath);
			try
			{
				var md5 = Convert.ToString(Request["md5"]);
				var ext = Convert.ToString(Request["ext"]);
				int chunk = 0;

				var fileName = md5 + "." + ext;

				FileInfo file = new FileInfo(string.Format("{0}/{1}",strRoot,fileName));
				if(file.Exists)
				{
					chunk = Int32.MaxValue;
				}
				else
				{
					if(Directory.Exists(string.Format("{0}/{1}",strRoot,"chunk\\" + md5)))
					{
						DirectoryInfo dicInfo = new DirectoryInfo(string.Format("{0}/{1}",strRoot,"chunk\\" + md5));
						var files = dicInfo.GetFiles();
						chunk = files.Count();
						if(chunk > 1)
						{
							chunk = chunk - 1; //当文件上传中时，页面刷新，上传中断，这时最后一个保存的块的大小可能会有异常，所以这里直接删除最后一个块文件
						}
					}
				}

				return CommonResult.Instance(0,string.Empty,chunk).ToJson();
			}
			catch
			{
				return CommonResult.Instance(0,string.Empty,0).ToJson();
			}
		}
		#endregion

		#region 合并文件
		/// <summary>
		/// 合并文件
		/// </summary>
		/// <returns></returns>
		public string MergeFiles()
		{
			var userModel = AllServices.UserService.GetUserBySession();

			string strTag = Request["tag"].ToStr();
			string strSize = Request["size"].ToStr();
			string strSysID = Request["sysID"].ToStr();
			int intDpiID = Request["dpi"].ToInt32();
			string definedName = Request["definedName"].ToStr();
			string strGuid = Request["md5"];
			string strExt = Request["ext"];

			DataModel dataModel = new DataModel();
			dataModel.Add("DpiID",intDpiID);
			dataModel.Add("Size",strSize);
			dataModel.Add("Guid",strGuid);
			dataModel.Add("Ext",strExt);
			dataModel.Add("DefinedName",definedName);

			string strConfig = Request["config"].ToStr("ImageUploadBaseConfig");
			var configInstance = ConfigHelper.GetConfig<Foundation.SiteConfig.Models.ImageUploadBaseConfig>(strConfig);

			CommonResult commonResult = AllServices.ImageUploadService.ChunkImageHandler(configInstance,userModel.UserName,dataModel,ActionType.Add);//合并保存文件
			if(commonResult.code < 0)
			{
				return CommonResult.Instance(0,string.Empty,"{\"chunked\" : false}").ToJson();
			}
			CommonResult commonResult1 = new CommonResult("请选择所属系统");
			if(!string.IsNullOrEmpty(strSysID))
			{
				int[] intSysIDs = ArrayHelper.ToIntArray(strSysID);
				commonResult1 = AddDocumentFolder(intSysIDs,(int)commonResult.data);
			}
			CommonResult commonResult2 = new CommonResult("请选择标签");
			if(!string.IsNullOrWhiteSpace(strTag))
			{
				commonResult2 = AddTag(strTag,(int)commonResult.data);
			}
			CommonResult commonResult3 = new CommonResult("任务列表清单关联失败");
			if(commonResult.data.ToInt32()!=0)
			{
				commonResult3 = AddTaskList((int)commonResult.data);
			}
			if(commonResult1.code < 1)
			{
				commonResult = new CommonResult("文件上传成功,标签新增失败");
			}
			else if(commonResult2.code < 1)
			{
				commonResult = new CommonResult("文件上传成功,所属系统失败");
			}
			else if(commonResult3.code < 1)
			{
				commonResult = new CommonResult("文件上传成功,所属系统成功，任务列表清单关联失败");
			}
			else if(commonResult1.code < 1 && commonResult2.code < 1 && commonResult3.code < 1)
			{
				commonResult = new CommonResult("文件上传成功,标签新增失败，所属系统失败");
			}
			return commonResult.ToJson();

		}
		#endregion
	}
}