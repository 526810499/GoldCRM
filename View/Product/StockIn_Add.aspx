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
                $.ligerDialog.warn('����������Ʒ');
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
                url: "Product_StockIn.form.xhd", /* ע���������ֶ�ӦCS�ķ������� */
                data: { id: oaid, inType: 0, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
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
                            { display: "��ע", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                            ]
                        );

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
                    f_grid(sid);
                }
            });
        }
        function ViewModel(id) {
            f_openWindow('product/product_add.aspx?pid=' + id, "�޸���Ʒ", 700, 580, f_addsave);
        }


        function f_grid(sid) {
            $("#maingridc4").ligerGrid({
                columns: [
                    {
                        display: '������', name: 'BarCode', align: 'left', width: 160, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=ViewModel('" + item.id + "')>" + item.BarCode + "</a>";
                            return html;
                        }
                    },
                    {
                        display: '��Ʒ����', name: 'product_name', align: 'left', width: 120
                    },
                    { display: '��Ʒ���', name: 'category_name', align: 'left', width: 120 },

                    {
                        display: '����(��)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                    {
                        display: '����С��(��)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.CostsTotal);
                        }
                    },
                    {
                        display: '�ɱ��ܼ�(��)', name: 'Totals', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.Totals);
                        }
                    },
                    {
                        display: '���۹���(��)', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesCostsTotal);
                        }
                    },
                    {
                        display: '���ۼ۸�(��)', name: 'SalesTotalPrice', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesTotalPrice);
                        }
                    },
                             {
                                 display: 'һ�ڼ�', name: 'FixedPrice', width: 120, render: function (item) { if (item.FixedPrice == null) { return '0'; } else { return toMoney(item.FixedPrice); } }
                             },
                ],
                allowHideColumn: false,
                title: '�����ϸ',
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
                    text: "�ֶ����",
                    icon: '../../../../images/icon/11.png',
                    click: add
                });
                //$("#btn_addcode").ligerButton({
                //    width: 80,
                //    text: "��������",
                //    icon: '../../images/icon/75.png',
                //    click: addBatchCode
                //});
                $("#btn_del").ligerButton({
                    width: 60,
                    text: "ɾ��",
                    icon: '../../images/icon/12.png',
                    click: pro_remove
                });

                //$("#btn_update").ligerButton({
                //    width: 60,
                //    text: "�޸�",
                //    icon: '../../images/icon/33.png',
                //    click: updateCode
                //});

                $("#maingridc4").ligerGetGridManager()._onResize();
            }
        }



        function add() {

            f_openWindow("product/product_add.aspx", "�ֶ������Ʒ", 1000, 600, f_addsave, 9003);

        }
        function f_addbatchsave(item, dialog) {
            f_addsave();
        }

        function updateCode() {

            var manager = $("#maingridc4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                f_openWindow('product/product_add.aspx?pid=' + rows.id, "�޸���Ʒ", 1000, 700, f_addsave);
            }
            else {
                $.ligerDialog.warn('��ѡ����Ʒ��');
            }
        }


        function f_addsave(item, dialog, batch) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
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
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }

        function f_load() {
            var manager = $("#maingridc4").ligerGetGridManager();
            manager.loadData(true);
        }

        //��������
        function addBatchCode() {
            f_openWindow("product/AddBatchProduct.aspx?sid=" + sid, "����������Ʒ", 1000, 400, f_addbatchsave, 9003);
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
