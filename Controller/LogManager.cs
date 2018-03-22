using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace XHD.Controller.IO
{
    public class LogManager
    {
        public static void Add(string log, string strDirectory)
        {

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string FileName = path + strDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                // Write the string to a file.
                StreamWriter file = new StreamWriter(FileName, true);
                file.WriteLine(DateTime.Now.ToString() + " > " + log, false);
                file.Close();
                Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public static void Add(string log)
        {
            Add(log, "Logs");
        }
    }

}
