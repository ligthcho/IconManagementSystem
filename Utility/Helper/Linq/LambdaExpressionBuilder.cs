using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Utility
{
	public static class LambdaExpressionBuilder
	{
		private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
		private static MethodInfo startsWithMethod =
								typeof(string).GetMethod("StartsWith",new Type[] { typeof(string) });
		private static MethodInfo endsWithMethod =
								typeof(string).GetMethod("EndsWith",new Type[] { typeof(string) });
		private static Expression GetExpression(ParameterExpression param,Filter filter)
		{
			MemberExpression member = Expression.Property(param,filter.PropertyName);
			Expression handledMember = member;
			ConstantExpression constant = Expression.Constant(filter.Value);

			if(handledMember.Type != filter.Value.GetType())
			{
				if(handledMember.Type == typeof(Int16) ||
					handledMember.Type == typeof(Int32) ||
					handledMember.Type == typeof(Int64))
					constant = Expression.Constant(Convert.ToInt32(filter.Value));
			}

			if(member.Member.MemberType == MemberTypes.Property)
			{
				Type propertyType = ((PropertyInfo)member.Member).PropertyType;
				if(propertyType == typeof(string))
				{
					handledMember = Expression.Call(member,typeof(string).GetMethod("ToLower",System.Type.EmptyTypes));
				}
				if(propertyType == typeof(DateTime?))
				{
					handledMember = Expression.Property(member,typeof(DateTime?).GetProperty("Value"));
				}
				if(propertyType == typeof(int?))
				{
					handledMember = Expression.Property(member,typeof(int?).GetProperty("Value"));
				}
				if(propertyType == typeof(Nullable<bool>))
				{
					handledMember = Expression.Property(member,typeof(Nullable<bool>).GetProperty("Value"));
				}
			}

			switch(filter.Operation)
			{
				case Op.Equals:
					return Expression.Equal(handledMember,constant);
				case Op.NotEqual:
					return Expression.NotEqual(handledMember,constant);
				case Op.GreaterThan:
					return Expression.GreaterThan(handledMember,constant);
				case Op.GreaterThanOrEqual:
					return Expression.GreaterThanOrEqual(handledMember,constant);
				case Op.LessThan:
					return Expression.LessThan(handledMember,constant);
				case Op.LessThanOrEqual:
					return Expression.LessThanOrEqual(handledMember,constant);
				case Op.Contains:
					return Expression.Call(handledMember,containsMethod,constant);
				case Op.StartsWith:
					return Expression.Call(handledMember,startsWithMethod,constant);
				case Op.EndsWith:
					return Expression.Call(handledMember,endsWithMethod,constant);
			}

			return null;
		}

		//public static Expression<Func<T, bool>> GetExpression<T>(Filter filter)
		//{
		//    ParameterExpression param = Expression.Parameter(typeof(T), "t");
		//    var exp = GetExpression(param, filter);
		//    return Expression.Lambda<Func<T, bool>>(exp, param);
		//}

		public static Expression<Func<T,bool>> GetExpressionByAndAlso<T>(List<Filter> filters)
		{
			ParameterExpression param = Expression.Parameter(typeof(T),"t");
			Expression exp = null;

			for(var i = 0;i < filters.Count;i++)
			{
				if(filters[i] != null)
				{
					if(exp == null)
					{
						exp = GetExpression(param,filters[i]);
					}
					else
					{
						exp = Expression.AndAlso(exp,GetExpression(param,filters[i]));
					}
				}
			}

			return exp == null ? null : Expression.Lambda<Func<T,bool>>(exp,param);
		}

		public static Expression<Func<T,bool>> GetExpressionByOr<T>(List<Filter> filters)
		{
			ParameterExpression param = Expression.Parameter(typeof(T),"t");
			Expression exp = null;

			for(var i = 0;i < filters.Count;i++)
			{
				if(filters[i] != null)
				{
					if(exp == null)
					{
						exp = GetExpression(param,filters[i]);
					}
					else
					{
						exp = Expression.Or(exp,GetExpression(param,filters[i]));
					}
				}
			}

			return exp == null ? null : Expression.Lambda<Func<T,bool>>(exp,param);
		}
	}
}
