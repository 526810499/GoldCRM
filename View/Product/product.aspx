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
    <script src="../JS/XHD.js?v=6" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">
        var checkedCustomer = [];
        var manager = "";
        var updateBtn = false;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    {
                        display: '条形码', name: 'BarCode', align: 'left', width: 160, render: function (item) {

                            var html = "<a href='javascript:void(0)' onclick=ViewModel('product','" + item.id + "')>" + item.BarCode + "</a>";
                            if ($("div"))
                                return html;
                        }
                    },
                    {
                        display: '商品名称', name: 'product_name', align: 'left', width: 120
                    },
                    {
                        display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                   { display: '商品类别', name: 'category_name', align: 'left', width: 120 },
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
                            return GetproductStatus(item.status);
                        }
                    },
                    {
                        display: '审核状态', name: 'authIn', width: 80, align: 'right', render: function (item) {
                            switch (item.authIn) {
                                case 101:
                                    return "<span style='color:#0066FF'> 调拨审核中 </span>";
                                case 102:
                                    return "<span style='color:#00CC66'> 出库审核中 </span>";
                                default:
                                    return "正常";
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
                isChecked: f_isChecked,
                onCheckRow: f_onCheckRow,
                onCheckAllRow: f_onCheckAllRow,
                onReload: function () {
                    checkedCustomer = [];
                },
                onContextmenu: function (parm, e) {
                    //actionCustomerID = parm.data.id;
                    //menu.show({ top: e.pageY, left: e.pageX });
                    //return false;
                }
            });

            $("#btn_serch").ligerButton({ text: "搜索", width: 60, click: doserch });


            toolbar();

        });

        function ViewModel(tag, id) {
            if (updateBtn) {
                f_openWindow('product/product_add.aspx?pid=' + id, "修改商品", 700, 580, f_save);
            } else {
                f_openWindow('product/product_add.aspx?pid=' + id, "查看商品", 700, 580);
            }
        }

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=product_list&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].text != "修改") {
                        arr[i].icon = "../" + arr[i].icon;
                        items.push(arr[i]);
                    } else {
                        updateBtn = true;
                    }
                }

                items.push({
                    id: "sbtn",
                    type: 'serchbtn',
                    text: '搜索',
                    icon: '../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });

                $("#toolbar").ligerToolBar({
                    items: items
                });

                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#sstatus").ligerComboBox({
                    data: productStatus, valueFieldID: 'status',
                });
                $("#stext").ligerTextBox({ width: 240 });
                $("#scode").ligerTextBox({ width: 240 });
                $("#sfl").ligerComboBox({
                    width: 150,
                    selectBoxWidth: 240,
                    selectBoxHeight: 200,
                    valueField: 'id',
                    textField: 'text',
                    treeLeafOnly: false,
                    tree: {
                        url: 'Product_category.tree.xhd?qxz=1&rnd=' + Math.random(),
                        idFieldName: 'id',
                        checkbox: false
                    },
                });
                $("#sck").ligerComboBox({
                    width: 150,
                    selectBoxWidth: 240,
                    selectBoxHeight: 200,
                    valueField: 'id',
                    textField: 'text',
                    treeLeafOnly: false,
                    tree: {
                        url: 'Product_warehouse.tree.xhd?zb=1&qxz=1&rnd=' + Math.random(),
                        idFieldName: 'id',
                        checkbox: false
                    },
                });
                $("#sbegtime").ligerDateEditor({ showTime: true, labelWidth: 100, labelAlign: 'left' });
                $("#sendtime").ligerDateEditor({ showTime: true, labelWidth: 100, labelAlign: 'left' });
                $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
                $('#serchform').ligerForm();
                $("div[toolbarid='sbtn']").click().hide();

                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }

        function serchpanel() {

            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
        }
        //查询
        function doserch() {
            checkedCustomer = [];
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = "scode=" + $("#scode").val();
            serchtxt += "&stext=" + $("#stext").val();
            serchtxt += "&status=" + $("#status").val();
            serchtxt += "&categoryid=" + $("#sfl_val").val();
            serchtxt += "&sbegtime=" + $("#sbegtime").val();
            serchtxt += "&sendtime=" + $("#sendtime").val();
            serchtxt += "&warehouse_id=" + $("#sck_val").val();
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product.grid.xhd?" + serchtxt);
        }


        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                f_openWindow('product/product_add.aspx?pid=' + rows.id, "修改商品", 700, 580, f_save);
            }
            else {
                $.ligerDialog.warn('请选择商品！');
            }
        }
        function add() {

            f_openWindow('product/product_add.aspx?categoryid=', "新增商品", 700, 580, f_save);
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("商品删除不能恢复，确定删除？", function (yes) {
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
                $.ligerDialog.warn("请选择商品");
            }

        }


        function ExcelDC() {

            if (checkedCustomer == null || checkedCustomer.length <= 0) {
                $.ligerDialog.warn("没有需要导出的商品");
                return;
            }
            var ids = "";
            $(checkedCustomer).each(function (i, v) {
                ids += v + ",";
            });

            location.href = "ExportProduct.aspx?ids=" + ids + "&rnd=" + Math.random();


        }



        function prints() {

            if (checkedCustomer == null || checkedCustomer.length <= 0) {
                $.ligerDialog.warn("没有需要打印的商品");
                return;
            }
            var ids = "";
            $(checkedCustomer).each(function (i, v) {
                ids += v + ",";
            });

            window.open("PrintProduct.aspx?ids=" + ids + "&rnd=" + Math.random());
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

        function f_onCheckAllRow(checked) {
            for (var rowid in this.records) {
                if (checked)
                    addCheckedCustomer(this.records[rowid]['id']);
                else
                    removeCheckedCustomer(this.records[rowid]['id']);
            }
        }


        function findCheckedCustomer(id) {
            for (var i = 0; i < checkedCustomer.length; i++) {
                if (checkedCustomer[i] == id) return i;
            }
            return -1;
        }
        function addCheckedCustomer(id) {
            if (findCheckedCustomer(id) == -1)
                checkedCustomer.push(id);
        }
        function removeCheckedCustomer(id) {
            var i = findCheckedCustomer(id);
            if (i == -1) return;
            checkedCustomer.splice(i, 1);
        }
        function f_isChecked(rowdata) {
            if (findCheckedCustomer(rowdata.id) == -1)
                return false;
            return true;
        }
        function f_onCheckRow(checked, data) {
            if (checked) addCheckedCustomer(data.id);
            else removeCheckedCustomer(data.id);
        }

    </script>
</head>
<body>

    <form id="form1" onsubmit="return false">

        <div style="padding: 10px;">
            <div id="toolbar" style="margin-top: 10px;"></div>
            <div id="grid">
                <div id="maingrid4" style="margin: -1px;"></div>
            </div>
        </div>

    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 720px'>
                <tr>
                    <td>
                        <div style='width: 40px; text-align: right; float: right'>分类：</div>
                    </td>
                    <td>
                        <input type='select' id='sfl' name='sfl' ltype='text' ligerui='{width:120}' />

                    </td>
                    <td>
                        <div style='width: 40px; text-align: right; float: right'>仓库：</div>
                    </td>
                    <td>
                        <div style='float: left'>
                            <input type='select' id='sck' name='sck' ltype='date' ligerui='{width:120}' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 80px; text-align: right; float: right'>状态：</div>
                    </td>
                    <td>
                        <input id='sstatus' name="sstatus" type='text' />

                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>入库时间：</div>
                    </td>
                    <td>
                        <input type='text' id='sbegtime' name='sbegtime' />

                    </td>
                    <td>~  
                    </td>
                    <td>
                        <input type='text' id='sendtime' name='sendtime' />
                    </td>
                    <td style="width: 20px"></td>
                </tr>
                <tr>
                    <td colspan="11" style="height: 10px"></td>
                </tr>
                <tr>
                    <td colspan="11" style="margin-top: 10px">
                        <table>
                            <tr>
                                <td>条形码： 
                                </td>
                                <td>
                                    <input type='text' id='scode' name='scode' />
                                </td>
                                <td style="width: 114px; text-align: right">商品名： 
                                </td>
                                <td>
                                    <input type='text' id='stext' name='stext' />
                                </td>

                                <td>
                                    <div id="btn_serch"></div>
                                    <div id="btn_reset"></div>
                                </td>
                            </tr>
                        </table>
                    </td>


                </tr>

            </table>
        </form>
    </div>
</body>
</html>
