using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// Product_RetrievalDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_RetrievalDetail
    {
        public Product_RetrievalDetail()
        { }
        #region Model
        private string _id;
        private decimal _weight;
        private int _number;
        private string _category_id;
        private string _retrid;

        public string categoryName { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        public string __status { get; set; }

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
        /// 订购种类
        /// </summary>
        public string category_id
        {
            set { _category_id = value; }
            get { return _category_id; }
        }
        /// <summary>
        /// 订购单号
        /// </summary>
        public string retrid
        {
            set { _retrid = value; }
            get { return _retrid; }
        }
        #endregion Model

    }
}

