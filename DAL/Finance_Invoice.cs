
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Finance_Invoice
	/// </summary>
	public partial class Finance_Invoice
	{
		public Finance_Invoice()
		{}
		#region  BasicMethod


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Finance_Invoice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Finance_Invoice(");
			strSql.Append("id,order_id,invoice_num,invoice_type_id,invoice_amount,invoice_content,invoice_date,empid,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@order_id,@invoice_num,@invoice_type_id,@invoice_amount,@invoice_content,@invoice_date,@empid,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@order_id", SqlDbType.VarChar,50),
					new SqlParameter("@invoice_num", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_type_id", SqlDbType.VarChar,50),
					new SqlParameter("@invoice_amount", SqlDbType.Decimal,9),
					new SqlParameter("@invoice_content", SqlDbType.VarChar,-1),
					new SqlParameter("@invoice_date", SqlDbType.DateTime),
					new SqlParameter("@empid", SqlDbType.VarChar,50),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.order_id;
			parameters[2].Value = model.invoice_num;
			parameters[3].Value = model.invoice_type_id;
			parameters[4].Value = model.invoice_amount;
			parameters[5].Value = model.invoice_content;
			parameters[6].Value = model.invoice_date;
			parameters[7].Value = model.empid;
			parameters[8].Value = model.create_id;
			parameters[9].Value = model.create_time;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Update(XHD.Model.Finance_Invoice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Finance_Invoice set ");
			strSql.Append("order_id=@order_id,");
			strSql.Append("invoice_num=@invoice_num,");
			strSql.Append("invoice_type_id=@invoice_type_id,");
			strSql.Append("invoice_amount=@invoice_amount,");
			strSql.Append("invoice_content=@invoice_content,");
			strSql.Append("invoice_date=@invoice_date,");
			strSql.Append("empid=@empid");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@order_id", SqlDbType.VarChar,50),
					new SqlParameter("@invoice_num", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_type_id", SqlDbType.VarChar,50),
					new SqlParameter("@invoice_amount", SqlDbType.Decimal,9),
					new SqlParameter("@invoice_content", SqlDbType.VarChar,-1),
					new SqlParameter("@invoice_date", SqlDbType.DateTime),
					new SqlParameter("@empid", SqlDbType.VarChar,50),					
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.order_id;
			parameters[1].Value = model.invoice_num;
			parameters[2].Value = model.invoice_type_id;
			parameters[3].Value = model.invoice_amount;
			parameters[4].Value = model.invoice_content;
			parameters[5].Value = model.invoice_date;
			parameters[6].Value = model.empid;
			parameters[7].Value = model.id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Finance_Invoice ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50)			};
			parameters[0].Value = id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Finance_Invoice ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT [Finance_Invoice].[id] ");
            strSql.Append("      ,[Finance_Invoice].[order_id] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_num] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_type_id] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_amount] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_content] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_date] ");
            strSql.Append("      ,[Finance_Invoice].[empid] ");
            strSql.Append("      ,[Finance_Invoice].[create_id] ");
            strSql.Append("      ,[Finance_Invoice].[create_time] ");
            strSql.Append("      , CRM_Customer.cus_name ");
            strSql.Append("      , Sale_order.Serialnumber as order_no ");
            strSql.Append("      , Sale_order.Order_amount ");
            strSql.Append("      , hr_employee.name ");
            strSql.Append("FROM [dbo].[Finance_Invoice] ");
            strSql.Append("    INNER JOIN Sale_order ON[Finance_Invoice].order_id = Sale_order.id ");
            strSql.Append("    INNER JOIN CRM_Customer ON Sale_order.Customer_id = CRM_Customer.id ");
            strSql.Append("    INNER JOIN hr_employee ON[Finance_Invoice].empid = hr_employee.id ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
            strSql.Append("      [Finance_Invoice].[id] ");
            strSql.Append("      ,[Finance_Invoice].[order_id] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_num] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_type_id] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_amount] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_content] ");
            strSql.Append("      ,[Finance_Invoice].[invoice_date] ");
            strSql.Append("      ,[Finance_Invoice].[empid] ");
            strSql.Append("      ,[Finance_Invoice].[create_id] ");
            strSql.Append("      ,[Finance_Invoice].[create_time] ");
            strSql.Append("      , CRM_Customer.cus_name ");
            strSql.Append("      , Sale_order.Serialnumber as order_no");
            strSql.Append("      , Sale_order.Order_amount ");
            strSql.Append("      , hr_employee.name ");
            strSql.Append("FROM [dbo].[Finance_Invoice] ");
            strSql.Append("    INNER JOIN Sale_order ON[Finance_Invoice].order_id = Sale_order.id ");
            strSql.Append("    INNER JOIN CRM_Customer ON Sale_order.Customer_id = CRM_Customer.id ");
            strSql.Append("    INNER JOIN hr_employee ON[Finance_Invoice].empid = hr_employee.id ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
            strSql_inner.Append("      ,[Finance_Invoice].[id] ");
            strSql_inner.Append("      ,[Finance_Invoice].[order_id] ");
            strSql_inner.Append("      ,[Finance_Invoice].[invoice_num] ");
            strSql_inner.Append("      ,[Finance_Invoice].[invoice_type_id] ");
            strSql_inner.Append("      ,[Finance_Invoice].[invoice_amount] ");
            strSql_inner.Append("      ,[Finance_Invoice].[invoice_content] ");
            strSql_inner.Append("      ,[Finance_Invoice].[invoice_date] ");
            strSql_inner.Append("      ,[Finance_Invoice].[empid] ");
            strSql_inner.Append("      ,[Finance_Invoice].[create_id] ");
            strSql_inner.Append("      ,[Finance_Invoice].[create_time] ");
            strSql_inner.Append("      , CRM_Customer.cus_name ");
            strSql_inner.Append("      , Sale_order.Serialnumber as order_no ");
            strSql_inner.Append("      , Sale_order.Order_amount ");
            strSql_inner.Append("      , hr_employee.name ");
            strSql_inner.Append("      , Sys_Param.params_name as invoice_type ");
            strSql_inner.Append("      , (select name from hr_employee where id = Finance_Invoice.create_id) as create_name ");
            strSql_inner.Append("FROM [dbo].[Finance_Invoice] ");
            strSql_inner.Append("    INNER JOIN Sale_order ON[Finance_Invoice].order_id = Sale_order.id ");
            strSql_inner.Append("    INNER JOIN CRM_Customer ON Sale_order.Customer_id = CRM_Customer.id ");
            strSql_inner.Append("    INNER JOIN hr_employee ON[Finance_Invoice].empid = hr_employee.id ");
            strSql_inner.Append("    INNER JOIN Sys_Param ON[Finance_Invoice].[invoice_type_id] = Sys_Param.id ");
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

        #endregion  ExtensionMethod
    }
}

