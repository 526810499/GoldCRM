using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// Sys_Param_Type
	/// </summary>
	public partial class Sys_Param_Type
	{
		private readonly XHD.DAL.Sys_Param_Type dal=new XHD.DAL.Sys_Param_Type();
		public Sys_Param_Type()
		{}
		#region  BasicMethod
		
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
		public DataSet GetAllList()
		{
			return GetList("");
		}
        /// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

