
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Task
	/// </summary>
	public partial class Task
	{
		public Task()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Task model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Task(");
			strSql.Append("id,task_title,task_content,task_type_id,customer_id,assign_id,executive_id,executive_time,task_status_id,Priority_id,remind_time,isCheck,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@task_title,@task_content,@task_type_id,@customer_id,@assign_id,@executive_id,@executive_time,@task_status_id,@Priority_id,@remind_time,@isCheck,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@task_title", SqlDbType.NVarChar,250),
					new SqlParameter("@task_content", SqlDbType.NVarChar,-1),
					new SqlParameter("@task_type_id", SqlDbType.VarChar,50),
					new SqlParameter("@customer_id", SqlDbType.VarChar,50),
					new SqlParameter("@assign_id", SqlDbType.VarChar,50),
					new SqlParameter("@executive_id", SqlDbType.VarChar,50),
					new SqlParameter("@executive_time", SqlDbType.DateTime),
					new SqlParameter("@task_status_id", SqlDbType.Int,4),
					new SqlParameter("@Priority_id", SqlDbType.Int,4),
					new SqlParameter("@remind_time", SqlDbType.DateTime),
					new SqlParameter("@isCheck", SqlDbType.Int,4),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.task_title;
			parameters[2].Value = model.task_content;
			parameters[3].Value = model.task_type_id;
			parameters[4].Value = model.customer_id;
			parameters[5].Value = model.assign_id;
			parameters[6].Value = model.executive_id;
			parameters[7].Value = model.executive_time;
			parameters[8].Value = model.task_status_id;
			parameters[9].Value = model.Priority_id;
			parameters[10].Value = model.remind_time;
			parameters[11].Value = model.isCheck;
			parameters[12].Value = model.create_id;
			parameters[13].Value = model.create_time;

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
        public bool Update(XHD.Model.Task model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Task set ");
            strSql.Append("task_title=@task_title,");
            strSql.Append("task_content=@task_content,");
            strSql.Append("task_type_id=@task_type_id,");
            strSql.Append("customer_id=@customer_id,");
            strSql.Append("executive_id=@executive_id,");
            strSql.Append("executive_time=@executive_time,");
            strSql.Append("Priority_id=@Priority_id,");
            strSql.Append("remind_time=@remind_time");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@task_title", SqlDbType.NVarChar,250),
                    new SqlParameter("@task_content", SqlDbType.NVarChar,-1),
                    new SqlParameter("@task_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@customer_id", SqlDbType.VarChar,50),
                    new SqlParameter("@executive_id", SqlDbType.VarChar,50),
                    new SqlParameter("@executive_time", SqlDbType.DateTime),
                    new SqlParameter("@Priority_id", SqlDbType.Int,4),
                    new SqlParameter("@remind_time", SqlDbType.DateTime),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.task_title;
            parameters[1].Value = model.task_content;
            parameters[2].Value = model.task_type_id;
            parameters[3].Value = model.customer_id;
            parameters[4].Value = model.executive_id;
            parameters[5].Value = model.executive_time;
            parameters[6].Value = model.Priority_id;
            parameters[7].Value = model.remind_time;
            parameters[8].Value = model.id;

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
			strSql.Append("delete from Task ");
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
			strSql.Append("delete from Task ");
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
			strSql.Append("select id,task_title,task_content,task_type_id,customer_id,assign_id,executive_id,executive_time,task_status_id,Priority_id,remind_time,isCheck,create_id,create_time ");
            strSql.Append(",(select params_name from Sys_Param where id = Task.[task_type_id]) as task_type ");
            strSql.Append(",(select name from hr_employee where id = Task.[assign_id]) as assign ");
            strSql.Append(",(select name from hr_employee where id = Task.[executive_id]) as executive ");
            strSql.Append(",(select cus_name from CRM_Customer where id = Task.[customer_id]) as customer ");
            strSql.Append(" FROM Task ");
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
			strSql.Append(" id,task_title,task_content,task_type_id,customer_id,assign_id,executive_id,executive_time,task_status_id,Priority_id,remind_time,isCheck,create_id,create_time ");
            strSql.Append(",(select params_name from Sys_Param where id = Task.[task_type_id]) as task_type ");
            strSql.Append(",(select name from hr_employee where id = Task.[assign_id]) as assign ");
            strSql.Append(",(select name from hr_employee where id = Task.[executive_id]) as executive ");
            strSql.Append(",(select cus_name from CRM_Customer where id = Task.[customer_id]) as customer ");
            strSql.Append(" FROM Task ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM Task ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,task_title,task_content,task_type_id,customer_id,assign_id,executive_id,executive_time,task_status_id,Priority_id,remind_time,isCheck,create_id,create_time ");
            strSql_grid.Append(",(select params_name from Sys_Param where id = w1.[task_type_id]) as task_type ");
            strSql_grid.Append(",(select name from hr_employee where id = w1.[assign_id]) as assign ");
            strSql_grid.Append(",(select name from hr_employee where id = w1.[executive_id]) as executive ");
            strSql_grid.Append(",(select cus_name from CRM_Customer where id = w1.[customer_id]) as customer ");
            strSql_grid.Append(" FROM ( SELECT id,task_title,task_content,task_type_id,customer_id,assign_id,executive_id,executive_time,task_status_id,Priority_id,remind_time,isCheck,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Task");
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
        ///     更新一条数据
        /// </summary>
        public bool UpdateStatu(Model.Task model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update Task set ");
            strSql.Append("task_status_id=@task_status_id");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@task_status_id", SqlDbType.Int, 4),
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };

            parameters[0].Value = model.task_status_id;
            parameters[1].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }
        #endregion  ExtensionMethod
    }
}

