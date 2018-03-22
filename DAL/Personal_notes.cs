
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Personal_notes
	/// </summary>
	public partial class Personal_notes
	{
		public Personal_notes()
		{}
		#region  BasicMethod
        	

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Personal_notes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Personal_notes(");
			strSql.Append("id,emp_id,emp_name,note_content,note_color,xyz,note_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@emp_id,@emp_name,@note_content,@note_color,@xyz,@note_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@emp_id", SqlDbType.VarChar,50),
					new SqlParameter("@emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@note_content", SqlDbType.VarChar,-1),
					new SqlParameter("@note_color", SqlDbType.VarChar,250),
					new SqlParameter("@xyz", SqlDbType.VarChar,250),
					new SqlParameter("@note_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.emp_id;
			parameters[2].Value = model.emp_name;
			parameters[3].Value = model.note_content;
			parameters[4].Value = model.note_color;
			parameters[5].Value = model.xyz;
			parameters[6].Value = model.note_time;

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
        public bool Update(XHD.Model.Personal_notes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Personal_notes set ");
            strSql.Append("xyz=@xyz");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {                   
                    new SqlParameter("@xyz", SqlDbType.VarChar,250),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
          
            parameters[0].Value = model.xyz;
            parameters[1].Value = model.id;

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
			strSql.Append("delete from Personal_notes ");
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,emp_id,emp_name,note_content,note_color,xyz,note_time ");
			strSql.Append(" FROM Personal_notes ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

        /// <summary>
        ///     获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            var strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top);
            }
            strSql.Append(" id,emp_id,emp_name,note_content,note_color,xyz,note_time ");
            strSql.Append(" FROM Personal_notes ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            strSql_total.Append(" SELECT COUNT(id) FROM Personal_notes ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,emp_id,emp_name,note_content,note_color,xyz,note_time ");
            strSql_grid.Append(" FROM ( SELECT id,emp_id,emp_name,note_content,note_color,xyz,note_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Personal_notes");
            if (strWhere.Trim() != "")
            {
                strSql_grid.Append(" WHERE " + strWhere);
                strSql_total.Append(" WHERE " + strWhere);
            }
            strSql_grid.Append("  ) as w1  ");
            strSql_grid.Append("WHERE n BETWEEN " + PageSize * (PageIndex - 1) + " AND " + PageSize * PageIndex);
            strSql_grid.Append(" ORDER BY " + filedOrder);
            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

