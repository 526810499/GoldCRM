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
                url: "Product.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
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
                                display: '商品', type: 'group', icon: '',
                                rows: [
                                   [
                                    { display: "商品名称", name: "T_product_name", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.product_name },
                                    { display: "商品类别", name: "T_product_category", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_category.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.category_id + "'}", validate: "{required:true}" }
                                   ],
                                   [
                                    { display: "重量(克)", name: "T_Weight", type: "text", options: "{width:180}", validate: "{required:true}", initValue: toMoney(obj.Weight, "") }
                                   ],
                                   [
                                    { display: "工费小计", name: "T_CostsTotal", type: "text", options: "{width:180,disabled:true}", validate: "{required:true}", initValue: toMoney(obj.CostsTotal) },
                                    { display: "主石重", name: "T_MainStoneWeight", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.MainStoneWeight) }
                                   ],
                                   [
                                    { display: "附石重", name: "T_AttStoneWeight", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.AttStoneWeight) },
                                    { display: "附石数", name: "T_AttStoneNumber", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.AttStoneNumber) }
                                   ],
                                [
                                    { display: "供应商", name: "T_SupplierID", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_supplier.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.SupplierID + "'}", validate: "{required:false}" },
                                    { display: "出厂码", name: "T_Sbarcode", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.Sbarcode) },
                                ],
                                   [{ display: "条形码", name: "T_BarCode", type: "text", options: "{width:180,disabled:true}", validate: "{required:false}", initValue: (obj.BarCode) },
                                    { display: "是否黄金类", name: "T_GType", type: "select", options: "{width:180,,data:[{id:0,text:'否'},{id:1,text:'是'}],selectBoxHeight:50, value:" + obj.IsGold + "}", validate: "{required:true}" }
                                   ],
                                  [
                                    { display: "证书编号", name: "T_CertificateNo", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.CertificateNo) },
                                    { display: "圈号手寸", name: "T_Circle", type: "text", options: "{width:180}", validate: "{required:false}", initValue: (obj.Circle) }
                                  ],
                                   [
                                    { display: "备注", name: "T_Remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.Remark }
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
