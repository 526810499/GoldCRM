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

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));
            var pid = getparastr("pid", "");
            loadForm(pid);
        });

        function f_save() {

            if ($(form1).valid()) {
                var T_product_category = $("#T_product_category_val").val();
                if (T_product_category == null || T_product_category.length <= 0) {
                    $.ligerDialog.warn('��Ʒ������ѡ��');
                    return false;
                }
                var T_product_categoryName = $("#T_product_category").val();

                var T_StockPrice = parseFloat($("#T_StockPrice").val());
                var T_Weight = parseFloat($("#T_Weight").val());

                //if (T_StockPrice <= 0 && T_product_categoryName != "����Ʒ") {
                //    $.ligerDialog.warn('������۲���ΪС��0��');
                //    return false;
                //}
                if (T_Weight <= 0) {
                    $.ligerDialog.warn('��������ΪС��0��');
                    return false;
                }

                var sct = parseFloat($("#T_SalesCostsTotal").val().replace(/\$|\,/g, ''));
                var stp = parseFloat($("#T_SalesTotalPrice").val().replace(/\$|\,/g, ''));
                if (sct <= 0 && stp <= 0) {
                    $.ligerDialog.warn('�۸��ܼ۲���Ϊ�գ�');
                    return false;
                }

                var sendtxt = "&pid=" + getparastr("pid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }


        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "Product.form.xhd", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'form', id: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }


                    if (!obj.category_id) {
                        obj.category_id = getparastr("categoryid", "");
                    }

                    if (obj.SupplierID == null || obj.SupplierID == undefined || obj.SupplierID == 0 || obj.SupplierID.length <= 0) {
                        obj.SupplierID = "";
                    }

                    if (obj.IsGold == null || obj.IsGold == undefined) { obj.IsGold = 0; }



                    $("#form1").ligerAutoForm({
                        labelWidth: 120, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '��Ʒ', type: 'group', icon: '',
                                rows: [
                                    [
                                     { display: "�������(����)", name: "T_BNumber", type: "text", options: "{width:180}", validate: "{required:true}", initValue: 1 }
                                    ],
                                   [
                                    { display: "��Ʒ����", name: "T_product_name", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.product_name },
                                    { display: "��Ʒ���", name: "T_product_category", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_category.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.category_id + "'}", validate: "{required:true}" }
                                   ],
                                   [
                                    { display: "�������", name: "T_StockPrice", type: "text", options: "{width:180,onChangeValue:function(value){ $('#T_StockPrice').val(toMoney(value));SetT_GoldTotal(); }}", validate: "{required:true}", initValue: toMoney(obj.StockPrice, "") },
                                    { display: "����(��)", name: "T_Weight", type: "text", options: "{width:180,onChangeValue:function(value){ $('#T_Weight').val(toMoney(value)); SetT_GoldTotal();  }}", validate: "{required:true}", initValue: toMoney(obj.Weight, "") }
                                   ],
                                   [
                                    { display: "������", name: "T_AttCosts", type: "text", options: "{width:180,onChangeValue:function(value){ $('#T_AttCosts').val(toMoney(value));SetT_CostsTotal(); }}", validate: "{required:true}", initValue: toMoney(obj.AttCosts, "") },
                                    { display: "��ʯ��", name: "T_MainStoneWeight", type: "text", options: "{width:180,onChangeValue:function(value){ $('#T_MainStoneWeight').val(toMoney(value)); }}", validate: "{required:false}", initValue: toMoney(obj.MainStoneWeight) }
                                   ],
                                   [
                                    { display: "��ʯ��", name: "T_AttStoneWeight", type: "text", options: "{width:180,onChangeValue:function(value){ $('#T_AttStoneWeight').val(toMoney(value)); }}", validate: "{required:false}", initValue: toMoney(obj.AttStoneWeight) },
                                    { display: "��ʯ��", name: "T_AttStoneNumber", type: "text", options: "{width:180,onChangeValue:function(value){ $('#T_AttStoneNumber').val(toMoney(value)); }}", validate: "{required:false}", initValue: toMoney(obj.AttStoneNumber) }
                                   ],
                                   [
                                    { display: "ʯ��", name: "T_StonePrice", type: "text", options: "{width:180,onChangeValue:function(value){ $('#T_StonePrice').val(toMoney(value));SetT_Totals(); }}", validate: "{required:false}", initValue: toMoney(obj.StonePrice) },
                                    { display: "���С��", name: "T_GoldTotal", type: "text", options: "{width:180,disabled:true,onChangeValue:function(value){ $('#T_GoldTotal').val(toMoney(value)); SetT_Totals(); }}", validate: "{required:true}", initValue: toMoney(obj.GoldTotal) }
                                   ],
                                  [
                                    { display: "����С��", name: "T_CostsTotal", type: "text", options: "{width:180,disabled:true,onChangeValue:function(value){   $('#T_CostsTotal').val(toMoney(value)); SetT_Totals(); }}", validate: "{required:true}", initValue: toMoney(obj.CostsTotal) },
                                    { display: "�ɱ��ܼ�", name: "T_Totals", type: "text", options: "{width:180,disabled:true,onChangeValue:function(value){ $('#T_Totals').val(toMoney(value)); }}", validate: "{required:true}", initValue: toMoney(obj.Totals) }
                                  ],
                                [
                                    { display: "��Ӧ��", name: "T_SupplierID", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_supplier.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.SupplierID + "'}", validate: "{required:false}" },
                                    { display: "������", name: "T_Sbarcode", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.Sbarcode) },
                                ],
                                   [{ display: "������", name: "T_BarCode", type: "text", options: "{width:180;}", validate: "{required:false}", initValue: (obj.BarCode) },
                                    { display: "Ʒ��", name: "T_GType", type: "select", options: "{width:180,onSelected:function(value){SetT_SalesTotalPrice(value);},data:[{id:0,text:'����'},{id:1,text:'�����'},{id:2,text:'Ӳ����'},{id:3,text:'K����'},{id:4,text:'��ʯ��'},{id:5,text:'��ɫ��ʯ��'},{id:6,text:'�����'},{id:7,text:'�ڼ���'},{id:8,text:'����'},{id:9,text:'������'}],selectBoxHeight:150, value:" + obj.IsGold + "}", validate: "{required:true}" }
                                   ],
                                 [
                                        { display: "���ۼ۸�", name: "T_SalesTotalPrice", type: "text", options: "{width:180,disabled:true,onChangeValue:function(value){   $('#T_SalesTotalPrice').val(toMoney(value));  ; }}", validate: "{required:true}", initValue: toMoney(obj.SalesTotalPrice) },
                                        { display: "���۹���", name: "T_SalesCostsTotal", type: "text", options: "{width:180,disabled:true,onChangeValue:function(value){ $('#T_SalesCostsTotal').val(toMoney(value)); }}", validate: "{required:true}", initValue: toMoney(obj.SalesCostsTotal) }
                                 ],
                                [
                                      { display: "��ǩ�۸�", name: "T_PriceTag", type: "text", options: "{width:180,disabled:true,onChangeValue:function(value){   $('#T_PriceTag').val(toMoney(value));  ; }}", validate: "{required:true}", initValue: toMoney(obj.PriceTag) },
                                      { display: "һ�ڼ�", name: "T_FixedPrice", type: "text", options: "{width:180,disabled:true,onChangeValue:function(value){ $('#T_FixedPrice').val(toMoney(value)); }}", validate: "{required:true}", initValue: toMoney(obj.FixedPrice) }
                                ],
                                 [
                                    { display: "֤����", name: "T_CertificateNo", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.CertificateNo) },
                                    { display: "Ȧ���ִ�", name: "T_Circle", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.Circle) }
                                 ],
                                   [
                                    { display: "��ע", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remarks }
                                   ]
                                ]
                            }
                        ]
                    });
                    $("#T_BarCode").attr("maxlength", "15");
                    if (oaid.length > 0) {
                        if (obj.StockPrice == null || obj.StockPrice == undefined || obj.StockPrice == 0) { $("#T_StockPrice").val(toMoney(0.00)); }
                        if (obj.AttCosts == null || obj.AttCosts == undefined || obj.AttCosts == 0) { $("#T_AttCosts").val(toMoney(0.00)); }
                        if (obj.status != 1) {
                            $("#T_BarCode").attr("disabled", "disabled");
                        }
                        //�Ƴ�����һ��
                        $(".l-group").children("dl")[0].remove();
                    }
                }
            });


        }

        //�ɱ��ܼ�=�����С��+����С��+ʯ�ۣ�
        function SetT_Totals() {
            var total = 0;
            var T_GoldTotal = $("#T_GoldTotal").val();
            var T_CostsTotal = $("#T_CostsTotal").val();
            var T_StonePrice = $("#T_StonePrice").val();
            total = parseFloat(T_StonePrice.replace(/\$|\,/g, '')) + parseFloat(T_CostsTotal.replace(/\$|\,/g, '')) + parseFloat(T_GoldTotal.replace(/\$|\,/g, ''));
            $("#T_Totals").val(toMoney(total));
            var gtype = 0;
            if ($("#T_GType").val() == "��") { gtype = 1; }
            SetT_SalesTotalPrice(gtype);

        }

        //���ۼ۸�=���ɱ��ܼ�*2.5��
        function SetT_SalesTotalPrice(value) {

            var T_Totals = $("#T_Totals").val();
            //��2���ƽ�����Ʒ���������۹��ѣ�����С��*3��
            if (value == 1) {
                var T_CostsTotal = $("#T_CostsTotal").val();
                var total = parseFloat(T_CostsTotal.replace(/\$|\,/g, '')) * 3;
                $("#T_SalesCostsTotal").val(toMoney(total));
                $("#T_SalesTotalPrice").val(0.00);
            } else {
                //�ǻƽ�����Ʒ��K�𣬽�������ʯ���������۵��ۣ��ɱ��ܼ�*2.5��
                var total = parseFloat(T_Totals.replace(/\$|\,/g, '')) * 2.5;
                $("#T_SalesCostsTotal").val(0.00);
                $("#T_SalesTotalPrice").val(toMoney(total));
            }

            //���ñ�ǩ�۸��
            SetT_PriceTag();
        }


        //�������*����
        function SetT_GoldTotal() {
            var total = 0;
            var T_StockPrice = $("#T_StockPrice").val();
            var T_Weight = $("#T_Weight").val();
            total = parseFloat(T_StockPrice.replace(/\$|\,/g, '')) * parseFloat(T_Weight.replace(/\$|\,/g, ''));
            $("#T_GoldTotal").val(toMoney(total));
            SetT_CostsTotal();
        }

        //������*����
        function SetT_CostsTotal() {
            var total = 0;
            var T_AttCosts = $("#T_AttCosts").val();
            var T_Weight = $("#T_Weight").val();
            total = parseFloat(T_AttCosts.replace(/\$|\,/g, '')) * parseFloat(T_Weight.replace(/\$|\,/g, ''));
            $("#T_CostsTotal").val(toMoney(total));
            SetT_Totals();
        }

        //���ñ�ǩ�۸�
        function SetT_PriceTag() {
            var ptype = parseInt($("#T_GType_val").val());

            //����С��
            var T_CostsTotal = parseFloat($("#T_CostsTotal").val().replace(/\$|\,/g, ''));
            //����
            var T_Weight = parseFloat($("#T_Weight").val().replace(/\$|\,/g, ''));
            //�ɱ���
            var T_Totals = parseFloat($("#T_Totals").val().replace(/\$|\,/g, ''));
            $("#T_PriceTag").val(0);
            $("#T_FixedPrice").val(0);
            switch (ptype) {
                case 1: //�����
                    if (T_CostsTotal <= 2) { $("#T_PriceTag").val(toMoney(parseFloat(T_Weight * 20))); }
                    else if (T_CostsTotal <= 22) { $("#T_PriceTag").val(toMoney(parseFloat(T_CostsTotal * 6))); }
                    else if (T_CostsTotal <= 40) { $("#T_PriceTag").val(toMoney(parseFloat(T_CostsTotal * 4))); }
                    break;
                case 2:  //Ӳ����
                    if (T_Weight <= 5) { $("#T_FixedPrice").val(toMoney(parseFloat(T_Totals * 0.5))); }
                    else if (T_Weight > 5 && T_CostsTotal <= 15) { $("#T_PriceTag").val(toMoney(parseFloat(T_CostsTotal * 6))); }
                    else if (T_Weight > 5 && T_CostsTotal <= 40) { $("#T_PriceTag").val(toMoney(parseFloat(T_CostsTotal * 4))); }
                    break;
                case 3://K��
                case 4://��ʯ
                    $("#T_FixedPrice").val(toMoney(parseFloat(T_Totals * 2.5)));
                    break;
                case 5://��ɫ��ʯ
                case 6://���
                    $("#T_FixedPrice").val(toMoney(parseFloat(T_Totals * 3)));
                    break;
                case 7://�ڼ�
                    $("#T_FixedPrice").val(toMoney(parseFloat(T_Totals * 6)));
                    break;
                case 8://����
                    $("#T_PriceTag").val(toMoney(parseFloat(T_CostsTotal * 4)));
                    break;
                case 9: //������
                    $("#T_PriceTag").val(toMoney(parseFloat(T_CostsTotal * 2.3)));
                    break;
                default:
                    break;
            }

        }



    </script>
</head>
<body>
    <div style="padding: 10px;">
        <form id="form1" onsubmit="return false">
        </form>
    </div>
</body>
</html>
