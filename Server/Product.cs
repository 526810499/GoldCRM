using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    public class Product : BaseCRMServer
    {
        public static BLL.Product product = new BLL.Product();
        public static Model.Product model = new Model.Product();


        public Product()
        {
        }

        public Product(HttpContext context) : base(context)
        {
            if (context.Request["allstock"].CInt(0, false) == 1)
            {
                allDataBtnid = "9596C821-74CC-462A-BB32-B5B65FF47012";
                depDataBtnid = "35666543-AAD2-43CA-81A7-8A6E9314442C";
            }
            else {
                allDataBtnid = "1992FBF3-206A-4DE5-A1C4-1EB3A4F597D8";
                depDataBtnid = "ADE0DF75-65F1-4D8A-93C9-597D60140046";
            }
        }

        public string save()
        {
            model.category_id = PageValidate.InputText(request["T_product_category_val"], 50);
            model.product_name = PageValidate.InputText(request["T_product_name"], 255);
            model.StockPrice = request["T_StockPrice"].CDecimal(0, false);
            model.Weight = request["T_Weight"].CDecimal(0, false);
            model.AttCosts = request["T_AttCosts"].CDecimal(0, false);
            model.MainStoneWeight = request["T_MainStoneWeight"].CDecimal(0, false);
            model.AttStoneWeight = request["T_AttStoneWeight"].CDecimal(0, false);
            model.AttStoneNumber = request["T_AttStoneNumber"].CDecimal(0, false);
            model.StonePrice = request["T_StonePrice"].CDecimal(0, false);
            model.GoldTotal = request["T_GoldTotal"].CDecimal(0, false);
            model.CostsTotal = request["T_CostsTotal"].CDecimal(0, false);
            model.Totals = request["T_Totals"].CDecimal(0, false);
            model.indep_id = "7C881F36-3597-483B-BC71-EB5D7CFDA2C7";
            model.SalesTotalPrice = request["T_SalesTotalPrice"].CDecimal(0, false);
            model.SalesCostsTotal = request["T_SalesCostsTotal"].CDecimal(0, false);
            model.SupplierID = request["T_SupplierID_val"].CInt(0, false);
            model.Sbarcode = PageValidate.InputText(request["T_Sbarcode"], 255);
            model.remarks = PageValidate.InputText(request["T_Remark"], 255);
            model.IsGold = (request["T_GType"].CString("") == "是" ? 1 : 0);
            model.BarCode = PageValidate.InputText(request["T_BarCode"], 50);
            model.CertificateNo = PageValidate.InputText(request["T_CertificateNo"], 50);
            model.Circle = PageValidate.InputText(request["T_Circle"], 50);

            string pid = PageValidate.InputText(request["pid"], 50);
            if (PageValidate.checkID(pid))
            {
                bool candel = true;
                if (uid != "admin")
                {
                    var getauth = new GetAuthorityByUid();
                    candel = getauth.GetBtnAuthority(emp_id.ToString(), "4AF81556-E83B-4305-A1F2-6863A59C29F0");
                    if (!candel)
                        return XhdResult.Error("无此权限！").ToString();
                }

                model.id = pid;
                DataSet ds = product.GetList($" id= '{pid}' ");
                if (ds.Tables[0].Rows.Count == 0)
                    return XhdResult.Error("参数不正确，更新失败！").ToString();

                DataRow dr = ds.Tables[0].Rows[0];
                int status = dr["status"].CInt(0, false);
                int authIn = dr["authIn"].CInt(0, false);
                string oldCode = dr["BarCode"].CString("");
                if (oldCode != model.BarCode.Trim().ToUpper())
                {
                    if (status != 1)
                    {
                        return XhdResult.Error("该商品状态已发生改变,条形码不能修改").ToString();
                    }
                    else if (authIn != 0)
                    {
                        return XhdResult.Error("该商品处于审核状态,条形码不能修改").ToString();
                    }
                    else {
                        string id = product.GetProductIdByCode(model.BarCode);
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            return XhdResult.Error("该条形码已存在,请更换").ToString();
                        }
                    }
                }
                model.BarCode = model.BarCode.Trim().ToUpper();
                product.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.product_name;
                string EventType = "商品修改";
                string EventID = model.id;

                string Log_Content = null;

                if (dr["category_name"].ToString() != request["T_product_category"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "商品类别", dr["category_name"],
                        request["T_product_category"]);

                if (dr["product_name"].ToString() != request["T_product_name"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "商品名字", dr["product_name"],
                        request["T_product_name"]);

                if (dr["StockPrice"].ToString() != request["T_StockPrice"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "进货金价", dr["StockPrice"],
                        request["T_StockPrice"]);

                if (dr["Weight"].ToString() != request["T_Weight"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "重量", dr["Weight"], request["T_Weight"]);

                if (dr["remarks"].ToString() != request["T_Remark"])
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "备注", dr["remarks"], request["T_Remark"]);

                if (decimal.Parse(dr["AttCosts"].ToString()) != decimal.Parse(request["T_AttCosts"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "附工费", dr["AttCosts"], request["T_AttCosts"]);

                if (decimal.Parse(dr["MainStoneWeight"].ToString()) != decimal.Parse(request["T_MainStoneWeight"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "MainStoneWeight", dr["MainStoneWeight"], request["T_MainStoneWeight"]);

                if (decimal.Parse(dr["AttStoneWeight"].ToString()) != decimal.Parse(request["T_AttStoneWeight"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "AttStoneWeight", dr["AttStoneWeight"], request["T_AttStoneWeight"]);

                if (decimal.Parse(dr["AttStoneNumber"].ToString()) != decimal.Parse(request["T_AttStoneNumber"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "AttStoneNumber", dr["AttStoneNumber"], request["T_AttStoneNumber"]);

                if (decimal.Parse(dr["StonePrice"].ToString()) != decimal.Parse(request["T_StonePrice"]))
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "StonePrice", dr["StonePrice"], request["T_StonePrice"]);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
            }
            else
            {
                bool candel = true;
                if (uid != "admin")
                {
                    var getauth = new GetAuthorityByUid();
                    candel = getauth.GetBtnAuthority(emp_id.ToString(), "3FD3566B-B807-452A-B3B3-00A709CCAF229");
                    if (!candel)
                        return XhdResult.Error("无此权限！").ToString();
                }


                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                model.id = Guid.NewGuid().ToString();
                model.createdep_id = dep_id;
                model.status = 1;

                try
                {
                    model.BarCode = GetBarCode(model.category_id);
                    product.Add(model);
                }
                catch (Exception error)
                {
                    if (error.Message.IndexOf("不能在具有唯一索引") > -1)
                    {
                        int t = 3;
                        while (t > 0)
                        {
                            try
                            {
                                model.BarCode = GetBarCode(model.category_id);
                                bool r = product.Add(model);
                                if (r) { t = -1; break; }
                                t++;
                            }
                            catch (Exception err1)
                            {
                                SoftLog.LogStr(err1, "Product");
                                return XhdResult.Error("添加失败,请重新添加！").ToString();
                            }
                        }
                    }
                    else {
                        return XhdResult.Error("添加失败,请重新添加！").ToString();
                    }
                }
            }

            return XhdResult.Success().ToString();
        }

        /// <summary>
        /// 获取条形码
        /// </summary>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        private string GetBarCode(string categoryid)
        {
            string code = DateTime.Now.GetHashCode().ToString().Replace("-", "");
            BLL.Product_category cbll = new BLL.Product_category();
            string CodingBegin = "Y";
            DataSet cds = cbll.GetList("id='" + model.category_id + "'");
            if (cds != null && cds.Tables.Count > 0)
            {
                CodingBegin = cds.Tables[0].Rows[0]["CodingBegins"].CString("Y");
            }
            int counts = cbll.GetCategoryCounts(CodingBegin);
            code = string.Format("{0}{1}{2}", CodingBegin, DateTime.Now.Year, counts.CString("1").PadLeft(5, '0'));//.PadRight(14, '0')

            return code.ToUpper();
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (sortname == "category_name")
            {
                sortname = "category_id";
            }
            if (sortname == "supplier_name")
            {
                sortname = "SupplierID";
            }
            if (sortname == "warehouse_name")
            {
                sortname = "warehouse_id";
            }
            if (sortname == "indep_name")
            {
                sortname = "indep_id";
            }
            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total = "0";
            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["categoryid"]))
                serchtxt += $" and category_id='{PageValidate.InputText(request["categoryid"], 50)}'";

            if (!string.IsNullOrEmpty(request["stext"]))
                serchtxt += $" and product_name like N'%{ PageValidate.InputText(request["stext"], 50) }%'";

            if (!string.IsNullOrEmpty(request["scode"]))
                serchtxt += $" and BarCode like N'{ PageValidate.InputText(request["scode"], 50) }%'";

            if (!string.IsNullOrEmpty(request["status"]))
                serchtxt += $" and status in({request["status"].CString("1").Trim(',')}) ";


            if (!string.IsNullOrEmpty(request["soutstatus"]))
                serchtxt += $" and outStatus={request["soutstatus"].CInt(0, false)} ";

            if (!string.IsNullOrEmpty(request["SupplierID"]) && request["SupplierID"].CString("") != "null")
                serchtxt += $" and SupplierID='{request["SupplierID"].CString("")}'";
            int wid = request["warehouse_id"].CInt(-1, false);
            if (!string.IsNullOrEmpty(request["warehouse_id"]) && wid > -1)
            {
                serchtxt += $" and warehouse_id='{request["warehouse_id"].CString("")}'";
            }

            if (!string.IsNullOrEmpty(request["sbegtime"]))
                serchtxt += " and create_time>='" + PageValidate.InputText(request["sbegtime"], 50).CDateTime(DateTime.Now, false) + "'";

            if (!string.IsNullOrEmpty(request["sendtime"]))
                serchtxt += " and create_time<='" + PageValidate.InputText(request["sendtime"], 50).CDateTime(DateTime.Now, false) + "'";


            if (!string.IsNullOrWhiteSpace(request["sindep_id"]))
            {
                serchtxt += " and indep_id='" + PageValidate.InputText(request["sindep_id"], 50) + "'";
            }

            //操作类型 
            string optype = request["optype"].CString("");

            //门店入库则状态不能是总部入库或者是已销售的
            if (optype == "mdrk")
            {
                serchtxt += $" and status not in(1,4) and outStatus<>1   and indep_id='{ dep_id }' ";
            }

            //门店出库则状态不能为总部操作状态，取不能是已销售的
            if (optype == "mdck")
            {
                serchtxt += $" and status not in(1,2,3,4) and outStatus<>3  and indep_id='{ dep_id }' ";
            }

            //门店调拨 状态不能为总部操作状态，取不能是已销售的
            if (optype == "mddb")
            {
                //门店调拨需要判断跨门店权限
                //string depids = TransDepartmentID();

                serchtxt += $" and status not in(1,2,3,4) and outStatus<>2   ";// and indep_id in({ depids })
            }

            //是否要取门店的
            if (string.IsNullOrWhiteSpace(optype))
            {
                if (request["depdata"].CInt(0, false) == 1)
                {
                    serchtxt += " and indep_id='" + dep_id + "'";
                }
                else {
                    //权限
                    serchtxt = GetSQLCreateIDWhere(serchtxt, true);
                }
            }


            if (request["sum"].CInt(0, false) == 1)
            {
                DataTable totalTable = new DataTable();
                DataSet ds = product.GetList(PageSize, PageIndex, serchtxt, sorttext, out totalTable);
                foreach (DataRow row in totalTable.Rows)
                {
                    Total = row["counts"].CString("0");
                }
                return GetGridJSON.DataTableToJSON(ds.Tables[0], Total, totalTable);//
            }
            else {

                DataSet ds = product.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
                return GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);//, totalTable

            }
        }



        public string form(string id)
        {
            if (!PageValidate.checkID(id)) return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = product.GetList($" id= '{id}' ");
            return DataToJson.DataToJSON(ds);

        }


        //del
        public string del(string id)
        {
            if (!PageValidate.checkID(id)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = product.GetList($" id= '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            var ccod = new BLL.Sale_order_details();
            if (ccod.GetList($"product_id = '{id}'").Tables[0].Rows.Count > 0)
                return XhdResult.Error("此商品下含有订单，不允许删除！").ToString();
            int status = ds.Tables[0].Rows[0]["status"].CInt(1, false);

            if (status != 1)
            {
                return XhdResult.Error("此商品当前非入库状态，不允许删除！").ToString();
            }

            bool candel = true;
            if (uid != "admin")
            {
                //controll auth
                var getauth = new GetAuthorityByUid();
                candel = getauth.GetBtnAuthority(emp_id.ToString(), "C55A626D-1A32-4EDF-832A-19DA1C2A4569");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = product.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志
            string EventType = "商品删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["product_name"].ToString();



            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }


        #region 打印商品条形码
        /// <summary>
        /// 导出商品条形码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string ExportPrint()
        {
            bool candel = CheckIsAdmin();

            if (!candel)
            {
                candel = CheckBtnAuthority("0DBE7337-D4D3-4895-8C8F-5E0A4ECEEE0A");
            }
            if (!candel)
            {
                return null;
            }


            string ids = System.Web.HttpContext.Current.Request["ids"];
            ids = PageValidate.InputText(ids, 50000);
            if (string.IsNullOrWhiteSpace(ids)) { return null; }
            ids = "'" + ids.Trim(',') + "'";
            ids = ids.Replace(",", "','");

            string where = $" id in({ids})";
            DataSet ds = product.GetList(where);
            if (ds != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                return JsonDyamicHelper.NetJsonConvertObject(ds.Tables[0]);
            }
            else {
                return null;
            }
        }

        #endregion



    }
}
