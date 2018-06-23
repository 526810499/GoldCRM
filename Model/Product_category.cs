
using System;
namespace XHD.Model
{
    /// <summary>
    /// Product_category:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Product_category
    {
        public Product_category()
        { }
        #region Model
        private string _id;
        private string _product_category;
        private string _parentid;
        private string _product_icon;

        private string _create_id;
        private DateTime? _create_time;


        /// <summary>
        /// 分类条形码开始标记
        /// </summary>
        public string CodingBegins { get; set; }

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
        public string product_category
        {
            set { _product_category = value; }
            get { return _product_category; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string parentid
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


    /// <summary>
    /// 相关序列化标记记录表
    /// </summary>
    [Serializable]
    public class Sys_SerialNumber
    {
        public long ID { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public int Counts { get; set; }

        /// <summary>
        /// 开头标记
        /// </summary>
        public string BegLetter { get; set; }

        /// <summary>
        /// 分类 1 条形码 2订单号
        /// </summary>
        public int Type { get; set; }

        public DateTime Times { get; set; }
    }

    /// <summary>
    /// 序列号类型
    /// </summary>
    public enum SerialNumberType
    {
        /// <summary>
        /// 商品条形码
        /// </summary>
        ProductBarcode = 1,
        /// <summary>
        /// 订单号
        /// </summary>
        OrderNO = 2
    }
}

