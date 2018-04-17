
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references

namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Product_allotDetail
    /// </summary>
    public partial class Product_allotDetail : BaseTransaction
    {
        public Product_allotDetail()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Product_allotDetail");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取调拨单下该商品对应订单状态
        /// </summary>
        /// <param name="allotid"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public int GetBarCodeStatus(string allotid, string barcode)
        {
            string sql = @"SELECT status FROM  Product_allotDetail(NOLOCK) INNER JOIN Product_allot(NOLOCK) ON dbo.Product_allot.id = dbo.Product_allotDetail.allotid
                        where allotid=@allotid and barcode=@barcode";
            SqlParameter[] parameters = {
                    new SqlParameter("@allotid", SqlDbType.VarChar,50) { Value=allotid}  ,
                  new SqlParameter("@barcode", SqlDbType.VarChar,50) { Value=barcode}  ,
            };
            return DbHelperSQL.ExecuteScalar(sql, parameters).CInt(0, false);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_allotDetail model)
        {
            bool rs = false;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("insert into Product_allotDetail(");
            strSql.Append("id,allotid,barcode,FromWarehouse,create_id,create_time,allotType)");
            strSql.Append(" values (");
            strSql.Append("@id,@allotid,@barcode,@FromWarehouse,@create_id,@create_time,@allotType)");
            
            //门店调拨的时候同步修改门店
            if (model.allotType == 0)
            {
                strSql.AppendLine(" update Product set Status=2,warehouse_id=@NowWarehouse where barcode=@barcode and Status<>4");//
            }
            else {
                strSql.AppendLine(" update Product set depopbefwid=warehouse_id,warehouse_id=@NowWarehouse,OutStatus=2 where barcode=@barcode and Status<>4");//
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@allotid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@FromWarehouse", SqlDbType.Int,4),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@NowWarehouse", SqlDbType.Int,4),
                    new SqlParameter("@allotType", SqlDbType.Int,4),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.allotid;
            parameters[2].Value = model.barcode;
            parameters[3].Value = model.FromWarehouse;
            parameters[4].Value = model.create_id;
            parameters[5].Value = model.create_time;
            parameters[6].Value = model.NowWarehouse;
            parameters[7].Value = model.allotType;

            System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand();

            cm.CommandText = strSql.ToString();
            foreach (SqlParameter p in parameters)
            {
                cm.Parameters.Add(p);
            }

            rs = ExecTran(cm, 2);
            return rs;

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Product_allotDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_allotDetail set ");
            strSql.Append("allotid=@allotid,");
            strSql.Append("barcode=@barcode,");
            strSql.Append("FromWarehouse=@FromWarehouse,");
            strSql.Append("create_id=@create_id,");
            strSql.Append("create_time=@create_time");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@allotid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@FromWarehouse", SqlDbType.Int,4),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.allotid;
            parameters[1].Value = model.barcode;
            parameters[2].Value = model.FromWarehouse;
            parameters[3].Value = model.create_id;
            parameters[4].Value = model.create_time;
            parameters[5].Value = model.id;

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
            strSql.Append("delete from Product_allotDetail ");
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
        /// 删除调一条商品
        /// </summary>
        /// <param name="allotid"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public bool Delete(int allotType,string allotid, string barcode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_allotDetail ");
            strSql.Append(" where allotid=@allotid and barcode=@barcode ");
            if (allotType == 0)
            {
                strSql.AppendLine(" update Product set Status=1 where barcode=@barcode");
            }
            else {
                strSql.AppendLine(" update Product set warehouse_id=depopbefwid where barcode=@barcode");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@allotid", SqlDbType.VarChar,50) { Value=allotid}  ,
                     new SqlParameter("@barcode", SqlDbType.VarChar,50) { Value=barcode}
            };

            System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand();
            cm.CommandText = strSql.ToString();
            foreach (SqlParameter p in parameters)
            {
                cm.Parameters.Add(p);
            }
            return ExecTran(cm, 2);
        }

        /// <summary>
        /// 删除调一条商品
        /// </summary>
        /// <param name="allotid"></param>
        /// <returns></returns>
        public bool DeleteByAllotid(string allotid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_allotDetail ");
            strSql.Append(" where allotid=allotid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@allotid", SqlDbType.VarChar,50) { Value=allotid}  ,
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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_allotDetail ");
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
        /// 得到一个对象实体
        /// </summary>
        public Model.Product_allotDetail GetModel(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,allotid,barcode,FromWarehouse,create_id,create_time,allotType from Product_allotDetail ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

            Model.Product_allotDetail model = new Model.Product_allotDetail();
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
        public Model.Product_allotDetail DataRowToModel(DataRow row)
        {
            Model.Product_allotDetail model = new Model.Product_allotDetail();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString();
                }
                if (row["allotid"] != null)
                {
                    model.allotid = row["allotid"].ToString();
                }
                if (row["barcode"] != null)
                {
                    model.barcode = row["barcode"].ToString();
                }
                if (row["FromWarehouse"] != null && row["FromWarehouse"].ToString() != "")
                {
                    model.FromWarehouse = int.Parse(row["FromWarehouse"].ToString());
                }
                if (row["create_id"] != null)
                {
                    model.create_id = row["create_id"].ToString();
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,allotid,barcode,FromWarehouse,create_id,create_time,allotType ");
            strSql.Append(" FROM Product_allotDetail ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Product_allotDetail ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(" FROM ( select Product_allotDetail.*, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_allotDetail");
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListProduct(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();
            strSql_total.Append(" SELECT COUNT(id) FROM view_AllotProduct ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(" FROM ( select view_AllotProduct.*, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from view_AllotProduct");
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
            strSql.Append(" id,allotid,barcode,FromWarehouse,create_id,create_time ");
            strSql.Append(" FROM Product_allotDetail ");
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
            strSql.Append("select count(1) FROM Product_allotDetail ");
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
                strSql.Append("order by T.id desc");
            }
            strSql.Append(")AS Row, T.*  from Product_allotDetail T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }


        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

