using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IBatisNet.Common.Pagination;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.Exceptions;
using System.Data;

namespace XHD.DBUtility
{
    /// <summary>
    ///     基于IBatisNet的数据访问基类
    /// </summary>
    public class BaseSqlMapDao
    {
        private readonly ISqlMapper sqlMap;

        public BaseSqlMapDao()
        {
            //DomSqlMapBuilder builder = new DomSqlMapBuilder(true);
            //sqlMap = builder.Configure();

            Assembly assembly = Assembly.Load("IBatisNetDemo");
            Stream stream = assembly.GetManifestResourceStream("IBatisNetDemo.sqlmap.config");

            var builder = new DomSqlMapBuilder();
            sqlMap = builder.Configure(stream);
        }

        ///// <summary>
        ///// IsqlMapper实例
        ///// </summary>
        ///// <returns></returns>
        //public static ISqlMapper sqlMap = (ContainerAccessorUtil.GetContainer())["sqlServerSqlMap"] as ISqlMapper;

        //public SqlMapper SqlMap
        //{
        //    get
        //    {
        //        Assembly assembly = Assembly.Load("IBatisNetDemo");
        //        Stream stream = assembly.GetManifestResourceStream("IBatisNetDemo.sqlmap.config");

        //        DomSqlMapBuilder builder = new DomSqlMapBuilder();
        //        builder.
        //        SqlMapper sqlMap = builder.Configure(stream);
        //    }
        //}

