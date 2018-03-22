namespace XHD.Excel
{
    //配置属性类，抽象了配置文件的配置属性，对应于数据库中的字段
    internal class Property
    {
        private bool isPrimaryKey=false;//是否为主键
        private string columnName = "";//数据库列名称
        private string headerText = "";//对应的excel列头名称
        private bool required = false;//是否为必填字段
        private string dataType = "string";//数据类型（默认为string类型）
        private int dataLength = 5000;//数据长度（默认为5000）
        private string defaultValue = "";//默认值(对应excel此列的值如果为空则会使用此值来导入)
        private string comment = "";//字段说明信息（非必要属性）
        private string codekey = null;//对应的代码表

        public Property()
        {
        }

        public bool IsPrimaryKey
        {
            get
            {
                return isPrimaryKey;
            }
            set
            {
                isPrimaryKey = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;
            }
        }

        public string HeaderText
        {
            get
            {
                return headerText;
            }
            set
            {
                headerText = value;
            }
        }

        public bool Required
        {
            get
            {
                return required;
            }
            set
            {
                required = value;
            }
        }

        public string DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }

        public int DataLength
        {
            get
            {
                return dataLength;
            }
            set
            {
                dataLength = value;
            }
        }

        public string DefaultValue//解析过的默认值（也就是说直接就是值，而不是其地址什么的）
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
            }
        }

        public string Comment//说明信息
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        }

        public string CodeKey//代码表
        {
            get
            {
                return codekey;
            }
            set
            {
                codekey = value;
            }
        }
    }
}
