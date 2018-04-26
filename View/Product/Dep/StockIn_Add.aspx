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
                $.ligerDialog.warn('��ѡ�����ֿ�');
                return false;
            }
            if (fdata.length <= 0) {
                $.ligerDialog.warn('����������Ʒ');
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
                url: "Product_StockIn.form.xhd", /* ע���������ֶ�ӦCS�ķ������� */
                data: { id: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
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
                           { display: "���ֿ�", name: "T_Warehouse", type: "select", options: "{width:180,disabled:true,treeLeafOnly: false,tree:{url:'Product_warehouse.tree.xhd',idFieldName: 'id',checkbox: false},value:'" + (obj.warehouse_id == undefined ? "" : obj.warehouse_id) + "'}", validate: "{required:true}" }
                           ],
                           [
                            { display: "��ע", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                           ]
                       );
                    } else {
                        rows.push(
                                [
                                { display: "���ֿ�", name: "T_Warehouse", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_warehouse.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + (obj.warehouse_id == undefined ? "" : obj.warehouse_id) + "'}", validate: "{required:true}" }
                                ],
                                [
                                { display: "��ע", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                                ]
                            );
                    }


                    if (obj != null && obj.status >= 0) {
                        rows.push([
                               { display: "״̬", name: "T_Status", type: "select", options: "{width:180,onSelected:function(value){},data:[{id:0,text:'δ�ύ'},{id:1,text:'�ύ����'}],selectBoxHeight:50, value:" + obj.status + "}", validate: "{required:true}" }
                        ]);
                    }
                    if (!obj.discount_amount)
                        obj.discount_amount = 0;

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '��ⵥ', type: 'group', icon: '',
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
                    { display: '��Ʒ����', name: 'product_name', align: 'left', width: 120 },
                    { display: '��Ʒ���', name: 'category_name', align: 'left', width: 120 },
                    { display: '������', name: 'BarCode', align: 'left', width: 160 },
                    {
                        display: '����(��)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                    {
                        display: '����С��(��)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.CostsTotal);
                        }
                    }
                    ,
                       { display: '��ע', name: 'remark', align: 'left', width: 180 }
                ],
                allowHideColumn: false,
                title: '�����ϸ',
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
                text: "�ֶ����",
                icon: '../../../../images/icon/11.png',
                click: add
            });
            $("#btn_addcode").ligerButton({
                width: 60,
                text: "ɨ�����",
                icon: '../../images/icon/75.png',
                click: addCode
            });

            $("#btn_del").ligerButton({
                width: 80,
                text: "ɾ��",
                icon: '../../images/icon/12.png',
                click: pro_remove
            });
            $("#maingridc4").ligerGetGridManager()._onResize();
        }

        var beforeFromID = "";
        function checkAdd() {
            var T_fromdep_val = $("#T_Warehouse_val").val();


            if (T_fromdep_val.length <= 0) {
                $.ligerDialog.warn('����ѡ�����ֿ�');
                return false;
            }

            var manager = $("#maingridc4").ligerGetGridManager();
            var fdata = manager.getData();
            if (fdata.length > 0) {

                if (T_fromdep_val.length <= 0) {
                    var warn = "����ѡ������ŵ꣡";
                    top.$.ligerDialog.warn(warn, "���桾ÿ�ε���ֻ�ܲ���һ�ֿ⡿", "", 9901);
                    return false;
                } else {
                    if (beforeFromID.length <= 0) {
                        beforeFromID = T_fromdep_val;
                    }

                    if (beforeFromID != T_fromdep_val) {
                        var warn = "���ֿ����ѡ��Ʒ�ֿⲻ����";
                        top.$.ligerDialog.warn(warn, "���桾ÿ��ֻ�ܲ���һ�ֿ⡿", "", 9901);
                        return false;
                    }
                }
            }
            beforeFromID = T_fromdep_val;
            return true;
        }

        function add() {
            if (checkAdd()) {
                f_openWindow("product/Take/AddProduct.aspx?code=1&depdata=0&notfindadd=0&optype=mdrk&warehouse_id=" + beforeFromID, "ѡ����Ʒ", 1000, 400, f_getpost, 9003);
            }
        }

        function addCode() {
            if (checkAdd()) {
                f_openWindow("product/Take/AddProduct.aspx?depdata=0&notfindadd=0&optype=mdrk&warehouse_id=" + beforeFromID, "ѡ��ɨ����Ʒ", 1000, 400, f_getpost, 9003);
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
                var warn = "��ѡ����Ʒ��";
                top.$.ligerDialog.warn(warn, "����", "", 9904);
                return;
            }
            else {
                //�����ظ�
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
