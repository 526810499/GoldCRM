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
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">

        var manager = "";
        var status = "";
        var SupplierID = "";
        var allotid = "";
        var warehouse_id = "";
        $(function () {
            warehouse_id = getparastr("warehouse_id", "");
            status = getparastr("status", "");
            SupplierID = getparastr("SupplierID", "");
            allotid = getparastr("allotid", "");
            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '商品名称', name: 'product_name', align: 'left', width: 120 },
                    { display: '商品类别', name: 'category_name', align: 'left', width: 120 },
                    { display: '条形码', name: 'BarCode', align: 'left', width: 160 },
                    {
                        display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                    {
                        display: '工费小计(￥)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.CostsTotal);
                        }
                    },
                       { display: '备注', name: 'remark', align: 'left', width: 180 }
                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                width: '100%',
                height: '100%',
                heightDiff: -2,
                title: "扫码添加商品",
            });
            toolbar();


        });
        function toolbar() {
            var items = [];
            items.push({ type: 'textbox', id: 'scode', text: '条形码：' });
            items.push({ type: 'button', text: '搜索', icon: '../../images/search.gif', disable: true, click: function () { doserch() } });

            $("#toolbar").ligerToolBar({
                items: items
            });


            $("#scode").ligerTextBox({ width: 250, onChangeValue: function (value) { doChangeSearch(); }, onFocus: function () { $("#scode").select(); }, onBlur: function () { doChangeSearch(); } });
            $("#scode").attr("maxlength", 14);
            $("#scode").on('input', function (e) {
                doChangeSearch();
            });
        }

        function doChangeSearch() {
            var v = $("#scode").val();
            if (v != undefined && v.length == 14) {
                doserch();
            }
        }
        var itemsCode = [];

        //查询
        function doserch() {
            var scode = $("#scode").val();
            //必须要输入条形码
            if (scode.length < 14 || itemsCode.indexOf(scode) > -1) {
                return false;
            }
            var notfindadd = getparastr("notfindadd", 1);
            var manager = $("#maingrid4").ligerGetGridManager();
            var serchtxt = "depdata=" + getparastr("depdata", "") + "&status=" + status + "&scode=" + scode + "&SupplierID=" + SupplierID + "&warehouse_id=" + warehouse_id + "&rnd=" + Math.random()
            var url = ("Product.grid.xhd?" + serchtxt);

            $.get(url, function (rdata, textStatus) {

                var data = eval('(' + rdata + ')');
                if (data.Total <= 0 && notfindadd == 1) {
                    var nobj = { id: scode, product_name: "", category_name: "", BarCode: scode, Weight: 0, SalesCostsTotal: 0, SalesTotalPrice: 0, status: 2, remark: "没有该商品条形码存在" };
                    itemsCode.push(scode);
                    manager.addRow(nobj);
                } else {
                    var rows = data.Rows;
                    $(rows).each(function (i, v) {
                        //商品状态为已销售，盘盈
                        if (v.status == 4) {
                            v.status = 2;
                            v.remark = "商品标记为已售";
                        } else {
                            v.status = 1;
                            v.remark = "";
                        }
                        if (itemsCode.indexOf(scode) <= -1) {
                            itemsCode.push(scode);
                            manager.addRow(v);
                        }
                    });
                }
            });
        }


        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getCheckedRows();
            return rows;
        }
    </script>
</head>
<body style="padding: 0px; overflow: hidden;">
    <form id="form1" onsubmit="return false">
        <div id="layout1" style="margin: -1px">
            <div position="center">
                <div id="toolbar" style="margin-top: 5px"></div>
                <div id="maingrid4" style="margin: -1px;"></div>

            </div>
        </div>


        <!--<a class="l-button" onclick="getChecked()" style="float:left;margin-right:10px;">获取选择(复选框)</a> -->
        <div style="display: none">
            <!--  数据统计代码 -->
        </div>
    </form>
</body>
</html>
