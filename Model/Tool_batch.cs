
using System;
namespace XHD.Model
{
	/// <summary>
	/// Tool_batch:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Tool_batch
	{
		public Tool_batch()
		{}
		#region Model
		private string _id;
		private string _batch_type;
		private string _batch_filter;
		private string _o_emp_id;
		private string _c_emp_id;
		private int? _b_count;
		
		private string _create_id;
		private DateTime? _create_time;
		/// <summary>
		/// 
		/// </summary>
		public string id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string batch_type
		{
			set{ _batch_type=value;}
			get{return _batch_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string batch_filter
		{
			set{ _batch_filter=value;}
			get{return _batch_filter;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string o_emp_id
		{
			set{ _o_emp_id=value;}
			get{return _o_emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string c_emp_id
		{
			set{ _c_emp_id=value;}
			get{return _c_emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? b_count
		{
			set{ _b_count=value;}
			get{return _b_count;}
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

