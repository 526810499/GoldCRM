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
    /// 商品入库
    /// </summary>
    public class Product_StockIn : BaseCRMServer
    {
        public static BLL.Product product = new BLL.Product();
        public static BLL.Product_StockIn bll = new BLL.Product_StockIn();
        public static BLL.Product_StockInDetial detailBll = new BLL.Product_StockInDetial();

        public static Model.Product_StockIn model = new Model.Product_StockIn();

        public static BLL.Product_out outBll = new BLL.Product_out();


        public static BLL.Product_allot allotBll = new BLL.Product_allot();

        private string authRightID = "";
        private string delRightID = "";

        public Product_StockIn()
        {
        }

        public Product_StockIn(HttpContext context) : base(context)
        {
            if (context.Request["intype"].CInt(0, false) == 0)
            {
                allDataBtnid = "B07133BA-7063-42E9-9AF2-26005AA99FB2";
                depDataBtnid = "B33D7B1B-50A7-41EF-8D2D-AE3E9D91D19B";
                authRightID = "38ED924E-E025-4FD0-87CB-D148EA2077A8";
                delRightID = "0DAC81A8-7C15-4D77-A28F-C6C7468D6287";
            }
            else {
                allDataBtnid = "6C83A2E2-030D-478F-B148-F19DC0F63DE8";
                depDataBtnid = "123CA5A1-FDA7-400C-9075-23384A3BAD14";
                authRightID = "4C7415E5-70C1-453F-8E9D-11AF41709039";
                delRightID = "D191B668-BC22-4C77-B410-842D3AFC8352";
            }

        }

        /// <summary>
        /// 总部入库保存
        /// </summary>
        /// <returns></returns>
        public string HQSave()
        {

            model.remark = PageValidate.InputText(request["T_Remark"], 255);
            model.inType = request["inType"].CInt(0, false);
            string id = PageValidate.InputText(request["id"], 50);

            string msg = "";
            try
            {
                if (PageValidate.checkID(id, false))
                {
                    DataSet ds = bll.GetList($" id= '{id}' ");
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return XhdResult.Error("系统错误，无数据！").ToString();
                    }
                    model.status = 1;
                    model.id = id;
                    //  int status = ds.Tables[0].Rows[0]["status"].CInt(0, false);
                    //修改备注信息
                    bll.HQUpdateStock(model);
                    //修改临时入库的
                    bll.HQUpdateProductStockStatus(model.id);
                }

            }
            catch (Exception error)
            {
                msg = error.ToString();
                SoftLog.LogStr(error.ToString(), "Product_StockIn");
                return XhdResult.Error("添加失败,请确认是否重复添加后在操作！").ToString();
            }
            finally
            {
                if (model.status == 1)
                {
                    bool r = detailBll.UpdateProductWareHouse(model.id, model.warehouse_id, dep_id);
                    if (!r)
                    {
                        msg += "提交保存失败";
                    }
                }
            }

            if (msg.Length > 0)
            {
                return XhdResult.Success(msg + "<br/>入库保存失败,请确认后在操作！").ToString();
            }

            return XhdResult.Success().ToString();
        }

        public string save()
        {
            model.warehouse_id = request["T_Warehouse_val"].CInt(0, false);
            model.remark = PageValidate.InputText(request["T_Remark"], 255);
            model.inType = request["inType"].CInt(0, false);
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
                    ////需要判断是否有审核权限
                    //if (status > 1)
                    //{
                    //    if (CheckBtnAuthority(authRightID))
                    //    {
                    //        return XhdResult.Error("您没有该操作权限,请确认后在操作！").ToString();
                    //    }
                    //}
                    model.status = status;
                    model.id = id;
                    DataSet ds = bll.GetList($" id= '{id}' ");
                    if (ds.Tables[0].Rows.Count == 0)
                        return XhdResult.Error("参数不正确，更新失败！").ToString();

                    DataRow dr = ds.Tables[0].Rows[0];
                    int dstatus = dr["status"].CInt(0, false);
                    //未提交的状态才能在添加
                    CanAdd = (dstatus == 0);
                    CanDel = (dstatus == 0);

                    bll.Update(model);

                    string UserID = emp_id;
                    string UserName = emp_name;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.remark;
                    string EventType = "门店入库单修改";
                    string EventID = model.id;

                    string Log_Content = postData;


                    if (dr["warehouse_id"].ToString() != request["T_Warehouse_val"])
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "NowWarehouse", dr["warehouse_id"],
                            request["T_Warehouse_val"]);

                    if (dr["Remark"].ToString() != request["T_Remark"])
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "statusRemark", dr["Remark"],
                            request["T_Remark"]);
                    if (dr["status"].ToString() != model.status.CString(""))
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "status", dr["status"],
                          model.status.CString(""));

                    if (!string.IsNullOrEmpty(Log_Content))
                        Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);
                }
                else
                {
                    isAdd = false;
                    model.createdep_id = dep_id;
                    model.create_id = emp_id;
                    model.create_time = DateTime.Now;
                    id = "RK" + DateTime.Now.ToString("yyMMdd") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                    model.id = id;
                    model.status = request["auth"].CInt(0, false) == 1 ? 1 : 0;

                    bll.Add(model);
                }


                List<Model.ProductAllot> ld = list.Where(t => t.__status == "delete").ToList();
                List<Model.ProductAllot> ladd = list.Where(t => t.__status == "add").ToList();

                if (!CanAdd && ladd != null && ladd.Count >= 0)
                {
                    return XhdResult.Error("入库单状态已发生改变不能执行添加操作,请确认后在操作！").ToString();
                }

                if (!CanDel && ld != null && ld.Count >= 0)
                {
                    return XhdResult.Error("入库单状态已发生改变不能执行删除操作,请确认后在操作！").ToString();
                }

                //循环添加调拨单订单信息
                foreach (Model.ProductAllot m in list)
                {
                    m.BarCode = PageValidate.InputText(m.BarCode, 50);
                    if (m.__status == "add" && CanAdd)
                    {
                        int pstatus = product.GetPorductStatusByBarCode(m.BarCode);
                        //不是出库的不是销售的都可以调拨
                        if (pstatus == 4)
                        {
                            msg += "<br/> 条形码【" + m.BarCode + "】异常";
                        }
                        else {

                            bool r = detailBll.Add(new Model.Product_StockInDetial()
                            {
                                id = Guid.NewGuid().ToString(),
                                stockid = id,
                                barcode = m.BarCode,
                                warehouse_id = model.warehouse_id,
                                createdep_id = dep_id,
                                remark = "",
                                oldwarehouse_id = m.warehouse_id.CInt(0, false),
                            });
                            if (!r)
                            {
                                msg += "<br/>条形码【" + m.BarCode + "】";
                            }
                        }
                    }
                    else if (m.__status == "delete" && CanDel)
                    {
                        //从调拨中删除调
                        detailBll.Delete(id, model.id, m.BarCode);
                    }
                }

            }
            catch (Exception error)
            {
                msg = error.ToString();
                SoftLog.LogStr(error.ToString(), "Product_StockIn");
                return XhdResult.Error("添加失败,请确认是否重复添加后在操作！").ToString();
            }


            if (msg.Length > 0)
            {
                return XhdResult.Success(msg + "<br/>商品状态发生改变,请确认后在操作！").ToString();
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
            int inType = request["inType"].CInt(0, false);
            string Total;
            string serchtxt = $" inType=" + inType;


            if (!string.IsNullOrEmpty(request["whid"]))
                serchtxt += $" and warehouse_id={request["whid"].CInt(0, false)}";

            if (!string.IsNullOrEmpty(request["FromType"]))
                serchtxt += $" and FromType={request["FromType"].CInt(0, false)}";

            if (!string.IsNullOrEmpty(request["status"]))
                serchtxt += $" and status={request["status"].CInt(0, false)}";

            if (!string.IsNullOrEmpty(request["sorderid"]))
                serchtxt += $" and id='{PageValidate.InputText(request["sorderid"], 50)}'";

            if (!string.IsNullOrWhiteSpace(request["sbegtime"]))
            {
                serchtxt += $" and create_time>='{request["sbegtime"].CDateTime(DateTime.Now, false)}'";
            }
            if (!string.IsNullOrWhiteSpace(request["sendtime"]))
            {
                serchtxt += $" and create_time<='{request["sendtime"].CDateTime(DateTime.Now, false)}'";
            }

            if (!string.IsNullOrEmpty(request["scode"]))
            {
                string scode = PageValidate.InputText(request["scode"], 50);
                DataSet dsc = new DataSet();
                if (inType == 1)
                {
                    dsc = detailBll.GetList($" ptd.barcode='{scode}'");
                }
                else {
                    dsc = new BLL.Product().GetList($" barcode='{scode}'");
                }
                if (dsc == null || dsc.Tables[0].Rows.Count <= 0)
                {
                    return GetGridJSON.DataTableToJSON1(null, "0");
                }
                string ids = "";
                foreach (DataRow dr in dsc.Tables[0].Rows)
                {
                    ids += $"'{dr["stockid"]}',";
                }
                serchtxt += $" and id in({ids.Trim(',')})";
            }

            serchtxt = GetSQLCreateIDWhere(serchtxt, true);

            DataSet ds = bll.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
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
                sortname = " ptd.BarCode ";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["stockid"]) && request["stockid"] != "null")
            {
                serchtxt += $" and ptd.stockid='{PageValidate.InputText(request["stockid"], 50)}'";
            }
            else {
                return GetGridJSON.DataTableToJSON1(new DataTable(), "0");
            }

            if (!string.IsNullOrEmpty(request["warehouse_id"]))
                serchtxt += $" and warehouse_id={PageValidate.InputText(request["warehouse_id"], 50)}";

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
            DataSet ds = new DataSet();
            //添加临时单
            if (id == "addtemp")
            {
                Model.Product_StockIn model = new Model.Product_StockIn();

                model.createdep_id = dep_id;
                model.create_id = emp_id;
                model.create_time = DateTime.Now;
                model.inType = request["inType"].CInt(0, false);
                id = "RK" + DateTime.Now.ToString("yyMMdd") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                model.id = id;
                model.status = -1;
                model.remark = "";

                bll.Add(model);

                return JsonDyamicHelper.NetJsonConvertObject(model);
            }
            else {
                ds = bll.GetList($" id= '{id}' ");
            }
            return DataToJson.DataToJSON(ds);
        }

        /// <summary>
        /// 检查门店入库是否有未完成的
        /// </summary>
        /// <returns></returns>
        public string CheckHQAddOrder(int inType)
        {
            string orderid = bll.CheckHQAddOrder(emp_id, -1, inType);

            return orderid;
        }

        private bool OutOrderUpStatus(string OrderID, DataTable table, int Status, string Remark)
        {
            string FromOutID = table.Rows[0]["FromOutID"].CString("");
            int inType = table.Rows[0]["inType"].CInt(0, false);
            //入库来源
            int FromType = table.Rows[0]["FromType"].CInt(0, false);
            if (string.IsNullOrWhiteSpace(FromOutID)) { return true; }
            bool r = false;
            try
            {
                //总部入库
                if (FromType == 0)
                {
                    r = outBll.AuthApproved(1, FromOutID, emp_id, Status, Remark);
                }
                else {
                    //门店调拨
                    r = allotBll.AuthApproved(1, FromOutID, emp_id, Status, Remark, table.Rows[0]["createdep_id"].CString(dep_id));
                }
            }
            catch (Exception error)
            {
                SoftLog.LogStr(error.ToString(), "OutOrderUpStatus");
            }


            return r;
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
            if (!PageValidate.checkID(id, false))
            {
                return XhdResult.Error("参数有误,请确认后在操作！").ToString();
            }

            DataSet ds = bll.GetList($" id= '{id}' ");
            DataTable table = ds.Tables[0];
            string FromOutID = table.Rows[0]["FromOutID"].CString("");
            int status = request["auth"].CInt(0, false);

            //审核不通过需要释放到锁库
            if (status != 2)
            {
                status = 3;
            }
            try
            {
                //审核操作
                bool r = bll.DeepAuthSotockIN(id, emp_id, remark, status) > 0;

                if (string.IsNullOrWhiteSpace(FromOutID))
                {
                    r = detailBll.UpdateProductWareHouse(model.id, model.warehouse_id, dep_id);
                    if (!r)
                    {
                        bll.DeepAuthSotockIN(id, emp_id, remark + "[审核出错]", 1);
                        return XhdResult.Error("审核处理失败,请确认该单下相应的商品是否发生状态改变!!").ToString();
                    }
                }
                else {

                    r = OutOrderUpStatus(id, table, status, remark);
                    if (!r)
                    {
                        bll.DeepAuthSotockIN(id, emp_id, remark + "[审核出错]", 1);
                        return XhdResult.Error("审核出错,请重试").ToString();
                    }
                }
            }
            catch (Exception error)
            {
                bll.DeepAuthSotockIN(id, emp_id, remark, 1);
                SoftLog.LogStr(error.ToString(), "HQOutOrderUpStatusAuth");
                return XhdResult.Error("参数有误,请确认后在操作！").ToString();
            }
            return XhdResult.Success().ToString();
        }

        /// <summary>
        /// 删除临时单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string TempDel(string id)
        {
            if (!PageValidate.checkID(id, false)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = bll.GetList($" id= '{id}' and create_id='{emp_id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            int status = ds.Tables[0].Rows[0]["status"].CInt(0, false);

            //审核不通过不需要盘点
            if (status != -1)
            {
                return XhdResult.Error("此入库单状态已改变，不允许删除！").ToString();
            }

            bool isdel = bll.DeleteTemp(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();



            return XhdResult.Success().ToString();

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string del(string id)
        {
            if (!PageValidate.checkID(id, false)) return XhdResult.Error("参数错误！").ToString();
            id = PageValidate.InputText(id, 50);
            DataSet ds = bll.GetList($" id= '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            int status = ds.Tables[0].Rows[0]["status"].CInt(0, false);
            string FromOutID = ds.Tables[0].Rows[0]["FromOutID"].CString("");
            //审核不通过不需要盘点
            if (status == 2)
            {
                return XhdResult.Error("此入库单已审核通过，不允许删除！").ToString();
            }

            bool candel = true;
            if (uid != "admin")
            {

                candel = CheckBtnAuthority(delRightID);
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = bll.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //有来源的
            if (string.IsNullOrWhiteSpace(FromOutID))
            {
                outBll.Delete(FromOutID, 0);
            }

            //日志
            string EventType = "门店入库单删除";

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
