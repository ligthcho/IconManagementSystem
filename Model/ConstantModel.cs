using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class ConstantModel
	{
		/// <summary>
		/// 是/否
		/// </summary>
		public enum YesOrNo
		{
			/// <summary>
			/// 否
			/// </summary>
			No = 0,
			/// <summary>
			/// 是
			/// </summary>
			Yes = 1
		}

		/// <summary>
		/// WorkFlowStatu 的摘要说明。
		/// </summary>
		public enum WorkFlowStatu:int
		{
			/// <summary>
			/// 无状态
			/// </summary>
			NotStatu = 0,
			/// <summary>
			/// 创建
			/// </summary>
			Create = 600001,
			/// <summary>
			/// 提交
			/// </summary>
			Submit = 600002,
			/// <summary>
			/// 审批通过
			/// </summary>
			AuditPass = 600003,
			/// <summary>
			/// 退回
			/// </summary>
			AuditFail = 600004,
			/// <summary>
			/// 删除请求
			/// </summary>
			DeleteRequest = 600005,
			/// <summary>
			/// 删除通过
			/// </summary>
			Deleted = 600006
		}

		/// <summary>
		/// 查询方式。
		/// </summary>
		public enum CompareType
		{
			// 大于
			Greater = 1,
			// 等于
			Equal,
			// 小于
			Less,
			// 大于等于
			GreaterEqual,
			// 小于等于
			LessEqual,
			// 不等于
			NotEqual,
			// 包含
			Include,
			// 不包含
			NotInclude,
			// 以..开始
			StartWith,
			// 以..结束
			EndWith,
			// 本年
			ThisYear,
			// 本月
			ThisMonth,
			// 本周
			ThisWeek,
			// 本天
			ThisDay
		}

		/// <summary>
		/// Add				新建、增加
		/// Input			导入方式增加
		/// Modify			入库前修改
		/// AuditModify		审批人修改
		/// SubmitedModify	入库后修改
		/// View			查看
		/// AuditView		审批人查看
		/// SubmitedView	入库资料查看
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
			/// <summary>
			/// 多次录入
			/// </summary>
			CompareModify,
			/// <summary>
			/// 审批修改
			/// </summary>
			AuditModify,
			/// <summary>
			/// 提交修改
			/// </summary>
			SubmitedModify,
			/// <summary>
			/// 查看
			/// </summary>
			View,
			/// <summary>
			/// 审批查看
			/// </summary>
			AuditView,
			/// <summary>
			/// 提交查看
			/// </summary>
			SubmitedView
		}

		/// <summary>
		/// 是否做查重检查。
		/// </summary>
		public enum CheckDittograph
		{
			/// <summary>
			/// 不做。
			/// </summary>
			Uncheck = 0,
			/// <summary>
			/// 做。
			/// </summary>
			Check = 1
		}

		/// <summary>
		/// 查重方式。
		/// </summary>
		public enum CheckDittographRuleType
		{
			/// <summary>
			/// 模糊（包含）
			/// </summary>
			Include = 0,
			/// <summary>
			/// 完全匹配。
			/// </summary>
			Equal = 1
		}

		public enum SystemCodeClassObligeID:long
		{
			/// <summary>
			/// 码表分类。
			/// </summary>
			CodeClass = 1,

			/// <summary>
			/// 职务。
			/// </summary>
			Duty = 2,

			/// <summary>
			/// 资料状态
			/// </summary>
			InfoStatus = 3,

			/// <summary>
			/// 文件类型
			/// </summary>
			FileType = 4,

			/// <summary>
			/// 价格单位
			/// </summary>
			PriceUnit = 5,

			/// <summary>
			/// 指标。
			/// </summary>
			InfoValue = 6,

			/// <summary>
			/// 出版周期
			/// </summary>
			PublishPeriod = 7,

			/// <summary>
			/// 组织分类。
			/// </summary>
			OrgaClass = 100001,

			/// <summary>
			/// 根节点。
			/// </summary>
			BaseClass = 100004,
			//******************-->
			/// <summary>
			/// 版块。
			/// </summary>
			//InfoSpace = 10000001,
			InfoSpace = 100100100001,

			/// <summary>
			/// 版块分类（频道）-新闻
			/// </summary>
			InfoXinwen = 100100100002,
			/// <summary>
			/// 版块分类（频道）-企业黄页
			/// </summary>
			InfoHuangye = 100100100003,
			/// <summary>
			/// 版块分类（频道）-供求频道
			/// </summary>
			InfoGongqiu = 100100100004,
			/// <summary>
			/// 版块分类（频道）-商圈
			/// </summary>
			InfoShangquan = 100100100005,
			/// <summary>
			/// 版块分类（频道）-旅游(娱乐八卦)
			/// </summary>
			InfoLvyou = 100100100006,
			/// <summary>
			/// 版块分类（频道）-人文
			/// </summary>
			InfoRenwen = 100100100007,
			/// <summary>
			/// 版块分类（频道）-商品展台
			/// </summary>
			InfoShangpin = 100100100008,
			/// <summary>
			/// 版块分类（频道）-人才
			/// </summary>
			InfoRencai = 100100100009,
			/// <summary>
			/// 版块分类（频道）-友情链接
			/// </summary>
			InfoLianjie = 100100100010,
			/// <summary>
			/// 版块分类（频道）-热键
			/// </summary>
			InfoRejian = 100100100011,
			/// <summary>
			/// 版块分类（频道）-短信信息
			/// </summary>
			InfoDuanxin = 100100100012,
			/// <summary>
			/// 版块分类（频道）-卖家热推
			/// </summary>
			InfoMaijia = 100100100013,
			/// <summary>
			/// 版块分类（频道）-自然
			/// </summary>
			InfoZiran = 100100100014,
			/// <summary>
			/// 版块分类（频道）-汉江
			/// </summary>
			InfoHanjiang = 100100100015,
			/// <summary>
			/// 版块分类（频道）-评论
			/// </summary>
			InfoPinglun = 100100100016,
			/// <summary>
			/// 版块分类（频道）-生活-社区精粹
			/// </summary>
			InfoShenHuo = 100100100017,
			/// <summary>
			/// 版块分类（频道）-信息反馈
			/// </summary>
			InfoXxfankui = 100100100018,
			/// <summary>
			/// 版块分类（频道）-同城
			/// </summary>
			InfoLocal = 100100100019,
			/// <summary>
			/// 版块分类（频道）-吃喝玩乐
			/// </summary>
			InfoChihewanle = 100100100020,
			/// <summary>
			/// 表单分类。
			/// </summary>
			//FormClass = 10000002,
			FormClass = 100100200001,

			/// <summary>
			/// 表单分类--卖家热推
			/// </summary>
			FormMaijia = 100100200002,
			/// <summary>
			/// 表单分类--推荐企业
			/// </summary>
			FormTuijian = 100100200003,
			/// <summary>
			/// 表单分类--广告
			/// </summary>
			FormGuanggao = 100100200004,
			/// <summary>
			/// 表单分类--便民服务
			/// </summary>
			FormBianmin = 100100200005,
			/// <summary>
			/// 表单分类--求购信息
			/// </summary>
			FormQiugou = 100100200006,
			/// <summary>
			/// 表单分类--供应信息
			/// </summary>
			FormGongying = 100100200007,
			/// <summary>
			/// 表单分类--发布产品
			/// </summary>
			FormFabu = 100100200008,
			/// <summary>
			/// 表单分类--企业黄页
			/// </summary>
			FormHuangye = 100100200009,
			/// <summary>
			/// 表单分类--推荐产品
			/// </summary>
			FormTJchanpin = 100100200010,
			/// <summary>
			/// 表单分类--评论
			/// </summary>
			FormPinglun = 100100200011,
			/// <summary>
			/// 表单分类--公告
			/// </summary>
			FormGonggao = 100100200012,
			/// <summary>
			/// 表单分类--个性化
			/// </summary>
			FormGxinghua = 100100200013,
			/// <summary>
			/// 表单分类--酷图
			/// </summary>
			FormPicture = 100100200017,

			/// <summary>
			/// 表单分类--竞价排名
			/// </summary>
			PriceCompete = 100100200020,

			/// <summary>
			/// 表单--我的视图
			/// </summary>
			MyViewForm = 100100200014,


			//******************<--
			/// <summary>
			/// 视图类。
			/// </summary>
			ViewClass = 10000003,

			/// <summary>
			/// 资料类别。//行业分类--企业黄页(频道)
			/// </summary>
			InfoCategory = 10000004,

			/// <summary>
			/// 资料分类。//产品分类(频道)
			/// </summary>
			InfoClass = 10000005,

			/// <summary>
			/// 主题分类//网志(频道)
			/// </summary>
			InfoSubjectClass = 10000006,

			/// <summary>
			/// 资料语言
			/// </summary>
			InfoLanguage = 10000007,

			/// <summary>
			/// 来源方式
			/// </summary>
			InfoSource = 10000008,

			/// <summary>
			/// 品牌
			/// </summary>
			InfoBrand = 10000009,

			/// <summary>
			/// 地区
			/// </summary>
			InfoLocation = 10000010,

			/// <summary>
			/// 人员地区编码
			/// </summary>
			StafferLocation = 10000010,

			/// <summary>
			/// 网页自动搜索来源方式
			/// </summary>
			AutoFindSource = 10000011,

			/// <summary>
			/// 报表
			/// </summary>
			ReportForm = 10000012,

			/// <summary>
			/// 竞价模式
			/// </summary>
			PriceCompeteClassify = 1000002401,

			/// <summary>
			/// 资金类别
			/// </summary>
			FundClassify = 1000002402,

			/// <summary>
			/// 付款方式
			/// </summary>
			PayForClassify = 1000002403,

			/// <summary>
			/// 行业发展数据版块
			/// </summary>
			ProfessionInfoSpace = 10000013,

			/// <summary>
			/// 学历
			/// </summary>
			Degree = 10000014,

			/// <summary>
			/// 财务周期
			/// </summary>
			FinancePeriod = 10000015,

			/// <summary>
			/// 财务周期：年
			/// </summary>
			Year = 10000016,

			/// <summary>
			/// 财务周期：月
			/// </summary>
			Month = 10000017,

			/// <summary>
			/// 财务周期：季
			/// </summary>
			Quarter = 10000018,

			/// <summary>
			/// 财务周期：日
			/// </summary>
			Day = 10000019,

			/// <summary>
			/// 数值单位
			/// </summary>
			NumberUnit = 10000020,

			/// <summary>
			/// 行业发展数据分析
			/// </summary>
			ProfessionAnalyse = 10000021,

			/// <summary>
			/// 表单之间关系
			/// </summary>
			FrRelationOID = 20000001,

			/// <summary>
			/// 首页突出版块
			/// </summary>
			KeystoneInfoSpace = 30000005,

			/// <summary>
			/// 黄页类别分类
			/// </summary>
			HuangyeClass = 10000030,

			/// <summary>
			/// 商品类别分类
			/// </summary>
			ChanpinClass = 10000031,

			/// <summary>
			/// 网志类别分类
			/// </summary>
			PersonLogClass = 10010006,

			/// <summary>
			/// 信誉等级
			/// </summary>
			CreditStanding = 10000040,

			///重复值，去掉07-06-14 m
			//		/// <summary>
			//		/// 网志分类
			//		/// </summary>
			//		personlog = 10000006,

			/// <summary>
			/// 网志(热点)频道分类
			/// </summary>

			personloghot = 100100100011,

			/// <summary>
			/// 城市 同城社区
			/// </summary>
			CityInfoSpace = 30000005,

			/// <summary>
			/// 网志(热点)表单分类
			/// </summary>
			personloghotfrom = 100100200015,
			/// <summary>
			/// 道酷公告（表单）-道酷公告板
			/// </summary>
			CallBoard = 100100200016
		}

		/// <summary>
		/// 资料分类字段
		/// </summary>
		public enum DetailClassField
		{
			/// <summary>
			/// 企业黄页
			/// </summary>
			Info_category_oid,
			/// <summary>
			/// 产品分类
			/// </summary>
			info_classify_oid,
			/// <summary>
			/// 主题分类
			/// </summary>
			info_subjectClass_oid
		}


	}
}
