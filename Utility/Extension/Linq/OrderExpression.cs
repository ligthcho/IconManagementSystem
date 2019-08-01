using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utility
{
	public static class OrderExpression
	{
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source,string property)
		{
			return ApplyOrder<T>(source,property,"OrderBy");
		}
		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source,string property)
		{
			return ApplyOrder<T>(source,property,"OrderByDescending");
		}
		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source,string property)
		{
			return ApplyOrder<T>(source,property,"ThenBy");
		}
		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source,string property)
		{
			return ApplyOrder<T>(source,property,"ThenByDescending");
		}
		static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source,string property,string methodName)
		{
			string[] props = property.Split('.');
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type,"x");
			Expression expr = arg;
			foreach(string prop in props)
			{
				// use reflection (not ComponentModel) to mirror LINQ  
				PropertyInfo pi = type.GetProperty(prop);
				//if (pi == null)
				//    throw new Exception("没有找到指定名称的公共属性");
				expr = Expression.Property(expr,pi);
				type = pi.PropertyType;
			}
			Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T),type);
			LambdaExpression lambda = Expression.Lambda(delegateType,expr,arg);

			object result = typeof(Queryable).GetMethods().Single(
					method => method.Name == methodName
							&& method.IsGenericMethodDefinition
							&& method.GetGenericArguments().Length == 2
							&& method.GetParameters().Length == 2)
					.MakeGenericMethod(typeof(T),type)
					.Invoke(null,new object[] { source,lambda });
			return (IOrderedQueryable<T>)result;
		}

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source,string property,string sortOrder)
		{
			if(sortOrder.ToLower() == "asc")
				return ApplyOrder<T>(source,property,"OrderBy");
			else
				return ApplyOrder<T>(source,property,"OrderByDescending");
		}
		public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> source,string property,string sortOrder)
		{
			if(sortOrder.ToLower() == "asc")
				return ApplyOrder<T>(source,property,"ThenBy");
			else
				return ApplyOrder<T>(source,property,"ThenByDescending");
		}
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> source,Dictionary<string,string> dicOrderBy)
		{
			if(dicOrderBy != null)
			{
				var isFirst = true;
				foreach(var orderBy in dicOrderBy)
				{
					if(isFirst)
					{
						source = source.OrderBy(orderBy.Key,orderBy.Value);
						isFirst = false;
					}
					else
						source = source.ThenBy(orderBy.Key,orderBy.Value);
				}
			}
			return source;
		}
	}

}