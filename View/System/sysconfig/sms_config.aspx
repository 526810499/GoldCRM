<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />

    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        var sys_id = "", sys_name = "", sys_version = "";
        var btn_reg, btn_serch;
        $(function () {
            btn_reg = $("#btn_reg").ligerButton({ text: "系统注册", width: 60, disabled: true, click: regSMS });
            btn_serch = $("#btn_serch").ligerButton({ text: "查询余额", width: 60, disabled: true, click: getBalance });
            $.ajax({
                type: "GET",
                url: "Sys_info.grid.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var rows = obj.Rows;
                    var sysinfo = {};
                    for (var i = 0; i < rows.length; i++) {
                        if (rows[i].sys_value == "null" || rows[i].sys_value == null) {
                            rows[i].sys_value = " ";
                        }
                        sysinfo[rows[i].sys_key] = rows[i].sys_value;
                    }
                    var isReg = sysinfo["sms_done"];

                    if (isReg == 1) {
                        $("#Label1").text("已经注册");
                        btn_reg.setDisabled();
                        btn_serch.setEnabled();
                    }
                    else {
                        $("#Label1").text("还未注册");
                        btn_reg.setEnabled();
                        btn_serch.setDisabled();
                    }
                }
            });

            
        });
        function regSMS() {
            f_openWindow("system/sysconfig/sms_config_add.aspx", "短信注册", 400, 200, f_save)
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (!issave) return;

            $.ajax({
                type: "GET",
                url: "Sys_info.regSMS.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: issave, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () { },
                success: function (result) {
                    var obj = eval(result);
                    if (obj.isSuccess) {
                        $.ligerDialog.success("注册成功！ ");
                        $("#Label1").text("已经注册");
                        btn_reg.setDisabled();
                        btn_serch.setEnabled();
                        dialog.close();
                    }
                    else {
                        top.$.ligerDialog.error(obj.text, "注册失败！", null, 9003);
                        btn_reg.setEnabled();
                        btn_serch.setDisabled();
                    }
                },
                error: function () {
                    top.$.ligerDialog.error('系统错误！', 9003);
                }
            });
        }
        function getBalance() {
            $.ajax({
                type: "GET",
                url: "SMS_Helper.getBalance.xhd?rnd=" + Math.random(), /* 注意后面的名字对应CS的方法名称 */
                contentType: "application/json; charset=utf-8",
                beforeSend: function () { },
                success: function (result) {
                    var sms = result * 10;
                    if (sms < 100) {
                        $.ligerDialog.warn(sms + ' 条', '短信剩余条数');
                    }
                    else {
                        $.ligerDialog.success(sms + ' 条', '短信剩余条数');
                    }
                },
                error: function () {
                    top.$.ligerDialog.error('系统错误！');
                }
            });
        }

    </script>
    <style type="text/css">
        .content {box-shadow: 0 2px 10px rgba(0,0,0,0.3);background:#fff; }
    </style>
</head>
<body style="padding: 0px; overflow-y: scroll;">
    <div style="padding: 10px;">
        <div class="content">
            <table style="width: 100%;" class="bodytable6">
                <tr>
                    <td height="23" width="150"  colspan="2">如果需要注册及充值，请联系{xx}CRM官方。QQ:250476029。</td>
                </tr>

                <tr>
                    <td height="23" width="150" >注册信息：</td>
                    <td height="23">
                        <span id="Label1"></span>
                    </td>
                </tr>
                <tr>
                    <td height="23" >操作：</td>
                    <td height="23">
                        <div id="btn_reg"></div>
                        <div id="btn_serch"></div>
                    </td>
                </tr>
                <tr>
                    <td height="23" colspan="2">
                        <p>
                            <span class="l-verify-star"><strong>{xx}CRM申明：</strong></span><br />
                        1、您如果需要使用短信通道，您必须遵守《信息安全保障责任书》。<br />
                        2、{xx}CRM只提供短信通道的使用，由于短信使用而产生的法律问题，{xx}CRM概不负责。<br />
                        3、您如果通过{xx}CRM使用此短信通道，表示您同意以上{xx}CRM的申明。</p>
                    </td>
                </tr>
                <tr>
                    <td height="23" colspan="2"><span class="l-verify-star"><strong>《信息安全保障责任书》</strong></span><br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    本公司在使用本软件进行短信通讯过程中，将严格遵守国家相关法律、法规，保证本公司短信内容的信息安全，并切实做到：<br />
                        一、建立健全本公司商品的内部保障制度、信息安全保密制度、用户信息安全管理制度，建立健全本公司信息安全责任制度和信息发布的审批制度，严格审查本公司商品所发布的信息。<br />
                        二、严格遵守《互联网信息服务管理办法》，对使用本软件编辑的短信内容进行把关，保证信息内容的健康、合法。<br />
                        三、对软件操作人员分配密码，并在使用软件发送短信时显示用户代码，不允许匿名的短信直接发送到其他手机用户。<br />
                        四、针对本企业应用的相关业务，明确用户群和用户范围，手机用户必须向企业登记自愿接受服务，不对未登记注册的用户提供信息。如通过本软件向手机用户提供有关资讯内容，需要向国家有关部门申请相关的资质证明，并自觉遵守相关规定。<br />
                        五、不擅自超出使用许可提供信息服务。<br />
                        六、不利用短信批发平台制作、复制、发布、传播含有下列内容的信息：  
                   
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1、 违反宪法所确定的基本原则的；  
                   
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2、危害国家安全，泄露国家秘密，颠覆国家政权，破坏国家统一的；<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3、 损坏国家荣誉和利益的；  
                   
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 4、 煽动民族仇恨、民族歧视，破坏民族团结的；  
                   
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 5、 破坏国家民族宗教政策，宣扬邪教和封建迷信的；  
                   
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 6、 散布谣言，扰乱社会秩序，破坏社会稳定的；<br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 7、 散布淫秽、色情、赌博、暴力、凶杀、恐怖或者教唆犯罪的；  
                   
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 8、 侮辱或者诽谤他人，侵害他人合法权益的；  
                   
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 9、含有法律、法规禁止的其他内容的；<br />
                        七、应建立专用的监控系统，对通过本系统发布的内容进行监控，并记录使用本系统发布信息的内容、时间、服务代码等内容，记录备份应至少保持6个月以上，在国家机关进行依法查询时，予以提供。<br />
                        八、若发现使用本系统所发布的信息明显属于上述第六条所列内容之一的，保证立即停止传输，并向国家有关机关报告。<br />
                        九、对使用本系统所发布的信息一时难以辨别是否属于以上所列内容之一的，应报相关主管部门审核同意后再发布。<br />
                        十、针对本企业应用的相关业务，发送短信过程中要自觉体现会员特征、留电话、留公司名称三大特征；保证发送对象为自己的会员，并愿意协助我方解决手机用户的投诉。<br />
                        十一、对客户的个人信息保密，未经客户同意不得向他人泄漏，但法律规定的除外。 
                     
                   

                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    本公司保证：在使用本系统过程中，服从监督和管理；若未做到上述一至十一条，本公司愿意承担由此引起的一切法律责任，并接受相应的处罚。</td>
                </tr>

            </table>
        </div>
    </div>
</body>
</html>
