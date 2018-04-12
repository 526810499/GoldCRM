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
                    { display: 'ID', name: 'id', width: 50 },
                    { display: '提醒员工', name: 'userName', width: 100 },
                    {
                        display: '业务类型', name: 'remindType', width: 150, render: function (item) {
                            if (item.remindType == 1) { return "客户生日提醒"; } else { return "未定义" + item.remindType; }
                        }
                    },
                    {
                        display: '创建时间', name: 'create_time', width: 150, render: function (item) {
                            return formatTimebytype(item.create_time, 'yyyy-MM-dd hh:mm:ss');
                        }
                    },
                    { display: '备注', name: 'remark', width: 450 },
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                url: "BusinessToRemindUserConfig.grid.xhd",
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
            $.get("toolbar.GetSys.xhd?mid=businesstoremind&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'username', text: '员工：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                $("#username").ligerTextBox({ width: 200 });
                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }

        function doserch() {
            var serchtxt = "rnd=" + Math.random();
            serchtxt += "and username=" + urlencode($("#username").val());
            manager._setUrl("BusinessToRemindUserConfig.grid.xhd?" + serchtxt);
        }

        function add() {
            f_openWindow("System/OtherConfig/BusinessToRemind_Add.aspx", "新增配置", 800, 650, f_save);

        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('System/OtherConfig/BusinessToRemind_Add.aspx?id=' + row.id, "修改配置", 800, 650, f_save);
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("删除后不能恢复，\n您确定要移除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "BusinessToRemindUserConfig.del.xhd", /* 注意后面的名字对应CS的方法名称 */
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
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };


        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "BusinessToRemindUserConfig.save.xhd", type: "POST",
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
