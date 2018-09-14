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
    <script src="../JS/XHD.js?v=3.0" type="text/javascript"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script type="text/javascript">

        var orderid = "";
        var sysSalePrice;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));
            orderid = getparastr("id", "");
            loadForm(orderid);
            GetsysSalePrice();
        });

        function GetsysSalePrice() {
            var prices = getCookie("this_broadcast", "");
            sysSalePrice = prices.split(',');

        }

        function f_save() {

            f_check();

            if (orderid.length <= 0 && f_postnum() == 0) {
                $.ligerDialog.warn("�������Ʒ��");
                return;
            }

            var manager = $("#maingrid4").ligerGetGridManager();
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");
                sendtxt += "&PostData=" + JSON.stringify(manager.getChanges());
                sendtxt += "&customer_id=" + getparastr("customer_id");
                return $("form :input").fieldSerialize() + sendtxt;
            }

        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "Sale_order.form.xhd", /* ע���������ֶ�ӦCS�ķ������� */
                data: { id: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var rows = [];
                    var customer_id = obj.Customer_id || getparastr("customer_id");
                    if (!customer_id) {
                        rows.push([{ display: "�ͻ�", name: "T_customer", validate: "{required:true}", width: 465 }]);
                    }
                    var dname = (decodeURI(getCookie("udepname", "")));
                    if (obj.emp_name == null || obj.emp_name == undefined) {
                        obj.emp_name = getCookie("xhdcrm_uid", "");
                        obj.emp_id = ("<%=XHD.Common.DEncrypt.DEncrypt.Decrypt(HttpUtility.UrlDecode(XHD.Common.CookieHelper.GetValue("uid")))%>");
                        obj.cashiername = obj.emp_name;
                        obj.cashier_id = obj.emp_id;
                    }
                    if (oaid != null && oaid.length > 0) {
                        dname = obj.dep_name;
                    }
                    rows.push(
                              [
                                 { display: "��Ա������", name: "T_VipCardType", type: "select", options: "{width:180,onChangeValue:function(){ newTotalAmount(); },data:[{id:0,text:'��'},{id:1,text:'��'},{id:2,text:'����'},{id:3,text:'Ա����'},{id:4,text:'�ɶ���'}],selectBoxHeight:50,value:'" + obj.VipCardType + "'}"  },
                                { display: "��Ա����", name: "T_vipcard", type: "text", initValue: obj.vipcard },
                              ],
                               [
                                { display: "�����ܽ��", name: "T_amount", type: "text", options: "{width:180,disabled:true,onChangeValue:function(){ getAmount(); }}", validate: "{required:true}", initValue: toMoney(obj.Order_amount) },
                                { display: "�Ż��ܽ��", name: "T_discount", type: "text", options: "{width:180,disabled:true,onChangeValue:function(){ getAmount(); }}", validate: "{required:true}", initValue: toMoney(obj.discount_amount) }
                               ],

                            [
                                { display: "Ӧ�ս��", name: "T_total", type: "text", options: "{width:180,disabled:false}", validate: "{required:true}", initValue: toMoney(obj.total_amount, "") },
                                { display: "���ս��", name: "T_arrears", type: "text", options: "{width:180,disabled:false}", validate: "{required:true}", initValue: toMoney(obj.arrears_money) }
                            ],
                            [
                              { display: "����״̬", name: "T_status", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=order_status',value:'" + obj.Order_status_id + "'}", validate: "{required:true}" }
                            ],
                            [
                              { display: "֧����ʽ", name: "T_paytype", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=pay_type',value:'" + obj.pay_type_id + "'}", validate: "{required:true}", initValue: formatTimebytype(obj.import_time, "yyyy-MM-dd") },
                              { display: "Pos����", name: "T_PayTheBill", type: "text", options: "{width:180}", initValue: (obj.PayTheBill) }
                            ],
                            [
                                { display: "�����ŵ�", name: "T_saledep_id", type: "select", options: "{width:180,treeLeafOnly: false,disabled:true,tree:{url:'hr_department.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false,value:'" + obj.saledep_id + "',emptyText:'" + dname + "'}}", initValue: dname, validate: "{required:true}" },
                               { display: "�ɽ�ʱ��", name: "T_date", type: "date", options: "{width:180}", validate: "{required:true}", initValue: formatTimebytype(obj.Order_date, "yyyy-MM-dd") },
                            ],
                            [
                                { display: "�ɽ���Ա", name: "T_emp", validate: "{required:true}", initValue: obj.emp_name },
                                { display: "����Ա", name: "T_cashier", validate: "{required:true}", initValue: obj.cashiername }
                            ],
                            [
                                { display: "��ע", name: "T_details", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.Order_details }
                            ]
                        );

                    if (!obj.discount_amount)
                        obj.discount_amount = 0;

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '����', type: 'group', icon: '',
                                rows: rows

                            }
                        ]
                    });
                    f_grid();

                    $("#T_customer").ligerComboBox({
                        width: 465,
                        onBeforeOpen: f_selectCustomer
                    });


                    $("#T_customer").val(obj.Customer_name);
                    $("#T_customer_val").val(obj.customer_id);


                    $("#T_emp").ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectEmp
                    });

                    $("#T_cashier").ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectCash
                    })

                    $("#T_emp").val(obj.emp_name);
                    $("#T_emp_val").val(obj.emp_id);

                    $("#T_cashier").val(obj.cashiername);
                    $("#T_cashier_val").val(obj.cashier_id);


                }
            });
        }
        function f_selectCustomer() {
            $.ligerDialog.open({
                zindex: 9005, title: 'ѡ��ͻ�', width: 650, height: 300, url: '../crm/customer/getCustomer.aspx?saleselect=1', buttons: [
                     { text: 'ȷ��', onclick: f_selectCustomerOK },
                     { text: 'ȡ��', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectCustomerOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('��ѡ����!');
                return;
            }

            $("#T_customer").val(data.cus_name);
            $("#T_customer_val").val(data.id);
            dialog.close();
        }

        function f_selectEmp() {
            $.ligerDialog.open({
                zindex: 9005, title: 'ѡ��Ա��', width: 650, height: 300, url: '../hr/getemp_auth.aspx?auth=3', buttons: [
                     { text: 'ȷ��', onclick: f_selectEmpOK },
                     { text: 'ȡ��', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectCash() {
            $.ligerDialog.open({
                zindex: 9005, title: 'ѡ��Ա��', width: 650, height: 300, url: '../hr/getemp_auth.aspx?auth=3', buttons: [
                     { text: 'ȷ��', onclick: f_selectCashOK },
                     { text: 'ȡ��', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectCashOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('��ѡ����!');
                return;
            }

            $("#T_cashier").val(data.name);
            $("#T_cashier_val").val(data.id);

            dialog.close();
        }

        function f_selectEmpOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('��ѡ����!');
                return;
            }

            $("#T_emp").val(data.name);
            $("#T_emp_val").val(data.id);

            if ($("#T_cashier").val().length <= 0) {
                $("#T_cashier").val(data.name);
                $("#T_cashier_val").val(data.id);
            }
            dialog.close();
        }

        function getAmount() {
            var T_amount = $("#T_amount").val();
            var T_discount = $("#T_discount").val();
            var T_total = $("#T_total").val();

            $("#T_discount").val(toMoney(T_discount));
            $("#T_total").val(toMoney(parseFloat(T_amount.replace(/\$|\,/g, '')) - parseFloat(T_discount.replace(/\$|\,/g, ''))));
        }



        function f_grid() {
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '��Ʒ����', name: 'product_name', align: 'left', width: 150 },
                    { display: '��Ʒ���', name: 'category_name', align: 'left', width: 150 },
                    {
                        display: 'Ʒ��', width: 100, name: 'cproperty',
                        render: function (item) {
                            if (item != null) {
                                for (var i = 0; i < productCategoryAttr.length; i++) {
                                    if (productCategoryAttr[i]['id'] == item.cproperty)
                                        return productCategoryAttr[i]['text']
                                }
                            } else {
                                return "����";
                            }
                        }
                    },
                    { display: '������', name: 'BarCode', align: 'left', width: 120 },
                    {
                        display: '����(��)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                    {
                        display: 'ʵʱ��(��)', name: 'SalesUnitPrice', width: 80, align: 'left', render: function (item) {
                            return toMoney(item.SalesUnitPrice);
                        }
                    },
                    {
                        display: 'ʵʱ�ܼ�(��)', name: 'RealTotal', width: 80, align: 'left', render: function (item) {
                            return toMoney(item.RealTotal);
                        }
                    },
                    {
                        display: '���۹���(��)', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesCostsTotal);
                        }
                    },
                    {
                        display: 'һ�ڼ�(��)', name: 'FixedPrice', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.FixedPrice);
                        }
                    },
                    {
                        display: '�����ܼ�(��)', name: 'amount', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.amount);
                        }, editor: { type: 'number' }
                    },
                    {
                        display: '���Ż�(��)', name: 'Discounts', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.Discounts);
                        }, editor: { type: 'number' }
                    },
                ],
                allowHideColumn: false,
                onAfterEdit: f_onAfterEdit,
                title: '��Ʒ��ϸ',
                usePager: false,
                enabledEdit: true,
                url: "Sale_order_details.grid.xhd?orderid=" + getparastr("id"),
                width: '100%',
                height: 350,
                heightDiff: -1,
                onLoaded: f_loaded
            });

        }

        function f_loaded() {
            if ($("#btn_add").length > 0) {
                $(".l-grid-loading").fadeOut();
                return;
            }
            var ads = getparastr("ads", 0);
            if (ads == 0) {
                $(".l-grid-loading").fadeOut();
                return;
            }

            $(".l-panel-header").append("<div style='width:150px;float:right'><div id = 'btn_add' style='margin-top:2px;'></div><div id = 'btn_del' style='margin-top:2px;'></div></div>");
            $(".l-grid-loading").fadeOut();
            $("#btn_add").ligerButton({
                width: 60,
                text: "���",
                icon: '../../images/icon/11.png',
                click: add
            })

            $("#btn_del").ligerButton({
                width: 60,
                text: "ɾ��",
                icon: '../../images/icon/12.png',
                click: pro_remove
            })
            $("#maingrid4").ligerGetGridManager()._onResize();
        }

        function f_onAfterEdit(e) {
            var manager = $("#maingrid4").ligerGetGridManager();

            $("#T_amount").val(toMoney(manager.getColumnDateByType('amount', 'sum') * 1.0));
            $("#T_discount").val(toMoney(manager.getColumnDateByType('Discounts', 'sum') * 1.0));
            getAmount();

        }

        function add() {
            f_openWindow("product/GetProduct2.aspx?depdata=1&status=1,101", "ѡ����Ʒ", 1000, 600, f_getpost, 9003);
        }
        function pro_remove() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.deleteSelectedRow();
            setTimeout(function () {
                $("#T_amount").val(toMoney(manager.getColumnDateByType('amount', 'sum') * 1.0));
                $("#T_discount").val(toMoney(manager.getColumnDateByType('Discounts', 'sum') * 1.0));
                getAmount();
            }, 50);
        }
        function f_getpost(item, dialog) {
            var rows = null;
            if (!dialog.frame.f_select()) {
                alert('��ѡ����Ʒ!');
                return;
            }
            else {
                rows = dialog.frame.f_select();

                //�����ظ�
                var manager = $("#maingrid4").ligerGetGridManager();
                var data = manager.getData();

                for (var i = 0; i < rows.length; i++) {
                    rows[i].product_id = rows[i].id;
                    rows[i].SaleType = 0;
                    rows[i].DiscountType = 0;
                    rows[i].DiscountCount = 0;
                    rows[i].Discounts = 0;
                    rows[i].RealTotal = 0;
                    rows[i].SalesUnitPrice = 0;
                    var add = 1;
                    for (var j = 0; j < data.length; j++) {
                        if (rows[i].product_id == data[j].product_id) {
                            add = 0;
                        }
                    }
                    if (add == 1) {

                        rows[i].quantity = 1;
                        var row = rows[i];
                        row = GetSaleDiscounts(row);
                        manager.addRow(row);
                    }
                }
                dialog.close();
            }
            $("#T_amount").val(toMoney(manager.getColumnDateByType('amount', 'sum') * 1.0));
            $("#T_discount").val(toMoney(manager.getColumnDateByType('Discounts', 'sum') * 1.0));
            getAmount();
        }

        //��ȡ�����Żݽ��
        function GetSaleDiscounts(row) {

            var cardType = parseInt($("#T_VipCardType_val").val());

            var cinfos = GetCategoryInfo(row.category_id);
            var categoryAttr = parseInt(cinfos.cproperty);
            var pcategory_id = cinfos.fparentid;
            var category_name = row.category_name;
            var FixedPrice = parseFloat(row.FixedPrice);
            //���۹���
            var SalesCostsTotal = parseFloat(row.SalesCostsTotal);
            //���ۼ۸�
            var SalesTotalPrice = parseFloat(row.SalesTotalPrice);
            row.SaleType = categoryAttr;
            if (isNaN(SalesCostsTotal)) { SalesCostsTotal = 0; }
            if (isNaN(SalesTotalPrice)) { SalesTotalPrice = 0; }
            if (isNaN(FixedPrice)) { FixedPrice = 0; }

            //û�л�Ա��ֱ�ӷ���
            if (cardType == null || isNaN(cardType) || cardType == undefined) {
                cardType = 0;
            }

            //1 �ֻ�����  2���λƽ�
            var ctype = 0;
            //���λƽ�
            if (pcategory_id == "750efaa0-f2e4-47f9-81ce-83b67d346a31") {
                ctype = 1;
            } else if (pcategory_id == "ff991a92-e1d4-4a73-bdf6-cc412b274446") {
                ctype = 2;
            }

            var Discounts = 0;
            var amount = SalesTotalPrice;
            //��һ�ڼ۰�һ�ڼ�����
            if (FixedPrice > 0) {
                switch (cardType) {

                    case 1://��
                        row.DiscountType = 1;
                        row.DiscountCount = 9;
                        amount = FixedPrice * 0.9;

                        break;
                    case 2://����
                        row.DiscountType = 1;
                        amount = FixedPrice;
                        break;
                    case 3://Ա��
                        row.DiscountType = 1;
                        row.DiscountCount = 8.5;
                        amount = FixedPrice * 0.85;
                        break;
                    case 4://�ɶ�
                        row.DiscountType = 1;
                        row.DiscountCount = 8;
                        amount = FixedPrice * 0.8;
                        break;
                    default://û�л�Ա��
                        amount = FixedPrice;
                        break;
                }
                amount = parseFloat(amount.toFixed(2));
                Discounts = (FixedPrice - amount);
                //Ӳ�������ܹ��Ѻͻƽ�����
                if (category_name.indexOf("Ӳ��") >= 0 || categoryAttr == 2) {
                    row.amount = amount + SalesCostsTotal + Discounts;
                    row.Discounts = Discounts;
                    return row;
                }
            }
            else {
                //K����
                if (categoryAttr == 3 || category_name.indexOf("18K") >= 0) {
                    switch (cardType) {

                        case 1://��
                            row.DiscountType = 1;
                            row.DiscountCount = 8;
                            amount = SalesTotalPrice * 0.8;
                            break;
                        case 2://����
                            row.DiscountType = 1;
                            row.DiscountCount = 8.8;
                            amount = SalesTotalPrice * 0.88;
                            break;
                        case 3://Ա��
                            row.DiscountType = 1;
                            row.DiscountCount = 6;
                            amount = SalesTotalPrice * 0.6;
                            break;
                        case 4://�ɶ�
                            row.DiscountType = 1;
                            row.DiscountCount = 5;
                            amount = SalesTotalPrice * 0.5;
                            break;
                        default://û�л�Ա��
                            amount = SalesTotalPrice;
                            break;
                    }
                    amount = parseFloat(amount.toFixed(2));
                    Discounts = (SalesTotalPrice - amount);

                }
                else if (categoryAttr == 1 || categoryAttr == 2 || ctype == 1 || ctype == 2) {//�ƽ���
                    //�ֻ�����
                    if (ctype == 1) {
                        var uprs = parseFloat(sysSalePrice[0]);
                        if (isNaN(uprs)) { uprs = 0; }
                        row.SalesUnitPrice = uprs;
                        SalesTotalPrice = uprs * parseFloat(row.Weight);
                        row.RealTotal = SalesTotalPrice;
                    } else if (ctype == 2) {//���ν�
                        var uprs = parseFloat(sysSalePrice[1]);
                        if (isNaN(uprs)) { uprs = 0;}
                        row.SalesUnitPrice = uprs;
                        SalesTotalPrice = uprs * parseFloat(row.Weight);
                        row.RealTotal = SalesTotalPrice;
                    }
                    // ÿ�˼����� ���Ѵ���
                    switch (cardType) {
                        case 0://û�л�Ա��
                            amount = SalesTotalPrice;
                            break;
                        case 1://��
                            row.DiscountType = 1;
                            row.DiscountCount = 9;
                            amount = SalesTotalPrice - (row.Weight * 50);
                            break;
                        case 2://����
                            row.DiscountType = 0;
                            amount = SalesTotalPrice - (row.Weight * 40);
                            break;
                        case 3://Ա��
                            row.DiscountType = 1;
                            row.DiscountCount = 8.5;
                            amount = SalesTotalPrice - (row.Weight * 60);
                            break;
                        case 4://�ɶ�
                            row.DiscountType = 1;
                            row.DiscountCount = 8;
                            amount = SalesTotalPrice - (row.Weight * 70);

                            break;
                    }
                    amount = parseFloat(amount.toFixed(2));
                    Discounts = parseFloat(SalesTotalPrice - amount);
                }
            }

            //�����Żݼ���
            // ���Ѵ���
            var CostsTotal = 0;
            switch (cardType) {

                case 1://��
                    CostsTotal = (SalesCostsTotal * 0.9);
                    break;
                case 2://����
                    CostsTotal = SalesCostsTotal;
                    break;
                case 3://Ա��
                    CostsTotal = (SalesCostsTotal * 0.85);
                    break;
                case 4://�ɶ�
                    CostsTotal = (SalesCostsTotal * 0.8);
                    break;
                default://û�л�Ա��
                    CostsTotal = SalesCostsTotal;
                    break;
            }
            CostsTotal = parseFloat(CostsTotal.toFixed(2));
            //�л�Ա�������Żݼ���
            if (cardType > 0) {
                Discounts = Discounts + (SalesCostsTotal - CostsTotal);
                Discounts = parseFloat(Discounts.toFixed(2));
            } else {
                Discounts = 0;
            }

            row.amount = (amount + CostsTotal + Discounts);

            row.Discounts = Discounts;

            return row;
        }

        //���¼���۸�
        function newTotalAmount() {
            //�����ظ�
            var manager = $("#maingrid4").ligerGetGridManager();
            if (manager == null) { return; }
            var data = manager.getData();
            for (var i = 0; i < data.length; i++) {
                var row = data[i];
                row = GetSaleDiscounts(row);
                manager.updateRow(manager.getRow(i), row);
            }
            $("#T_amount").val(toMoney(manager.getColumnDateByType('amount', 'sum') * 1.0));
            $("#T_discount").val(toMoney(manager.getColumnDateByType('Discounts', 'sum') * 1.0));
            getAmount();
        }

        function f_postnum() {
            var manager = $("#maingrid4").ligerGetGridManager();
            return manager.getColumnDateByType('amount', 'count') * 1.0;
        }

        function f_check() {
            var g = $("#maingrid4").ligerGetGridManager().endEdit(true);
        }
        function remote() {
            var url = "PB_BicycleType.Exist.xhd?id=" + getparastr("id") + "&rnd=" + Math.random();
            return url;
        }

        function toMoney2(values) {
            if (parseFloat(values) > 0) {
                return toMoney(values);
            }
            return "";
        }

        function arrearsAmount() {
            var T_total = parseFloat($("#T_total").val().replace(/\$|\,/g, ''));
            var T_receive = parseFloat($("#T_receive").val().replace(/\$|\,/g, ''));
            var T_arrears = T_total - T_receive;
            $("#T_arrears").val(toMoney(T_arrears));
        }

    </script>

</head>
<body style="overflow: hidden;">
    <form id="form1" onsubmit="return false">
    </form>
    <div style="padding: 5px 4px 5px 2px;">

        <div id="maingrid4">
        </div>
    </div>
</body>
</html>
