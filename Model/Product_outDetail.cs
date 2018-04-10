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
    public partial class Product_outDetail
    {
        public Product_outDetail()
        { }
        #region Model
        public string __status { get; set; }
        private string _id;
        private string _outid;
        private string _barcode;
        private string _create_id;
        private DateTime _create_time;

        /// <summary>
        /// 出库单类型 0 总部出库 1 部门出库单
        /// </summary>
        public int outType { get; set; }

        /// <summary>
        /// 来自哪个仓库，原仓库
        /// </summary>
       public string FromWarehouse { get; set; }

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
        public string outid
        {
            set { _outid = value; }
            get { return _outid; }
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

