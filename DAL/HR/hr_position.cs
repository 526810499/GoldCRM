
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:hr_position
	/// </summary>
	public partial class hr_position
	{
		public hr_position()
		{}
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.hr_position model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hr_position(");
            strSql.Append("id,position_name,position_order,position_level,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@position_name,@position_order,@position_level,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@position_name", SqlDbType.NVarChar,250),
                    new SqlParameter("@position_order", SqlDbType.Int,4),
                    new SqlParameter("@position_level", SqlDbType.VarChar,50),                    
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.position_name;
            parameters[2].Value = model.position_order;
            parameters[3].Value = model.position_level;
            parameters[4].Value = model.create_id;
            parameters[5].Value = model.create_time;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(XHD.Model.hr_position model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hr_position set ");
			strSql.Append("position_name=@position_name,");
			strSql.Append("position_order=@position_order,");
			strSql.Append("position_level=@position_level");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@position_name", SqlDbType.NVarChar,250),
					new SqlParameter("@position_order", SqlDbType.Int,4),
					new SqlParameter("@position_level", SqlDbType.VarChar,50),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.position_name;
			parameters[1].Value = model.position_order;
			parameters[2].Value = model.position_level;
			parameters[3].Value = model.id;

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
			strSql.Append("delete from hr_position ");
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
			strSql.Append("delete from hr_position ");
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
			strSql.Append("select id,position_name,position_order,position_level,create_id,create_time ");
			strSql.Append(" FROM hr_position ");
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
			strSql.Append(" id,position_name,position_order,position_level,create_id,create_time,ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n ");
			strSql.Append(" FROM hr_position ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM hr_position ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,position_name,position_order,position_level,create_id,create_time ");
			strSql_grid.Append(" FROM ( SELECT id,position_name,position_order,position_level,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from hr_position");
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

		#endregion  ExtensionMethod
	}
}

