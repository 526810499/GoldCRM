
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references

namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Product_allot
    /// </summary>
    public class Product_allot : BaseTransaction
    {
        public Product_allot()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Product_allot");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_allot model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_allot(");
            strSql.Append("id,remark,NowWarehouse,create_id,create_time,status,update_id,update_time,createdep_id,allotType)");
            strSql.Append(" values (");
            strSql.Append("@id,@remark,@NowWarehouse,@create_id,@create_time,@status,@update_id,@update_time,@createdep_id,@allotType)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@NowWarehouse", SqlDbType.Int,4),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@update_id", SqlDbType.VarChar,50),
                    new SqlParameter("@update_time", SqlDbType.DateTime),
                    new SqlParameter("@createdep_id",SqlDbType.VarChar,50),
                   new SqlParameter("@allotType", SqlDbType.Int,4),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.Remark;
            parameters[2].Value = model.NowWarehouse;
            parameters[3].Value = model.create_id;
            parameters[4].Value = model.create_time;
            parameters[5].Value = model.status;
            parameters[6].Value = model.update_id;
            parameters[7].Value = model.update_time;
            parameters[8].Value = model.createdep_id;
            parameters[9].Value = model.allotType;

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
        public bool Update(Model.Product_allot model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_allot set ");
            strSql.Append("NowWarehouse=@NowWarehouse,");
            strSql.Append("status=@status,");
            strSql.Append("update_id=@update_id,");
            strSql.Append("update_time=@update_time,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@NowWarehouse", SqlDbType.Int,4) {Value=model.NowWarehouse },
                    new SqlParameter("@status", SqlDbType.Int,4) { Value=model.status},
                    new SqlParameter("@update_id", SqlDbType.VarChar,50) { Value=model.update_id},
                    new SqlParameter("@update_time", SqlDbType.DateTime) { Value=DateTime.Now},
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=model.id},
                    new SqlParameter("@remark", SqlDbType.NVarChar, 50){ Value = model.Remark}
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
        /// 获取调度单下的商品总数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CountPorduct(string id)
        {

            string sql = "select count(1) from Product_allotDetail(nolock) where allotid=@allotid";
            SqlParameter[] par = { new SqlParameter("@allotid", SqlDbType.VarChar, 50) { Value = id }, };
            return DbHelperSQL.ExecuteScalar(sql, par).CInt(0, false);
        }
 
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authuser_id"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AuthApproved(string id, string authuser_id, int status, string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_allot set ");
            strSql.Append("authuser_id=@authuser_id,");
            strSql.Append("status=@status,");
            strSql.Append("authuser_time=@authuser_time,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id ");
            int rows = 1;
            //审核不通过需要释放
            if (status == 3)
            {
                string sql = @"UPDATE payuser SET status=1,warehouse_id=0 FROM dbo.Product AS payuser inner JOIN Product_allotDetail AS bu ON payuser.barcode=bu.barcode WHERE bu.allotid=@id and payuser.status=2 ";
                strSql.AppendLine(sql);
                rows += CountPorduct(id);
            }

            SqlParameter[] parameters = {
                    new SqlParameter("@status", SqlDbType.Int,4) { Value=status},
                    new SqlParameter("@authuser_id", SqlDbType.VarChar,50) { Value=authuser_id},
                    new SqlParameter("@authuser_time", SqlDbType.DateTime) { Value=DateTime.Now},
                   new SqlParameter("@remark", SqlDbType.NVarChar,50) { Value=remark},
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=id},
              };

            System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand();
            cm.CommandText = strSql.ToString();
            foreach (SqlParameter p in parameters)
            {
                cm.Parameters.Add(p);
            }
            return ExecTran(cm, rows);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_allotDetail ");
            strSql.Append(" where allotid=@id ");

            strSql.Append("delete from Product_allot ");

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
            strSql.Append("delete from Product_allot ");
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
        public Model.Product_allot GetModel(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,NowWarehouse,create_id,create_time,status,update_id,update_time,allotType from Product_allot ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

            Model.Product_allot model = new Model.Product_allot();
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
        public Model.Product_allot DataRowToModel(DataRow row)
        {
            Model.Product_allot model = new Model.Product_allot();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString();
                }
                if (row["NowWarehouse"] != null && row["NowWarehouse"].ToString() != "")
                {
                    model.NowWarehouse = int.Parse(row["NowWarehouse"].ToString());
                }
                if (row["create_id"] != null)
                {
                    model.create_id = row["create_id"].ToString();
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
                if (row["update_id"] != null)
                {
                    model.update_id = row["update_id"].ToString();
                }
                if (row["update_time"] != null && row["update_time"].ToString() != "")
                {
                    model.update_time = DateTime.Parse(row["update_time"].ToString());
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
            strSql.Append("select id,NowWarehouse,create_id,create_time,status,update_id,update_time,remark,allotType ");
            strSql.Append(" FROM Product_allot ");
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
            strSql.Append(" id,NowWarehouse,create_id,create_time,status,update_id,update_time,remark,allotType ");
            strSql.Append(" FROM Product_allot ");
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
            strSql.Append("select count(1) FROM Product_allot ");
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
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();
            strSql_total.Append(" SELECT COUNT(id) FROM Product_allot ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(",(select name from hr_employee where id = w1.create_id) as CreateName");
            strSql_grid.Append(",(select Product_warehouse from Product_warehouse where id = w1.NowWarehouse) as NowWarehouseName");
            strSql_grid.Append(" FROM (select  *, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_allot");
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

