
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:SMS
	/// </summary>
	public partial class SMS
	{
		public SMS()
		{}
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.SMS model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SMS(");
            strSql.Append("id,sms_title,sms_content,contact_ids,sms_mobiles,isSend,sendtime,check_id,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@sms_title,@sms_content,@contact_ids,@sms_mobiles,@isSend,@sendtime,@check_id,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@sms_title", SqlDbType.NVarChar,250),
                    new SqlParameter("@sms_content", SqlDbType.NVarChar,-1),
                    new SqlParameter("@contact_ids", SqlDbType.VarChar,-1),
                    new SqlParameter("@sms_mobiles", SqlDbType.VarChar,-1),
                    new SqlParameter("@isSend", SqlDbType.Int,4),
                    new SqlParameter("@sendtime", SqlDbType.DateTime),
                    new SqlParameter("@check_id", SqlDbType.VarChar,50),                    
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.sms_title;
            parameters[2].Value = model.sms_content;
            parameters[3].Value = model.contact_ids;
            parameters[4].Value = model.sms_mobiles;
            parameters[5].Value = model.isSend;
            parameters[6].Value = model.sendtime;
            parameters[7].Value = model.check_id;
            parameters[8].Value = model.create_id;
            parameters[9].Value = model.create_time;

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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SMS ");
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
			strSql.Append("delete from SMS ");
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
			strSql.Append("select id,sms_title,sms_content,contact_ids,sms_mobiles,isSend,sendtime,check_id,create_id,create_time ");
			strSql.Append(" FROM SMS ");
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
			strSql.Append(" id,sms_title,sms_content,contact_ids,sms_mobiles,isSend,sendtime,check_id,create_id,create_time ");
			strSql.Append(" FROM SMS ");
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
			strSql_total.Append(" SELECT COUNT(id) FROM SMS ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,sms_title,sms_content,contact_ids,sms_mobiles,isSend,sendtime,check_id,create_id,create_time ");
			strSql_grid.Append(" FROM ( SELECT id,sms_title,sms_content,contact_ids,sms_mobiles,isSend,sendtime,check_id,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from SMS");
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
        /// <summary>
        /// 审核
        /// </summary>
        public bool check(XHD.Model.SMS model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SMS set ");            
            strSql.Append("isSend=@isSend,");
            strSql.Append("sendtime=@sendtime,");
            strSql.Append("check_id=@check_id");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {  
                    new SqlParameter("@isSend", SqlDbType.Int,4),
                    new SqlParameter("@sendtime", SqlDbType.DateTime),
                    new SqlParameter("@check_id", SqlDbType.VarChar,50),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};            
            parameters[0].Value = model.isSend;
            parameters[1].Value = model.sendtime;
            parameters[2].Value = model.check_id;
            parameters[3].Value = model.id;

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
        #endregion  ExtensionMethod
    }
}

