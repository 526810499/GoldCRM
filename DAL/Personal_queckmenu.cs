
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Personal_queckmenu
	/// </summary>
	public partial class Personal_queckmenu
	{
		public Personal_queckmenu()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Personal_queckmenu model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Personal_queckmenu(");
			strSql.Append("user_id,menu_id)");
			strSql.Append(" values (");
			strSql.Append("@user_id,@menu_id)");
			SqlParameter[] parameters = {
					new SqlParameter("@user_id", SqlDbType.VarChar,50),
					new SqlParameter("@menu_id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.user_id;
			parameters[1].Value = model.menu_id;

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
		public bool Delete(XHD.Model.Personal_queckmenu model)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append(" delete from Personal_queckmenu ");
			strSql.Append(" where ");
            strSql.Append(" user_id=@user_id");
            strSql.Append(" and menu_id=@menu_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@user_id", SqlDbType.VarChar,50),
                    new SqlParameter("@menu_id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.menu_id;

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
			strSql.Append("select user_id,menu_id ");
            strSql.Append(" ,(select Menu_name from Sys_Menu where Menu_id = Personal_queckmenu.menu_id) as Menu_name");
            strSql.Append(" ,(select Menu_icon from Sys_Menu where Menu_id = Personal_queckmenu.menu_id) as Menu_icon");
            strSql.Append(" ,(select Menu_url from Sys_Menu where Menu_id = Personal_queckmenu.menu_id) as Menu_url");
            strSql.Append(" FROM Personal_queckmenu ");
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
			strSql.Append(" user_id,menu_id ");
            strSql.Append(" ,(select Menu_name from Sys_Menu where Menu_id = Personal_queckmenu.menu_id) as Menu_name");
            strSql.Append(" ,(select Menu_icon from Sys_Menu where Menu_id = Personal_queckmenu.menu_id) as Menu_icon");
            strSql.Append(" ,(select Menu_url from Sys_Menu where Menu_id = Personal_queckmenu.menu_id) as Menu_url");
            strSql.Append(" FROM Personal_queckmenu ");
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

