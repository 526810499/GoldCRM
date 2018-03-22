
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:public_notice
	/// </summary>
	public partial class public_notice
	{
		public public_notice()
		{}
		#region  BasicMethod


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.public_notice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into public_notice(");
			strSql.Append("id,notice_title,notice_content,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@notice_title,@notice_content,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@notice_title", SqlDbType.VarChar,250),
					new SqlParameter("@notice_content", SqlDbType.VarChar,-1),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.notice_title;
			parameters[2].Value = model.notice_content;
			parameters[3].Value = model.create_id;
			parameters[4].Value = model.create_time;

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
		public bool Update(XHD.Model.public_notice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update public_notice set ");
			strSql.Append("notice_title=@notice_title,");
			strSql.Append("notice_content=@notice_content");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@notice_title", SqlDbType.VarChar,250),
					new SqlParameter("@notice_content", SqlDbType.VarChar,-1),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.notice_title;
			parameters[1].Value = model.notice_content;
			parameters[2].Value = model.id;

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
			strSql.Append("delete from public_notice ");
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
			strSql.Append("delete from public_notice ");
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
			strSql.Append("select id,notice_title,notice_content,create_id,create_time ");
            strSql.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = public_notice.create_id)) as [dep_name] ");
            strSql.Append(", (select name from hr_employee where id = public_notice.create_id) as [create_name] ");
            strSql.Append(" FROM public_notice ");
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
			strSql.Append(" id,notice_title,create_id,create_time ");
            strSql.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = public_notice.create_id)) as [dep_name] ");
            strSql.Append(", (select name from hr_employee where id = public_notice.create_id) as [create_name] ");
            strSql.Append(" FROM public_notice ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM public_notice ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,notice_title,create_id,create_time ");
            strSql_grid.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = w1.create_id)) as [dep_name] ");
            strSql_grid.Append(", (select name from hr_employee where id = w1.create_id) as [create_name] ");
            strSql_grid.Append(" FROM ( SELECT id,notice_title,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from public_notice");
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

