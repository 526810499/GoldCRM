<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var str1 = getparastr("rid");
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID', type: 'int', width: 50 },
                    { display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    { display: '名字', name: 'name' },
                    { display: '性别', name: 'sex', width: 50 },
                    { display: '部门', name: 'dname' },
                    { display: '职务', name: 'zhiwu' }
                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "Sys_role_emp.emplist.xhd?role_id=" + getparastr("role_id"),
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -6
            });

            toolbar();
        });

        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getCheckedRows();
            return rows;
        }

        function toolbar() {
            var items = [];

            items.push({ type: 'textbox', id: 'stext', text: '姓名：' });
            items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

            $("#toolbar").ligerToolBar({
                items: items

            });

            $("#stext").ligerTextBox({ width: 200 })
            $("#maingrid4").ligerGetGridManager()._onResize();

        }
        //查询
        function doserch() {
            var sendtxt = "&role_id=" + getparastr("role_id") + "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("Sys_role_emp.emplist.xhd?" + serchtxt);
            manager.loadData(true);
        }

    </script>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div style="padding-top:10px;">
            <div id="toolbar"></div>
            <div id="maingrid4" style="margin:-1px;"></div>
        </div>
    </form>


</body>
</html>
