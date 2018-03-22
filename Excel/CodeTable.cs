using System;
using System.Collections.Generic;
using System.Text;

namespace XHD.Excel
{
    internal class CodeTable
    {
        public CodeTable() {

        }

        private string codename;//代码表标识
        private string tablename;//代码表名
        private string primarykey;//代码表key列
        private string referencecolumn;//代码表名称列
        private string condition;//查询条件

        public string CodeName
        {
            get { return codename; }
            set { codename = value; }
        }

        public string TableName
        {
            get { return tablename; }
            set { tablename = value; }
        }

        public string PrimaryKey
        {
            get { return primarykey; }
            set { primarykey = value; }
        }

        public string ReferenceColumn
        {
            get { return referencecolumn; }
            set { referencecolumn = value; }
        }

        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }
    }
}
