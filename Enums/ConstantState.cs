using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums
{
	/// <summary>
	/// 账户（用户、角色）状态, 0:作废,1:使用,2:禁用
	/// </summary>
	public enum AccountState
	{
		/// <summary>
		/// 启用
		/// </summary>
		Enable,
		/// <summary>
		/// 禁用
		/// </summary>
		Disable
	}
	/// <summary>
	/// 文件状态, 0-作废,10-使用,20-停用
	/// </summary>
	public enum ItemState
	{
		/// <summary>
		/// 作废
		/// </summary>
		Invalid = 0,
		/// <summary>
		/// 使用
		/// </summary>
		Enable = 10,
		/// <summary>
		/// 退返
		/// </summary>
		Return = 11,
		/// <summary>
		/// 停用
		/// </summary>
		Disable = 20
	}

	/// <summary>
	/// 动作状态
	/// </summary>
	/// </summary>
	public enum ActionType
	{
		/// <summary>
		/// 处理
		/// </summary>
		Treatment,
		/// <summary>
		/// 预览
		/// </summary>
		Show,
		/// <summary>
		/// 添加
		/// </summary>
		Add,
		/// <summary>
		/// 导入
		/// </summary>
		Input,
		/// <summary>
		/// 修改
		/// </summary>
		Modify,
	}
}
