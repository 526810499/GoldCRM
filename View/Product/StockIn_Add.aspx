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
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js?v=2" type="text/javascript"></script>

    <script type="text/javascript">
        var sid = "";
        var isAddTemp = 0;
        var status = 0;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));
            sid = getparastr("id", "");
            status = getparastr("status", 0);
            loadForm(sid);

        });

        function f_save() {
            var manager = $("#maingridc4").ligerGetGridManager();
            var fdata = manager.getData();

            if (fdata.length <= 0) {
                $.ligerDialog.warn('请添加入库商品');
                return false;
            }
            if ($(form1).valid()) {
                var sendtxt = "T_Remark=" + $("#T_Remark").val() + "&id=" + sid;
                return sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "Product_StockIn.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, inType: 0, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    if (oaid == "addtemp") {
                        sid = obj.id;
                        isAddTemp = 1;
                    }
                    if (obj.status == -1) { isAddTemp = 1; }

                    var rows = [];

                    rows.push(

                            [
                            { display: "备注", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                            ]
                        );

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
                    f_grid(sid);
                }
            });
        }
        function ViewModel(id) {
            f_openWindow('product/product_add.aspx?pid=' + id, "修改商品", 700, 580, f_addsave);
        }


        function f_grid(sid) {
            $("#maingridc4").ligerGrid({
                columns: [
                    {
                        display: '条形码', name: 'BarCode', align: 'left', width: 160, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=ViewModel('" + item.id + "')>" + item.BarCode + "</a>";
                            return html;
                        }
                    },
                    {
                        display: '商品名称', name: 'product_name', align: 'left', width: 120
                    },
                    { display: '商品类别', name: 'category_name', align: 'left', width: 120 },

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
                    {
                        display: '成本总价(￥)', name: 'Totals', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.Totals);
                        }
                    },
                    {
                        display: '销售工费(￥)', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesCostsTotal);
                        }
                    },
                    {
                        display: '销售价格(￥)', name: 'SalesTotalPrice', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesTotalPrice);
                        }
                    },
                             {
                                 display: '一口价', name: 'FixedPrice', width: 120, render: function (item) { if (item.FixedPrice == null) { return '0'; } else { return toMoney(item.FixedPrice); } }
                             },
                ],
                allowHideColumn: false,
                title: '入库明细',
                enabledEdit: false,
                url: "Product.StockGridDetail.xhd?stockid=" + sid,
                pageSize: 20,
                pageSizeOptions: [10, 20, 30, 40, 50, 60, 80, 100, 120],
                width: '99%', height: '180',
                heightDiff: 0,
                checkbox: false,
                width: '100%',
                height: 400,
                heightDiff: -1,
                onLoaded: f_loaded,

            });

        }

        function f_loaded() {

            $(".l-panel-header").append("<div id='headerBtn' style='width:350px;float:right;margin-bottom:2px;'><div id = 'btn_addcode' style='margin-top:2px;'></div><div id = 'btn_add' style='margin-top:2px;'></div><div id = 'btn_update' style='margin-top:2px;'></div><div id = 'btn_del' style='margin-top:2px;'></div></div>");
            $(".l-grid-loading").fadeOut();

            if (status != 1) {
                $("#btn_add").ligerButton({
                    width: 80,
                    text: "手动添加",
                    icon: '../../../../images/icon/11.png',
                    click: add
                });
                //$("#btn_addcode").ligerButton({
                //    width: 80,
                //    text: "批量导入",
                //    icon: '../../images/icon/75.png',
                //    click: addBatchCode
                //});
                $("#btn_del").ligerButton({
                    width: 60,
                    text: "删除",
                    icon: '../../images/icon/12.png',
                    click: pro_remove
                });

                //$("#btn_update").ligerButton({
                //    width: 60,
                //    text: "修改",
                //    icon: '../../images/icon/33.png',
                //    click: updateCode
                //});

                $("#maingridc4").ligerGetGridManager()._onResize();
            }
        }



        function add() {

            f_openWindow("product/product_add.aspx", "手动添加商品", 1000, 600, f_addsave, 9003);

        }
        function f_addbatchsave(item, dialog) {
            f_addsave();
        }

        function updateCode() {

            var manager = $("#maingridc4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                f_openWindow('product/product_add.aspx?pid=' + rows.id, "修改商品", 1000, 700, f_addsave);
            }
            else {
                $.ligerDialog.warn('请选择商品！');
            }
        }


        function f_addsave(item, dialog, batch) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Product.save.xhd?StockID=" + sid + "&isAddTemp=" + isAddTemp, type: "POST",
                    data: issave,
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
            var manager = $("#maingridc4").ligerGetGridManager();
            manager.loadData(true);
        }

        //批量导入
        function addBatchCode() {
            f_openWindow("product/AddBatchProduct.aspx?sid=" + sid, "批量导入商品", 1000, 400, f_addbatchsave, 9003);
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
