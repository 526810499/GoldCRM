using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using XHD.Common;
using XHD.Controller;

namespace XHD.Server
{
    /// <summary>
    /// 调拨
    /// </summary>
    public class Product_allot : BaseCRMServer
    {
        public static BLL.Product product = new BLL.Product();
        public static BLL.Product_allot allotBll = new BLL.Product_allot();
        public static BLL.Product_allotDetail allotDetailBll = new BLL.Product_allotDetail();

        public static Model.Product_allot model = new Model.Product_allot();

        private string authRightID = "38ED924E-E025-4FD0-87CB-D148EA2077A8";


        public Product_allot()
        {
        }

        public Product_allot(HttpContext context) : base(context)
        {
            allDataBtnid = "B07133BA-7063-42E9-9AF2-26005AA99FB2";
            depDataBtnid = "B33D7B1B-50A7-41EF-8D2D-AE3E9D91D19B";

        }

        public string save()
        {
            model.NowWarehouse = request["T_NowWarehouse_val"].CInt(0, false);
            model.Remark = PageValidate.InputText(request["T_Remark"], 255);
            model.update_id = emp_id;
            model.update_time = DateTime.Now;
            string id = PageValidate.InputText(request["id"], 50);
            string postData = request["postData"].CString("");
            bool isAdd = true;
            string msg = "";
            bool CanAdd = true;
            bool CanDel = true;
            try
            {
                List<Model.ProductAllot> list = JsonDyamicHelper.NetJsonConvertJson<List<Model.ProductAllot>>(postData);

                //新增的必须要有产品
                if ((list == null || list.Count <= 0) && string.IsNullOrWhiteSpace(id))
                {
                    return XhdResult.Error("产品信息为空,请确认后在操作！").ToString();
                }

                if (PageValidate.checkID(id, false))
                {
                    isAdd = false;
                    int status = request["auth"].CInt(0, false);
                    //需要判断是否有审核权限
                    if (status > 1)
                    {
                        if (CheckBtnAuthority(authRightID))
                        {
                            return XhdResult.Error("您没有该操作权限,请确认后在操作！").ToString();
                        }
                    }
                    model.status = status;
                    model.id = id;
                    DataSet ds = allotBll.GetList($" id= '{id}' ");
                    if (ds.Tables[0].Rows.Count == 0)
                        return XhdResult.Error("参数不正确，更新失败！").ToString();

                    DataRow dr = ds.Tables[0].Rows[0];
                    int dstatus = dr["status"].CInt(0, false);
                    //未提交的状态才能在添加
                    CanAdd = (dstatus == 0);
                    CanDel = (dstatus == 0);

                    allotBll.Update(model);

                    string UserID = emp_id;
                    string UserName = emp_name;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.Remark;
                    string EventType = "调拨单修改";
                    string EventID = model.id;

                    string Log_Content = null;


                    if (dr["NowWarehouse"].ToString() != request["T_NowWarehouse_val"])
                        Log_Content += string.Format("【{0}】{1} → {2} \n", "NowWarehouse", dr["NowWarehouse"],
                            request["T_NowWarehouse_val"]);

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
                    model.createdep_id = dep_id;
                    model.create_id = emp_id;
                    model.create_time = DateTime.Now;
                    id = "DB-" + DateTime.Now.ToString("yy-MM-dd-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                    model.id = id;
                    model.status = request["auth"].CInt(0, false) == 1 ? 1 : 0;

                    allotBll.Add(model);
                }

                //循环添加调拨单订单信息
                foreach (Model.ProductAllot m in list)
                {
                    if (m.__status == "add" && CanAdd)
                    {
                        int pstatus = product.GetPorductStatusByBarCode(m.BarCode);
                        //不是出库的不是销售的都可以调拨
                        if (pstatus == 3|| pstatus == 4)
                        {
                            msg += "\r\n 条形码【" + m.BarCode + "】";
                        }
                        else {

                            bool r = allotDetailBll.Add(new Model.Product_allotDetail()
                            {
                                id = Guid.NewGuid().ToString(),
                                allotid = id,
                                barcode = m.BarCode,
                                NowWarehouse = model.NowWarehouse,
                                FromWarehouse = m.warehouse_id.CInt(0, false),
                                create_id = emp_id,
                                create_time = DateTime.Now,
                                allotType = model.allotType
                            });
                            if (!r)
                            {
                                msg += "\r\n 条形码【" + m.BarCode + "】";
                            }
                        }
                    }
                    else if (m.__status == "delete" && CanDel)
                    {
                        //从调拨中删除调
                        allotDetailBll.Delete(id, m.BarCode);
                    }
                }

            }
            catch (Exception error)
            {
                msg = error.ToString();
                SoftLog.LogStr(error.ToString(), "SaveAllotid");
                return XhdResult.Error("添加失败,请确认是否重复添加后在操作！").ToString();
            }

            if (msg.Length > 0)
            {
                //添加提交审核的但是商品状态改变的修改状态
                if (model.status == 1 && isAdd)
                {
                    allotBll.Update(model);
                }
                return XhdResult.Success(msg+ "产品状态发生改变,请确认后在添加或提交审核").ToString();
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
            string serchtxt = $" 1=1 ";


            if (!string.IsNullOrEmpty(request["whid"]))
                serchtxt += $" and NowWarehouse={request["whid"].CInt(0, false)}";

            if (!string.IsNullOrEmpty(request["status"]))
                serchtxt += $" and status={request["status"].CInt(0, false)}";

            if (!string.IsNullOrEmpty(request["sorderid"]))
                serchtxt += $" and id='{PageValidate.InputText(request["sorderid"], 50)}'";

            if (!string.IsNullOrEmpty(request["scode"]))
            {
                string scode = request["scode"];
                DataSet dsc = allotDetailBll.GetList($" barcode='{scode}'");
                if (dsc == null || dsc.Tables.Count <= 0)
                {
                    return GetGridJSON.DataTableToJSON1(null, "0");
                }
                serchtxt += $" and id='{dsc.Tables[0].Rows[0]["allotid"]}'";
            }
            serchtxt = GetSQLCreateIDWhere(serchtxt, true);

            DataSet ds = allotBll.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);
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
                sortname = " create_time";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            string sorttext = " " + sortname + " " + sortorder;

            string Total;
            string serchtxt = $" 1=1 ";
            if (!string.IsNullOrEmpty(request["allotid"]) && request["allotid"] != "null")
            {
                serchtxt += $" and allotid='{PageValidate.InputText(request["allotid"], 50)}'";
            }
            else {
                return GetGridJSON.DataTableToJSON1(new DataTable(), "0");
            }

            if (!string.IsNullOrEmpty(request["FromWarehouse"]))
                serchtxt += $" and FromWarehouse={PageValidate.InputText(request["FromWarehouse"], 50)}";

            if (!string.IsNullOrEmpty(request["id"]))
                serchtxt += $" and id='{PageValidate.InputText(request["id"], 50)}'";

            DataSet ds = allotDetailBll.GetListProduct(PageSize, PageIndex, serchtxt, sorttext, out Total);
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
            DataSet ds = allotBll.GetList($" id= '{id}' ");
            return DataToJson.DataToJSON(ds);

        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Auth(string id)
        {
            if (CheckBtnAuthority(authRightID))
            {
                return XhdResult.Error("您没有该操作权限,请确认后在操作！").ToString();
            }
            id = PageValidate.InputText(request["id"], 50);
            string remark = PageValidate.InputText(request["remark"], 250);
            if (PageValidate.checkID(id, false))
            {
                int status = request["auth"].CInt(0, false);

                //审核不通过需要释放到锁库
                if (status != 2)
                {
                    status = 3;
                }
                bool r = allotBll.AuthApproved(id, emp_id, status, remark);
                if (r)
                {
                    return XhdResult.Success().ToString();
                }
                else {
                    return XhdResult.Error("审核处理失败,请确认该单下相应的产品是否发生状态改变").ToString();
                }
            }

            return "";
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
            DataSet ds = allotBll.GetList($" id= '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            int status = ds.Tables[0].Rows[0]["status"].CInt(0, false);

            //审核不通过不需要盘点
            if (status == 2)
            {
                return XhdResult.Error("此调拨单已审核通过，不允许删除！").ToString();
            }

            bool candel = true;
            if (uid != "admin")
            {

                candel = CheckBtnAuthority("0DAC81A8-7C15-4D77-A28F-C6C7468D6287");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = allotBll.Delete(id);
            if (!isdel) return XhdResult.Error("系统错误，删除失败！").ToString();

            //日志
            string EventType = "调拨单删除";

            string UserID = emp_id;
            string UserName = emp_name;
            string IPStreet = request.UserHostAddress;
            string EventID = id;
            string EventTitle = ds.Tables[0].Rows[0]["remark"].ToString();



            Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null);

            return XhdResult.Success().ToString();

        }



        /// <summary>
        /// 检查调度单ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string CheckAllotid(string id)
        {
            id = PageValidate.InputText(request["id"], 50);
            string remark = PageValidate.InputText(request["remark"], 250);
            if (PageValidate.checkID(id, false))
            {
                bool r = allotBll.CountPorduct(id) > 0;
                if (r)
                {
                    return XhdResult.Success().ToString();
                }
                else {
                    return XhdResult.Error("请确认调度是否正确,调度单下是否有产品").ToString();
                }
            }

            return XhdResult.Error("请确认调度是否正确").ToString();
        }
    }
}
