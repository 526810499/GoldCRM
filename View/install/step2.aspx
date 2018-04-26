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
    <meta http-equiv="content-type" content="text/html; charset=utf-8">
    <title>永坤金行-CRM-安装</title>

    <link href="../CSS/input.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        var btn_next;
        $(function () {
            $("#btn_retry").ligerButton({ text: "重新检测", width: 60, click: check });
            btn_next = $("#btn_next").ligerButton({ text: "下一步", width: 60, click: f_next });
            check();
        });
        function f_next() {
            window.location.href = "step3.aspx";
        }
        function check() {
            $.ajax({
                type: 'post',
                url: 'install.initcheck.xhd',
                data: [
                { name: 'rnd', value: Math.random() }
                ],
                beforeSend: function () {
                    $.ligerDialog.waitting("检测中...");
                    btn_next.setDisabled();
                },
                success: function (result) {
                    $.ligerDialog.closeWaitting();

                    var arr = new Array();
                    arr = result.split(",");
                    if (arr[0] == 1) $("#Span0").text("通过！"); else $("#Span0").text("失败！");
                    if (arr[1] == 1) $("#Span1").text("通过！"); else $("#Span1").text("失败！");
                    if (arr[2] == 1) $("#Span2").text("通过！"); else $("#Span2").text("失败！");
                    if (arr[3] == 0) $("#Span3").text("通过！"); else $("#Span3").text("失败，已配置！");
                    $(".bodytable0 span").css({ "font-weight": "bolder", "color": "#f00" });
                    if (arr[0] == 1 && arr[1] == 1 && arr[2] == 1 && arr[3] == 0) {
                        btn_next.setEnabled();
                    }
                    else {
                        btn_next.setDisabled();
                    }
                },
                error: function () {
                    $.ligerDialog.closeWaitting();
                    alert("检测失败");
                }
            });
        }
    </script>
    <style type="text/css">
       
        img { border: none; }

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
                                        <span style="color: #ff0; font-weight: bolder;">Step2:</span>现在对您的运行环境进行检测，以确认您的环境符合要求.
                                   
                                    </p>
                                    <p>
                                        <font style="color: #ff0; font-weight: bolder;">注意:</font>如果出现目录或文件没有写入和删除权限情况,请选择该目录或文件-&gt;右键属性-&gt;安全-&gt;添加, 在&quot;输入对象名称来选择&quot;中输入&quot;Network Service&quot;,点击&quot;确定&quot;.选择&quot;组或用户名称&quot;中&quot;Network Service&quot;用户组,在下面 &quot;Network Service&quot;的权限中勾选&quot;修改&quot;的&quot;允许&quot;复选框,点击&quot;确定&quot;后再次重新刷新本页面继续.
                                   
                                    </p>
                                </td>
                                <td style="width: 300px; text-align: center;">
                                    <img src="../images/logo/logo.png" width="234" alt="XHD crm" /></td>
                            </tr>
                        </table>
                        <div style="box-shadow: -20px 20px 20px rgba(0,0,0,0.3); background: #fff;">
                            <table class="bodytable0" style="width: 100%; margin: -1px">

                                <tr>
                                    <td height="23" width="50%" class="table_label">conn.config文件是否可写：</td>
                                    <td height="23">
                                        <span id="Span0"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" class="table_label">install文件夹是否可写：</td>
                                    <td height="23">
                                        <span id="Span1"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" class="table_label">App_Data文件夹是否可写：</td>
                                    <td height="23">
                                        <span id="Span2"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" class="table_label">是否已经配置完毕：</td>
                                    <td height="23">
                                        <span id="Span3"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="23" colspan="2">注：如已经配置完毕，您需要重新配置，请在conn.config文件里，把“<span class="auto-style1">CompleteConfig</span>”节配置为<span class="auto-style1">false</span></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height="145" >
                        <div id="btn_next" style="margin-right:5px;float:right;"></div>
                        <div id="btn_retry" style="margin-right:5px;float:right;"></div>
                        
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
