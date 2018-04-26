<%@ Page Language="C#" AutoEventWireup="true" %>

<%
    //刷新静态方法缓存  
    XHD.Server.install inss = new XHD.Server.install();
    string filename = "/conn.config";
    inss.CheckConfig(filename);
    string filename1 ="/Web.config";
    inss.CheckConfig(filename1);
 %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="ie=8 chrome=1" http-equiv="X-UA-Compatible">
    <meta http-equiv="content-type" content="text/html; charset=gb2312">
    <title>永坤金行-CRM-安装</title>

    <link href="../CSS/input.css" rel="stylesheet" />
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#btn_next").click(function () { runconfig() })
        });
        function runconfig() {
            window.location.href = "../default.aspx";
        }

    </script>
    <style type="text/css">
        span { font-weight: bolder; }
        img { border: none; }
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(../images/login/login_bg.png) repeat-x; font-size: 12px; }
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
                                        <span style="color: #ff0; font-weight: bolder;">Success:</span>系统配置成功，现在开始您的永坤金行-CRM办公之旅吧！
                                    </p>
                                </td>
                                <td style="width: 300px; text-align: center;">
                                    <img src="../images/logo/logo.png" width="234" alt="XHD crm" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">&nbsp;
                        <input type="button" value="开始使用" style="width: 80px; height: 25px;" id="btn_next" /></td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
