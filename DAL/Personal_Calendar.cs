
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Personal_Calendar
	/// </summary>
	public partial class Personal_Calendar
	{
		public Personal_Calendar()
		{}
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Personal_Calendar model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Personal_Calendar(");
            strSql.Append("id,emp_id,customer_id,Subject,MasterId,CalendarType,StartTime,EndTime,IsAllDayEvent,Category,InstanceType,UPAccount,UPName,UPTime)");
            strSql.Append(" values (");
            strSql.Append("@id,@emp_id,@customer_id,@Subject,@MasterId,@CalendarType,@StartTime,@EndTime,@IsAllDayEvent,@Category,@InstanceType,@UPAccount,@UPName,@UPTime)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@customer_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Subject", SqlDbType.VarChar,-1),
                    new SqlParameter("@MasterId", SqlDbType.Int,4),
                    new SqlParameter("@CalendarType", SqlDbType.TinyInt,1),
                    new SqlParameter("@StartTime", SqlDbType.DateTime),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@IsAllDayEvent", SqlDbType.Bit,1),
                    new SqlParameter("@Category", SqlDbType.Int,4),
                    new SqlParameter("@InstanceType", SqlDbType.TinyInt,1),
                    new SqlParameter("@UPAccount", SqlDbType.VarChar,250),
                    new SqlParameter("@UPName", SqlDbType.VarChar,250),
                    new SqlParameter("@UPTime", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.emp_id;
            parameters[2].Value = model.customer_id;
            parameters[3].Value = model.Subject;
            parameters[4].Value = model.MasterId;
            parameters[5].Value = model.CalendarType;
            parameters[6].Value = model.StartTime;
            parameters[7].Value = model.EndTime;
            parameters[8].Value = model.IsAllDayEvent;
            parameters[9].Value = model.Category;
            parameters[10].Value = model.InstanceType;
            parameters[11].Value = model.UPAccount;
            parameters[12].Value = model.UPName;
            parameters[13].Value = model.UPTime;

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
		public bool Update(XHD.Model.Personal_Calendar model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Personal_Calendar set ");
            strSql.Append("customer_id=@customer_id,");
            strSql.Append("Subject=@Subject,");            
            strSql.Append("StartTime=@StartTime,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("IsAllDayEvent=@IsAllDayEvent,");
            strSql.Append("Category=@Category,");
            strSql.Append("InstanceType=@InstanceType,");
            strSql.Append("UPTime=@UPTime");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {                    
                    new SqlParameter("@customer_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Subject", SqlDbType.VarChar,-1),
                    new SqlParameter("@MasterId", SqlDbType.Int,4),
                    new SqlParameter("@CalendarType", SqlDbType.TinyInt,1),
                    new SqlParameter("@StartTime", SqlDbType.DateTime),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@IsAllDayEvent", SqlDbType.Bit,1),
                    new SqlParameter("@Category", SqlDbType.Int,4),
                    new SqlParameter("@InstanceType", SqlDbType.TinyInt,1),
                    new SqlParameter("@UPAccount", SqlDbType.VarChar,250),
                    new SqlParameter("@UPName", SqlDbType.VarChar,250),
                    new SqlParameter("@UPTime", SqlDbType.DateTime),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
           
            parameters[0].Value = model.customer_id;
            parameters[1].Value = model.Subject;
            parameters[2].Value = model.MasterId;
            parameters[3].Value = model.CalendarType;
            parameters[4].Value = model.StartTime;
            parameters[5].Value = model.EndTime;
            parameters[6].Value = model.IsAllDayEvent;
            parameters[7].Value = model.Category;
            parameters[8].Value = model.InstanceType;
            parameters[9].Value = model.UPAccount;
            parameters[10].Value = model.UPName;
            parameters[11].Value = DateTime.Now;
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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Personal_Calendar ");
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
			strSql.Append("delete from Personal_Calendar ");
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
        /// 得到一个对象实体
        /// </summary>
        public XHD.Model.Personal_Calendar GetModel(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,emp_id,customer_id,Subject,MasterId,CalendarType,StartTime,EndTime,IsAllDayEvent,Category,InstanceType,UPAccount,UPName,UPTime  from Personal_Calendar ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)           };
            parameters[0].Value = id;

            XHD.Model.Personal_Calendar model = new XHD.Model.Personal_Calendar();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XHD.Model.Personal_Calendar DataRowToModel(DataRow row)
        {
            XHD.Model.Personal_Calendar model = new XHD.Model.Personal_Calendar();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString();
                }
                if (row["emp_id"] != null)
                {
                    model.emp_id = row["emp_id"].ToString();
                }
               
                if (row["customer_id"] != null)
                {
                    model.customer_id = row["customer_id"].ToString();
                }
                if (row["Subject"] != null)
                {
                    model.Subject = row["Subject"].ToString();
                }
                
                if (row["MasterId"] != null && row["MasterId"].ToString() != "")
                {
                    model.MasterId = int.Parse(row["MasterId"].ToString());
                }
               
                if (row["CalendarType"] != null && row["CalendarType"].ToString() != "")
                {
                    model.CalendarType = int.Parse(row["CalendarType"].ToString());
                }
                if (row["StartTime"] != null && row["StartTime"].ToString() != "")
                {
                    model.StartTime = DateTime.Parse(row["StartTime"].ToString());
                }
                if (row["EndTime"] != null && row["EndTime"].ToString() != "")
                {
                    model.EndTime = DateTime.Parse(row["EndTime"].ToString());
                }
                if (row["IsAllDayEvent"] != null && row["IsAllDayEvent"].ToString() != "")
                {
                    if ((row["IsAllDayEvent"].ToString() == "1") || (row["IsAllDayEvent"].ToString().ToLower() == "true"))
                    {
                        model.IsAllDayEvent = true;
                    }
                    else
                    {
                        model.IsAllDayEvent = false;
                    }
                }
                       
                if (row["Category"] != null && row["Category"].ToString() != "")
                {
                    model.InstanceType = int.Parse(row["Category"].ToString());
                }
                if (row["InstanceType"] != null && row["InstanceType"].ToString() != "")
                {
                    model.InstanceType = int.Parse(row["InstanceType"].ToString());
                }
                
                if (row["UPAccount"] != null)
                {
                    model.UPAccount = row["UPAccount"].ToString();
                }
                if (row["UPName"] != null)
                {
                    model.UPName = row["UPName"].ToString();
                }
                if (row["UPTime"] != null && row["UPTime"].ToString() != "")
                {
                    model.UPTime = DateTime.Parse(row["UPTime"].ToString());
                }
                
               
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,emp_id,customer_id,Subject,MasterId,CalendarType,StartTime,EndTime,IsAllDayEvent,Category,InstanceType,UPAccount,UPName,UPTime  ");
			strSql.Append(" FROM Personal_Calendar ");
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
			strSql.Append(" id,emp_id,customer_id,Subject,MasterId,CalendarType,StartTime,EndTime,IsAllDayEvent,Category,InstanceType,UPAccount,UPName,UPTime  ");
			strSql.Append(" FROM Personal_Calendar ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Personal_Calendar ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,emp_id,customer_id,Subject,MasterId,CalendarType,StartTime,EndTime,IsAllDayEvent,Category,InstanceType,UPAccount,UPName,UPTime ");
            strSql_grid.Append(" FROM ( SELECT id,emp_id,customer_id,Subject,MasterId,CalendarType,StartTime,EndTime,IsAllDayEvent,Category,InstanceType,UPAccount,UPName,UPTime, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Personal_Calendar");
            if (strWhere.Trim() != "")
            {
                strSql_grid.Append(" WHERE " + strWhere);
                strSql_total.Append(" WHERE " + strWhere);
            }
            strSql_grid.Append("  ) as w1  ");
            strSql_grid.Append("WHERE n BETWEEN " + PageSize * (PageIndex - 1) + " AND " + PageSize * PageIndex);
            strSql_grid.Append(" ORDER BY " + filedOrder);
            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }
        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        ///     更新一条数据
        /// </summary>
        public bool quickUpdate(Model.Personal_Calendar model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update Personal_Calendar set ");
            strSql.Append("MasterId=@MasterId,");
            strSql.Append("StartTime=@StartTime,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("UPAccount=@UPAccount,");
            strSql.Append("UPTime=@UPTime");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@MasterId", SqlDbType.Int, 4),
                new SqlParameter("@StartTime", SqlDbType.DateTime),
                new SqlParameter("@EndTime", SqlDbType.DateTime),
                new SqlParameter("@UPAccount", SqlDbType.VarChar, 250),
                new SqlParameter("@UPTime", SqlDbType.DateTime),
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };
            parameters[0].Value = model.MasterId;
            parameters[1].Value = model.StartTime;
            parameters[2].Value = model.EndTime;
            parameters[3].Value = model.UPAccount;
            parameters[4].Value = model.UPTime;
            parameters[5].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }
        #endregion  ExtensionMethod
    }
}

