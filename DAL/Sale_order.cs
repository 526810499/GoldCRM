
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Sale_order
    /// </summary>
    public partial class Sale_order
    {
        public Sale_order()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Sale_order");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)           };
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }



        /// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Sale_order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sale_order(");
            strSql.Append("id,Serialnumber,Customer_id,Order_date,pay_type_id,Order_status_id,Order_amount,discount_amount,total_amount,emp_id,receive_money,arrears_money,invoice_money,arrears_invoice,Order_details,create_id,create_time,cashier_id,vipcard,createdep_id,PayTheBill)");
            strSql.Append(" values (");
            strSql.Append("@id,@Serialnumber,@Customer_id,@Order_date,@pay_type_id,@Order_status_id,@Order_amount,@discount_amount,@total_amount,@emp_id,@receive_money,@arrears_money,@invoice_money,@arrears_invoice,@Order_details,@create_id,@create_time,@cashier_id,@vipcard,@createdep_id,@PayTheBill)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
                    new SqlParameter("@Customer_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Order_date", SqlDbType.DateTime),
                    new SqlParameter("@pay_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Order_status_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Order_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@discount_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@total_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@receive_money", SqlDbType.Decimal,9),
                    new SqlParameter("@arrears_money", SqlDbType.Decimal,9),
                    new SqlParameter("@invoice_money", SqlDbType.Decimal,9),
                    new SqlParameter("@arrears_invoice", SqlDbType.Decimal,9),
                    new SqlParameter("@Order_details", SqlDbType.VarChar,-1),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                   new SqlParameter("@cashier_id", SqlDbType.VarChar,50),
                   new SqlParameter("@vipcard", SqlDbType.VarChar,50),
                  new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                  new SqlParameter("@PayTheBill",SqlDbType.NVarChar,50),
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.Serialnumber;
            parameters[2].Value = model.Customer_id;
            parameters[3].Value = model.Order_date;
            parameters[4].Value = model.pay_type_id;
            parameters[5].Value = model.Order_status_id;
            parameters[6].Value = model.Order_amount;
            parameters[7].Value = model.discount_amount;
            parameters[8].Value = model.total_amount;
            parameters[9].Value = model.emp_id;
            parameters[10].Value = model.receive_money;
            parameters[11].Value = model.arrears_money;
            parameters[12].Value = model.invoice_money;
            parameters[13].Value = model.arrears_invoice;
            parameters[14].Value = model.Order_details;
            parameters[15].Value = model.create_id;
            parameters[16].Value = model.create_time;
            parameters[17].Value = model.cashier_id;
            parameters[18].Value = model.vipcard;
            parameters[19].Value = model.createdep_id;
            parameters[20].Value = model.PayTheBill;

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
        public bool Update(XHD.Model.Sale_order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sale_order set ");
            strSql.Append("Order_date=@Order_date,");
            strSql.Append("pay_type_id=@pay_type_id,");
            strSql.Append("Order_status_id=@Order_status_id,");
            strSql.Append("Order_amount=@Order_amount,");
            strSql.Append("discount_amount=@discount_amount,");
            strSql.Append("total_amount=@total_amount,");
            strSql.Append("emp_id=@emp_id,");
            strSql.Append("Order_details=@Order_details,");
            strSql.Append("cashier_id=@cashier_id,");
            strSql.Append("vipcard=@vipcard,");
            strSql.Append("receive_money=@receive_money,");
            strSql.Append("arrears_money=@arrears_money,");
            strSql.Append(" createdep_id=@createdep_id,PayTheBill=@PayTheBill  ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Order_date", SqlDbType.DateTime),
                    new SqlParameter("@pay_type_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Order_status_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Order_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@discount_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@total_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@Order_details", SqlDbType.VarChar,-1),
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@cashier_id", SqlDbType.VarChar,50),
                    new SqlParameter("@vipcard", SqlDbType.VarChar,50),
                    new SqlParameter("@receive_money", SqlDbType.Decimal,9),
                    new SqlParameter("@arrears_money", SqlDbType.Decimal,9),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@PayTheBill", SqlDbType.VarChar,50),
               };
            parameters[0].Value = model.Order_date;
            parameters[1].Value = model.pay_type_id;
            parameters[2].Value = model.Order_status_id;
            parameters[3].Value = model.Order_amount;
            parameters[4].Value = model.discount_amount;
            parameters[5].Value = model.total_amount;
            parameters[6].Value = model.emp_id;
            parameters[7].Value = model.Order_details;
            parameters[8].Value = model.id;
            parameters[9].Value = model.cashier_id;
            parameters[10].Value = model.vipcard;
            parameters[11].Value = model.receive_money;
            parameters[12].Value = model.arrears_money;
            parameters[13].Value = model.createdep_id;
            parameters[14].Value = model.PayTheBill;
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
            strSql.Append("delete from Sale_order ");
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
            strSql.Append("delete from Sale_order ");
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
            strSql.Append(" SELECT ");
            strSql.Append("      Sale_order.[id] ");
            strSql.Append("      , Sale_order.[Serialnumber] ");
            strSql.Append("      , Sale_order.[Customer_id] ");
            strSql.Append("      , Sale_order.[Order_date] ");
            strSql.Append("      , Sale_order.[pay_type_id] ");
            strSql.Append("      , (SELECT params_name FROM Sys_Param where id = Sale_order.pay_type_id) as pay_type ");
            strSql.Append("      , Sale_order.[Order_status_id] ");
            strSql.Append("      , (SELECT params_name FROM Sys_Param where id = Sale_order.Order_status_id) as Order_status ");
            strSql.Append("      , Sale_order.[Order_amount] ");
            strSql.Append("      , Sale_order.[discount_amount] ");
            strSql.Append("      , Sale_order.[total_amount] ");
            strSql.Append("      , Sale_order.[emp_id] ");
            strSql.Append("      , Sale_order.[receive_money] ");
            strSql.Append("      , Sale_order.[arrears_money] ");
            strSql.Append("      , Sale_order.[invoice_money] ");
            strSql.Append("      , Sale_order.[arrears_invoice] ");
            strSql.Append("      , Sale_order.[Order_details] ");
            strSql.Append("      , Sale_order.[createdep_id] ");
            strSql.Append("      , Sale_order.[vipcard] ");
            strSql.Append("      , Sale_order.[create_id] ");
            strSql.Append("      , Sale_order.[create_time],cashier_id,vipcard,createdep_id,PayTheBill ");
            strSql.Append("      , CRM_Customer.cus_name ");
            strSql.Append("      , hr_employee.name as emp_name ");
            strSql.Append("      , chr.name as cashiername ");
            strSql.Append("      , hr_department.dep_name ");
            strSql.Append("  FROM [dbo].[Sale_order] ");
            strSql.Append("        INNER JOIN CRM_Customer(nolock) ON CRM_Customer.id = Sale_order.Customer_id ");
            strSql.Append("        INNER JOIN hr_employee(nolock) ON hr_employee.id = Sale_order.emp_id ");
            strSql.Append("        INNER JOIN hr_employee(nolock) as chr ON chr.id = Sale_order.cashier_id ");
            strSql.Append("        INNER JOIN hr_department(nolock) ON hr_department.id = hr_employee.dep_id ");
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
            strSql.Append("      Sale_order.[id] ");
            strSql.Append("      , Sale_order.[Serialnumber] ");
            strSql.Append("      , Sale_order.[Customer_id] ");
            strSql.Append("      , Sale_order.[Order_date] ");
            strSql.Append("      , Sale_order.[pay_type_id] ");
            strSql.Append("      , (SELECT params_name FROM Sys_Param where id = Sale_order.pay_type_id) as pay_type ");
            strSql.Append("      , Sale_order.[Order_status_id] ");
            strSql.Append("      , (SELECT params_name FROM Sys_Param where id = Sale_order.Order_status_id) as Order_status ");
            strSql.Append("      , Sale_order.[Order_amount] ");
            strSql.Append("      , Sale_order.[discount_amount] ");
            strSql.Append("      , Sale_order.[total_amount] ");
            strSql.Append("      , Sale_order.[emp_id] ");
            strSql.Append("      , Sale_order.[receive_money] ");
            strSql.Append("      , Sale_order.[arrears_money] ");
            strSql.Append("      , Sale_order.[invoice_money] ");
            strSql.Append("      , Sale_order.[arrears_invoice] ");
            strSql.Append("      , Sale_order.[Order_details] ");
            strSql.Append("      , Sale_order.[create_id] ");
            strSql.Append("      , Sale_order.[create_time] ");
            strSql.Append("      , Sale_order.[createdep_id] ");
            strSql.Append("      , Sale_order.[vipcard] ");
            strSql.Append("      , CRM_Customer.cus_name,cashier_id,vipcard,createdep_id,PayTheBill  ");
            strSql.Append("      , hr_employee.name as emp_name ");
            strSql.Append("      , hr_department.dep_name ");
            strSql.Append("      , chr.name as cashiername ");
            strSql.Append("  FROM[dbo].[Sale_order] ");
            strSql.Append("        INNER JOIN CRM_Customer(nolock) ON CRM_Customer.id = Sale_order.Customer_id ");
            strSql.Append("        INNER JOIN hr_employee(nolock) ON hr_employee.id = Sale_order.emp_id ");
            strSql.Append("        INNER JOIN hr_employee(nolock) as chr ON chr.id = Sale_order.cashier_id ");
            strSql.Append("        INNER JOIN hr_department(nolock) ON hr_department.id = hr_employee.dep_id ");
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
            strSql_inner.Append("      ,Sale_order.[id] ");
            strSql_inner.Append("      , Sale_order.[Serialnumber] ");
            strSql_inner.Append("      , Sale_order.[Customer_id] ");
            strSql_inner.Append("      , Sale_order.[Order_date] ");
            strSql_inner.Append("      , Sale_order.[pay_type_id] ");
            strSql_inner.Append("      , (SELECT params_name FROM Sys_Param where id = Sale_order.pay_type_id) as pay_type ");
            strSql_inner.Append("      , Sale_order.[Order_status_id] ");
            strSql_inner.Append("      , (SELECT params_name FROM Sys_Param where id = Sale_order.Order_status_id) as Order_status ");
            strSql_inner.Append("      , Sale_order.[Order_amount] ");
            strSql_inner.Append("      , Sale_order.[discount_amount] ");
            strSql_inner.Append("      , Sale_order.[total_amount] ");
            strSql_inner.Append("      , Sale_order.[emp_id] ");
            strSql_inner.Append("      , Sale_order.[receive_money] ");
            strSql_inner.Append("      , Sale_order.[arrears_money] ");
            strSql_inner.Append("      , Sale_order.[invoice_money] ");
            strSql_inner.Append("      , Sale_order.[arrears_invoice] ");
            strSql_inner.Append("      , Sale_order.[Order_details] ");
            strSql_inner.Append("      , Sale_order.[create_id] ");
            strSql_inner.Append("      , Sale_order.[create_time] ");
            strSql_inner.Append("      , Sale_order.[createdep_id] ");
            strSql_inner.Append("      , xsdep.[dep_name] as saledepname ");
            strSql_inner.Append("      , Sale_order.[vipcard],PayTheBill  ");
            strSql_inner.Append("      , CRM_Customer.cus_name ");
            strSql_inner.Append("      , hr_employee.name as emp_name ");
            strSql_inner.Append("      , hr_department.dep_name  ,cashier_id ");
            strSql_inner.Append("  FROM[dbo].[Sale_order] ");
            strSql_inner.Append("        INNER JOIN CRM_Customer ON CRM_Customer.id = Sale_order.Customer_id ");
            strSql_inner.Append("        INNER JOIN hr_employee ON hr_employee.id = Sale_order.emp_id ");
            strSql_inner.Append("        INNER JOIN hr_department ON hr_department.id = hr_employee.dep_id ");
            strSql_inner.Append("        INNER JOIN hr_department(nolock) as xsdep ON xsdep.id = Sale_order.createdep_id ");
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
        ///     更新发票
        /// </summary>
        public bool UpdateInvoice(string orderid)
        {
            var strSql1 = new StringBuilder();
            strSql1.Append(" /*更新发票*/ ");
            strSql1.Append(" UPDATE Sale_order SET ");
            strSql1.Append($"     invoice_money=(SELECT SUM(ISNULL(invoice_amount,0)) AS Expr1 FROM Finance_invoice WHERE order_id = '{orderid}' )  ");
            strSql1.Append($" WHERE id='{ orderid }' ");

            var strSql2 = new StringBuilder();
            strSql2.Append(" /*更新发票*/ ");
            strSql2.Append(" UPDATE Sale_order SET ");
            strSql2.Append("     arrears_invoice= ISNULL(Order_amount,0) - ISNULL(invoice_money,0)  ");
            strSql2.Append($" WHERE id='{ orderid }'");

            int rows1 = DbHelperSQL.ExecuteSql(strSql1.ToString());
            int rows2 = DbHelperSQL.ExecuteSql(strSql2.ToString());

            if (rows1 > 0 && rows2 > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     更新收款
        /// </summary>
        public bool UpdateReceive(string orderid)
        {
            var strSql1 = new StringBuilder();
            strSql1.Append("/*根据订单ID更新*/");
            strSql1.Append("UPDATE Sale_order");
            strSql1.Append($"    SET receive_money = (SELECT SUM(ISNULL(received_amount, 0)) as EX1 FROM Finance_Receivable WHERE order_id = '{orderid}')");
            strSql1.Append($"WHERE id = '{orderid}'");

            var strSql2 = new StringBuilder();
            strSql2.Append(" /*更新收款*/ ");
            strSql2.Append(" UPDATE Sale_order SET ");
            strSql2.Append("     arrears_money= ISNULL(Order_amount,0) - ISNULL(receive_money,0)  ");
            strSql2.Append(" WHERE (id='" + orderid + "') ");

            int rows1 = DbHelperSQL.ExecuteSql(strSql1.ToString());
            int rows2 = DbHelperSQL.ExecuteSql(strSql2.ToString());

            if (rows1 > 0 && rows2 > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        ///     同比环比
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet Compared_empcusorder(string year1, string month1, string year2, string month2, string idlist)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select hr_employee.name as yy,");
            strSql.Append(" SUM(case when YEAR( Sale_order.Order_date)=('" + year1 + "') and MONTH(Sale_order.Order_date)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( Sale_order.Order_date)=('" + year2 + "') and MONTH(Sale_order.Order_date)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append(" from hr_employee left outer join Sale_order ");
            strSql.Append(" on hr_employee.id=Sale_order.emp_id ");
            strSql.Append(" where hr_employee.id in " + idlist);
            strSql.Append(" group by hr_employee.name,hr_employee.id ");
            strSql.Append(" order by hr_employee.id");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        ///     客户成交统计
        /// </summary>
        public DataSet report_emporder(int year, string idlist)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select name,yy,isnull([1],0) as 'm1',isnull([2],0) as 'm2',isnull([3],0) as 'm3',isnull([4],0) as 'm4',isnull([5],0) as 'm5',isnull([6],0) as 'm6',");
            strSql.Append(" isnull([7],0) as 'm7',isnull([8],0) as 'm8',isnull([9],0) as 'm9',isnull([10],0) as 'm10',isnull([11],0) as 'm11',isnull([12],0) as 'm12' ");
            strSql.Append(" from");
            strSql.Append(" (SELECT   hr_employee.id, hr_employee.name, COUNT(derivedtbl_1.id) AS cn, YEAR(derivedtbl_1.Order_date) AS yy, ");
            strSql.Append(" MONTH(derivedtbl_1.Order_date) AS mm");
            strSql.Append(" FROM      hr_employee LEFT OUTER JOIN");
            strSql.Append("  (SELECT   id, emp_id, Order_date");
            strSql.Append("  FROM Sale_order");
            strSql.Append("  where (YEAR(Order_date) = " + year + ")) AS derivedtbl_1 ON hr_employee.id = derivedtbl_1.emp_id");
            strSql.Append(" WHERE hr_employee.id in " + idlist);
            strSql.Append(" GROUP BY hr_employee.id, hr_employee.name, YEAR(derivedtbl_1.Order_date), MONTH(derivedtbl_1.Order_date)) as tt");
            strSql.Append(" pivot");
            strSql.Append(" (sum(cn) for mm in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12]))");
            strSql.Append(" as pvt");

            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion  ExtensionMethod
    }
}

