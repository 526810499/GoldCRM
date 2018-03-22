<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>


    <script type="text/javascript">
        var dialog = frameElement.dialog;
        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 150, allowLeftResize: false, allowLeftCollapse: true, space: 2 ,heightDiff:1});
            $("#tree1").ligerTree({
                //url: '../data/Sys_Menu.GetSysApp&rnd=' + Math.random(),
                url: 'Sys_App.GetAppList.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                //usericon: 'App_icon',
                iconpath: '../',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return rowid + 1; } },
                    { display: '菜单名称', name: 'Menu_name', align: 'left', width: 190 }
                ],
                rownumbers: true,
                rowid: "Menu_id",
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                checkbox: true,
                url: "Sys_base.GetMenu.xhd",
                width: '100%', height: '100%',
                usePager: false,
                tree: { columnName: 'Menu_name' },
                heightDiff: 30
            });


            var manager = $("#maingrid4").ligerGetGridManager();
            manager._onResize();
        });

        function onSelect(note) {
            $(" .l-checked", ".l-grid-header-table").removeClass("l-checked");
            //加载数据
            var manager = $("#maingrid4").ligerGetGridManager();
            //manager._showData({ "Rows": [], "Total": 0 });
            manager._clearGrid();
            var url = "Sys_base.GetMenu.xhd?appid=" + note.data.id + '&rdm=' + Math.random();
            manager._setUrl(url);
            manager.loadData(true);
            //执行操作
        }

        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getCheckedRows();

            //alert(rows.length);
            //return rows;

            if (rows.length < 1) return;

            var data = [];

            $(rows).each(function (i, item) {
                var isleaf = manager.hasChildren(item);
                // alert(JSON.stringify(item));
                if (!isleaf)
                    data.push(item);
            })

            return data;
        }

    </script>

</head>
<body style="padding: 0px; overflow: hidden;">
    <div id="layout1" style="margin: -1px;">
        <div position="left" title="系统目录">
            <div id="treediv1" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto; padding-top: 5px;">
                <ul id="tree1"></ul>
            </div>
        </div>
        <div position="center" title="目录">

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </div>
</body>
</html>
