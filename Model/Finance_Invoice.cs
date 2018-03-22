
using System;
namespace XHD.Model
{
	/// <summary>
	/// Finance_Invoice:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Finance_Invoice
	{
		public Finance_Invoice()
		{}
		#region Model
		private string _id;
		private string _order_id;
		private string _invoice_num;
		private string _invoice_type_id;
		private decimal? _invoice_amount;
		private string _invoice_content;
		private DateTime? _invoice_date;
		private string _empid;
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
		public string order_id
		{
			set{ _order_id=value;}
			get{return _order_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string invoice_num
		{
			set{ _invoice_num=value;}
			get{return _invoice_num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string invoice_type_id
		{
			set{ _invoice_type_id=value;}
			get{return _invoice_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? invoice_amount
		{
			set{ _invoice_amount=value;}
			get{return _invoice_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string invoice_content
		{
			set{ _invoice_content=value;}
			get{return _invoice_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? invoice_date
		{
			set{ _invoice_date=value;}
			get{return _invoice_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string empid
		{
			set{ _empid=value;}
			get{return _empid;}
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

