using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XHD.Common;
using XHD.Model;

namespace XHD.BLL
{
    /// <summary>
    /// 获取今日滚动播报信息
    /// </summary>
    public partial class BTodayBroadcast
    {

        private const string TODAYBROADCASTCACHEKEY = "TodayBroadcast";

        private readonly DAL.DTodayBroadcast dal = new DAL.DTodayBroadcast();
        public BTodayBroadcast()
        { }
        #region  BasicMethod
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.TodayBroadcast model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 获取今日播报
        /// </summary>
        /// <returns></returns>
        public TodayBroadcast GetTodayBroadcast()
        {
            return dal.GetTodayBroadcast();
        }

        /// <summary>
        /// 获取今日播报
        /// </summary>
        /// <returns></returns>
        public TodayBroadcast GetTodayBroadcast(long id)
        {
            return dal.GetTodayBroadcast(id);
        }

        /// <summary>
        /// 获取轮播播报
        /// </summary>
        /// <returns></returns>
        public TodayBroadcast GetBannerTodayBroadcast()
        {

            TodayBroadcast cacheModel = (TodayBroadcast)DataCache.GetCache(TODAYBROADCASTCACHEKEY);
            if (cacheModel == null)
            {
                cacheModel = GetTodayBroadcast();
                if (cacheModel != null)
                {
                    DataCache.SetCache(TODAYBROADCASTCACHEKEY, cacheModel, DateTime.MaxValue, new TimeSpan(12, 0, 0));
                }
            }
            return cacheModel;
        }

        /// <summary>
        /// 清除Key
        /// </summary>
        private void ClearTodayBroadcastCache()
        {
            DataCache.ClearCache(TODAYBROADCASTCACHEKEY);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.TodayBroadcast model)
        {
            ClearTodayBroadcastCache();
            return dal.Update(model);
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

