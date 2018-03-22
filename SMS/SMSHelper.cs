using System;

namespace XHD.SMS
{
    public class SMSHelper
    {
        public SMSHelper(){}

        SMS_Server.SDKService yimei = new SMS_Server.SDKService();
        /// <summary>
        /// 3.1序列号注册
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <param name="serialpass"></param>
        /// <returns></returns>
        public int registEx(string softwareSerialNo, string key, string serialpass)
        {
            return yimei.registEx(softwareSerialNo, key, serialpass);
        }
        /// <summary>
        /// 3.2	注册企业信息
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="serialPwd"></param>
        /// <param name="eName"></param>
        /// <param name="linkMan"></param>
        /// <param name="phoneNum"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="fax"></param>
        /// <param name="address"></param>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public int registDetailInfo(string softwareSerialNo, string serialPwd, string eName, string linkMan, string phoneNum, string mobile, string email, string fax, string address, string postcode)
        {
            return yimei.registDetailInfo(softwareSerialNo, serialPwd, eName, linkMan, phoneNum, mobile, email, fax, address, postcode);
        }

        /// <summary>
        /// 3.3	注销序列号
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public int logout(string softwareSerialNo, string key)
        {
            return yimei.logout(softwareSerialNo, key);
        }

        /// <summary>
        /// 3.4	查询单价
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public double getEachFee(string softwareSerialNo, string key)
        {
            return yimei.getEachFee(softwareSerialNo, key);
        }

        /// <summary>
        /// 3.5	查询余额
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public double getBalance(string softwareSerialNo, string key)
        {
            return yimei.getBalance(softwareSerialNo, key);
        }

        /// <summary>
        /// 3.6	序列号充值
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <param name="cardNo"></param>
        /// <param name="cardPass"></param>
        /// <returns></returns>
        public int chargeUp(string softwareSerialNo, string key, string cardNo, string cardPass)
        {
            return yimei.chargeUp(softwareSerialNo, key, cardNo, cardPass);
        }

        /// <summary>
        /// 3.7	发送短信
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <param name="mobiles"></param>
        /// <param name="smsContent"></param>
        /// <param name="addSerial"></param>
        /// <returns></returns>
        public int sendSMS(string softwareSerialNo, string key, string[] mobiles, string smsContent, int smsID)
        {
            return sendSMS(softwareSerialNo, key, null, mobiles, smsContent, null, "GBK", 3, Convert.ToInt64(smsID.ToString()));
        }
        /// <summary>
        /// 3.7	发送短信
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <param name="mobiles"></param>
        /// <param name="smsContent"></param>
        /// <param name="addSerial"></param>
        /// <returns></returns>
        public int sendSMS(string softwareSerialNo, string key, string[] mobiles, string smsContent, long smsID)
        {
            return sendSMS(softwareSerialNo, key, null, mobiles, smsContent, null, "GBK", 3, smsID);
        }
        /// <summary>
        /// 3.7	发送短信
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <param name="sendTime"></param>
        /// <param name="mobiles"></param>
        /// <param name="smsContent"></param>
        /// <param name="addSerial"></param>
        /// <param name="srcCharset"></param>
        /// <param name="smsPriority"></param>
        /// <param name="smsID"></param>
        /// <returns></returns>
        public int sendSMS(string softwareSerialNo, string key, string sendTime, string[] mobiles, string smsContent, string addSerial, string srcCharset, int smsPriority, long smsID)
        {
            return yimei.sendSMS(softwareSerialNo, key, sendTime, mobiles, smsContent, addSerial, srcCharset, smsPriority, smsID);
        }

        

        /// <summary>
        /// 3.8	发送语音验证码
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <param name="sendTime"></param>
        /// <param name="mobiles"></param>
        /// <param name="checkCode"></param>
        /// <param name="addSerial"></param>
        /// <param name="srcCharset"></param>
        /// <param name="smsPriority"></param>
        /// <param name="smsID"></param>
        /// <returns></returns>
        public string sendVoice(string softwareSerialNo, string key, string sendTime, string[] mobiles, string checkCode, string addSerial, string srcCharset, int smsPriority, long smsID)
        {
            return yimei.sendVoice(softwareSerialNo, key, sendTime, mobiles, checkCode, addSerial, srcCharset, smsPriority, smsID);
        }

        /// <summary>
        /// 3.9	接收短信
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public SMS_Server.mo[] getMO(string softwareSerialNo, string key)
        {
            return yimei.getMO(softwareSerialNo, key);
        }

        /// <summary>
        /// 接收短信或语音验证码状态报告
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public SMS_Server.statusReport[] getReport(string softwareSerialNo, string key)
        {
            return yimei.getReport(softwareSerialNo, key);
        }

