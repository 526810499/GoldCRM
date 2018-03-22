<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title> 
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../../lib/ligerUI/skins/touch/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery.form.js" type="text/javascript"></script>      
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var str1 = getparastr("rid");
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '序号', name: 'n', width: 50 },
                    { display: '姓名', name: 'C_name', width: 100 },
                    { display: '号码', name: 'mobile', width: 100 },
                    {
                        display: '状态', name: 'reportStatus', width: 80, render: function (item, i)
                        {
                            if (item.reportStatus == 0)
                                return "已接收";
                            else
                                return "发送失败"
                        }
                    }

                ],               
                checkbox: false,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "Sys_SMS_reports.getStatus.xhd?id=" +getparastr('id'),
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: 0
            });
            //toolbar();

            //$(document).keydown(function (e) {
            //    if (e.keyCode == 13 && e.target.applyligerui) {
            //        doserch();
            //    }
            //});
        });
        function toolbar() { 
            var items = []; 
            items.push({ type: 'textbox', id: 'company', text: '姓名：' });
            items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

            $("#serchbar1").ligerToolBar({
                items: items

            });
             $("#company").ligerTextBox({ width: 200, nullText: "输入关键词智能搜索客户" });
            $("#maingrid4").ligerGetGridManager()._onResize();            

           
        }
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            $.ligerDialog.waitting('数据查询中,请稍候...');
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("CRM_Customer.grid.xhd?" + serchtxt);
            manager.loadData(true);
            $.ligerDialog.closeWaitting();
        }
        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            //alert(rows);
            return rows;
        }


    </script>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div>
            <div id="serchbar1"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
