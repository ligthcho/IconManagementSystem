namespace Utility
{
	public class Filter
	{
		public Filter()
		{
		}
		public Filter(string _propertyName,Op _operation,object _value)
		{
			this.PropertyName = _propertyName;
			this.Operation = _operation;
			this.Value = _value;
		}
		public static Filter Add(string propertyName,Op operation,object value)
		{
			return new Filter(propertyName,operation,value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="operation"></param>
		/// <param name="value"></param>
		/// <param name="ignoreNullable">是否忽略可空</param>
		/// <returns></returns>
		public static Filter Add(string propertyName,Op operation,object value,bool ignoreNullable)
		{
			if((value == null || value.ToString() == string.Empty) && ignoreNullable)
				return null;
			else
				return new Filter(propertyName,operation,value);
		}
		/// <summary>
		/// 属性名
		/// </summary>
		public string PropertyName
		{
			get; set;
		}
		/// <summary>
		/// 操作符枚举
		/// </summary>
		public Op Operation
		{
			get; set;
		}
		/// <summary>
		/// 值
		/// </summary>
		public object Value
		{
			get; set;
		}
	}

	/// <summary>
	/// 操作符枚举
	/// </summary>
	public enum Op
	{
		/// <summary>
		/// 等于
		/// </summary>
		Equals,
		/// <summary>
		/// 不等于
		/// </summary>
		NotEqual,
		/// <summary>
		/// 大于
		/// </summary>
		GreaterThan,
		/// <summary>
		/// 小于
		/// </summary>
		LessThan,
		/// <summary>
		/// 大于或等于
		/// </summary>
		GreaterThanOrEqual,
		/// <summary>
		/// 小于或等于
		/// </summary>
		LessThanOrEqual,
		/// <summary>
		/// 包含
		/// </summary>
		Contains,
		/// <summary>
		/// 开头包含
		/// </summary>
		StartsWith,
		/// <summary>
		/// 结尾包含
		/// </summary>
		EndsWith
	}
}
