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
    <script src="../../lib/json2.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 150, allowLeftResize: false, allowLeftCollapse: true, space: 2 });
            $("#tree1").ligerTree({
                //url: '../data/S_Sys_Menu.GetSysApp&rnd=' + Math.random(),
                url: 'Sys_App.GetAppList.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                usericon: 'App_icon',
                iconpath: '../../',
                checkbox: false,
                itemopen: false
            });

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid1").ligerGrid({
                columns: [
                    { display: 'ID', name: 'Menu_id', type: 'int', width: 120, align: 'left' },
                    { display: '菜单名', name: 'Menu_name', align: 'left' },
                    { display: '链接地址', name: 'Menu_url', align: 'left', width: 300 },
                    {
                        display: '图标', name: 'Menu_icon', width: 50, render: function (item) {
                            return "<img style='width:16px;height:16px;margin-top:4px;' src='../../" + item.Menu_icon + "'/>"
                        }
                    },
                    //{ display: '响应事件', name: 'Menu_handler' },
                    { display: '排序', name: 'Menu_order', width: 50 }

                ],
                onSelectRow: function (data, rowindex, rowobj) {
                    var manager = $("#maingrid2").ligerGetGridManager();
                    //manager.showData({ Rows: [], Total: 0 });
                    var url = "Sys_Button.GetGrid.xhd?menuid=" + data.Menu_id + "&rnd=" + Math.random();
                    manager._setUrl(url);
                },
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                tree: { columnName: 'Menu_name' },
                url: "Sys_Menu.GetMenuV2.xhd?parentid=-1",
                width: '100%',
                height: '50%',
                heightDiff: -5

            });
            $("#maingrid2").ligerGrid({
                columns: [
                    { display: 'ID', name: 'Btn_id', width: 350 },
                    { display: '名称', name: 'Btn_name', width: 150 },
                    { display: '菜单ID', name: 'Menu_id', width: 150 },
                    {
                        display: '图标', name: 'Btn_icon', width: 50, render: function (item) {
                            return "<img src='../../" + item.Btn_icon + "' style='width:16px;height:16px;margin-top:3px;'/>"
                        }
                    },
                    { display: '响应事件', name: 'Btn_handler', width: 100 },
                    { display: '排序', name: 'Btn_order', width: 60 }

                ],
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],

                url: "Sys_Button.GetGrid.xhd?menuid=-1",
                width: '100%',
                height: '100%',
                heightDiff: -5,
                onRClickToSelect: true
            });
            toolbar();



        });
        function toolbar() {

            var items = [];
            items.push({ type: 'button', text: '新增', icon: '../../images/icon/11.png', disable: true, click: add });
            items.push({ type: 'button', text: '修改', icon: '../../images/icon/33.png', disable: true, click: edit });
            items.push({ type: 'button', text: '删除', icon: '../../images/icon/12.png', disable: true, click: del });
            items.push({ type: 'button', text: '批量添加', icon: '../../images/icon/13.png', disable: true, click: batch });

            $("#toolbar").ligerToolBar({
                items: items

            });

            $("#maingrid2").ligerGetGridManager()._onResize();

        }


        function onSelect(note) {
            var manager = $("#maingrid1").ligerGetGridManager();
            // manager.showData({ Rows: [], Total: 0 });
            var url = "Sys_Menu.GetMenuV2.xhd?appid=" + note.data.id + "&rnd=" + Math.random();
            manager._setUrl(url);
        }

        function edit() {
            var row = $("#maingrid2").ligerGetGridManager().getSelectedRow();
            if (row) {
                f_openWindow('System/sysbase/Sys_Button_add.aspx?btnid=' + row.Btn_id + "&menuid=" + row.Menu_id, "修改按钮", 480, 380, f_save);
            }
            else {
                $.ligerDialog.warn('请选择按钮！');
            }
        }
        function add() {
            var manager = $("#maingrid1").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('System/sysbase/Sys_Button_add.aspx?menuid=' + row.Menu_id, "新增按钮", 480, 380, f_save);
            }
            else {
                $.ligerDialog.warn('请选择主菜单目录！');
            }
        }

        function del() {
            var manager = $("#maingrid2").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("删除后不能恢复，\n您确定要删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "Sys_Button.del.xhd",
                            data: { id: row.Btn_id },
                            success: function (result) {
                                treereload();
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }
        function batch() {
            var row = $("#maingrid1").ligerGetGridManager().getSelectedRow()
            if (row) {
                var menuid = row.Menu_id;
                var savetext0 = "Action=save&T_btn_name=%E6%96%B0%E5%A2%9E&T_btn_handler=add()&T_btn_icon=images/icon/11.png&T_btn_order=10&btnid=&menuid=" + menuid + "&rnd=" + Math.random();
                var savetext1 = "Action=save&T_btn_name=%E4%BF%AE%E6%94%B9&T_btn_handler=edit()&T_btn_icon=images/icon/33.png&T_btn_order=20&btnid=&menuid=" + menuid + "&rnd=" + Math.random();
                var savetext2 = "Action=save&T_btn_name=%E5%88%A0%E9%99%A4&T_btn_handler=del()&T_btn_icon=images/icon/12.png&T_btn_order=30&btnid=&menuid=" + menuid + "&rnd=" + Math.random();

                setTimeout(b_save(savetext0), 50);
                setTimeout(b_save(savetext1), 100);
                setTimeout(b_save(savetext2), 150);
            }
            else {
                $.ligerDialog.warn('请选择目录！');
            }
        }
        function b_save(issave) {
            if (issave) {
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Sys_Button.save.xhd", type: "get",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        treereload();

                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('操作失败！');
                    }
                });

            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Sys_Button.save.xhd", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        treereload();

                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('操作失败！');
                    }
                });

            }
        }

        function treereload() {
            var manager = $("#maingrid2").ligerGetGridManager();
            manager.loadData(true);
        }
    </script>
</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">

        <div id="layout1" style="margin-top: -1px; margin-left: -1px">
            <div position="left" title="主菜单模块">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center" title="子菜单">

                <div id="maingrid1" style="margin-top: -1px; margin-left: -1px"></div>
                <div id="toolbar" style="margin-top: 5px;"></div>
                <div id="maingrid2" style="margin-top: -1px; margin-left: -1px"></div>
            </div>
        </div>
    </form>
</body>
</html>
