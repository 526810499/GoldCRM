
using System;
namespace XHD.Model
{
	/// <summary>
	/// hr_department:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class hr_department
	{
		public hr_department()
		{}
		#region Model
		private string _id;
		private string _dep_name;
		private string _parentid;
		private string _parentname;
		private string _dep_type;
		private string _dep_icon;
		private string _dep_chief;
		private string _dep_tel;
		private string _dep_fax;
		private string _dep_add;
		private string _dep_email;
		private string _dep_descript;
		private int? _dep_order;
		
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
		public string dep_name
		{
			set{ _dep_name=value;}
			get{return _dep_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string parentid
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string parentname
		{
			set{ _parentname=value;}
			get{return _parentname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_type
		{
			set{ _dep_type=value;}
			get{return _dep_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_icon
		{
			set{ _dep_icon=value;}
			get{return _dep_icon;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_chief
		{
			set{ _dep_chief=value;}
			get{return _dep_chief;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_tel
		{
			set{ _dep_tel=value;}
			get{return _dep_tel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_fax
		{
			set{ _dep_fax=value;}
			get{return _dep_fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_add
		{
			set{ _dep_add=value;}
			get{return _dep_add;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_email
		{
			set{ _dep_email=value;}
			get{return _dep_email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_descript
		{
			set{ _dep_descript=value;}
			get{return _dep_descript;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? dep_order
		{
			set{ _dep_order=value;}
			get{return _dep_order;}
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

