
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:Sys_data_authority
	/// </summary>
	public partial class Sys_data_authority
	{
		public Sys_data_authority()
		{}
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.Sys_data_authority model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Sys_data_authority(");
            strSql.Append("Role_id,dep_id,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@Role_id,@dep_id,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@Role_id", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_id", SqlDbType.VarChar,50),                    
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.Role_id;
            parameters[1].Value = model.dep_id;
            parameters[2].Value = model.create_id;
            parameters[3].Value = model.create_time;

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
			strSql.Append("delete from Sys_data_authority ");
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
			strSql.Append("select Role_id,dep_id,create_id,create_time ");
			strSql.Append(" FROM Sys_data_authority ");
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

