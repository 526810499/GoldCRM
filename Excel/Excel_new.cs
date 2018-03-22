using System;
using System.Collections.Generic;
using System.Text;
using XHD.DBUtility;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace XHD.Excel
{
    //Excel导入核心类
    public class Excel_new
    {
        private Dictionary<string, Config> configs = null;//配置文件全路径和对应的配置类键值集合
        private string excelPath = "";//excel路径（可以为目录）
        private string configPath = "";//config路径（可以为目录）
        private NPOICell npoiCell = null;//Aspose.Cells封装类
        private string NewID = "0";
        private int headerIndex = 0;//列头所在行
        private Dictionary<string, Dictionary<string, string>> ctcode = new Dictionary<string, Dictionary<string, string>>();//代码表字典
        private DataTable temp_dt = new DataTable();//零时表
        private int success = 0;//成功条数
        private int error = 0;//失败条数
        private string errortxt = null;

        public Excel_new()
        {

        }

        public Excel_new(string excelPath, string configPath, string id)
        {
            this.NewID = id;
            this.excelPath = excelPath;
            this.configPath = configPath;
            ConfigHelper.Instance().ExcelPath = excelPath;
            ConfigHelper.Instance().ConfigPath = configPath;
            InitConfig();
        }
        //导入操作核心方法，负责整个Excel导入
        public string Import()
        {
            if (configs.Count > 0)
            {
                string x = ExcuteImport();
                return x;
            }
            else
            {
                throw new Exception("Config对象个数为0，无法导入！");
            }
        }

        //执行Excel导入
        private string ExcuteImport()
        {
            string sqlSelect = "";//代码表

            int dataIndex = 1;//数据起始行
            int tcount = dataIndex;
            int rowcount = dataIndex;



            //return excelFullName;
            //生成代码表
            List<EntityCodeTable> codetable = configs[configPath].Codetable;
            foreach (EntityCodeTable ect in codetable)
            {
                foreach (CodeTable ct in ect.Codetable)
                {
                    sqlSelect = $"select {ct.PrimaryKey} as ctkey,{ct.ReferenceColumn} as cttext from {ct.TableName} where 1=1";
                    if (!string.IsNullOrEmpty(ct.Condition)) sqlSelect += $" and {ct.Condition}";

                    DataSet ds = DbHelperSQL.Query(sqlSelect);

                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    if (ds.Tables[0].Rows.Count > 0)
                    {                        
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow dr = ds.Tables[0].Rows[i];

                            dic.Add(dr["cttext"].ToString(), dr["ctkey"].ToString());
                        }                        
                    }
                    ctcode.Add(ct.CodeName, dic);
                }

            }

            //生成表
            npoiCell = new NPOICell(excelPath);
            List<Entity> entities = configs[configPath].Entities;//注意每个config中不一定只有一个实体，可以配置多个，这样每个excel可以导入到多张表中
            headerIndex = configs[configPath].HeaderIndex;
            dataIndex = configs[configPath].DataIndex;
            rowcount = npoiCell.GetRowCount();
            foreach (Entity entity in entities)
            {
                foreach (Property p in entity.Propertys)
                {
                    string columntype = p.DataType;
                    //if (string.IsNullOrEmpty(columntype))
                    //    columntype = "System.String";
                    //else
                    columntype = "System." + columntype;
                    if (columntype == "System.string" )
                        columntype = "System.String";

                    temp_dt.Columns.Add(p.ColumnName, Type.GetType(columntype));
                }
            }
            while (IsEnd(npoiCell, dataIndex))
            {
                foreach (Entity entity in entities)
                {
                    if (entity.Propertys.Count > 0)//说明配置了Column字段
                    {
                        GetRow(npoiCell, entity, dataIndex);
                    }

                }
                dataIndex++;
            }

            //将数据插入数据库
            string connectionString = XHD.DBUtility.PubConstant.ConnectionString;
            //context.Response.Write(connectionString);
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using (SqlBulkCopy sqlBC = new SqlBulkCopy(conn))
            {
                sqlBC.BatchSize = 1000;
                sqlBC.BulkCopyTimeout = 3600;
                sqlBC.NotifyAfter = 10000;

                //sqlBC.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied); 阶段性完成事件呼叫

                foreach (Entity entity in entities)
                {
                    sqlBC.DestinationTableName = entity.Name;
                    if (entity.Propertys.Count > 0)//说明配置了Column字段
                    {
                        foreach (Property p in entity.Propertys)//遍历实体的所有属性
                        {
                            sqlBC.ColumnMappings.Add(p.ColumnName, p.ColumnName);
                        }
                    }
                }
                try
                {
                    sqlBC.WriteToServer(temp_dt);
                }
                catch (Exception ex)
                {
                    Controller.IO.LogManager.Add(ex.ToString());
                }
            }

            //return XHD.Common.GetGridJSON.DataTableToJSON(temp_dt);
            return $"{{\"success\":{success},\"error\":{error},\"message\":\"{errortxt}\"}}";
        }
        /// <summary>
        /// 得到查询值sql语句段
        /// </summary>
        /// <param name="npoiCell"></param>
        /// <param name="entity"></param>
        /// <param name="headerIndex"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        private void GetRow(NPOICell npoiCell, Entity entity, int dataIndex)
        {
            DataRow dr = temp_dt.NewRow();
            string v = "";
            foreach (Property p in entity.Propertys)//遍历实体的所有属性
            {
                if (!string.IsNullOrEmpty(p.CodeKey))
                {
                    v = GetCodeTableValue(npoiCell, p, dataIndex);//注意如果单元格本身的值就有“'”的情况
                    if (v == "xhdresult:error")
                    {
                        error++;
                        errortxt += $"第【{dataIndex}】行【{p.HeaderText}】列在系统中找不到对应的值。";
                        return;
                    }
                }
                else//说明此属性是一个代码表字段
                {
                    int columnindex = GetColumnIndexByHeaderText(npoiCell, p.HeaderText);
                    if (npoiCell.IsMerged(dataIndex, columnindex))//是否为合并单元格(对于合并单元格取此合并单元格的第一个值)
                    {
                        v = npoiCell.GetMergedCellValue(dataIndex, columnindex);
                    }
                    else
                    {
                        v = npoiCell.GetCellValue(dataIndex, columnindex);
                    }
                    if (v == "")//说明单元格中没有任何值，就要考虑“默认值”和“必须”属性
                    {
                        if (GetDefaultValue(npoiCell, entity.Name, p.ColumnName, p.DefaultValue) != "")//说明有默认值
                        {
                            v = GetDefaultValue(npoiCell, entity.Name, p.ColumnName, p.DefaultValue);
                        }
                        else//如果单元格没有值并且无默认值，则检查此属性是否是必须的
                        {
                            if (!p.Required)
                            {
                                v = "";
                            }
                            else
                            {
                                errortxt += $"列【{p.HeaderText}】不能为空。";
                                return;
                                //throw new Exception("列\"" + p.HeaderText + "\"" + "不能为空！");
                            }
                        }
                    }

                    //属性长度检查
                    if (p.DataLength != 0 && p.DataLength != 5000)
                    {
                        if (!ValidateDataLength(v, p.DataLength))
                        {
                            error++;
                            errortxt += $"列【{p.HeaderText}】中存在长度超过【{p.DataLength}】的数据。";
                            return;
                            //throw new Exception("列\"" + p.HeaderText + "\"中存长度超过\"" + p.DataLength.ToString() + "\"的数据！");
                        }
                    }

                    //检查类型
                    if (p.DataType != "" && p.DataType != "string")
                    {
                        if (p.DataType == "DateTime")
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(v))
                                {
                                    System.IFormatProvider format = new System.Globalization.CultureInfo("zh-cn", true);
                                    DateTime dateTime = DateTime.Parse(v, format);
                                }                               
                            }
                            catch
                            {
                                error++;
                                errortxt = $"列【{p.HeaderText}】中存在非【{p.DataType}】类型数据。{v}";
                            }
                        }
                        //if (!ValidateDataType(v, p.DataType))
                        //{

                        //    return;
                        //    //throw new Exception("列\"" + p.HeaderText + "\"中存在非\"" + p.DataType + "\"类型数据！");
                        //}
                    }

                }
                if (v=="")
                    dr[p.ColumnName] = DBNull.Value;
                else
                    dr[p.ColumnName] = v;
            }
            success++;
            temp_dt.Rows.Add(dr);
        }

        /// <summary>
        /// 得到代码表的对应值
        /// </summary>
        /// <param name="npoiCell"></param>
        /// <param name="property"></param>
        /// <param name="headerIndex"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        private string GetCodeTableValue(NPOICell npoiCell, Property property, int dataIndex)
        {
            int columnindex = GetColumnIndexByHeaderText(npoiCell, property.HeaderText);
            string value = npoiCell.IsMerged(dataIndex, columnindex) ? npoiCell.GetMergedCellValue(dataIndex, columnindex) : npoiCell.GetCellValue(dataIndex, columnindex).Replace("'", "''");

            if (string.IsNullOrEmpty(value))
                return "";

            Dictionary<string, string> dic = ctcode[property.CodeKey];

            string key = null;
            if (dic.ContainsKey(value))
                key = dic[value].ToString();
            else
                key = "xhdresult:error";

            return key;
        }
        /// <summary>
        /// 根据EndFlag标记判断当前数据行是否结束
        /// </summary>
        /// <param name="npoiCell"></param>
        /// <param name="endFlag"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsEnd(NPOICell npoiCell, int row)
        {
            int rowcount = npoiCell.GetRowCount();
            return row < rowcount;
        }

        /// <summary>
        /// 根据HeaderText配置节确定列索引（从0开始）
        /// </summary>
        /// <param name="npoiCell"></param>
        /// <param name="headerIndex"></param>
        /// <param name="headerText"></param>
        /// <returns></returns>
        private int GetColumnIndexByHeaderText(NPOICell npoiCell, string headerText)
        {
            int columnIndex = npoiCell.GetColumnCount();
            int r = -1;

            for (int i = 0; i < columnIndex; i++)
            {
                if (npoiCell.GetCellValue(headerIndex, i) == headerText)
                {
                    r = i;
                }
            }

            return r;
        }

        /// <summary>
        /// 根据DefaultValue配置节确定默认值
        /// </summary>
        /// <param name="npoiCell"></param>
        /// <param name="entityName"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetDefaultValue(NPOICell npoiCell, string entityName, string columnName, string defaultValue)
        {
            string r = "";
            switch (defaultValue)
            {
                case "NewID": r = this.NewID.ToString(); break;
                case "NewGuid": r = Guid.NewGuid().ToString(); break;
                case "NewTime": r = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); break;
                default: r = defaultValue; break;
            }
            return r;
        }


        /// <summary>
        /// 得到查询字段sql语句段
        /// </summary>
        /// <param name="npoiCell"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GetSqlFieldString(NPOICell npoiCell, Entity entity)
        {
            string sql = "";
            foreach (Property p in entity.Propertys)
            {
                sql += p.ColumnName + ",";
            }
            sql = sql.TrimEnd(',');
            return sql;
        }

        //数据类型校验
        private bool ValidateDataType(string value, string type)
        {
            bool r = false;

            switch (type.ToLower())
            {
                case "number":
                    double n = 0;
                    r = double.TryParse(value, out n);
                    break;
                case "DateTime":
                    try
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            System.IFormatProvider format = new System.Globalization.CultureInfo("zh-cn", true);
                            DateTime dateTime = DateTime.Parse(value, format);
                            return true;
                        }
                        r = false;
                    }
                    catch
                    {
                        r = false;
                    }
                    break;
                case "string":
                    r = true;
                    break;
                default: break; ;
            }
            return r;
        }

        //数据长度校验
        private bool ValidateDataLength(string value, int length)
        {
            if (value.Length > length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        //初始化，主要将excel文件和配置类对应关系存放到configs对象中，方便以后遍历
        private void InitConfig()
        {
            configs = new Dictionary<string, Config>();
            List<string> excelFullNames = new List<string>();
            if (Directory.Exists(configPath))//判断目录是否存在(注意：除了多套excel对应多套模板，还可能有一个模板对应多个excel的情况）
            {
                FileHelper.InitFileInfoList();
                List<string> excelConfigFileFullNames = FileHelper.GetFileInfo(".xml", ConfigHelper.Instance().ExcelPath, true, true);//在目录中查找所有名称中包含".xml"的文件
                if (excelConfigFileFullNames.Count == 1)//说明是一个excel对应一个xml配置文件的情况
                {
                    AddConfigsByXmlFullNameHasNothingToExcelname(excelConfigFileFullNames[0]);
                }
                else if (excelConfigFileFullNames.Count > 1)//说明目录中有多个xml文件，对应多个excel
                {
                    foreach (string excelConfigFileFullName in excelConfigFileFullNames)
                    {
                        AddConfigsByXmlFullName(excelConfigFileFullName);
                    }
                }
                else
                {
                    throw new Exception("所指定目录不包含任何XLM模板，请重新指定！");
                }

            }
            else//说明指定的不是目录而是excel文件路径
            {
                AddConfigByExcelFullName(configPath);
            }

        }

        //根据Excel全路径构造路径和配置类对应关系（主要用于指导Excel全路径的情况）
        private void AddConfigByExcelFullName(string excelFullName)
        {
            string excelConfigFileFullName = excelFullName.Substring(0, excelFullName.LastIndexOf("\\")) + "\\" + Path.GetFileNameWithoutExtension(excelFullName) + ".xml";
            Config config = null;
            if (File.Exists(excelConfigFileFullName))
            {
                config = new Config(excelConfigFileFullName);//创建配置（Config）对象
                configs.Add(excelFullName, config);
            }
            else
            {
                throw new Exception("所指定文件没有对应的配置文件，请重新指定！" + excelConfigFileFullName);
            }
        }

        //根据XML文件全路径构造路径和配置类对应关系(得到一个xml文件对应的所有excel然后构造config对象存放到configs中)
        private void AddConfigsByXmlFullName(string xmlFullName)
        {
            string excelDirectory = xmlFullName.Substring(0, xmlFullName.LastIndexOf("\\"));//根据路径取得对应的目录
            Config config = null;
            FileHelper.InitFileInfoList();
            List<string> excelFullNames = FileHelper.GetFileInfo(Path.GetFileNameWithoutExtension(xmlFullName), excelDirectory, true, true);//根据xml的名称搜索包含此名称的文件
            if (excelFullNames.Count >= 1)
            {
                foreach (string excelFullName in excelFullNames)
                {
                    if (excelFullNames.IndexOf(".xls") != -1)//必须是excel文件(排除xml文件)
                    {
                        config = new Config(xmlFullName);
                        configs.Add(excelFullName, config);
                    }
                }
            }
            else
            {
                throw new Exception("所指定模板不包含对应的Excel文件，请重新指定！");
            }
        }

        //根据XML文件全路径构造路径和配置类对应关系（此种情况由于只有一个xml，必然对应一个或多个excel文件，所以只需要查找excel文件即可）
        private void AddConfigsByXmlFullNameHasNothingToExcelname(string xmlFullName)
        {
            Config config = null;
            string excelDirectory = xmlFullName.Substring(0, xmlFullName.LastIndexOf("\\"));
            FileHelper.InitFileInfoList();
            List<string> excelFullNames = FileHelper.GetFileInfo(".xls", excelDirectory, true, true);
            if (excelFullNames.Count >= 1)
            {
                foreach (string excelFullName in excelFullNames)
                {
                    config = new Config(xmlFullName);
                    configs.Add(excelFullName, config);
                }
            }
            else
            {
                throw new Exception("所指定模板不包含对应的Excel文件，请重新指定！");
            }
        }
    }

    public class ctlist
    {
        public string ctkey;
        public string cttext;

        public ctlist(string text, string value)
        {
            this.CtKey = value;
            this.CtText = text;
        }

        public string CtKey
        {
            get { return ctkey; }
            set { ctkey = value; }
        }

        public string CtText
        {
            get { return cttext; }
            set { cttext = value; }
        }
    }
}
