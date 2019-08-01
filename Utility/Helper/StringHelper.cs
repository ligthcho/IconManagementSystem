using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public static class StringHelper
	{
		/// <summary>  
		/// 根据GUID获取16位的唯一字符串  
		/// </summary>  
		/// <returns></returns>  
		public static string GuidTo16String()
		{
			long i = 1;
			foreach(byte b in Guid.NewGuid().ToByteArray())
				i *= ((int)b + 1);
			string res = string.Format("{0:x}",i - DateTime.Now.Ticks);
			return res.PadRight(16,'0'); //如果不足16位就用0补齐
		}

		/// <summary>
		/// 获取排序号（1970/01/01 到现在的秒数）
		/// </summary>
		/// <returns></returns>
		public static long GetSortNum()
		{
			DateTime beginTime = new DateTime(1970,1,1,0,0,0,0);
			DateTime endTime = DateTime.Now;
			double second = (endTime - beginTime).TotalSeconds;
			return (long)second;
		}
		/// <summary>
		/// 生成随机数 1到100（默认值）
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static string GetRandom(int minValue = 0,int maxValue = 100)
		{
			int randomValue = Math.Abs(Guid.NewGuid().GetHashCode());
			Random r = new Random(randomValue);
			return Convert.ToString(r.Next(minValue,maxValue));
		}
	}
}
