namespace XHD.Excel
{
    //代码表类，是对CodeTable的抽象
    internal class DictionaryEntity
    {
        private string name = "";//代码表名称，对应数据库中主表名称
        private string primaryKey = "";//代码表主键，也就是字表的对应外键
        private string referenceColumn = "";//代码字段关联名称列，也就是我们导入时所依据的excel对应值
        private string condition = "";//相关条件
        public DictionaryEntity()
        {
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

        public string PrimaryKey
        {
            get
            {
                return primaryKey;
            }
            set
            {
                primaryKey = value;
            }
        }

        public string ReferenceColumn
        {
            get
            {
                return referenceColumn;
            }
            set
            {
                referenceColumn = value;
            }
        }

        public string Condition
        {
            get
            {
                return condition;
            }
            set
            {
                condition = value;
            }
        }
    }
}
