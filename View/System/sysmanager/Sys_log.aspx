<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                        //{ display: '序号', width: 50, render: function (item,i) { return item.n; } },
                        { display: '日志类型', name: 'EventType', width: 120 },
                        //{ display: '标识', name: 'EventID', width: 40 },
                        { display: '标题', name: 'EventTitle', width: 250 },
                        { display: '操作人', name: 'UserID', width: 120, render: function (item, i) { return item.UserName } },
                        {
                            display: '操作时间', name: 'EventDate', width: 180, render: function (item) {
                                return formatTime(item.EventDate);
                            }
                        },
                        { display: 'IP', name: 'IPStreet', width: 160 }

                ],
                rownumbers: true,
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "Sys_log.grid.xhd",
                width: '100%', height: '100%',
                heightDiff: -11,

                detail: {
                    height: 'auto',
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        $(p).append(
                            "<table class='bodytable0'  style='width:99%;margin:5px'>" +
                                //"<tr>" +
                                //    "<td height='23' width='10%' class='table_label'>日志类型：</td><td height='23'  width='15%'>" + r.EventType + "</td>" +                                    
                                //    "<td height='23' width='10%' class='table_label'>标题：</td><td height='23'  width='15%'>" + r.EventTitle + "</td>" +
                                //    "<td height='23' width='10%' class='table_label'>IP：</td><td height='23'  width='15%'>" + r.IPStreet + "</td>" +
                                //"</tr>" +
                                "<tr>" +

                                    "<td height='23' width='10%' class='table_label'>标识：</td><td height='23'  >" + r.EventID + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>日志内容：</td><td height='23'>" + r.Log_Content.replace(/\n/g, "<br />") + "</td>" +
                                "</tr>" +
                            "</table>");
                    }
                }
            });
            $("#toolbar").ligerToolBar({
                items: [
                { type: 'textbox', id: 'stype', text: '类型：' },
                { type: 'textbox', id: 'sstart', text: '时间：' },
                { type: 'textbox', id: 'sdend', text: "" },
                { type: 'textbox', id: 'stext', text: '关键字：' },
                { type: 'button', text: '智能搜索', icon: '../../images/search.gif', disable: true, click: function () { doserch() } },
                { type: 'button', text: '重置', icon: '../../images/edit.gif', disable: true, click: function () { $("#serchform").each(function () { this.reset(); }); } }
                ]
            });



            $("#stype").ligerComboBox({ width: 100, url: "Sys_log.logtype.xhd", onSuccess: function () { $("#maingrid4").ligerGetGridManager()._onResize(); } })
            $("#sstart").ligerDateEditor({ width: 100 })
            $("#sdend").ligerDateEditor({ width: 100 })
            $("#stext").ligerTextBox({ width: 200 })

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());





        });
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("Sys_log.grid.xhd?" + serchtxt);
            manager.loadData(true);
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };


    </script>
</head>
<body>

    <div style="padding: 10px;">
        <form id="serchform">
            <div id="toolbar"></div>

        </form>
        <form id="form1" onsubmit="return false">
            <div id="maingrid4" style="background: #fff;"></div>

        </form>
    </div>


</body>
</html>
