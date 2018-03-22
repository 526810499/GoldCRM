
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// Sys_authority
	/// </summary>
	public partial class Sys_authority
	{
		private readonly XHD.DAL.Sys_authority dal=new XHD.DAL.Sys_authority();
		public Sys_authority()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Sys_authority model)
		{
			return dal.Add(model);
		}

		
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string where)
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete(where);
		}
		
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
        

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

