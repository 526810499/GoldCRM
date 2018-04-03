
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using XHD.BLL;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class xhd_Service : BaseCRMServer
    {
 
        public xhd_Service()
        {
        }

        public xhd_Service(HttpContext context) : base(context) { }

        /// <summary>
        /// 建议
        /// </summary>
        /// <returns></returns>
        public int suggest()
        {
            return 1;
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public string getVersion()
        {
            return "";
        }

    }
}