        /// <summary>
        ///     得到列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="statementName">操作名称，对应xml中的Statement的id</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        protected IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject)
        {
            try
            {
                return sqlMap.QueryForList<T>(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException(
                    "Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        ///     得到指定数量的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject">参数</param>
        /// <param name="skipResults">跳过的记录数</param>
        /// <param name="maxResults">最大返回的记录数</param>
        /// <returns></returns>
        protected IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject, int skipResults,
            int maxResults)
        {
            try
            {
                return sqlMap.QueryForList<T>(statementName, parameterObject, skipResults, maxResults);
            }
            catch (Exception e)
            {
                throw new DataMapperException(
                    "Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        ///     得到分页的列表
        /// </summary>
        /// <param name="statementName">操作名称</param>
        /// <param name="parameterObject">参数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        protected IPaginatedList ExecuteQueryForPaginatedList(string statementName, object parameterObject, int pageSize)
        {
            try
            {
                return sqlMap.QueryForPaginatedList(statementName, parameterObject, pageSize);
            }
            catch (Exception e)
            {
                throw new DataMapperException(
                    "Error executing query '" + statementName + "' for paginated list.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        ///     查询得到对象的一个实例
        /// </summary>
        /// <typeparam name="T">对象type</typeparam>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        protected T ExecuteQueryForObject<T>(string statementName, object parameterObject)
        {
            try
            {
                return sqlMap.QueryForObject<T>(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException(
                    "Error executing query '" + statementName + "' for object.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        ///     执行添加
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        protected void ExecuteInsert(string statementName, object parameterObject)
        {
            try
            {
                sqlMap.Insert(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException(
                    "Error executing query '" + statementName + "' for insert.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        ///     执行修改
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        protected void ExecuteUpdate(string statementName, object parameterObject)
        {
            try
            {
                sqlMap.Update(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException(
                    "Error executing query '" + statementName + "' for update.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        ///     执行删除
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        protected void ExecuteDelete(string statementName, object parameterObject)
        {
            try
            {
                sqlMap.Delete(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException(
                    "Error executing query '" + statementName + "' for delete.  Cause: " + e.Message, e);
            }
        }

        ///// <summary>
        ///// 得到流水号
        ///// </summary>
        ///// <param name="tableName">表名</param>
        ///// <returns></returns>
        //public int GetId(string tableName)
        //{
        //    try
        //    {
        //        Stream stream = sqlMap.QueryForObject("GetStreamId", tableName) as Stream;
        //        return stream.IMaxID;
        //    }
        //    catch (Exception e)
        //    {
        //        throw (e);
        //    }
        //}
    }


    /// <summary>
    /// 用于快速转换数据库对象.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ModelConvertHelper<T> where T : new()
    {

        #region DataTable

        /// <summary>
        /// 将DataTable 转为Model, 如果DATATABLE为空，或不存在任何行，返回空列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToModels(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new List<T>();
            }
            // 获得此模型的类型
            IList<T> ts = new List<T>();
            Type type = typeof(T);

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    string tempName = pi.Name;
                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite)
                            continue;
                        object value = dr[tempName];

                        if (value == null)
                            continue;
                        if (value == DBNull.Value)
                            continue;

                        var pType = GetPropertyType(pi.PropertyType);
                        if (pType.IsEnum)
                        {
                            pi.SetValue(t, Enum.Parse(pType, value.ToString().Trim(), true), null);
                        }
                        else {
                            pi.SetValue(t, Convert.ChangeType(value, pType), null);
                        }

                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 将DataTable的第一行 转为 Model,如果没数据则返回null
        /// </summary>
        /// <param name="dt">表格</param>
        /// <returns></returns>
        public static T ToModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0) return default(T);
            return ToModel(dt.Rows[0]);
        }
        /// <summary>
        /// 将DataRow读取到的一行 转为 Model
        /// </summary>
        /// <param name="dr">行</param>
        /// <returns></returns>
        public static T ToModel(DataRow dr)
        {
            // 获得此模型的类型
            Type type = typeof(T);
            string tempName = "";
            T t = new T();
            // 获得此模型的公共属性
            PropertyInfo[] propertys = t.GetType().GetProperties();
            DataTable dt = dr.Table;
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;
                if (dt.Columns.Contains(tempName))
                {
                    // 判断此属性是否有Setter
                    if (!pi.CanWrite)
                        continue;
                    object value = dr[tempName];
                    if (value == null)
                        continue;
                    if (value == DBNull.Value)
                        continue;

                    var pType = GetPropertyType(pi.PropertyType);
                    if (pType.IsEnum)
                    {
                        pi.SetValue(t, Enum.Parse(pType, value.ToString().Trim(), true), null);
                    }
                    else {
                        pi.SetValue(t, Convert.ChangeType(value, pType), null);
                    }
                }
            }
            return t;
        }

        #endregion

        #region reader
        /// <summary>
        /// 将DataReader读取的内容转为Model，结束后不会自动关闭Reader
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static IList<T> ToModels(IDataReader dr)
        {
            IList<T> ts = new List<T>();
            while (dr.Read())
            {
                ts.Add(ToModel(dr));
            }
            return ts;
        }

        /// <summary>
        /// 将 SqlDataReader 转为Model, 如果 SqlDataReader.read() 有值 ，返回对象，否则返回Null
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ToModel(IDataRecord dr)
        {
            // 获得此模型的类型
            Type type = typeof(T);
            T t = new T();
            // 获得此模型的公共属性
            PropertyInfo[] propertys = t.GetType().GetProperties();
            int clen = dr.FieldCount;

            //遍历所有列名 key = 字段名  value = 值
            Dictionary<string, object> nv = new Dictionary<string, object>();

            for (int i = 0; i < clen; i++)
            {
                string fieldname = dr.GetName(i).ToLower();
                nv[fieldname] = dr[i];
            }

            foreach (PropertyInfo pi in propertys)
            {
                var tempName = pi.Name.ToLower();
                if (nv.ContainsKey(tempName))
                {

                    if (!pi.CanWrite)
                        continue;
                    object value = nv[tempName];
                    if (value != DBNull.Value)
                    {
                        var pType = GetPropertyType(pi.PropertyType);
                        if (pType.IsEnum)
                        {
                            pi.SetValue(t, Enum.Parse(pType, value.ToString().Trim(), true), null);
                        }
                        else {
                            pi.SetValue(t, Convert.ChangeType(value, pType), null);
                        }
                    }
                }
            }
            return t;
        }

        private static Type GetPropertyType(Type pType)
        {
            var gTypes = pType.GetGenericArguments();
            if (gTypes.Length > 0) return gTypes[0];
            return pType;
        }
        #endregion

    }
}