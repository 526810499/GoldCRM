
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// Sys_Button
	/// </summary>
	public partial class Sys_Button
	{
		private readonly XHD.DAL.Sys_Button dal=new XHD.DAL.Sys_Button();
		public Sys_Button()
		{}
		#region  BasicMethod
		

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Sys_Button model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.Sys_Button model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string Btn_id)
		{
			
			return dal.Delete(Btn_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Btn_idlist )
		{
			return dal.DeleteList(XHD.Common.PageValidate.SafeLongFilter(Btn_idlist,0) );
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


		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

