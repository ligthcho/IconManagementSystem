using System.Collections.Generic;

namespace Services
{
	/// <summary>
	/// 一般操作结果的返回
	/// </summary>
	public class CommonResult
	{
		/// <summary>
		/// 操作成功
		/// </summary>
		public CommonResult()
		{
			this.code = 0;
			this.errmsg = string.Empty;
			this.data = string.Empty;
		}
		/// <summary>
		/// 操作失败
		/// </summary>
		/// <param name="errMsg">错误信息</param>
		public CommonResult(string errMsg)
		{
			this.code = -1;
			this.errmsg = errMsg;
			this.data = string.Empty;
		}
		public CommonResult(int code,string errMsg,object data)
		{
			this.code = code;
			this.errmsg = errMsg;
			this.data = data;
		}
		/// <summary>
		/// 操作成功(返回json字符串)
		/// </summary>
		/// <returns></returns>
		public static string ToJsonStr()
		{
			var instance = new CommonResult();
			return Utility.ObjExtension.ToJson(instance);
		}
		/// <summary>
		/// 操作失败(返回json字符串)
		/// </summary>
		/// <returns></returns>
		public static string ToJsonStr(string errMsg)
		{
			var instance = new CommonResult(errMsg);
			return Utility.ObjExtension.ToJson(instance);
		}
		/// <summary>
		/// 操作结果(返回json字符串)
		/// </summary>
		/// <returns></returns>
		public static string ToJsonStr(int code,string errMsg,string data)
		{
			var instance = new CommonResult(code,errMsg,data);
			return Utility.ObjExtension.ToJson(instance);
		}
		/// <summary>
		/// 操作成功(返回CommonResult实例)
		/// </summary>
		/// <returns></returns>
		public static CommonResult Instance()
		{
			return Instance(1,string.Empty,string.Empty);
		}
		/// <summary>
		/// 操作成功(返回CommonResult实例)
		/// </summary>
		/// <returns></returns>
		public static CommonResult Instance(int intSeccessUID)
		{
			return Instance(1,string.Empty,intSeccessUID);
		}
		/// <summary>
		/// 操作失败(返回CommonResult实例)
		/// </summary>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public static CommonResult Instance(string errMsg = null)
		{
			return Instance(-1,errMsg,string.Empty);
		}
		/// <summary>
		/// 操作成功(返回CommonResult实例)
		/// </summary>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public static CommonResult Instance(object data = null)
		{
			return Instance(0,string.Empty,data);
		}
		/// <summary>
		/// 操作结果(返回CommonResult实例)
		/// </summary>
		/// <param name="code"></param>
		/// <param name="errMsg"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static CommonResult Instance(int code = 0,string errMsg = null,object data = null)
		{
			return new CommonResult(code,errMsg,data);
		}
		public int code
		{
			get; set;
		}
		public string errmsg
		{
			get; set;
		}
		public object data
		{
			get; set;
		}
	}

	/// <summary>
	/// Service层获取分页数据时的返回类型
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Page<T>
	{
		public Page(int pageIndex,int pageSize,int pageCount,List<T> list)
		{
			this.PageIndex = pageIndex;//当前页码
			this.PageSize = pageSize;//每页显示记录数
			this.PageCount = pageCount;//总条数
			this.TotalPage = (this.PageCount - 1) / this.PageSize + 1;//总页数
			this.List = list;//数据
		}
		public int PageIndex
		{
			get; set;
		}
		public int PageSize
		{
			get; set;
		}
		public int PageCount
		{
			get; set;
		}
		public List<T> List
		{
			get; set;
		}
		public int TotalPage
		{
			get; set;
		}
	}
}
