using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// 以旧换新:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_OldChangeNew
    {
        public Product_OldChangeNew()
        { }
        #region Model
        private string _id;
        private decimal _oldweight;
        private decimal _oldtotalprice;
        private decimal _oldcharge;
        private decimal _newweight;
        private decimal _newtotalprice;
        private decimal _coststotalprice;
        private decimal _discount;
        private decimal _diftotalprice;
        private string _remark;
        private DateTime _create_time;
        private string _create_id;


        /// <summary>
        /// 门店
        /// </summary>
        public string createdep_id { get; set; }

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
        /// 旧金重
        /// </summary>
        public decimal oldWeight
        {
            set { _oldweight = value; }
            get { return _oldweight; }
        }
        /// <summary>
        /// 旧金价值
        /// </summary>
        public decimal oldTotalPrice
        {
            set { _oldtotalprice = value; }
            get { return _oldtotalprice; }
        }
        /// <summary>
        /// 旧金折旧费
        /// </summary>
        public decimal oldCharge
        {
            set { _oldcharge = value; }
            get { return _oldcharge; }
        }
        /// <summary>
        /// 新金克重
        /// </summary>
        public decimal newWeight
        {
            set { _newweight = value; }
            get { return _newweight; }
        }
        /// <summary>
        /// 新金价值
        /// </summary>
        public decimal newTotalPrice
        {
            set { _newtotalprice = value; }
            get { return _newtotalprice; }
        }
        /// <summary>
        /// 工费
        /// </summary>
        public decimal costsTotalPrice
        {
            set { _coststotalprice = value; }
            get { return _coststotalprice; }
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal discount
        {
            set { _discount = value; }
            get { return _discount; }
        }
        /// <summary>
        /// 需补差额
        /// </summary>
        public decimal difTotalPrice
        {
            set { _diftotalprice = value; }
            get { return _diftotalprice; }
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

