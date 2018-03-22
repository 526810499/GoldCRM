
using System;
namespace XHD.Model
{
	/// <summary>
	/// Personal_queckmenu:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Personal_queckmenu
	{
		public Personal_queckmenu()
		{}
		#region Model
		private string _user_id;
		private string _menu_id;
		/// <summary>
		/// 
		/// </summary>
		public string user_id
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string menu_id
		{
			set{ _menu_id=value;}
			get{return _menu_id;}
		}
		#endregion Model

	}
}

