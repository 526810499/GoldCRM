
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
	/// 数据访问类:Sale_order_details
	/// </summary>
	public partial class Sale_order_details : BaseTransaction
    {
        public Sale_order_details()
        { }

        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sale_order_details model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sale_order_details(");
            strSql.Append("order_id,product_id,agio,quantity,amount,BarCode)");
            strSql.Append(" values (");
            strSql.Append("@order_id,@product_id,@agio,@quantity,@amount,@BarCode); ");
            strSql.AppendLine(" update Product set status=4,OutStatus=4 where id=@product_id; ");
            SqlParameter[] parameters = {
                    new SqlParameter("@order_id", SqlDbType.VarChar,250),
                    new SqlParameter("@product_id", SqlDbType.VarChar,250),
                    new SqlParameter("@agio", SqlDbType.Decimal,9),
                    new SqlParameter("@quantity", SqlDbType.Int,4),
                    new SqlParameter("@amount", SqlDbType.Decimal,9),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
            };
            parameters[0].Value = model.order_id;
            parameters[1].Value = model.product_id;
            parameters[2].Value = model.agio;
            parameters[3].Value = model.quantity;
            parameters[4].Value = model.amount;
            parameters[5].Value = model.BarCode;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strSql.ToString();
            foreach (SqlParameter par in parameters)
            {
                cmd.Parameters.Add(par);
            }

            return ExecTran(cmd, 2);
        }

        /// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.Sale_order_details model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sale_order_details set ");
            strSql.Append("product_id=@product_id,");
            strSql.Append("agio=@agio,");
            strSql.Append("quantity=@quantity,");
            strSql.Append("amount=@amount");
            strSql.Append(" where order_id=@order_id and product_id=@product_id  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@order_id", SqlDbType.VarChar,250),
                    new SqlParameter("@product_id", SqlDbType.VarChar,250),
                    new SqlParameter("@agio", SqlDbType.Decimal,9),
                    new SqlParameter("@quantity", SqlDbType.Int,4),
                    new SqlParameter("@amount", SqlDbType.Decimal,9)};
            parameters[0].Value = model.order_id;
            parameters[1].Value = model.product_id;
            parameters[2].Value = model.agio;
            parameters[3].Value = model.quantity;
            parameters[4].Value = model.amount;

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
        public bool Delete(string whereStr)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sale_order_details ");
            strSql.Append(" where " + whereStr);

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
        /// 删除一条数据
        /// </summary>
        public bool Delete(string order_id, string product_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sale_order_details where order_id=@order_id and  product_id=@product_id; ");
            strSql.AppendLine(" update Product set status=3,OutStatus=3 where id=@product_id; ");


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strSql.ToString();

            cmd.Parameters.Add(new SqlParameter("@order_id", SqlDbType.VarChar, 250) { Value = order_id });
            cmd.Parameters.Add(new SqlParameter("@product_id", SqlDbType.VarChar, 50) { Value = product_id });

            return ExecTran(cmd, 2);
        }

        public int GetDetailCount(string OrderID)
        {
            string sql = "select count(1) from Sale_order_details(nolock) where order_id=@order_id ";
            SqlParameter[] par = {
                new SqlParameter("@order_id",OrderID)
            };

            return DbHelperSQL.ExecuteScalar(sql, par).CInt(0, false);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  ");
            strSql.Append("        Sale_order_details.[order_id] ");
            strSql.Append("      , Sale_order_details.[product_id] ");
            strSql.Append("      , Sale_order_details.[agio] ");
            strSql.Append("      , Sale_order_details.[quantity] ");
            strSql.Append("      , Sale_order_details.[amount] ");
            strSql.Append("      , Product.product_name ");
            strSql.Append("      , Product.specifications ");
            strSql.Append("      , Product.indep_id ");
            strSql.Append("      , Product.BarCode ");
            strSql.Append("      , isnull(Product.SalesCostsTotal,0) as SalesCostsTotal ");
            strSql.Append("      , isnull(Product.SalesTotalPrice,0 ) as SalesTotalPrice");
            strSql.Append("      , Product.Weight,Product.CostsTotal ");
            strSql.Append("      ,Product_category.product_category as category_name ");
            strSql.Append("  FROM[dbo].[Sale_order_details] ");
            strSql.Append("  INNER JOIN Product ON Product.id = Sale_order_details.product_id ");
            strSql.Append("  INNER JOIN Product_category(nolock) ON Product.category_id = Product_category.id ");
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
            strSql.Append("        Sale_order_details.[order_id] ");
            strSql.Append("      , Sale_order_details.[product_id] ");
            strSql.Append("      , Sale_order_details.[agio] ");
            strSql.Append("      , Sale_order_details.[quantity] ");
            strSql.Append("      , Sale_order_details.[amount] ");
            strSql.Append("      , Product.product_name ");
            strSql.Append("      , Product.specifications ");
            strSql.Append("      , Product.indep_id ");
            strSql.Append("FROM[dbo].[Sale_order_details] ");
            strSql.Append("  INNER JOIN Product ON Product.id = Sale_order_details.product_id ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

