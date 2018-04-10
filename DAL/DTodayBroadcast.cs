using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XHD.DBUtility;
using XHD.Model;

namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:TodayBroadcast
    /// </summary>
    public partial class DTodayBroadcast
    {
        public DTodayBroadcast()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.TodayBroadcast model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TodayBroadcast(");
            strSql.Append("TodayGlodPrice,TodayOtherPrice1,TodayOtherPrice2,TodayOtherPrice3,TodayOtherPrice4,OtherBrodcast,Remark,update_id,update_time,create_id,create_time,createdep_id)");
            strSql.Append(" values (");
            strSql.Append("@TodayGlodPrice,@TodayOtherPrice1,@TodayOtherPrice2,@TodayOtherPrice3,@TodayOtherPrice4,@OtherBrodcast,@Remark,@update_id,@update_time,@create_id,@create_time,@createdep_id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@TodayGlodPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice1", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice2", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice3", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice4", SqlDbType.Decimal,9),
                    new SqlParameter("@OtherBrodcast", SqlDbType.NVarChar,2500),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@update_id", SqlDbType.VarChar,50),
                    new SqlParameter("@update_time", SqlDbType.DateTime),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.TodayGlodPrice;
            parameters[1].Value = model.TodayOtherPrice1;
            parameters[2].Value = model.TodayOtherPrice2;
            parameters[3].Value = model.TodayOtherPrice3;
            parameters[4].Value = model.TodayOtherPrice4;
            parameters[5].Value = model.OtherBrodcast;
            parameters[6].Value = model.Remark;
            parameters[7].Value = model.update_id;
            parameters[8].Value = model.update_time;
            parameters[9].Value = model.create_id;
            parameters[10].Value = model.create_time;
            parameters[11].Value = model.createdep_id;

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
        /// 获取今日播报
        /// </summary>
        /// <returns></returns>
        public TodayBroadcast GetTodayBroadcast()
        {
            string sql = "select top 1 * from TodayBroadcast ";

            DataSet ds = DbHelperSQL.Query(sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ModelConvertHelper<TodayBroadcast>.ToModel(ds.Tables[0]);
            }
            return null;
        }


        /// <summary>
        /// 获取今日播报
        /// </summary>
        /// <returns></returns>
        public TodayBroadcast GetTodayBroadcast(long id)
        {
            string sql = "select  * from TodayBroadcast where id=" + id;


            DataSet ds = DbHelperSQL.Query(sql);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                return ModelConvertHelper<TodayBroadcast>.ToModel(ds.Tables[0]);
            }
            return null;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.TodayBroadcast model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TodayBroadcast set ");
            strSql.Append("TodayGlodPrice=@TodayGlodPrice,");
            strSql.Append("TodayOtherPrice1=@TodayOtherPrice1,");
            strSql.Append("TodayOtherPrice2=@TodayOtherPrice2,");
            strSql.Append("TodayOtherPrice3=@TodayOtherPrice3,");
            strSql.Append("TodayOtherPrice4=@TodayOtherPrice4,");
            strSql.Append("OtherBrodcast=@OtherBrodcast,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("update_id=@update_id,");
            strSql.Append("update_time=@update_time,");
            strSql.Append("create_id=@create_id,");
            strSql.Append("create_time=@create_time,");
            strSql.Append("createdep_id=@createdep_id");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@TodayGlodPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice1", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice2", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice3", SqlDbType.Decimal,9),
                    new SqlParameter("@TodayOtherPrice4", SqlDbType.Decimal,9),
                    new SqlParameter("@OtherBrodcast", SqlDbType.NVarChar,2500),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@update_id", SqlDbType.VarChar,50),
                    new SqlParameter("@update_time", SqlDbType.DateTime),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@id", SqlDbType.BigInt,8)};
            parameters[0].Value = model.TodayGlodPrice;
            parameters[1].Value = model.TodayOtherPrice1;
            parameters[2].Value = model.TodayOtherPrice2;
            parameters[3].Value = model.TodayOtherPrice3;
            parameters[4].Value = model.TodayOtherPrice4;
            parameters[5].Value = model.OtherBrodcast;
            parameters[6].Value = model.Remark;
            parameters[7].Value = model.update_id;
            parameters[8].Value = model.update_time;
            parameters[9].Value = model.create_id;
            parameters[10].Value = model.create_time;
            parameters[11].Value = model.createdep_id;
            parameters[12].Value = model.id;

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

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

