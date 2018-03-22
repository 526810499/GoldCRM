using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace XHD.Controller
{
    public class XhdResult
    {
        private bool success;

        public XhdResult() { }

        public bool isSuccess
        {
            get { return success; }
        }

        public string Message { get; set; }
        public string Type { get; set; }

        #region Success

        public static XhdResult Success()
        {
            return new XhdResult
            {
                success = true
            };
        }

        public static XhdResult Success(string message)
        {
            return new XhdResult
            {
                success = true,
                Message = message
            };
        }

        public static XhdResult Success(string message, string type)
        {
            return new XhdResult
            {
                success = true,
                Message = message,
                Type = type
            };
        }
        #endregion

        #region Error

        public static XhdResult Error()
        {
            return new XhdResult
            {
                success = false
            };
        }

        public static XhdResult Error(string message)
        {
            return new XhdResult
            {
                success = false,
                Message = message
            };
        }

        public static XhdResult Error(string message, string type)
        {
            return new XhdResult
            {
                success = false,
                Message = message,
                Type = type
            };
        }

        #endregion

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}
