
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// Finance_Receive
	/// </summary>
	public partial class Finance_Receive
	{
		private readonly XHD.DAL.Finance_Receive dal=new XHD.DAL.Finance_Receive();
		public Finance_Receive()
		{}
		#region  BasicMethod
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Finance_Receive model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.Finance_Receive model)
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
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(XHD.Common.PageValidate.SafeLongFilter(idlist,0) );
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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
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
        /// <summary>
        ///     更新应收与订单
        /// </summary>
        public bool UpdateReceive(string Receivableid, string orderid)
        {
            return dal.UpdateReceive(Receivableid, orderid);
        }
        #endregion  ExtensionMethod
    }
}

