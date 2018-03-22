
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:hr_post
	/// </summary>
	public partial class hr_post
	{
		public hr_post()
		{}
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.hr_post model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hr_post(");
            strSql.Append("id,post_name,position_id,dep_id,emp_id,default_post,note,post_descript,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@post_name,@position_id,@dep_id,@emp_id,@default_post,@note,@post_descript,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@post_name", SqlDbType.NVarChar,255),
                    new SqlParameter("@position_id", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                    new SqlParameter("@default_post", SqlDbType.Int,4),
                    new SqlParameter("@note", SqlDbType.NVarChar,-1),
                    new SqlParameter("@post_descript", SqlDbType.NVarChar,-1),                    
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.post_name;
            parameters[2].Value = model.position_id;
            parameters[3].Value = model.dep_id;
            parameters[4].Value = model.emp_id;
            parameters[5].Value = model.default_post;
            parameters[6].Value = model.note;
            parameters[7].Value = model.post_descript;
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
        public bool Update(XHD.Model.hr_post model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hr_post set ");
			strSql.Append("post_name=@post_name,");
			strSql.Append("position_id=@position_id,");
			strSql.Append("dep_id=@dep_id,");
			strSql.Append("emp_id=@emp_id,");
			strSql.Append("default_post=@default_post,");
			strSql.Append("note=@note,");
			strSql.Append("post_descript=@post_descript");
			strSql.Append(" where id=@id  ");
			SqlParameter[] parameters = {
					new SqlParameter("@post_name", SqlDbType.NVarChar,255),
					new SqlParameter("@position_id", SqlDbType.VarChar,50),
					new SqlParameter("@dep_id", SqlDbType.VarChar,50),
					new SqlParameter("@emp_id", SqlDbType.VarChar,50),
					new SqlParameter("@default_post", SqlDbType.Int,4),
					new SqlParameter("@note", SqlDbType.NVarChar,-1),
					new SqlParameter("@post_descript", SqlDbType.NVarChar,-1),
					new SqlParameter("@id", SqlDbType.VarChar,50)};
			parameters[0].Value = model.post_name;
			parameters[1].Value = model.position_id;
			parameters[2].Value = model.dep_id;
			parameters[3].Value = model.emp_id;
			parameters[4].Value = model.default_post;
			parameters[5].Value = model.note;
			parameters[6].Value = model.post_descript;
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
			strSql.Append("delete from hr_post ");
			strSql.Append(" where id=@id  ");
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,post_name,position_id,dep_id,emp_id,default_post,note,post_descript,create_id,create_time ");
            strSql.Append(" ,(select position_name from hr_position where id = hr_post.[position_id]) as [position_name]  ");
            strSql.Append(" ,(select position_order from hr_position where id = hr_post.[position_id]) as [position_order]  ");
            strSql.Append(" ,(select dep_name from hr_department where id = hr_post.[dep_id]) as [dep_name]  ");
            strSql.Append(" ,(select name from hr_employee where id = hr_post.[emp_id]) as [emp_name]  ");
            strSql.Append(" FROM hr_post ");
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
			strSql.Append(" id,post_name,position_id,dep_id,emp_id,default_post,note,post_descript,create_id,create_time ");
            strSql.Append(" ,(select position_name from hr_position where id = hr_post.[position_id]) as [position_name]  ");
            strSql.Append(" ,(select position_order from hr_position where id = hr_post.[position_id]) as [position_order]  ");
            strSql.Append(" ,(select dep_name from hr_department where id = hr_post.[dep_id]) as [dep_name]  ");
            strSql.Append(" ,(select name from hr_employee where id = hr_post.[emp_id]) as [emp_name]  ");
            strSql.Append(" FROM hr_post ");
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
			strSql_total.Append(" SELECT COUNT(create_time) FROM hr_post ");
			strSql_grid.Append("SELECT ");
			strSql_grid.Append("      n,id,post_name,position_id,dep_id,emp_id,default_post,note,post_descript,create_id,create_time ");
            strSql_grid.Append(" ,(select position_name from hr_position where id = w1.[position_id]) as [position_name]  ");
            strSql_grid.Append(" ,(select position_order from hr_position where id = w1.[position_id]) as [position_order]  ");
            strSql_grid.Append(" ,(select dep_name from hr_department where id = w1.[dep_id]) as [dep_name]  ");
            strSql_grid.Append(" ,(select name from hr_employee where id = w1.[emp_id]) as [emp_name]  ");
            strSql_grid.Append(" FROM ( SELECT id,post_name,position_id,dep_id,emp_id,default_post,note,post_descript,create_id,create_time, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from hr_post");
			if (strWhere.Trim() != "")
			{
				strSql_grid.Append(" WHERE " + strWhere);
				strSql_total.Append(" WHERE " + strWhere);
			}
			strSql_grid.Append("  ) as w1  ");
			strSql_grid.Append("WHERE n BETWEEN " + (PageSize * (PageIndex - 1) + 1) + " AND " + PageSize * PageIndex);
			strSql_grid.Append(" ORDER BY " + filedOrder );
			Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql_grid.ToString());
		 }

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        ///     更新岗位人员
        /// </summary>
        public bool UpdatePostEmp(Model.hr_post model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update hr_post set ");
            strSql.Append("emp_id=@emp_id,");
            strSql.Append("default_post=@default_post");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@emp_id", SqlDbType.VarChar,50),
                new SqlParameter("@default_post", SqlDbType.Int, 4),
                new SqlParameter("@id", SqlDbType.VarChar,50)
            };

            parameters[0].Value = model.emp_id;
            parameters[1].Value = model.default_post;
            parameters[2].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     清空更新岗位人员
        /// </summary>
        public bool UpdatePostEmpbyEid(string empid)
        {
            var strSql = new StringBuilder();
            strSql.Append("update hr_post set ");
            strSql.Append("emp_id='',");
            strSql.Append("default_post=0");
            strSql.Append(" where emp_id=@emp_id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@emp_id", SqlDbType.VarChar,50)
            };

            parameters[0].Value = empid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }
        #endregion  ExtensionMethod
    }
}

