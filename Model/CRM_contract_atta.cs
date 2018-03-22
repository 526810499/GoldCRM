
using System;
namespace XHD.Model
{
	/// <summary>
	/// CRM_contract_atta:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_contract_atta
	{
		public CRM_contract_atta()
		{}
		#region Model
		private string _id;
		private string _contract_id;
		private string _file_name;
		private string _real_name;
		private int? _file_size;
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
		public string contract_id
		{
			set{ _contract_id=value;}
			get{return _contract_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string file_name
		{
			set{ _file_name=value;}
			get{return _file_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string real_name
		{
			set{ _real_name=value;}
			get{return _real_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? file_size
		{
			set{ _file_size=value;}
			get{return _file_size;}
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

