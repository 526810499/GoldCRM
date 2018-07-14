
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
        /// 会员卡类型  0 无 1 金卡、2银卡、3 员工价、4股东价
        /// </summary>
        public int VipCardType { get; set; }

        /// <summary>
        /// 销售分类 销售分类 0 无 1黄金 2一口价硬金 3 工费 4 K金
        /// </summary>
        public int SaleType { get; set; }
        /// <summary>
        /// 折扣类型 0 无 1折扣 2立减
        /// </summary>
        public int DiscountType { get; set; }

        /// <summary>
        /// 折扣 DiscountType 1 时对应的折扣比
        /// </summary>
        public decimal DiscountCount { get; set; }

        /// <summary>
        /// 实时计算金额
        /// </summary>
        public decimal RealTotal { get; set; }

        /// <summary>
        /// 实时销售单价
        /// </summary>
        public decimal SalesUnitPrice { get; set; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Discounts { get; set; }

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

