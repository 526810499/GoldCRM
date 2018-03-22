
using System;
namespace XHD.Model
{
    /// <summary>
    /// SMS:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class SMS
    {
        public SMS()
        { }
        #region Model
        private string _id;
        private string _sms_title;
        private string _sms_content;
        private string _contact_ids;
        private string _sms_mobiles;
        private int? _issend;
        private DateTime? _sendtime;
        private string _check_id;
        
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
        public string sms_title
        {
            set { _sms_title = value; }
            get { return _sms_title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sms_content
        {
            set { _sms_content = value; }
            get { return _sms_content; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string contact_ids
        {
            set { _contact_ids = value; }
            get { return _contact_ids; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sms_mobiles
        {
            set { _sms_mobiles = value; }
            get { return _sms_mobiles; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isSend
        {
            set { _issend = value; }
            get { return _issend; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? sendtime
        {
            set { _sendtime = value; }
            get { return _sendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string check_id
        {
            set { _check_id = value; }
            get { return _check_id; }
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

