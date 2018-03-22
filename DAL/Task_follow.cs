
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Task_follow
	/// </summary>
	public partial class Task_follow
	{
		public Task_follow()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Task_follow model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Task_follow(");
			strSql.Append("id,task_id,follow_id,follow_time,follow_content,follow_status)");
			strSql.Append(" values (");
			strSql.Append("@id,@task_id,@follow_id,@follow_time,@follow_content,@follow_status)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@task_id", SqlDbType.VarChar,50),
					new SqlParameter("@follow_id", SqlDbType.VarChar,50),
					new SqlParameter("@follow_time", SqlDbType.DateTime),
					new SqlParameter("@follow_content", SqlDbType.NVarChar,-1),
					new SqlParameter("@follow_status", SqlDbType.Int,4)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.task_id;
			parameters[2].Value = model.follow_id;
			parameters[3].Value = model.follow_time;
			parameters[4].Value = model.follow_content;
			parameters[5].Value = model.follow_status;

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
		public bool Update(XHD.Model.Task_follow model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Task_follow set ");
			strSql.Append("follow_content=@follow_content");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@follow_content", SqlDbType.NVarChar,-1),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.follow_content;
			parameters[1].Value = model.id;

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
			strSql.Append("delete from Task_follow ");
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
			strSql.Append("delete from Task_follow ");
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
			strSql.Append("select id,task_id,follow_id,follow_time,follow_content,follow_status ");
			strSql.Append(" FROM Task_follow ");
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
			strSql.Append(" id,task_id,follow_id,follow_time,follow_content,follow_status ");
			strSql.Append(" FROM Task_follow ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM Task_follow ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,task_id,follow_id,follow_time,follow_content,follow_status ");
			strSql_grid.Append(" FROM ( SELECT id,task_id,follow_id,follow_time,follow_content,follow_status, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Task_follow");
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
        ///     删除一条数据
        /// </summary>
        public bool DeleteWhere(string strWhere)
        {
            var strSql = new StringBuilder();
            strSql.Append("delete from Task_follow ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            return false;
        }
        #endregion  ExtensionMethod
    }
}

