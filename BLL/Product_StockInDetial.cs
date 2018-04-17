using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XHD.BLL
{
    /// <summary>
    /// Product_StockInDetial
    /// </summary>
    public partial class Product_StockInDetial
    {
        private readonly DAL.Product_StockInDetial dal = new DAL.Product_StockInDetial();
        public Product_StockInDetial()
        { }
        #region  BasicMethod


        /// <summary>
        /// 修改商品现存仓库
        /// </summary>
        /// <param name="stockid"></param>
        /// <param name="warehouse_id"></param>
        /// <returns></returns>
        public bool UpdateProductWareHouse(string stockid, int warehouse_id, string dep_id)
        {
            return dal.UpdateProductWareHouse(stockid, warehouse_id, dep_id);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_StockInDetial model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Product_StockInDetial model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string stockid, string barcode)
        {

            return dal.Delete(id, stockid, barcode);
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
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

