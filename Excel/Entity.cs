using System.Collections.Generic;

namespace XHD.Excel
{
    //实体类，对配置文件中Table的抽象，对应数据库中的表
    internal class Entity
    {
        private string name = "";//表名称
        private bool deleteRepeat = false;//是否删除重复（默认为false，如果设为true则在插入时首先根据主键删除重复信息）
        private List<Property> propertys = null;//属性集合（每个实体对应多个Property）
        private List<string> excludedColumns = null;//排除字段（也就是指明哪些字段不用导入）

        public Entity()
        {
            propertys = new List<Property>();
            excludedColumns = new List<string>();
        }

        public Entity(string name)
        {
            this.name=name;
            excludedColumns = new List<string>();
            propertys = new List<Property>();
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

        public bool DeleteRepeat
        {
            get
            {
                return deleteRepeat;
            }
            set
            {
                deleteRepeat = value;
            }
        }

        public List<string> ExcludedColumns
        {
            get
            {
                return excludedColumns;
            }
            set
            {
                excludedColumns = value;
            }
        }

        public List<Property> Propertys
        {
            get
            {
                return propertys;
            }
            set
            {
                propertys = value;
            }
        }      
    }
}
