using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcWeb.Filter
{
	//登录认证特性
	public class AuthenticationAttribute:ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			//if(filterContext.HttpContext.Session["username"] == null)
			//	filterContext.Result = new RedirectToRouteResult("Login",new RouteValueDictionary { { "from",Request.Url.ToString() } });
			if(!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute),true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute),true))
			{
				var userModel = AllServices.UserService.GetUserBySession();
				if(userModel == null)
				{
					//返回首页
					filterContext.Result = new RedirectResult("/Home/Index");// new RedirectToRouteResult("Login",new RouteValueDictionary { { "from","/Task/PublishingTask" } });
				}
			}
			base.OnActionExecuting(filterContext);
		}
	}
}