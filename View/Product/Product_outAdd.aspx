<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js?v=2" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));
            loadForm(getparastr("id"));

        });

        function f_save() {
            var manager = $("#maingridc4").ligerGetGridManager();
            var fdata = manager.getData();
            var T_NowWarehouse_val = $("#T_NowWarehouse_val").val();
            if (T_NowWarehouse_val.length <= 0) {
                $.ligerDialog.warn('请选择调度仓库');
                return false;
            }
            if (fdata.length <= 0) {
                $.ligerDialog.warn('请添加出库商品');
                return false;
            }
            if ($(form1).valid()) {
                var sendtxt = "T_NowWarehouse_val=" + T_NowWarehouse_val + "&T_Remark=" + $("#T_Remark").val() + "&id=" + getparastr("id");
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
                    var datas = { warehouse_id: v.warehouse_id, BarCode: v.BarCode, __status: v.__status };
                    items.push(datas);
                });
            }
            return items;
        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "Product_out.form.xhd", /* 注意后面的名字对应CS的方法名称 */
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

                    rows.push(
                            [
                            { display: "出库至仓库", name: "T_NowWarehouse", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_warehouse.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + (obj.NowWarehouse == undefined ? "" : obj.NowWarehouse) + "'}", validate: "{required:true}" }
                            ],
                            [
                             { display: "备注", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                            ]
                        );
                    if (obj != null && obj.status >= 0) {
                        rows.push([
                               { display: "状态", name: "T_Status", type: "select", options: "{width:180,onSelected:function(value){},data:[{id:0,text:'等待提交'},{id:1,text:'等待审核'},{id:2,text:'审核通过'},{id:3,text:'审核不通过'}],selectBoxHeight:50, value:" + obj.status + "}", validate: "{required:true}" }
                        ]);
                    }
                    if (!obj.discount_amount)
                        obj.discount_amount = 0;

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '出库', type: 'group', icon: '',
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
                allowHideColumn: false,
                title: '出库明细',
                usePager: false,
                enabledEdit: false,
                url: "Product_out.gridDetail.xhd?outid=" + getparastr("id"),
                width: '100%',
                height: 500,
                heightDiff: -1,
                onLoaded: f_loaded
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
            $("#btn_addcode").ligerButton({
                width: 80,
                text: "扫码添加",
                icon: '../../images/icon/75.png',
                click: addCode
            });
            $("#btn_add").ligerButton({
                width: 80,
                text: "手动添加",
                icon: '../../images/icon/11.png',
                click: add
            });

            $("#btn_del").ligerButton({
                width: 80,
                text: "删除",
                icon: '../../images/icon/12.png',
                click: pro_remove
            });
            $("#maingridc4").ligerGetGridManager()._onResize();
        }


        function add() {

            var buttons = [];
            buttons.push({ text: '保存', onclick: f_getpost });
            f_openWindow2("product/GetProduct2.aspx?status=1,2", "选择商品", 1000, 400, buttons, 9003);
        }

        function addCode() {
            f_openWindow("product/GetCodeProduct.aspx?status=1,2", "选择扫码商品", 1000, 400, f_getpost, 9003);
        }

        function pro_remove() {
            var manager = $("#maingridc4").ligerGetGridManager();
            manager.deleteSelectedRow();


            var fdata = manager.getData();
            if (fdata.length <= 0) {
                $("#T_allot_id").removeAttr("disabled");
            }
        }


        function f_getpost(item, dialog) {
            var rows = null;
            if (!dialog.frame.f_select()) {
                alert('请选择商品!');
                return;
            }
            else {
                rows = dialog.frame.f_select();

                //过滤重复
                var manager = $("#maingridc4").ligerGetGridManager();
                var data = manager.getData();

                for (var i = 0; i < rows.length; i++) {
                    rows[i].product_id = rows[i].id;
                    var add = 1;
                    for (var j = 0; j < data.length; j++) {
                        if (rows[i].product_id == data[j].product_id) {
                            add = 0;
                        }
                    }
                    if (add == 1) {
                        rows[i].quantity = 1;
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