        /// <summary>
        /// 3.11	修改密码
        /// </summary>
        /// <param name="softwareSerialNo"></param>
        /// <param name="key"></param>
        /// <param name="serialPwd"></param>
        /// <param name="serialPwdNew"></param>
        /// <returns></returns>
        public int serialPwdUpd(string softwareSerialNo, string key, string serialPwd, string serialPwdNew)
        {
            return yimei.serialPwdUpd(softwareSerialNo, key, serialPwd, serialPwdNew);
        }
        
        
        /// <summary>
        /// 状态码
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string sms_result(int status)
        {
            string returnTxt = "";
            switch (status)
            {
                case 0: returnTxt = "成功"; break;
                case -1: returnTxt = "系统异常"; break;
                case -2: returnTxt = "客户端异常"; break;
                case -101: returnTxt = "命令不被支持"; break;
                case -102: returnTxt = "RegistryTransInfo删除信息失败"; break;
                case -103: returnTxt = "RegistryInfo更新信息失败"; break;
                case -104: returnTxt = "请求超过限制"; break;
                case -110: returnTxt = "号码注册激活失败"; break;
                case -111: returnTxt = "企业注册失败"; break;
                case -113: returnTxt = "充值失败"; break;
                case -117: returnTxt = "发送短信失败"; break;
                case -118: returnTxt = "接收MO失败"; break;
                case -119: returnTxt = "接收Report失败"; break;
                case -120: returnTxt = "修改密码失败"; break;
                case -122: returnTxt = "号码注销激活失败"; break;
                case -123: returnTxt = "查询单价失败"; break;
                case -124: returnTxt = "查询余额失败"; break;
                case -125: returnTxt = "设置MO转发失败"; break;
                case -126: returnTxt = "路由信息失败"; break;
                case -127: returnTxt = "计费失败0余额"; break;
                case -128: returnTxt = "计费失败余额不足"; break;
                case -190: returnTxt = "数据操作失败"; break;
                case -1100: returnTxt = "序列号错误,序列号不存在内存中,或尝试攻击的用户"; break;
                case -1102: returnTxt = "序列号密码错误"; break;
                case -1103: returnTxt = "序列号Key错"; break;
                case -1104: returnTxt = "路由失败，请联系系统管理员"; break;
                case -1105: returnTxt = "注册号状态异常, 未用 1"; break;
                case -1107: returnTxt = "注册号状态异常, 停用 3"; break;
                case -1108: returnTxt = "注册号状态异常, 停止 5"; break;
                case -1131: returnTxt = "充值卡无效"; break;
                case -1132: returnTxt = "充值密码无效"; break;
                case -1133: returnTxt = "充值卡绑定异常"; break;
                case -1134: returnTxt = "充值状态无效"; break;
                case -1135: returnTxt = "充值金额无效"; break;
                case -1901: returnTxt = "数据库插入操作失败"; break;
                case -1902: returnTxt = "数据库更新操作失败"; break;
                case -1903: returnTxt = "数据库删除操作失败"; break;
                case -9000: returnTxt = "数据格式错误,数据超出数据库允许范围"; break;
                case -9001: returnTxt = "序列号格式错误"; break;
                case -9002: returnTxt = "密码格式错误"; break;
                case -9003: returnTxt = "客户端Key格式错误"; break;
                case -9004: returnTxt = "设置转发格式错误"; break;
                case -9005: returnTxt = "公司地址格式错误"; break;
                case -9006: returnTxt = "企业中文名格式错误"; break;
                case -9007: returnTxt = "企业中文名简称格式错误"; break;
                case -9008: returnTxt = "邮件地址格式错误"; break;
                case -9009: returnTxt = "企业英文名格式错误"; break;
                case -9010: returnTxt = "企业英文名简称格式错误"; break;
                case -9011: returnTxt = "传真格式错误"; break;
                case -9012: returnTxt = "联系人格式错误"; break;
                case -9013: returnTxt = "联系电话"; break;
                case -9014: returnTxt = "邮编格式错误"; break;
                case -9015: returnTxt = "新密码格式错误"; break;
                case -9016: returnTxt = "发送短信包大小超出范围"; break;
                case -9017: returnTxt = "发送短信内容格式错误"; break;
                case -9018: returnTxt = "发送短信扩展号格式错误"; break;
                case -9019: returnTxt = "发送短信优先级格式错误"; break;
                case -9020: returnTxt = "发送短信手机号格式错误"; break;
                case -9021: returnTxt = "发送短信定时时间格式错误"; break;
                case -9022: returnTxt = "发送短信唯一序列值错误"; break;
                case -9023: returnTxt = "充值卡号格式错误"; break;
                case -9024: returnTxt = "充值密码格式错误"; break;
                case -9025: returnTxt = "客户端请求sdk5超时"; break;
                default: returnTxt = "成功"; break;
            }
            return returnTxt;
        }
    }
}
