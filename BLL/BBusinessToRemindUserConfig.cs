using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XHD.BLL
{
    /// <summary>
    /// 数据访问类:BusinessToRemindUserConfig
    /// </summary>
    public partial class BBusinessToRemindUserConfig
    {
        private DAL.DBusinessToRemindUserConfig dal = new DAL.DBusinessToRemindUserConfig();
        public BBusinessToRemindUserConfig()
        { }
        #region  BasicMethod


        /// <summary>
        /// 删除当前用户的生日提醒
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeletBrithDayRemind(string userid)
        {
            return dal.DeletBrithDayRemind(userid);
        }


        /// <summary>
        /// 删除当前用户的生日提醒
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet BrithDayRemind(string userid)
        {
            return dal.BrithDayRemind(userid);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(Model.BusinessToRemindUserConfig model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.BusinessToRemindUserConfig model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long id)
        {

            return dal.Delete(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long id, string userid, int remindType)
        {

            return dal.Delete(id, userid, remindType);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return dal.DeleteList(idlist);
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

