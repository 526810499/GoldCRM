
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Product_warehouse
    /// </summary>
    public partial class Product_warehouse
    {
		public Product_warehouse()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(XHD.Model.Product_warehouse model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Product_warehouse(");
			strSql.Append("product_warehouse,parentid,product_icon,create_id,create_time)");
			strSql.Append(" values (");
			strSql.Append("@product_warehouse,@parentid,@product_icon,@create_id,@create_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@product_warehouse", SqlDbType.VarChar,250),
					new SqlParameter("@parentid", SqlDbType.VarChar,50),
					new SqlParameter("@product_icon", SqlDbType.VarChar,250),					
					new SqlParameter("@create_id", SqlDbType.VarChar,50),
					new SqlParameter("@create_time", SqlDbType.DateTime),
            };
			parameters[0].Value = model.product_warehouse;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.product_icon;
			parameters[3].Value = model.create_id;
			parameters[4].Value = model.create_time;


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
		public bool Update(XHD.Model.Product_warehouse model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update product_warehouse set ");
			strSql.Append("product_warehouse=@product_warehouse,");
			strSql.Append("parentid=@parentid,");
			strSql.Append("product_icon=@product_icon");
            strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@product_warehouse", SqlDbType.VarChar,250),
					new SqlParameter("@parentid", SqlDbType.VarChar,50),
					new SqlParameter("@product_icon", SqlDbType.VarChar,250),
					new SqlParameter("@id", SqlDbType.Int,4),
            };
			parameters[0].Value = model.product_warehouse;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.product_icon;
			parameters[3].Value = model.id;

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
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from product_warehouse ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)			};
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
			strSql.Append("delete from product_warehouse ");
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
			strSql.Append("select id,product_warehouse,parentid,product_icon,create_id,create_time ");
			strSql.Append(" FROM product_warehouse ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
            strSql.Append(" id,product_warehouse,parentid,product_icon,create_id,create_time ");
            strSql.Append(" FROM product_warehouse ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

