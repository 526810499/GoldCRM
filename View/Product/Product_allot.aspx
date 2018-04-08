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
    <script src="../JS/XHD.js?v=1" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 200, allowLeftResize: false, allowLeftCollapse: true, space: 2, heightDiff: -5 });
            $("#tree1").ligerTree({
                url: 'Product_warehouse.tree.xhd?qb=1&rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                //parentIDFieldName: 'pid',
                usericon: 'd_icon',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    //$(".l-first div:first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '订单号', name: 'id', align: 'left', width: 350 },
                    { display: '调拨到仓库', name: 'NowWarehouseName', align: 'left', width: 120 },
                    { display: '创建人', name: 'CreateName', align: 'left', width: 160 },
                    {
                        display: '创建时间', name: 'create_time', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
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
                    { display: '备注', name: 'remark', width: 200 }

                ],
                dataAction: 'server',
                url: "Product_allot.grid.xhd?rnd=" + Math.random(),
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
                                { display: '产品名称', name: 'product_name', align: 'left', width: 120 },
                                { display: '产品类别', name: 'category_name', align: 'left', width: 120 },
                                { display: '条形码', name: 'BarCode', align: 'left', width: 200 },
                                {
                                    display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                                        return toMoney(item.Weight);
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
                                        return (item.AttStoneNumber);
                                    }
                                },
                                {
                                    display: '销售工费(￥)', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.CostsTotal);
                                    }
                                },
                                {
                                    display: '销售价格(￥)', name: 'SalesTotalPrice', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.SalesTotalPrice);
                                    }
                                }
                            ],
                            usePager: false,
                            checkbox: false,
                            url: "Product_allot.gridDetail.xhd?allotid=" + r.id,
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
            toolbar();

        });

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=product_allot&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'sstatus', text: '状态：' });
                items.push({ type: 'textbox', id: 'sorderid', text: '调拨单号：' });
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
            var tdata = treemanager.getSelected();
            var cid = "";
            if (tdata != null && tdata.data != null) {
                cid = tdata.data.id;
            }
            serchtxt += "&whid=" + cid;
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product_allot.grid.xhd?" + serchtxt);
        }

        //重置
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#form1").each(function () {
                this.reset();
            });
        }

        function auth() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            console.log("rows", rows);
            if (rows && rows != undefined) {
                var buttons = [];
                if (rows.status == 1) {
                    buttons.push({ text: '审核通过', onclick: f_saveYesAuth });
                    buttons.push({ text: '审核不通过', onclick: f_saveNoAuth });
                }
                f_openWindow2('product/Product_allotAdd.aspx?authbtn=1&id=' + rows.id + "&astatus=" + rows.status, "审核调拨单", 700, 580, buttons);
            }
            else {
                $.ligerDialog.warn('请选择产品！');
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
                f_openWindow2('product/Product_allotAdd.aspx?id=' + rows.id + "&astatus=" + rows.status, "修改调拨单", 700, 580, buttons);
            }
            else {
                $.ligerDialog.warn('请选择产品！');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            var whid = "";
            if (notes != null && notes != undefined) {
                whid = notes.data.id;
            }
            var buttons = [];
            buttons.push({ text: '保存', onclick: f_save });
            buttons.push({ text: '保存并提交审核', onclick: f_saveAuth });
            f_openWindow2('product/Product_allotAdd.aspx?astatus=0&whid=' + whid, "新增调拨单", 1050, 580, buttons);
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("调拨单删除不能恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Product_allot.del.xhd", type: "POST",
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
                $.ligerDialog.warn("请选择调拨单");
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
                    url: "Product_allot.save.xhd?auth=" + auth, type: "POST",
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
                    url: "Product_allot.Auth.xhd?auth=" + auth, type: "POST",
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
    <div style="padding: 5px 10px 0px 5px;">
        <form id="form1" onsubmit="return false">
            <div id="layout1" style="">
                <div position="left" title="仓库">
                    <div id="treediv" style="width: 200px; height: 100%; margin: -1px; float: left; overflow: auto; margin-top: 2px;">
                        <ul id="tree1"></ul>
                    </div>
                </div>
                <div position="center">
                    <div id="toolbar" style="margin-top: 10px;"></div>
                    <div id="maingrid4" style="margin: -1px;"></div>

                </div>
            </div>
        </form>

    </div>
</body>
</html>
