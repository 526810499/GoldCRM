using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
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
        /// bitmap 转byte
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] Bitmap2Byte(this Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
        /// <summary>
        /// bytes 转 bitmap
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(this byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
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
                return CInt(obj,defaultValue);
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
                return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : Convert.ToDouble(obj);
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
                return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : Convert.ToSingle(obj);
 
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
                return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : Convert.ToInt64(obj);

               
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

        /**//// <summary>
            /// MD5 16位加密 加密后密码为大写
            /// </summary>
            /// <param name="ConvertString"></param>
            /// <returns></returns>
        public static string MD5ToHex(this string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /**//// <summary>
            /// MD5　32位加密
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
        public static string ToMD5(this string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符

                pwd = pwd + s[i].ToString("X");

            }
            return pwd;
        }


        /// <summary>
        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串
        /// </summary>
        /// <param name="CnStr">汉字字符串</param>
        /// <param name="ignoreNumber">是否忽略数字</param>
        /// <returns>相对应的汉语拼音首字母串</returns>

        public static string GetSpellCode(this string CnStr, bool ignoreNumber = false)
        {

            string strTemp = "";

            int iLen = CnStr.Length;

            int i = 0;

            for (i = 0; i <= iLen - 1; i++)
            {
                string t = GetCharSpellCode(CnStr.Substring(i, 1), ignoreNumber); ;
                strTemp += t;

            }

            return strTemp;

        }

        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
        /// </summary>
        /// <param name="CnChar">单个汉字</param>
        /// <param name="ignoreNumber">是否忽略数字</param>
        /// <returns>单个大写字母</returns>

        private static string GetCharSpellCode(this string CnChar, bool ignoreNumber = false)
        {

            long iCnChar;

            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回

            if (ZW.Length == 1)
            {
                int assci = (char)ZW[0];
                //0到9的忽略
                if (ignoreNumber && assci > 46 && assci < 59)
                {
                    return "";
                }
                return CnChar.ToUpper();

            }

            else {

                // get the array of byte from the single char

                int i1 = (short)(ZW[0]);

                int i2 = (short)(ZW[1]);

                iCnChar = i1 * 256 + i2;

            }

            // iCnChar match the constant

            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {

                return "A";

            }

            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {

                return "B";

            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {

                return "C";

            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {

                return "D";

            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {

                return "E";

            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {

                return "F";

            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {

                return "G";

            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {

                return "H";

            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {

                return "J";

            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {

                return "K";

            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {

                return "L";

            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {

                return "M";

            }
            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {

                return "N";

            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {

                return "O";

            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {

                return "P";

            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {

                return "Q";

            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {

                return "R";

            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {

                return "S";

            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {

                return "T";

            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {

                return "W";

            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {

                return "X";

            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {

                return "Y";

            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {

                return "Z";

            }
            else

                return ("?");

        }

    }
}
