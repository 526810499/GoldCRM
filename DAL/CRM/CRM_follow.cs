
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:CRM_follow
    /// </summary>
    public partial class CRM_follow
    {
        public CRM_follow()
        { }
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.CRM_follow model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CRM_follow(");
            strSql.Append("id,customer_id,contact_id,follow_aim_id,follow_type_id,follow_content,follow_time,employee_id)");
            strSql.Append(" values (");
            strSql.Append("@id,@customer_id,@contact_id,@follow_aim_id,@follow_type_id,@follow_content,@follow_time,@employee_id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@customer_id", SqlDbType.VarChar,50),
                    new SqlParameter("@contact_id", SqlDbType.VarChar,50),
                    new SqlParameter("@follow_aim_id", SqlDbType.VarChar,50),
                    new SqlParameter("@follow_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@follow_content", SqlDbType.VarChar,-1),
                    new SqlParameter("@follow_time", SqlDbType.DateTime),
                    new SqlParameter("@employee_id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.customer_id;
            parameters[2].Value = model.contact_id;
            parameters[3].Value = model.follow_aim_id;
            parameters[4].Value = model.follow_type_id;
            parameters[5].Value = model.follow_content;
            parameters[6].Value = model.follow_time;
            parameters[7].Value = model.employee_id;

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
		public bool Update(XHD.Model.CRM_follow model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CRM_follow set ");
            strSql.Append("contact_id=@contact_id,");
            strSql.Append("follow_aim_id=@follow_aim_id,");
            strSql.Append("follow_type_id=@follow_type_id,");
            strSql.Append("follow_content=@follow_content");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@contact_id", SqlDbType.VarChar,50),
                    new SqlParameter("@follow_aim_id", SqlDbType.VarChar,50),
                    new SqlParameter("@follow_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@follow_content", SqlDbType.VarChar,-1),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.contact_id;
            parameters[1].Value = model.follow_aim_id;
            parameters[2].Value = model.follow_type_id;
            parameters[3].Value = model.follow_content;
            parameters[4].Value = model.id;

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
            strSql.Append("delete from CRM_follow ");
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
            strSql.Append("delete from CRM_follow ");
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
            strSql.Append("select id,customer_id,contact_id,follow_aim_id,follow_type_id,follow_content,follow_time,employee_id ");
            strSql.Append(",( select C_name from CRM_Contact where id=CRM_follow.[contact_id]) as contact_name ");
            strSql.Append(",( select cus_name from CRM_Customer where id=CRM_follow.[customer_id]) as Customer_name ");
            strSql.Append(",( select params_name from Sys_Param where id = CRM_follow.[follow_type_id]) as Follow_Type ");
            strSql.Append(",( select dep_name from hr_department where id = (select dep_id from hr_employee where id = CRM_follow.employee_id)) as [Department] ");
            strSql.Append(",( select name from hr_employee where id=CRM_follow.[employee_id]) as [Employee] ");
            strSql.Append(",( select params_name from Sys_Param where id = CRM_follow.follow_aim_id) as Follow_aim ");
            strSql.Append(" FROM CRM_follow ");
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
            strSql.Append(" id,customer_id,contact_id,follow_aim_id,follow_type_id,follow_content,follow_time,employee_id ");
            strSql.Append(",( select C_name from CRM_Contact where id=CRM_follow.[contact_id]) as contact_name ");
            strSql.Append(",( select cus_name from CRM_Customer where id=CRM_follow.[customer_id]) as Customer_name ");
            strSql.Append(",( select params_name from Sys_Param where id = CRM_follow.[follow_type_id]) as Follow_Type ");
            strSql.Append(",( select dep_name from hr_department where id = (select dep_id from hr_employee where id = CRM_follow.employee_id)) as [Department] ");
            strSql.Append(",( select name from hr_employee where id=CRM_follow.[employee_id]) as [Employee] ");
            strSql.Append(",( select params_name from Sys_Param where id = CRM_follow.follow_aim_id) as Follow_aim ");
            strSql.Append(" FROM CRM_follow ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM CRM_follow ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,customer_id,contact_id,follow_aim_id,follow_type_id,follow_content,follow_time,employee_id ");
            strSql_grid.Append(",( select C_name from CRM_Contact where id=w1.[contact_id]) as contact_name ");
            strSql_grid.Append(",( select cus_name from CRM_Customer where id=w1.[customer_id]) as Customer_name ");
            strSql_grid.Append(",( select params_name from Sys_Param where id = w1.[follow_type_id]) as Follow_Type ");
            strSql_grid.Append(",( select dep_name from hr_department where id = (select dep_id from hr_employee where id = w1.employee_id)) as [Department] ");
            strSql_grid.Append(",( select name from hr_employee where id=w1.[employee_id]) as [Employee] ");
            strSql_grid.Append(",( select params_name from Sys_Param where id = w1.follow_aim_id) as Follow_aim ");
            strSql_grid.Append(" FROM ( SELECT id,customer_id,contact_id,follow_aim_id,follow_type_id,follow_content,follow_time,employee_id, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from CRM_follow");
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
        public DataSet Reports_year(string items, int year, string where)
        {
            Random rnd = new Random();

            string temptable = "#temp_follow_" + rnd.Next(1,100);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("if OBJECT_ID('Tempdb.." + temptable + "') is not null ");
            strSql.Append("    drop TABLE " + temptable);
            //strSql.Append("go");
            strSql.Append(" begin ");
            //strSql.Append("    --预统计表 #t");
            strSql.Append("    select ");
            strSql.Append("        params_name,'m'+convert(varchar,month(follow_time)) mm,count(id)tNum into  " + temptable);
            strSql.Append("   from (select w1.*,(select params_name from Sys_Param where id = w1." + items + "_id) as params_name from CRM_follow w1 ");
            strSql.Append("    where datediff(YEAR,w1.[follow_time],'" + year + "-1-1')=0 ) w2 ");
            if (where.Trim() != "")
            {
                strSql.Append(" where " + where);
            }
            strSql.Append("    group by w2.params_name,'m'+convert(varchar,month(w2.follow_time)) ");

            //strSql.Append("    --生成SQL");
            strSql.Append("    declare @sql varchar(8000) ");
            strSql.Append("    set @sql='select params_name items ' ");
            strSql.Append("    select @sql = @sql + ',sum(case mm when ' + char(39) +mm+ char(39) + ' then tNum else 0 end) ['+ mm +']' ");
            strSql.Append("        from (select distinct mm from " + temptable + ")as data ");
            strSql.Append("    set @sql = @sql + ' from " + temptable + " group by params_name' ");

            strSql.Append("    exec(@sql) ");
            strSql.Append(" end ");
            strSql.Append("    drop TABLE " + temptable);
            //strSql.Append("go");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 客户跟进【同比环比】
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataSet Compared_follow(string year1, string month1, string year2, string month2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select Follow_Type as yy, count(Follow_Type)as xx,");
            strSql.Append(" SUM(case when YEAR( follow_time)=('" + year1 + "') and MONTH(follow_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( follow_time)=('" + year2 + "') and MONTH(follow_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append(" FROM (select (select params_name from Sys_Param where id = CRM_follow.[Follow_Type_id]) as Follow_Type,* from CRM_follow) as w1 ");
            strSql.Append("group by Follow_Type");

            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet Compared_empcusfollow(string year1, string month1, string year2, string month2, string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select hr_employee.name as yy,");
            strSql.Append(" SUM(case when YEAR( CRM_follow.follow_time)=('" + year1 + "') and MONTH(CRM_follow.follow_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( CRM_follow.follow_time)=('" + year2 + "') and MONTH(CRM_follow.follow_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append(" from hr_employee left outer join CRM_follow ");
            strSql.Append(" on hr_employee.id=CRM_follow.employee_id ");
            strSql.Append(" where hr_employee.id in " + idlist);
            strSql.Append(" group by hr_employee.name,hr_employee.id ");
            strSql.Append(" order by hr_employee.id");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 客户跟进统计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet report_empfollow(int year, string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select name,yy,isnull([1],0) as 'm1',isnull([2],0) as 'm2',isnull([3],0) as 'm3',isnull([4],0) as 'm4',isnull([5],0) as 'm5',isnull([6],0) as 'm6',");
            strSql.Append(" isnull([7],0) as 'm7',isnull([8],0) as 'm8',isnull([9],0) as 'm9',isnull([10],0) as 'm10',isnull([11],0) as 'm11',isnull([12],0) as 'm12' ");
            strSql.Append(" from");
            strSql.Append(" (SELECT   hr_employee.id, hr_employee.name, COUNT(derivedtbl_1.id) AS cn, YEAR(derivedtbl_1.follow_time) AS yy, ");
            strSql.Append(" MONTH(derivedtbl_1.follow_time) AS mm");
            strSql.Append(" FROM      hr_employee LEFT OUTER JOIN");
            strSql.Append("  (SELECT   id, employee_id, follow_time");
            strSql.Append("  FROM      CRM_follow");
            strSql.Append("  WHERE  (YEAR(follow_time) = " + year + ")) AS derivedtbl_1 ON hr_employee.id = derivedtbl_1.employee_id");
            strSql.Append(" WHERE hr_employee.id in " + idlist);
            strSql.Append(" GROUP BY hr_employee.id, hr_employee.name, YEAR(derivedtbl_1.follow_time), MONTH(derivedtbl_1.follow_time)) as tt");
            strSql.Append(" pivot");
            strSql.Append(" (sum(cn) for mm in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12]))");
            strSql.Append(" as pvt");

            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  ExtensionMethod
    }
}

