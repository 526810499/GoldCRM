<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            loadForm(getparastr("pid"));
        });

        function f_save() {

            if ($(form1).valid()) {
                var T_product_category = $("#T_product_category_val").val();
                if (T_product_category.length <= 0) {
                    $.ligerDialog.warn('商品分类需选择');
                    return false;
                }

                var T_StockPrice = parseFloat($("#T_StockPrice").val());
                var T_Weight = parseFloat($("#T_Weight").val());

                if (T_StockPrice <= 0) {
                    $.ligerDialog.warn('进货金价不能为小于0！');
                    return false;
                }
                if (T_Weight <= 0) {
                    $.ligerDialog.warn('重量不能为小于0！');
                    return false;
                }

                var sct = parseFloat($("#T_SalesCostsTotal").val().replace(/\$|\,/g, ''));
                var stp = parseFloat($("#T_SalesTotalPrice").val().replace(/\$|\,/g, ''));
                if (sct <= 0 && stp <= 0) {
                    $.ligerDialog.warn('价格总价不能为空！');
                    return false;
                }

                var sendtxt = "&pid=" + getparastr("pid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }


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
                                    { display: "商品类别", name: "T_category_name", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.category_name },
                                   ],
                                   [
                                     { display: "重量(克)", name: "T_Weight", type: "text", options: "{width:180}", validate: "{required:true}", initValue: toMoney(obj.Weight) }
                                    ,
                                     { display: "主石重", name: "T_MainStoneWeight", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.MainStoneWeight) }
                                   ],
                                   [
                                    { display: "附石重", name: "T_AttStoneWeight", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.AttStoneWeight) },
                                    { display: "附石数", name: "T_AttStoneNumber", type: "text", options: "{width:180}", validate: "{required:false}", initValue: toMoney(obj.AttStoneNumber) }
                                   ],
                                  [
                                   { display: "销售工费", name: "T_SalesCostsTotal", type: "text", options: "{width:180}", validate: "{required:true}", initValue: toMoney(obj.SalesCostsTotal) }
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
