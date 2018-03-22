
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Tool_batch
	/// </summary>
	public partial class Tool_batch
	{
		public Tool_batch()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Tool_batch model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tool_batch(");
			strSql.Append("id,batch_type,batch_filter,o_emp_id,c_emp_id,b_count,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@batch_type,@batch_filter,@o_emp_id,@c_emp_id,@b_count,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@batch_type", SqlDbType.VarChar,50),
					new SqlParameter("@batch_filter", SqlDbType.VarChar,-1),
					new SqlParameter("@o_emp_id", SqlDbType.VarChar,50),
					new SqlParameter("@c_emp_id", SqlDbType.VarChar,50),
					new SqlParameter("@b_count", SqlDbType.Int,4),					
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.batch_type;
			parameters[2].Value = model.batch_filter;
			parameters[3].Value = model.o_emp_id;
			parameters[4].Value = model.c_emp_id;
			parameters[5].Value = model.b_count;
			parameters[6].Value = model.create_id;
			parameters[7].Value = model.create_time;

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
			strSql.Append("select id,batch_type,batch_filter,o_emp_id,c_emp_id,b_count,create_id,create_time ");
            strSql.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = Tool_batch.o_emp_id)) as [o_dep_name] ");
            strSql.Append(", (select name from hr_employee where id = Tool_batch.o_emp_id) as [o_emp_name] ");
            strSql.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = Tool_batch.c_emp_id)) as [c_dep_name] ");
            strSql.Append(", (select name from hr_employee where id = Tool_batch.c_emp_id) as [c_emp_name] ");
            strSql.Append(", (select name from hr_employee where id = Tool_batch.create_id) as [create_name] ");
            strSql.Append(" FROM Tool_batch ");
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
			strSql.Append(" id,batch_type,batch_filter,o_emp_id,c_emp_id,b_count,create_id,create_time ");
            strSql.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = Tool_batch.o_emp_id)) as [o_dep_name] ");
            strSql.Append(", (select name from hr_employee where id = Tool_batch.o_emp_id) as [o_emp_name] ");
            strSql.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = Tool_batch.c_emp_id)) as [c_dep_name] ");
            strSql.Append(", (select name from hr_employee where id = Tool_batch.c_emp_id) as [c_emp_name] ");
            strSql.Append(", (select name from hr_employee where id = Tool_batch.create_id) as [create_name] ");
            strSql.Append(" FROM Tool_batch ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM Tool_batch ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      id,batch_type,batch_filter,o_emp_id,c_emp_id,b_count,create_id,create_time ");
            strSql_grid.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = w1.o_emp_id)) as [o_dep_name] ");
            strSql_grid.Append(", (select name from hr_employee where id = w1.o_emp_id) as [o_emp_name] ");
            strSql_grid.Append(", (select dep_name from hr_department where id = (select dep_id from hr_employee where id = w1.c_emp_id)) as [c_dep_name] ");
            strSql_grid.Append(", (select name from hr_employee where id = w1.c_emp_id) as [c_emp_name] ");
            strSql_grid.Append(", (select name from hr_employee where id = w1.create_id) as [create_name] ");
            strSql_grid.Append(" FROM ( SELECT id,batch_type,batch_filter,o_emp_id,c_emp_id,b_count,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Tool_batch");
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

