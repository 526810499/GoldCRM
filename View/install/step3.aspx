<%@ Page Language="C#" AutoEventWireup="true" %>

<%
    //刷新静态方法缓存  
    XHD.Server.install inss = new XHD.Server.install();
    string filename = Server.MapPath("/conn.config");
    inss.CheckConfig(filename);
    string filename1 = Server.MapPath("/Web.config");
    inss.CheckConfig(filename1);

    //判断是否已配置
    XHD.Server.install ins = new XHD.Server.install();
    int configed = ins.configed();

    if (configed == 1)
    {
        Response.Redirect("remind.aspx");
    }

%>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="ie=8 chrome=1" http-equiv="X-UA-Compatible">
    <meta http-equiv="content-type" content="text/html; charset=gb2312">
    <title>永坤金行-CRM-安装</title>

    <link href="../CSS/input.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>

    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">
        var btn_next;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("form").ligerForm();

            $("#btn_test").ligerButton({ text: "测试连接", width: 60, click: check });
            btn_next = $("#btn_next").ligerButton({ text: "开始安装", width: 60, click: runconfig });
            btn_next.setDisabled();
        });
        function check() {
            if ($(form1).valid()) {
                var sendtxt = $("form :input").fieldSerialize() + "&rnd=" + Math.random();
                $.ajax({
                    type: 'post',
                    url: 'install.checkconnect.xhd',
                    data: sendtxt,
                    beforeSend: function () {
                        $.ligerDialog.waitting("检测中...");                       
                    },
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        if (result == "false:connect") {
                            $.ligerDialog.error("连接错误，请检查服务器名，用户名和密码！");
                            btn_next.setDisabled();
                        }
                        else if (result == "false:configed") {
                            $.ligerDialog.error('检测失败，系统已经配置过，如需重新配置，请在conn.config文件里，把CompleteConfig节配置为false。');
                            btn_next.setDisabled();
                        }
                        else if (result == "warn:dbname") {
                            $.ligerDialog.warn('已存在名为"' + $("#t_db_name").val() + '"的数据库,请保证此数据库下无永坤金行-CRM的相关表。<br/>已连接成功，可以开始配置。');
                            btn_next.setEnabled();
                        }
                        else if (result == "false:dbfile") {
                            $.ligerDialog.error('App_Data文件夹下已存在名为"' + $("#t_db_name").val() + '.mdf"的文件');
                            btn_next.setDisabled();
                        }
                        else if (result == "success") {
                            $.ligerDialog.success("连接成功，可以开始配置。");
                            btn_next.setEnabled();
                        }
                        else {
                            $.ligerDialog.error("系统错误！");
                            btn_next.setDisabled();
                        }
                     

                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error("检测失败!");
                    }

                });

            }
            else {
                btn_next.setDisabled();
            }
        }
        function runconfig() {
            if ($(form1).valid()) {
                var sendtxt = $("form :input").fieldSerialize() + "&rnd=" + Math.random();
                $.ajax({
                    type: 'post',
                    url: 'install.startconfig.xhd',
                    data: sendtxt,
                    beforeSend: function () {
                        $.ligerDialog.waitting("系统配置中,请稍后...");
                        $("#btn_next").attr("disabled", "disabled");
                    },
                    success: function (result) {
                        window.location.href = "success.aspx";
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error("配置失败！系统错误。<br/>1、请检查此账号是否有创建数据库的权限。<br/>2、如已创建数据库，请保证此数据库下无永坤金行-CRM的相关表。<br/>3、联系永坤金行-CRM官方技术人员解决。");
                    }
                });
            }
            else {
                $("#btn_next").attr("disabled", "disabled");
            }
        }
        function btn_reset() {
            $("#btn_next").attr("disabled", "disabled");
        }
    </script>
    <style type="text/css">
        span { font-weight: bolder; }
        img { border: none; }
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(../images/login/login_bg.png) repeat-x; font-size: 12px; }
        .bodytable0 td { height: 30px; }
    </style>
    <script type="text/javascript">
        if (top.location != self.location) top.location = self.location;
    </script>
</head>
<body>
    <form id="form1" name="form1">

        <div style="width: 731px; margin-left: 50px; margin-top: 100px;">
            <table class="bodytable3" id="Table1" width="732" height="358" border="0" cellpadding="0" cellspacing="0">
                 <tr>
                    <td style="height:40px;">
                        <span style="font-size: 28px; font-weight: bolder;">欢迎使用永坤金行-CRM</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 400px;">
                                    <p>
                                        <span style="color:#ff0;font-weight:bolder;">Step3:</span>现在对您的运行环境进行配置。
                                   
                                    </p>
                                </td>
                                <td style="width: 300px; text-align: center;">
                                    <img src="../images/logo/logo.png" width="234" alt="XHD crm" /></td>
                            </tr>
                        </table>
                        <div style="box-shadow: 5px 20px 20px rgba(0,0,0,0.3); background: #fff;">
                            <table class="bodytable0" style="width: 100%; margin: -1px; line-height: 25px;">

                                <tr>
                                    <td colspan="3" height="25">数据库配置：</td>
                                </tr>

                                <tr>
                                    <td width="150px">服务器名：</td>
                                    <td height="25">
                                        <input type="text" id="t_name" name="t_name" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                    </td>
                                    <td height="25">如：.\sqlexpress，您的SQL服务器名</td>
                                </tr>
                                <tr>
                                    <td>登录名：</td>
                                    <td height="25">
                                        <input type="text" id="t_uid" name="t_uid" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                    </td>
                                    <td height="25">如：sa，您的SQL登陆账号，请不要用windows登陆验证</td>
                                </tr>
                                <tr>
                                    <td>密码：</td>
                                    <td height="25">
                                        <input type="text" id="t_pwd" name="t_pwd" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                    </td>
                                    <td height="25">数据库账号的密码</td>
                                </tr>
                                <tr>
                                    <td>是否加密：</td>
                                    <td height="25">
                                        <input type="text" id="t_Encrypt" name="t_Encrypt" ltype="select" ligerui="{width:200,data:[{'text':'加密',id:0},{'text':'不加密',id:1}],initValue:0}" validate="{required:true}" /></td>
                                    <td height="25">连接字符串是否加密</td>
                                </tr>
                                <tr>
                                    <td>数据库名：</td>
                                    <td height="25">
                                        <input type="text" id="t_db_name" name="t_db_name" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" /></td>
                                    <td height="25">数据库名称，如已创建好数据库，请填写已建好的数据库名称</td>
                                </tr>
                                <tr>
                                    <td colspan="3">1、如不知填写，请访问官方论坛寻求帮助：<a href="http://bbs.xhdcrm.com" target="_blank">http://bbs.xhdcrm.com</a><br />
                                        2、如果是重新配置，请谨慎操作，以防数据丢失。</td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">&nbsp;
                        <div id="btn_next"  style="margin-right:5px;float:right;"></div>
                        <div id="btn_test"  style="margin-right:5px;float:right;"></div>                       
                        
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
