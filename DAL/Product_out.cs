
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
    public class Product_out : BaseTransaction
    {
        public Product_out()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Product_out");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_out model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_out(");
            strSql.Append("id,remark,allot_id,create_id,create_time,status,update_id,update_time,createdep_id,outdep_id,NowWarehouse,outType)");
            strSql.Append(" values (");
            strSql.Append("@id,@remark,@allot_id,@create_id,@create_time,@status,@update_id,@update_time,@createdep_id,@outdep_id,@NowWarehouse,@outType)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@allot_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@update_id", SqlDbType.VarChar,50),
                    new SqlParameter("@update_time", SqlDbType.DateTime),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@outdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@NowWarehouse", SqlDbType.Int,4),
                   new SqlParameter("@outType", SqlDbType.Int,4),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.Remark;
            parameters[2].Value = model.allot_id;
            parameters[3].Value = model.create_id;
            parameters[4].Value = model.create_time;
            parameters[5].Value = model.status;
            parameters[6].Value = model.update_id;
            parameters[7].Value = model.update_time;
            parameters[8].Value = model.createdep_id;
            parameters[9].Value = model.outdep_id;
            parameters[10].Value = model.NowWarehouse;
            parameters[11].Value = model.outType;


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
        public bool Update(Model.Product_out model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_out set ");
            strSql.Append("allot_id=@allot_id,");
            strSql.Append("status=@status,");
            strSql.Append("update_id=@update_id,");
            strSql.Append("update_time=@update_time,");
            strSql.Append("remark=@remark,NowWarehouse=@NowWarehouse");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@allot_id", SqlDbType.VarChar,50) {Value=model.allot_id },
                    new SqlParameter("@status", SqlDbType.Int,4) { Value=model.status},
                    new SqlParameter("@update_id", SqlDbType.VarChar,50) { Value=model.update_id},
                    new SqlParameter("@update_time", SqlDbType.DateTime) { Value=DateTime.Now},
                    new SqlParameter("@id", SqlDbType.VarChar,50) { Value=model.id},
                    new SqlParameter("@remark", SqlDbType.NVarChar, 50){ Value = model.Remark},
                    new SqlParameter("@NowWarehouse",SqlDbType.Int,4) { Value=model.NowWarehouse}
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

            string sql = "select count(1) from Product_outDetail(nolock) where outid=@outid";
            SqlParameter[] par = { new SqlParameter("@outid", SqlDbType.VarChar, 50) { Value = id }, };
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
        public bool AuthApproved(int outType, string id, string authuser_id, int status, string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_out set ");
            strSql.Append("authuser_id=@authuser_id,");
            strSql.Append("status=@status,");
            strSql.Append("authuser_time=@authuser_time,");
            strSql.Append("remark=@remark");
            strSql.Append(" where id=@id ");
            int rows = 1;
            rows += CountPorduct(id);
            int pstatus = 3;
            //审核 通过需要同步信息
            if (status == 2)
            {
                if (outType == 1)
                {
                    pstatus = 103;
                }
                string sql = @"UPDATE payuser SET payuser.authIn=0,payuser.status=@pstatus,warehouse_id=bu.ToWarehouse FROM dbo.Product(nolock) AS payuser inner JOIN Product_outDetail(nolock) AS bu ON payuser.barcode=bu.barcode WHERE bu.outid=@id and payuser.status<>4 ";
                strSql.AppendLine(sql);

            }
            else {
                string sql = @"UPDATE payuser SET payuser.authIn=0  FROM dbo.Product(nolock) AS payuser inner JOIN Product_outDetail(nolock) AS bu ON payuser.barcode=bu.barcode WHERE bu.outid=@id and payuser.status<>4 ";
                strSql.AppendLine(sql);
            }

            SqlParameter[] parameters = {
                    new SqlParameter("@status", SqlDbType.Int,4) { Value=status},
                     new SqlParameter("@pstatus", SqlDbType.Int,4) { Value=pstatus},
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
        public bool Delete(string id, int outType)
        {

            StringBuilder strSql = new StringBuilder();

            if (outType == 1)
            {
                strSql.AppendLine("UPDATE payuser SET warehouse_id=depopbefwid  FROM dbo.Product(nolock) AS payuser inner JOIN Product_outDetail(nolock) AS bu ON payuser.barcode=bu.barcode WHERE bu.outid=@id");
            }
            strSql.Append("delete from Product_outDetail where outid=@id ");
            strSql.Append("delete from Product_out ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

            System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand();
            cm.CommandText = strSql.ToString();
            foreach (SqlParameter p in parameters)
            {
                cm.Parameters.Add(p);
            }
            return ExecTran(cm, -1);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_out ");
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
        public Model.Product_out GetModel(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,allot_id,create_id,create_time,status,update_id,update_time,NowWarehouse,outType from Product_out ");
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
        public Model.Product_out DataRowToModel(DataRow row)
        {
            Model.Product_out model = new Model.Product_out();
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
            strSql.Append("select id,allot_id,create_id,create_time,status,update_id,update_time,remark,NowWarehouse,outType ");
            strSql.Append(" FROM Product_out ");
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
            strSql.Append(" id,allot_id,create_id,create_time,status,update_id,update_time,remark ");
            strSql.Append(" FROM Product_out ");
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
            strSql.Append("select count(1) FROM Product_out ");
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
            strSql_total.Append(" SELECT COUNT(id) FROM Product_out ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      * ");
            strSql_grid.Append(",(select name from hr_employee where id = w1.create_id) as CreateName");
            strSql_grid.Append(",(select Product_warehouse from Product_warehouse where id = w1.NowWarehouse) as NowWarehouseName");
            strSql_grid.Append(" FROM (select  *, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_out");
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

