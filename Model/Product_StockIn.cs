using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// Product_StockIn:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_StockIn
    {
        public Product_StockIn()
        { }
        #region Model
        private string _id;
        private string _create_id;
        private DateTime _create_time;
        private int _status = 0;
        private string _remark;
        private string _createdep_id;
        private int _intype;
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
        /// 状态 0 未保存 1 已提交入库  -1 临时单
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
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
        /// 入库门店
        /// </summary>
        public string createdep_id
        {
            set { _createdep_id = value; }
            get { return _createdep_id; }
        }
        /// <summary>
        /// 入库类型 0 总部 1门店
        /// </summary>
        public int inType
        {
            set { _intype = value; }
            get { return _intype; }
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

