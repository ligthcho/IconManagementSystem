using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
	public class ProxyModel
	{
		/// <summary>
		/// 代理服务器地址
		/// </summary>
		private String m_ProxyAddress=String.Empty;//ProxyAddress

		/// <summary>
		/// 代理服务器地址属性
		/// </summary>
		public String ProxyAddress
		{
			get
			{
				return m_ProxyAddress;
			}
			set
			{
				m_ProxyAddress=value;
			}
		}

		/// <summary>
		/// 代理服务器端口
		/// </summary>
		private int m_ProxyPort=80;//ProxyPort

		/// <summary>
		/// 代理服务器端口属性
		/// </summary>
		public int ProxyPort
		{
			get
			{
				return m_ProxyPort;
			}
			set
			{
				m_ProxyPort=value;
			}
		}

		/// <summary>
		/// 代理服务器账户
		/// </summary>
		private String m_ProxyUserName=String.Empty;//ProxyUserName

		/// <summary>
		/// 代理服务器账户属性
		/// </summary>
		public String ProxyUserName
		{
			get
			{
				return m_ProxyUserName;
			}
			set
			{
				m_ProxyUserName=value;
			}
		}

		/// <summary>
		/// 代理服务器密码
		/// </summary>
		private String m_ProxyPassword=String.Empty;//ProxyPassword

		/// <summary>
		/// 代理服务器密码属性
		/// </summary>
		public String ProxyPassword
		{
			get
			{
				return m_ProxyPassword;
			}
			set
			{
				m_ProxyPassword=value;
			}
		}

		/// <summary>
		/// 代理服务器用户名密码关联的域
		/// </summary>
		private String m_ProxyDomain=String.Empty;//ProxyDomain

		/// <summary>
		/// 代理服务器用户名密码关联的域属性
		/// </summary>
		public String ProxyDomain
		{
			get
			{
				return m_ProxyDomain;
			}
			set
			{
				m_ProxyDomain=value;
			}
		}
	}

}
