
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Sys_log_Err
	/// </summary>
	public partial class Sys_log_Err
	{
		public Sys_log_Err()
		{}
        #region  BasicMethod		


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_log_Err model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_log_Err(");
            strSql.Append("id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip)");
            strSql.Append(" values (");
            strSql.Append("@id,@Err_typeid,@Err_type,@Err_time,@Err_url,@Err_message,@Err_source,@Err_trace,@Err_emp_id,@Err_emp_name,@Err_ip)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@Err_typeid", SqlDbType.Int,4),
                    new SqlParameter("@Err_type", SqlDbType.NVarChar,250),
                    new SqlParameter("@Err_time", SqlDbType.DateTime),
                    new SqlParameter("@Err_url", SqlDbType.VarChar,500),
                    new SqlParameter("@Err_message", SqlDbType.NVarChar,-1),
                    new SqlParameter("@Err_source", SqlDbType.VarChar,500),
                    new SqlParameter("@Err_trace", SqlDbType.NVarChar,-1),
                    new SqlParameter("@Err_emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Err_emp_name", SqlDbType.NVarChar,250),
                    new SqlParameter("@Err_ip", SqlDbType.VarChar,250)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.Err_typeid;
            parameters[2].Value = model.Err_type;
            parameters[3].Value = model.Err_time;
            parameters[4].Value = model.Err_url;
            parameters[5].Value = model.Err_message;
            parameters[6].Value = model.Err_source;
            parameters[7].Value = model.Err_trace;
            parameters[8].Value = model.Err_emp_id;
            parameters[9].Value = model.Err_emp_name;
            parameters[10].Value = model.Err_ip;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
			strSql.Append("delete from Sys_log_Err ");
			strSql.Append(" where id=@id and Err_time=@Err_time ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50)		};
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip ");
			strSql.Append(" FROM Sys_log_Err ");
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
			strSql.Append(" id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip ");
			strSql.Append(" FROM Sys_log_Err ");
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
			strSql_total.Append(" SELECT COUNT(Err_time) FROM Sys_log_Err ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip ");
			strSql_grid.Append(" FROM ( SELECT id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Sys_log_Err");
			if (strWhere.Trim() != "")
			{
				strSql_grid.Append(" WHERE " + strWhere);
				strSql_total.Append(" WHERE " + strWhere);
			}
			strSql_grid.Append("  ) as w1  ");
			strSql_grid.Append("WHERE n BETWEEN " + (PageSize * (PageIndex - 1) + 1) + " AND " + PageSize * PageIndex);
			strSql_grid.Append(" ORDER BY " + filedOrder );
			Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql_grid.ToString());
		 }

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        ///     获得数据列表
        /// </summary>
        public DataSet GetLogtype()
        {
            var strSql = new StringBuilder();
            strSql.Append("select distinct Err_type FROM Sys_log_Err order by Err_type");

            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  ExtensionMethod
    }
}

