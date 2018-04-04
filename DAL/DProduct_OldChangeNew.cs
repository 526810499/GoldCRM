using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XHD.DBUtility;
using XHD.Model;

namespace XHD.DAL
{
    /// <summary>
    /// 以旧换新
    /// </summary>
    /// <summary>
    /// 数据访问类:DProduct_OldChangeNew
    /// </summary>
    public partial class DProduct_OldChangeNew
    {
        public DProduct_OldChangeNew()
        { }
        #region  BasicMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Product_OldChangeNew model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product_OldChangeNew(");
            strSql.Append("id,oldWeight,oldTotalPrice,oldCharge,newWeight,newTotalPrice,costsTotalPrice,discount,difTotalPrice,status,remark,create_time,create_id,createdep_id)");
            strSql.Append(" values (");
            strSql.Append("@id,@oldWeight,@oldTotalPrice,@oldCharge,@newWeight,@newTotalPrice,@costsTotalPrice,@discount,@difTotalPrice,@status,@remark,@create_time,@create_id,@createdep_id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@oldWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@oldTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@oldCharge", SqlDbType.Decimal,9),
                    new SqlParameter("@newWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@newTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@costsTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@discount", SqlDbType.Decimal,9),
                    new SqlParameter("@difTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@createdep_id",SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.id;
            parameters[1].Value = model.oldWeight;
            parameters[2].Value = model.oldTotalPrice;
            parameters[3].Value = model.oldCharge;
            parameters[4].Value = model.newWeight;
            parameters[5].Value = model.newTotalPrice;
            parameters[6].Value = model.costsTotalPrice;
            parameters[7].Value = model.discount;
            parameters[8].Value = model.difTotalPrice;
            parameters[9].Value = model.status;
            parameters[10].Value = model.remark;
            parameters[11].Value = model.create_time;
            parameters[12].Value = model.create_id;
            parameters[13].Value = model.createdep_id;

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
        public bool Update(Model.Product_OldChangeNew model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product_OldChangeNew set ");
            strSql.Append("oldWeight=@oldWeight,");
            strSql.Append("oldTotalPrice=@oldTotalPrice,");
            strSql.Append("oldCharge=@oldCharge,");
            strSql.Append("newWeight=@newWeight,");
            strSql.Append("newTotalPrice=@newTotalPrice,");
            strSql.Append("costsTotalPrice=@costsTotalPrice,");
            strSql.Append("discount=@discount,");
            strSql.Append("difTotalPrice=@difTotalPrice,");
            strSql.Append("status=@status,");
            strSql.Append("remark=@remark ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@oldWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@oldTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@oldCharge", SqlDbType.Decimal,9),
                    new SqlParameter("@newWeight", SqlDbType.Decimal,9),
                    new SqlParameter("@newTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@costsTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@discount", SqlDbType.Decimal,9),
                    new SqlParameter("@difTotalPrice", SqlDbType.Decimal,9),
                    new SqlParameter("@status", SqlDbType.Int,4),
                    new SqlParameter("@remark", SqlDbType.NVarChar,50),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.oldWeight;
            parameters[1].Value = model.oldTotalPrice;
            parameters[2].Value = model.oldCharge;
            parameters[3].Value = model.newWeight;
            parameters[4].Value = model.newTotalPrice;
            parameters[5].Value = model.costsTotalPrice;
            parameters[6].Value = model.discount;
            parameters[7].Value = model.difTotalPrice;
            parameters[8].Value = model.status;
            parameters[9].Value = model.remark;
            parameters[10].Value = model.id;

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
            strSql.Append("delete from Product_OldChangeNew ");
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
            strSql.Append("delete from Product_OldChangeNew ");
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
            strSql.Append("select id,oldWeight,oldTotalPrice,oldCharge,newWeight,newTotalPrice,costsTotalPrice,discount,difTotalPrice,status,remark,create_time,create_id,createdep_id ");
            strSql.Append(" FROM Product_OldChangeNew ");
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
            strSql.Append(" id,oldWeight,oldTotalPrice,oldCharge,newWeight,newTotalPrice,costsTotalPrice,discount,difTotalPrice,status,remark,create_time,create_id,createdep_id ");
            strSql.Append(" FROM Product_OldChangeNew ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Product_OldChangeNew ");
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
            strSql.Append(")AS Row, T.*  from Product_OldChangeNew T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();
            strSql_total.Append(" SELECT COUNT(id) FROM Product_OldChangeNew(nolock) ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      *");
            strSql_grid.Append(" FROM ( SELECT *, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from Product_OldChangeNew(nolock)");
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
			parameters[0].Value = "Product_OldChangeNew";
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

