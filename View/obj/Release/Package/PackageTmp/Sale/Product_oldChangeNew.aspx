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
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#maingrid4").ligerGrid({
                columns: [
                     { display: '订单ID', name: 'id', width: 250, align: 'left' },
                    {
                        display: '旧金重', name: 'oldWeight', width: 150, align: 'left', render: function (item) {
                            return toMoney(item.oldWeight);
                        }
                    },
                    {
                        display: '旧金价值', name: 'oldTotalPrice', width: 150, render: function (item) {
                            return toMoney(item.oldTotalPrice);
                        }
                    },
                    {
                        display: '旧金折旧费', name: 'oldCharge', width: 150, render: function (item) {
                            return toMoney(item.oldCharge);
                        }
                    }
                     ,
                    {
                        display: '新金重', name: 'newWeight', width: 150, render: function (item) {
                            return toMoney(item.newWeight);
                        }
                    },
                    {
                        display: '新金价值', name: 'newTotalPrice', width: 150, render: function (item) {
                            return toMoney(item.newTotalPrice);
                        }
                    },
                    {
                        display: '工费', name: 'costsTotalPrice', width: 150, render: function (item) {
                            return toMoney(item.costsTotalPrice);
                        }
                    }
                     ,
                    {
                        display: '折扣', name: 'discount', width: 150, render: function (item) {
                            return toMoney(item.discount);
                        }
                    },
                    {
                        display: '需补费用', name: 'difTotalPrice', width: 150, render: function (item) {
                            return toMoney(item.difTotalPrice);
                        }
                    },
                    {
                        display: '创建时间', name: 'create_time', width: 200, render: function (item) {
                            return formatTime(item.create_time);
                        }
                    }
                            ,
                    {
                        display: '备注', name: 'remark', width: 250,
                    }
                ],
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "SProduct_OldChangeNew.grid.xhd?grid=tree",
                width: '100%',
                height: '100%',
                heightDiff: -10,
                onRClickToSelect: true,

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
            $.get("toolbar.GetSys.xhd?mid=product_oldchangenew&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
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

                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }

        function add() {
            f_openWindow("Sale/Product_oldChangeNew_add.aspx?id=", "新增以旧换新", 680, 420, f_save);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                console.log("row", row);
                f_openWindow('Sale/Product_oldChangeNew_add.aspx?id=' + row.id, "修改以旧换新", 680, 420, f_save);
            } else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("删除不能恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "SProduct_OldChangeNew.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
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
                            },
                            error: function () {
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('删除失败！', "", null, 9003);
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择信息！");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "SProduct_OldChangeNew.save.xhd", type: "POST",
                    data: issave,
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
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();

                    }
                });

            }
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };
    </script>
    <style type="text/css">
        .l-leaving {
            background: #eee;
            color: #999;
        }
    </style>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div style="padding: 10px;">
            <div id="toolbar"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
