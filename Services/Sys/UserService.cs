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

namespace Services
{
	public class UserService
	{
		public readonly UIDBEntities db = new Models.UIDBEntities();
		/// <summary>
		/// 用户登录
		/// </summary>
		/// <param name="strUserName"></param>
		/// <param name="strPassword"></param>
		/// <returns></returns>
		public CommonResult Login(string strUserName,string strPassword)
		{
			//if(strUserName.ToLower() == AllConfigServices.SettingsConfig.SuperAdminAccount && strPassword == AllConfigServices.SettingsConfig.SuperAdminPassword)
			//{
				SysFormsAuthenticationHelper<Models.User>.SetAuthSession(strUserName,new User()
				{
					//UserID = strUserName
					UserName = strUserName,
					Password = strPassword
				});
				//return CommonResult.Instance();
			//}
			 
			strPassword = strPassword.ToMD5();
			var userInfo = db.User.Where(u => u.UserName == strUserName && u.Password == strPassword).FirstOrDefault();
			if(userInfo != null)
			{
				int intState = (int)ItemState.Disable;
				if(userInfo.State == intState)
				{
					return CommonResult.Instance("账号被禁用，请联系管理员");
				}
				else
				{
					var q = from permission in db.Permission
							join userPermission in db.UserPermission on permission.PermissionNo equals userPermission.PermissionID
							join user in db.User on userPermission.UserNo equals user.UserNo
							where user.UserNo == userInfo.UserNo
							select permission;
					var listRole = q.ToList();
					if(listRole.Where(lisRole => lisRole.State == intState).Count() > 0)
					{
						return CommonResult.Instance("账号所属角色被禁用，请联系管理员");
					}
				}
			}
			else
			{
				return CommonResult.Instance("账号或密码错误");
			}
			//return CommonResult.Instance();
			return CommonResult.Instance(1,"登陆成功",userInfo);
		}
		/// <summary>
		/// 用户登出
		/// </summary>
		public void Logout()
		{
			SysFormsAuthenticationHelper<User>.SignOut();
		}
		/// <summary>
		/// 是否已登录
		/// </summary>
		/// <returns></returns>
		public bool IsLogined()
		{
			string strUserId = SysFormsAuthenticationHelper<User>.GetUserId();
			return !string.IsNullOrEmpty(strUserId);
		}
		/// <summary>
		/// 从客户端cookie中获取用户信息
		/// </summary>
		/// <returns></returns>
		public User GetUserByCookie()
		{
			return SysFormsAuthenticationHelper<User>.GetUserInstance();
		}
		/// <summary>
		/// 从服务端session中获取用户信息
		/// </summary>
		/// <returns></returns>
		public User GetUserBySession()
		{
			return SysFormsAuthenticationHelper<User>.GetUserSession();
		}
		/// <summary>
		/// 根据UserName获取用户信息
		/// </summary>
		/// <param name="strUserName"></param>
		/// <returns></returns>
		public User GetByUserID(string strUserName)
		{
			return db.User.Where(user => user.UserName == strUserName).FirstOrDefault();
		}
		/// <summary>
		/// 加载分页数据
		/// </summary>
		/// <param name="intPageIndex">当前页码</param>
		/// <param name="intPageSize">每页显示记录数</param>
		/// <param name="userWhereLambda"></param>
		/// <param name="permissionWhereLambda"></param>
		/// <param name="dicOrderBy"></param>
		/// <returns></returns>
		public Page<User> GetByPage(int intPageIndex,int intPageSize,Expression<Func<User,bool>> userWhereLambda = null,Expression<Func<Permission,bool>> permissionWhereLambda = null,Dictionary<string,string> dicOrderBy = null)
		{
			if(userWhereLambda == null)
			{
				userWhereLambda = u => 1 == 1;
			}
			if(permissionWhereLambda == null)
			{
				permissionWhereLambda = u => 1 == 1;
			}
			var q = db.User.Where(userWhereLambda).OrderBy(dicOrderBy);
			var list = q.Skip((intPageIndex - 1) * intPageSize).Take(intPageSize).ToList();

			return new Page<User>(intPageIndex,intPageSize,q.Count(),list);
		}
	}
}
