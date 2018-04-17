using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XHD.DBUtility;

namespace XHD.DAL
{
    /// <summary>
    /// 事物基类
    /// </summary>
    public class BaseTransaction
    {

        protected virtual bool ExecTran(System.Data.SqlClient.SqlCommand cm, int execRows)
        {
            System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(DbHelperSQL.connectionString);
            bool rs = false;
            cm.Connection = cnn;
            cnn.Open();
            System.Data.SqlClient.SqlTransaction trans = cnn.BeginTransaction();
            try
            {
                cm.Transaction = trans;
                int rows = cm.ExecuteNonQuery();

                if (rows == execRows || execRows == -1)
                {
                    rs = true;
                    trans.Commit();
                }
                else {
                    trans.Rollback();
                }
            }
            catch (Exception error)
            {
                trans.Rollback();
                SoftLog.LogStr(error.ToString(), this.GetType().Name);
            }
            finally
            {
                cnn.Close();
                trans.Dispose();
                cnn.Dispose();
            }
            return rs;
        }

    }
}
