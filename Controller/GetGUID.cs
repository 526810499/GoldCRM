using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHD.Controller
{
    public class GUID
    {
        /// <summary>
        /// 产生一个GUID
        /// </summary>
        /// <returns></returns>
        public static Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 获取长整型GUID
        /// </summary>
        /// <returns></returns>
        public static long GetLongGuid()
        {
            return GetLongGuid(GetGuid().ToString());
        }

        /// <summary>
        /// 获取长整型GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static long GetLongGuid(string guid)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(guid);
            var longguid= BitConverter.ToInt64(buffer, 0);

            return longguid;
        }
    }
}
