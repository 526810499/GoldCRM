using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XHD.DBUtility;

namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:BusinessToRemindUserConfig
    /// </summary>
    public partial class DBusinessToRemindUserConfig
    {
        public DBusinessToRemindUserConfig()
        { }
        #region  BasicMethod


        /// <summary>
        /// 删除当前用户的生日提醒
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeletBrithDayRemind(string userid)
        {
            string sql = "delete CBirthdayReminder where ruserid=@ruserid";
            SqlParameter[] par = { new SqlParameter("@ruserid", SqlDbType.VarChar, 50) { Value = userid } };

            return DbHelperSQL.ExecuteSql(sql, par);
        }

        /// <summary>
        /// 删除当前用户的生日提醒
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet BrithDayRemind(string userid)
        {
            string sql = "select * from CBirthdayReminder(nolock) where ruserid=@ruserid";
            SqlParameter[] par = { new SqlParameter("@ruserid", SqlDbType.VarChar, 50) { Value = userid } };

            return DbHelperSQL.Query(sql, par);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select bc.* ,hc.name as userName");
            strSql.Append(" FROM BusinessToRemindUserConfig(nolock) as bc ");
            strSql.Append("  INNER JOIN hr_employee(nolock) as hc ON hc.id = bc.userid ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();
            strSql_total.Append(" SELECT COUNT(id) FROM BusinessToRemindUserConfig as bc ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");

            strSql_grid.Append(" FROM (select bc.*,hc.name as userName , ROW_NUMBER() OVER( Order by bc.id ) AS n from BusinessToRemindUserConfig(nolock) as bc inner join hr_employee(nolock) as hc on hc.id = bc.userid ");
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



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(Model.BusinessToRemindUserConfig model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BusinessToRemindUserConfig(");
            strSql.Append("userid,create_id,create_time,remindType,remark,rcount)");
            strSql.Append(" values (");
            strSql.Append("@userid,@create_id,@create_time,@remindType,@remark,@rcount)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@userid", SqlDbType.VarChar,50),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@remindType", SqlDbType.Int,4),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@rcount", SqlDbType.Int,4)};
            parameters[0].Value = model.userid;
            parameters[1].Value = model.create_id;
            parameters[2].Value = model.create_time;
            parameters[3].Value = model.remindType;
            parameters[4].Value = model.remark;
            parameters[5].Value = model.rcount;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.BusinessToRemindUserConfig model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update BusinessToRemindUserConfig set ");
            strSql.Append("create_id=@create_id,");
            strSql.Append("create_time=@create_time,");
            strSql.Append("remark=@remark,");
            strSql.Append("rcount=@rcount");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@rcount", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.BigInt,8),
                    new SqlParameter("@userid", SqlDbType.VarChar,50),
                    new SqlParameter("@remindType", SqlDbType.Int,4)};
            parameters[0].Value = model.create_id;
            parameters[1].Value = model.create_time;
            parameters[2].Value = model.remark;
            parameters[3].Value = model.rcount;
            parameters[4].Value = model.id;
            parameters[5].Value = model.userid;
            parameters[6].Value = model.remindType;

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
        public bool Delete(long id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BusinessToRemindUserConfig ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt)
            };
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(long id, string userid, int remindType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from BusinessToRemindUserConfig ");
            strSql.Append(" where id=@id and userid=@userid and remindType=@remindType ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt,8),
                    new SqlParameter("@userid", SqlDbType.VarChar,50),
                    new SqlParameter("@remindType", SqlDbType.Int,4)          };
            parameters[0].Value = id;
            parameters[1].Value = userid;
            parameters[2].Value = remindType;

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
            strSql.Append("delete from BusinessToRemindUserConfig ");
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

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

