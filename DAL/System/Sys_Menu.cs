
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Sys_Menu
    /// </summary>
    public partial class Sys_Menu
    {
        public Sys_Menu()
        { }
        #region  BasicMethod




        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_Menu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_Menu(");
            strSql.Append("Menu_id,Menu_name,parentid,App_id,Menu_url,Menu_icon,Menu_order,Menu_type,isMobile,m_css,m_color)");
            strSql.Append(" values (");
            strSql.Append("@Menu_id,@Menu_name,@parentid,@App_id,@Menu_url,@Menu_icon,@Menu_order,@Menu_type,@isMobile,@m_css,@m_color)");
            SqlParameter[] parameters = {
                    new SqlParameter("@Menu_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Menu_name", SqlDbType.VarChar,255),
                    new SqlParameter("@parentid", SqlDbType.VarChar,50),
                    new SqlParameter("@App_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Menu_url", SqlDbType.VarChar,255),
                    new SqlParameter("@Menu_icon", SqlDbType.VarChar,50),
                    new SqlParameter("@Menu_order", SqlDbType.Int,4),
                    new SqlParameter("@Menu_type", SqlDbType.VarChar,50),
                    new SqlParameter("@isMobile", SqlDbType.Int,4),
                    new SqlParameter("@m_css", SqlDbType.VarChar,50),
                    new SqlParameter("@m_color", SqlDbType.VarChar,50),
                    };
            parameters[0].Value = model.Menu_id;
            parameters[1].Value = model.Menu_name;
            parameters[2].Value = model.parentid;
            parameters[3].Value = model.App_id;
            parameters[4].Value = model.Menu_url;
            parameters[5].Value = model.Menu_icon;
            parameters[6].Value = model.Menu_order;
            parameters[7].Value = model.Menu_type;
            parameters[8].Value = model.isMobile;
            parameters[9].Value = model.m_css;
            parameters[10].Value = model.m_color;

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
		public bool Update(XHD.Model.Sys_Menu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sys_Menu set ");
            strSql.Append("Menu_name=@Menu_name,");
            strSql.Append("parentid=@parentid,");
            strSql.Append("Menu_url=@Menu_url,");
            strSql.Append("Menu_icon=@Menu_icon,");
            strSql.Append("Menu_order=@Menu_order,");
            strSql.Append("isMobile=@isMobile,");
            strSql.Append("m_css=@m_css,");
            strSql.Append("m_color=@m_color");
            strSql.Append(" where Menu_id=@Menu_id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Menu_name", SqlDbType.VarChar,255),
                    new SqlParameter("@parentid", SqlDbType.VarChar,50),
                    new SqlParameter("@Menu_url", SqlDbType.VarChar,255),
                    new SqlParameter("@Menu_icon", SqlDbType.VarChar,50),
                    new SqlParameter("@Menu_order", SqlDbType.Int,4),
                    new SqlParameter("@isMobile", SqlDbType.Int,4),
                    new SqlParameter("@m_css", SqlDbType.VarChar,50),
                    new SqlParameter("@m_color", SqlDbType.VarChar,50),
                    new SqlParameter("@Menu_id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.Menu_name;
            parameters[1].Value = model.parentid;
            parameters[2].Value = model.Menu_url;
            parameters[3].Value = model.Menu_icon;
            parameters[4].Value = model.Menu_order;
            parameters[5].Value = model.isMobile;
            parameters[6].Value = model.m_css;
            parameters[7].Value = model.m_color;
            parameters[8].Value = model.Menu_id;

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
        public bool Delete(string Menu_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sys_Menu ");
            strSql.Append(" where Menu_id=@Menu_id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Menu_id", SqlDbType.VarChar,50)          };
            parameters[0].Value = Menu_id;

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
        public bool DeleteList(string Menu_idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sys_Menu ");
            strSql.Append(" where Menu_id in (" + Menu_idlist + ")  ");
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
        /// 获取应用下的数量
        /// </summary>
        /// <param name="App_id"></param>
        /// <returns></returns>
        public int CountMenu(string App_id)
        {
            string sql = "select count(1) from Sys_Menu where App_id=@App_id";
            SqlParameter[] parameters = {
                    new SqlParameter("@App_id", SqlDbType.VarChar,50)          };
            parameters[0].Value = App_id;

            return DbHelperSQL.ExecuteScalar(sql, parameters).CInt(0, false);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Menu_id,Menu_name,parentid,App_id,Menu_url,Menu_icon,Menu_order,Menu_type,isMobile,m_css,m_color ");
            strSql.Append(" FROM Sys_Menu ");
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
            strSql.Append(" Menu_id,Menu_name,parentid,App_id,Menu_url,Menu_icon,Menu_order,Menu_type,isMobile,m_css,m_color ");
            strSql.Append(" FROM Sys_Menu ");
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
            strSql_total.Append(" SELECT COUNT(Menu_id) FROM Sys_Menu ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,Menu_id,Menu_name,parentid,App_id,Menu_url,Menu_icon,Menu_order,Menu_type,isMobile,m_css,m_color");
            strSql_grid.Append(" FROM ( SELECT Menu_id,Menu_name,parentid,App_id,Menu_url,Menu_icon,Menu_order,Menu_type,isMobile,m_css,m_color, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Sys_Menu");
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

