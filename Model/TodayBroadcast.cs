using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Model
{
    /// <summary>
    /// 今日播报
    /// </summary>
    [Serializable]
    public partial class TodayBroadcast
    {
        public TodayBroadcast()
        { }
        #region Model
        private long _id;
        private decimal _todayglodprice;
        private decimal _todayotherprice1;
        private decimal _todayotherprice2;
        private decimal _todayotherprice3;
        private decimal _todayotherprice4;
        private string _otherbrodcast;
        private string _remark;
        private string _update_id;
        private DateTime _update_time;
        private string _create_id;
        private DateTime _create_time;
        private string _createdep_id;
        /// <summary>
        /// 
        /// </summary>
        public long id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 今日黄金报价
        /// </summary>
        public decimal TodayGlodPrice
        {
            set { _todayglodprice = value; }
            get { return _todayglodprice; }
        }
        /// <summary>
        /// 非黄金报价
        /// </summary>
        public decimal TodayOtherPrice1
        {
            set { _todayotherprice1 = value; }
            get { return _todayotherprice1; }
        }
        /// <summary>
        /// 非黄金报价
        /// </summary>
        public decimal TodayOtherPrice2
        {
            set { _todayotherprice2 = value; }
            get { return _todayotherprice2; }
        }
        /// <summary>
        /// 非黄金报价
        /// </summary>
        public decimal TodayOtherPrice3
        {
            set { _todayotherprice3 = value; }
            get { return _todayotherprice3; }
        }
        /// <summary>
        /// 非黄金报价
        /// </summary>
        public decimal TodayOtherPrice4
        {
            set { _todayotherprice4 = value; }
            get { return _todayotherprice4; }
        }
        /// <summary>
        /// 其他滚动播报
        /// </summary>
        public string OtherBrodcast
        {
            set { _otherbrodcast = value; }
            get { return _otherbrodcast; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public string update_id
        {
            set { _update_id = value; }
            get { return _update_id; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
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
        /// 部门
        /// </summary>
        public string createdep_id
        {
            set { _createdep_id = value; }
            get { return _createdep_id; }
        }
        #endregion Model

    }
}

