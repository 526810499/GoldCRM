
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
    /// <summary>
    /// Sys_App
    /// </summary>
    public partial class Sys_App
    {
        private readonly XHD.DAL.Sys_App dal = new XHD.DAL.Sys_App();
        public Sys_App()
        { }
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_App model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 修改应用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(Model.Sys_App model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除对应的应用
        /// </summary>
        /// <param name="App_id"></param>
        /// <returns></returns>
        public bool Delete(string App_id)
        {
            return dal.Delete(App_id);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }


        /// <summary>
        /// 分页获取数据列表
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }


        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

