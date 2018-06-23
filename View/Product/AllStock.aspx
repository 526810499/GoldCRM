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
                           var html = "<a href='javascript:void(0)' onclick=view('product','" + item.id + "')>" + item.BarCode + "</a>";
                           return html;
                       }
                   },
                   {
                       display: '商品名称', name: 'product_name', align: 'left', width: 120
                   },
                   { display: '商品类别', name: 'category_name', align: 'left', width: 120, totalSummary: { type: 'total', render: function (item) { return "<span id='tspan'>合计:</span>"; } } },

                   {
                       display: '重量(克)', name: 'Weight', width: 60, align: 'left', render: function (item) {
                           return toMoney(item.Weight);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='Weight'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '进货金价(￥)', name: 'StockPrice', width: 80, align: 'left', render: function (item) {
                           return toMoney(item.StockPrice);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='StockPrice'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '附工费(￥)', name: 'AttCosts', width: 80, align: 'right', render: function (item) {
                           return toMoney(item.AttCosts);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='AttCosts'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '主石重', name: 'MainStoneWeight', width: 60, align: 'right', render: function (item) {
                           return toMoney(item.MainStoneWeight);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='MainStoneWeight'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '附石重', name: 'AttStoneWeight', width: 60, align: 'right', render: function (item) {
                           return toMoney(item.AttStoneWeight);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='AttStoneWeight'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '附石数', name: 'AttStoneNumber', width: 50, align: 'right', render: function (item) {
                           return toMoney(item.AttStoneNumber);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='AttStoneNumber'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '石价(￥)', name: 'StonePrice', width: 80, align: 'right', render: function (item) {
                           return toMoney(item.StonePrice);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='StonePrice'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '金价小计(￥)', name: 'GoldTotal', width: 120, align: 'right', render: function (item) {
                           return toMoney(item.GoldTotal);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='GoldTotal'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '工费小计(￥)', name: 'CostsTotal', width: 120, align: 'right', render: function (item) {
                           return toMoney(item.CostsTotal);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='CostsTotal'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '成本总价(￥)', name: 'Totals', width: 120, align: 'right', render: function (item) {
                           return toMoney(item.Totals);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='Totals'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '销售工费(￥)', name: 'SalesCostsTotal', width: 120, align: 'right', render: function (item) {
                           return toMoney(item.SalesCostsTotal);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='SalesCostsTotal'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   {
                       display: '销售价格(￥)', name: 'SalesTotalPrice', width: 120, align: 'right', render: function (item) {
                           return toMoney(item.SalesTotalPrice);
                       }, totalSummary: { type: 'sum', render: function (item, i) { return "￥<span id='SalesTotalPrice'>" + toMoney(item.sum) + "</span>"; } }
                   },
                   { display: '一口价', name: 'FixedPrice', width: 120, render: function (item) { if (item.FixedPrice == null) { return '0'; } else { return toMoney(item.FixedPrice); } } },
                   { display: '关联门店', name: 'indep_name', width: 120, render: function (item) { if (item.indep_name == null) { return "总部" } else { return item.indep_name; } } },

                 {
                     display: '状态', name: 'status', width: 60, align: 'right', render: function (item) {
                         return GetproductStatus(item.status);
                     }
                 }
            ];


            $("#maingrid4").ligerGrid({
                columns: columns,
                dataAction: 'server',
                url: "Product.grid.xhd?allstock=1&sum=1&categoryid=&rnd=" + Math.random(),
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
            $.get("toolbar.GetSys.xhd?mid=allstock&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
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

                $("#toolbar").ligerToolBar({
                    items: items
                });

                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#sstatus").ligerComboBox({
                    data: productStatus
                    , valueFieldID: 'status',

                    isMultiSelect: true,
                    width: 150,
                    split: ",",
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
                //$("#sck").ligerComboBox({
                //    width: 150,
                //    selectBoxWidth: 240,
                //    selectBoxHeight: 200,
                //    valueField: 'id',
                //    textField: 'text',
                //    treeLeafOnly: false,
                //    tree: {
                //        url: 'Product_warehouse.tree.xhd?zb=1&qxz=1&rnd=' + Math.random(),
                //        idFieldName: 'id',
                //        checkbox: false
                //    },
                //});
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
        function GetSearchWhere() {
            var sendtxt = "&allstock=1&sum=1&rnd=" + Math.random();
            var serchtxt = "scode=" + $("#scode").val();
            serchtxt += "&stext=" + $("#stext").val();
            serchtxt += "&status=" + $("#status").val();
            serchtxt += "&categoryid=" + $("#sfl_val").val();
            //serchtxt += "&warehouse_id=" + $("#sck_val").val();
            serchtxt += "&sindep_id=" + $("#sdep_val").val();
            serchtxt += sendtxt;
            return serchtxt;
        }

        //查询
        function doserch() {

            var serchtxt = GetSearchWhere();
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product.grid.xhd?" + serchtxt);
        }

        function ExcelDC() {
            var serchtxt = GetSearchWhere();

            location.href = "Product.ExoportHQStock.xhd?etype=7" + serchtxt;

        }



        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                f_openWindow('product/product_add.aspx?pid=' + rows.id, "查看商品", 700, 580);
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
                    <%--                    <td>
                        <div style='width: 40px; text-align: right; float: right'>仓库：</div>
                    </td>
                    <td>
                        <div style='float: left'>
                            <input type='select' id='sck' name='sck' ltype='date' ligerui='{width:120}' />
                        </div>
                    </td>--%>
                    <td>
                        <div style='width: 40px; text-align: right; float: right'>门店：</div>
                    </td>
                    <td>
                        <div style='float: left'>
                            <input type='select' id='sdep' name='sck' ltype='sdep' ligerui='{width:120}' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 80px; text-align: right; float: right'>状态：</div>
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
