
using System;
namespace XHD.Model
{
    /// <summary>
    /// Finance_Receivable:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Finance_Receivable
    {
        public Finance_Receivable()
        { }
        #region Model
        private string _id;
        private string _receivable_no;
        private string _order_id;
        private DateTime? _receivable_time;
        private decimal? _receivable_amount;
        private decimal? _received_amount;
        private decimal? _arrears_amount;
        private string _remark;
        
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
        public string receivable_no
        {
            set { _receivable_no = value; }
            get { return _receivable_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string order_id
        {
            set { _order_id = value; }
            get { return _order_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? receivable_time
        {
            set { _receivable_time = value; }
            get { return _receivable_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? receivable_amount
        {
            set { _receivable_amount = value; }
            get { return _receivable_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? received_amount
        {
            set { _received_amount = value; }
            get { return _received_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? arrears_amount
        {
            set { _arrears_amount = value; }
            get { return _arrears_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
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

