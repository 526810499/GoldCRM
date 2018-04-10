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
                    { display: '类别名称', name: 'product_category', width: 250, align: 'left' },
                    {
                        display: '条形码头', name: 'CodingBegins', width: 50
                    }
                ],
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "Product_category.grid.xhd?grid=tree",
                width: '100%',
                height: '100%',
                tree: { columnName: 'product_category' },
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
            $.get("toolbar.GetSys.xhd?mid=product_category&rnd=" + Math.random(), function (data, textStatus) {
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
            f_openWindow("product/product_category_add.aspx", "新增类别", 480, 320, f_save);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('product/product_category_add.aspx?cid=' + row.id, "修改类别", 480, 320, f_save);
            } else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("产品类别删除不能恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Product_category.del.xhd", type: "POST",
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
                $.ligerDialog.warn("请选择产品类别！");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Product_category.save.xhd", type: "POST",
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
            <div></div>
        </div>
    </form>


</body>
</html>
