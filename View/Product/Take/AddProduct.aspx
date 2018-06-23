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
    <script src="../../JS/XHD.js?v=6" type="text/javascript"></script>
    <script type="text/javascript">

        var manager = "";
        var status = "";
        var SupplierID = "";
        var allotid = "";
        var warehouse_id = "";
        var isCode = 0;
        $(function () {
            warehouse_id = getparastr("warehouse_id", "");
            status = getparastr("status", "");
            SupplierID = getparastr("SupplierID", "");
            allotid = getparastr("allotid", "");
            isCode = getparastr("code", "0");
            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            var serchtxt = "depdata=" + getparastr("depdata", "") + "&status=" + status + "&warehouse_id=" + warehouse_id;
            serchtxt += "&optype=" + getparastr("optype", "");
            var url = ("Product.grid.xhd?" + serchtxt);
            if (isCode == 0) {
                url = "";
            }
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
                      { display: '关联门店', name: 'indep_name', width: 120, render: function (item) { if (item.indep_name == null) { return "总部" } else { return item.indep_name; } } },
                       { display: '备注', name: 'remark', align: 'left', width: 100 },
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
                checkbox: true,
                url: url,
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
            if (isCode == 1) {
                items.push({ type: 'textbox', id: 'sfl', text: '分类：' });
                //items.push({ type: 'textbox', id: 'sck', text: '现存仓库：' });
                items.push({ type: 'textbox', id: 'stext', text: '商品名：' });
            }
            items.push({ type: 'textbox', id: 'scode', text: '条形码：' });
            items.push({ type: 'button', text: '搜索', icon: '../../images/search.gif', disable: true, click: function () { doserch() } });

            $("#toolbar").ligerToolBar({
                items: items
            });
            if (isCode == 1) {
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
                $("#stext").ligerTextBox({ width: 150 });
            }
            $("#scode").ligerTextBox({ width: 150 });
            $("#scode").on('input', function (e) {
                doChangeSearch();
            });


            manager = $("#maingrid4").ligerGetGridManager();
        }

        function doChangeSearch() {
            var v = $("#scode").val();
            if (v != undefined && v.length == 7 || isCode == 1) {
                doserch();
            }
        }
        var itemsCode = [];

        //查询
        function doserch() {
            var scode = $("#scode").val();
            //必须要输入条形码
            if ((scode.length < 7 || itemsCode.indexOf(scode) > -1) && isCode == 0) {
                return false;
            }
            var notfindadd = getparastr("notfindadd", 1);
            var manager = $("#maingrid4").ligerGetGridManager();
            var serchtxt = "depdata=" + getparastr("depdata", "") + "&status=" + status + "&scode=" + scode + "&SupplierID=" + SupplierID + "&rnd=" + Math.random()

            if (isCode == 1) {
                serchtxt += "&categoryid=" + $("#sfl_val").val();
            }
            serchtxt += "&warehouse_id=" + warehouse_id;
            serchtxt += "&optype=" + getparastr("optype","");
            var url = ("Product.grid.xhd?" + serchtxt);

            if (isCode == 1) {
                var manager = $("#maingrid4").ligerGetGridManager();
                manager._setUrl(url);
                return false;
            }

            $.get(url, function (rdata, textStatus) {

                var data = eval('(' + rdata + ')');
                if (data.Total <= 0 && notfindadd == 1 && scode.length == 7) {
                    if (itemsCode.indexOf(scode) <= -1) {
                        var nobj = { id: scode, product_name: "", category_name: "", BarCode: scode, Weight: 0, SalesCostsTotal: 0, SalesTotalPrice: 0, status: 9999, remark: "库存中未找到商品" };
                        itemsCode.push(scode);
                        manager.addRow(nobj);
                    }
                } else {
                    var rows = data.Rows;

                    var data = manager.getData();
                    $(rows).each(function (i, objs) {
                        var v = objs;
                        var tcode = scode;
                        if (isCode == 1) {
                            tcode = v.BarCode;
                        }
                        var canAdd = true;

                        if (canAdd && itemsCode.indexOf(tcode) <= -1) {
                            itemsCode.push(tcode);
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
