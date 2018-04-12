﻿
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
    public partial class Product: BaseTransaction
    {
        public Product()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product(");
            strSql.Append("id,product_name,category_id,status,Weight,unit,cost,price,agio,remarks,specifications,create_id,create_time,AttCosts,StockPrice,MainStoneWeight,AttStoneWeight,AttStoneNumber,StonePrice,GoldTotal,CostsTotal,Totals,Sbarcode,ImgLogo,BarCode,OutStatus,SalesTotalPrice,SalesCostsTotal,IsGold,SupplierID,createdep_id)");
            strSql.Append(" values (");
            strSql.Append("@id,@product_name,@category_id,@status,@Weight,@unit,@cost,@price,@agio,@remarks,@specifications,@create_id,@create_time,@AttCosts,@StockPrice,@MainStoneWeight,@AttStoneWeight,@AttStoneNumber,@StonePrice,@GoldTotal,@CostsTotal,@Totals,@Sbarcode,@ImgLogo,@BarCode,@OutStatus,@SalesTotalPrice,@SalesCostsTotal,@IsGold,@SupplierID,@createdep_id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@product_name", SqlDbType.VarChar,250),
                    new SqlParameter("@category_id", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@Weight", SqlDbType.Decimal,9),
                    new SqlParameter("@unit", SqlDbType.VarChar,250),
                    new SqlParameter("@cost", SqlDbType.Decimal,9),
                    new SqlParameter("@price", SqlDbType.Decimal,9),
                    new SqlParameter("@agio", SqlDbType.Decimal,9),
                    new SqlParameter("@remarks", SqlDbType.VarChar,2000),
                    new SqlParameter("@specifications", SqlDbType.VarChar,250),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@AttCosts", SqlDbType.Decimal,9),
                    new SqlParameter("@StockPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@MainStoneWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@AttStoneWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@AttStoneNumber", SqlDbType.Decimal,9),
                    new SqlParameter("@StonePrice", SqlDbType.Decimal,9),
                    new SqlParameter("@GoldTotal", SqlDbType.Decimal,9),
                    new SqlParameter("@CostsTotal", SqlDbType.Decimal,9),
                    new SqlParameter("@Totals", SqlDbType.Decimal,9),
                    new SqlParameter("@Sbarcode", SqlDbType.VarChar,50),
                    new SqlParameter("@ImgLogo", SqlDbType.VarChar,250),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
                    new SqlParameter("@OutStatus", SqlDbType.Int,4),
                    new SqlParameter("@SalesTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@SalesCostsTotal", SqlDbType.Decimal,9),
                    new SqlParameter("@IsGold", SqlDbType.Int,4),
                    new SqlParameter("@SupplierID",SqlDbType.NVarChar,50),
                    new SqlParameter("@createdep_id",SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.product_name;
            parameters[2].Value = model.category_id;
            parameters[3].Value = model.status;
            parameters[4].Value = model.Weight;
            parameters[5].Value = model.unit;
            parameters[6].Value = model.cost;
            parameters[7].Value = model.price;
            parameters[8].Value = model.agio;
            parameters[9].Value = model.remarks;
            parameters[10].Value = model.specifications;
            parameters[11].Value = model.create_id;
            parameters[12].Value = model.create_time;
            parameters[13].Value = model.AttCosts;
            parameters[14].Value = model.StockPrice;
            parameters[15].Value = model.MainStoneWeight;
            parameters[16].Value = model.AttStoneWeight;
            parameters[17].Value = model.AttStoneNumber;
            parameters[18].Value = model.StonePrice;
            parameters[19].Value = model.GoldTotal;
            parameters[20].Value = model.CostsTotal;
            parameters[21].Value = model.Totals;
            parameters[22].Value = model.Sbarcode;
            parameters[23].Value = model.ImgLogo;
            parameters[24].Value = model.BarCode;
            parameters[25].Value = model.OutStatus;
            parameters[26].Value = model.SalesTotalPrice;
            parameters[27].Value = model.SalesCostsTotal;
            parameters[28].Value = model.IsGold;
            parameters[29].Value = model.SupplierID;
            parameters[30].Value = model.createdep_id;
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
        public bool Update(Model.Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product set ");
            strSql.Append("product_name=@product_name,");
            strSql.Append("category_id=@category_id,");
            strSql.Append("status=@status,");
            strSql.Append("Weight=@Weight,");
            strSql.Append("unit=@unit,");
            strSql.Append("cost=@cost,");
            strSql.Append("price=@price,");
            strSql.Append("agio=@agio,");
            strSql.Append("remarks=@remarks,");
            strSql.Append("specifications=@specifications,");
            strSql.Append("create_id=@create_id,");
            strSql.Append("create_time=@create_time,");
            strSql.Append("AttCosts=@AttCosts,");
            strSql.Append("StockPrice=@StockPrice,");
            strSql.Append("MainStoneWeight=@MainStoneWeight,");
            strSql.Append("AttStoneWeight=@AttStoneWeight,");
            strSql.Append("AttStoneNumber=@AttStoneNumber,");
            strSql.Append("StonePrice=@StonePrice,");
            strSql.Append("GoldTotal=@GoldTotal,");
            strSql.Append("CostsTotal=@CostsTotal,");
            strSql.Append("Totals=@Totals,");
            strSql.Append("Sbarcode=@Sbarcode,");
            strSql.Append("ImgLogo=@ImgLogo,");
            strSql.Append("OutStatus=@OutStatus,");
            strSql.Append("SalesTotalPrice=@SalesTotalPrice,");
            strSql.Append("SalesCostsTotal=@SalesCostsTotal,");
            strSql.Append("SupplierID=@SupplierID,");
            strSql.Append("IsGold=@IsGold");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@product_name", SqlDbType.VarChar,250),
                    new SqlParameter("@category_id", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@Weight", SqlDbType.Decimal,9),
                    new SqlParameter("@unit", SqlDbType.VarChar,250),
                    new SqlParameter("@cost", SqlDbType.Decimal,9),
                    new SqlParameter("@price", SqlDbType.Decimal,9),
                    new SqlParameter("@agio", SqlDbType.Decimal,9),
                    new SqlParameter("@remarks", SqlDbType.VarChar,2000),
                    new SqlParameter("@specifications", SqlDbType.VarChar,250),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@AttCosts", SqlDbType.Decimal,9),
                    new SqlParameter("@StockPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@MainStoneWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@AttStoneWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@AttStoneNumber", SqlDbType.Decimal,9),
                    new SqlParameter("@StonePrice", SqlDbType.Decimal,9),
                    new SqlParameter("@GoldTotal", SqlDbType.Decimal,9),
                    new SqlParameter("@CostsTotal", SqlDbType.Decimal,9),
                    new SqlParameter("@Totals", SqlDbType.Decimal,9),
                    new SqlParameter("@Sbarcode", SqlDbType.VarChar,50),
                    new SqlParameter("@ImgLogo", SqlDbType.VarChar,250),
                    new SqlParameter("@OutStatus", SqlDbType.Int,4),
                    new SqlParameter("@SalesTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@SalesCostsTotal", SqlDbType.Decimal,9),
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
                   new SqlParameter("@IsGold", SqlDbType.Int,4),
                 new SqlParameter("@SupplierID", SqlDbType.VarChar,50),
            };
            parameters[0].Value = model.product_name;
            parameters[1].Value = model.category_id;
            parameters[2].Value = model.status;
            parameters[3].Value = model.Weight;
            parameters[4].Value = model.unit;
            parameters[5].Value = model.cost;
            parameters[6].Value = model.price;
            parameters[7].Value = model.agio;
            parameters[8].Value = model.remarks;
            parameters[9].Value = model.specifications;
            parameters[10].Value = model.create_id;
            parameters[11].Value = model.create_time;
            parameters[12].Value = model.AttCosts;
            parameters[13].Value = model.StockPrice;
            parameters[14].Value = model.MainStoneWeight;
            parameters[15].Value = model.AttStoneWeight;
            parameters[16].Value = model.AttStoneNumber;
            parameters[17].Value = model.StonePrice;
            parameters[18].Value = model.GoldTotal;
            parameters[19].Value = model.CostsTotal;
            parameters[20].Value = model.Totals;
            parameters[21].Value = model.Sbarcode;
            parameters[22].Value = model.ImgLogo;
            parameters[23].Value = model.OutStatus;
            parameters[24].Value = model.SalesTotalPrice;
            parameters[25].Value = model.SalesCostsTotal;
            parameters[26].Value = model.id;
            parameters[27].Value = model.BarCode;
            parameters[28].Value = model.IsGold;
            parameters[29].Value = model.SupplierID;
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
        /// 通过条形码获取产品状态
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public int GetPorductStatusByBarCode(string BarCode)
        {
            string sql = "select Status from  Product where BarCode=@BarCode";
            SqlParameter[] parameters = {
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50) { Value=BarCode}        };

            return DbHelperSQL.ExecuteScalar(sql, parameters).CInt(-1, false);
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
            strSql.Append("select id, product_name, category_id, status, Weight,  create_id, create_time, AttCosts, StockPrice, MainStoneWeight, AttStoneWeight, AttStoneNumber, StonePrice, GoldTotal, CostsTotal, Totals, Sbarcode, ImgLogo, BarCode, OutStatus, SalesTotalPrice, SalesCostsTotal, SupplierID,IsGold,remarks  ");
            strSql.Append(@",(select product_category from Product_category where id = Product.category_id) as category_name,
                            (select product_supplier from Product_supplier where id = Product.SupplierID) as supplier_name ");
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
            strSql.Append(" id, product_name, category_id, status, Weight,  create_id, create_time, AttCosts, StockPrice, MainStoneWeight, AttStoneWeight, AttStoneNumber, StonePrice, GoldTotal, CostsTotal, Totals, Sbarcode, ImgLogo, BarCode, OutStatus, SalesTotalPrice, SalesCostsTotal, SupplierID ,IsGold,remarks  ");
            strSql.Append(",(select product_category from Product_category(nolock) where id = w1.category_id) as category_name");
            strSql.Append(",(select product_supplier from Product_supplier(nolock) where id = w1.SupplierID) as supplier_name");
            strSql.Append(",(select product_warehouse from Product_warehouse(nolock) where id = w1.warehouse_id) as warehouse_name");
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
            strSql_grid.Append("      n,id, product_name, category_id, status, Weight, create_id, create_time, AttCosts, StockPrice, MainStoneWeight, AttStoneWeight, AttStoneNumber, StonePrice, GoldTotal, CostsTotal, Totals, Sbarcode, ImgLogo, BarCode, OutStatus, SalesTotalPrice, SalesCostsTotal, SupplierID,IsGold,remarks ");
            strSql_grid.Append(",(select product_category from Product_category(nolock) where id = w1.category_id) as category_name");
            strSql_grid.Append(",(select product_supplier from Product_supplier(nolock) where id = w1.SupplierID) as supplier_name");
            strSql_grid.Append(",(select product_warehouse from Product_warehouse(nolock) where id = w1.warehouse_id) as warehouse_name");
            strSql_grid.Append(" FROM ( SELECT id, product_name, category_id, status, Weight, create_id, create_time, AttCosts, StockPrice, MainStoneWeight, AttStoneWeight, AttStoneNumber, StonePrice, GoldTotal, CostsTotal, Totals, Sbarcode, ImgLogo, BarCode, OutStatus, SalesTotalPrice, SalesCostsTotal, SupplierID,IsGold,remarks,warehouse_id, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product");
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

