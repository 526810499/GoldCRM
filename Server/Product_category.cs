
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Product_category : BaseCRMServer
    {
        public static BLL.Product_category category = new BLL.Product_category();
        public static Model.Product_category model = new Model.Product_category();



        public Product_category()
        {
        }

        public Product_category(HttpContext context) : base(context) { }

        public string save()
        {
            model.parentid = PageValidate.InputText(request["T_category_parent_val"], 50);
            model.product_category = PageValidate.InputText(request["T_category_name"], 250);
            model.product_icon = PageValidate.InputText(request["T_category_icon"], 250);
            model.cproperty = request["category_attr"].CInt(0, false);
            model.CodingBegins = PageValidate.InputText(request["T_CodingBegins"], 50).ToUpper();
            string id = PageValidate.InputText(request["id"], 50);
            string pid = PageValidate.InputText(request["T_category_parent_val"], 50);
            if (string.IsNullOrWhiteSpace(model.parentid))
            {
                model.parentid = "root";
            }

            if (string.IsNullOrWhiteSpace(model.product_category))
            {
                return XhdResult.Error("分类名称不能为空！").ToString();
            }

            Model.Product_category categoryOldModel = category.GetcategoryID(model.product_category);
            if (categoryOldModel != null && !string.IsNullOrWhiteSpace(categoryOldModel.id) && categoryOldModel.id != id)
            {
                return XhdResult.Error("当前分类名称已存在请确认！").ToString();
            }

            if (PageValidate.checkID(id))
            {
                model.id = id;

                DataSet ds = category.GetList($" id= '{id}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];

                if (id == pid)
                    return XhdResult.Error("上级不能是自己，更新失败！").ToString();

                string oldparentid = dr["parentid"].CString("");
                if (model.parentid != oldparentid || string.IsNullOrWhiteSpace(dr["fparentid"].CString("")))
                {
                    if (model.parentid == "root" || model.parentid == "")
                    {
                        model.fparentid = model.id;
                    }
                    else {
                        model.fparentid = GetParentid(model.parentid).id;
                    }
                }

                category.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.product_category;
                string EventType = "商品类别修改";
                string EventID = model.id;
                string Log_Content = null;

                if (dr["product_category"].CString("") != request["T_category_name"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "商品类别", dr["product_category"].ToString(), request["T_category_name"]);

                if (dr["product_icon"].CString("") != request["T_category_icon"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "类别图标", dr["product_icon"].ToString(), request["T_category_icon"]);

                if (dr["parentid"].CString("") != request["T_category_parent_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "上级类别", dr["parentid"].ToString(), request["T_category_parent_val"]);
                if (dr["CodingBegins"].CString("") != request["T_CodingBegins"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "CodingBegins", dr["CodingBegins"].ToString(), request["T_CodingBegins"]);
                if (dr["cproperty"].CString("") != request["T_category_attr_val"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "cproperty", dr["cproperty"].ToString(), request["T_category_attr_val"]);
                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                model.id = Guid.NewGuid().ToString();
                if (model.parentid == "root" || model.parentid == "")
                {
                    model.fparentid = model.id;
                }
                else {
                    model.fparentid = GetParentid(model.parentid).id;
                }

                model.create_id = emp_id;
                model.create_time = DateTime.Now;

                category.Add(model);

            }

            if (!string.IsNullOrWhiteSpace(model.CodingBegins))
            {
                category.AddNO(model.CodingBegins);
            }

            return XhdResult.Success().ToString();
        }

        /// <summary>
        /// 递归获取父级的父级
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private Model.Product_category GetParentid(string pid)
        {

            Model.Product_category nmodel = category.GetModel(pid);

            if (nmodel.parentid.CString("") == "root" || nmodel.parentid.CString("") == "")
            {
                return nmodel;
            }
            return GetParentid(nmodel.parentid);
        }

        public string grid()
        {
            string sorttext = " create_time desc ";

            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["company"]))
                serchtxt += " and product_category like N'%" + PageValidate.InputText(request["company"], 50) + "%'";

            DataSet ds = category.GetList(0, serchtxt, sorttext);
            return "{\"Rows\":[" + GetTasks.GetTasksString("root", ds.Tables[0]) + "]}";

        }

        public string tree()
        {
            DataSet ds = category.GetList($"1=1");
            var str = new StringBuilder();
            str.Append("[");
            bool qb = request["qb"].CInt(0, false) == 1;
            if (qb)
            {
                str.Append("{\"id\":\"\",\"text\":\"全部\",\"d_icon\":\"\"");
                str.Append(",\"children\":[");
            }
            bool qxz = request["qxz"].CInt(0, false) == 1;
            if (qxz)
            {
                str.Append("{\"id\":\"\",\"text\":\"==所有==\",\"d_icon\":\"\"},");
            }
            qxz = request["qxzg"].CInt(0, false) == 1;
            if (qxz)
            {
                str.Append("{\"id\":\"root\",\"text\":\"==根==\",\"d_icon\":\"\"},");
            }
            str.Append(GetTreeString("root", ds.Tables[0]));
            str.Replace(",", "", str.Length - 1, 1);

            if (qb)
            {
                str.Append("]}");
            }
            str.Append("]");
            return str.ToString();
        }

        public string combo()
        {
            DataSet ds = category.GetList($"1=1");
            var str = new StringBuilder();
            str.Append("[");
            str.Append("{\"id\":\"root\",\"text\":\"无\",\"d_icon\":\"\"},");
            str.Append(GetTreeString("root", ds.Tables[0]));
            str.Replace(",", "", str.Length - 1, 1);
            str.Append("]");
            return str.ToString();
        }

        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = category.GetList($"id='{id}' ");
            return DataToJson.DataToJSON(ds);

        }

        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return "false";
            id = PageValidate.InputText(id, 50);
            DataSet ds = category.GetList($" id = '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            var product = new BLL.Product();
            if (product.GetList($" category_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此类别下含有商品，不允许删除！").ToString();

            if (category.GetList($"parentid = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此类别下含有下级，不允许删除！").ToString();

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "ECD91B11-2CEF-497D-883F-C431496E68CC");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = category.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误！").ToString();

            //日志
            string EventType = "商品类别删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["product_category"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }


        private static string GetTreeString(string Id, DataTable table)
        {
            DataRow[] rows = table.Select($"parentid='{Id}'");

            if (rows.Length == 0) return string.Empty;
            ;
            var str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{\"id\":\"" + (string)row["id"] + "\",\"text\":\"" + (string)row["product_category"] + "\",\"d_icon\":\"../../" + (string)row["product_icon"] + "\"");

                if (GetTreeString((string)row["id"], table).Length > 0)
                {
                    str.Append(",\"children\":[");
                    str.Append(GetTreeString((string)row["id"], table));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }


        /// <summary>
        /// 批量导入
        /// </summary>
        /// <returns></returns>
        public string ImportCategory()
        {
            //品名	克重	单价	成本	标签价	条码
            string msg = "";
            try
            {
                string pid = request["pid"].CString("");
                pid = PageValidate.InputText(pid, 50);
                HttpPostedFile uploadFile = request.Files[0];
                if (uploadFile == null || string.IsNullOrWhiteSpace(pid))
                {
                    return XhdResult.Error("确认参数是否正常").ToString();
                }
                string filename = uploadFile.FileName;
                ExcelExtension ension = ExcelExtension.XLS;
                if (filename.LastIndexOf(".xlsx") > -1)
                {
                    ension = ExcelExtension.XLSX;
                }
                Stream stream = uploadFile.InputStream;

                DataTable table = ExportHelper.ReadExcelToDataTable(stream, ension);
                if (table == null || table.Rows.Count <= 0)
                {
                    return XhdResult.Error("表格数据未读到").ToString();
                }
                if (!table.Columns.Contains("分类名称"))
                {
                    return XhdResult.Error("请确认导入的Excel 模板格式是否正确").ToString();
                }



                Model.Product_category pmodel = category.GetModel(pid);
                if (pmodel == null || string.IsNullOrWhiteSpace(pmodel.id))
                {
                    return XhdResult.Error("参数不正确，更新失败！").ToString();
                }

                int cproperty = pmodel.cproperty;
                string fparentid = pmodel.fparentid;
                string oldparentid = pmodel.parentid;
                if (string.IsNullOrWhiteSpace(fparentid))
                {
                    if (pmodel.parentid == "root" || pmodel.parentid == "")
                    {
                        fparentid = model.id;
                    }
                    else {
                        fparentid = GetParentid(model.parentid).id;
                    }
                }


                int index = 1;
                int rs = 0;
                int continueCount = 0;

                foreach (DataRow rows in table.Rows)
                {
                    try
                    {
                        string name = rows["分类名称"].CString("").Trim();
                        name = PageValidate.InputText(name, 150);

                        Model.Product_category categoryOldModel = category.GetcategoryID(name);
                        if (categoryOldModel != null && !string.IsNullOrWhiteSpace(categoryOldModel.id))
                        {
                            msg += $"第{index}行,分类名称【{name}】已存在" + Environment.NewLine;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            continueCount++;
                            //连续3行空白结束
                            if (continueCount > 3)
                            {
                                break;
                            }
                            continue;
                        }

                        Model.Product_category addmodel = new Model.Product_category()
                        {
                            id = Guid.NewGuid().ToString(),
                            create_id = emp_id,
                            create_time = DateTime.Now,
                            product_category = name,
                            parentid = pid,
                            fparentid = fparentid,
                            product_icon = "",
                            cproperty = cproperty,
                            CodingBegins = "",
                        };
                        bool r = category.Add(addmodel);
                        if (r) { rs++; }
                        else {
                            msg += $"第{index}行,分类名称【{name}】添加失败" + Environment.NewLine;
                        }
                        continueCount = 0;
                    }
                    catch (Exception err)
                    {
                        SoftLog.LogStr(err, "ImportCategory");
                        msg += $"第{index}行,添加异常" + Environment.NewLine;
                    }

                    index++;
                }
                msg = $"成功加入{rs} 行数据 " + Environment.NewLine + msg;

            }
            catch (Exception error)
            {
                msg += "异常：" + error.ToString();
                SoftLog.LogStr(error, "ImportCategory");
            }

            return XhdResult.Success(msg).ToString();

        }

        /// <summary>
        /// 获取分类属性信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetCategoryInfo(string ID)
        {
            Model.Product_category model = category.GetModel(ID);

            return JsonDyamicHelper.NetJsonConvertObject(model);
        }
    }
}
