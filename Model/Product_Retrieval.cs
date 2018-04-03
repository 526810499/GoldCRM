using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// 订购补货
    /// </summary>
    [Serializable]
    public partial class Product_Retrieval
    {
        public Product_Retrieval()
        { }
        #region Model
        private string _id;
        private decimal _weight;
        private int _number;
        private string _dep_id;
        private string _category_id;
        private string _remark;
        private DateTime _create_time;
        private string _create_id;

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 克重
        /// </summary>
        public decimal weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int number
        {
            set { _number = value; }
            get { return _number; }
        }
        /// <summary>
        /// 订购部门
        /// </summary>
        public string dep_id
        {
            set { _dep_id = value; }
            get { return _dep_id; }
        }
        /// <summary>
        /// 订购种类
        /// </summary>
        public string category_id
        {
            set { _category_id = value; }
            get { return _category_id; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
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
        /// 创建id
        /// </summary>
        public string create_id
        {
            set { _create_id = value; }
            get { return _create_id; }
        }
        #endregion Model

    }
}

