using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace XHD.BLL
{
    /// <summary>
    /// Product_StockIn
    /// </summary>
    public partial class Product_StockIn
    {
        private readonly DAL.Product_StockIn dal = new DAL.Product_StockIn();
        public Product_StockIn()
        { }
        #region  BasicMethod
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_StockIn model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Product_StockIn model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 总部入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool HQUpdateStock(Model.Product_StockIn model)
        {
            return dal.HQUpdateStock(model);
        }

        /// <summary>
        /// 总部入库修改入库商品状态
        /// </summary>
        /// <param name="Stauts"></param>
        /// <param name="StockID"></param>
        /// <returns></returns>
        public bool HQUpdateProductStockStatus(string StockID)
        {
            return dal.HQUpdateProductStockStatus(StockID);
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
            return dal.DeleteList(idlist);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 检查总部入库订单
        /// </summary>
        /// <param name="createid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string CheckHQAddOrder(string createid, int status,int inType)
        {
            return dal.CheckHQAddOrder(createid, status, inType);
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

