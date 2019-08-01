using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic;

namespace Logic
{
	/// <summary>
	/// 加解密、编解码、字符处理、数制转换
	/// </summary>
	public class CodeLogic
	{
		#region 读取配置
		/*
		<!--┏Code┓-->
		<!--AES Key>16位，IV>16位-->
		<add key="CodeAESKey" value="1234567890123456"/>
		<add key="CodeAESIV"  value="1234567890123456"/>
		<!--DES Key>8位，IV>8位-->
		<add key="CodeDESKey" value="12345678"/>
		<add key="CodeDESIV"  value="12345678"/>
		<!--┗Code┛-->
		*/
		public static string CODE_DES_KEY = ConfigLogic.GetString("CodeDESKey");
		public static string CODE_DES_IV = ConfigLogic.GetString("CodeDESIV");

		public static string CODE_AES_KEY = ConfigLogic.GetString("CodeAESKey");
		public static string CODE_AES_IV = ConfigLogic.GetString("CodeAESIV");
		#endregion 读取配置

		#region 加密解密
		#region MD5
		/// <summary>
		/// The Encrypt method encrypts a clean string into hashed string
		/// MD5将传入的字符串进行MD5加密，然后进行Hash
		/// </summary>
		/// <param name="strClean"></param>
		/// <returns></returns>
		public static string EncryptMD5Hash(string strClean)
		{
			Byte[] bytesClear=new UnicodeEncoding().GetBytes(strClean);
			Byte[] bytesHash=((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytesClear);
			return BitConverter.ToString(bytesHash);
		}

		/// <summary>
		/// MD5将传入的字符串进行MD5加密
		/// </summary>
		/// <param name="strClean">传入的字符串</param>
		/// <returns>MD5加密过的字符串</returns>
		public static string EncryptMD5(string strClean)
		{
			//MD5CryptoServiceProvider mD5CryptoServiceProvider=new MD5CryptoServiceProvider();
			//return BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(UTF8Encoding.Default.GetBytes(strClean))).Replace("-",string.Empty);

			StringBuilder stringBuilder=new StringBuilder();
			using(MD5 md5=MD5.Create())
			{
				byte[] bytesClear=Encoding.UTF8.GetBytes(strClean);//UTF8Encoding.Default.GetBytes(msg);//Encoding.UTF8.GetBytes(msg);
				byte[] bytesHash=md5.ComputeHash(bytesClear);
				foreach(byte item in bytesHash)
				{
					stringBuilder.Append(item.ToString("x2"));
				}
			}
			return stringBuilder.ToString().ToUpper();
		}
		#endregion MD5

		#region SHA1
		/// <summary>
		/// The Encrypt method encrypts a clean string into hashed string
		/// SHA1将传入的字符串进行SHA1加密，然后进行Hash
		/// </summary>
		/// <param name="strClean"></param>
		/// <returns></returns>
		public static string EncryptSHA1Hash(string strClean)
		{
			Byte[] bytesClear=new UnicodeEncoding().GetBytes(strClean);
			Byte[] bytesHash=((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(bytesClear);
			return BitConverter.ToString(bytesHash);
		}
		/// <summary>
		/// SHA1将传入的字符串进行SHA1加密
		/// </summary>
		/// <param name="strClean">传入的字符串</param>
		/// <returns>SHA1加密过的字符串</returns>
		public static string EncryptSHA1(string strClean)
		{
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider=new SHA1CryptoServiceProvider();
			return BitConverter.ToString(sHA1CryptoServiceProvider.ComputeHash(UTF8Encoding.Default.GetBytes(strClean))).Replace("-",string.Empty);
		}
		#endregion SHA1

		#region DES
		/// <summary>
		/// 使用缺省的密钥和偏移量进行DES加密。
		/// </summary>
		/// <param name="strToEncrypt">要加密的字符串。</param>
		/// <returns>以Base64格式返回的加密字符串。</returns>
		public static string EncryptDES(string strToEncrypt)
		{
			return EncryptDES(strToEncrypt,CODE_DES_KEY,CODE_DES_IV);
		}

		/// <summary>
		/// 使用缺省的偏移量进行DES加密。
		/// </summary>
		/// <param name="strToEncrypt">要加密的字符串。</param>
		/// <param name="strKey">密钥，且必须为8位。</param>
		/// <returns>以Base64格式返回的加密字符串。</returns>
		public static string EncryptDES(string strToEncrypt,string strKey)
		{
			return EncryptDES(strToEncrypt,strKey,CODE_DES_IV);
		}

		/// <summary>
		/// 进行DES加密。
		/// </summary>
		/// <param name="strToEncrypt">要加密的字符串。</param>
		/// <param name="strKey">密钥，且必须为8位。</param>
		/// <param name="strIV">偏移量，且必须为8位。</param>
		/// <returns>以Base64格式返回的加密字符串。</returns>
		public static string EncryptDES(string strToEncrypt,string strKey,string strIV)
		{
			using(DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				byte[] byteInputByteArray = Encoding.UTF8.GetBytes(strToEncrypt);
				dESCryptoServiceProvider.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
				dESCryptoServiceProvider.IV = ASCIIEncoding.ASCII.GetBytes(strIV);
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				using(CryptoStream cryptoStream = new CryptoStream(memoryStream,dESCryptoServiceProvider.CreateEncryptor(),CryptoStreamMode.Write))
				{
					cryptoStream.Write(byteInputByteArray,0,byteInputByteArray.Length);
					cryptoStream.FlushFinalBlock();
					cryptoStream.Close();
				}
				string strEncrypt = Convert.ToBase64String(memoryStream.ToArray());
				memoryStream.Close();
				return strEncrypt;
			}
		}

		/// <summary>
		/// 使用缺省的密钥和偏移量进行DES解密。
		/// </summary>
		/// <param name="strToDecrypt">要解密的以Base64</param>
		/// <returns>已解密的字符串。</returns>
		public static string DecryptDES(string strToDecrypt)
		{
			return DecryptDES(strToDecrypt,CODE_DES_KEY,CODE_DES_IV);
		}

		/// <summary>
		/// 使用缺省的偏移量进行DES解密。
		/// </summary>
		/// <param name="strToDecrypt">要解密的以Base64</param>
		/// <param name="strKey">密钥，且必须为8位。</param>
		/// <returns>已解密的字符串。</returns>
		public static string DecryptDES(string strToDecrypt,string strKey)
		{
			return DecryptDES(strToDecrypt,strKey,CODE_DES_IV);
		}

		/// <summary>
		/// 进行DES解密。
		/// </summary>
		/// <param name="strToDecrypt">要解密的以Base64</param>
		/// <param name="strKey">密钥，且必须为8位。</param>
		/// <param name="strIV">偏移量，且必须为8位。</param>
		/// <returns>已解密的字符串。</returns>
		public static string DecryptDES(string strToDecrypt,string strKey,string strIV)
		{
			byte[] byteInputByteArray = Convert.FromBase64String(strToDecrypt);
			using(DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider())
			{
				dESCryptoServiceProvider.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
				dESCryptoServiceProvider.IV = ASCIIEncoding.ASCII.GetBytes(strIV);
				System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
				using(CryptoStream cryptoStream = new CryptoStream(memoryStream,dESCryptoServiceProvider.CreateDecryptor(),CryptoStreamMode.Write))
				{
					cryptoStream.Write(byteInputByteArray,0,byteInputByteArray.Length);
					cryptoStream.FlushFinalBlock();
					cryptoStream.Close();
				}
				string strDecrypt = Encoding.UTF8.GetString(memoryStream.ToArray());
				memoryStream.Close();
				return strDecrypt;
			}
		}
		#endregion DES

		#region AES
		/// <summary>
		/// AES加密
		/// </summary>
		/// <param name="strToEncrypt">被加密的明文</param>
		/// <returns>密文</returns>
		public static string EncryptAES(string strToEncrypt)
		{
			return EncryptAES(strToEncrypt,CODE_AES_KEY,CODE_AES_IV);
		}

		/// <summary>
		/// AES加密
		/// </summary>
		/// <param name="strToEncrypt">被加密的明文</param>
		/// <param name="strKey">密钥</param>
		/// <returns>密文</returns>
		public static string EncryptAES(string strToEncrypt,string strKey)
		{
			return EncryptAES(strToEncrypt,strKey,CODE_AES_IV);
		}
		/// <summary>
		/// AES加密
		/// </summary>
		/// <param name="strToEncrypt">被加密的明文</param>
		/// <param name="strKey">密钥</param>
		/// <param name="strIV">向量</param>
		/// <returns>密文</returns>
		public static string EncryptAES(string strToEncrypt,string strKey,string strIV)
		{
			byte[] byteKeyArray = UTF8Encoding.UTF8.GetBytes(strKey);
			byte[] byteIVArray = UTF8Encoding.UTF8.GetBytes(strIV);
			byte[] byteToEncryptArray = Encoding.UTF8.GetBytes(strToEncrypt);

			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Key = byteKeyArray;
			rijndaelManaged.IV = byteIVArray;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;

			ICryptoTransform iCryptoTransform = rijndaelManaged.CreateEncryptor();
			byte[] byteResultArray = iCryptoTransform.TransformFinalBlock(byteToEncryptArray,0,byteToEncryptArray.Length);

			return Convert.ToBase64String(byteResultArray,0,byteResultArray.Length);
		}

		/// <summary>
		/// AES解密
		/// </summary>
		/// <param name="strToDecrypt">被解密的密文</param>
		/// <returns>明文</returns>
		public static string DecryptAES(string strToDecrypt)
		{
			return DecryptAES(strToDecrypt,CODE_AES_KEY,CODE_AES_IV);
		}

		/// <summary>
		/// AES解密
		/// </summary>
		/// <param name="strToDecrypt">被解密的密文</param>
		/// <param name="strKey">密钥</param>
		/// <returns>明文</returns>
		public static string DecryptAES(string strToDecrypt,string strKey)
		{
			return DecryptAES(strToDecrypt,strKey,CODE_AES_IV);
		}

		/// <summary>
		/// AES解密
		/// </summary>
		/// <param name="strToDecrypt">被解密的密文</param>
		/// <param name="strKey">密钥</param>
		/// <param name="strIV">向量</param>
		/// <returns>明文</returns>
		public static string DecryptAES(string strToDecrypt,string strKey,string strIV)
		{
			byte[] byteKeyArray = UTF8Encoding.UTF8.GetBytes(strKey);
			byte[] byteIVArray = UTF8Encoding.UTF8.GetBytes(strIV);
			byte[] byteToEncryptArray = Convert.FromBase64String(strToDecrypt);

			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Key = byteKeyArray;
			rijndaelManaged.IV = byteIVArray;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;

			ICryptoTransform iCryptoTransform = rijndaelManaged.CreateDecryptor();
			byte[] byteResultArray = iCryptoTransform.TransformFinalBlock(byteToEncryptArray,0,byteToEncryptArray.Length);

			return UTF8Encoding.UTF8.GetString(byteResultArray);
		}
		#endregion AES
		#endregion 加密解密

		#region 字符串编码
		/// <summary>  
		/// 字符串转为Html 10进制 ASCII码 字符串
		/// </summary>  
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string StringToHtmlASCII10(string strText)
		{
			char[] charbuffers = strText.ToCharArray();
			byte[] buffer;
			StringBuilder stringBuilderReturn = new StringBuilder();
			for(int i = 0;i < charbuffers.Length;i++)
			{
				buffer = System.Text.Encoding.ASCII.GetBytes(charbuffers[i].ToString());
				stringBuilderReturn.Append(String.Format("&#{0};",buffer[0]));
			}
			return stringBuilderReturn.ToString();
		}

		/// <summary>  
		/// Html 10进制 ASCII 字符串转为正常字符串
		/// </summary>  
		/// <param name="strAscii10"></param>
		/// <returns></returns>
		public static string HtmlASCII10ToString(string strAscii10)
		{
			string strReturn = string.Empty;
			try
			{
				foreach(string str in strAscii10.Split(';'))
				{
					if(str.Trim() != string.Empty)
					{
						strReturn += ((char)Convert.ToInt32(str.Replace("&#",string.Empty))).ToString();
					}
				}
			}
			catch
			{
				strReturn = string.Empty;
			}
			return strReturn;
		}

		/// <summary>
		/// 对 URL 字符串按照为使用Little-Endian字节顺序的UTF-32格式进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlEncodeToUTF32(string strText)
		{
			return UrlEncode(strText,System.Text.Encoding.UTF32);
		}

		/// <summary>
		/// 对 URL 字符串按照为使用Little-Endian字节顺序的UTF-16格式进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlEncodeToUnicode(string strText)
		{
			return UrlEncode(strText,System.Text.Encoding.Unicode);
		}

		/// <summary>
		/// 对 URL 字符串按照为UTF-8格式进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlEncodeToUTF8(string strText)
		{
			return UrlEncode(strText,System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// 对 URL 字符串按照为UTF-7格式进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlEncodeToUTF7(string strText)
		{
			return UrlEncode(strText,System.Text.Encoding.UTF7);
		}

		/// <summary>
		/// 对 URL 字符串按照为ASCII(7位)字符集进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlEncodeToASCII(string strText)
		{
			return UrlEncode(strText,System.Text.Encoding.ASCII);
		}

		/// <summary>
		/// 对 URL 字符串按照系统的当前ANSI代码页进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlEncodeToDefault(string strText)
		{
			return UrlEncode(strText,System.Text.Encoding.Default);
		}

		/// <summary>
		/// 对 URL 字符串按照GB2312（简体中文编码表）进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlEncodeToGB2312(string strText)
		{
			return UrlEncode(strText,System.Text.Encoding.GetEncoding("GB2312"));
		}

		/// <summary>
		/// 使用指定的编码对象对 URL 字符串进行编码
		/// </summary>
		/// <param name="strText"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string UrlEncode(string strText,System.Text.Encoding encoding)
		{
			return System.Web.HttpUtility.UrlEncode(strText,encoding);
		}
		#endregion 字符串编码

		#region 字符串解码
		/// <summary>
		/// 字符串按照使用Little-Endian字节顺序的UTF-32格式进行解码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlDecodeByUTF32(string strText)
		{
			return System.Web.HttpUtility.UrlDecode(strText,System.Text.Encoding.UTF32);
		}

		/// <summary>
		/// 字符串按照使用Little-Endian字节顺序的UTF-16格式进行解码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlDecodeByUnicode(string strText)
		{
			return System.Web.HttpUtility.UrlDecode(strText,System.Text.Encoding.Unicode);
		}

		/// <summary>
		/// 字符串按照UTF-8格式进行解码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlDecodeByUTF8(string strText)
		{
			return System.Web.HttpUtility.UrlDecode(strText,System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// 字符串按照UTF-7格式进行解码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlDecodeByUTF7(string strText)
		{
			return System.Web.HttpUtility.UrlDecode(strText,System.Text.Encoding.UTF7);
		}

		/// <summary>
		/// 字符串按照ASCII(7位)字符集进行解码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlDecodeByASCII(string strText)
		{
			return System.Web.HttpUtility.UrlDecode(strText,System.Text.Encoding.ASCII);
		}

		/// <summary>
		/// 字符串按照系统的当前ANSI代码页进行解码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlDecodeByDefault(string strText)
		{
			return System.Web.HttpUtility.UrlDecode(strText,System.Text.Encoding.Default);
		}

		/// <summary>
		/// 字符串按照GB2312（简体中文编码表）进行解码
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string UrlDecodeByGB2312(string strText)
		{
			return System.Web.HttpUtility.UrlDecode(strText,System.Text.Encoding.GetEncoding("GB2312"));
		}

		/// <summary>
		/// 使用指定的编码对象将 URL 编码的字符串转换为已解码的字符串
		/// </summary>
		/// <param name="strText"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string UrlDecode(string strText,System.Text.Encoding encoding)
		{
			return System.Web.HttpUtility.UrlDecode(strText,encoding);
		}
		#endregion 字符串解码

		#region 亚洲字符串处理
		/// <summary>
		/// 简体中文字符串转换为繁体中文（Traditional Chinese）
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToTraditionalChinese(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.TraditionalChinese,0);
		}

		/// <summary>
		/// 繁体中文字符串转换为简体中文（Simplified Chinese）
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToSimplifiedChinese(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.SimplifiedChinese,0);
		}

		/// <summary>
		/// 中文字符（汉字）转换为英文字母表示的拼音
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToPhoneticize(string strText)
		{
			return PinyinLogic.Convert(strText);
		}

		/// <summary>
		/// 字符串中的窄（半角单字节）字符转换为宽（全角双字节）字符（适用于亚洲区域设置）
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToWide(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.Wide,0);
		}

		/// <summary>
		/// 字符串中的宽（全角双字节）字符转换为窄（半角单字节）字符（适用于亚洲区域设置）
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToNarrow(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.Narrow,0);
		}

		/// <summary>
		/// 字符串中的平假名字符转换为片假名字符（适用于日文区域设置）
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToKatakana(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.Katakana,0);
		}

		/// <summary>
		/// 字符串中的片假名字符转换为平假名字符（适用于日文区域设置）
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToHiragana(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.Hiragana,0);
		}
		#endregion 亚洲字符串处理

		#region 西文字母、单词处理
		/// <summary>
		/// 字符串字母转换为大写形式
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToUppercase(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.Uppercase,0);
		}

