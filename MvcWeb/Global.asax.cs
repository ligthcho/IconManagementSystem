using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Utility;

namespace MvcWeb
{
	public class MvcApplication:System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Log4Net.config")));
		}
		protected void Application_Error(object sender,EventArgs e)
		{
			Log4NetHelper.Error("程序发生未捕获的异常\t\t\t" + HttpContext.Current.Error.Message,HttpContext.Current.Error);
		}
	}
}
