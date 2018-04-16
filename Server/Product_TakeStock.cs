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


        public Product_TakeStock()
        {
        }

        public Product_TakeStock(HttpContext context) : base(context)
        {
            allDataBtnid = "73D96F9F-E5E2-438B-B34A-411A0EA62288";
            depDataBtnid = "2AA4ABED-ECB2-4F49-B649-ADE2428907B8";
        }

        public string save()
        {
            model.warehouse_id = request["T_Warehouse_val"].CInt(0, false);
            model.remark = PageValidate.InputText(request["T_Remark"], 255);

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

                //新增的必须要有商品
                if ((list == null || list.Count <= 0) && string.IsNullOrWhiteSpace(id))
                {
                    return XhdResult.Error("商品信息为空,请确认后在操作！").ToString();
                }

                if (PageValidate.checkID(id, false))
                {

                }
                else {
                    id = "PD-" + DateTime.Now.ToString("yy-MM-dd-HH:mm-") + DateTime.Now.GetHashCode().ToString().Replace("-", "");
                    model.id = id;
                    model.status = request["auth"].CInt(0, false) == 1 ? 1 : 0;
                    model.create_id = emp_id;
                    model.create_time = DateTime.Now;
                    model.createdep_id = dep_id;
                    tBll.Add(model);
                }
                List<Model.ProductAllot> addList = list.Where(m => m.__status == "add").ToList();
                string where = "";
                foreach (Model.ProductAllot p in addList)
                {
                    where += "'" + PageValidate.InputText(p.BarCode, 20) + "',";
                }

                DataSet addDs = new DataSet();
                DataTable addTable = new DataTable();
                if (!string.IsNullOrWhiteSpace(where))
                {
                    where = where.Trim(',');
                    addDs = new BLL.Product().GetTakeList(where);
                    addTable = addDs.Tables[0];
                }

                //循环添加调拨单订单信息
                foreach (Model.ProductAllot m in list)
                {
                    if (m.__status == "add")
                    {

                    }
                    else if (m.__status == "delete")
                    {
                        if (model.status != 0)
                        {
                            msg = "已提交审核不能删除";
                        }
                        else {
                            detailBll.Delete(m.id, model.id);
                        }
                    }
                }

            }
            catch (Exception error)
            {
                msg = error.ToString();
                SoftLog.LogStr(error.ToString(), "SaveOut");
                return XhdResult.Error("添加失败,请确认是否重复添加后在操作！").ToString();
            }

            if (msg.Length > 0)
            {

                msg += "状态发生改变,请确认在添加或提交审核";
                return XhdResult.Success(msg).ToString();
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
            int warehouse_id = request["sck_val"].CInt(0, false);

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
                string scode = request["scode"];
                DataSet dsc = detailBll.GetList($" barcode='{scode}'");
                if (dsc == null || dsc.Tables.Count <= 0)
                {
                    return GetGridJSON.DataTableToJSON1(null, "0");
                }
                serchtxt += $" and id='{dsc.Tables[0].Rows[0]["takeid"]}'";
            }

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
                sortname = " create_time";
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
            DataSet ds = detailBll.GetList($" id= '{id}' ");
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
                bool r = false;
                if (r)
                {
                    return XhdResult.Success().ToString();
                }
                else {
                    return XhdResult.Error("审核处理失败,请确认该单下相应的商品是否发生状态改变").ToString();
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
            DataSet ds = tBll.GetList($" id= '{id}' ");
            if (ds.Tables[0].Rows.Count < 1)
                return XhdResult.Error("系统错误，无数据！").ToString();

            int status = ds.Tables[0].Rows[0]["status"].CInt(0, false);
            if (status == 2)
            {
                return XhdResult.Error("此出库单已审核通过，不允许删除！").ToString();
            }


            bool candel = true;
            if (uid != "admin")
            {

                candel = CheckBtnAuthority("540730D4-4046-41AC-A04F-46A9F2205C58");
                if (!candel)
                    return XhdResult.Error("无此权限！").ToString();
            }

            bool isdel = tBll.Delete(id);
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


    }
}
