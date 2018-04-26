<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js?v=2" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));
            loadForm(getparastr("id", ""));

        });

        function f_save() {
            var manager = $("#maingridc4").ligerGetGridManager();
            var fdata = manager.getData();
            var T_NowWarehouse_val = $("#T_Warehouse_val").val();
            if (T_NowWarehouse_val.length <= 0) {
                $.ligerDialog.warn('请选择入库仓库');
                return false;
            }
            if (fdata.length <= 0) {
                $.ligerDialog.warn('请添加入库商品');
                return false;
            }
            if ($(form1).valid()) {
                var sendtxt = "T_Warehouse_val=" + T_NowWarehouse_val + "&T_Remark=" + $("#T_Remark").val() + "&id=" + getparastr("id", "");
                sendtxt += "&PostData=" + JSON.stringify(GetPostData());
                return sendtxt;
            }
        }
        function GetPostData() {
            var manager = $("#maingridc4").ligerGetGridManager();
            var data = manager.getChanges();
            var items = [];
            if (data != null) {

                $(data).each(function (i, v) {
                    var datas = { id: v.id, warehouse_id: v.warehouse_id, BarCode: v.BarCode, __status: v.__status, status: v.status, remark: v.remark };
                    items.push(datas);
                });
            }
            return items;
        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "Product_StockIn.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    var rows = [];
                    if (oaid.length > 0) {
                        rows.push(
                           [
                           { display: "入库仓库", name: "T_Warehouse", type: "select", options: "{width:180,disabled:true,treeLeafOnly: false,tree:{url:'Product_warehouse.tree.xhd',idFieldName: 'id',checkbox: false},value:'" + (obj.warehouse_id == undefined ? "" : obj.warehouse_id) + "'}", validate: "{required:true}" }
                           ],
                           [
                            { display: "备注", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                           ]
                       );
                    } else {
                        rows.push(
                                [
                                { display: "入库仓库", name: "T_Warehouse", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_warehouse.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + (obj.warehouse_id == undefined ? "" : obj.warehouse_id) + "'}", validate: "{required:true}" }
                                ],
                                [
                                { display: "备注", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                                ]
                            );
                    }


                    if (obj != null && obj.status >= 0) {
                        rows.push([
                               { display: "状态", name: "T_Status", type: "select", options: "{width:180,onSelected:function(value){},data:[{id:0,text:'未提交'},{id:1,text:'提交保存'}],selectBoxHeight:50, value:" + obj.status + "}", validate: "{required:true}" }
                        ]);
                    }
                    if (!obj.discount_amount)
                        obj.discount_amount = 0;

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '入库单', type: 'group', icon: '',
                                rows: rows

                            }
                        ]
                    });
                    f_grid();
                }
            });
        }



        function f_grid() {
            $("#maingridc4").ligerGrid({
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
                    }
                    ,
                       { display: '备注', name: 'remark', align: 'left', width: 180 }
                ],
                allowHideColumn: false,
                title: '入库明细',
                usePager: false,
                enabledEdit: false,
                url: "Product_StockIn.gridDetail.xhd?stockid=" + getparastr("id", ""),
                width: '100%',
                height: 500,
                heightDiff: -1,
                onLoaded: f_loaded,

            });

        }

        function f_loaded() {
            $(".l-grid-loading").fadeOut();
            if (parseInt(getparastr("astatus", "0")) != 0) {
                return;
            }
            if ($("#btn_add").length > 0)
                return;

            $(".l-panel-header").append("<div id='headerBtn' style='width:290px;float:right;margin-bottom:2px;'><div id = 'btn_addcode' style='margin-top:2px;'></div><div id = 'btn_add' style='margin-top:2px;'></div><div id = 'btn_del' style='margin-top:2px;'></div></div>");
            $(".l-grid-loading").fadeOut();


            $("#btn_add").ligerButton({
                width: 80,
                text: "手动添加",
                icon: '../../../../images/icon/11.png',
                click: add
            });
            $("#btn_addcode").ligerButton({
                width: 60,
                text: "扫码添加",
                icon: '../../images/icon/75.png',
                click: addCode
            });

            $("#btn_del").ligerButton({
                width: 80,
                text: "删除",
                icon: '../../images/icon/12.png',
                click: pro_remove
            });
            $("#maingridc4").ligerGetGridManager()._onResize();
        }

        var beforeFromID = "";
        function checkAdd() {
            var T_fromdep_val = $("#T_Warehouse_val").val();


            if (T_fromdep_val.length <= 0) {
                $.ligerDialog.warn('请先选择入库仓库');
                return false;
            }

            var manager = $("#maingridc4").ligerGetGridManager();
            var fdata = manager.getData();
            if (fdata.length > 0) {

                if (T_fromdep_val.length <= 0) {
                    var warn = "请先选择调出门店！";
                    top.$.ligerDialog.warn(warn, "警告【每次调拨只能操作一仓库】", "", 9901);
                    return false;
                } else {
                    if (beforeFromID.length <= 0) {
                        beforeFromID = T_fromdep_val;
                    }

                    if (beforeFromID != T_fromdep_val) {
                        var warn = "入库仓库和已选商品仓库不符！";
                        top.$.ligerDialog.warn(warn, "警告【每次只能操作一仓库】", "", 9901);
                        return false;
                    }
                }
            }
            beforeFromID = T_fromdep_val;
            return true;
        }

        function add() {
            if (checkAdd()) {
                f_openWindow("product/Take/AddProduct.aspx?code=1&depdata=0&notfindadd=0&optype=mdrk&warehouse_id=" + beforeFromID, "选择商品", 1000, 400, f_getpost, 9003);
            }
        }

        function addCode() {
            if (checkAdd()) {
                f_openWindow("product/Take/AddProduct.aspx?depdata=0&notfindadd=0&optype=mdrk&warehouse_id=" + beforeFromID, "选择扫码商品", 1000, 400, f_getpost, 9003);
            }
        }

        function pro_remove() {
            var manager = $("#maingridc4").ligerGetGridManager();
            manager.deleteSelectedRow();

            var fdata = manager.getData();
            if (fdata.length <= 0) {
                $("#T_Warehouse").removeAttr("disabled");
            }
        }


        function f_getpost(item, dialog) {
            var rows = dialog.frame.f_select();
            if (rows == null || rows.length <= 0) {
                var warn = "请选择商品！";
                top.$.ligerDialog.warn(warn, "警告", "", 9904);
                return;
            }
            else {
                //过滤重复
                var manager = $("#maingridc4").ligerGetGridManager();
                var data = manager.getData();

                for (var i = 0; i < rows.length; i++) {
                    rows[i].BarCode = rows[i].BarCode;
                    var add = 1;
                    console.log(rows[i]);
                    for (var j = 0; j < data.length; j++) {
                        if (rows[i].BarCode == data[j].BarCode) {
                            add = 0;
                        }
                    }
                    if (add == 1) {
                        manager.addRow(rows[i]);
                    }
                }
                dialog.close();
            }
        }

    </script>

</head>
<body style="overflow: hidden;">
    <form id="form1" onsubmit="return false">
    </form>
    <div style="padding: 5px 4px 5px 2px;">

        <div id="maingridc4">
        </div>
    </div>
</body>
</html>
