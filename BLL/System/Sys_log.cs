
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// Sys_log
	/// </summary>
	public partial class Sys_log
	{
		private readonly XHD.DAL.Sys_log dal=new XHD.DAL.Sys_log();
		public Sys_log()
		{}
		#region  BasicMethod
		

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Sys_log model)
		{
			return dal.Add(model);
		}
		
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string id)
		{
			
			return dal.Delete(id);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetLogtype()
        {
            return dal.GetLogtype();
        }


        #endregion  ExtensionMethod
    }
}

