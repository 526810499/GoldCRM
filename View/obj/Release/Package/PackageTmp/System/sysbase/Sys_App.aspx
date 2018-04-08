<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#maingrid4").ligerGrid({
                columns: [
                   { display: 'ID', name: 'id', type: 'int', width: 280, align: 'left' },
                    { display: '菜单名', name: 'App_name', width: 250, align: 'left' },
                    {
                        display: '图标', name: 'App_icon', width: 250, render: function (item) {
                            return "<img style='width:16px;height:16px;margin-top:8px;' src='../../" + item.App_icon + "'/>";
                        }
                    },
                    { display: '排序', name: 'App_order' }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                url: "Sys_App.GridData.xhd",
                width: '100%',
                height: '100%',
                heightDiff: -11,
                onRClickToSelect: true,
                rownumbers: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            toolbar();
        });
        function toolbar() {
            var items = [];
            items.push({ type: 'button', text: '新增', icon: '../../images/icon/11.png', disable: true, click: add });
            items.push({ type: 'button', text: '修改', icon: '../../images/icon/33.png', disable: true, click: edit });
            items.push({ type: 'button', text: '删除', icon: '../../images/icon/12.png', disable: true, click: del });

            $("#toolbar").ligerToolBar({
                items: items

            });
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

 
        function add() {
            f_openWindow("System/sysbase/Sys_App_Add.aspx", "新增主菜单", 700, 350, f_save);

        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('System/sysbase/Sys_App_Add.aspx?id=' + row.id, "修改主菜单", 700, 350, f_save);
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("主菜单删除后不能恢复，\n您确定要移除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "Sys_App.delete.xhd", /* 注意后面的名字对应CS的方法名称 */
                            data: { id: row.id }, /* 注意参数的格式和名称 */
                            dataType: 'json',
                            success: function (result) {
                                $.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_reload();
                                }
                                else {
                                    $.ligerDialog.error(obj.Message);
                                }
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Sys_App.save.xhd", type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        top.$.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_reload();
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
    </script>
</head>
<body>
    <form id="mainform" onsubmit="return false">
        <div style="padding: 10px;">
            <div id="toolbar"></div>
            <div id="grid" style="">
                <div id="maingrid4" style="margin: -1px;"></div>
            </div>
        </div>
    </form>
</body>
</html>
