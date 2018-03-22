using System.Collections.Generic;

namespace XHD.Excel
{
    //实体类，对配置文件中Table的抽象，对应数据库中的表
    internal class EntityCodeTable
    {
        private string name = "";//表名称       
        private List<CodeTable> codetable = null;//代码表集合（每个实体对应多个Property）

        public EntityCodeTable()
        {           
            codetable = new List<CodeTable>();
        }

        public EntityCodeTable(string name)
        {
            this.name=name;          
            codetable = new List<CodeTable>();
        }
        
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }     

        public List<CodeTable> Codetable
        {
            get
            {
                return codetable;
            }
            set
            {
                codetable = value;
            }
        }
    }
}
