using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XHD.BLL
{
    /// <summary>
    /// 盘点
    /// </summary>
    public partial class Product_TakeStock
    {
        private readonly DAL.Product_TakeStock dal = new DAL.Product_TakeStock();
        public Product_TakeStock()
        { }
        #region  BasicMethod
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_TakeStock model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Product_TakeStock model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Auth(Model.Product_TakeStock model)
        {
            return dal.Auth(model);
        }


        /// <summary>
        /// 盘点没有盘点进来的商品
        /// </summary>
        /// <param name="takeid"></param>
        /// <param name="warehouse_id"></param>
        /// <returns></returns>
        public int ProductClearingTake(string takeid, int warehouse_id,string createdep_id)
        {
            return dal.ProductClearingTake(takeid, warehouse_id, createdep_id);
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
        /// 获取盘点商品
        /// </summary>
        /// <param name="warehouse_id"></param>
        /// <param name="takeid"></param>
        /// <returns></returns>
        public DataSet GetTakeList(int warehouse_id, string takeid)
        {
            return dal.GetTakeList(warehouse_id, takeid);
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
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

