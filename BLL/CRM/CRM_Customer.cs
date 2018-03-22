
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// CRM_Customer
	/// </summary>
	public partial class CRM_Customer
	{
		private readonly XHD.DAL.CRM_Customer dal=new XHD.DAL.CRM_Customer();
		public CRM_Customer()
		{}
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
		public bool Add(XHD.Model.CRM_Customer model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.CRM_Customer model)
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
        ///     批量转客源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update_batch(Model.CRM_Customer model, string strWhere)
        {
            return dal.Update_batch(model, strWhere);
        }

        /// <summary>
        ///     预删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDelete"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool AdvanceDelete(string id, int isDelete, string time)
        {
            return dal.AdvanceDelete(id, isDelete, time);
        }

        /// <summary>
        ///     更新最后跟进
        /// </summary>
        public bool UpdateLastFollow(string id)
        {
            return dal.UpdateLastFollow(id);
        }

        public DataSet Reports_year(string items, int year, string company_id)
        {
            return dal.Reports_year(items, year, company_id);
        }

        /// <summary>
        ///     同比环比【客户新增】
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="project_id"></param>
        /// <returns></returns>
        public DataSet Compared(string year1, string month1, string year2, string month2)
        {
            return dal.Compared(year1, month1, year2, month2);
        }

        public DataSet Compared_type(string year1, string month1, string year2, string month2)
        {
            return dal.Compared_type(year1, month1, year2, month2);
        }

        public DataSet Compared_level(string year1, string month1, string year2, string month2)
        {
            return dal.Compared_level(year1, month1, year2, month2);
        }

        public DataSet Compared_source(string year1, string month1, string year2, string month2)
        {
            return dal.Compared_source(year1, month1, year2, month2);
        }

        public DataSet Compared_empcusadd(string year1, string month1, string year2, string month2, string idlist)
        //, string idlist)
        {
            return dal.Compared_empcusadd(year1, month1, year2, month2, idlist); //, idlist);
        }

        /// <summary>
        ///     客户新增统计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet report_empcus(int year, string idlist)
        {
            return dal.report_empcus(year, idlist);
        }

        /// <summary>
        ///     统计漏斗
        /// </summary>
        public DataSet Funnel(string strWhere, string year )
        {
            return dal.Funnel(strWhere, year);
        }

        public DataSet GetMapList(string strWhere)
        {
            return dal.GetMapList(strWhere);
        }

        /// <summary>
        /// 更新手机数据
        /// </summary>
        public bool UpdateApp(XHD.Model.CRM_Customer model)
        {
            return dal.UpdateApp(model);
        }
        #endregion  ExtensionMethod
    }
}

