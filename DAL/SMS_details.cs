
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:SMS_details
	/// </summary>
	public partial class SMS_details
	{
		public SMS_details()
		{}
        #region  BasicMethod


        /// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.SMS_details model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SMS_details(");
            strSql.Append("sms_id,contact_id,mobiles)");
            strSql.Append(" values (");
            strSql.Append("@sms_id,@contact_id,@mobiles)");
            SqlParameter[] parameters = {
                    new SqlParameter("@sms_id", SqlDbType.VarChar,50),
                    new SqlParameter("@contact_id", SqlDbType.VarChar,50),
                    new SqlParameter("@mobiles", SqlDbType.VarChar,20)};
            parameters[0].Value = model.sms_id;
            parameters[1].Value = model.contact_id;
            parameters[2].Value = model.mobiles;

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
        public bool Delete(string where)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SMS_details ");
			strSql.Append(" where "+where);
			SqlParameter[] parameters = {
			};

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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sms_id,contact_id,mobiles ");
            strSql.Append(" FROM SMS_details ");
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
            strSql.Append(" sms_id,contact_id,mobiles ");
            strSql.Append(" FROM SMS_details ");
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
            strSql_total.Append(" SELECT COUNT() FROM SMS_details ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,sms_id,contact_id,mobiles ");
            strSql_grid.Append(" FROM ( SELECT sms_id,contact_id,mobiles, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from SMS_details");
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

