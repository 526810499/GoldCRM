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
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/XHD.js?v=3" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#maingrid4").ligerGrid({
                columns: [
                       {
                           display: '订单号', name: 'id', width: 250, align: 'left', render: function (item) {
                               var html = "<a href='javascript:void(0)' onclick=view('pretrieval','" + item.id + "')>" + item.id + "</a>";
                               return html;
                           }
                       },
                      { display: '订购门店', name: 'depname', width: 250, align: 'left' },
                    {
                        display: '创建时间', name: 'create_time', width: 200, render: function (item) {
                            return formatTime(item.create_time);
                        }
                    }
                     ,
                    {
                        display: '备注', name: 'remark', width: 250,
                    }
                ],
                detail: {
                    height: 'auto',
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        var grid = document.createElement('div');
                        $(p).append(grid);
                        $(grid).css('margin', 3).ligerGrid({
                            columns:
                                               [
                                           { display: "克重", name: "weight", width: 150 },
                                           { display: "数量", name: "number", width: 150 },
                                           {
                                               display: "品类", name: "categoryName", width: 150
                                           }
                                               ],
                            allowHideColumn: false,
                            title: '订购明细',
                            usePager: false,
                            enabledEdit: true,
                            rownumbers: true,
                            url: "SProduct_Retrieval.gridDetail.xhd?retrid=" + r.id,
                            width: '99%', height: '180',
                            heightDiff: 0
                        });

                    }
                },
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "SProduct_Retrieval.grid.xhd?grid=tree",
                width: '100%',
                height: '100%',
                heightDiff: -10,
                onRClickToSelect: true,
 
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }

            });



            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            toolbar();
        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=product_retrieval&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'sorderid', text: '订单号：' });
                items.push({ type: 'textbox', id: 'sdeep', text: '订购部门：' });
                items.push({ type: 'textbox', id: 'sbegtime', text: '订购时间：' });
                items.push({ type: 'textbox', id: 'sendtime', text: '结束：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items
                });

                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#sorderid").ligerTextBox({ width: 200 });
                $("#sdeep").ligerComboBox({
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
                $("#sbegtime").ligerDateEditor({ showTime: true, labelWidth: 100, labelAlign: 'left' });
                $("#sendtime").ligerDateEditor({ showTime: true, labelWidth: 100, labelAlign: 'left' });


                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("SProduct_Retrieval.grid.xhd?" + serchtxt);

        }

        function add() {
            f_openWindow("product/Product_Retrieval_Add.aspx?id=", "新增补货", 680, 420, f_save);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                console.log("row", row);
                f_openWindow('product/Product_Retrieval_Add.aspx?id=' + row.id, "修改补货", 680, 420, f_save);
            } else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("删除不能恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "SProduct_Retrieval.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
                            dataType: 'json',
                            success: function (result) {
                                $.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_reload();
                                }
                                else {
                                    $.ligerDialog.error(obj.Message);
                                }
                            },
                            error: function () {
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('删除失败！', "", null, 9003);
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择信息！");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "SProduct_Retrieval.save.xhd", type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_reload();
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();

                    }
                });

            }
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };
    </script>
    <style type="text/css">
        .l-leaving {
            background: #eee;
            color: #999;
        }
    </style>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div style="padding: 10px;">
            <div id="toolbar"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
