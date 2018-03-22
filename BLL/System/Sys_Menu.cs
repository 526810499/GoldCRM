/*
* Sys_Menu.cs
*
* 功 能： N/A
* 类 名： Sys_Menu
*
* Ver    变更日期             负责人     变更内容
* ───────────────────────────────────
* V1.0  2015-11-01 12:44:12    黄润伟    
*
* Copyright © 2015 www.xhdcrm.com All rights reserved.
*┌──────────────────────────────────┐
*│　版权所有：{xx}                      　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
    /// <summary>
    /// Sys_Menu
    /// </summary>
    public partial class Sys_Menu
    {
        private readonly XHD.DAL.Sys_Menu dal = new XHD.DAL.Sys_Menu();
        public Sys_Menu()
        { }
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_Menu model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(XHD.Model.Sys_Menu model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string Menu_id)
        {

            return dal.Delete(Menu_id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string Menu_idlist)
        {
            return dal.DeleteList(XHD.Common.PageValidate.SafeLongFilter(Menu_idlist, 0));
        }


        /// <summary>
        /// 获取应用下的数量
        /// </summary>
        /// <param name="App_id"></param>
        /// <returns></returns>
        public int CountMenu(string App_id)
        {
            return dal.CountMenu(App_id);
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
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

