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
    <script type="text/javascript">

        var manager = "";
        var status = "";
        var SupplierID = "";

        $(function () {
            status = getparastr("status", "");
            SupplierID = getparastr("SupplierID", "");


            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            var url = "Product.grid.xhd?status=" + status + "&SupplierID=" + SupplierID+"&depdata=" + getparastr("depdata", "");
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '��Ʒ����', name: 'product_name', align: 'left', width: 120 },
                    { display: '��Ʒ���', name: 'category_name', align: 'left', width: 120 },
                    { display: '������', name: 'BarCode', align: 'left', width: 160 },
                    {
                        display: '����(��)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                    {
                        display: '��ʯ��', name: 'MainStoneWeight', width: 60, align: 'right', render: function (item) {
                            return toMoney(item.MainStoneWeight);
                        }
                    },
                    {
                        display: '��ʯ��', name: 'AttStoneWeight', width: 60, align: 'right', render: function (item) {
                            return toMoney(item.AttStoneWeight);
                        }
                    },
                    {
                        display: '��ʯ��', name: 'AttStoneNumber', width: 50, align: 'right', render: function (item) {
                            return (item.AttStoneNumber);
                        }
                    },
                    {
                        display: '����С��(��)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.CostsTotal);
                        }
                    },
                   { display: '�ִ�ֿ�', name: 'warehouse_name', width: 100, render: function (item) { if (item.warehouse_name == null) { return '�ֿܲ�'; } else { return item.warehouse_name; } } }

                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                onSelectRow: function (row, index, data) {
                    //alert('onSelectRow:' + index + " | " + data.ProductName); 
                },
                url: url,
                width: '100%',
                height: '100%',
                heightDiff: -2,
                title: "��Ʒѡ��",
            });
            toolbar();
        });
        function toolbar() {
            var items = [];
            items.push({ type: 'textbox', id: 'sfl', text: '���ࣺ' });
            items.push({ type: 'textbox', id: 'sck', text: '�ִ�ֿ⣺' });
            items.push({ type: 'textbox', id: 'stext', text: '��Ʒ����' });
            items.push({ type: 'textbox', id: 'scode', text: '�����룺' });
            items.push({ type: 'button', text: '����', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

            $("#toolbar").ligerToolBar({
                items: items
            });

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
                    //parentIDFieldName: 'pid',
                    checkbox: false
                },
            });

            $("#stext").ligerTextBox({ width: 150 });
            $("#scode").ligerTextBox({ width: 150 });
            $("#scode").on('input', function (e) {
                doChangeSearch();
            });
        }

        function doChangeSearch() {
            var v = $("#scode").val();
            if (v != undefined && v.length == 13) {
                doserch();
            }
        }

        //��ѯ
        function doserch() {

            var cid = "";

            var serchtxt = "depdata=" + getparastr("depdata", "") + "&categoryid=" + $("#sfl_val").val() + "&status=" + status + "&stext=" + $("#stext").val() + "&scode=" + $("#scode").val() + "&warehouse_id=" + $("#sck_val").val() + "&rnd=" + Math.random()
            var manager = $("#maingrid4").ligerGetGridManager();
            var url = "Product.grid.xhd?" + serchtxt;
 
            manager._setUrl(url);

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


        <!--<a class="l-button" onclick="getChecked()" style="float:left;margin-right:10px;">��ȡѡ��(��ѡ��)</a> -->
        <div style="display: none">
            <!--  ����ͳ�ƴ��� -->
        </div>
    </form>
</body>
</html>
