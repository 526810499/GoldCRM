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
    <script src="../../JS/XHD.js?v=1" type="text/javascript"></script>
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
                            var html = "<a href='javascript:void(0)' onclick=PView('" + item.id + "')>" + item.id + "</a>";
                            return html;
                        }
                    },
                {
                    display: '入库审核类型', name: 'FromType', width: 100, align: 'left', render: function (item) {
                        switch (item.status) {
                            case 1:
                                return "<span style='color:#0066FF'> 门店调拨 </span>";
                            default:
                                return "<span style='color:#00CC66'> 总部出库 </span>";
                        }
                    }
                },
                    { display: '入库门店', name: 'dep_name', align: 'left', width: 120 },
                    { display: '创建人', name: 'CreateName', align: 'left', width: 120 },
                    {
                        display: '创建时间', name: 'create_time', width: 150, align: 'left', render: function (item) {
                            return formatTime(item.create_time, 'yyyy-MM-dd HH:mm:ss');
                        }
                    },
                    {
                        display: '状态', name: 'status', width: 100, align: 'left', render: function (item) {
                            switch (item.status) {
                                case 1:
                                    return "<span style='color:#0066FF'> 等待审核 </span>";
                                case 2:
                                    return "<span style='color:#00CC66'> 审核通过 </span>";
                                case 3:
                                    return "<span style='color:red'> 审核不通过 </span>";
                            }
                        }
                    },
                    { display: '备注', name: 'remark', width: 120 }

                ],
                dataAction: 'server',
                url: "Product_StockIn.grid.xhd?intype=1&rnd=" + Math.random(),
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
                                { display: '商品名称', name: 'product_name', align: 'left', width: 120 },
                                { display: '商品类别', name: 'category_name', align: 'left', width: 120 },
                                { display: '条形码', name: 'BarCode', align: 'left', width: 160 },
                                {
                                    display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                                        return toMoney(item.Weight);
                                    }
                                },
                                {
                                    display: '销售工费', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.SalesCostsTotal);
                                    }
                                }, {
                                    display: '销售价格', name: 'SalesTotalPrice', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.SalesTotalPrice);
                                    }
                                }, { display: '一口价', name: 'FixedPrice', width: 120, render: function (item) { if (item.FixedPrice == null) { return '0'; } else { return toMoney(item.FixedPrice); } } },
                               { display: '备注', name: 'remark', align: 'left', width: 180 }
                            ],
                            usePager: false,
                            checkbox: false,

                            url: "Product_StockIn.gridDetail.xhd?stockid=" + r.id,
                            width: '99%', height: '180',
                            heightDiff: 0
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
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=depstockin&rnd=" + Math.random(), function (data, textStatus) {
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
                    { text: '等待审核', id: '1' },
                    { text: '审核通过', id: '2' },
                     { text: '审核不通过', id: '3' },
                    ], valueFieldID: 'status',
                });
                $("#scode").ligerTextBox({ width: 250 });
                $("#ftype").ligerComboBox({
                    data: [
                    { text: '所有', id: '' },
                    { text: '门店调拨', id: '1' },
                    { text: '总部出库', id: '0' },
                    ], valueFieldID: 'FromType',
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
            var serchtxt = "intype=1&status=" + $("#status").val() + "&FromType=" + $("#FromType").val();
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

        function auth() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                var buttons = [];
                if (rows.status == 1) {
                    buttons.push({ text: '审核通过', onclick: f_saveAuth });
                    buttons.push({ text: '审核不通过', onclick: f_saveAuthN });
                }
                f_openWindow2('product/Dep/StockIn_Add.aspx?authbtn=1&id=' + rows.id + "&astatus=" + rows.status, "审核入库单", 1050, 680, buttons);
            }
            else {
                $.ligerDialog.warn('请选择入库单！');
            }
        }

        function PView(id) {

            f_openWindow2('product/Dep/StockIn_Add.aspx?id=' + id, "查看入库单", 1050, 780);

        }


        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                var buttons = [];
                if (rows.status == 1) {
                    buttons.push({ text: '审核通过', onclick: f_saveAuth });
                    buttons.push({ text: '审核不通过', onclick: f_saveAuthN });
                }
                f_openWindow2('product/Dep/StockIn_Add.aspx?id=' + rows.id + "&astatus=" + rows.status, "修改审核入库单", 1050, 780, buttons);
            }
            else {
                $.ligerDialog.warn('请选择入库单！');
            }
        }
        function add() {
            var buttons = [];
            buttons.push({ text: '保存', onclick: f_save });
            //buttons.push({ text: '生成盘点单', onclick: create_take });
            buttons.push({ text: '保存提交审核', onclick: f_saveAuth });
            f_openWindow2('product/Dep/StockIn_Add.aspx?astatus=0', "新增入库单", 1050, 780, buttons);
        }

        function create_take() {


        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                var msg = "入库单删除不能恢复，确定删除？";
                if (row.status == 1 || row.status == 3) {
                    $.ligerDialog.warn("等待审核或已审核通过的入库单不能删除");
                    return false;
                }

                $.ligerDialog.confirm(msg, function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Product_StockIn.del.xhd?intype=1", type: "POST",
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
                $.ligerDialog.warn("请选择入库单");
            }

        }

        function f_saveAuth(item, dialog) {
            Auth(item, dialog, 2);
        }

        function f_saveAuthN(item, dialog) {
            Auth(item, dialog, 3);
        }

        function f_save(item, dialog) {
            Save(item, dialog, 0);
        }

        function Auth(item, dialog, auth) {
            var issave = dialog.frame.f_saveAuth();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Product_StockIn.Auth.xhd?intype=1&auth=" + auth, type: "POST",
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

        function Save(item, dialog, auth) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Product_StockIn.save.xhd?intype=1&auth=" + auth, type: "POST",
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
        function ExcelDC() {

            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                location.href = "/product/ExportProduct.aspx?etype=3&stockid=" + row.id + "&rnd=" + Math.random();
            }
            else {
                $.ligerDialog.warn("请选择入库单");
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
                    <td>
                        <div style='width: 100px; text-align: right; float: right'>入库审核类型：</div>
                    </td>
                    <td>
                        <input type='select' id='ftype' name='ftype' ltype='text' ligerui='{width:120}' />

                    </td>


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
