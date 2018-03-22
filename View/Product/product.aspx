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
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 200, allowLeftResize: false, allowLeftCollapse: true, space: 2, heightDiff: -5 });
            $("#tree1").ligerTree({
                url: 'Product_category.tree.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                //parentIDFieldName: 'pid',
                usericon: 'd_icon',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first div:first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    { display: '产品名称', name: 'product_name', width: 120 },
                    { display: '产品类别', name: 'category_name', width: 120 },
                    { display: '产品规格', name: 'specifications', width: 120 },
                    {
                        display: '成本价（￥）', name: 'cost', width: 120, align: 'right', render: function (item) {
                            return toMoney(item.cost);
                        }
                    },
                    {
                        display: '报价（￥）', name: 'price', width: 120, align: 'right', render: function (item) {
                            return toMoney(item.price);
                        }
                    },
                    {
                        display: '销售价（￥）', name: 'agio', width: 120, align: 'right', render: function (item) {
                            return toMoney(item.agio);
                        }
                    },
                    { display: '单位', name: 'unit', width: 120 },
                    { display: '备注', name: 'remarks', width: 120 }

                ],
                dataAction: 'server',
                url: "Product_category.grid.xhd?categoryid=0&rnd=" + Math.random(),
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                width: '100%',
                height: '100%',
                heightDiff: -8,

                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
            toolbar();

        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=product_list&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'stext', text: '产品名：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                $("#stext").ligerTextBox({ width: 200 });
                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }


        function onSelect(note) {
            var manager = $("#maingrid4").ligerGetGridManager();
            var url = "Product.grid.xhd?categoryid=" + note.data.id + "&rnd=" + Math.random();
            manager._setUrl(url);
        }
        //查询
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;

            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product.grid.xhd?" + serchtxt);
        }

        //重置
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#form1").each(function () {
                this.reset();
            });
        }



        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                f_openWindow('product/product_add.aspx?pid=' + rows.id, "修改产品", 700, 380, f_save);
            }
            else {
                $.ligerDialog.warn('请选择产品！');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();

            if (notes != null && notes != undefined) {
                f_openWindow('product/product_add.aspx?categoryid=' + notes.data.id, "新增产品", 700, 380, f_save);
            }
            else {
                $.ligerDialog.warn('请选择产品类别！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("产品删除不能恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Product.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
                            dataType: 'json',
                            success: function (result) {
                                $.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_load();
                                }
                                else {
                                    $.ligerDialog.error(obj.Message);
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
            else {
                $.ligerDialog.warn("请选择产品");
            }

        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Product.save.xhd", type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_load();
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }
                        //f_load();     
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('操作失败！');
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
    <div style="padding: 5px 10px 0px 5px;">
        <form id="form1" onsubmit="return false">
            <div id="layout1" style="">
                <div position="left" title="产品类别">
                    <div id="treediv" style="width: 200px; height: 100%; margin: -1px; float: left; overflow: auto;margin-top:2px;">
                        <ul id="tree1"></ul>
                    </div>
                </div>
                <div position="center">
                    <div id="toolbar" style="margin-top: 10px;"></div>
                    <div id="maingrid4" style="margin: -1px;"></div>

                </div>
            </div>
        </form>

    </div>
</body>
</html>
