
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Sys_info
	/// </summary>
	public partial class Sys_info
	{
		public Sys_info()
		{}
		#region  BasicMethod


		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.Sys_info model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_info set ");			
			strSql.Append("sys_value=@sys_value");
			strSql.Append(" where  ");
            strSql.Append("sys_key=@sys_key");
			SqlParameter[] parameters = {
					new SqlParameter("@sys_key", SqlDbType.VarChar,50),
					new SqlParameter("@sys_value", SqlDbType.NVarChar,-1)
            };
			parameters[0].Value = model.sys_key;
			parameters[1].Value = model.sys_value;

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
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select sys_key,sys_value ");
			strSql.Append(" FROM Sys_info(nolock) ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		
		
		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

