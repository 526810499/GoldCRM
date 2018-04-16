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
    /// 数据访问类:Product_TakeStock
    /// </summary>
    public partial class Product_TakeStock
    {
        public Product_TakeStock()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_TakeStock model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_TakeStock(");
            strSql.Append("id,takeType,status,update_id,update_time,authuser_id,authuser_time,create_time,create_id,remark,createdep_id,warehouse_id)");
            strSql.Append(" values (");
            strSql.Append("@id,@takeType,@status,@update_id,@update_time,@authuser_id,@authuser_time,@create_time,@create_id,@remark,@createdep_id,@warehouse_id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@takeType", SqlDbType.Int,4),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@update_id", SqlDbType.VarChar,50),
                    new SqlParameter("@update_time", SqlDbType.DateTime),
                    new SqlParameter("@authuser_id", SqlDbType.VarChar,50),
                    new SqlParameter("@authuser_time", SqlDbType.DateTime),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@remark", SqlDbType.NVarChar,150),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@warehouse_id",SqlDbType.Int,4)
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.takeType;
            parameters[2].Value = model.status;
            parameters[3].Value = model.update_id;
            parameters[4].Value = model.update_time;
            parameters[5].Value = model.authuser_id;
            parameters[6].Value = model.authuser_time;
            parameters[7].Value = model.create_time;
            parameters[8].Value = model.create_id;
            parameters[9].Value = model.remark;
            parameters[10].Value = model.createdep_id;
            parameters[11].Value = model.warehouse_id;

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
        public bool Update(Model.Product_TakeStock model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_TakeStock set ");
            strSql.Append("takeType=@takeType,");
            strSql.Append("status=@status,");
            strSql.Append("update_id=@update_id,");
            strSql.Append("update_time=@update_time,");
            strSql.Append("authuser_id=@authuser_id,");
            strSql.Append("authuser_time=@authuser_time,");
            strSql.Append("create_time=@create_time,");
            strSql.Append("create_id=@create_id,");
            strSql.Append("remark=@remark,");
            strSql.Append("createdep_id=@createdep_id");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@takeType", SqlDbType.Int,4),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@update_id", SqlDbType.VarChar,50),
                    new SqlParameter("@update_time", SqlDbType.DateTime),
                    new SqlParameter("@authuser_id", SqlDbType.VarChar,50),
                    new SqlParameter("@authuser_time", SqlDbType.DateTime),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@remark", SqlDbType.NVarChar,150),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.takeType;
            parameters[1].Value = model.status;
            parameters[2].Value = model.update_id;
            parameters[3].Value = model.update_time;
            parameters[4].Value = model.authuser_id;
            parameters[5].Value = model.authuser_time;
            parameters[6].Value = model.create_time;
            parameters[7].Value = model.create_id;
            parameters[8].Value = model.remark;
            parameters[9].Value = model.createdep_id;
            parameters[10].Value = model.id;

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
            strSql.Append("delete from Product_TakeStockDetail ");
            strSql.Append(" where takeid=@id ");

            strSql.Append("delete from Product_TakeStock ");
            strSql.Append(" where id=@id ");

            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
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
            strSql.Append("delete from Product_TakeStock ");
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
            strSql.Append("select id,takeType,status,update_id,update_time,authuser_id,authuser_time,create_time,create_id,remark,createdep_id,warehouse_id ");
            strSql.Append(" FROM Product_TakeStock ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Product_TakeStock(nolock) ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(",(select name from hr_employee where id = w1.create_id) as CreateName");
            strSql_grid.Append(",(select dep_name from hr_department(nolock) where id = w1.createdep_id) as dep_name");
            strSql_grid.Append(",(select product_warehouse from Product_warehouse(nolock) where id = w1.warehouse_id) as product_warehouse");
            strSql_grid.Append(" FROM (select  *, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_TakeStock(nolock) ");
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

