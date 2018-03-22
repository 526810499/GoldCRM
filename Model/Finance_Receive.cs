
using System;
namespace XHD.Model
{
	/// <summary>
	/// Finance_Receive:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Finance_Receive
	{
		public Finance_Receive()
		{}
		#region Model
		private string _id;
		private string _receive_num;
		private string _pay_type_id;
		private decimal? _receive_amount;
		private DateTime? _receive_date;
		private string _payee_id;
		private string _receivable_id;
		private string _remarks;
		
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
		public string Receive_num
		{
			set{ _receive_num=value;}
			get{return _receive_num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Pay_type_id
		{
			set{ _pay_type_id=value;}
			get{return _pay_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Receive_amount
		{
			set{ _receive_amount=value;}
			get{return _receive_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Receive_date
		{
			set{ _receive_date=value;}
			get{return _receive_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Payee_id
		{
			set{ _payee_id=value;}
			get{return _payee_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Receivable_id
		{
			set{ _receivable_id=value;}
			get{return _receivable_id;}
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

