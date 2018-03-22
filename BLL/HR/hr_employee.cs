
using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// hr_employee
	/// </summary>
	public partial class hr_employee
	{
		private readonly XHD.DAL.hr_employee dal=new XHD.DAL.hr_employee();
		public hr_employee()
		{}
		#region  BasicMethod
		

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.hr_employee model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.hr_employee model)
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
		/// 得到一个对象实体
		/// </summary>
		public XHD.Model.hr_employee GetModel(string id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public XHD.Model.hr_employee GetModelByCache(string id)
		{
			
			string CacheKey = "hr_employeeModel-" + id;
			object objModel = XHD.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(id);
					if (objModel != null)
					{
						int ModelCache = XHD.Common.ConfigHelper.GetConfigInt("ModelCache");
						XHD.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (XHD.Model.hr_employee)objModel;
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
		public List<XHD.Model.hr_employee> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<XHD.Model.hr_employee> DataTableToList(DataTable dt)
		{
			List<XHD.Model.hr_employee> modelList = new List<XHD.Model.hr_employee>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				XHD.Model.hr_employee model;
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
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
		}

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        ///     更新岗位
        /// </summary>
        public bool UpdatePost(Model.hr_employee model)
        {
            return dal.UpdatePost(model);
        }
        public bool UpdateDefaultCity(Model.hr_employee model)
        {
            return dal.UpdateDefaultCity(model);
        }
        /// <summary>
        ///     获取密码
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataSet GetPWD(string ID)
        {
            return dal.GetPWD(ID);
        }

        /// <summary>
        ///     更新密码
        /// </summary>
        public bool changepwd(Model.hr_employee model)
        {
            return dal.changepwd(model);
        }

        /// <summary>
        ///     获取权限
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataSet GetRole(string ID)
        {
            return dal.GetRole(ID);
        }

        /// <summary>
        ///     个人信息修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool PersonalUpdate(Model.hr_employee model)
        {
            return dal.PersonalUpdate(model);
        }

       
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListWithCompany(string strWhere)
        {
            return dal.GetListWithCompany(strWhere);
        }
        #endregion  ExtensionMethod
    }
}

