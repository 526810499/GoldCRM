using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// Product_TakeStock:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_TakeStock
    {
        public Product_TakeStock()
        { }
        #region Model
        private string _id;
        private int _taketype;
        private int _status = 0;
        private string _update_id;
        private DateTime _update_time;
        private string _authuser_id;
        private DateTime _authuser_time;
        private DateTime _create_time;
        private string _create_id;
        private string _remark;
        private string _createdep_id;

        /// <summary>
        /// 盘点仓库
        /// </summary>
        public int warehouse_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 盘点分类 0 总部 1门店
        /// </summary>
        public int takeType
        {
            set { _taketype = value; }
            get { return _taketype; }
        }
        /// <summary>
        /// 0 未生成盘点单 1已生成等待审核  2审核通过 3审核不通过
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
        /// <summary>
        /// 
        /// </summary>
        public string authuser_id
        {
            set { _authuser_id = value; }
            get { return _authuser_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime authuser_time
        {
            set { _authuser_time = value; }
            get { return _authuser_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
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
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string createdep_id
        {
            set { _createdep_id = value; }
            get { return _createdep_id; }
        }
        #endregion Model

    }
}