		/// <summary>
		/// 字符串字母转换为小写形式
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToLowercase(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.Lowercase,0);
		}

		/// <summary>
		/// 字符串中每个单词的首字母转换为大写
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string ToProperCase(string strText)
		{
			return Microsoft.VisualBasic.Strings.StrConv(strText,VbStrConv.ProperCase,0);
		}
		#endregion 西文字母、单词处理

		#region 数制转换
		/// <summary>
		/// 将十六进制数字转换为十进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText16To10(string strText)
		{
			return NumberTextConvert(strText,16,10);
		}

		/// <summary>
		/// 将十六进制数字转换为八进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText16To8(string strText)
		{
			return NumberTextConvert(strText,16,8);
		}

		/// <summary>
		/// 将十六进制数字转换为二进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText16To2(string strText)
		{
			return NumberTextConvert(strText,16,2);
		}

		/// <summary>
		/// 将八进制数字转换为十六进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText8To16(string strText)
		{
			return NumberTextConvert(strText,8,16);
		}

		/// <summary>
		/// 将八进制数字转换为十进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText8To10(string strText)
		{
			return NumberTextConvert(strText,8,10);
		}

		/// <summary>
		/// 将八进制数字转换为二进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText8To2(string strText)
		{
			return NumberTextConvert(strText,8,2);
		}

