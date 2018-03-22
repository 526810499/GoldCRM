
using System;
namespace XHD.Model
{
    /// <summary>
    /// Sale_order:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sale_order
    {
        public Sale_order()
        { }
        #region Model
        private string _id;
        private string _serialnumber;
        private string _customer_id;
        private DateTime? _order_date;
        private string _pay_type_id;
        private string _order_status_id;
        private decimal? _order_amount;
        private decimal? _discount_amount;
        private decimal? _total_amount;
        private string _emp_id;
        private decimal? _receive_money;
        private decimal? _arrears_money;
        private decimal? _invoice_money;
        private decimal? _arrears_invoice;
        private string _order_details;
        private string _create_id;
        private DateTime? _create_time;
        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Serialnumber
        {
            set { _serialnumber = value; }
            get { return _serialnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Customer_id
        {
            set { _customer_id = value; }
            get { return _customer_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Order_date
        {
            set { _order_date = value; }
            get { return _order_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string pay_type_id
        {
            set { _pay_type_id = value; }
            get { return _pay_type_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Order_status_id
        {
            set { _order_status_id = value; }
            get { return _order_status_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Order_amount
        {
            set { _order_amount = value; }
            get { return _order_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? discount_amount
        {
            set { _discount_amount = value; }
            get { return _discount_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? total_amount
        {
            set { _total_amount = value; }
            get { return _total_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string emp_id
        {
            set { _emp_id = value; }
            get { return _emp_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? receive_money
        {
            set { _receive_money = value; }
            get { return _receive_money; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? arrears_money
        {
            set { _arrears_money = value; }
            get { return _arrears_money; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? invoice_money
        {
            set { _invoice_money = value; }
            get { return _invoice_money; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? arrears_invoice
        {
            set { _arrears_invoice = value; }
            get { return _arrears_invoice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Order_details
        {
            set { _order_details = value; }
            get { return _order_details; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string create_id
        {
            set { _create_id = value; }
            get { return _create_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        #endregion Model

    }
}

