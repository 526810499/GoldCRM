using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// BusinessToRemindUserConfig:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BusinessToRemindUserConfig
    {
        public BusinessToRemindUserConfig()
        { }
        #region Model
        private long _id;
        private string _userid;
        private string _create_id;
        private DateTime _create_time;
        private int _remindtype;
        private string _remark;
        private int _rcount;
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
        public string userid
        {
            set { _userid = value; }
            get { return _userid; }
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
        /// 
        /// </summary>
        public int remindType
        {
            set { _remindtype = value; }
            get { return _remindtype; }
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
        /// 提醒次数
        /// </summary>
        public int rcount
        {
            set { _rcount = value; }
            get { return _rcount; }
        }
        #endregion Model

    }
}

