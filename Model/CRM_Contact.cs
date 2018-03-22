
using System;
namespace XHD.Model
{
	/// <summary>
	/// CRM_Contact:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_Contact
	{
		public CRM_Contact()
		{}
		#region Model
		private string _id;
		private string _c_name;
		private int? _c_sex;
		private string _c_department;
		private string _c_position;
		private DateTime? _c_birthday;
		private string _c_tel;
		private string _c_fax;
		private string _c_email;
		private string _c_mob;
		private string _c_qq;
		private string _c_add;
		private string _c_hobby;
		private string _c_remarks;
		private string _customer_id;
		
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
		public string C_name
		{
			set{ _c_name=value;}
			get{return _c_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? C_sex
		{
			set{ _c_sex=value;}
			get{return _c_sex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_department
		{
			set{ _c_department=value;}
			get{return _c_department;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_position
		{
			set{ _c_position=value;}
			get{return _c_position;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? C_birthday
		{
			set{ _c_birthday=value;}
			get{return _c_birthday;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_tel
		{
			set{ _c_tel=value;}
			get{return _c_tel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_fax
		{
			set{ _c_fax=value;}
			get{return _c_fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_email
		{
			set{ _c_email=value;}
			get{return _c_email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_mob
		{
			set{ _c_mob=value;}
			get{return _c_mob;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_QQ
		{
			set{ _c_qq=value;}
			get{return _c_qq;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_add
		{
			set{ _c_add=value;}
			get{return _c_add;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_hobby
		{
			set{ _c_hobby=value;}
			get{return _c_hobby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_remarks
		{
			set{ _c_remarks=value;}
			get{return _c_remarks;}
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

