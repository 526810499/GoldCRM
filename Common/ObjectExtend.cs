using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace XHD
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 对字符串的object类型取TrimE,该对象可为空,为空则返回string.Empty
        /// </summary>
        /// <param name="strObj"></param>
        /// <returns></returns>
        public static string TrimE(this object strObj)
        {
            return (strObj == null) ? string.Empty : strObj.ToString().TrimE();
        }

        /// <summary>
        /// 从对象中取得字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string CString(this object obj, string defaultValue)
        {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : (obj.ToString().Trim());
        }



        /// <summary>
        /// 从对象中取得int数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int CInt(this object obj, int defaultValue)
        {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 从对象中取得int数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是否处理异常</param>
        /// <returns></returns>
        public static int CInt(this object obj, int defaultValue, bool isCatch)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                if (isCatch)
                {
                    throw ex;
                }
                else
                {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象中取得double数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是否处理异常</param>
        /// <returns></returns>
        public static double CDouble(this object obj, double defaultValue, bool isCatch)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch (Exception ex)
            {
                if (isCatch)
                {
                    throw ex;
                }
                else
                {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象中取得double数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是否处理异常</param>
        /// <returns></returns>
        public static float CFloat(this object obj, float defaultValue, bool isCatch)
        {
            try
            {
                return Convert.ToSingle(obj);
            }
            catch (Exception ex)
            {
                if (isCatch)
                {
                    throw ex;
                }
                else
                {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象中取得时间数据,取不到时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float? CFloat(this object obj)
        {
            if (obj.isDbNullOrNull() || obj.CString("") == "")
            {
                return null;
            }
            else
            {
                return Convert.ToSingle(obj);
            }
        }

        /// <summary>
        /// 从对象中取得int数据 取不到时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? CDouble(this object obj)
        {
            if (obj.isDbNullOrNull() || string.IsNullOrEmpty(obj.CString("")))
            {
                return null;
            }
            else
            {
                return Convert.ToDouble(obj);
            }
        }


        /// <summary>
        /// 从对象中取得long数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是否处理异常</param>
        /// <returns></returns>
        public static long CLong(this object obj, int defaultValue, bool isCatch)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch (Exception ex)
            {
                if (isCatch)
                {
                    throw ex;
                }
                else
                {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象中取得时间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime CDateTime(this object obj, DateTime defaultValue)
        {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : Convert.ToDateTime(obj);
        }
        /// <summary>
        /// 从对象中取得时间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime CDateTime(this object obj, DateTime defaultValue, bool isCatch)
        {
            try
            {
                return CDateTime(obj, defaultValue);
            }
            catch (Exception ex)
            {
                if (!isCatch) return defaultValue;
                throw ex;
            }
        }

        /// <summary>
        /// 判断对象是否是空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool isDbNullOrNull(this object obj)
        {
            return obj == null || obj == DBNull.Value;
        }

        /// <summary>
        /// 判断输入的是否数字字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_Number(this object str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str.ToString());
        }

        /// <summary>
        /// 从对象中取得int数据 取不到时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? CInt(this object obj)
        {
            if (obj.isDbNullOrNull() || string.IsNullOrEmpty(obj.CString("")))
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }



        /// <summary>
        /// 从对象中取得时间数据,取不到时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? CDecimal(this object obj)
        {
            if (obj.isDbNullOrNull() || obj.CString("") == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDecimal(obj);
            }
        }

        /// <summary>
        /// 从对象中取得long数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是否处理异常</param>
        /// <returns></returns>
        public static decimal CDecimal(this object obj, decimal defaultValue, bool isCatch)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch (Exception ex)
            {
                if (isCatch)
                {
                    throw ex;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 从对象中取得时间数据,取不到时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? CDateTime(this object obj)
        {
            if (obj.isDbNullOrNull() || obj.CString("") == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }

        /// <summary>
        /// 从对象中取得bool数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool CBoolean(this object obj, bool defaultValue)
        {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : Convert.ToBoolean(obj);
        }

        /// <summary>
        /// 如果对象为空,取默认值 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object CDefault(this object obj, object defaultValue)
        {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : obj;
        }

        /// <summary>
        /// 序列化对象为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(this object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            string result = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);

                byte[] byt = new byte[stream.Length];
                byt = stream.ToArray();

                result = Convert.ToBase64String(byt);
                stream.Flush();
            }
            return result;
        }




    }
}
