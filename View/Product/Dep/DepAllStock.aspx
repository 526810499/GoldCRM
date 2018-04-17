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
    <script src="../../JS/XHD.js?v=4" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var ExtenData;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            var columns = [
                   {
                       display: '条形码', name: 'BarCode', align: 'left', width: 160, render: function (item) {
                           var html = "<a href='javascript:void(0)' onclick=view('depproduct','" + item.id + "')>" + item.BarCode + "</a>";
                           return html;
                       }
                   },
                   {
                       display: '商品名称', name: 'product_name', align: 'left', width: 120
                   },
                   { display: '商品类别', name: 'category_name', align: 'left', width: 120, totalSummary: { type: 'total', render: function (item) { return "<span id='tspan'>合计:</span>"; } } },

                   {
                       display: '重量(克)', name: 'Weight', width: 80, align: 'left', render: function (item) {
                           return toMoney(item.Weight);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='Weight'>" + item.sum + "</span>"; } }
                   },
                   {
                       display: '主石重', name: 'MainStoneWeight', width: 60, align: 'right', render: function (item) {
                           return toMoney(item.MainStoneWeight);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='MainStoneWeight'>" + item.sum + "</span>"; } }
                   },
                   {
                       display: '附石重', name: 'AttStoneWeight', width: 60, align: 'right', render: function (item) {
                           return toMoney(item.AttStoneWeight);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='AttStoneWeight'>" + item.sum + "</span>"; } }
                   },
                   {
                       display: '附石数', name: 'AttStoneNumber', width: 50, align: 'right', render: function (item) {
                           return toMoney(item.AttStoneNumber);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='AttStoneNumber'>" + item.sum + "</span>"; } }
                   },
                   {
                       display: '工费小计(￥)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                           return toMoney(item.CostsTotal);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='CostsTotal'>" + item.sum + "</span>"; } }
                   },
                    { display: '现存仓库', name: 'warehouse_name', width: 120, render: function (item) { if (item.warehouse_name == null) { return '总仓库'; } else { return item.warehouse_name; } } },
                   {
                       display: '状态', name: 'status', width: 80, align: 'right', render: function (item) {
                           switch (item.OutStatus) {
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
            ];


            $("#maingrid4").ligerGrid({
                columns: columns,
                dataAction: 'server',
                url: "Product.grid.xhd?sum=1&depdata=1&rnd=" + Math.random(),
                pageSize: 30,
                pageSizeOptions: [20, 30, 40, 60, 100, 120, 200],
                width: '100%',
                height: '100%',
                heightDiff: -8,
                checkbox: false,
                onSuccess: function (data, grid) {
                    if (data != null && data.Exten != null) {
                        ExtenData = data.Exten[0];
                    }
                },
                onAfterShowData: function (grid) {
                    for (var n in ExtenData) {
                        $("#" + n).text(ExtenData[n]);
                    }
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
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=depstockall&rnd=" + Math.random(), function (data, textStatus) {
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
                    icon: '../../images/search.gif',
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
                $("#sdep").ligerComboBox({
                    width: 150,
                    selectBoxWidth: 240,
                    selectBoxHeight: 200,
                    valueField: 'id',
                    textField: 'text',
                    treeLeafOnly: false,
                    tree: {
                        url: 'hr_department.tree.xhd?qxz=1&rnd=' + Math.random(),
                        idFieldName: 'id',
                        checkbox: false
                    },
                });
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
            var sendtxt = "&depdata=1&sum=1&rnd=" + Math.random();
            var serchtxt = "scode=" + $("#scode").val();
            serchtxt += "&stext=" + $("#stext").val();
            serchtxt += "&soutstatus=" + $("#status").val();
            serchtxt += "&categoryid=" + $("#sfl_val").val();
            serchtxt += "&warehouse_id=" + $("#sck_val").val();
            serchtxt += sendtxt;
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product.grid.xhd?" + serchtxt);
        }


        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                f_openWindow('product/product_View.aspx?depdata=1&pid=' + rows.id, "查看商品", 700, 580);
            }
            else {
                $.ligerDialog.warn('请选择商品！');
            }
        }
        function add() {

            // f_openWindow('product/product_add.aspx?categoryid=', "新增商品", 700, 580, f_save);
        }


        function f_load() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
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
                        <input type='select' id='sfl' name='sfl' ltype='text' ligerui='{width:120}' /></td>
                    <td>
                        <div style='width: 40px; text-align: right; float: right'>仓库：</div>
                    </td>
                    <td>
                        <div style='float: left'>
                            <input type='select' id='sck' name='sck' ltype='date' ligerui='{width:120}' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 80px; text-align: right; float: right'>订单状态：</div>
                    </td>
                    <td>
                        <input id='sstatus' name="sstatus" type='text' /></td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>商品名：</div>
                    </td>
                    <td>


                        <input type='text' id='stext' name='stext' />

                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>条形码：</div>
                    </td>
                    <td>

                        <input type='text' id='scode' name='scode' />

                    </td>
                    <td></td>
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
