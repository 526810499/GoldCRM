<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" />
    <meta name="renderer" content="webkit" />
<%--    <title>永坤金行-CRM-登录</title>--%>
    <link rel="shortcut icon" type="image/x-icon" href="images/logo/favicon.ico" />
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="CSS/input.css" rel="stylesheet" type="text/css" />
    <link href="CSS/login/loginstyle.css" rel="stylesheet" />
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

            $(".submit_btn").hover(function () {
                $(this).addClass("btn_login_over");
            }, function () {
                $(this).removeClass("btn_login_over");
            }).click(function () {
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
                            location.href = "main1.aspx";
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
        });



        if (top.location != self.location) top.location = self.location;

    </script>
    <style>
        body {
            height: 100%;
            background: #16a085;
            overflow: hidden;
        }
    </style>
</head>
<body>
    <form id="form1" name="form1">
        <dl class="admin_login">
 
            <dd class="user_icon">

                <input id="T_uid" name="T_uid" type="text" placeholder="账号" class="login_txtbx" />
            </dd>
            <dd class="pwd_icon">
                <input id="T_pwd" name="T_pwd" type="password" placeholder="密码" class="login_txtbx" />
            </dd>
            <dd class="val_icon">


                <img id="validate" onclick="this.src=this.src+'?'" src="ValidateCode.aspx" class="J_codeimg" style="cursor: pointer; width: 80px; height: 30px; padding: 6px; margin-left: 15px; border-radius: 10px; z-index: 9999" alt="看不清楚，换一张" title="看不清楚，换一张" />

                <div class="checkcode">
                    <input type="text" style="width: 178px" id="T_validate" name="T_validte" placeholder="验证码" maxlength="4" class="login_txtbx">
                </div>

            </dd>
            <dd>
                <input type="button" value="立即登陆" class="submit_btn" />
            </dd>
            <dd>
                <p>© 2018-2216 ycrm 版权所有</p>
                <p></p>
            </dd>
        </dl>

    </form>


</body>

</html>
