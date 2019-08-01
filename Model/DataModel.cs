using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

namespace Model
{
	public class DataModel:Hashtable
	{
		private ArrayList arrayList = new ArrayList();
		public override void Add(object key,object value)
		{
			if(!base.ContainsKey(key))
			{
				base.Add(key,value);
				arrayList.Add(key);
			}
		}
		public override void Clear()
		{
			base.Clear();
			arrayList.Clear();
		}
		public override void Remove(object key)
		{
			base.Remove(key);
			arrayList.Remove(key);
		}
		public override ICollection Keys
		{
			get
			{
				return arrayList;
			}
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

		public string GetString(string strKey)
		{
			if(base.ContainsKey(strKey))
			{
				try
				{
					return base[strKey].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
			else
			{
				return string.Empty;
			}
		}

		public int GetInt(string strKey)
		{
			if(base.ContainsKey(strKey))
			{
				try
				{
					return Convert.ToInt32(base[strKey]);
				}
				catch
				{
					return 0;
				}
            }
			else
			{
				return 0;
			}
		}

		public decimal GetDecimal(string strKey)
		{
			if(base.ContainsKey(strKey))
			{
				try
				{
					return Convert.ToDecimal(base[strKey]);
				}
				catch
				{
					return 0;
				}
			}
			else
			{
				return 0;
			}
		}
	}
}
