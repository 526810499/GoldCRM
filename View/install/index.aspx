<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="XHD.Server" %>

<%
    //刷新静态方法缓存  
    var inss = new install();
    string filename = "/conn.config";
    inss.CheckConfig(filename);
    string filename1 = "/Web.config";
    inss.CheckConfig(filename1);

    //判断是否已配置
    var ins = new install();
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
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#btn_next").click(function () {
                window.location.href = "step2.aspx";
            });
        })
        </script>
    <style type="text/css">
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
        <div style="margin-left: 50px; margin-top: 100px; width: 731px;">
            <table id="__01" width="732" height="358" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 40px;">
                        <span style="font-size: 28px; font-weight: bolder;">欢迎使用永坤金行-CRM</span>
                    </td>
                </tr>
                <tr>
                    <td>

                        <table class="bodytable3">
                            <tr>
                                <td style="border-bottom: 1px solid #d2e2f2; width: 400px;">
                                   

                                </td>
                                <td style="border-bottom: 1px solid #d2e2f2;">
                                    <img src="../images/logo/logo.png" width="234" alt="XHD crm" /></td>
                            </tr>
                        </table>
                        <p>
                            <span style="-webkit-text-stroke-width: 0px; color: rgb(0, 0, 0); display: inline !important; float: none; font: 12px/25px 宋体, SimSun; font-size-adjust: none; font-stretch: normal; letter-spacing: normal; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px;">&nbsp; 永坤金行-CRM是一款开源免费的客户关系管理系统，是为帮助企业快速成长发展而开发的一款优秀的CRM客户关系管理系统，能帮助您管理客户与销售，能协同进行工作，并能方便的进行二次开发与扩展，是您企业信息化进程最佳的选择。</span>
                        </p>
                        <p>
                            &nbsp;&nbsp; 永坤金行-CRM能在手机和平板等移动设备上使用，对于业务员来说，非常方便。&nbsp; &nbsp;
                           
                        </p>
                        <p>
                            &nbsp;&nbsp; 如需进行<strong>二次开发</strong>，请联系我们。比如<span class="auto-style2"><strong>审批工作流，OA，进销存</strong></span>等功能。&nbsp;
                           
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">
                        <input type="button" value="同意" style="height: 25px; width: 80px;" id="btn_next" />
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
