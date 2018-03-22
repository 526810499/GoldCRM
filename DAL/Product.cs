
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Product
    /// </summary>
    public partial class Product
    {
        public Product()
        { }
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product(");
            strSql.Append("id,product_name,category_id,status,unit,cost,price,agio,remarks,specifications,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@product_name,@category_id,@status,@unit,@cost,@price,@agio,@remarks,@specifications,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@product_name", SqlDbType.VarChar,250),
                    new SqlParameter("@category_id", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.VarChar,250),
                    new SqlParameter("@unit", SqlDbType.VarChar,250),
                    new SqlParameter("@cost", SqlDbType.Decimal,9),
                    new SqlParameter("@price", SqlDbType.Decimal,9),
                    new SqlParameter("@agio", SqlDbType.Decimal,9),
                    new SqlParameter("@remarks", SqlDbType.VarChar,-1),
                    new SqlParameter("@specifications", SqlDbType.VarChar,250),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.product_name;
            parameters[2].Value = model.category_id;
            parameters[3].Value = model.status;
            parameters[4].Value = model.unit;
            parameters[5].Value = model.cost;
            parameters[6].Value = model.price;
            parameters[7].Value = model.agio;
            parameters[8].Value = model.remarks;
            parameters[9].Value = model.specifications;
            parameters[10].Value = model.create_id;
            parameters[11].Value = model.create_time;

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
        public bool Update(XHD.Model.Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product set ");
            strSql.Append("product_name=@product_name,");
            strSql.Append("category_id=@category_id,");
            strSql.Append("status=@status,");
            strSql.Append("unit=@unit,");
            strSql.Append("cost=@cost,");
            strSql.Append("price=@price,");
            strSql.Append("agio=@agio,");
            strSql.Append("remarks=@remarks,");
            strSql.Append("specifications=@specifications");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@product_name", SqlDbType.VarChar,250),
                    new SqlParameter("@category_id", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.VarChar,250),
                    new SqlParameter("@unit", SqlDbType.VarChar,250),
                    new SqlParameter("@cost", SqlDbType.Decimal,9),
                    new SqlParameter("@price", SqlDbType.Decimal,9),
                    new SqlParameter("@agio", SqlDbType.Decimal,9),
                    new SqlParameter("@remarks", SqlDbType.VarChar,-1),
                    new SqlParameter("@specifications", SqlDbType.VarChar,250),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.product_name;
            parameters[1].Value = model.category_id;
            parameters[2].Value = model.status;
            parameters[3].Value = model.unit;
            parameters[4].Value = model.cost;
            parameters[5].Value = model.price;
            parameters[6].Value = model.agio;
            parameters[7].Value = model.remarks;
            parameters[8].Value = model.specifications;
            parameters[9].Value = model.id;

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
            strSql.Append("delete from Product ");
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
            strSql.Append("delete from Product ");
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
            strSql.Append("select id,product_name,category_id,status,unit,cost,price,agio,remarks,specifications,create_id,create_time  ");
            strSql.Append(",(select product_category from Product_category where id = Product.category_id) as category_name");
            strSql.Append(" FROM Product ");
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
            strSql.Append(" id,product_name,category_id,status,unit,cost,price,agio,remarks,specifications,create_id,create_time   ");
            strSql.Append(",(select product_category from Product_category where id = Product.category_id) as category_name");
            strSql.Append(" FROM Product ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Product ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,product_name,category_id,status,unit,cost,price,agio,remarks,specifications,create_id,create_time  ");
            strSql_grid.Append(",(select product_category from Product_category where id = w1.category_id) as category_name");
            strSql_grid.Append(" FROM ( SELECT id,product_name,category_id,status,unit,cost,price,agio,remarks,specifications,create_id,create_time , ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product");
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

