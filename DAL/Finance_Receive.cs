
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Finance_Receive
    /// </summary>
    public partial class Finance_Receive
    {
        public Finance_Receive()
        { }
        #region  BasicMethod


        /// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Finance_Receive model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Finance_Receive(");
            strSql.Append("id,Receive_num,Pay_type_id,Receive_amount,Receive_date,Payee_id,Receivable_id,Remarks,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@Receive_num,@Pay_type_id,@Receive_amount,@Receive_date,@Payee_id,@Receivable_id,@Remarks,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@Receive_num", SqlDbType.VarChar,50),
                    new SqlParameter("@Pay_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Receive_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@Receive_date", SqlDbType.DateTime),
                    new SqlParameter("@Payee_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Receivable_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Remarks", SqlDbType.NVarChar,-1),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.Receive_num;
            parameters[2].Value = model.Pay_type_id;
            parameters[3].Value = model.Receive_amount;
            parameters[4].Value = model.Receive_date;
            parameters[5].Value = model.Payee_id;
            parameters[6].Value = model.Receivable_id;
            parameters[7].Value = model.Remarks;
            parameters[8].Value = model.create_id;
            parameters[9].Value = model.create_time;

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
        public bool Update(XHD.Model.Finance_Receive model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Finance_Receive set ");
            strSql.Append("Pay_type_id=@Pay_type_id,");
            strSql.Append("Receive_amount=@Receive_amount,");
            strSql.Append("Receive_date=@Receive_date,");
            strSql.Append("Payee_id=@Payee_id,");
            strSql.Append("Receivable_id=@Receivable_id,");
            strSql.Append("Remarks=@Remarks");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Pay_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Receive_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@Receive_date", SqlDbType.DateTime),
                    new SqlParameter("@Payee_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Receivable_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Remarks", SqlDbType.VarChar,-1),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.Pay_type_id;
            parameters[1].Value = model.Receive_amount;
            parameters[2].Value = model.Receive_date;
            parameters[3].Value = model.Payee_id;
            parameters[4].Value = model.Receivable_id;
            parameters[5].Value = model.Remarks;
            parameters[6].Value = model.id;

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
            strSql.Append("delete from Finance_Receive ");
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
            strSql.Append("delete from Finance_Receive ");
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
            strSql.Append("select ");
            strSql.Append("    Finance_Receive.[id] ");
            strSql.Append("    , Finance_Receive.[Receive_num] ");
            strSql.Append("    , Finance_Receive.[Pay_type_id] ");
            strSql.Append("    , Finance_Receive.[Receive_amount] ");
            strSql.Append("    , Finance_Receive.[Receive_date] ");
            strSql.Append("    , Finance_Receive.[Payee_id] ");
            strSql.Append("    , Finance_Receive.[Receivable_id] ");
            strSql.Append("    , Finance_Receive.[Remarks] ");
            strSql.Append("    , Finance_Receive.[create_id] ");
            strSql.Append("    , Finance_Receive.[create_time] ");
            strSql.Append("    , Sys_Param.params_name as Pay_type");
            strSql.Append("    , Finance_Receivable.receivable_no ");
            strSql.Append("    , Finance_Receivable.receivable_amount ");
            strSql.Append("    , Finance_Receivable.order_id ");
            strSql.Append("    , hr_employee.name as payee ");
            strSql.Append("FROM [dbo].[Finance_Receive] ");
            strSql.Append("    INNER JOIN Sys_Param ON Sys_Param.id = Finance_Receive.Pay_type_id ");
            strSql.Append("    INNER JOIN Finance_Receivable ON Finance_Receivable.id = Finance_Receive.Receivable_id ");
            strSql.Append("    INNER JOIN hr_employee ON hr_employee.id = Finance_Receive.Payee_id ");
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
            strSql.Append("    Finance_Receive.[id] ");
            strSql.Append("    , Finance_Receive.[Receive_num] ");
            strSql.Append("    , Finance_Receive.[Pay_type_id] ");
            strSql.Append("    , Finance_Receive.[Receive_amount] ");
            strSql.Append("    , Finance_Receive.[Receive_date] ");
            strSql.Append("    , Finance_Receive.[Payee_id] ");
            strSql.Append("    , Finance_Receive.[Receivable_id] ");
            strSql.Append("    , Finance_Receive.[Remarks] ");
            strSql.Append("    , Finance_Receive.[create_id] ");
            strSql.Append("    , Finance_Receive.[create_time] ");
            strSql.Append("    , Sys_Param.params_name as Pay_type");
            strSql.Append("    , Finance_Receivable.receivable_no ");
            strSql.Append("    , Finance_Receivable.receivable_amount ");
            strSql.Append("    , Finance_Receivable.order_id ");
            strSql.Append("    , hr_employee.name as payee ");
            strSql.Append("FROM [dbo].[Finance_Receive] ");
            strSql.Append("    INNER JOIN Sys_Param ON Sys_Param.id = Finance_Receive.Pay_type_id ");
            strSql.Append("    INNER JOIN Finance_Receivable ON Finance_Receivable.id = Finance_Receive.Receivable_id ");
            strSql.Append("    INNER JOIN hr_employee ON hr_employee.id = Finance_Receive.Payee_id ");
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
            StringBuilder strSql_inner = new StringBuilder();
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();

            //联合数据
            strSql_inner.Append("( ");
            strSql_inner.Append("   SELECT  ");
            strSql_inner.Append($"      ROW_NUMBER() OVER(ORDER BY {filedOrder}) AS n ");
            strSql_inner.Append("    , Finance_Receive.[id] ");
            strSql_inner.Append("    , Finance_Receive.[Receive_num] ");
            strSql_inner.Append("    , Finance_Receive.[Pay_type_id] ");
            strSql_inner.Append("    , Finance_Receive.[Receive_amount] ");
            strSql_inner.Append("    , Finance_Receive.[Receive_date] ");
            strSql_inner.Append("    , Finance_Receive.[Payee_id] ");
            strSql_inner.Append("    , Finance_Receive.[Receivable_id] ");
            strSql_inner.Append("    , Finance_Receive.[Remarks] ");
            strSql_inner.Append("    , Finance_Receive.[create_id] ");
            strSql_inner.Append("    , Finance_Receive.[create_time] ");
            strSql_inner.Append("    , Sys_Param.params_name as Pay_type");
            strSql_inner.Append("    , Finance_Receivable.receivable_no ");
            strSql_inner.Append("    , Finance_Receivable.receivable_amount ");
            strSql_inner.Append("    , Finance_Receivable.order_id ");
            strSql_inner.Append("    ,hr_employee.name as payee ");
            strSql_inner.Append("FROM [dbo].[Finance_Receive] ");
            strSql_inner.Append("    INNER JOIN Sys_Param ON Sys_Param.id = Finance_Receive.Pay_type_id ");
            strSql_inner.Append("    INNER JOIN Finance_Receivable ON Finance_Receivable.id = Finance_Receive.Receivable_id ");
            strSql_inner.Append("    INNER JOIN hr_employee ON hr_employee.id = Finance_Receive.Payee_id ");
            if (strWhere.Trim() != "")
            {
                strSql_inner.Append(" WHERE " + strWhere);
            }
            strSql_inner.Append(") w1 ");

            //Total数据
            strSql_total.Append(" SELECT COUNT(ID) FROM ");
            strSql_total.Append(strSql_inner.ToString());

            //grid数据
            strSql_grid.Append("SELECT * FROM ");
            strSql_grid.Append(strSql_inner.ToString());
            strSql_grid.Append("WHERE n BETWEEN " + (PageSize * (PageIndex - 1) + 1) + " AND " + PageSize * PageIndex);

            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        /// <summary>
        ///     更新应收与订单
        /// </summary>
        public bool UpdateReceive(string Receivableid, string orderid)
        {
            var strSql = new StringBuilder();

            strSql.Append(" /*根据应收单ID更新*/");
            strSql.Append("UPDATE Finance_Receivable");
            strSql.Append("    SET received_amount = (SELECT SUM(ISNULL(Receive_amount, 0)) as EX1 FROM Finance_Receive WHERE Receivable_id = @Receivableid)");
            strSql.Append("WHERE id = @Receivableid");

            strSql.Append("/*更新未收*/");
            strSql.Append("UPDATE Finance_Receivable");
            strSql.Append("    SET arrears_amount = (ISNULL(receivable_amount, 0) - ISNULL(received_amount, 0))");
            strSql.Append("WHERE id = @Receivableid");

            strSql.Append("/*根据订单ID更新*/");
            strSql.Append("UPDATE Sale_order");
            strSql.Append("    SET receive_money = (SELECT SUM(ISNULL(received_amount, 0)) as EX1 FROM Finance_Receivable WHERE order_id = @orderid)");
            strSql.Append("WHERE id = @orderid");

            strSql.Append("/*更新未收*/");
            strSql.Append("UPDATE Sale_order");
            strSql.Append("    SET arrears_money = (ISNULL(Order_amount, 0) - ISNULL(receive_money, 0))");
            strSql.Append("WHERE id = @orderid ");

            SqlParameter[] parameters = {
                    new SqlParameter("@Receivableid", SqlDbType.VarChar,50),
                    new SqlParameter("@orderid", SqlDbType.VarChar,50)};
            parameters[0].Value = Receivableid;
            parameters[1].Value = orderid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

            if (rows > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion  ExtensionMethod
    }
}

