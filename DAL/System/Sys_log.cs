
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Sys_log
    /// </summary>
    public partial class Sys_log
    {
        public Sys_log()
        { }
        #region  BasicMethod   


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_log model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_log(");
            strSql.Append("id,EventType,EventID,EventTitle,Log_Content,UserID,UserName,IPStreet,EventDate)");
            strSql.Append(" values (");
            strSql.Append("@id,@EventType,@EventID,@EventTitle,@Log_Content,@UserID,@UserName,@IPStreet,@EventDate)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@EventType", SqlDbType.NVarChar,250),
                    new SqlParameter("@EventID", SqlDbType.VarChar,50),
                    new SqlParameter("@EventTitle", SqlDbType.NVarChar,250),
                    new SqlParameter("@Log_Content", SqlDbType.NVarChar,-1),
                    new SqlParameter("@UserID", SqlDbType.VarChar,50),
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50),
                    new SqlParameter("@IPStreet", SqlDbType.VarChar,50),
                    new SqlParameter("@EventDate", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.EventType;
            parameters[2].Value = model.EventID;
            parameters[3].Value = model.EventTitle;
            parameters[4].Value = model.Log_Content;
            parameters[5].Value = model.UserID;
            parameters[6].Value = model.UserName;
            parameters[7].Value = model.IPStreet;
            parameters[8].Value = model.EventDate;

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
            strSql.Append("delete from Sys_log ");
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
            strSql.Append("delete from Sys_log ");
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
            strSql.Append("select id,EventType,EventID,EventTitle,Log_Content,UserID,IPStreet,EventDate ");
            strSql.Append(",(select name from hr_employee where id = Sys_log.UserID) as UserName ");
            strSql.Append(" FROM Sys_log ");
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
            strSql.Append(" id,EventType,EventID,EventTitle,Log_Content,UserID,IPStreet,EventDate ");
            strSql.Append(",(select name from hr_employee where id = Sys_log.UserID) as UserName ");
            strSql.Append(" FROM Sys_log ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Sys_log ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,EventType,EventID,EventTitle,Log_Content,UserID,IPStreet,EventDate ");
            strSql_grid.Append(",(select name from hr_employee where id = w1.UserID) as UserName ");
            strSql_grid.Append(" FROM ( SELECT id,EventType,EventID,EventTitle,Log_Content,UserID,IPStreet,EventDate, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Sys_log");
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
        /// <summary>
        ///     获得数据列表
        /// </summary>
        public DataSet GetLogtype()
        {
            var strSql = new StringBuilder();
            strSql.Append("select distinct EventType FROM Sys_log order by EventType");

            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion  ExtensionMethod
    }
}

