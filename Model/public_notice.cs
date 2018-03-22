
using System;
namespace XHD.Model
{
	/// <summary>
	/// public_notice:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class public_notice
	{
		public public_notice()
		{}
		#region Model
		private string _id;
		private string _notice_title;
		private string _notice_content;
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
		public string notice_title
		{
			set{ _notice_title=value;}
			get{return _notice_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string notice_content
		{
			set{ _notice_content=value;}
			get{return _notice_content;}
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

