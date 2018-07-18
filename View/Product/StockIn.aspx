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
    <script src="../../JS/XHD.js?v=91" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    {
                        display: '入库单号', name: 'id', align: 'left', width: 300, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=PView('" + item.id + "'," + item.status + ")>" + item.id + "</a>";
                            return html;
                        }
                    },
                    //{ display: '入库仓库', name: 'product_warehouse', align: 'left', width: 200 },
                    //{ display: '入库门店', name: 'dep_name', align: 'left', width: 120 },
                    { display: '创建人', name: 'CreateName', align: 'left', width: 120 },
                    {
                        display: '创建时间', name: 'create_time', width: 150, align: 'left', render: function (item) {
                            return formatTime(item.create_time, 'yyyy-MM-dd HH:mm:ss');
                        }
                    },
                    {
                        display: '状态', name: 'status', width: 100, align: 'left', render: function (item) {
                            switch (item.status) {
                                case 0:
                                    return "<span style='color:#0066FF'> 未保存提交 </span>";
                                case 1:
                                    return "<span style='color:#00CC66'> 已保存并入库 </span>";
                            }
                        }
                    },
                    { display: '备注', name: 'remark', width: 120 }

                ],
                dataAction: 'server',
                url: "Product_StockIn.grid.xhd?intype=0&rnd=" + Math.random(),
                pageSize: 30,
                pageSizeOptions: [10, 20, 30, 40, 50, 60, 80, 100, 120],
                width: '100%',
                height: '100%',
                heightDiff: -8,
                detail: {
                    height: 'auto',
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        var grid = document.createElement('div');
                        $(p).append(grid);
                        $(grid).css('margin', 3).ligerGrid({
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
                               display: '单价(￥)', name: 'price', width: 80, align: 'right', render: function (item) {
                                   return toMoney(item.price);
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
                                display: '单价(￥)', name: 'price', width: 80, align: 'right', render: function (item) {
                                    return toMoney(item.price);
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
                             {
                                 display: '一口价', name: 'FixedPrice', width: 120, render: function (item) { if (item.FixedPrice == null) { return '0'; } else { return toMoney(item.FixedPrice); } }
                             },

                          {
                              display: '当前状态', name: 'status', width: 80, align: 'right', render: function (item) {
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
                            url: "Product.StockGridDetail.xhd?stockid=" + r.id + "&rnd=" + Math.random(),
                            pageSize: 30,
                            pageSizeOptions: [10, 20, 30, 40, 50, 60, 80, 100, 120],
                            width: '99%', height: '180',
                            heightDiff: 0,
                            checkbox: false,
                            //isChecked: f_isChecked,
                            //onCheckRow: f_onCheckRow,
                            //onCheckAllRow: f_onCheckAllRow,
                            onReload: function () {
                                checkedCustomer = [];
                            },
                            onContextmenu: function (parm, e) {
                                //actionCustomerID = parm.data.id;
                                //menu.show({ top: e.pageY, left: e.pageX });
                                //return false;
                            }
                        })

                    }
                },
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#btn_serch").ligerButton({ text: "搜索", width: 60, click: doserch });

            toolbar();

        });

        function ViewModel(tag, id) {
            f_openWindow('product/product_add.aspx?pid=' + id, "修改商品", 700, 720, product_save);
        }

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=product_list&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);

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



                //items.push({ type: 'textbox', id: 'sstatus', text: '状态：' });
                //items.push({ type: 'textbox', id: 'sorderid', text: '单号：' });
                //items.push({ type: 'textbox', id: 'scode', text: '条形码：' });
                //items.push({ type: 'textbox', id: 'sck', text: '盘点仓库：' });
                //items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items
                });
                $("#sorderid").ligerTextBox({ width: 200 });
                $("#scode").ligerTextBox({ width: 250 });
                $("#sstatus").ligerComboBox({
                    data: [
                    { text: '所有', id: '' },
                    { text: '未提交', id: '0' },
                    { text: '提交保存', id: '1' }
                    ], valueFieldID: 'status',
                });
                $("#swarehouse_id").ligerComboBox({
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


        function ExcelDC() {

            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                location.href = "ExportProduct.aspx?etype=2&stockid=" + row.id + "&rnd=" + Math.random();
            }
            else {
                $.ligerDialog.warn("请选择入库单");
            }
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

        function onSelect(note) {
            doserch();
        }
        //查询
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = "intype=0&status=" + $("#status").val();
            serchtxt += "&sorderid=" + $("#sorderid").val();
            serchtxt += "&scode=" + $("#scode").val();
            //serchtxt += "&swarehouse_id=" + $("#swarehouse_id_val").val();
            serchtxt += "&sbegtime=" + $("#sbegtime").val();
            serchtxt += "&sendtime=" + $("#sendtime").val();
            sendtxt += sendtxt;
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product_StockIn.grid.xhd?" + serchtxt);
        }

        //重置
        function doclear() {

            $("#form1").each(function () {
                this.reset();
            });
        }


        function PView(id, status) {
            var buttons = [];
            if (status != 1) {
                buttons.push({ text: '保存并入库', onclick: f_saveAuth });
            }
            f_openWindow2('product/StockIn_Add.aspx?id=' + id + "&status=" + status, "查看入库单", 1050, 680, buttons);

        }

        function product_save(item, dialog) {
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
        function product_load() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }


        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                var buttons = [];
                if (rows.status != 1) {
                    buttons.push({ text: '保存并入库', onclick: f_saveAuth });
                }
                f_openWindow2('product/StockIn_Add.aspx?id=' + rows.id + "&status=" + rows.status, "修改入库单", 1050, 680, buttons);
            }
            else {
                $.ligerDialog.warn('请选择入库单！');
            }
        }
        function add() {
            var oid = "addtemp";
            $.ajax({
                url: "Product_StockIn.CheckHQAddOrder.xhd?intype=0", type: "Get",
                data: { rnd: Math.random() },
                async: false,
                success: function (result) {
                    if (result != null && result.length > 0) {
                        $.ligerDialog.confirm("上笔入库单未完成是否继续上笔订单操作,否则上笔入库单将回收删除", function (yes) {
                            if (yes) {
                                oid = result;
                                AddOrder(oid);
                            } else {
                                $.ajax({
                                    url: "Product_StockIn.TempDel.xhd?intype=0", type: "POST",
                                    data: { id: result, rnd: Math.random() },
                                    dataType: 'json',
                                    success: function (result) {
                                        var obj = eval(result);
                                        if (obj.isSuccess) {
                                            f_load();
                                        }
                                    },
                                    error: function () {

                                    }
                                });
                            }
                        });
                    } else {
                        AddOrder(oid);
                    }
                },
                error: function () {

                }
            });

        }
        function AddOrder(oid) {

            var buttons = [];
            buttons.push({ text: '保存并入库', onclick: f_saveAuth });
            f_openWindow2('product/StockIn_Add.aspx?id=' + oid, "新增入库单", 1050, 680, buttons, 9002, false, f_load);
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                var msg = "入库单删除不能恢复已入库商品也会删除，确定删除？";
                if (row.status == 1) {
                    $.ligerDialog.warn("已提交入库不能删除");
                    return false;
                }

                $.ligerDialog.confirm(msg, function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Product_StockIn.del.xhd?intype=0", type: "POST",
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
                });
            }
            else {
                $.ligerDialog.warn("请选择入库单");
            }

        }

        function f_saveAuth(item, dialog) {
            Save(item, dialog, 1);
        }

        function f_save(item, dialog) {
            Save(item, dialog, 0);
        }

        function Save(item, dialog, auth) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Product_StockIn.HQSave.xhd?intype=0&auth=" + auth, type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            if (obj.Message != null && obj.Message != undefined) {
                                $.ligerDialog.warn(obj.Message);
                            }
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
                        <div style='width: 40px; text-align: right; float: right'>单号：</div>
                    </td>
                    <td>
                        <div style='float: left'>
                            <input type='text' id='sorderid' name='sorderid' ltype='text' ligerui='{width:120}' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 80px; text-align: right; float: right'>条形码：</div>
                    </td>
                    <td>
                        <input id='scode' name="scode" type='text' /></td>
                    <%--                    <td>
                        <div style='width: 60px; text-align: right; float: right'>盘点仓库：</div>
                    </td>
                    <td>
                        <input type='select' id='swarehouse_id' name='swarehouse_id' ltype='text' ligerui='{width:120}' />

                    </td>--%>


                    <td>
                        <div style='width: 80px; text-align: right; float: right'>状态：</div>
                    </td>
                    <td>
                        <input type='select' id='sstatus' name='sstatus' ltype='text' ligerui='{width:120}' /></td>
                    <td>
                        <div style='width: 80px; text-align: right; float: right'>时间：</div>
                    </td>
                    <td>
                        <input type='text' id='sbegtime' name='sbegtime' />

                    </td>
                    <td>~  
                    </td>
                    <td>
                        <input type='text' id='sendtime' name='sendtime' />
                    </td>

                    <td>
                        <div id="btn_serch"></div>
                        <div id="btn_reset"></div>
                    </td>
                </tr>

            </table>
        </form>
    </div>


</body>
</html>
