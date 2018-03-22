using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Sys_Param
    /// </summary>
    public partial class Sys_Param
    {
        public Sys_Param()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Sys_Param");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)           };
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_Param model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_Param(");
            strSql.Append("id,params_name,params_type,params_order,create_id,create_time,KeyID)");
            strSql.Append(" values (");
            strSql.Append("@id,@params_name,@params_type,@params_order,@create_id,@create_time,@KeyID)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@params_name", SqlDbType.VarChar,250),
                    new SqlParameter("@params_type", SqlDbType.VarChar,50),
                    new SqlParameter("@params_order", SqlDbType.Int,4),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                       new SqlParameter("@KeyID", SqlDbType.BigInt)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.params_name;
            parameters[2].Value = model.params_type;
            parameters[3].Value = model.params_order;
            parameters[4].Value = model.create_id;
            parameters[5].Value = model.create_time;
            parameters[6].Value = model.KeyID;
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
        /// 判断当前Key 标记是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckKeyIDIsTrue(XHD.Model.Sys_Param model)
        {
            string sql = "select count(1) from Sys_Param(nolock) where params_type=@params_type and KeyID=@KeyID and ID<>@ID";
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=model.id},

                    new SqlParameter("@params_type", SqlDbType.VarChar,50) { Value=model.params_type},

                       new SqlParameter("@KeyID", SqlDbType.BigInt) { Value=model.KeyID} };

            return DbHelperSQL.ExecuteScalar(sql, parameters).CInt(0, false) > 0;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(XHD.Model.Sys_Param model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sys_Param set ");
            strSql.Append("params_name=@params_name,");
            strSql.Append("params_order=@params_order,");
            strSql.Append("KeyID=@KeyID");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@params_name", SqlDbType.VarChar,250) { Value=model.params_name},
                    new SqlParameter("@params_order", SqlDbType.Int,4) { Value=model.params_order},
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=model.id},
                  new SqlParameter("@KeyID", SqlDbType.BigInt) { Value=model.KeyID} };

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
            strSql.Append("delete from Sys_Param ");
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
            strSql.Append("delete from Sys_Param ");
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
            strSql.Append("select id,params_name,params_type,params_order,create_id,create_time,KeyID ");
            strSql.Append(" FROM Sys_Param ");
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
            strSql.Append(" id,params_name,params_type,params_order,create_id,create_time,KeyID ");
            strSql.Append(" FROM Sys_Param ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Sys_Param ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,params_name,params_type,params_order,create_id,create_time,KeyID ");
            strSql_grid.Append(" FROM ( SELECT id,params_name,params_type,params_order,create_id,create_time,KeyID, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Sys_Param");
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

