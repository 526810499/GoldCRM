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
        $(function () {

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'id', type: 'int', width: 50 },
                    //{ display: '序号', width: 50, render: function (item, i) { return item.n; } },
                    { display: '职务', name: 'position_name', width:150 },
                    { display: '级别', name: 'position_level', width: 70 },
                    { display: '行号', name: 'position_order', width: 70 }

                ],
                rownumbers: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                onSelectRow: function (row, index, data) {
                    //alert('onSelectRow:' + index + " | " + data.ProductName); 
                },
                url: "hr_position.grid.xhd",
                width: '100%',
                height: '100%',
                heightDiff: -11,
                onRClickToSelect: true,
                
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            toolbar();

            
        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=hr_position&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                var manager = $("#maingrid4").ligerGetGridManager();
                manager._onResize();
            });
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };        

        function add() {
            f_openWindow('hr/hr_position_add.aspx', "新建职务", 390, 200,f_save);
        }
        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('hr/hr_position_add.aspx?pid=' + row.id, "修改职务", 390, 200,f_save);
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
                            url: "hr_position.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
                            dataType: "json",
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
                                top.$.ligerDialog.error('删除失败！');
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择行！");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "hr_position.save.xhd", type: "POST",
                    data: issave,
                    dataType: "json",
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

    <form id="form1" onsubmit="return false">
        <div style="padding:10px;">
            <div id="toolbar"></div>
            
            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
