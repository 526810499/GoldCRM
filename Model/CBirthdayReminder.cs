using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// CBirthdayReminder:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CBirthdayReminder
    {
        public CBirthdayReminder()
        { }
        #region Model
        private long _id;
        private string _cuserid;
        private string _cusername;
        private DateTime _birthday;
        private int _reminder;
        private int _rcount;
        private string _ruserid;
        /// <summary>
        /// 
        /// </summary>
        public long id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string cuserid
        {
            set { _cuserid = value; }
            get { return _cuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string cusername
        {
            set { _cusername = value; }
            get { return _cusername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime birthday
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 是否提醒 1是 0否
        /// </summary>
        public int reminder
        {
            set { _reminder = value; }
            get { return _reminder; }
        }
        /// <summary>
        /// 提醒次数
        /// </summary>
        public int rcount
        {
            set { _rcount = value; }
            get { return _rcount; }
        }
        /// <summary>
        /// 需要提醒的用户
        /// </summary>
        public string ruserid
        {
            set { _ruserid = value; }
            get { return _ruserid; }
        }
        #endregion Model

    }
}

