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
       Response.Redirect( "../login.aspx");
    }
    else
    {
        Response.Redirect("../login1.aspx");
    }
   
 %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="ie=8 chrome=1" http-equiv="X-UA-Compatible">
    <meta http-equiv="content-type" content="text/html; charset=gb2312">
    <title>{xx}CRM-安装</title>   
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>  
    <script type="text/javascript">
          $(function () {
           
            $("#btn_next").click(function () {
                window.location.href = "../login.aspx";
            });             
        })
    </script>
    <style type="text/css">
        img { border: none; }
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(../images/login/login_bg.png) repeat-x; font-size: 12px; }
        .auto-style1 { color: #FF0000; }
    
* { font-size: 12px; }
* { font-family: Verdana, Geneva, sans-serif; font-size: 12px; }
        .auto-style2 { font-weight: bolder; }
    </style>
    <script type="text/javascript">
        if (top.location != self.location) top.location = self.location;
    </script>
</head>
<body>
    <form id="form1" name="form1">
        <div style="width: 731px; margin-left: 50px; margin-top: 100px;">
            <table id="__01" width="732" height="358" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <h2>欢迎使用{xx}CRM</h2>

                        <table class="auto-style1">
                            <tr>
                                <td style="border-bottom: 1px solid #d2e2f2; width: 400px;">
                                  
                                </td>
                                <td style="border-bottom: 1px solid #d2e2f2;">
                                    <img src="../images/logo/xhd.png" width="234" alt="XHD crm" /></td>
                            </tr>
                        </table>
                        <p>
                            <span style="WHITE-SPACE: normal; TEXT-TRANSFORM: none; WORD-SPACING: 0px; FLOAT: none; COLOR: rgb(0,0,0); FONT: 12px/25px 宋体,SimSun; DISPLAY: inline !important; LETTER-SPACING: normal; TEXT-INDENT: 0px; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px">&nbsp; 系统</span>已经配置完毕，您需要重新配置，请在conn.config文件里，把“<span class="auto-style2">CompleteConfig</span>”节配置为<span class="auto-style2">false</span></p>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">
                        <input type="button" value="同意" style="width: 80px; height: 25px;" id="btn_next" />
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
