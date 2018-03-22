
using System;
namespace XHD.Model
{
	/// <summary>
	/// Sys_authority:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_authority
	{
		public Sys_authority()
		{}
		#region Model
		private string _role_id;
		private string _app_id;
		private int? _auth_type;
		private string _auth_id;
		
		private string _create_id;
		private DateTime? _create_time;
		/// <summary>
		/// 
		/// </summary>
		public string Role_id
		{
			set{ _role_id=value;}
			get{return _role_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string App_id
		{
			set{ _app_id=value;}
			get{return _app_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Auth_type
		{
			set{ _auth_type=value;}
			get{return _auth_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Auth_id
		{
			set{ _auth_id=value;}
			get{return _auth_id;}
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

