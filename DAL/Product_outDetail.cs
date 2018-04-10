
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
    public partial class Product_outDetail : BaseTransaction
    {
        public Product_outDetail()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Product_outDetail ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }




        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_outDetail model)
        {
            bool rs = false;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("insert into Product_outDetail(");
            strSql.Append("id,outid,barcode,create_id,create_time,FromWarehouse,outType)");
            strSql.Append(" values (");
            strSql.Append("@id,@outid,@barcode,@create_id,@create_time,@FromWarehouse,@outType) ");
            strSql.AppendLine(" update Product set Status=3 where barcode=@barcode and Status in(1,2); ");

            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@outid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@FromWarehouse", SqlDbType.Int,4),
                    new SqlParameter("@outType", SqlDbType.Int,4),

            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.outid;
            parameters[2].Value = model.barcode;
            parameters[3].Value = model.create_id;
            parameters[4].Value = model.create_time;
            parameters[5].Value = model.FromWarehouse.CInt(0, false);
            parameters[6].Value = model.outType;

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
        public bool Update(Model.Product_outDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_outDetail set ");
            strSql.Append("outid=@outid,");
            strSql.Append("barcode=@barcode,");
            strSql.Append("create_id=@create_id,");
            strSql.Append("create_time=@create_time");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@outid", SqlDbType.VarChar,50),
                    new SqlParameter("@barcode", SqlDbType.VarChar,50),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.outid;
            parameters[1].Value = model.barcode;
            parameters[2].Value = model.create_id;
            parameters[3].Value = model.create_time;
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
            strSql.Append("delete from Product_outDetail ");
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
        public bool Delete(string outid, string barcode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_outDetail ");
            strSql.Append(" where outid=@outid and barcode=@barcode ");
            strSql.AppendLine(" update Product set Status=1 where barcode=@barcode");
            SqlParameter[] parameters = {
                    new SqlParameter("@outid", SqlDbType.VarChar,50) { Value=outid}  ,
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
        /// <param name="outid"></param>
        /// <returns></returns>
        public bool DeleteByAllotid(string outid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_outDetail ");
            strSql.Append(" where outid=@outid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@outid", SqlDbType.VarChar,50) { Value=outid}  ,
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
            strSql.Append("delete from Product_outDetail ");
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
        public Model.Product_outDetail GetModel(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,outid,barcode,create_id,create_time,FromWarehouse,outType from Product_outDetail ");
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
        public Model.Product_outDetail DataRowToModel(DataRow row)
        {
            Model.Product_outDetail model = new Model.Product_outDetail();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString();
                }
                if (row["outid"] != null)
                {
                    model.outid = row["outid"].ToString();
                }
                if (row["barcode"] != null)
                {
                    model.barcode = row["barcode"].ToString();
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
            strSql.Append("select id,outid,barcode,create_id,create_time,FromWarehouse,outType ");
            strSql.Append(" FROM Product_outDetail ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Product_outDetail ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(" FROM ( select Product_outDetail.*, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_outDetail");
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
            strSql_total.Append(" SELECT COUNT(id) FROM view_outProduct ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(" FROM ( select view_outProduct.*, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from view_outProduct");
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
            strSql.Append(" id,outid,barcode,create_id,create_time,FromWarehouse,outType ");
            strSql.Append(" FROM Product_outDetail ");
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
            strSql.Append("select count(1) FROM Product_outDetail ");
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
            strSql.Append(")AS Row, T.*  from Product_outDetail T ");
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

