
using System;
namespace XHD.Model
{
    /// <summary>
    /// Product:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product
    {
        public Product()
        { }
        #region Model
        private string _id;
        private string _product_name;
        private string _category_id;
        private string _status;
        private string _unit;
        private decimal? _cost;
        private decimal? _price;
        private decimal? _agio;
        private string _remarks;
        private string _specifications;
        private string _create_id;
        private DateTime? _create_time;
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
        public string product_name
        {
            set { _product_name = value; }
            get { return _product_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string category_id
        {
            set { _category_id = value; }
            get { return _category_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? cost
        {
            set { _cost = value; }
            get { return _cost; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? agio
        {
            set { _agio = value; }
            get { return _agio; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string specifications
        {
            set { _specifications = value; }
            get { return _specifications; }
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
        public DateTime? create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        #endregion Model

    }
}

