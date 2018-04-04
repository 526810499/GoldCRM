using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XHD.DBUtility;

namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:Product_retrieval
    /// </summary>
    public partial class DProduct_Retrieval
    {
        public DProduct_Retrieval()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Product_Retrieval model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_retrieval(");
            strSql.Append("id,weight,number,createdep_id,category_id,remark,create_time,create_id,status)");
            strSql.Append(" values (");
            strSql.Append("@id,@weight,@number,@createdep_id,@category_id,@remark,@create_time,@create_id,@status)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@weight", SqlDbType.Decimal,9),
                    new SqlParameter("@number", SqlDbType.Int,4),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@category_id", SqlDbType.VarChar,50),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.Int,4)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.weight;
            parameters[2].Value = model.number;
            parameters[3].Value = model.createdep_id;
            parameters[4].Value = model.category_id;
            parameters[5].Value = model.remark;
            parameters[6].Value = model.create_time;
            parameters[7].Value = model.create_id;
            parameters[8].Value = model.status;

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
        public bool Update(Model.Product_Retrieval model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_retrieval set ");
            strSql.Append("weight=@weight,");
            strSql.Append("number=@number,");
            strSql.Append("createdep_id=@createdep_id,");
            strSql.Append("category_id=@category_id,");
            strSql.Append("remark=@remark,");
            strSql.Append("status=@status");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@weight", SqlDbType.Decimal,9),
                    new SqlParameter("@number", SqlDbType.Int,4),
                    new SqlParameter("@createdep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@category_id", SqlDbType.VarChar,50),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.weight;
            parameters[1].Value = model.number;
            parameters[2].Value = model.createdep_id;
            parameters[3].Value = model.category_id;
            parameters[4].Value = model.remark;
            parameters[5].Value = model.status;
            parameters[6].Value = model.id;

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
        public bool Delete(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_retrieval ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)         };
            parameters[0].Value = id;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product_retrieval ");
            strSql.Append(" where id in (" + idlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,weight,number,createdep_id,category_id,remark,create_time,create_id,status ");
            strSql.Append(" FROM Product_retrieval ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            strSql.Append(" id,weight,number,createdep_id,category_id,remark,create_time,create_id,status ");
            strSql.Append(" FROM Product_retrieval ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            strSql_total.Append(" SELECT COUNT(id) FROM Product_retrieval(nolock) ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      *");
            strSql_grid.Append(",(select product_category from Product_category(nolock) where id = w1.category_id) as category_name");
            strSql_grid.Append(",(select dep_name from hr_department(nolock) where id = w1.createdep_id) as depname");
            strSql_grid.Append(" FROM ( SELECT *, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_retrieval");
            if (strWhere.Trim() != "")
            {
                strSql_grid.Append(" WHERE " + strWhere);
                strSql_total.Append(" WHERE " + strWhere);
            }
            strSql_grid.Append("  ) as w1  ");
            strSql_grid.Append("WHERE n BETWEEN " + (PageSize * (PageIndex - 1) + 1) + " AND " + PageSize * PageIndex);
            strSql_grid.Append(" ORDER BY " + filedOrder);
            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }


        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Product_retrieval ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.id desc");
            }
            strSql.Append(")AS Row, T.*  from Product_retrieval T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Product_retrieval";
			parameters[1].Value = "id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

