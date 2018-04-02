using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// Product_allotDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_allotDetail
    {
        public Product_allotDetail()
        { }
        #region Model
        public string __status { get; set; }
        private string _id;
        private string _allotid;
        private string _barcode;
        private int _fromwarehouse;
        private string _create_id;
        private DateTime _create_time;
        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 调拨单id
        /// </summary>
        public string allotid
        {
            set { _allotid = value; }
            get { return _allotid; }
        }
        /// <summary>
        /// 商品条形码
        /// </summary>
        public string barcode
        {
            set { _barcode = value; }
            get { return _barcode; }
        }
        /// <summary>
        /// 来自仓库
        /// </summary>
        public int FromWarehouse
        {
            set { _fromwarehouse = value; }
            get { return _fromwarehouse; }
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
        public DateTime create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        #endregion Model

    }
}

