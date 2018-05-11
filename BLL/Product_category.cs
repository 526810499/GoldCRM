
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
    /// <summary>
    /// Product_category
    /// </summary>
    public partial class Product_category
    {
        private readonly XHD.DAL.Product_category dal = new XHD.DAL.Product_category();
        public Product_category()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Product_category model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(XHD.Model.Product_category model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 添加编码
        /// </summary>
        /// <param name="CodingBegins"></param>
        public void AddNO(string CodingBegins)
        {
            dal.AddNO(CodingBegins);
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
        /// 获取当前这个分类ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int GetCategoryCounts(string ID)
        {
            return dal.GetCategoryCounts(ID);
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



        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

