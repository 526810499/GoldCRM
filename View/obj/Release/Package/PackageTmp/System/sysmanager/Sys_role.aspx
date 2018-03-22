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
                    //{ display: 'ID', name: 'RoleID', type: 'int', width: 50 },
                    //{ display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    { display: '角色名', name: 'RoleName',width:200 },
                    { display: '角色描述', name: 'RoleDscript', width: 450 },
                    { display: '排序', name: 'RoleSort', width: 50 }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                url: "Sys_role.grid.xhd",
                width: '100%',
                height: '100%',
                heightDiff: -11,
                onRClickToSelect: true,
                rownumbers:true,
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
            $.get("toolbar.GetSys.xhd?mid=Sys_role&rnd=" + Math.random(), function (data, textStatus) {
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
                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };
        function authorized() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                top.$.ligerDialog.open({
                    width: 800, height: 500, showToggle: true, url: 'System/sysmanager/Sys_role_authorized.aspx?Role_id=' + row.id, title: "授权", buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                dialog.frame.f_save();
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                    ]
                });
            }
            else {
                $.ligerDialog.warn("请选择行");
            }

        }
        

        function add() {
            f_openWindow("System/sysmanager/Sys_role_add.aspx", "新增角色", 700, 350,f_save);
            //tt = $("#toolbar").ligerGetToolBarManager();

            //alert(tt.getItemIdList());
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('System/sysmanager/Sys_role_add.aspx?id=' + row.id, "修改角色", 700, 350,f_save);
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("角色删除后不能恢复，\n您确定要移除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "Sys_role.del.xhd", /* 注意后面的名字对应CS的方法名称 */
                            data: { id: row.id }, /* 注意参数的格式和名称 */
                            dataType:'json',
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

        function role_emp() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                top.$.ligerDialog.open({
                    width: 700, height: 500, showToggle: true, url: 'system/sysmanager/Role_emp.aspx?rid=' + row.id, title: "角色人员管理", buttons: [
                        //{ text: '确定', onclick: f_importOK },
                        { text: '关闭', onclick: f_importCancel }
                    ]
                });
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }
        function data_authorized() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                top.$.ligerDialog.open({
                    width: 600, height: 500, showToggle: true, url: 'system/sysmanager/Sys_data_authorized.aspx?Role_id=' + row.id, title: "数据权限管理", buttons: [
                        { text: '确定', onclick: f_importOK },
                        { text: '取消', onclick: f_importCancel }
                    ]
                });
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }
        function f_importOK(item, dialog) {
            var isSave = dialog.frame.f_save();
            if (isSave == "true") {
                dialog.close();
            }
        }
        function f_importCancel(item, dialog) {
            dialog.close();
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Sys_role.SysSave.xhd", type: "POST",
                    data: issave,
                    dataType:'json',
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
