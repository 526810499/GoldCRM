
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Sys_role
    /// </summary>
    public partial class Sys_role
    {
        public Sys_role()
        { }
        #region  BasicMethod		


        /// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Sys_role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_role(");
            strSql.Append("id,RoleName,RoleDscript,RoleSort,DataAuth,PublicAuth,create_id,create_time,SpecialRole)");
            strSql.Append(" values (");
            strSql.Append("@id,@RoleName,@RoleDscript,@RoleSort,@DataAuth,@PublicAuth,@create_id,@create_time,@SpecialRole)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,255),
                    new SqlParameter("@RoleDscript", SqlDbType.NVarChar,255),
                    new SqlParameter("@RoleSort", SqlDbType.Int,4),
                    new SqlParameter("@DataAuth", SqlDbType.Int,4),
                    new SqlParameter("@PublicAuth", SqlDbType.Int,4),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                   new SqlParameter("@SpecialRole", SqlDbType.Int,4),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.RoleName;
            parameters[2].Value = model.RoleDscript;
            parameters[3].Value = model.RoleSort;
            parameters[4].Value = model.DataAuth;
            parameters[5].Value = model.PublicAuth;
            parameters[6].Value = model.create_id;
            parameters[7].Value = model.create_time;
            parameters[8].Value = model.SpecialRole;
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
		public bool Update(XHD.Model.Sys_role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sys_role set ");
            strSql.Append("RoleName=@RoleName,");
            strSql.Append("RoleDscript=@RoleDscript,");
            strSql.Append("RoleSort=@RoleSort,");
            strSql.Append("DataAuth=@DataAuth,");
            strSql.Append("PublicAuth=@PublicAuth,SpecialRole=@SpecialRole");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,255),
                    new SqlParameter("@RoleDscript", SqlDbType.NVarChar,255),
                    new SqlParameter("@RoleSort", SqlDbType.Int,4),
                    new SqlParameter("@DataAuth", SqlDbType.Int,4),
                    new SqlParameter("@PublicAuth", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                   new SqlParameter("@SpecialRole", SqlDbType.Int,4),
            };
            parameters[0].Value = model.RoleName;
            parameters[1].Value = model.RoleDscript;
            parameters[2].Value = model.RoleSort;
            parameters[3].Value = model.DataAuth;
            parameters[4].Value = model.PublicAuth;
            parameters[5].Value = model.id;
            parameters[6].Value = model.SpecialRole;
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sys_role ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)           };
            parameters[0].Value = id;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sys_role ");
            strSql.Append(" where id in (" + idlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,RoleName,RoleDscript,RoleSort,DataAuth,PublicAuth,create_id,create_time,SpecialRole ");
            strSql.Append(" FROM Sys_role ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,RoleName,RoleDscript,RoleSort,DataAuth,PublicAuth,create_id,create_time,SpecialRole  ");
            strSql.Append(" FROM Sys_role ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Sys_role ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,RoleName,RoleDscript,RoleSort,DataAuth,PublicAuth,create_id,create_time,SpecialRole  ");
            strSql_grid.Append(" FROM ( SELECT id,RoleName,RoleDscript,RoleSort,DataAuth,PublicAuth,create_id,create_time,SpecialRole, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Sys_role");
            if (strWhere.Trim() != "")
            {
                strSql_grid.Append(" WHERE " + strWhere);
                strSql_total.Append(" WHERE " + strWhere);
            }
            strSql_grid.Append("  ) as w1  ");
            strSql_grid.Append("WHERE n BETWEEN " + (PageSize * (PageIndex - 1) + 1) + " AND " + PageSize * PageIndex);
            strSql_grid.Append(" ORDER BY " + filedOrder);
            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }

        #endregion  BasicMethod

        #region  ExtensionMethod       
        #endregion  ExtensionMethod
    }
}

