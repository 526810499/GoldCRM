<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="ie=edge chrome=1" http-equiv="X-UA-Compatible">
    <meta name="renderer" content="webkit">
    <title>永坤金行-CRM-登录</title>
    <link rel="shortcut icon" type="image/x-icon" href="images/logo/favicon.ico" />
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="CSS/input.css" rel="stylesheet" type="text/css" />
    <%-- <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"/>--%>
    <script src="lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/common.js" type="text/javascript"></script>

    <script src="JS/jquery.md5.js" type="text/javascript"></script>
    <script src="JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("input[ltype=text],input[ltype=password]", this).ligerTextBox();

            $("#btn_login").hover(function () {
                $(this).addClass("btn_login_over");
            }, function () {
                $(this).removeClass("btn_login_over");
            }).mousedown(function () {
                check();
            });

            if (getCookie("xhdcrm_uid") && getCookie("xhdcrm_uid") != null)
                $("#T_uid").val(getCookie("xhdcrm_uid"))

            $(document).keydown(function (e) {
                if (e.keyCode == 13) {
                    check();
                }
            });

            $("#reset").click(function () {
                $(":input", "#form1").not(":button,:submit:reset:hidden").val("");
            });

            function check() {
                if ($(form1).valid()) {
                    dologin();
                }
            }

            function dologin() {
                var company = $("#T_company").val();
                var uid = $("#T_uid").val();
                var pwd = $("#T_pwd").val();
                var validate = $("#T_validate").val();
 
                if (uid == "") {
                    $.ligerDialog.warn("账号不能为空！");
                    $("#T_uid").focus();
                    return;
                }
                if (pwd == "") {
                    $.ligerDialog.warn("密码不能为空！");
                    $("#T_pwd").focus();
                    return;
                }
                if (validate == "") {
                    $.ligerDialog.warn("验证码不能为空！");
                    $("#T_validate").focus();
                    return;
                }
                else if (validate.length != 4) {
                    $.ligerDialog.warn("验证码错误！");
                    $("#T_validate").focus();
                    return;
                }


                $.ajax({
                    type: 'post', dataType: 'json',
                    url: 'login.check.xhd',
                    data: [

                        { name: 'username', value: uid },
                        { name: 'password', value: $.md5(pwd) },
                        { name: 'validate', value: validate },
                        { name: 'rnd', value: Math.random() }
                    ],
                    dataType: 'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            SetCookie("xhdcrm_uid", uid, 30);
                            location.href = "main.aspx";
                        }
                        else {
                            $("#validate").click();
                            $.ligerDialog.error(obj.Message);

                        }
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $("#validate").click();
                        $.ligerDialog.warn('发生系统错误,请与系统管理员联系!');
                    },
                    beforeSend: function () {
                        $.ligerDialog.waitting("正在登录中,请稍后...");
                        $("#btn_lgoin").attr("disabled", true);
                    },
                    complete: function () {
                        $("#btn_login").attr("disabled", false);
                    }
                });
            }
        })


    </script>
    <style type="text/css">
        .btn_login_over {
            filter: alpha(opacity=80);
            opacity: 0.80;
        }

        img {
            border: none;
        }

        .text {
            border: #d2e2f2 1px solid;
            height: 19px;
        }

        body {
            BACKGROUND: #fff url(images/login/login_bg.png) repeat-x;
        }
    </style>
    <script type="text/javascript">
        if (top.location != self.location) top.location = self.location;
    </script>
</head>
<body>
    <form id="form1" name="form1">
        <div style="margin-left: 20px; margin-top: 10px;">
            <a href="#" target="_blank">
                <%-- <img src="images/logo/logo.png" height="45" alt="crm" />--%>
            </a>
        </div>

        <div style="width: 497px; margin-left: 300px; margin-top: 150px;">
            <div style="float: left; width: 200px; height: 279px; background: url(' images/login/login_02.png') no-repeat;"></div>
            <div style="float: left; width: 187px; height: 279px;">
                <div style="margin-top: 60px;">
                    <input id="T_uid" name="T_uid" type="text" style="width: 160px;" ltype="text" validate="{required:true}" />
                </div>
                <div style="margin-top: 12px;">
                    <input id="T_pwd" name="T_pwd" type="password" style="width: 160px" ltype="text" validate="{required:true}" />
                </div>
                <div style="margin-top: 12px;">
                    <div style="float: left; width: 77px;">
                        <img id="validate" onclick="this.src=this.src+'?'" src="ValidateCode.aspx" style="cursor: pointer; border: 1px solid #AECAF0; height: 22px;" alt="看不清楚，换一张" title="看不清楚，换一张" />
                    </div>
                    <div style="float: left; width: 82px; padding-left: 3px;">
                        <input id="T_validate" name="T_validte" type="text" style="width: 80px" ltype="text" validate="{required:true}" />
                    </div>
                </div>
                <div style="clear: both"></div>
                <div style="margin-top: 14px;">版本：v 2.0</div>
                <div style="margin-top: 14px;">
                    <div id="btn_login" style="background: url(images/login/login.png); width: 140px; height: 35px; cursor: pointer;"></div>
                </div>
            </div>
            <div style="float: right; width: 110px; height: 279px; background: url(' images/login/login_04.png') no-repeat;"></div>
        </div>
    </form>
</body>

</html>
