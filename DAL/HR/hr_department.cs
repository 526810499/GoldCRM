
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:hr_department
	/// </summary>
	public partial class hr_department
	{
		public hr_department()
		{}
        #region  BasicMethod		

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.hr_department model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hr_department(");
            strSql.Append("id,dep_name,parentid,parentname,dep_type,dep_icon,dep_chief,dep_tel,dep_fax,dep_add,dep_email,dep_descript,dep_order,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@dep_name,@parentid,@parentname,@dep_type,@dep_icon,@dep_chief,@dep_tel,@dep_fax,@dep_add,@dep_email,@dep_descript,@dep_order,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_name", SqlDbType.NVarChar,50),
                    new SqlParameter("@parentid", SqlDbType.VarChar,50),
                    new SqlParameter("@parentname", SqlDbType.NVarChar,50),
                    new SqlParameter("@dep_type", SqlDbType.NVarChar,50),
                    new SqlParameter("@dep_icon", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_chief", SqlDbType.NVarChar,50),
                    new SqlParameter("@dep_tel", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_fax", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_add", SqlDbType.NVarChar,255),
                    new SqlParameter("@dep_email", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_descript", SqlDbType.NVarChar,255),
                    new SqlParameter("@dep_order", SqlDbType.Int,4),                    
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.dep_name;
            parameters[2].Value = model.parentid;
            parameters[3].Value = model.parentname;
            parameters[4].Value = model.dep_type;
            parameters[5].Value = model.dep_icon;
            parameters[6].Value = model.dep_chief;
            parameters[7].Value = model.dep_tel;
            parameters[8].Value = model.dep_fax;
            parameters[9].Value = model.dep_add;
            parameters[10].Value = model.dep_email;
            parameters[11].Value = model.dep_descript;
            parameters[12].Value = model.dep_order;
            parameters[13].Value = model.create_id;
            parameters[14].Value = model.create_time;

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
        public bool Update(XHD.Model.hr_department model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hr_department set ");
			strSql.Append("dep_name=@dep_name,");
			strSql.Append("parentid=@parentid,");
			strSql.Append("parentname=@parentname,");
			strSql.Append("dep_type=@dep_type,");
			strSql.Append("dep_icon=@dep_icon,");
			strSql.Append("dep_chief=@dep_chief,");
			strSql.Append("dep_tel=@dep_tel,");
			strSql.Append("dep_fax=@dep_fax,");
			strSql.Append("dep_add=@dep_add,");
			strSql.Append("dep_email=@dep_email,");
			strSql.Append("dep_descript=@dep_descript,");
			strSql.Append("dep_order=@dep_order");
			strSql.Append(" where id=@id  ");
			SqlParameter[] parameters = {
					new SqlParameter("@dep_name", SqlDbType.NVarChar,50),
					new SqlParameter("@parentid", SqlDbType.VarChar,50),
					new SqlParameter("@parentname", SqlDbType.NVarChar,50),
					new SqlParameter("@dep_type", SqlDbType.NVarChar,50),
					new SqlParameter("@dep_icon", SqlDbType.VarChar,50),
					new SqlParameter("@dep_chief", SqlDbType.NVarChar,50),
					new SqlParameter("@dep_tel", SqlDbType.VarChar,50),
					new SqlParameter("@dep_fax", SqlDbType.VarChar,50),
					new SqlParameter("@dep_add", SqlDbType.NVarChar,255),
					new SqlParameter("@dep_email", SqlDbType.VarChar,50),
					new SqlParameter("@dep_descript", SqlDbType.NVarChar,255),
					new SqlParameter("@dep_order", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.dep_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentname;
			parameters[3].Value = model.dep_type;
			parameters[4].Value = model.dep_icon;
			parameters[5].Value = model.dep_chief;
			parameters[6].Value = model.dep_tel;
			parameters[7].Value = model.dep_fax;
			parameters[8].Value = model.dep_add;
			parameters[9].Value = model.dep_email;
			parameters[10].Value = model.dep_descript;
			parameters[11].Value = model.dep_order;
			parameters[12].Value = model.id;

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
			strSql.Append("delete from hr_department ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50)
            };
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
			strSql.Append("select id,dep_name,parentid,parentname,dep_type,dep_icon,dep_chief,dep_tel,dep_fax,dep_add,dep_email,dep_descript,dep_order,create_id,create_time ");
			strSql.Append(" FROM hr_department ");
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
			strSql.Append(" id,dep_name,parentid,parentname,dep_type,dep_icon,dep_chief,dep_tel,dep_fax,dep_add,dep_email,dep_descript,dep_order,create_id,create_time ");
			strSql.Append(" FROM hr_department ");
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
			strSql_total.Append(" SELECT COUNT(create_time) FROM hr_department ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      id,dep_name,parentid,parentname,dep_type,dep_icon,dep_chief,dep_tel,dep_fax,dep_add,dep_email,dep_descript,dep_order,create_id,create_time ");
			strSql_grid.Append(" FROM ( SELECT id,dep_name,parentid,parentname,dep_type,dep_icon,dep_chief,dep_tel,dep_fax,dep_add,dep_email,dep_descript,dep_order,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from hr_department");
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

