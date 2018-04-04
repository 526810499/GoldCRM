
using System;
namespace XHD.Model
{
	/// <summary>
	/// Sys_Button:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Button
	{
		public Sys_Button()
		{}
		#region Model
		private string _btn_id;
		private string _btn_name;
		private string _btn_type;
		private string _btn_icon;
		private string _btn_handler;
		private string _menu_id;
		private string _menu_name;
		private int? _btn_order;
		private string _create_id;
		private DateTime? _create_time;

        /// <summary>
        /// 是否隐藏 隐藏的则做条件搜索权限使用
        /// </summary>
        public int isHide { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Btn_id
		{
			set{ _btn_id=value;}
			get{return _btn_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Btn_name
		{
			set{ _btn_name=value;}
			get{return _btn_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Btn_type
		{
			set{ _btn_type=value;}
			get{return _btn_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Btn_icon
		{
			set{ _btn_icon=value;}
			get{return _btn_icon;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Btn_handler
		{
			set{ _btn_handler=value;}
			get{return _btn_handler;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_id
		{
			set{ _menu_id=value;}
			get{return _menu_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_name
		{
			set{ _menu_name=value;}
			get{return _menu_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Btn_order
		{
			set{ _btn_order=value;}
			get{return _btn_order;}
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

