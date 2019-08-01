using System;
using System.Net;
using System.Collections.Generic;

namespace Logic
{
    public class NetLogic
    {
        /// <summary>
        /// 根据计算机名称得到计算机的IP地址
        /// </summary>
        /// <param name="strHostName">传入的计算机名称</param>
        /// <returns>计算机的IP地址</returns>
        public static string GetIPAddressByHostName(string strHostName)
        {
            string strLIPAddres = string.Empty;
			string strSIPAddres = string.Empty;
            IPAddress[] ipAddressList = Dns.GetHostAddresses(strHostName.Trim());
            if (ipAddressList.Length > 1)
            {
                strLIPAddres = ipAddressList[0].ToString();
                strSIPAddres = ipAddressList[1].ToString();
            }
            else
            {
                strLIPAddres = ipAddressList[0].ToString();
                strSIPAddres = "没有可用的连接";
            }
            strLIPAddres.Trim();
            strSIPAddres.Trim();
            return strLIPAddres;
        }

        /// <summary>
        /// 根据计算机的IP地址得到计算机名称
        /// </summary>
        /// <param name="strHostName">计算机的IP地址</param>
        /// <returns>计算机名称</returns>
        public static string GetHostNameByIPAddress(string strIPAddress)
        {
            string strHostName = String.Empty;
            if (strIPAddress.IndexOf(".") != -1)
            {
                try
                {
                    IPAddress hostIPAddress = IPAddress.Parse(strIPAddress);
                    IPHostEntry hostInfo = Dns.GetHostEntry(hostIPAddress);
                    strHostName = hostInfo.HostName;
                }
                catch (Exception ex)
                {
                    string strTemp = "StackTrace：" + ex.StackTrace + "\r\nMessage：" + ex.Message;
                    strHostName = strIPAddress;
                }
            }
            else
            {
                strHostName = strIPAddress;
            }
            return strHostName;
        }
	}
}
