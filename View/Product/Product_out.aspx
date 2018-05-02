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
    <script src="../JS/XHD.js?v=3" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            // $("#layout1").ligerLayout({   allowLeftResize: false, allowLeftCollapse: true, space: 2, heightDiff: -5 });


            // treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    {
                        display: '订单号', name: 'id', align: 'left', width: 300, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('pout','" + item.id + "')>" + item.id + "</a>";
                            return html;
                        }
                    },
                    { display: '出库至门店', name: 'todep_name', align: 'left', width: 300 },
                    { display: '创建人', name: 'CreateName', align: 'left', width: 160 },
                    {
                        display: '创建时间', name: 'create_time', width: 100, align: 'left', render: function (item) {
                            return formatTime(item.create_time);
                        }
                    },
                    {
                        display: '状态', name: 'status', width: 80, align: 'left', render: function (item) {
                            switch (item.status) {
                                case 0:
                                    return "<span style='color:#0066FF'> 等待提交 </span>";
                                case 1:
                                    return "<span style='color:#00CC66'> 等待审核 </span>";
                                case 2:
                                    return "<span style='color:#009900'> 审核通过 </span>";
                                case 3:
                                    return "<span style='color:#FF3300'> 审核不通过 </span>";
                            }
                        }
                    },
                    { display: '备注', name: 'remark', width: 120 }

                ],
                dataAction: 'server',
                url: "Product_out.grid.xhd?rnd=" + Math.random(),
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
                                { display: '条形码', name: 'BarCode', align: 'left', width: 200 },
                                {
                                    display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                                        return toMoney(item.Weight);
                                    }
                                },
                                {
                                    display: '工费小计(￥)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.CostsTotal);
                                    }
                                }
                            ],
                            usePager: false,
                            checkbox: false,

                            url: "Product_out.gridDetail.xhd?outid=" + r.id,
                            width: '99%', height: '180',
                            heightDiff: 0
                        })

                    }
                },
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    // menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
            toolbar();

        });

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=product_out&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'sstatus', text: '状态：' });
                items.push({ type: 'textbox', id: 'sorderid', text: '出库单号：' });
                items.push({ type: 'textbox', id: 'allotid', text: '调拨单号：' });
                items.push({ type: 'textbox', id: 'scode', text: '条形码：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items

                });
                //menu = $.ligerMenu({
                //    width: 120, items: getMenuItems(data)
                //});
                $("#sorderid").ligerTextBox({ width: 200 });
                $("#scode").ligerTextBox({ width: 250 });
                $("#sstatus").ligerComboBox({
                    data: [
                    { text: '所有', id: '' },
                    { text: '等待提交', id: '0' },
                    { text: '等待审核', id: '1' },
                    { text: '审核通过', id: '2' },
                    { text: '审核不通过', id: '3' }
                    ], valueFieldID: 'status',
                });
                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }


        function onSelect(note) {
            doserch();
        }
        //查询
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;

            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product_out.grid.xhd?" + serchtxt);
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
                    buttons.push({ text: '审核通过', onclick: f_saveYesAuth });
                    buttons.push({ text: '审核不通过', onclick: f_saveNoAuth });
                }
                f_openWindow2('product/Product_outAdd.aspx?authbtn=1&id=' + rows.id + "&astatus=" + rows.status, "审核出库单", 1050, 680, buttons);
            }
            else {
                $.ligerDialog.warn('请选择调度单！');
            }
        }


        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                var buttons = [];
                if (rows.status == 0) {
                    buttons.push({ text: '保存', onclick: f_save });
                    buttons.push({ text: '保存并提交审核', onclick: f_saveAuth });
                }
                f_openWindow2('product/Product_outAdd.aspx?id=' + rows.id + "&astatus=" + rows.status, "修改出库单", 1050, 680, buttons);
            }
            else {
                $.ligerDialog.warn('请选择调度单！');
            }
        }
        function add() {
            var buttons = [];
            buttons.push({ text: '保存', onclick: f_save });
            buttons.push({ text: '保存并提交审核', onclick: f_saveAuth });
            f_openWindow2('product/Product_outAdd.aspx?astatus=0', "新增出库单", 1050, 680, buttons);
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("出库单删除不能恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Product_out.del.xhd", type: "POST",
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
                $.ligerDialog.warn("请选择出库单");
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
                    url: "Product_out.save.xhd?auth=" + auth, type: "POST",
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

        function f_saveYesAuth(item, dialog) {
            UserAuth(item, dialog, 2);
        }
        function f_saveNoAuth(item, dialog) {
            UserAuth(item, dialog, 3);
        }
        function UserAuth(item, dialog, auth) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Product_out.Auth.xhd?auth=" + auth, type: "POST",
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



        function prints() {

            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                window.open("PrintOutProduct.aspx?stime=" + (row.create_time) + "&outids=" + row.id + "&rnd=" + Math.random());
            }
            else {
                $.ligerDialog.warn("请选择出库单");
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

                <div position="center">
                    <div id="toolbar" style="margin-top: 10px;"></div>
                    <div id="maingrid4" style="margin: -1px;"></div>

                </div>
            </div>
        </form>

    </div>
</body>
</html>
