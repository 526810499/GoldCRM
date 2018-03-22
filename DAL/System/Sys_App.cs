
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Sys_App
    /// </summary>
    public partial class Sys_App
    {
        public Sys_App()
        { }
        #region  BasicMethod




        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_App model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_App(");
            strSql.Append("id,App_name,App_order,App_url,App_handler,App_type,App_icon)");
            strSql.Append(" values (");
            strSql.Append("@id,@App_name,@App_order,@App_url,@App_handler,@App_type,@App_icon)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@App_name", SqlDbType.NVarChar,100),
                    new SqlParameter("@App_order", SqlDbType.Int,4),
                    new SqlParameter("@App_url", SqlDbType.VarChar,250),
                    new SqlParameter("@App_handler", SqlDbType.VarChar,250),
                    new SqlParameter("@App_type", SqlDbType.VarChar,50),
                    new SqlParameter("@App_icon", SqlDbType.VarChar,250)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.App_name;
            parameters[2].Value = model.App_order;
            parameters[3].Value = model.App_url;
            parameters[4].Value = model.App_handler;
            parameters[5].Value = model.App_type;
            parameters[6].Value = model.App_icon;

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
        /// 删除对应的应用
        /// </summary>
        /// <param name="App_id"></param>
        /// <returns></returns>
        public bool Delete(string App_id)
        {
            string sql = "delete Sys_App where id=@App_id";
            SqlParameter[] parameters = {
                    new SqlParameter("@App_id", SqlDbType.VarChar,50)          };
            parameters[0].Value = App_id;

            return DbHelperSQL.ExecuteSql(sql, parameters) > 0;
        }

        /// <summary>
        /// 修改应用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(Model.Sys_App model)
        {

            string sql = "update Sys_App set App_name=@App_name,App_order=@App_order,App_url=@App_url,App_handler=@App_handler,App_type=@App_type,App_icon=@App_icon where id=@id";

            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=model.id},
                    new SqlParameter("@App_name", SqlDbType.NVarChar,100) { Value=model.App_name},
                    new SqlParameter("@App_order", SqlDbType.Int,4){ Value=model.App_order},
                    new SqlParameter("@App_url", SqlDbType.VarChar,250){ Value=model.App_url},
                    new SqlParameter("@App_handler", SqlDbType.VarChar,250){ Value=model.App_handler},
                    new SqlParameter("@App_type", SqlDbType.VarChar,50){ Value=model.App_type},
                    new SqlParameter("@App_icon", SqlDbType.VarChar,250){ Value=model.App_icon}};
            return DbHelperSQL.ExecuteSql(sql, parameters) > 0;
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,App_name,App_order,App_url,App_handler,App_type,App_icon ");
            strSql.Append(" FROM Sys_App ");
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
            strSql.Append(" id,App_name,App_order,App_url,App_handler,App_type,App_icon ");
            strSql.Append(" FROM Sys_App ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Sys_App(nolock) ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,App_name,App_order,App_url,App_handler,App_type,App_icon ");
            strSql_grid.Append(" FROM ( SELECT id,App_name,App_order,App_url,App_handler,App_type,App_icon, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Sys_App");
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

