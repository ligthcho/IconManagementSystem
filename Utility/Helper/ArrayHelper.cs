using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public class ArrayHelper
	{
		/// <summary>
		/// 字符串数组转换整形数组
		/// </summary>
		/// <param name="Content">字符串数组</param>
		/// <returns></returns>
		public static int[] ToIntArray(string[] Content)
		{
			int[] c = new int[Content.Length];
			for(int i = 0;i < Content.Length;i++)
			{
				c[i] = Convert.ToInt32(Content[i].ToString());
			}
			return c;
		}

		/// <summary>
		/// 字符串转换整形数组
		/// </summary>
		/// <param name="strContent">字符串</param>
		/// <returns></returns>
		public static int[] ToIntArray(string strContent)
		{
			int[] intList = Array.ConvertAll<string,int>(strContent.Split(','),s => int.Parse(s));
			return intList;
		}
	}
}
