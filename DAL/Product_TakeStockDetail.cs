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
    /// 数据访问类:Product_TakeStockDetail
    /// </summary>
    public partial class Product_TakeStockDetail
    {
        public Product_TakeStockDetail()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_TakeStockDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_TakeStockDetail(");
            strSql.Append("id,takeid,warehouse_id,barcode,taketime,status,remark)");
            strSql.Append(" values (");
            strSql.Append("@id,@takeid,@warehouse_id,@barcode,@taketime,@status,@remark)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@takeid", SqlDbType.VarChar,50),
                    new SqlParameter("@warehouse_id", SqlDbType.Int,4),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@taketime", SqlDbType.DateTime),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@remark", SqlDbType.VarChar,150)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.takeid;
            parameters[2].Value = model.warehouse_id;
            parameters[3].Value = model.barcode;
            parameters[4].Value = model.taketime;
            parameters[5].Value = model.status;
            parameters[6].Value = model.remark;

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
        public bool Update(Model.Product_TakeStockDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_TakeStockDetail set ");
            strSql.Append("warehouse_id=@warehouse_id,");
            strSql.Append("taketime=@taketime,");
            strSql.Append("status=@status,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id and takeid=@takeid and barcode=@barcode ");
            SqlParameter[] parameters = {
                    new SqlParameter("@warehouse_id", SqlDbType.Int,4),
                    new SqlParameter("@taketime", SqlDbType.DateTime),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@remark", SqlDbType.VarChar,150),
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@takeid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50)};
            parameters[0].Value = model.warehouse_id;
            parameters[1].Value = model.taketime;
            parameters[2].Value = model.status;
            parameters[3].Value = model.remark;
            parameters[4].Value = model.id;
            parameters[5].Value = model.takeid;
            parameters[6].Value = model.barcode;

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
        public bool Delete(string id,string takeid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_TakeStockDetail ");
            strSql.Append(" where  id=@id and takeid=@takeid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                     new SqlParameter("@takeid", SqlDbType.VarChar,50),
     };
            parameters[0].Value = id;
            parameters[1].Value = takeid;
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
        public bool DeleteAllByTakeID(string takeid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_TakeStockDetail ");
            strSql.Append(" where  takeid=@takeid   ");
            SqlParameter[] parameters = {
                    new SqlParameter("@takeid", SqlDbType.VarChar,50),   };
            parameters[0].Value = takeid;

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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,takeid,warehouse_id,barcode,taketime,status,remark ");
            strSql.Append(" FROM Product_TakeStockDetail ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM view_TakeStock(nolock)  ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(" FROM (select  *, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from view_TakeStock(nolock)");
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

