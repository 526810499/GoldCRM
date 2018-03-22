
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:CRM_contract_atta
	/// </summary>
	public partial class CRM_contract_atta
	{
		public CRM_contract_atta()
		{}
		#region  BasicMethod


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.CRM_contract_atta model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_contract_atta(");
			strSql.Append("id,contract_id,file_name,real_name,file_size,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@contract_id,@file_name,@real_name,@file_size,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@contract_id", SqlDbType.VarChar,50),
					new SqlParameter("@file_name", SqlDbType.VarChar,250),
					new SqlParameter("@real_name", SqlDbType.VarChar,250),
					new SqlParameter("@file_size", SqlDbType.Int,4),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.contract_id;
			parameters[2].Value = model.file_name;
			parameters[3].Value = model.real_name;
			parameters[4].Value = model.file_size;
			parameters[5].Value = model.create_id;
			parameters[6].Value = model.create_time;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.CRM_contract_atta model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_contract_atta set ");
			strSql.Append("contract_id=@contract_id,");
			strSql.Append("file_name=@file_name,");
			strSql.Append("real_name=@real_name,");
			strSql.Append("file_size=@file_size,");
			strSql.Append("create_id=@create_id,");
			strSql.Append("create_time=@create_time");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@contract_id", SqlDbType.VarChar,50),
					new SqlParameter("@file_name", SqlDbType.VarChar,250),
					new SqlParameter("@real_name", SqlDbType.VarChar,250),
					new SqlParameter("@file_size", SqlDbType.Int,4),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.contract_id;
			parameters[1].Value = model.file_name;
			parameters[2].Value = model.real_name;
			parameters[3].Value = model.file_size;
			parameters[4].Value = model.create_id;
			parameters[5].Value = model.create_time;
			parameters[6].Value = model.id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from CRM_contract_atta ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50)			};
			parameters[0].Value = id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from CRM_contract_atta ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,contract_id,file_name,real_name,file_size,create_id,create_time ");
			strSql.Append(" FROM CRM_contract_atta ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" id,contract_id,file_name,real_name,file_size,create_id,create_time ");
			strSql.Append(" FROM CRM_contract_atta ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			StringBuilder strSql_grid = new StringBuilder();
			StringBuilder strSql_total = new StringBuilder();
			strSql_total.Append(" SELECT COUNT(id) FROM CRM_contract_atta ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,contract_id,file_name,real_name,file_size,create_id,create_time ");
			strSql_grid.Append(" FROM ( SELECT id,contract_id,file_name,real_name,file_size,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from CRM_contract_atta");
			if (strWhere.Trim() != "")
			{
				strSql_grid.Append(" WHERE " + strWhere);
				strSql_total.Append(" WHERE " + strWhere);
			}
			strSql_grid.Append("  ) as w1  ");
			strSql_grid.Append("WHERE n BETWEEN " + PageSize * (PageIndex - 1) + " AND " + PageSize * PageIndex);
			strSql_grid.Append(" ORDER BY " + filedOrder );
			Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql_grid.ToString());
		 }

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        /// 条件删除
        /// </summary>
        public bool DeleteWhere(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CRM_contract_atta ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion  ExtensionMethod
    }
}

