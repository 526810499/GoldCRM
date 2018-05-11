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
    <script src="../JS/XHD.js?v=4" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            loadForm(getparastr("pid"));
        });

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
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '��Ʒ', type: 'group', icon: '',
                                rows: [
                                   [
                                    { display: "��Ʒ����", name: "T_product_name", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.product_name },
                                    { display: "��Ʒ���", name: "T_product_category", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_category.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.category_id + "'}", validate: "{required:true}" }
                                   ],
                                   [
                                    { display: "����(��)", name: "T_Weight", type: "text", options: "{width:180}", validate: "{required:true}", initValue: toMoney(obj.Weight, "") }
                                   ],
                                   [
                                    { display: "����С��", name: "T_CostsTotal", type: "text", options: "{width:180,disabled:true}", validate: "{required:true}", initValue: toMoney(obj.CostsTotal) },
                                    { display: "��ʯ��", name: "T_MainStoneWeight", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.MainStoneWeight) }
                                   ],
                                   [
                                    { display: "��ʯ��", name: "T_AttStoneWeight", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.AttStoneWeight) },
                                    { display: "��ʯ��", name: "T_AttStoneNumber", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.AttStoneNumber) }
                                   ],
                                [
                                    { display: "��Ӧ��", name: "T_SupplierID", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_supplier.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.SupplierID + "'}", validate: "{required:false}" },
                                    { display: "������", name: "T_Sbarcode", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.Sbarcode) },
                                ],
                                   [{ display: "������", name: "T_BarCode", type: "text", options: "{width:180,disabled:true}", validate: "{required:false}", initValue: (obj.BarCode) },
                                    { display: "�Ƿ�ƽ���", name: "T_GType", type: "select", options: "{width:180,,data:[{id:0,text:'��'},{id:1,text:'��'}],selectBoxHeight:50, value:" + obj.IsGold + "}", validate: "{required:true}" }
                                   ],
                                  [
                                    { display: "֤����", name: "T_CertificateNo", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.CertificateNo) },
                                    { display: "Ȧ���ִ�", name: "T_Circle", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.Circle) }
                                  ],
                                   [
                                    { display: "��ע", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.Remark }
                                   ]
                                ]
                            }
                        ]
                    });

                }
            });


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
