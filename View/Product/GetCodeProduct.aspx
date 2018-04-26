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
    <script type="text/javascript">

        var manager = "";
        var status = "";
        var SupplierID = "";
        var allotid = "";
        $(function () {
            //  $("#layout1").ligerLayout({ leftWidth: 200, allowLeftResize: false, allowLeftCollapse: true, space: 2, heightDiff: 1 });

            status = getparastr("status", "");
            SupplierID = getparastr("SupplierID", "");
            allotid = getparastr("allotid", "");
            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '��Ʒ����', name: 'product_name', align: 'left', width: 120 },
                    { display: '��Ʒ���', name: 'category_name', align: 'left', width: 120 },
                    { display: '������', name: 'BarCode', align: 'left', width: 120 },
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
                        display: '����С��(��)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.CostsTotal);
                        }
                    },
                      { display: '�����ŵ�', name: 'indep_name', width: 120, render: function (item) { if (item.indep_name == null) { return "�ܲ�" } else { return item.indep_name; } } },
                    { display: '�ִ�ֿ�', name: 'warehouse_name', width: 100, render: function (item) { if (item.warehouse_name == null) { return '�ֿܲ�'; } else { return item.warehouse_name; } } },
                    {
                        display: '״̬', name: 'status', width: 80, align: 'right', render: function (item) {
                            return GetproductStatus(item.status);
                        }
                    },
                    {
                        display: '���״̬', name: 'authIn', width: 80, align: 'right', render: function (item) {
                            switch (item.authIn) {
                                case 101:
                                    return "<span style='color:#0066FF'> ��������� </span>";
                                case 102:
                                    return "<span style='color:#00CC66'> ��������� </span>";
                                default:
                                    return "����";
                            }
                        }
                    }
                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                width: '100%',
                height: '100%',
                heightDiff: -2,
                title: "ɨ�������Ʒ",
            });
            toolbar();


        });
        function toolbar() {
            var items = [];
            items.push({ type: 'textbox', id: 'scode', text: '�����룺' });
            items.push({ type: 'button', text: '����', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

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
        //��ѯ
        function doserch() {
            var scode = $("#scode").val();
            if (itemsCode.indexOf(scode) > -1 || scode.length < 14) {
                return false;
            }
            var manager = $("#maingrid4").ligerGetGridManager();
            var serchtxt = "depdata=" + getparastr("depdata", "") + "&status=" + status + "&scode=" + scode + "&SupplierID=" + SupplierID + "&rnd=" + Math.random() + "&sindep_id=" + getparastr("depid", "");
            serchtxt += "&optype=" + getparastr("optype", "");
            var url = ("Product.grid.xhd?" + serchtxt);

            $.get(url, function (rdata, textStatus) {

                var data = eval('(' + rdata + ')');
                if (data.Total <= 0) {
                    $.ligerDialog.waitting('��������δ�ҵ���Ʒ!!'); setTimeout(function () { $.ligerDialog.closeWaitting(); }, 400);
                } else {
                    var rows = data.Rows;
                    $(rows).each(function (i, v) {

                        v.quantity = 1;
                        manager.addRow(v);
                        if (itemsCode.indexOf(scode) <= -1) {
                            itemsCode.push(scode);
                        }
                        //if (v.status != 1) {
                        //    $.ligerDialog.warn('����������Ʒ���ܵ���');
                        //}
                        //else {
                        //    manager.addRow(v);
                        //}
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
            <div position="left" title="��Ʒ���" style="display: none">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;">
                    <ul id="tree1"></ul>
                </div>
            </div>
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
