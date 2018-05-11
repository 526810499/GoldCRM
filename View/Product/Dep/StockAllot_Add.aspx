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
        var whid = "";

        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));
            whid = getparastr("whid", "");
            loadForm(getparastr("id"));

        });

        function f_save() {
            var manager = $("#maingridc4").ligerGetGridManager();
            var fdata = manager.getData();
            var T_fromdep_val = $("#T_fromdep_id_val").val();
            var T_todep_val = $("#T_todep_id_val").val();
            if (T_fromdep_val.length <= 0) {
                $.ligerDialog.warn('请先选择调出门店');
                return false;
            }
            if (T_todep_val.length <= 0) {
                $.ligerDialog.warn('请先选择调入门店');
                return false;
            }

            if (T_fromdep_val == T_todep_val) {
                top.$.ligerDialog.warn("调出以调入同一门店无需操作", "调出以调入同一门店无需操作", "", 9901);
                return false;
            }

            if (fdata.length <= 0) {
                $.ligerDialog.warn('请添加调度商品');
                return false;
            }
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");
                sendtxt += "&PostData=" + JSON.stringify(GetPostData());
                return $("form :input").fieldSerialize() + sendtxt;
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
                url: "Product_allot.form.xhd", /* 注意后面的名字对应CS的方法名称 */
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
                    //if (obj.fromdep_id == null || obj.fromdep_id == undefined) {
                    //    obj.fromdep_id = getCookie("udepid", "");
                    //}
                    rows.push(
                            [
                                  { display: "调出门店", name: "T_fromdep_id", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'hr_department.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + (obj.fromdep_id == undefined ? "" : obj.fromdep_id) + "'}", validate: "{required:true}" }
                            ],
                            [
                              { display: "调入门店", name: "T_todep_id", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'hr_department.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + (obj.todep_id == undefined ? "" : obj.todep_id) + "'}", validate: "{required:true}" }
                            ],
                            //[
                            // { display: "调拨至仓库", name: "T_NowWarehouse", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_warehouse.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + (obj.NowWarehouse == undefined ? whid : obj.NowWarehouse) + "'}", validate: "{required:true}" }
                            //],
                            [
                             { display: "备注", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                            ]
                        );
                    if (obj != null && obj.status >= 0) {
                        rows.push([
                               { display: "状态", name: "T_Status", type: "select", options: "{width:180,onSelected:function(value){},data:[{id:0,text:'等待提交'},{id:1,text:'等待审核'},{id:2,text:'审核通过'},{id:3,text:'审核不通过'}],selectBoxHeight:50, value:" + obj.status + "}", validate: "{required:true}" }
                        ]);
                    }
                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '调拨', type: 'group', icon: '',
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
                        display: '销售工费', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesCostsTotal);
                        }
                    },
                    //{ display: '现存仓库', name: 'warehouse_name', width: 100, render: function (item) { if (item.warehouse_name == null) { return '总仓库'; } else { return item.warehouse_name; } } },
                      { display: '关联门店', name: 'indep_name', width: 120, render: function (item) { if (item.indep_name == null) { return "总部" } else { return item.indep_name; } } },
                ],
                allowHideColumn: false,
                title: '商品明细',
                usePager: false,
                enabledEdit: false,
                url: "Product_allot.gridDetail.xhd?allotid=" + getparastr("id"),
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
                icon: '../../../../images/icon/75.png',
                click: addCode
            });
            $("#btn_add").ligerButton({
                width: 80,
                text: "手动添加",
                icon: '../../../../images/icon/11.png',
                click: add
            });
            $("#btn_del").ligerButton({
                width: 80,
                text: "删除",
                icon: '../../../../images/icon/12.png',
                click: pro_remove
            });
            $("#maingridc4").ligerGetGridManager()._onResize();
        }




        function addCode() {

            if (checkAdd()) {
                f_openWindow("product/GetCodeProduct.aspx?depdata=1&depid=" + beforeFromID + "&optype=mddb", "选择扫码商品", 1200, 600, f_getpost, 9003);
            }

        }
        var beforeFromID = "";
        function checkAdd() {
            var T_fromdep_val = $("#T_fromdep_id_val").val();
            var T_todep_val = $("#T_todep_id_val").val();

            if (T_fromdep_val.length <= 0) {
                $.ligerDialog.warn('请先选择调出门店');
                return false;
            }
            if (T_todep_val.length <= 0) {
                $.ligerDialog.warn('请先选择调入门店');
                return false;
            }

            if (T_fromdep_val == T_todep_val) {
                top.$.ligerDialog.warn("调出以调入同一门店无需操作", "调出以调入同一门店无需操作", "", 9901);
                return false;
            }

            var manager = $("#maingridc4").ligerGetGridManager();
            var fdata = manager.getData();
            if (fdata.length > 0) {
                if (T_fromdep_val.length <= 0) {
                    var warn = "请先选择调出门店！";
                    top.$.ligerDialog.warn(warn, "警告【每次调拨只能操作一门店】", "", 9901);
                    return false;
                } else {
                    if (beforeFromID.length <= 0) {
                        beforeFromID = T_fromdep_val;
                    }

                    if (beforeFromID != T_fromdep_val) {
                        var warn = "调出门店和已选商品门店不符！";
                        top.$.ligerDialog.warn(warn, "警告【每次调拨只能操作一门店】", "", 9901);
                        return false;
                    }
                }
            }
            beforeFromID = T_fromdep_val;
            return true;
        }

        function add() {

            if (checkAdd()) {
                f_openWindow("product/GetProduct2.aspx?depdata=1&depid=" + beforeFromID + "&optype=mddb", "选择商品", 1200, 600, f_getpost, 9003);
            }

        }

        function pro_remove() {
            var manager = $("#maingridc4").ligerGetGridManager();
            manager.deleteSelectedRow();
        }


        function f_getpost(item, dialog) {
            var rows = null;
            if (!dialog.frame.f_select()) {

                $.ligerDialog.warn('请选择商品');
                return;
            }
            else {
                rows = dialog.frame.f_select();

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
