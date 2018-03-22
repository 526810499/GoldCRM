
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Finance_Receivable
	/// </summary>
	public partial class Finance_Receivable
	{
		public Finance_Receivable()
		{}
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Finance_Receivable model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Finance_Receivable(");
            strSql.Append("id,receivable_no,order_id,receivable_time,receivable_amount,received_amount,arrears_amount,Remark,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@receivable_no,@order_id,@receivable_time,@receivable_amount,@received_amount,@arrears_amount,@Remark,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@receivable_no", SqlDbType.VarChar,50),
                    new SqlParameter("@order_id", SqlDbType.VarChar,50),
                    new SqlParameter("@receivable_time", SqlDbType.DateTime),
                    new SqlParameter("@receivable_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@received_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@arrears_amount", SqlDbType.Decimal,9),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.receivable_no;
            parameters[2].Value = model.order_id;
            parameters[3].Value = model.receivable_time;
            parameters[4].Value = model.receivable_amount;
            parameters[5].Value = model.received_amount;
            parameters[6].Value = model.arrears_amount;
            parameters[7].Value = model.Remark;
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
        public bool Update(XHD.Model.Finance_Receivable model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Finance_Receivable set ");
			strSql.Append("order_id=@order_id,");
			strSql.Append("receivable_time=@receivable_time,");
			strSql.Append("receivable_amount=@receivable_amount,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@receivable_no", SqlDbType.VarChar,50),
					new SqlParameter("@order_id", SqlDbType.VarChar,50),
					new SqlParameter("@receivable_time", SqlDbType.DateTime),
					new SqlParameter("@receivable_amount", SqlDbType.Decimal,9),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.receivable_no;
			parameters[1].Value = model.order_id;
			parameters[2].Value = model.receivable_time;
			parameters[3].Value = model.receivable_amount;
			parameters[4].Value = model.Remark;
			parameters[5].Value = model.id;

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
			strSql.Append("delete from Finance_Receivable ");
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
			strSql.Append("delete from Finance_Receivable ");
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
            strSql.Append("select ");
            strSql.Append("      Finance_Receivable.[id] ");
            strSql.Append("      , Finance_Receivable.[receivable_no] ");
            strSql.Append("      , Finance_Receivable.[order_id] ");
            strSql.Append("      , Finance_Receivable.[receivable_time] ");
            strSql.Append("      , Finance_Receivable.[receivable_amount] ");
            strSql.Append("      , Finance_Receivable.[received_amount] ");
            strSql.Append("      , Finance_Receivable.[arrears_amount] ");
            strSql.Append("      , Finance_Receivable.[Remark] ");
            strSql.Append("      , Finance_Receivable.[create_id] ");
            strSql.Append("      , Finance_Receivable.[create_time] ");
            strSql.Append("      , Sale_order.Serialnumber as Order_no ");
            strSql.Append("      , Sale_order.Order_amount ");
            strSql.Append("      , CRM_Customer.id as Cus_id ");
            strSql.Append("      , CRM_Customer.cus_name ");
            strSql.Append("FROM[dbo].[Finance_Receivable] ");
            strSql.Append("       INNER JOIN Sale_order ON Finance_Receivable.order_id = Sale_order.id ");
            strSql.Append("       INNER JOIN CRM_Customer ON Sale_order.Customer_id = CRM_Customer.id ");
            if (strWhere.Trim()!="")
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
            strSql.Append("      Finance_Receivable.[id] ");
            strSql.Append("      , Finance_Receivable.[receivable_no] ");
            strSql.Append("      , Finance_Receivable.[order_id] ");
            strSql.Append("      , Finance_Receivable.[receivable_time] ");
            strSql.Append("      , Finance_Receivable.[receivable_amount] ");
            strSql.Append("      , Finance_Receivable.[received_amount] ");
            strSql.Append("      , Finance_Receivable.[arrears_amount] ");
            strSql.Append("      , Finance_Receivable.[Remark] ");
            strSql.Append("      , Finance_Receivable.[create_id] ");
            strSql.Append("      , Finance_Receivable.[create_time] ");
            strSql.Append("      , Sale_order.Serialnumber as Order_no ");
            strSql.Append("      , Sale_order.Order_amount ");
            strSql.Append("      , CRM_Customer.id as Cus_id ");
            strSql.Append("      , CRM_Customer.cus_name ");
            strSql.Append("FROM[dbo].[Finance_Receivable] ");
            strSql.Append("       INNER JOIN Sale_order ON Finance_Receivable.order_id = Sale_order.id ");
            strSql.Append("       INNER JOIN CRM_Customer ON Sale_order.Customer_id = CRM_Customer.id ");
            if (strWhere.Trim()!="")
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
            strSql_inner.Append("      , Finance_Receivable.[id] ");
            strSql_inner.Append("      , Finance_Receivable.[receivable_no] ");
            strSql_inner.Append("      , Finance_Receivable.[order_id] ");
            strSql_inner.Append("      , Finance_Receivable.[receivable_time] ");
            strSql_inner.Append("      , Finance_Receivable.[receivable_amount] ");
            strSql_inner.Append("      , Finance_Receivable.[received_amount] ");
            strSql_inner.Append("      , Finance_Receivable.[arrears_amount] ");
            strSql_inner.Append("      , Finance_Receivable.[Remark] ");
            strSql_inner.Append("      , Finance_Receivable.[create_id] ");
            strSql_inner.Append("      , Finance_Receivable.[create_time] ");
            strSql_inner.Append("      , Sale_order.Serialnumber as Order_no ");
            strSql_inner.Append("      , Sale_order.Order_amount ");
            strSql_inner.Append("      , CRM_Customer.id as Cus_id ");
            strSql_inner.Append("      , CRM_Customer.cus_name ");
            strSql_inner.Append("FROM[dbo].[Finance_Receivable] ");
            strSql_inner.Append("       INNER JOIN Sale_order ON Finance_Receivable.order_id = Sale_order.id ");
            strSql_inner.Append("       INNER JOIN CRM_Customer ON Sale_order.Customer_id = CRM_Customer.id "); 
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

