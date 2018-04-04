
using System;
namespace XHD.Model
{/// <summary>
 /// Product:实体类(属性说明自动提取数据库字段的描述信息)
 /// </summary>
    [Serializable]
    public partial class Product
    {
        public Product()
        { }
        #region Model



        private string _id;
        private string _product_name;
        private string _category_id;
        private int _status;
        private decimal? _weight;
        private string _unit;
        private decimal? _cost;
        private decimal? _price;
        private decimal? _agio;
        private string _remarks;
        private string _specifications;
        private string _create_id;
        private DateTime? _create_time;
        private decimal? _attcosts;
        private decimal? _stockprice;
        private decimal? _mainstoneweight;
        private decimal? _attstoneweight;
        private decimal? _attstonenumber;
        private decimal? _stoneprice;
        private decimal? _goldtotal;
        private decimal? _coststotal;
        private decimal? _totals;
        private string _sbarcode;
        private string _imglogo;
        private string _barcode;
        private int? _outstatus;
        private decimal? _salestotalprice;
        private decimal? _salescoststotal;

        /// <summary>
        /// 部门id
        /// </summary>
        public string createdep_id { get; set; }


        public int warehouse_id { get; set; }

        /// <summary>
        /// 是否黄金 1 是 0否
        /// </summary>
        public int IsGold { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public int SupplierID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string product_name
        {
            set { _product_name = value; }
            get { return _product_name; }
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public string category_id
        {
            set { _category_id = value; }
            get { return _category_id; }
        }
        /// <summary>
        /// 1 入库 2 调拨 3 出库 4已销售
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 重量单位
        /// </summary>
        public string unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal? cost
        {
            set { _cost = value; }
            get { return _cost; }
        }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal? agio
        {
            set { _agio = value; }
            get { return _agio; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }
        /// <summary>
        /// 产品规格
        /// </summary>
        public string specifications
        {
            set { _specifications = value; }
            get { return _specifications; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string create_id
        {
            set { _create_id = value; }
            get { return _create_id; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        /// 付工费
        /// </summary>
        public decimal? AttCosts
        {
            set { _attcosts = value; }
            get { return _attcosts; }
        }
        /// <summary>
        /// 进货金价
        /// </summary>
        public decimal? StockPrice
        {
            set { _stockprice = value; }
            get { return _stockprice; }
        }
        /// <summary>
        /// 主石重
        /// </summary>
        public decimal? MainStoneWeight
        {
            set { _mainstoneweight = value; }
            get { return _mainstoneweight; }
        }
        /// <summary>
        /// 附石重
        /// </summary>
        public decimal? AttStoneWeight
        {
            set { _attstoneweight = value; }
            get { return _attstoneweight; }
        }
        /// <summary>
        /// 附石数
        /// </summary>
        public decimal? AttStoneNumber
        {
            set { _attstonenumber = value; }
            get { return _attstonenumber; }
        }
        /// <summary>
        /// 石价
        /// </summary>
        public decimal? StonePrice
        {
            set { _stoneprice = value; }
            get { return _stoneprice; }
        }
        /// <summary>
        /// 金价小计
        /// </summary>
        public decimal? GoldTotal
        {
            set { _goldtotal = value; }
            get { return _goldtotal; }
        }
        /// <summary>
        /// 工费小计
        /// </summary>
        public decimal? CostsTotal
        {
            set { _coststotal = value; }
            get { return _coststotal; }
        }
        /// <summary>
        /// 成本总价
        /// </summary>
        public decimal? Totals
        {
            set { _totals = value; }
            get { return _totals; }
        }
        /// <summary>
        /// 出厂条码证
        /// </summary>
        public string Sbarcode
        {
            set { _sbarcode = value; }
            get { return _sbarcode; }
        }
        /// <summary>
        /// 图片logo
        /// </summary>
        public string ImgLogo
        {
            set { _imglogo = value; }
            get { return _imglogo; }
        }
        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode
        {
            set { _barcode = value; }
            get { return _barcode; }
        }
        /// <summary>
        /// 出库状态 1未出库 2 已出库
        /// </summary>
        public int? OutStatus
        {
            set { _outstatus = value; }
            get { return _outstatus; }
        }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal? SalesTotalPrice
        {
            set { _salestotalprice = value; }
            get { return _salestotalprice; }
        }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal? SalesCostsTotal
        {
            set { _salescoststotal = value; }
            get { return _salescoststotal; }
        }
        #endregion Model

    }



    [Serializable]
    public  class ProductAllot
    {
        public string warehouse_id { get; set; }

        public string BarCode { get; set; }

        public string __status { get; set; }
    }
}

