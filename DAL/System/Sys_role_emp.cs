
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Sys_role_emp
	/// </summary>
	public partial class Sys_role_emp
	{
		public Sys_role_emp()
		{}
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_role_emp model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_role_emp(");
            strSql.Append("RoleID,empID)");
            strSql.Append(" values (");
            strSql.Append("@RoleID,@empID)");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.VarChar,50),
                    new SqlParameter("@empID", SqlDbType.VarChar,50)};
            parameters[0].Value = model.RoleID;
            parameters[1].Value = model.empID;

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
        public bool Delete(string where)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_role_emp ");
			strSql.Append(" where "+where);

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
			strSql.Append("select RoleID,empID ");
			strSql.Append(" FROM Sys_role_emp ");
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

