using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XHD.DBUtility;

namespace XHD.DAL
{
    // <summary>
    /// 数据访问类:Product_RetrievalDetail
    /// </summary>
    public partial class DProduct_RetrievalDetail
    {
        public DProduct_RetrievalDetail()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_RetrievalDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_RetrievalDetail(");
            strSql.Append("id,weight,number,category_id,retrid)");
            strSql.Append(" values (");
            strSql.Append("@id,@weight,@number,@category_id,@retrid)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@weight", SqlDbType.Decimal,9),
                    new SqlParameter("@number", SqlDbType.Int,4),
                    new SqlParameter("@category_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@retrid", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.weight;
            parameters[2].Value = model.number;
            parameters[3].Value = model.category_id;
            parameters[4].Value = model.retrid;

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
        /// 增加一条数据
        /// </summary>
        public bool Update(Model.Product_RetrievalDetail model)
        {
            string sql = @"update Product_RetrievalDetail set weight=@weight,number=@number,category_id=@category_id where id=@id and retrid=@retrid";

            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=model.id},
                    new SqlParameter("@weight", SqlDbType.Decimal,9) { Value=model.weight},
                    new SqlParameter("@number", SqlDbType.Int,4) { Value=model.number},
                    new SqlParameter("@category_id", SqlDbType.NVarChar,50) { Value=model.category_id},
                    new SqlParameter("@retrid", SqlDbType.NVarChar,50) { Value=model.retrid}

            };


            int rows = DbHelperSQL.ExecuteSql(sql, parameters);
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();
            strSql_total.Append(" SELECT COUNT(id) FROM Product_RetrievalDetail ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(",(select product_category from Product_category(nolock) where id = w1.category_id) as categoryName");
            strSql_grid.Append(" FROM ( select Product_RetrievalDetail.*, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_RetrievalDetail");
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string retrid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_RetrievalDetail ");
            strSql.Append(" where id=@id  and retrid=@retrid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=id},
                   new SqlParameter("@retrid", SqlDbType.NVarChar,50) { Value=retrid}
            };


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
        public bool DeleteAll(string retrid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_RetrievalDetail ");
            strSql.Append(" where   retrid=@retrid ");
            SqlParameter[] parameters = {
                   new SqlParameter("@retrid", SqlDbType.NVarChar,50) { Value=retrid}
            };


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
            strSql.Append("delete from Product_RetrievalDetail ");
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
            strSql.Append("delete from Product_RetrievalDetail ");
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

