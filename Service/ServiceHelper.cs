using System;
using System.Collections.Generic;
using System.Text;

namespace XHD.Service
{
    public class ServiceHelper
    {
        public ServiceHelper() { }

 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HostAddress"></param>
        /// <param name="HostName"></param>
        /// <param name="Suggestion"></param>
        /// <param name="QQ"></param>
        /// <param name="tel"></param>
        /// <param name="email"></param>
        /// <param name="SuggestContent"></param>
        /// <returns></returns>
        public int suggest( string Suggestion, string QQ, string tel, string email, string SuggestContent)
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string getVersion(string guid,string name)
        {
            return "1.0";
        }
    }
}
