
using System;
namespace XHD.Model
{
    /// <summary>
    /// SMS_details:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class SMS_details
    {
        public SMS_details()
        { }
        #region Model
        private string _sms_id;
        private string _contact_id;
        private string _mobiles;
        
        /// <summary>
        /// 
        /// </summary>
        public string sms_id
        {
            set { _sms_id = value; }
            get { return _sms_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string contact_id
        {
            set { _contact_id = value; }
            get { return _contact_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mobiles
        {
            set { _mobiles = value; }
            get { return _mobiles; }
        }
      
        #endregion Model

    }
}

