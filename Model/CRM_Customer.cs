
using System;
namespace XHD.Model
{
	/// <summary>
	/// CRM_Customer:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_Customer
	{
		public CRM_Customer()
		{}
		#region Model
		private string _id;
		private string _serialnumber;
		private string _cus_name;
		private string _cus_add;
		private string _cus_tel;
		private string _cus_fax;
		private string _cus_website;
		private string _cus_industry_id;
		private string _provinces_id;
		private string _city_id;
		private string _cus_type_id;
		private string _cus_level_id;
		private string _cus_source_id;
		private string _descripe;
		private string _remarks;
		private string _emp_id;
		private int? _isprivate;
		private DateTime? _lastfollow;
		private string _xy;
        private string _cus_extend;
        private int? _isdelete;
		private DateTime? _delete_time;
		
		private string _create_id;
		private DateTime? _create_time;

        /// <summary>
        /// 用户生日
        /// </summary>
        public DateTime? birthday { get; set; }


        /// <summary>
        /// 创建部门
        /// </summary>
        public string createdep_id { get; set; }

        /// <summary>
        /// 用户积分
        /// </summary>
        public int integral { get; set; }

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
		public string Serialnumber
		{
			set{ _serialnumber=value;}
			get{return _serialnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_name
		{
			set{ _cus_name=value;}
			get{return _cus_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_add
		{
			set{ _cus_add=value;}
			get{return _cus_add;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_tel
		{
			set{ _cus_tel=value;}
			get{return _cus_tel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_fax
		{
			set{ _cus_fax=value;}
			get{return _cus_fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_website
		{
			set{ _cus_website=value;}
			get{return _cus_website;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_industry_id
		{
			set{ _cus_industry_id=value;}
			get{return _cus_industry_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Provinces_id
		{
			set{ _provinces_id=value;}
			get{return _provinces_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string City_id
		{
			set{ _city_id=value;}
			get{return _city_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_type_id
		{
			set{ _cus_type_id=value;}
			get{return _cus_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_level_id
		{
			set{ _cus_level_id=value;}
			get{return _cus_level_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string cus_source_id
		{
			set{ _cus_source_id=value;}
			get{return _cus_source_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DesCripe
		{
			set{ _descripe=value;}
			get{return _descripe;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 业务员
		/// </summary>
		public string emp_id
		{
			set{ _emp_id=value;}
			get{return _emp_id;}
		}
        /// <summary>
        /// 业务部门
        /// </summary>
        public string emp_depid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? isPrivate
		{
			set{ _isprivate=value;}
			get{return _isprivate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? lastfollow
		{
			set{ _lastfollow=value;}
			get{return _lastfollow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string xy
		{
			set{ _xy=value;}
			get{return _xy;}
		}

        public string cus_extend
        {
            set { _cus_extend = value; }
            get { return _cus_extend; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Delete_time
		{
			set{ _delete_time=value;}
			get{return _delete_time;}
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

