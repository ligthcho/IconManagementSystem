using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public class FormulaHelper
	{
		/// <summary>
		/// 获取配对符号中的内容
		/// </summary>
		/// <param name="strInput"></param>
		/// <returns></returns>
		public static List<BracketPair> GetBracketList(string strInput)
		{
			return GetBracketList(strInput,"(",")");
		}

		/// <summary>
		/// 获取配对符号中的内容
		/// </summary>
		/// <param name="strInput"></param>
		/// <returns></returns>
		public static List<BracketPair> GetBracketList(string strInput,string strLeftBracket,string strRightBracket)
		{
			if(strLeftBracket == string.Empty || strRightBracket == string.Empty)
			{
				strLeftBracket = "[";
				strRightBracket = "]";
			}
			List<BracketPair> listBracketPair = new List<BracketPair>();
			List<Bracket> listBracket = new List<Bracket>();
			bool boolStartValue = false;
			int intIndex = 0;

			//将所有的括号按照前后顺序找出来，放到一个List里边
			foreach(char charInput in strInput)
			{
				if(charInput.ToString() == strLeftBracket || charInput.ToString() == strRightBracket)
				{
					//单引号成对出现之后，才允许添加到list
					if(!boolStartValue)
					{
						listBracket.Add(new Bracket(charInput.ToString(),intIndex));
					}
				}

				intIndex++;
			}

			//如果左、右括号不是成对出现的,返回null
			if(listBracket.Count % 2 != 0)
			{
				return listBracketPair;
			}

			//获取左、右括号对
			if(listBracket.Count > 0)
			{
				//使用一个双遍历，首先获取左括号，然后查找右括号
				for(int k = 0;k < listBracket.Count;k++)
				{
					//如果是左括号
					if(listBracket[k].Code == strLeftBracket)
					{
						//考虑到括号嵌套的可能性，需要计算配对括号之间还有多少括号需要配对
						int intDistanceLeft = 0;
						int intDistanceRight = 0;

						for(int j = k + 1;j < listBracket.Count;j++)
						{
							//如果在左括号之后发现右括号之前又发现左括号，计数
							if(listBracket[j].Code == strLeftBracket)
							{
								intDistanceLeft++;
							}

							//如果发现右括号
							if(listBracket[j].Code == strRightBracket)
							{
								//检查右括号是否和左括号级次相等，如相等将左括号和右括号添加到配对表中，并跳出查找右括号的循环
								if(intDistanceRight == intDistanceLeft)
								{
									listBracketPair.Add(new BracketPair(listBracket[k],listBracket[j],strInput.Substring(listBracket[k].Index + 1,listBracket[j].Index - listBracket[k].Index - 1)));
									break;
								}
								//如果还不是匹配的右括号，计数
								intDistanceRight++;
							}
						}
					}
				}
			}

			return listBracketPair;
		}
	}

	///
	/// 定义一个符号
	///
	public class Bracket
	{
		public Bracket(string strCode,int intIndex)
		{
			Code = strCode;
			Index = intIndex;
		}

		public string Code
		{
			get;
			set;
		}
		public int Index
		{
			get;
			set;
		}

		public override string ToString()
		{
			return JsonHelper.ToJson(this);
		}
	}

	///
	/// 定义一个符号对
	///
	public class BracketPair
	{
		public BracketPair(Bracket bracketLeft,Bracket bracketRight,string strInclude)
		{
			BracketLeft = bracketLeft;
			BracketRight = bracketRight;
			Include = strInclude;
		}

		public Bracket BracketLeft
		{
			get;
			set;
		}
		public Bracket BracketRight
		{
			get;
			set;
		}
		public string Include
		{
			get;
			set;
		}

		public override string ToString()
		{
			return JsonHelper.ToJson(this);
		}
	}
}
