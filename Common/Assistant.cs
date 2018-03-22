using System;

namespace XHD.Common
{
    /// <summary>
    ///     Assistant 的摘要说明。
    /// </summary>
    public sealed class Assistant
    {
        #region

        /// <summary>
        ///     从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            var rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp*i*((int) DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        public static string GetRandomNum(int CodeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,0";
            return GetRandomCode(allChar, CodeCount);
        }

        public static string GetRandomAll(int CodeCount)
        {
            string allChar =
                "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            return GetRandomCode(allChar, CodeCount);
        }

        #endregion
    }
}