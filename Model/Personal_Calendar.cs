
using System;
namespace XHD.Model
{
    /// <summary>
    /// Personal_Calendar:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Personal_Calendar
    {
        public Personal_Calendar()
        { }
        #region Model
        private string _id;
        private string _emp_id;
        private string _customer_id;
        private string _subject;
        private int? _masterid;
        private int? _calendartype;
        private DateTime? _starttime;
        private DateTime? _endtime;
        private bool _isalldayevent;
        private int? _category;
        private int? _instancetype;
        private string _upaccount;
        private string _upname;
        private DateTime? _uptime;
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
        public string emp_id
        {
            set { _emp_id = value; }
            get { return _emp_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string customer_id
        {
            set { _customer_id = value; }
            get { return _customer_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Subject
        {
            set { _subject = value; }
            get { return _subject; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? MasterId
        {
            set { _masterid = value; }
            get { return _masterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CalendarType
        {
            set { _calendartype = value; }
            get { return _calendartype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAllDayEvent
        {
            set { _isalldayevent = value; }
            get { return _isalldayevent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Category
        {
            set { _category = value; }
            get { return _category; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? InstanceType
        {
            set { _instancetype = value; }
            get { return _instancetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UPAccount
        {
            set { _upaccount = value; }
            get { return _upaccount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UPName
        {
            set { _upname = value; }
            get { return _upname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UPTime
        {
            set { _uptime = value; }
            get { return _uptime; }
        }
        #endregion Model

    }
}

