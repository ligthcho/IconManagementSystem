using System;
using System.Collections;

namespace Model
{
	public class WhereModel:ArrayList
	{
		public WhereModel()
		{
		}

		public WhereModel(object value)
		{
			this.Clear();
			this.Add(value);
		}

		public override string ToString()
		{
			string strString = String.Empty;
			foreach(object value in this)
			{
				strString += value.ToString() + " and ";
			}

			if(strString != String.Empty)
			{
				if(strString.EndsWith(" and "))
				{
					strString = strString.Substring(0,strString.Length - 5);
				}
				strString = " where " + strString + " ";
			}

			return strString;
		}
	}
}
