
using System;
namespace XHD.Model
{
	/// <summary>
	/// hr_post:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class hr_post
	{
		public hr_post()
		{}
		#region Model
		private string _id;
		private string _post_name;
		private string _position_id;
		private string _dep_id;
		private string _emp_id;
		private int? _default_post;
		private string _note;
		private string _post_descript;
		
		private string _create_id;
		private DateTime _create_time;
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
		public string post_name
		{
			set{ _post_name=value;}
			get{return _post_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string position_id
		{
			set{ _position_id=value;}
			get{return _position_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_id
		{
			set{ _dep_id=value;}
			get{return _dep_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string emp_id
		{
			set{ _emp_id=value;}
			get{return _emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? default_post
		{
			set{ _default_post=value;}
			get{return _default_post;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string post_descript
		{
			set{ _post_descript=value;}
			get{return _post_descript;}
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
		public DateTime create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		#endregion Model

	}
}

