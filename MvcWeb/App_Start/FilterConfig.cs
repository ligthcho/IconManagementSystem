using MvcWeb.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWeb
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{//特性过滤器
		   //filters.Add(new HandleErrorAttribute());
			filters.Add(new AuthenticationAttribute());
		}
	}
}