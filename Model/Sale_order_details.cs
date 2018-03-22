
using System;
namespace XHD.Model
{
    /// <summary>
    /// Sale_order_details:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sale_order_details
    {
        public Sale_order_details()
        { }
        #region Model
        private string _order_id;
        private string _product_id;
        private decimal? _agio;
        private int? _quantity;
        private decimal? _amount;
        
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
        public string product_id
        {
            set { _product_id = value; }
            get { return _product_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? agio
        {
            set { _agio = value; }
            get { return _agio; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
       
        #endregion Model

    }
}

