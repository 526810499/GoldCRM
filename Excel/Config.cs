using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace XHD.Excel
{
    //配置类，是对整个配置的抽象
    internal class Config
    {
        #region　excel读取相关成员变量
        private XmlHelper xml = null;
        #endregion

        #region 配置文件相关成员变量
        private string endFlag = "RowBlank";//Excel结束标志
        private byte headerIndex = 0;//列头所在行
        private byte dataIndex = 1;//数据行起始位置
        private List<Entity> entities = null;//配置类对应的实体类（也就是说配置时，每个Config节点中可以有多个Table节点）
        private List<EntityCodeTable> codetables = null;//代码表
        #endregion

        public Config(string excelConfigFullName)
        {
            //初始化成员变量
            entities = new List<Entity>();
            codetables = new List<EntityCodeTable>();
            //读取配置文件，类初始化
            Init(excelConfigFullName);
        }

        public string EndFlag
        {
            get
            {
                return endFlag;
            }
        }

        public int HeaderIndex
        {
            get
            {
                return headerIndex;
            }
        }

        public int DataIndex
        {
            get
            {
                return dataIndex;
            }
        }

        public List<Entity> Entities
        {
            get
            {
                return entities;
            }
        }

        public List<EntityCodeTable> Codetable
        {
            get
            {
                return codetables;
            }
        }

        //初始化配置对象
        private void Init(string excelConfigFullName)
        {
            int t = 1;
            bool r = true;
            xml = new XmlHelper(excelConfigFullName);
            endFlag = xml.GetSingleNodeAttribute("Config", "EndFlag") != "" ? xml.GetSingleNodeAttribute("Config", "EndFlag") : "RowBlank";//默认判断是否读取结束的标志设为“RowBlank”，也就是空行。
            r = int.TryParse(xml.GetSingleNodeAttribute("Config", "HeaderIndex"), out t);
            if (r)
            {
                headerIndex = Convert.ToByte(xml.GetSingleNodeAttribute("Config", "HeaderIndex"));
            }
            r = int.TryParse(xml.GetSingleNodeAttribute("Config", "DataIndex"), out t);
            if (r)
            {
                dataIndex = Convert.ToByte(xml.GetSingleNodeAttribute("Config", "DataIndex"));
            }
            if (File.Exists(excelConfigFullName))//根据完整路径判断文件是否存在
            {
                AddEnities(excelConfigFullName);
                AddCodetable(excelConfigFullName);
            }
            else
            {
                throw new Exception("相应配置文件未找到，请检查相应文件是否存在！");
            }
        }

        //初始化实体对象
        private void AddEnities(string fullName)
        {
            xml = new XmlHelper(fullName);
            XmlNodeList tables = xml.GetNodes("Config/Table");
            Entity entity = null;
            Property property = null;
            bool t = true;
            bool r = true;
            int i = 0;
            foreach (XmlNode table in tables)
            {
                entity = new Entity(xml.GetNodeAttribute(table, "Name"));
                r = bool.TryParse(xml.GetNodeAttribute(table, "DeleteRepeat"), out t);
                if (r)
                {
                    entity.DeleteRepeat = Convert.ToBoolean(xml.GetNodeAttribute(table, "DeleteRepeat"));
                }

                if (xml.GetFirstChildNode(table) != null)//说明有列配置
                {

                    foreach (XmlNode column in xml.GetChildNodes(table))
                    {
                        property = new Property();
                        r = bool.TryParse(xml.GetNodeAttribute(column, "IsPrimaryKey"), out t);
                        if (r)
                        {
                            property.IsPrimaryKey = Convert.ToBoolean(xml.GetNodeAttribute(column, "IsPrimaryKey"));
                        }
                        property.ColumnName = xml.GetNodeAttribute(column, "ColumnName");
                        if (xml.GetNodeAttribute(column, "HeaderText") != "")
                        {
                            property.HeaderText = xml.GetNodeAttribute(column, "HeaderText");
                        }
                        r = bool.TryParse(xml.GetNodeAttribute(column, "Required"), out t);
                        if (r)
                        {
                            property.Required = Convert.ToBoolean(xml.GetNodeAttribute(column, "Required"));
                        }
                        property.DataType = xml.GetNodeAttribute(column, "DataType") != "" ? xml.GetNodeAttribute(column, "DataType") : "string";
                        if (xml.GetNodeAttribute(column, "DataLength") != "")
                        {
                            r = int.TryParse(xml.GetNodeAttribute(column, "DataLength"), out i);
                            if (r)
                            {
                                property.DataLength = i;
                            }
                        }
                        property.Comment = xml.GetNodeAttribute(column, "Comment");
                        property.DefaultValue = xml.GetNodeAttribute(column, "DefaultValue") != "" ? xml.GetNodeAttribute(column, "DefaultValue") : "";
                        property.CodeKey = xml.GetNodeAttribute(column, "CodeKey");
                        entity.Propertys.Add(property);
                    }
                    entities.Add(entity);
                }
                else
                {
                    string excludedColumns = xml.GetNodeAttribute(table, "ExcludedColumns");
                    if (excludedColumns != "")
                    {
                        foreach (string ec in excludedColumns.Split(','))
                        {
                            entity.ExcludedColumns.Add(ec);
                        }
                    }
                    entities.Add(entity);
                }

            }
        }

        //初始化代码表
        private void AddCodetable(string fullName)
        {
            xml = new XmlHelper(fullName);
            XmlNodeList tables = xml.GetNodes("Config/CodeTable");
            EntityCodeTable entity = null;
            CodeTable codetable = null;

            foreach (XmlNode table in tables)
            {
                entity = new EntityCodeTable(xml.GetNodeAttribute(table, "Name"));

                if (xml.GetFirstChildNode(table) != null)//说明有列配置
                {
                    foreach (XmlNode column in xml.GetChildNodes(table))
                    {
                        codetable = new CodeTable();
                       
                        codetable.CodeName = xml.GetNodeAttribute(column, "CodeName");
                        codetable.TableName = xml.GetNodeAttribute(column, "TableName");
                        codetable.PrimaryKey = xml.GetNodeAttribute(column, "PrimaryKey");
                        codetable.ReferenceColumn = xml.GetNodeAttribute(column, "ReferenceColumn");
                        codetable.Condition = xml.GetNodeAttribute(column, "Condition");

                        entity.Codetable.Add(codetable);
                    }
                    codetables.Add(entity);
                }
            }
        }

    }
}
