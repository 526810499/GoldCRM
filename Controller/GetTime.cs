

using System;

namespace XHD.Controller
{
    public class GetTime
    {
        public static int GetTimeZone()
        {
            DateTime now = DateTime.Now;
            DateTime utcnow = now.ToUniversalTime();

            TimeSpan sp = now - utcnow;

            return sp.Hours;
        }

        public static long MilliTimeStamp(DateTime theDate)
        {
            var d1 = new DateTime(1970, 1, 1);
            DateTime d2 = theDate.ToUniversalTime();
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long) ts.TotalMilliseconds;
        }
    }
}