		/// <summary>
		/// 将十进制数字转换为十六进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText10To16(string strText)
		{
			return NumberTextConvert(strText,10,16);
		}

		/// <summary>
		/// 将十进制数字转换为八进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText10To8(string strText)
		{
			return NumberTextConvert(strText,10,8);
		}

		/// <summary>
		/// 将十进制数字转换为二进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText10To2(string strText)
		{
			return NumberTextConvert(strText,10,2);
		}

		/// <summary>
		/// 将二进制数字转换为十六进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText2To16(string strText)
		{
			return NumberTextConvert(strText,2,16);
		}

		/// <summary>
		/// 将二进制数字转换为十进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText2To10(string strText)
		{
			return NumberTextConvert(strText,2,10);
		}

		/// <summary>
		/// 将二进制数字转换为八进制数字
		/// </summary>
		/// <param name="strText"></param>
		/// <returns></returns>
		public static string NumberText2To8(string strText)
		{
			return NumberTextConvert(strText,2,8);
		}

		/// <summary>
		/// 数制转换
		/// </summary>
		/// <param name="strText"></param>
		/// <param name="intFromBase"></param>
		/// <param name="intToBase"></param>
		/// <returns></returns>
		public static string NumberTextConvert(string strText,int intFromBase,int intToBase)
		{
			long longValue = Convert.ToInt64(strText,intFromBase);
			return Convert.ToString(longValue,intToBase);
		}
		#endregion 数制转换
	}
}
