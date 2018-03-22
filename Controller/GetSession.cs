

using System.Web;
using System.Web.SessionState;

namespace XHD.Controller
{
    public class GetSession : IRequiresSessionState
    {
        private static HttpSessionState Session = HttpContext.Current.Session;
        private HttpRequest Request = HttpContext.Current.Request;
        private HttpResponse Response = HttpContext.Current.Response;
        private HttpServerUtility Server = HttpContext.Current.Server;

        public static HttpSessionState session
        {
            get { return Session; }
            set { Session = value; }
        }
    }
}