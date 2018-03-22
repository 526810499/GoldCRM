
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:CRM_Contact
	/// </summary>
	public partial class CRM_Contact
	{
		public CRM_Contact()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.CRM_Contact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_Contact(");
			strSql.Append("id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,customer_id,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@C_name,@C_sex,@C_department,@C_position,@C_birthday,@C_tel,@C_fax,@C_email,@C_mob,@C_QQ,@C_add,@C_hobby,@C_remarks,@customer_id,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@C_name", SqlDbType.VarChar,250),
					new SqlParameter("@C_sex", SqlDbType.Int,4),
					new SqlParameter("@C_department", SqlDbType.VarChar,250),
					new SqlParameter("@C_position", SqlDbType.VarChar,250),
					new SqlParameter("@C_birthday", SqlDbType.DateTime),
					new SqlParameter("@C_tel", SqlDbType.VarChar,250),
					new SqlParameter("@C_fax", SqlDbType.VarChar,250),
					new SqlParameter("@C_email", SqlDbType.VarChar,250),
					new SqlParameter("@C_mob", SqlDbType.VarChar,250),
					new SqlParameter("@C_QQ", SqlDbType.VarChar,250),
					new SqlParameter("@C_add", SqlDbType.VarChar,250),
					new SqlParameter("@C_hobby", SqlDbType.VarChar,250),
					new SqlParameter("@C_remarks", SqlDbType.VarChar,-1),
					new SqlParameter("@customer_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.C_name;
			parameters[2].Value = model.C_sex;
			parameters[3].Value = model.C_department;
			parameters[4].Value = model.C_position;
			parameters[5].Value = model.C_birthday;
			parameters[6].Value = model.C_tel;
			parameters[7].Value = model.C_fax;
			parameters[8].Value = model.C_email;
			parameters[9].Value = model.C_mob;
			parameters[10].Value = model.C_QQ;
			parameters[11].Value = model.C_add;
			parameters[12].Value = model.C_hobby;
			parameters[13].Value = model.C_remarks;
			parameters[14].Value = model.customer_id;
			parameters[15].Value = model.create_id;
			parameters[16].Value = model.create_time;

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
		public bool Update(XHD.Model.CRM_Contact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_Contact set ");
			strSql.Append("C_name=@C_name,");
			strSql.Append("C_sex=@C_sex,");
			strSql.Append("C_department=@C_department,");
			strSql.Append("C_position=@C_position,");
			strSql.Append("C_birthday=@C_birthday,");
			strSql.Append("C_tel=@C_tel,");
			strSql.Append("C_fax=@C_fax,");
			strSql.Append("C_email=@C_email,");
			strSql.Append("C_mob=@C_mob,");
			strSql.Append("C_QQ=@C_QQ,");
			strSql.Append("C_add=@C_add,");
			strSql.Append("C_hobby=@C_hobby,");
			strSql.Append("C_remarks=@C_remarks");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@C_name", SqlDbType.VarChar,250),
					new SqlParameter("@C_sex", SqlDbType.Int,4),
					new SqlParameter("@C_department", SqlDbType.VarChar,250),
					new SqlParameter("@C_position", SqlDbType.VarChar,250),
					new SqlParameter("@C_birthday", SqlDbType.DateTime),
					new SqlParameter("@C_tel", SqlDbType.VarChar,250),
					new SqlParameter("@C_fax", SqlDbType.VarChar,250),
					new SqlParameter("@C_email", SqlDbType.VarChar,250),
					new SqlParameter("@C_mob", SqlDbType.VarChar,250),
					new SqlParameter("@C_QQ", SqlDbType.VarChar,250),
					new SqlParameter("@C_add", SqlDbType.VarChar,250),
					new SqlParameter("@C_hobby", SqlDbType.VarChar,250),
					new SqlParameter("@C_remarks", SqlDbType.VarChar,-1),
					new SqlParameter("@customer_id", SqlDbType.VarChar,50),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.C_name;
			parameters[1].Value = model.C_sex;
			parameters[2].Value = model.C_department;
			parameters[3].Value = model.C_position;
			parameters[4].Value = model.C_birthday;
			parameters[5].Value = model.C_tel;
			parameters[6].Value = model.C_fax;
			parameters[7].Value = model.C_email;
			parameters[8].Value = model.C_mob;
			parameters[9].Value = model.C_QQ;
			parameters[10].Value = model.C_add;
			parameters[11].Value = model.C_hobby;
			parameters[12].Value = model.C_remarks;
			parameters[13].Value = model.customer_id;
			parameters[14].Value = model.id;

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
			strSql.Append("delete from CRM_Contact ");
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
			strSql.Append("delete from CRM_Contact ");
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
			strSql.Append("select id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,customer_id,create_id,create_time ");
            strSql.Append(",(select cus_name from CRM_Customer where id = CRM_Contact.customer_id ) as customer ");
            strSql.Append(" FROM CRM_Contact ");
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
			strSql.Append(" id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,customer_id,create_id,create_time ");
            strSql.Append(",(select cus_name from CRM_Customer where id = CRM_Contact.customer_id ) as customer ");
            strSql.Append(" FROM CRM_Contact ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM CRM_Contact ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,customer_id,create_id,create_time ");
            strSql_grid.Append(",(select cus_name from CRM_Customer where id = w1.customer_id ) as customer ");
            strSql_grid.Append(" FROM ( SELECT id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,customer_id,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from CRM_Contact");
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

