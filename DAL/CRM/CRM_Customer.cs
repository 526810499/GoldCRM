
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:CRM_Customer
    /// </summary>
    public partial class CRM_Customer
    {
        public CRM_Customer()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CRM_Customer");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)           };
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 修改用户积分
        /// </summary>
        /// <param name="id"></param>
        /// <param name="integral"></param>
        /// <returns></returns>
        public bool UpdateIntegral(string id, int integral)
        {
            string sql = "update CRM_Customer set integral=isnull(integral,0)+@integral where id=@id";
            SqlParameter[] par = {
                new SqlParameter("@id",SqlDbType.VarChar,50) { Value=id},
                new SqlParameter("@integral",SqlDbType.Int) { Value=integral}
            };
            return DbHelperSQL.ExecuteSql(sql, par) > 0;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.CRM_Customer model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CRM_Customer(");
            strSql.Append("id,Serialnumber,cus_name,cus_add,cus_tel,cus_fax,cus_website,cus_industry_id,Provinces_id,City_id,cus_type_id,cus_level_id,cus_source_id,DesCripe,Remarks,emp_id,isPrivate,lastfollow,xy,isDelete,Delete_time,create_id,create_time,cus_extend,birthday,integral,birthdays,birthmonths,createdep_id,emp_depid)");
            strSql.Append(" values (");
            strSql.Append("@id,@Serialnumber,@cus_name,@cus_add,@cus_tel,@cus_fax,@cus_website,@cus_industry_id,@Provinces_id,@City_id,@cus_type_id,@cus_level_id,@cus_source_id,@DesCripe,@Remarks,@emp_id,@isPrivate,@lastfollow,@xy,@isDelete,@Delete_time,@create_id,@create_time,@cus_extend,@birthday,@integral,@birthdays,@birthmonths,@createdep_id,@emp_depid)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_name", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_add", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_tel", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_fax", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_website", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_industry_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Provinces_id", SqlDbType.VarChar,50),
                    new SqlParameter("@City_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_level_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_source_id", SqlDbType.VarChar,50),
                    new SqlParameter("@DesCripe", SqlDbType.VarChar,4000),
                    new SqlParameter("@Remarks", SqlDbType.VarChar,4000),
                    new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@isPrivate", SqlDbType.Int,4),
                    new SqlParameter("@lastfollow", SqlDbType.DateTime),
                    new SqlParameter("@xy", SqlDbType.VarChar,50),
                    new SqlParameter("@isDelete", SqlDbType.Int,4),
                    new SqlParameter("@Delete_time", SqlDbType.DateTime),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@cus_extend",SqlDbType.VarChar,-1),
                    new SqlParameter("@birthday",SqlDbType.DateTime),
                    new SqlParameter("@integral",SqlDbType.Int,4),
                   new SqlParameter("@birthdays",SqlDbType.Int,4),
                  new SqlParameter("@birthmonths",SqlDbType.Int,4),
                  new SqlParameter("@birthyear",SqlDbType.Int,4),
                  new SqlParameter("@createdep_id",SqlDbType.VarChar,50),
                 new SqlParameter("@emp_depid",SqlDbType.VarChar,50),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.Serialnumber;
            parameters[2].Value = model.cus_name;
            parameters[3].Value = model.cus_add;
            parameters[4].Value = model.cus_tel;
            parameters[5].Value = model.cus_fax;
            parameters[6].Value = model.cus_website;
            parameters[7].Value = model.cus_industry_id;
            parameters[8].Value = model.Provinces_id;
            parameters[9].Value = model.City_id;
            parameters[10].Value = model.cus_type_id;
            parameters[11].Value = model.cus_level_id;
            parameters[12].Value = model.cus_source_id;
            parameters[13].Value = model.DesCripe;
            parameters[14].Value = model.Remarks;
            parameters[15].Value = model.emp_id;
            parameters[16].Value = model.isPrivate;
            parameters[17].Value = model.lastfollow;
            parameters[18].Value = model.xy;
            parameters[19].Value = model.isDelete;
            parameters[20].Value = model.Delete_time;
            parameters[21].Value = model.create_id;
            parameters[22].Value = model.create_time;
            parameters[23].Value = model.cus_extend;
            parameters[24].Value = model.birthday;
            parameters[25].Value = model.integral;
            parameters[26].Value = (model.birthday.HasValue && model.birthday != DateTime.MaxValue ? (model.birthday.CDateTime(DateTime.Now, false).Day).CInt(0, false) : 0);
            parameters[27].Value = (model.birthday.HasValue && model.birthday != DateTime.MaxValue ? (model.birthday.CDateTime(DateTime.Now, false).Month).CInt(0, false) : 0);
            parameters[28].Value = (model.birthday.HasValue && model.birthday != DateTime.MaxValue ? (model.birthday.CDateTime(DateTime.Now, false).Year).CInt(0, false) : 0);
            parameters[29].Value = model.createdep_id;
            parameters[30].Value = model.emp_depid;
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
        public bool Update(XHD.Model.CRM_Customer model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CRM_Customer set ");
            strSql.Append("Serialnumber=@Serialnumber,");
            strSql.Append("cus_name=@cus_name,");
            strSql.Append("cus_add=@cus_add,");
            strSql.Append("cus_tel=@cus_tel,");
            strSql.Append("cus_fax=@cus_fax,");
            strSql.Append("cus_website=@cus_website,");
            strSql.Append("cus_industry_id=@cus_industry_id,");
            strSql.Append("Provinces_id=@Provinces_id,");
            strSql.Append("City_id=@City_id,");
            strSql.Append("cus_type_id=@cus_type_id,");
            strSql.Append("cus_level_id=@cus_level_id,");
            strSql.Append("cus_source_id=@cus_source_id,");
            strSql.Append("DesCripe=@DesCripe,");
            strSql.Append("Remarks=@Remarks,");
            strSql.Append("emp_id=@emp_id,");
            strSql.Append("isPrivate=@isPrivate,");
            strSql.Append("birthday=@birthday,");
            strSql.Append("cus_extend=@cus_extend,");
            strSql.Append("xy=@xy");
            strSql.Append(",integral=@integral,birthdays=@birthdays,birthmonths=@birthmonths,birthyear=@birthyear ");
            strSql.Append(",emp_depid=@emp_depid");


            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_name", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_add", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_tel", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_fax", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_website", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_industry_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Provinces_id", SqlDbType.VarChar,50),
                    new SqlParameter("@City_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_level_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_source_id", SqlDbType.VarChar,50),
                    new SqlParameter("@DesCripe", SqlDbType.VarChar,4000),
                    new SqlParameter("@Remarks", SqlDbType.VarChar,4000),
                    new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@isPrivate", SqlDbType.Int,4),
                    new SqlParameter("@xy", SqlDbType.VarChar,50),
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_extend",SqlDbType.VarChar,-1),
                   new SqlParameter("@birthday",SqlDbType.DateTime),
                   new SqlParameter("@integral",SqlDbType.Int,4),
                   new SqlParameter("@birthdays",SqlDbType.Int,4),
                  new SqlParameter("@birthmonths",SqlDbType.Int,4),
                  new SqlParameter("@birthyear",SqlDbType.Int,4),
                   new SqlParameter("@emp_depid", SqlDbType.VarChar,50),

            };
            parameters[0].Value = model.Serialnumber;
            parameters[1].Value = model.cus_name;
            parameters[2].Value = model.cus_add;
            parameters[3].Value = model.cus_tel;
            parameters[4].Value = model.cus_fax;
            parameters[5].Value = model.cus_website;
            parameters[6].Value = model.cus_industry_id;
            parameters[7].Value = model.Provinces_id;
            parameters[8].Value = model.City_id;
            parameters[9].Value = model.cus_type_id;
            parameters[10].Value = model.cus_level_id;
            parameters[11].Value = model.cus_source_id;
            parameters[12].Value = model.DesCripe;
            parameters[13].Value = model.Remarks;
            parameters[14].Value = model.emp_id;
            parameters[15].Value = model.isPrivate;
            parameters[16].Value = model.xy;
            parameters[17].Value = model.id;
            parameters[18].Value = model.cus_extend;
            parameters[19].Value = model.birthday;
            parameters[20].Value = model.integral.CInt(0, false);
            parameters[21].Value = (model.birthday.HasValue && model.birthday != DateTime.MaxValue ? (model.birthday.CDateTime(DateTime.Now, false).Day).CInt(0, false) : 0);
            parameters[22].Value = (model.birthday.HasValue && model.birthday != DateTime.MaxValue ? (model.birthday.CDateTime(DateTime.Now, false).Month).CInt(0, false) : 0);
            parameters[23].Value = (model.birthday.HasValue && model.birthday != DateTime.MaxValue ? (model.birthday.CDateTime(DateTime.Now, false).Year).CInt(0, false) : 0);

            parameters[24].Value = model.emp_depid;
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
            strSql.Append("delete from CRM_Customer ");
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
            strSql.Append("delete from CRM_Customer ");
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
            strSql.Append("select integral,birthday,id,Serialnumber,cus_name,cus_add,cus_tel,cus_fax,cus_website,cus_industry_id,Provinces_id,City_id,cus_type_id,cus_level_id,cus_source_id,DesCripe,Remarks,emp_id,isPrivate,lastfollow,xy,isDelete,Delete_time,create_id,create_time,cus_extend,createdep_id,emp_depid ");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_industry_id) as cus_industry");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_level_id) as cus_level");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_type_id) as cus_type");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_source_id) as cus_source");
            strSql.Append(",(select City from Sys_Param_City where id = CRM_Customer.City_id) as City");
            strSql.Append(",(select Provinces from Sys_Param_Provinces where id = CRM_Customer.Provinces_id) as Provinces");
            strSql.Append(",(select dep_name from hr_department where id = (select dep_id from hr_employee where id = CRM_Customer.emp_id)) as department");
            strSql.Append(",(select name from hr_employee where id = CRM_Customer.emp_id) as employee");
            strSql.Append(" FROM CRM_Customer ");
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
            strSql.Append(" integral,birthday,id,Serialnumber,cus_name,cus_add,cus_tel,cus_fax,cus_website,cus_industry_id,Provinces_id,City_id,cus_type_id,cus_level_id,cus_source_id,DesCripe,Remarks,emp_id,isPrivate,lastfollow,xy,isDelete,Delete_time,create_id,create_time,cus_extend ");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_industry_id) as cus_industry");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_level_id) as cus_level");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_type_id) as cus_type");
            strSql.Append(",(select params_name from Sys_Param where id = CRM_Customer.cus_source_id) as cus_source");
            strSql.Append(",(select City from Sys_Param_City where id = CRM_Customer.City_id) as City");
            strSql.Append(",(select Provinces from Sys_Param_Provinces where id = CRM_Customer.Provinces_id) as Provinces");
            strSql.Append(",(select dep_name from hr_department where id = (select dep_id from hr_employee where id = CRM_Customer.emp_id)) as department");
            strSql.Append(",(select name from hr_employee where id = CRM_Customer.emp_id) as employee");
            strSql.Append(" FROM CRM_Customer ");
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
            StringBuilder strSql_inner = new StringBuilder();
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();

            //联合数据
            strSql_inner.Append("( SELECT integral,birthday,id, Serialnumber, cus_name, cus_add, cus_tel, cus_fax, cus_website, cus_industry_id, Provinces_id, City_id, cus_type_id, cus_level_id, cus_source_id, DesCripe, Remarks, emp_id, isPrivate, lastfollow, xy, isDelete, Delete_time, create_id, create_time,cus_extend  ");
            strSql_inner.Append("        ,cus_industry,cus_level, cus_type,cus_source,City,Provinces,department,employee");
            strSql_inner.Append($"        ,ROW_NUMBER() OVER(Order by {filedOrder}) AS n FROM ");
            strSql_inner.Append("   (");
            strSql_inner.Append("        SELECT  ");
            strSql_inner.Append("            * ");
            strSql_inner.Append("            , (select params_name from Sys_Param(nolock) where id = w1.cus_industry_id) as cus_industry ");
            strSql_inner.Append("            , (select params_name from Sys_Param(nolock) where id = w1.cus_level_id) as cus_level ");
            strSql_inner.Append("            , (select params_name from Sys_Param(nolock) where id = w1.cus_type_id) as cus_type ");
            strSql_inner.Append("            , (select params_name from Sys_Param(nolock) where id = w1.cus_source_id) as cus_source ");
            strSql_inner.Append("            , (select City from Sys_Param_City(nolock) where id = w1.City_id) as City ");
            strSql_inner.Append("            , (select Provinces from Sys_Param_Provinces(nolock) where id = w1.Provinces_id) as Provinces ");
            strSql_inner.Append("            , (select dep_name from hr_department(nolock) where id = (select dep_id from hr_employee(nolock) where id = w1.emp_id)) as department ");
            strSql_inner.Append("            , (select name from hr_employee where id = w1.emp_id) as employee ");
            strSql_inner.Append("        FROM(SELECT * from CRM_Customer(nolock) UNION SELECT * FROM CRM_Customer(nolock) where isPrivate = 1) as w1 ");
            strSql_inner.Append("   ) w2 ");
            if (strWhere.Trim() != "")
            {
                strSql_inner.Append(" WHERE " + strWhere);
            }
            strSql_inner.Append(") W3");

            //Total数据
            strSql_total.Append(" SELECT COUNT(ID) FROM ");
            strSql_total.Append(strSql_inner.ToString());

            //grid数据
            strSql_grid.Append(" SELECT * FROM ");
            strSql_grid.Append(strSql_inner.ToString());
            strSql_grid.Append(" WHERE n BETWEEN " + PageSize * (PageIndex - 1) + " AND " + PageSize * PageIndex);

            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        /// <summary>
        ///     批量转客户
        /// </summary>
        public bool Update_batch(Model.CRM_Customer model, string strWhere)
        {
            var strSql = new StringBuilder();
            strSql.Append("update CRM_Customer set ");
            strSql.Append("emp_id=@emp_id");
            strSql.Append(" where emp_id=@create_id");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            SqlParameter[] parameters =
            {
                new SqlParameter("@emp_id", SqlDbType.VarChar, 50),
                new SqlParameter("@create_id", SqlDbType.VarChar, 50)
            };

            parameters[0].Value = model.emp_id;
            parameters[1].Value = model.create_id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     预删除
        /// </summary>
        public bool AdvanceDelete(string id, int isDelete, string time)
        {
            var strSql = new StringBuilder();
            strSql.Append("update CRM_Customer set ");
            strSql.Append($"isDelete={isDelete}");
            strSql.Append($",Delete_time='{ time }'");
            strSql.Append($" where id='{id}'");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     更新最后跟进
        /// </summary>
        public bool UpdateLastFollow(string id)
        {
            var strSql = new StringBuilder();
            strSql.Append("update CRM_Customer set ");
            strSql.Append("[lastfollow] = (select max(follow_time) as follow_time from dbo.CRM_follow where CRM_Customer.id = CRM_follow.customer_id)");
            strSql.Append($" where CRM_Customer.id='{id}'");

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        public DataSet Reports_year(string items, int year, string where)
        {
            Random rnd = new Random();
            string temptable = "#temp_customer_" + rnd.Next(1, 100);

            var strSql = new StringBuilder();
            strSql.Append("if OBJECT_ID('Tempdb.." + temptable + "') is not null ");
            strSql.Append("    drop TABLE  " + temptable + " ");
            //strSql.Append("go");
            strSql.Append(" begin ");
            //strSql.Append("    --预统计表 "+temptable+"");
            strSql.Append("    select ");
            strSql.Append("        params_name,'m'+convert(varchar,month(create_time)) mm,count(id)tNum into " + temptable + " ");
            strSql.Append("   from (select w1.*,(select params_name from Sys_Param where id = w1." + items + "_id) as params_name from CRM_Customer w1 ");
            strSql.Append("    where datediff(YEAR,w1.[create_time],'" + year + "-1-1')=0 and isDelete=0 ) w2 ");
            if (where.Trim() != "")
            {
                strSql.Append(" where " + where);
            }
            strSql.Append("    group by w2.params_name,'m'+convert(varchar,month(w2.create_time)) ");

            //strSql.Append("    --生成SQL");
            strSql.Append("    declare @sql varchar(8000) ");
            strSql.Append("    set @sql='select params_name items ' ");
            strSql.Append("    select @sql = @sql + ',sum(case mm when ' + char(39) +mm+ char(39) + ' then tNum else 0 end) ['+ mm +']' ");
            strSql.Append("        from (select distinct mm from " + temptable + ")as data ");
            strSql.Append("    set @sql = @sql + ' from " + temptable + " group by params_name' ");

            strSql.Append("    exec(@sql) ");
            strSql.Append(" end ");

            strSql.Append("    drop TABLE  " + temptable + " ");

            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        ///     同比环比【客户新增】
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataSet Compared(string year1, string month1, string year2, string month2)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select count(id) as yy,");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year1 + "') and MONTH(create_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year2 + "') and MONTH(create_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append($" FROM CRM_Customer WHERE isDelete=0 ");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        ///     客户类型【同比环比】
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataSet Compared_type(string year1, string month1, string year2, string month2)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select CustomerType as yy,count(CustomerType) as xx,");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year1 + "') and MONTH(create_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year2 + "') and MONTH(create_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append($" FROM (select (select params_name from Sys_Param where id = CRM_Customer.[cus_type_id]) as CustomerType,* from CRM_Customer) as w1 ");
            strSql.Append($" WHERE isDelete=0");
            strSql.Append("group by CustomerType");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        ///     客户级别【同比环比】
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataSet Compared_level(string year1, string month1, string year2, string month2)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select CustomerLevel as yy,count(CustomerLevel) as xx,");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year1 + "') and MONTH(create_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year2 + "') and MONTH(create_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append(" FROM (select (select params_name from Sys_Param where id = CRM_Customer.[cus_level_id]) as CustomerLevel ,* from CRM_Customer) as w1");
            strSql.Append($" WHERE isDelete=0 ");
            strSql.Append("group by CustomerLevel");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        ///     客户来源【同比环比】
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataSet Compared_source(string year1, string month1, string year2, string month2)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select CustomerSource as yy,count(CustomerSource) as xx,");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year1 + "') and MONTH(create_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( create_time)=('" + year2 + "') and MONTH(create_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append(" FROM (select (select params_name from Sys_Param where id = CRM_Customer.[cus_Source_id]) as [CustomerSource],* from CRM_Customer) as w1 ");
            strSql.Append($" WHERE isDelete=0 ");
            strSql.Append("group by CustomerSource");

            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet Compared_empcusadd(string year1, string month1, string year2, string month2, string idlist)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select hr_employee.name as yy,");
            strSql.Append(" SUM(case when YEAR( CRM_Customer.create_time)=('" + year1 + "') and MONTH(CRM_Customer.create_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( CRM_Customer.create_time)=('" + year2 + "') and MONTH(CRM_Customer.create_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append(" FROM hr_employee left outer join  CRM_Customer ");
            strSql.Append(" on hr_employee.id = CRM_Customer.Create_id ");
            strSql.Append(" where  CRM_Customer.isDelete=0 and hr_employee.id in " + idlist);
            strSql.Append(" group by hr_employee.name,hr_employee.id ");
            //strSql.Append(" order by hr_employee.id ");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        ///     客户新增统计
        /// </summary>
        public DataSet report_empcus(int year, string idlist)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select name,yy,isnull([1],0) as 'm1',isnull([2],0) as 'm2',isnull([3],0) as 'm3',isnull([4],0) as 'm4',isnull([5],0) as 'm5',isnull([6],0) as 'm6',");
            strSql.Append(" isnull([7],0) as 'm7',isnull([8],0) as 'm8',isnull([9],0) as 'm9',isnull([10],0) as 'm10',isnull([11],0) as 'm11',isnull([12],0) as 'm12' ");
            strSql.Append(" from");
            strSql.Append(" (SELECT   hr_employee.id, hr_employee.name, COUNT(derivedtbl_1.id) AS cn, YEAR(derivedtbl_1.create_time) AS yy, ");
            strSql.Append(" MONTH(derivedtbl_1.create_time) AS mm");
            strSql.Append(" FROM      hr_employee LEFT OUTER JOIN");
            strSql.Append("  (SELECT   id, Create_id, create_time");
            strSql.Append("  FROM      CRM_Customer");
            strSql.Append("  WHERE isdelete=0 and  (YEAR(create_time) = " + year + ")) AS derivedtbl_1 ON hr_employee.id = derivedtbl_1.Create_id");
            strSql.Append(" WHERE hr_employee.id in " + idlist);
            strSql.Append(" GROUP BY hr_employee.id, hr_employee.name, YEAR(derivedtbl_1.create_time), MONTH(derivedtbl_1.create_time)) as tt");
            strSql.Append(" pivot");
            strSql.Append(" (sum(cn) for mm in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12]))");
            strSql.Append(" as pvt");

            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        ///     统计漏斗
        /// </summary>
        public DataSet Funnel(string strWhere, string year)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select * from ");
            strSql.Append("( ");
            strSql.Append("	select  ");
            strSql.Append("		a.params_name as CustomerType, ");
            strSql.Append("		a.id as CustomerType_id, ");
            strSql.Append("		a.params_order , ");
            strSql.Append("		COUNT(b.id) as cc  ");
            strSql.Append("	from  ");
            strSql.Append("		Sys_Param as a left join ( ");
            strSql.Append("			select * from CRM_Customer  ");

            if (year.Trim() != "")
            {
                strSql.Append("			where datediff(year,create_time,'" + year + "-01-01')=0  ");
            }

            strSql.Append("			)as b  ");
            strSql.Append("		on a.id = b.cus_type_id  ");
            strSql.Append("	where a.params_type = 'cus_type' ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" and  " + strWhere);
            }

            strSql.Append("	group by  ");
            strSql.Append("		a.params_name, ");
            strSql.Append("		a.id, ");
            strSql.Append("		a.params_order ");
            strSql.Append(") as t1 ");
            strSql.Append("order by params_order ");

            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetMapList(string strWhere)
        {
            var strSql = new StringBuilder();
            strSql.Append("SELECT ");
            strSql.Append("     [id],[cus_name],[cus_add],[cus_tel],[xy]     ");
            strSql.Append("FROM [dbo].[CRM_Customer] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 更新手机数据
        /// </summary>
        public bool UpdateApp(XHD.Model.CRM_Customer model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CRM_Customer set ");
            strSql.Append("cus_name=@cus_name,");
            strSql.Append("cus_add=@cus_add,");
            strSql.Append("cus_tel=@cus_tel,");
            strSql.Append("cus_fax=@cus_fax,");
            strSql.Append("cus_website=@cus_website,");
            strSql.Append("cus_industry_id=@cus_industry_id,");
            strSql.Append("Provinces_id=@Provinces_id,");
            strSql.Append("City_id=@City_id,");
            strSql.Append("cus_type_id=@cus_type_id,");
            strSql.Append("cus_level_id=@cus_level_id,");
            strSql.Append("cus_source_id=@cus_source_id,");
            strSql.Append("DesCripe=@DesCripe,");
            strSql.Append("Remarks=@Remarks,");
            strSql.Append("emp_id=@emp_id,");
            strSql.Append("isPrivate=@isPrivate");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_name", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_add", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_tel", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_fax", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_website", SqlDbType.VarChar,250),
                    new SqlParameter("@cus_industry_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Provinces_id", SqlDbType.VarChar,50),
                    new SqlParameter("@City_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_level_id", SqlDbType.VarChar,50),
                    new SqlParameter("@cus_source_id", SqlDbType.VarChar,50),
                    new SqlParameter("@DesCripe", SqlDbType.VarChar,4000),
                    new SqlParameter("@Remarks", SqlDbType.VarChar,4000),
                    new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@isPrivate", SqlDbType.Int,4),
                    new SqlParameter("@xy", SqlDbType.VarChar,50),
                    new SqlParameter("@id", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.Serialnumber;
            parameters[1].Value = model.cus_name;
            parameters[2].Value = model.cus_add;
            parameters[3].Value = model.cus_tel;
            parameters[4].Value = model.cus_fax;
            parameters[5].Value = model.cus_website;
            parameters[6].Value = model.cus_industry_id;
            parameters[7].Value = model.Provinces_id;
            parameters[8].Value = model.City_id;
            parameters[9].Value = model.cus_type_id;
            parameters[10].Value = model.cus_level_id;
            parameters[11].Value = model.cus_source_id;
            parameters[12].Value = model.DesCripe;
            parameters[13].Value = model.Remarks;
            parameters[14].Value = model.emp_id;
            parameters[15].Value = model.isPrivate;
            parameters[16].Value = model.xy;
            parameters[17].Value = model.id;

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

