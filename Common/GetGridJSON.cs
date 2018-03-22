using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XHD.Common
{
    public class GetGridJSON
    {
        #region

        public static string DataTableToJSON(DataTable dt)
        {
            if(dt.Rows.Count==0) return @"{""Rows"":[],""Total"":0}";
            try
            {
                string rowsjson = JsonConvert.SerializeObject(dt, new DataTableConverter());
                string json = @"{""Rows"":" + rowsjson + @",""Total"":""" + dt.Rows.Count + @"""}";
                return json;
            }
            catch 
            {
                return @"{""Rows"":[],""Total"":0}";
            }
        }

        public static string DataTableToJSON1(DataTable dt, string Total)
        {
            if (dt.Rows.Count == 0) return @"{""Rows"":[],""Total"":0}";
            try
            {
                string rowsjson = JsonConvert.SerializeObject(dt, new DataTableConverter());
                string json = @"{""Rows"":" + rowsjson + @",""Total"":""" + Total + @"""}";
                return json;
            }
            catch 
            {
                return @"{""Rows"":[],""Total"":0}";
            }
        }

        /// <summary>
        ///     ÆÕÍ¨Êý×éjson
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJSON2(DataTable dt)
        {
            if (dt.Rows.Count == 0) return @"{""Rows"":[],""Total"":0}";
            try
            {
                string rowsjson = JsonConvert.SerializeObject(dt, new DataTableConverter());
                return rowsjson;
            }
            catch 
            {
                return "[]";
            }
        }

        #endregion
    }
}