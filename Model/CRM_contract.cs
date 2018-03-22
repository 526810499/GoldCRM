
using System;
namespace XHD.Model
{
	/// <summary>
	/// CRM_contract:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_contract
	{
		public CRM_contract()
		{}
		#region Model
		private string _id;
		private string _contract_name;
		private string _serialnumber;
		private string _customer_id;
		private decimal? _contract_amount;
		private int? _pay_cycle;
		private DateTime? _start_date;
		private DateTime? _end_date;
		private DateTime? _sign_date;
		private string _customer_contractor;
		private string _our_contractor_id;
		private string _main_content;
		private string _remarks;
		private string _creater_id;
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
		public string Contract_name
		{
			set{ _contract_name=value;}
			get{return _contract_name;}
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
		public string Customer_id
		{
			set{ _customer_id=value;}
			get{return _customer_id;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public decimal? Contract_amount
		{
			set{ _contract_amount=value;}
			get{return _contract_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Pay_cycle
		{
			set{ _pay_cycle=value;}
			get{return _pay_cycle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Start_date
		{
			set{ _start_date=value;}
			get{return _start_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? End_date
		{
			set{ _end_date=value;}
			get{return _end_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Sign_date
		{
			set{ _sign_date=value;}
			get{return _sign_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Customer_Contractor
		{
			set{ _customer_contractor=value;}
			get{return _customer_contractor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Our_Contractor_id
		{
			set{ _our_contractor_id=value;}
			get{return _our_contractor_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Main_Content
		{
			set{ _main_content=value;}
			get{return _main_content;}
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
		/// 
		/// </summary>
		public string creater_id
		{
			set{ _creater_id=value;}
			get{return _creater_id;}
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

