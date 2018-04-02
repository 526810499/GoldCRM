
using System;
namespace XHD.Model
{
    /// <summary>
    /// 仓库
    /// </summary>
    [Serializable]
    public partial class Product_warehouse
    {
        public Product_warehouse()
        { }
        #region Model
        private int _id;
        private string _product_warehouse;
        private int _parentid;
        private string _product_icon;
        /// <summary>
        /// 仓库ID
        /// </summary>
        public int warehouseID { get; set; }

        private string _create_id;
        private DateTime? _create_time;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string product_warehouse
        {
            set { _product_warehouse = value; }
            get { return _product_warehouse; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int parentid
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string product_icon
        {
            set { _product_icon = value; }
            get { return _product_icon; }
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

