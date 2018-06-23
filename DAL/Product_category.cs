
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
using XHD.Model;

namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Product_category
    /// </summary>
    public partial class Product_category
    {
        public Product_category()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Product_category model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_category(");
            strSql.Append("id,product_category,parentid,product_icon,create_id,create_time,CodingBegins,Counts)");
            strSql.Append(" values (");
            strSql.Append("@id,@product_category,@parentid,@product_icon,@create_id,@create_time,@CodingBegins,@Counts)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@product_category", SqlDbType.VarChar,250),
                    new SqlParameter("@parentid", SqlDbType.VarChar,50),
                    new SqlParameter("@product_icon", SqlDbType.VarChar,250),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@CodingBegins",SqlDbType.VarChar,50),
                    new SqlParameter("@Counts",SqlDbType.Int,4)
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.product_category;
            parameters[2].Value = model.parentid;
            parameters[3].Value = model.product_icon;
            parameters[4].Value = model.create_id;
            parameters[5].Value = model.create_time;
            parameters[6].Value = model.CodingBegins;
            parameters[7].Value = 10000;


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
        public bool Update(XHD.Model.Product_category model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_category set ");
            strSql.Append("product_category=@product_category,");
            strSql.Append("parentid=@parentid,");
            strSql.Append("product_icon=@product_icon,CodingBegins=@CodingBegins");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@product_category", SqlDbType.VarChar,250),
                    new SqlParameter("@parentid", SqlDbType.VarChar,50),
                    new SqlParameter("@product_icon", SqlDbType.VarChar,250),
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                new SqlParameter("@CodingBegins",SqlDbType.VarChar,50)
};
            parameters[0].Value = model.product_category;
            parameters[1].Value = model.parentid;
            parameters[2].Value = model.product_icon;
            parameters[3].Value = model.id;
            parameters[4].Value = model.CodingBegins;

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
        /// 添加编码
        /// </summary>
        /// <param name="CodingBegins"></param>
        public void AddNO(string CodingBegins)
        {
            string sql = @"
                IF NOT EXISTS(SELECT 1 FROM Product_CategoryNoTB WHERE CodingBegin=@CodingBegin)BEGIN
                   INSERT INTO Product_CategoryNoTB(CodingBegin,Counts) VALUES(@CodingBegin,1)
                END";
            SqlParameter[] parameters = {
                new SqlParameter("@CodingBegin",SqlDbType.VarChar,50) { Value=CodingBegins}
            };

            DbHelperSQL.ExecuteSql(sql, parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_category ");
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
            strSql.Append("delete from Product_category ");
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
            strSql.Append("select id,product_category,parentid,product_icon,create_id,create_time,CodingBegins ");
            strSql.Append(" FROM Product_category ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取当前这个分类ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int GetCategoryCounts(string CodingBegin)
        {
            string sql = @" 
                        DECLARE @MaxCount INT
                        update Product_CategoryNoTB set Counts=isnull(Counts,0)+1,@MaxCount=(isnull(Counts,0)+1) where CodingBegin=@CodingBegin 
                        select @MaxCount
                      ";
            SqlParameter[] parameters = {
                    new SqlParameter("@CodingBegin", SqlDbType.VarChar,50)           };
            parameters[0].Value = CodingBegin;

            return DbHelperSQL.ExecuteScalar(sql, parameters).CInt(0, false);
        }

        /// <summary>
        /// 获取序列号条形码
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public Sys_SerialNumber GetSerialNumber(SerialNumberType Type)
        {
            string sql = @"
                        DECLARE @Counts     INT
                        DECLARE @BegLetter    nvarchar(50)
                        DECLARE @Times        smalldatetime
                        DECLARE  @ID        bigint
                        update Sys_SerialNumber set Counts=isnull(Counts,0)+1,@ID=ID,@Counts=(isnull(Counts,0)+1),@BegLetter=BegLetter,@Times=Times where Type=@Type 
                        select @Counts as Counts,@BegLetter as BegLetter,@Times as Times,@ID as ID
                         ";
            SqlParameter[] parameters = {
                    new SqlParameter("@Type", SqlDbType.Int)
            };
            parameters[0].Value = (int)Type;

            DataTable table = DbHelperSQL.Query(sql, parameters).Tables[0];

            return ModelConvertHelper<Sys_SerialNumber>.ToModel(table);
        }

        /// <summary>
        /// 重置序列号
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="BegLetter"></param>
        /// <param name="Counts"></param>
        /// <returns></returns>
        public bool ResetSerialNumber(long ID, string BegLetter, int Counts)
        {
            string sql = " update Sys_SerialNumber set Counts=@Counts,BegLetter=@BegLetter,Times=@Times where ID=@ID  ";
            SqlParameter[] parameters = {
                    new SqlParameter("@Counts", SqlDbType.Int) { Value=Counts},
                    new SqlParameter("@BegLetter",SqlDbType.NVarChar,50) { Value=BegLetter},
                    new SqlParameter("@Times",SqlDbType.SmallDateTime) { Value=DateTime.Now.Date},
                    new SqlParameter("@ID",SqlDbType.BigInt) { Value=ID}
            };

            return DbHelperSQL.ExecuteSql(sql, parameters) > 0;
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
            strSql.Append(" id,product_category,parentid,product_icon,create_id,create_time,CodingBegins,0 as isextend ");
            strSql.Append(" FROM Product_category ");
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

