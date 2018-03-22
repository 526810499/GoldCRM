
using System;
namespace XHD.Model
{
	/// <summary>
	/// Task:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Task
	{
		public Task()
		{}
		#region Model
		private string _id;
		private string _task_title;
		private string _task_content;
		private string _task_type_id;
		private string _customer_id;
		private string _assign_id;
		private string _executive_id;
		private DateTime? _executive_time;
		private int? _task_status_id;
		private int? _priority_id;
		private DateTime? _remind_time;
		private int? _ischeck=0;
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
		public string task_title
		{
			set{ _task_title=value;}
			get{return _task_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string task_content
		{
			set{ _task_content=value;}
			get{return _task_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string task_type_id
		{
			set{ _task_type_id=value;}
			get{return _task_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string customer_id
		{
			set{ _customer_id=value;}
			get{return _customer_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string assign_id
		{
			set{ _assign_id=value;}
			get{return _assign_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string executive_id
		{
			set{ _executive_id=value;}
			get{return _executive_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? executive_time
		{
			set{ _executive_time=value;}
			get{return _executive_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? task_status_id
		{
			set{ _task_status_id=value;}
			get{return _task_status_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Priority_id
		{
			set{ _priority_id=value;}
			get{return _priority_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? remind_time
		{
			set{ _remind_time=value;}
			get{return _remind_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isCheck
		{
			set{ _ischeck=value;}
			get{return _ischeck;}
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

