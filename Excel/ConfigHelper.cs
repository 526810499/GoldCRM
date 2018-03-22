namespace XHD.Excel
{
    //配置辅助类，主要用于读取应用程序配置文件
    internal class ConfigHelper
    {
        private static ConfigHelper configHelper = null;
        private static object obj = new object();
        private string configPath = "";//配置文件目录
        private string excelPath = "";//Excel文件所在路径或者其根目录（此时会将其下所有excel全部倒入）
        private bool useTransaction = false;

        private ConfigHelper()
        {
            
        }

        public static ConfigHelper Instance()
        {
            lock (obj)
            {
                if (configHelper == null)
                {
                    configHelper = new ConfigHelper();
                }
            }
            return configHelper;
        }
        public string ConfigPath
        {
            get
            {
                return configPath;
            }
            set
            {
                configPath = value;
            }
        }

        public string ExcelPath
        {
            get
            {
                return excelPath;
            }
            set
            {
                excelPath = value;
            }
        }

        public bool UseTransaction
        {
            get
            {
                return useTransaction;
            }
            set
            {
                useTransaction = value;
            }
        }
    }
}
