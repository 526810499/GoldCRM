
using System;
namespace XHD.Model
{
	/// <summary>
	/// Sys_role_emp:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_role_emp
	{
		public Sys_role_emp()
		{}
		#region Model
		private string _roleid;
		private string _empid;
		
		/// <summary>
		/// 
		/// </summary>
		public string RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string empID
		{
			set{ _empid=value;}
			get{return _empid;}
		}
		
		#endregion Model

	}
}

