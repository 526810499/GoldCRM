<%@ Page Language="C#" AutoEventWireup="true" %>

<%--Èë¿ÚÒ³Ãæ--%>
<%
    //ÅÐ¶ÏÊÇ·ñÒÑÅäÖÃ
    XHD.Server.install ins = new XHD.Server.install();
    int configed = ins.configed();

    if (configed == 1)
    {
        //ÅÐ¶ÏÊÇ·ñµÇÂ½
        HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        if (Request.IsAuthenticated && null!=cookie)
            Response.Redirect("main.aspx");

        else
            Response.Redirect("login.aspx");
        //Response.Redirect("login.aspx");
    }
    else
        Response.Redirect("install/index.aspx");
 %>

