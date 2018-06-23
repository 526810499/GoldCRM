using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XHD.BLL
{   /// <summary>
    /// Product_allotDetail
    /// </summary>
    public partial class Product_allotDetail
    {
        private readonly DAL.Product_allotDetail dal = new DAL.Product_allotDetail();
        public Product_allotDetail()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_allotDetail model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 获取调拨单下该商品对应订单状态
        /// </summary>
        /// <param name="allotid"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public int GetBarCodeStatus(string allotid, string barcode)
        {
            return dal.GetBarCodeStatus(allotid, barcode);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Product_allotDetail model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {

            return dal.Delete(id);
        }
        /// <summary>
        /// 删除调一条商品
        /// </summary>
        /// <param name="allotid"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public bool Delete(int allotType, string allotid, string barcode)
        {
            return dal.Delete(allotType, allotid, barcode);
        }
        /// <summary>
        /// 删除调一条商品
        /// </summary>
        /// <param name="allotid"></param>
        /// <returns></returns>
        public bool DeleteByAllotid(string allotid)
        {
            return dal.DeleteByAllotid(allotid);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return dal.DeleteList(idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.Product_allotDetail GetModel(string id)
        {

            return dal.GetModel(id);
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
        /// 通过试图查找
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetListView(string strWhere)
        {
            return dal.GetListView(strWhere);
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
        public DataSet GetListProduct(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetListProduct(PageSize, PageIndex, strWhere, filedOrder, out Total);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.Product_allotDetail> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.Product_allotDetail> DataTableToList(DataTable dt)
        {
            List<Model.Product_allotDetail> modelList = new List<Model.Product_allotDetail>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.Product_allotDetail model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
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
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

