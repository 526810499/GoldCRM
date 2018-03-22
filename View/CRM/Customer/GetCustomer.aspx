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
        $(function () {
            var str1 = getparastr("rid");
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    { display: '客户名称', name: 'cus_name', width: 160, align: 'left' },

                    {
                        display: '客户所属', name: '', width: 120, render: function (item) {
                            return "【"+item.department + "】" + item.employee;
                        }
                    },

                    {
                        display: '最后跟进', name: 'lastfollow', width: 90, render: function (item) {
                            return formatTimebytype(item.lastfollow, 'yyyy-MM-dd');
                        }
                    },
                    { display: '电话', name: 'cus_tel', width: 150 }

                ],
                onAfterShowData: function (grid) {
                    $("tr[rowtype='已成交']").addClass("l-treeleve2").removeClass("l-grid-row-alt");                       
                },
                checkbox: false,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "CRM_Customer.grid.xhd?rnd=" + Math.random(),
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -2
            });
            toolbar();

            $(document).keydown(function (e) {
                if (e.keyCode == 13 && e.target.applyligerui) {
                    doserch();
                }
            });
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
            <div id="serchbar1" style="margin-top:10px;"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
