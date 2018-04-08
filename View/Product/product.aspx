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
            $("#layout1").ligerLayout({ leftWidth: 180, allowLeftResize: false, allowLeftCollapse: true, space: 2, heightDiff: -5 });
            $("#tree1").ligerTree({
                url: 'Product_category.tree.xhd?qb=1&rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                //parentIDFieldName: 'pid',
                usericon: 'd_icon',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    //$(".l-first div:first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '产品名称', name: 'product_name', align: 'left', width: 120 },
                    { display: '产品类别', name: 'category_name', align: 'left', width: 120 },
                    { display: '条形码', name: 'BarCode', align: 'left', width: 160 },
                    {
                        display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                    {
                        display: '进货金价(￥)', name: 'StockPrice', width: 80, align: 'left', render: function (item) {
                            return toMoney(item.StockPrice);
                        }
                    },
                    {
                        display: '附工费(￥)', name: 'AttCosts', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.AttCosts);
                        }
                    },
                    {
                        display: '主石重', name: 'MainStoneWeight', width: 60, align: 'right', render: function (item) {
                            return toMoney(item.MainStoneWeight);
                        }
                    },
                    {
                        display: '附石重', name: 'AttStoneWeight', width: 60, align: 'right', render: function (item) {
                            return toMoney(item.AttStoneWeight);
                        }
                    },
                    {
                        display: '附石数', name: 'AttStoneNumber', width: 50, align: 'right', render: function (item) {
                            return toMoney(item.AttStoneNumber);
                        }
                    },
                    {
                        display: '石价(￥)', name: 'StonePrice', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.StonePrice);
                        }
                    },
                    {
                        display: '金价小计(￥)', name: 'GoldTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.GoldTotal);
                        }
                    },
                    {
                        display: '工费小计(￥)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.CostsTotal);
                        }
                    },
                    {
                        display: '成本总价(￥)', name: 'Totals', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.Totals);
                        }
                    },
                    {
                        display: '销售工费(￥)', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesCostsTotal);
                        }
                    },
                    {
                        display: '销售价格(￥)', name: 'SalesTotalPrice', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesTotalPrice);
                        }
                    },
                    { display: '供应商', name: 'supplier_name', width: 100 },
                     { display: '现存仓库', name: 'warehouse_name', width: 100, render: function (item) { if (item.warehouse_name == null) { return '总仓库'; } else { return item.warehouse_name; } } },
                    {
                        display: '状态', name: 'status', width: 80, align: 'right', render: function (item) {
                            switch (item.status) {
                                case 1:
                                    return "<span style='color:#0066FF'> 入库 </span>";
                                case 2:
                                    return "<span style='color:#00CC66'> 调拨中 </span>";
                                case 3:
                                    return "<span style='color:#009900'> 出库中 </span>";
                                case 4:
                                    return "<span style='color:#FF3300'> 已销售 </span>";
                            }
                        }
                    }

                ],
                dataAction: 'server',
                url: "Product.grid.xhd?categoryid=&rnd=" + Math.random(),
                pageSize: 30,
                pageSizeOptions: [10, 20, 30, 40, 50, 60, 80, 100, 120],
                width: '100%',
                height: '100%',
                heightDiff: -8,
                checkbox: true,
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
                items.push({ type: 'textbox', id: 'sstatus', text: '状态：' });
                items.push({ type: 'textbox', id: 'stext', text: '产品名：' });
                items.push({ type: 'textbox', id: 'scode', text: '条形码：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                $("#sstatus").ligerComboBox({
                    data: [
                    { text: '所有', id: '' },
                    { text: '入库', id: '1' },
                    { text: '调拨', id: '2' },
                    { text: '出库', id: '3' },
                    { text: '已销售', id: '4' }
                    ], valueFieldID: 'status',
                });
                $("#stext").ligerTextBox({ width: 200 });
                $("#scode").ligerTextBox({ width: 250 });
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
            var tdata = treemanager.getSelected();

            var cid = "";
            if (tdata != null && tdata.data != null) {
                cid = tdata.data.id;
            }
            serchtxt += "&categoryid=" + cid;
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
                f_openWindow('product/product_add.aspx?pid=' + rows.id, "修改产品", 700, 580, f_save);
            }
            else {
                $.ligerDialog.warn('请选择产品！');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            var categoryid = "";
            if (notes != null && notes != undefined) {
                categoryid = notes.data.id;
            }
            f_openWindow('product/product_add.aspx?categoryid=' + categoryid, "新增产品", 700, 580, f_save);
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

        function print() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getCheckedRows();
            if (rows == null || rows.length <= 0) {
                $.ligerDialog.warn("没有需要打印的产品");
                return;
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
                    <div id="treediv" style="width: 200px; height: 100%; margin: -1px; float: left; overflow: auto; margin-top: 2px;">
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
