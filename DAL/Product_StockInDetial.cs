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
    /// 数据访问类:Product_StockInDetial
    /// </summary>
    public partial class Product_StockInDetial : BaseTransaction
    {
        public Product_StockInDetial()
        { }
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_StockInDetial model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_StockInDetial(");
            strSql.Append("id,stockid,barcode,createdep_id,remark,warehouse_id,oldwarehouse_id)");
            strSql.Append(" values (");
            strSql.Append("@id,@stockid,@barcode,@createdep_id,@remark,@warehouse_id,@oldwarehouse_id)");
            strSql.AppendLine(@" update Product set OutStatus=1 where BarCode=@barcode ");

            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@stockid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@warehouse_id", SqlDbType.Int,4),
                    new SqlParameter("@oldwarehouse_id",SqlDbType.Int,4)
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.stockid;
            parameters[2].Value = model.barcode;
            parameters[3].Value = model.createdep_id;
            parameters[4].Value = model.remark;
            parameters[5].Value = model.warehouse_id;
            parameters[6].Value = model.oldwarehouse_id;

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
        /// 修改商品现存仓库
        /// </summary>
        /// <param name="stockid"></param>
        /// <param name="warehouse_id"></param>
        /// <returns></returns>
        public bool UpdateProductWareHouse(string stockid, int warehouse_id, string dep_id)
        {
            string sql = @"
                    UPDATE p SET p.indep_id=@dep_id,OutStatus=1,p.warehouse_id=@warehouse_id
                    FROM dbo.Product AS p 
                    INNER  JOIN Product_StockInDetial AS pst ON p.BarCode=pst.BarCode 
                    WHERE pst.stockid=@stockid ";

            SqlParameter[] parameters = {
                    new SqlParameter("@warehouse_id", SqlDbType.Int,4) { Value=warehouse_id},
                    new SqlParameter("@dep_id", SqlDbType.VarChar,50) { Value=dep_id},
                    new SqlParameter("@stockid", SqlDbType.VarChar,50) { Value=stockid},
            };
            System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand();
            cm.CommandText = sql;
            foreach (SqlParameter p in parameters)
            {
                cm.Parameters.Add(p);
            }
            return ExecTran(cm, -1);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Product_StockInDetial model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_StockInDetial set ");
            strSql.Append("remark=@remark,");
            strSql.Append("warehouse_id=@warehouse_id");
            strSql.Append(" where id=@id and stockid=@stockid and barcode=@barcode and createdep_id=@createdep_id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@warehouse_id", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@stockid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.remark;
            parameters[1].Value = model.warehouse_id;
            parameters[2].Value = model.id;
            parameters[3].Value = model.stockid;
            parameters[4].Value = model.barcode;
            parameters[5].Value = model.createdep_id;

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
        public bool Delete(string id, string stockid, string barcode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_StockInDetial ");
            strSql.Append(" where id=@id and stockid=@stockid and barcode=@barcode ");
            strSql.AppendLine(@" update Product set OutStatus=0 where BarCode=@barcode ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@stockid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50)
            };
            parameters[0].Value = id;
            parameters[1].Value = stockid;
            parameters[2].Value = barcode;

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
            strSql.Append("select id,stockid,barcode,createdep_id,remark,warehouse_id,oldwarehouse_id ");
            strSql.Append(" FROM Product_StockInDetial ");
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
            strSql.Append(" id,stockid,barcode,createdep_id,remark,warehouse_id,oldwarehouse_id ");
            strSql.Append(" FROM Product_StockInDetial ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Product_StockInDetial ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.createdep_id desc");
            }
            strSql.Append(")AS Row, T.*  from Product_StockInDetial T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();
            strSql_total.Append(" SELECT COUNT(id) FROM Product_StockInDetial(nolock) ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(@" FROM ( select   c.product_category AS category_name, p.BarCode, p.Weight, p.SalesCostsTotal, 
      p.SalesTotalPrice, p.CostsTotal, p.Totals, p.product_name, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from dbo.Product_StockInDetial(NOLOCK) AS ptd INNER JOIN dbo.Product(NOLOCK) AS p ON ptd.BarCode = p.BarCode INNER JOIN dbo.Product_category(NOLOCK) AS c ON p.category_id = c.id ");
            if (strWhere.Trim() != "")
            {
                strSql_grid.Append(" WHERE " + strWhere);
                strSql_total.Append(" WHERE " + strWhere);
            }
            strSql_grid.Append("  ) as w1  ");
            strSql_grid.Append("WHERE n BETWEEN " + (PageSize * (PageIndex - 1) + 1) + " AND " + PageSize * PageIndex);
            strSql_grid.Append(" ORDER BY " + filedOrder.Replace("ptd.", ""));
            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Product_StockInDetial";
			parameters[1].Value = "createdep_id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

