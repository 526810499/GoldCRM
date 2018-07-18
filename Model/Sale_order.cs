
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
        /// 收银员
        /// </summary>
        public string cashier_id { get; set; }

        /// <summary>
        /// 销售部门
        /// </summary>
        public string createdep_id { get; set; }

        /// <summary>
        /// 销售票据
        /// </summary>
        public string PayTheBill { get; set; }

        /// <summary>
        /// 销售部门
        /// </summary>
        public string saledep_id { get; set; }

        /// <summary>
        ///财务核销状态 -1 未提交保存 0 已销售 1核销
        /// </summary>
        public int VerifyStatus { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        public string vipcard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string Serialnumber
        {
            set { _serialnumber = value; }
            get { return _serialnumber; }
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string Customer_id
        {
            set { _customer_id = value; }
            get { return _customer_id; }
        }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime? Order_date
        {
            set { _order_date = value; }
            get { return _order_date; }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string pay_type_id
        {
            set { _pay_type_id = value; }
            get { return _pay_type_id; }
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string Order_status_id
        {
            set { _order_status_id = value; }
            get { return _order_status_id; }
        }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal? Order_amount
        {
            set { _order_amount = value; }
            get { return _order_amount; }
        }
        /// <summary>
        /// 订单折扣优惠金额
        /// </summary>
        public decimal? discount_amount
        {
            set { _discount_amount = value; }
            get { return _discount_amount; }
        }
        /// <summary>
        /// 订单实收金额
        /// </summary>
        public decimal? total_amount
        {
            set { _total_amount = value; }
            get { return _total_amount; }
        }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public string emp_id
        {
            set { _emp_id = value; }
            get { return _emp_id; }
        }
        /// <summary>
        /// 已收总金额
        /// </summary>
        public decimal? receive_money
        {
            set { _receive_money = value; }
            get { return _receive_money; }
        }
        /// <summary>
        /// 未收余额
        /// </summary>
        public decimal? arrears_money
        {
            set { _arrears_money = value; }
            get { return _arrears_money; }
        }
        /// <summary>
        /// 已开票额
        /// </summary>
        public decimal? invoice_money
        {
            set { _invoice_money = value; }
            get { return _invoice_money; }
        }
        /// <summary>
        /// 未开票额
        /// </summary>
        public decimal? arrears_invoice
        {
            set { _arrears_invoice = value; }
            get { return _arrears_invoice; }
        }
        /// <summary>
        /// 订单备注
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

