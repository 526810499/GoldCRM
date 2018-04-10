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
    ///     ����IBatisNet�����ݷ��ʻ���
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
        ///// IsqlMapperʵ��
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
        ///     �õ��б�
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="statementName">�������ƣ���Ӧxml�е�Statement��id</param>
        /// <param name="parameterObject">����</param>
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
        ///     �õ�ָ�������ļ�¼��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject">����</param>
        /// <param name="skipResults">�����ļ�¼��</param>
        /// <param name="maxResults">��󷵻صļ�¼��</param>
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
        ///     �õ���ҳ���б�
        /// </summary>
        /// <param name="statementName">��������</param>
        /// <param name="parameterObject">����</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
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
        ///     ��ѯ�õ������һ��ʵ��
        /// </summary>
        /// <typeparam name="T">����type</typeparam>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
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
        ///     ִ�����
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
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
        ///     ִ���޸�
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
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
        ///     ִ��ɾ��
        /// </summary>
        /// <param name="statementName">������</param>
        /// <param name="parameterObject">����</param>
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
        ///// �õ���ˮ��
        ///// </summary>
        ///// <param name="tableName">����</param>
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
    /// ���ڿ���ת�����ݿ����.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ModelConvertHelper<T> where T : new()
    {

        #region DataTable

        /// <summary>
        /// ��DataTable תΪModel, ���DATATABLEΪ�գ��򲻴����κ��У����ؿ��б�
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToModels(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new List<T>();
            }
            // ��ô�ģ�͵�����
            IList<T> ts = new List<T>();
            Type type = typeof(T);

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // ��ô�ģ�͵Ĺ�������
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    string tempName = pi.Name;
                    // ���DataTable�Ƿ��������
                    if (dt.Columns.Contains(tempName))
                    {
                        // �жϴ������Ƿ���Setter
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
        /// ��DataTable�ĵ�һ�� תΪ Model,���û�����򷵻�null
        /// </summary>
        /// <param name="dt">���</param>
        /// <returns></returns>
        public static T ToModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0) return default(T);
            return ToModel(dt.Rows[0]);
        }
        /// <summary>
        /// ��DataRow��ȡ����һ�� תΪ Model
        /// </summary>
        /// <param name="dr">��</param>
        /// <returns></returns>
        public static T ToModel(DataRow dr)
        {
            // ��ô�ģ�͵�����
            Type type = typeof(T);
            string tempName = "";
            T t = new T();
            // ��ô�ģ�͵Ĺ�������
            PropertyInfo[] propertys = t.GetType().GetProperties();
            DataTable dt = dr.Table;
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;
                if (dt.Columns.Contains(tempName))
                {
                    // �жϴ������Ƿ���Setter
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
        /// ��DataReader��ȡ������תΪModel�������󲻻��Զ��ر�Reader
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
        /// �� SqlDataReader תΪModel, ��� SqlDataReader.read() ��ֵ �����ض��󣬷��򷵻�Null
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ToModel(IDataRecord dr)
        {
            // ��ô�ģ�͵�����
            Type type = typeof(T);
            T t = new T();
            // ��ô�ģ�͵Ĺ�������
            PropertyInfo[] propertys = t.GetType().GetProperties();
            int clen = dr.FieldCount;

            //������������ key = �ֶ���  value = ֵ
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