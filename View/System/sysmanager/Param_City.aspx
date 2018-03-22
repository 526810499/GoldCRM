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
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 200, allowLeftResize: false, allowLeftCollapse: true, space: 5, heightDiff: -10 });
            $("#tree1").ligerTree({
                url: 'Sys_Provinces.tree.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first div:first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            var winH = $(window).height()
            $("#provicecontent").height(winH - 56);

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', name: 'id', width: 50, render: function (item, i) { return item.n; } },
                    { display: '城市', name: 'City', width: 120 },
                    { display: '排序', name: 'City_order', width: 120 }


                ],
                rownumbers:true,
                dataAction: 'server',
                url: "Sys_City.grid.xhd?provincesid=0&rnd=" + Math.random(),
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                width: '100%',
                height: '100%',
                heightDiff: -10,

                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
            toolbar();

            manager = $("#maingrid4").ligerGetGridManager();

        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=sys_city&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }

                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                $("#stext").ligerTextBox({ width: 200 });
                manager._onResize();
            });
        }


        function onSelect(note) {
            var url = "Sys_City.grid.xhd?provincesid=" + note.data.id + "&rnd=" + Math.random();
            manager._setUrl(url);
        }


        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (!row) { top.$.ligerDialog.error('请选择行！'); return; }

            f_openWindow('System/sysmanager/Param_City_add.aspx?id=' + row.id, "修改城市", 390, 300, f_save);

        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();

            if (notes != null && notes != undefined) {
                f_openWindow('System/sysmanager/Param_City_add.aspx?pid=' + notes.data.id, "新增城市 上级机构【" + notes.data.text + "】", 390, 300, f_save);
            }
            else {
                $.ligerDialog.warn('请选择产品类别！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (!row) { top.$.ligerDialog.error('请选择行！'); return; }
            if (row.City_type == "sys") { $.ligerDialog.warn("系统参数，不能删除！"); return; }

            $.ligerDialog.confirm("删除不能恢复，确定删除？", function (yes) {
                if (yes) {
                    $.ajax({
                        url: "Sys_City.del.xhd", type: "POST",
                        data: { id: row.id, rnd: Math.random() },
                        dataType: 'json',
                        success: function (result) {
                            top.$.ligerDialog.closeWaitting();

                            var obj = eval(result);

                            if (obj.isSuccess) {
                                f_load();
                            }
                            else {
                                top.$.ligerDialog.error(obj.Message);
                            }
                        },
                        error: function () {
                            top.$.ligerDialog.closeWaitting();
                            top.$.ligerDialog.error('删除失败！');
                        }
                    });
                }
            })


        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Sys_City.save.xhd", type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        top.$.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_load();
                        }
                        else {
                            top.$.ligerDialog.error(obj.Message);
                        }
                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('操作失败！');
                    }
                });

            }
        }
        function f_load() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }

    </script>

</head>
<body style="padding: 0px; overflow: hidden;">
    <form id="form1" onsubmit="return false">
        <div id="layout1" style="margin: 10px">
            <div position="left" title="省份" style="overflow-y: scroll;padding-top:5px;" id="provicecontent">
                <ul id="tree1"></ul>
            </div>
            <div position="center">
                <div id="toolbar" style="margin-top: 10px;"></div>
                <div id="maingrid4" style="margin: -1px;"></div>

            </div>
        </div>
    </form>
</body>
</html>
