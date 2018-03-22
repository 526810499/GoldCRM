
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:CRM_contract
	/// </summary>
	public partial class CRM_contract
	{
		public CRM_contract()
		{}
		#region  BasicMethod


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.CRM_contract model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_contract(");
			strSql.Append("id,Contract_name,Serialnumber,Customer_id,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_id,Main_Content,Remarks,creater_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@Contract_name,@Serialnumber,@Customer_id,@Contract_amount,@Pay_cycle,@Start_date,@End_date,@Sign_date,@Customer_Contractor,@Our_Contractor_id,@Main_Content,@Remarks,@creater_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,50),
					new SqlParameter("@Contract_name", SqlDbType.VarChar,250),
					new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
					new SqlParameter("@Customer_id", SqlDbType.VarChar,50),					
					new SqlParameter("@Contract_amount", SqlDbType.Float,8),
					new SqlParameter("@Pay_cycle", SqlDbType.Int,4),
					new SqlParameter("@Start_date", SqlDbType.DateTime),
					new SqlParameter("@End_date", SqlDbType.DateTime),
					new SqlParameter("@Sign_date", SqlDbType.DateTime),
					new SqlParameter("@Customer_Contractor", SqlDbType.VarChar,250),
					new SqlParameter("@Our_Contractor_id", SqlDbType.VarChar,50),
					new SqlParameter("@Main_Content", SqlDbType.VarChar,-1),
					new SqlParameter("@Remarks", SqlDbType.VarChar,-1),
					new SqlParameter("@creater_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.Contract_name;
			parameters[2].Value = model.Serialnumber;
			parameters[3].Value = model.Customer_id;
			parameters[4].Value = model.Contract_amount;
			parameters[5].Value = model.Pay_cycle;
			parameters[6].Value = model.Start_date;
			parameters[7].Value = model.End_date;
			parameters[8].Value = model.Sign_date;
			parameters[9].Value = model.Customer_Contractor;
			parameters[10].Value = model.Our_Contractor_id;
			parameters[11].Value = model.Main_Content;
			parameters[12].Value = model.Remarks;
			parameters[13].Value = model.creater_id;
			parameters[14].Value = model.create_time;

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
		public bool Update(XHD.Model.CRM_contract model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_contract set ");
			strSql.Append("Contract_name=@Contract_name,");
			strSql.Append("Serialnumber=@Serialnumber,");
			strSql.Append("Contract_amount=@Contract_amount,");
			strSql.Append("Pay_cycle=@Pay_cycle,");
			strSql.Append("Start_date=@Start_date,");
			strSql.Append("End_date=@End_date,");
			strSql.Append("Sign_date=@Sign_date,");
			strSql.Append("Customer_Contractor=@Customer_Contractor,");
			strSql.Append("Our_Contractor_id=@Our_Contractor_id,");
			strSql.Append("Main_Content=@Main_Content,");
			strSql.Append("Remarks=@Remarks");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Contract_name", SqlDbType.VarChar,250),
					new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
					new SqlParameter("@Customer_id", SqlDbType.VarChar,50),
					new SqlParameter("@Contract_amount", SqlDbType.Float,8),
					new SqlParameter("@Pay_cycle", SqlDbType.Int,4),
					new SqlParameter("@Start_date", SqlDbType.DateTime),
					new SqlParameter("@End_date", SqlDbType.DateTime),
					new SqlParameter("@Sign_date", SqlDbType.DateTime),
					new SqlParameter("@Customer_Contractor", SqlDbType.VarChar,250),
					new SqlParameter("@Our_Contractor_id", SqlDbType.VarChar,50),
					new SqlParameter("@Main_Content", SqlDbType.VarChar,-1),
					new SqlParameter("@Remarks", SqlDbType.VarChar,-1),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.Contract_name;
			parameters[1].Value = model.Serialnumber;
			parameters[2].Value = model.Customer_id;
			parameters[3].Value = model.Contract_amount;
			parameters[4].Value = model.Pay_cycle;
			parameters[5].Value = model.Start_date;
			parameters[6].Value = model.End_date;
			parameters[7].Value = model.Sign_date;
			parameters[8].Value = model.Customer_Contractor;
			parameters[9].Value = model.Our_Contractor_id;
			parameters[10].Value = model.Main_Content;
			parameters[11].Value = model.Remarks;
			parameters[12].Value = model.id;

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
			strSql.Append("delete from CRM_contract ");
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
			strSql.Append("delete from CRM_contract ");
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
			strSql.Append("select id,Contract_name,Serialnumber,Customer_id,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_id,Main_Content,Remarks,creater_id,create_time ");
            strSql.Append(",(select name from hr_employee where id = CRM_contract.Our_Contractor_id) as Our_Contractor");
            strSql.Append(",(select cus_name from CRM_customer where id = CRM_contract.Customer_id) as cus_name");
            strSql.Append(" FROM CRM_contract ");
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
			strSql.Append(" id,Contract_name,Serialnumber,Customer_id,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_id,Main_Content,Remarks,creater_id,create_time ");
            strSql.Append(",(select name from hr_employee where id = CRM_contract.Our_Contractor_id) as Our_Contractor");
            strSql.Append(",(select cus_name from CRM_customer where id = CRM_contract.Customer_id) as cus_name");
            strSql.Append(" FROM CRM_contract ");
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
			StringBuilder strSql_grid = new StringBuilder();
			StringBuilder strSql_total = new StringBuilder();
			strSql_total.Append(" SELECT COUNT(id) FROM CRM_contract ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,Contract_name,Serialnumber,Customer_id,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_id,Main_Content,Remarks,creater_id,create_time ");
            strSql_grid.Append(" ,(select name from hr_employee where id = w1.Our_Contractor_id) as Our_Contractor");
            strSql_grid.Append(" ,(select cus_name from CRM_customer where id = w1.Customer_id) as cus_name");
            strSql_grid.Append(" ,(select dep_name from hr_department where id = (select dep_id from hr_employee where id = w1.Our_Contractor_id)) as department ");
            strSql_grid.Append(" FROM ( SELECT id,Contract_name,Serialnumber,Customer_id,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_id,Main_Content,Remarks,creater_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from CRM_contract");
			if (strWhere.Trim() != "")
			{
				strSql_grid.Append(" WHERE " + strWhere);
				strSql_total.Append(" WHERE " + strWhere);
			}
			strSql_grid.Append("  ) as w1  ");
			strSql_grid.Append("WHERE n BETWEEN " + PageSize * (PageIndex - 1) + " AND " + PageSize * PageIndex);
			strSql_grid.Append(" ORDER BY " + filedOrder );
			Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql_grid.ToString());
		 }

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

