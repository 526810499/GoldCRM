using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XHD.BLL
{
    public class BProduct_RetrievalDetail
    {
        private readonly DAL.DProduct_RetrievalDetail dal = new DAL.DProduct_RetrievalDetail();
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_RetrievalDetail model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Update(Model.Product_RetrievalDetail model)
        {
            return dal.Update(model);
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
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
        public bool Delete(string id, string retrid)
        {
            return dal.Delete(id, retrid);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteAll(string retrid)
        {
            return dal.DeleteAll(retrid);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return dal.DeleteList(idlist);
        }
    }
}
