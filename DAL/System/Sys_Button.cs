
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Sys_Button
	/// </summary>
	public partial class Sys_Button
	{
		public Sys_Button()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Sys_Button model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_Button(");
			strSql.Append("Btn_id,Btn_name,Btn_type,Btn_icon,Btn_handler,Menu_id,Btn_order,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@Btn_id,@Btn_name,@Btn_type,@Btn_icon,@Btn_handler,@Menu_id,@Btn_order,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_id", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_name", SqlDbType.NVarChar,255),
					new SqlParameter("@Btn_type", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_icon", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_handler", SqlDbType.VarChar,255),
					new SqlParameter("@Menu_id", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_order", SqlDbType.Int,4),
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime)};
			parameters[0].Value = model.Btn_id;
			parameters[1].Value = model.Btn_name;
			parameters[2].Value = model.Btn_type;
			parameters[3].Value = model.Btn_icon;
			parameters[4].Value = model.Btn_handler;
			parameters[5].Value = model.Menu_id;
			parameters[6].Value = model.Btn_order;
			parameters[7].Value = model.create_id;
			parameters[8].Value = model.create_time;

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
		public bool Update(XHD.Model.Sys_Button model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_Button set ");
			strSql.Append("Btn_name=@Btn_name,");
			strSql.Append("Btn_type=@Btn_type,");
			strSql.Append("Btn_icon=@Btn_icon,");
			strSql.Append("Btn_handler=@Btn_handler,");
			strSql.Append("Menu_id=@Menu_id,");
			strSql.Append("Btn_order=@Btn_order");
			strSql.Append(" where Btn_id=@Btn_id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_name", SqlDbType.NVarChar,255),
					new SqlParameter("@Btn_type", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_icon", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_handler", SqlDbType.VarChar,255),
					new SqlParameter("@Menu_id", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_order", SqlDbType.Int,4),
					new SqlParameter("@Btn_id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.Btn_name;
			parameters[1].Value = model.Btn_type;
			parameters[2].Value = model.Btn_icon;
			parameters[3].Value = model.Btn_handler;
			parameters[4].Value = model.Menu_id;
			parameters[5].Value = model.Btn_order;
			parameters[6].Value = model.Btn_id;

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
		public bool Delete(string Btn_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_Button ");
			strSql.Append(" where Btn_id=@Btn_id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_id", SqlDbType.VarChar,50)			};
			parameters[0].Value = Btn_id;

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
		public bool DeleteList(string Btn_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_Button ");
			strSql.Append(" where Btn_id in ("+Btn_idlist + ")  ");
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
			strSql.Append("select Btn_id,Btn_name,Btn_type,Btn_icon,Btn_handler,Menu_id,Btn_order,create_id,create_time ");
			strSql.Append(" FROM Sys_Button ");
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
			strSql.Append(" Btn_id,Btn_name,Btn_type,Btn_icon,Btn_handler,Menu_id,Btn_order,create_id,create_time ");
			strSql.Append(" FROM Sys_Button ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

