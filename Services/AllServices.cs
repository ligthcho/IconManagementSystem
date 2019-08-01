using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class AllServices
	{
		/// <summary>
		/// 后台用户服务类
		/// </summary>
		public static UserService UserService
		{
			get
			{
				return new UserService();
			}
		}
		/// <summary>
		/// 主任务类
		/// </summary>
		public static TaskService TaskService
		{
			get
			{
				return new TaskService();
			}
		}
		/// <summary>
		/// 子任务类
		/// </summary>
		public static TaskListService TaskListService
		{
			get
			{
				return new TaskListService();
			}
		}
		/// <summary>
		/// 文件类
		/// </summary>
		public static FolderService FolderService
		{
			get
			{
				return new FolderService();
			}
		}
		/// <summary>
		/// 项目类
		/// </summary>
		public static ProjectService ProjectService
		{
			get
			{
				return new ProjectService();
			}
		}

		/// <summary>
		/// 图片管理服务类
		/// </summary>
		public static UploadImageService UploadImageService
		{
			get
			{
				return new UploadImageService();
			}
		}

		/// <summary>
		/// 文件与文件夹服务类
		/// </summary>
		public static DocumentFolderService DocumentFolderService
		{
			get
			{
				return new DocumentFolderService();
			}
		}

		/// <summary>
		/// 文件标签类
		/// </summary>
		public static DocumentTagService DocumentTagService
		{
			get
			{
				return new DocumentTagService();
			}
		}

		/// <summary>
		/// 文件类
		/// </summary>
		public static DocumentService DocumentService
		{
			get
			{
				return new DocumentService();
			}
		}

		#region Common
		/// <summary>
		/// 上传图片处理服务类
		/// </summary>
		public static ImageUploadService ImageUploadService
		{
			get
			{
				return new ImageUploadService();
			}
		}
		/// <summary>
		///  下载图片处理服务类
		/// </summary>
		public static ImageDownloadService ImageDownloadService
		{
			get
			{
				return new ImageDownloadService();
			}
		}
		#endregion
	}
}
