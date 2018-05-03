using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    /// <summary>
    /// 商品盘点
    /// </summary>
    public class Product_TakeStock : BaseCRMServer
    {
        public static BLL.Product_TakeStock tBll = new BLL.Product_TakeStock();

        public static BLL.Product_TakeStockDetail detailBll = new BLL.Product_TakeStockDetail();

        public static Model.Product_TakeStock model = new Model.Product_TakeStock();
        private string authRightID = "6A0BE2D0-AAB9-472D-8016-D14C9E90F6A8";
        private string delRightID = "6A0BE2D0-AAB9-472D-8016-D14C9E90F6A8";

        public Product_TakeStock()
        {
        }

        public Product_TakeStock(HttpContext context) : base(context)
        {
            if (context.Request["taketype"].CInt(0, false) == 0)
            {
                allDataBtnid = "73D96F9F-E5E2-438B-B34A-411A0EA62288";
                depDataBtnid = "2AA4ABED-ECB2-4F49-B649-ADE2428907B8";
                delRightID = "540730D4-4046-41AC-A04F-46A9F2205C58";
                authRightID = "6A0BE2D0-AAB9-472D-8016-D14C9E90F6A8";
            }
            else {
                delRightID = "B9FCB203-4B46-42EC-9DFC-5704574E8A8C";
                authRightID = "202C1841-BB0E-4591-8C53-78875647E4E2";
                depDataBtnid = "105EBC81-D637-4E9F-8300-0841D8FEF20F";
                allDataBtnid = "245ED03C-56A4-41D9-A502-544CE38B31AC";

            }
        }

        public string save()
        {
            model.warehouse_id = request["T_Warehouse_val"].CInt(0, false);
            model.remark = PageValidate.InputText(request["T_Remark"], 255);

            model.update_id = emp_id;
            model.update_time = DateTime.Now;
            model.takeType = request["takeType"].CInt(0, false);
            string id = PageValidate.InputText(request["id"], 50);
            string postData = request["postData"].CString("");
            bool isAdd = true;
            string msg = "";
            bool CanAdd = true;
            bool CanDel = true;
            try
            {
                List<Model.ProductAllot> list = JsonDyamicHelper.NetJsonConvertJson<List<Model.ProductAllot>>(postData);

                //新增的必须要有商品
                if ((list == null || list.Count <= 0) && string.IsNullOrWhiteSpace(id))
                {
                    return XhdResult.Error("商品信息为空,请确认后在操作！").ToString();
                }

                if (PageValidate.checkID(id, false))
                {
                    isAdd = false;
                    int status = request["auth"].CInt(0, false);
                    //需要判断是否有审核权限
                    if (status > 1)
                    {
                        if (!CheckIsAdmin() && !CheckBtnAuthority(authRightID))
                        {
                            return XhdResult.Error("您没有该操作权限,请确认后在操作！").ToString();
                        }
                    }
                    model.status = status;
                    model.id = id;
                    DataSet ds = tBll.GetList($" id= '{id}' ");
                    if (ds.Tables[0].Rows.Count == 0)
                        return XhdResult.Error("参数不正确，更新失败！").ToString();

                    DataRow dr = ds.Tables[0].Rows[0];
                    int dstatus = dr["status"].CInt(0, false);
                    //未提交的状态才能在添加
                    CanAdd = (dstatus == 0);
                    CanDel = (dstatus == 0);

                    tBll.Update(model);

                    string UserID = emp_id;
                    string UserName = emp_name;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = "库存盘点";
                    string EventType = "库存盘点";
                    string EventID = model.id;

                    string Log_Content = postData;

                    if (dr["warehouse_id"].ToString() != request["T_Warehouse_val"])
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "NowWarehouse", dr["warehouse_id"],
                            request["T_Warehouse_val"]);

                    if (dr["remark"].ToString() != request["T_Remark"])
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "remark", dr["remark"],
                            request["T_Remark"]);
                    if (dr["status"].ToString() != model.status.CString(""))
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "status", dr["status"],
                          model.status.CString(""));


                    if (!string.IsNullOrEmpty(Log_Content))
                        Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
                }
                else {
                    isAdd = false;
                    id = "PD" + DateTime.Now.ToString("yyMMdd") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                    model.id = id;
                    model.status = request["auth"].CInt(0, false) == 1 ? 1 : 0;
                    model.create_id = emp_id;
                    model.create_time = DateTime.Now;
                    model.createdep_id = dep_id;
                    model.authuser_id = "";
                    model.authuser_time = DateTime.Now;
                    tBll.Add(model);
                }

                List<Model.ProductAllot> ld = list.Where(t => t.__status == "delete").ToList();
                List<Model.ProductAllot> ladd = list.Where(t => t.__status == "add").ToList();

                if (!CanAdd && ladd != null && ladd.Count >= 0)
                {
                    return XhdResult.Error("盘点单状态已发生改变不能执行添加操作,请确认后在操作！").ToString();
                }

                if (!CanDel && ld != null && ld.Count >= 0)
                {
                    return XhdResult.Error("盘点单状态已发生改变不能执行删除操作,请确认后在操作！").ToString();
                }
                BLL.Product pbll = new BLL.Product();
                //循环添加调拨单订单信息
                foreach (Model.ProductAllot m in list)
                {
                    m.BarCode = PageValidate.InputText(m.BarCode, 50);
                    if (m.__status == "add")
                    {
                        //如果是正常的需要在确认下，避免被客户端篡改上传
                        if (m.status == 1)
                        {
                            DataSet pds = pbll.GetList($" barcode='{m.BarCode}'");
                            if (pds.Tables[0].Rows.Count <= 0)
                            {
                                msg += "<br/>条形码【" + m.BarCode + "】条形码异常未添加";
                                continue;
                            }
                        }

                        bool r = detailBll.Add(new Model.Product_TakeStockDetail()
                        {
                            id = Guid.NewGuid().ToString(),
                            takeid = model.id,
                            barcode = m.BarCode,
                            status = m.status,
                            taketime = DateTime.Now,
                            warehouse_id = model.warehouse_id,
                            remark = m.remark.CString(""),
                            createdep_id = model.createdep_id,
                        });
                        if (!r)
                        {
                            msg += "<br/>条形码【" + m.BarCode + "】条形添加失败";
                        }
                    }
                    else if (m.__status == "delete")
                    {
                        bool r = detailBll.Delete(m.id, model.id);
                    }
                }
            }
            catch (Exception error)
            {
                msg = error.ToString();
                SoftLog.LogStr(error.ToString(), "Product_TakeStock");
                return XhdResult.Error("添加失败,请确认是否重复添加后在操作！").ToString();
            }


            if (msg.Length > 0)
            {
                //添加提交审核的但是商品状态改变的修改状态
                if (model.status == 1 && isAdd)
                {
                    model.status = 0;
                    tBll.Update(model);
                }
                msg += "状态发生改变,请确认在添加后在提交审核";
            }
            else {
                //状态修改为提交审核的需要在生成下清算
                if (model.status == 1)
                {
                    SaveProductClearingTake(model.id, model.warehouse_id, model.createdep_id);
                }
            }

            return XhdResult.Success().ToString();
        }

        public string grid()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            int takeType = request["taketype"].CInt(0, false);
            string serchtxt = $" taketype=" + takeType;
            int warehouse_id = request["swarehouse_id"].CInt(0, false);

            if (warehouse_id > 0)
            {
                serchtxt += $" and warehouse_id={warehouse_id}";
            }
            if (!string.IsNullOrEmpty(request["status"]))
                serchtxt += $" and status={request["status"].CInt(0, false)}";

            if (!string.IsNullOrEmpty(request["sorderid"]))
                serchtxt += $" and id='{PageValidate.InputText(request["sorderid"], 50)}'";

            if (!string.IsNullOrEmpty(request["scode"]))
            {
                string scode = PageValidate.InputText(request["scode"], 50);
                DataSet dsc = detailBll.GetList($" barcode='{scode}'");
                if (dsc == null || dsc.Tables[0].Rows.Count <= 0)
                {
                    return GetGridJSON.DataTableToJSON1(null, "0");
                }
                string ids = "";
                foreach (DataRow dr in dsc.Tables[0].Rows)
                {
                    ids += $"'{dr["takeid"]}',";
                }
                serchtxt += $" and id in({ids.Trim(',')})";
            }

            if (!string.IsNullOrEmpty(request["sbegtime"]))
                serchtxt += $" and create_time>='{request["sbegtime"].CDateTime(DateTime.Now, false)}'";

            if (!string.IsNullOrEmpty(request["sendtime"]))
                serchtxt += $" and create_time<='{request["sendtime"].CDateTime(DateTime.Now, false)}'";


            serchtxt = GetSQLCreateIDWhere(serchtxt, true);

            DataSet ds = tBll.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }

        /// <summary>
        /// 调拨详单
        /// </summary>
        /// <returns></returns>
        public string gridDetail()
        {
            int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
            int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
            string sortname = request["sortname"];
            string sortorder = request["sortorder"];

            if (string.IsNullOrEmpty(sortname))
                sortname = " taketime ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["takeid"]) && request["takeid"] != "null")
            {
                serchtxt += $" and takeid='{PageValidate.InputText(request["takeid"], 50)}'";
            }
            else {
                return GetGridJSON.DataTableToJSON1(new DataTable(), "0");
            }


            if (!string.IsNullOrEmpty(request["id"]))
                serchtxt += $" and id='{PageValidate.InputText(request["id"], 50)}'";

            DataSet ds = detailBll.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
            string dt = GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
            return (dt);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string form(string id)
        {
            if (!PageValidate.checkID(id, false) || id == "null") return "{}";
            id = PageValidate.InputText(id, 50);
            DataSet ds = tBll.GetList($" id= '{id}' ");
            return DataToJson.DataToJSON(ds);

        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Auth(string id)
        {
            if (!CheckIsAdmin() && !CheckBtnAuthority(authRightID))
            {
                return XhdResult.Error("您没有该操作权限,请确认后在操作！").ToString();
            }
            id = PageValidate.InputText(request["id"], 50);
            string remark = PageValidate.InputText(request["remark"], 250);

            if (PageValidate.checkID(id, false))
            {
                int status = request["auth"].CInt(0, false);
                if (status != 2)
                {
                    status = 3;
                }
                bool r = tBll.Auth(new Model.Product_TakeStock() { id = id, remark = remark, status = status, authuser_id = emp_id }); ;
                if (r)
                {
                    return XhdResult.Success().ToString();
                }
                else {
                    return XhdResult.Error("审核处理失败").ToString();
                }
            }

            return XhdResult.Error("审核处理异常").ToString();
        }

        /// <summary>
        /// 盘点清算，清算没有录入的
        /// </summary>
        /// <param name="takeid"></param>
        /// <param name="warehouse_id"></param>
        /// <returns></returns>
        public string ProductClearingTake(string takeid)
        {
            return SaveProductClearingTake(takeid, 0, dep_id);
        }

        /// <summary>
        /// 盘点清算，清算没有录入的
        /// </summary>
        /// <param name="takeid"></param>
        /// <param name="warehouse_id"></param>
        /// <returns></returns>
        public string SaveProductClearingTake(string takeid, int warehouse_id, string createdep_id)
        {
            DataSet ds = tBll.GetList($" id= '{PageValidate.InputText(takeid, 50)}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            if (string.IsNullOrWhiteSpace(takeid))
            {
                return XhdResult.Error("系统错误，无数据！").ToString();
            }
            int result = tBll.ProductClearingTake(takeid, warehouse_id, createdep_id);

            return XhdResult.Success(result.CString("")).ToString();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string del(string id)
        {
            bool candel = true;
            if (uid != "admin")
            {

                candel = CheckBtnAuthority(delRightID);
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            if (!PageValidate.checkID(id, false)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = tBll.GetList($" id= '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            int status = ds.Tables[0].Rows[0]["status"].CInt(0, false);
            if (status == 2)
            {
                return XhdResult.Error("盘点单已审核通过，不允许删除！").ToString();
            }
 
            bool isdel = tBll.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志
            string EventType = "盘点单删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["remark"].ToString();

            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }


    }
}
