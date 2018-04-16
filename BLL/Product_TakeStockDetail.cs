using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XHD.BLL
{
    /// <summary>
    /// 盘点明细
    /// </summary>
    public class Product_TakeStockDetail
    {
        private readonly DAL.Product_TakeStockDetail dal = new DAL.Product_TakeStockDetail();
        public Product_TakeStockDetail()
        { }
        #region  BasicMethod
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_TakeStockDetail model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Product_TakeStockDetail model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string takeid)
        {

            return dal.Delete(id, takeid);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteAllByTakeID(string takeid)
        {
            return dal.DeleteAllByTakeID(takeid);
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

