using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Sys_Param_City
	/// </summary>
	public partial class Sys_Param_City
	{
		public Sys_Param_City()
		{}
		#region  BasicMethod
        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Sys_Param_City model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_Param_City(");
			strSql.Append("id,Provinces_id,City,City_order,City_type,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@Provinces_id,@City,@City_order,@City_type,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@Provinces_id", SqlDbType.VarChar,50),
					new SqlParameter("@City", SqlDbType.VarChar,250),
					new SqlParameter("@City_order", SqlDbType.Int,4),
					new SqlParameter("@City_type", SqlDbType.VarChar,50),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.Provinces_id;
			parameters[2].Value = model.City;
			parameters[3].Value = model.City_order;
			parameters[4].Value = model.City_type;
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
		public bool Update(XHD.Model.Sys_Param_City model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_Param_City set ");
			strSql.Append("Provinces_id=@Provinces_id,");
			strSql.Append("City=@City,");
			strSql.Append("City_order=@City_order");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Provinces_id", SqlDbType.VarChar,50),
					new SqlParameter("@City", SqlDbType.VarChar,250),
					new SqlParameter("@City_order", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.Provinces_id;
			parameters[1].Value = model.City;
			parameters[2].Value = model.City_order;
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
			strSql.Append("delete from Sys_Param_City ");
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
			strSql.Append("delete from Sys_Param_City ");
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
			strSql.Append("select id,Provinces_id,City,City_order,City_type,create_id,create_time ");
			strSql.Append(" FROM Sys_Param_City ");
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
			strSql.Append(" id,Provinces_id,City,City_order,City_type,create_id,create_time ");
			strSql.Append(" FROM Sys_Param_City ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM Sys_Param_City ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,Provinces_id,City,City_order,City_type,create_id,create_time ");
			strSql_grid.Append(" FROM ( SELECT id,Provinces_id,City,City_order,City_type,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Sys_Param_City");
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

