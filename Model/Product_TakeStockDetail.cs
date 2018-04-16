using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// Product_TakeStockDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_TakeStockDetail
    {
        public Product_TakeStockDetail()
        { }
        #region Model
        private string _id;
        private string _takeid;
        private int _warehouse_id;
        private string _barcode;
        private DateTime _taketime;
        private int _status;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 盘点单id
        /// </summary>
        public string takeid
        {
            set { _takeid = value; }
            get { return _takeid; }
        }
        /// <summary>
        /// 货品仓库
        /// </summary>
        public int warehouse_id
        {
            set { _warehouse_id = value; }
            get { return _warehouse_id; }
        }
        /// <summary>
        /// 商品
        /// </summary>
        public string barcode
        {
            set { _barcode = value; }
            get { return _barcode; }
        }
        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime taketime
        {
            set { _taketime = value; }
            get { return _taketime; }
        }
        /// <summary>
        /// 盘点状态 1正常 2盘盈 3盘亏 
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

    }
}

