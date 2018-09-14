
using System;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using XHD.Common;
using XHD.Controller;
using System.IO;
using System.Text;

namespace XHD.Server
{


    /// <summary>
    /// 轮播播报
    /// </summary>
    public class STodayBroadcast : BaseCRMServer
    {
        BLL.BTodayBroadcast bll = new BLL.BTodayBroadcast();

        public STodayBroadcast()
        {
        }

        public STodayBroadcast(HttpContext context) : base(context)
        {
            allDataBtnid = "8270940A-23F3-42E7-B855-9D1F5276EA37";
            depDataBtnid = "45A1B8CC-3D6E-4520-A3DB-172AD3B617C6";

        }

        /// <summary>
        ///播报价格设置到cookie
        /// </summary>
        /// <param name="model"></param>
        private void SetCookiePrice(Model.TodayBroadcast model)
        {
            if (model == null) { return; }
            //价格设置到cookie里
            CookieHelper.Add("this_broadcast", $"{model.TodayGlodPrice},{model.TodayOtherPrice1},{model.TodayOtherPrice2},{model.TodayOtherPrice3}");
        }

        /// <summary>
        /// 获取今日轮播
        /// </summary>
        /// <returns></returns>
        public string GetTodayBroadcast()
        {
            Model.TodayBroadcast model = new Model.TodayBroadcast();
            model = bll.GetBannerTodayBroadcast();
            SetCookiePrice(model);
            return (JsonDyamicHelper.NetJsonConvertObject(model));
        }

        public string save()
        {
            Model.TodayBroadcast oldModel = new Model.TodayBroadcast();
            Model.TodayBroadcast model = new Model.TodayBroadcast();
            model.id = request["id"].CLong(0, false);
            model.Remark = PageValidate.InputText(request["T_Remark"], 50);
            model.OtherBrodcast = PageValidate.InputText(request["T_OtherBrodcast"], 2550).CString("");
            model.TodayGlodPrice = request["T_TodayGlodPrice"].CDecimal(0, false);
            model.TodayOtherPrice1 = request["T_TodayOtherPrice1"].CDecimal(0, false);
            model.TodayOtherPrice2 = request["T_TodayOtherPrice2"].CDecimal(0, false);
            model.TodayOtherPrice3 = request["T_TodayOtherPrice3"].CDecimal(0, false);
            model.TodayOtherPrice4 = request["T_TodayOtherPrice4"].CDecimal(0, false);
            model.createdep_id = dep_id;
            model.create_id = emp_id;
            model.create_time = DateTime.Now;
            model.update_id = emp_id;
            model.update_time = DateTime.Now;
            bool rs = false;
            if (model.id > 0)
            {
                oldModel = bll.GetTodayBroadcast(model.id);
                if (oldModel == null)
                {
                    return XhdResult.Error("信息为空,请确认后在操作！").ToString();
                }

                rs = bll.Update(model);

                string UserID = emp_id;
                string UserName = emp_name;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.Remark + "_" + model.TodayGlodPrice;
                string EventType = "今日播报修改";
                string EventID = model.id.CString("");

                string Log_Content = null;

                if (model.TodayGlodPrice != oldModel.TodayGlodPrice)
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "TodayGlodPrice", model.TodayGlodPrice, oldModel.TodayGlodPrice);

                if (model.TodayOtherPrice1 != oldModel.TodayOtherPrice1)
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "TodayOtherPrice1", model.TodayOtherPrice1, oldModel.TodayOtherPrice1);


                if (model.TodayOtherPrice2 != oldModel.TodayOtherPrice2)
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "TodayOtherPrice2", model.TodayOtherPrice2, oldModel.TodayOtherPrice2);


                if (model.TodayOtherPrice3 != oldModel.TodayOtherPrice3)
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "TodayOtherPrice3", model.TodayOtherPrice3, oldModel.TodayOtherPrice3);

                if (model.TodayOtherPrice4 != oldModel.TodayOtherPrice4)
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "TodayOtherPrice4", model.TodayOtherPrice4, oldModel.TodayOtherPrice4);

                if (model.Remark != oldModel.Remark)
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "Remark", model.Remark, oldModel.Remark);

                if (model.OtherBrodcast != oldModel.OtherBrodcast)
                    Log_Content += string.Format("【{0}】{1} → {2} \n", "OtherBrodcast", model.OtherBrodcast, oldModel.OtherBrodcast);

                if (!string.IsNullOrEmpty(Log_Content))
                    Syslog.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, Log_Content);

            }
            else {

                rs = bll.Add(model);

            }
            if (rs)
            {
                return XhdResult.Success().ToString();
            }
            else {
                return XhdResult.Error("保存操作失败").ToString();
            }

        }

        public string from()
        {
            Model.TodayBroadcast model = new Model.TodayBroadcast();
            model = bll.GetTodayBroadcast();
            if (model == null) { model = new Model.TodayBroadcast(); }
 
            return (JsonDyamicHelper.NetJsonConvertObject(model));
        }
    }
}
