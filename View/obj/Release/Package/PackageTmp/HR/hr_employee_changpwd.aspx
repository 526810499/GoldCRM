<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        var dialog = frameElement.dialog;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("#T_newpwd").focus();
            $("form").ligerForm();
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&empid=" + getparastr("empid") + "&rnd=" + Math.random();
                var postdata = $("form :input").fieldSerialize() + sendtxt;
                $.ajax({
                    url: "hr_employee.allchangepwd.xhd", type: "POST",
                    data: postdata,
                    dataType: "json",
                    success: function (result) {
                        $.ligerDialog.closeWaitting();
                        var obj = eval(result);

                        if (obj.isSuccess) {
                            top.getuserinfo();
                            dialog.close();
                        }
                        else {
                            top.$.ligerDialog.error(obj.Message);
                        }
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('操作失败！系统错误。');
                    }
                });
            }
        }

    </script>
</head>
<body style="margin: 5px 5px 5px 5px">
    <form id="form1" onsubmit="return false">

        <div>
            <table class="aztable" border="0" cellpadding="3" cellspacing="1"
                style="background: #fff; width: 280px;">

                <tr>
                    <td height="23" width="70px">
                        <div align="right" style="width: 61px">
                            新密码：
                        </div>
                    </td>
                    <td height="23">
                        
                            <input type="password" id="T_newpwd" name="T_newpwd" ligerui="{width:180}"
                                validate="{required:true,minlength:4,maxlength:25,messages:{required:'请输入新密码'}}" ltype="password" />
                       

                    </td>
                </tr>

                <tr>
                    <td height="23" width="70px">
                        <div align="right" style="width: 62px">确认密码：</div>

                    </td>
                    <td height="23">
                       
                            <input type="password" id="T_confime" name="T_confime" ligerui="{width:180}"
                                validate="{required:true,minlength:4,maxlength:25,equalTo:'#T_newpwd',equalTo:'#T_newpwd',messages:{required:'请再次输入新密码'}}" ltype="password" />
                       

                    </td>
                </tr>

                <tr>
                    <td height="23" width="70px"></td>
                    <td height="23">&nbsp;</td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
