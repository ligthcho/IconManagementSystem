using System;
using Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace Logic
{
	public class JsonLogic
	{
		private static JsonSerializerSettings JsonSerializerSettings;

        public JsonLogic()
        {
            IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
            isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            JsonSerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            JsonSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            JsonSerializerSettings.Converters.Add(isoDateTimeConverter);
        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="objJson">要序列化的对象。</param>
        /// <returns></returns>
        public static string ToJson(object objJson)
		{
			try
			{
				if(null == objJson)
				{
					return null;
				}
				return JsonConvert.SerializeObject(objJson,Formatting.None,JsonSerializerSettings);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 将指定的 JSON 数据反序列化成指定对象。
		/// </summary>
		/// <typeparam name="T">对象类型。</typeparam>
		/// <param name="strJson">JSON 数据。</param>
		/// <returns></returns>
		public static T FromJson<T>(string strJson)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(strJson,JsonSerializerSettings);
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// 将指定的 JSON 数据反序列化成指定对象。
		/// </summary>
		/// <typeparam name="T">对象类型。</typeparam>
		/// <param name="strJsonList">JSON 数组数据。</param>
		/// <returns></returns>
		public static List<T> FromJsonList<T>(string strJsonList)
		{
			List<T> listT = new List<T>();
			if(strJsonList == null)
			{
				return listT;
			}
			if(strJsonList.Equals("[]"))
			{
				return listT;
			}
			if(strJsonList.StartsWith("["))
			{
				strJsonList = strJsonList.Substring(1);
			}
			else
			{
				return listT;
			}
			if(strJsonList.EndsWith("]"))
			{
				strJsonList = strJsonList.Substring(0,strJsonList.Length - 1);
			}
			else
			{
				return listT;
			}
			if(strJsonList == string.Empty)
			{
				return listT;
			}
			List<BracketPair> listBracketPair = FormulaLogic.GetBracketList(strJsonList,"{","}");
			//string[] strJsons=System.Text.RegularExpressions.Regex.Split(strJsonList.Replace("},{","}},{{"),"},{");
			if(listBracketPair.Count < 1)
			{
				return listT;
			}
			foreach(BracketPair bracketPair in listBracketPair)
			{
				string strJson = string.Empty;
				strJson = bracketPair.BracketLeft.Code + bracketPair.Include + bracketPair.BracketRight.Code;
				if(strJson != string.Empty)
				{
					listT.Add(FromJson<T>(strJson));
				}
			}
			return listT;
		}

		/// <summary>
		/// 将指定的 JSON 数据反序列化成DataModel
		/// </summary>
		/// <param name="strJson"></param>
		/// <returns></returns>
		public static DataModel ToDataModel(string strJson)
		{
			return FromJson<DataModel>(strJson);
		}

		/// <summary>
		/// 将指定的 JSON 数据反序列化成DataModelList
		/// </summary>
		/// <param name="strJson"></param>
		/// <returns></returns>
		public static List<DataModel> ToListDataModel(string strJsonList)
		{
			List<DataModel> listDataModel = new List<DataModel>();
			if(strJsonList == null)
			{
				return listDataModel;
			}
			if(strJsonList.Equals("[]"))
			{
				return listDataModel;
			}
			if(strJsonList.StartsWith("["))
			{
				strJsonList = strJsonList.Substring(1);
			}
			else
			{
				return listDataModel;
			}
			if(strJsonList.EndsWith("]"))
			{
				strJsonList = strJsonList.Substring(0,strJsonList.Length - 1);
			}
			else
			{
				return listDataModel;
			}
			if(strJsonList == string.Empty)
			{
				return listDataModel;
			}
			List<BracketPair> listBracketPair = FormulaLogic.GetBracketList(strJsonList,"{","}");
			//string[] strJsons=System.Text.RegularExpressions.Regex.Split(strJsonList.Replace("},{","}},{{"),"},{");
			if(listBracketPair.Count < 1)
			{
				return listDataModel;
			}
			foreach(BracketPair bracketPair in listBracketPair)
			{
				string strJson = string.Empty;
				strJson = bracketPair.BracketLeft.Code + bracketPair.Include + bracketPair.BracketRight.Code;
				if(strJson != string.Empty)
				{
					listDataModel.Add(ToDataModel(strJson));
				}
			}
			return listDataModel;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jArray"></param>
		/// <returns></returns>
		public static string[] JArrayToStrings(JArray jArray)
		{
			string strStrings = string.Empty;
			for(int i=0;i<jArray.Count;i++)
			{
				strStrings+=Convert.ToInt32(jArray[i]).ToString()+"\a";
			}
			if(strStrings.EndsWith("\a"))
			{
				strStrings=strStrings.Substring(0,strStrings.Length-1);
			}
			return strStrings.Split('\a');
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jArray"></param>
		/// <returns></returns>
		public static int[] JArrayToInts(JArray jArray)
		{
			string[] strStrings=JArrayToStrings(jArray);
			int[] ints=new int[strStrings.Length];
			for(int i=0;i<strStrings.Length;i++)
			{
				ints[i]=Convert.ToInt32(strStrings[i].ToString());
			}
			return ints;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objTag"></param>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static decimal GetTagDecimal(object objTag,string strName)
		{
			if(objTag==null)
			{
				return 0;
			}
			else if(objTag.ToString().Trim()==String.Empty)
			{
				return 0;
			}
			else
			{
				try
				{
					return Convert.ToDecimal(ToDataModel(objTag.ToString())[strName]);
				}
				catch
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objTag"></param>
		/// <param name="strName"></param>
		/// <returns></returns>
		public static string GetTagString(object objTag,string strName)
		{
			if(objTag==null)
			{
				return String.Empty;
			}
			else if(objTag.ToString().Trim()==String.Empty)
			{
				return String.Empty;
			}
			else
			{
				try
				{
					return Convert.ToString(ToDataModel(objTag.ToString())[strName]);
				}
				catch
				{
					return String.Empty;
				}
			}
		}
	}
}
