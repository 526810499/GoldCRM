
using System;
namespace XHD.Model
{
	/// <summary>
	/// Sys_Param_Type:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Param_Type
	{
		public Sys_Param_Type()
		{}
		#region Model
		private long _id;
		private string _params_name;
		private int? _params_order;
		private string _create_id;
		private DateTime? _create_time;
		/// <summary>
		/// 
		/// </summary>
		public long id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string params_name
		{
			set{ _params_name=value;}
			get{return _params_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? params_order
		{
			set{ _params_order=value;}
			get{return _params_order;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string create_id
		{
			set{ _create_id=value;}
			get{return _create_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		#endregion Model

	}
}

