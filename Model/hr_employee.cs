
using System;
namespace XHD.Model
{
    /// <summary>
    /// hr_employee:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class hr_employee
    {
        public hr_employee()
        { }
        #region Model
        private string _id;
        private string _uid;
        private string _pwd;
        private string _name;
        private string _idcard;
        private string _birthday;
        private string _dep_id;
        private string _post_id;
        private string _email;
        private string _sex;
        private string _tel;
        private string _status;
        private string _position_id;
        private int? _sort;
        private string _entrydate;
        private string _address;
        private string _remarks;
        private string _education;
        private string _level;
        private string _professional;
        private string _schools;
        private string _title;
        private string _portal;
        private string _theme;
        private int? _canlogin;
        
        private string _default_city;
        private string _create_id;
        private DateTime _create_time;
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
        public string uid
        {
            set { _uid = value; }
            get { return _uid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string pwd
        {
            set { _pwd = value; }
            get { return _pwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string idcard
        {
            set { _idcard = value; }
            get { return _idcard; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string birthday
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dep_id
        {
            set { _dep_id = value; }
            get { return _dep_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string post_id
        {
            set { _post_id = value; }
            get { return _post_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tel
        {
            set { _tel = value; }
            get { return _tel; }
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
        public string position_id
        {
            set { _position_id = value; }
            get { return _position_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? sort
        {
            set { _sort = value; }
            get { return _sort; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EntryDate
        {
            set { _entrydate = value; }
            get { return _entrydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string address
        {
            set { _address = value; }
            get { return _address; }
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
        public string education
        {
            set { _education = value; }
            get { return _education; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string level
        {
            set { _level = value; }
            get { return _level; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string professional
        {
            set { _professional = value; }
            get { return _professional; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string schools
        {
            set { _schools = value; }
            get { return _schools; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string portal
        {
            set { _portal = value; }
            get { return _portal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string theme
        {
            set { _theme = value; }
            get { return _theme; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? canlogin
        {
            set { _canlogin = value; }
            get { return _canlogin; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string default_city
        {
            set { _default_city = value; }
            get { return _default_city; }
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
        public DateTime create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        #endregion Model

    }
}

