using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// Product_StockInDetial:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_StockInDetial
    {
        public Product_StockInDetial()
        { }
        #region Model
        private string _id;
        private string _stockid;
        private string _barcode;
        private string _createdep_id;
        private string _remark;
        private int _warehouse_id;
        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string stockid
        {
            set { _stockid = value; }
            get { return _stockid; }
        }
        /// <summary>
        /// 条形码
        /// </summary>
        public string barcode
        {
            set { _barcode = value; }
            get { return _barcode; }
        }
        /// <summary>
        /// 入库门店
        /// </summary>
        public string createdep_id
        {
            set { _createdep_id = value; }
            get { return _createdep_id; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 现存仓库
        /// </summary>
        public int warehouse_id
        {
            set { _warehouse_id = value; }
            get { return _warehouse_id; }
        }
        #endregion Model

    }
}

