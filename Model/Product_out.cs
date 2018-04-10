using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// Product_allot:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_out
    {
        public Product_out()
        { }
        #region Model
        private string _id;
        private int _nowwarehouse;
        private string _create_id;
        private DateTime _create_time;
        private int _status;
        private string _update_id;
        private DateTime _update_time;

        /// <summary>
        /// 出库分类 0 总部出库单 1门店出库单
        /// </summary>
        public int outType { get; set; }

        /// <summary>
        /// 调拨到哪个门店部门
        /// </summary>
        public string outdep_id { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public string createdep_id { get; set; }
       
        public string Remark { get; set;}

        /// <summary>
        /// 调拨单号
        /// </summary>
        public string allot_id { get; set; }
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
        public int NowWarehouse
        {
            set { _nowwarehouse = value; }
            get { return _nowwarehouse; }
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
        /// <summary>
        ///0 未提交审核 1 提交等待审核 2审核通过 3审核不通过
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string update_id
        {
            set { _update_id = value; }
            get { return _update_id; }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
        }
        #endregion Model

    }
}

