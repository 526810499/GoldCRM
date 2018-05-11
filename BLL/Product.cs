
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
    /// <summary>
    /// Product
    /// </summary>
    public partial class Product
    {
        private readonly XHD.DAL.Product dal = new XHD.DAL.Product();
        public Product()
        { }
        #region  BasicMethod


        /// <summary>
        /// 通过条形码获取商品状态
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public int GetPorductStatusByBarCode(string BarCode)
        {
            return dal.GetPorductStatusByBarCode(BarCode);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Product model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(XHD.Model.Product model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 根据条形码获取商品ID
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public string GetProductIdByCode(string BarCode)
        {
            return dal.GetProductIdByCode(BarCode);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {

            return dal.Delete(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return dal.DeleteList(XHD.Common.PageValidate.SafeLongFilter(idlist, 0));
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetTakeList(string strWhere)
        {
            return dal.GetTakeList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetExportList(string strWhere)
        {

            return dal.GetExportList(strWhere);
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

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out DataTable totalTable)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out totalTable);
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

