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
    <script src="../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        //图标
        var jiconlist, winicons, currentComboBox;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();
            loadForm(getparastr("id"));
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "Product_supplier.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {

                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_product_warehouse").val(obj.product_supplier);
                    $("#T_contact").val(obj.contact);
                    $("#T_contactphone").val(obj.contactphone);
                    $("#T_Adress").val(obj.address);
                    $("#T_Remark").val(obj.remark);
                }
            });
        }



    </script>


</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">
        <table border="0" cellpadding="3" cellspacing="1" style="background: #fff; width: 400px;" class="aztable">

            <tr>
                <td height="23" style="width: 85px" colspan="2">

                    <div align="left" style="width: 62px">名称：</div>
                </td>
                <td height="23">

                    <input type="text" id="T_product_warehouse" name="T_product_warehouse" ltype="text" ligerui="{width:180}" validate="{required:true}" />

                </td>
            </tr>
            <tr>
                <td height="23" colspan="2">

                    <div align="left" style="width: 62px">联系人：</div>
                </td>
                <td height="23">
                    <input type="text" id="T_contact" name="T_contact" validate="{required:true}" ltype="text" ligerui="{width:180}" />
                </td>
            </tr>
            <tr>
                <td height="23" colspan="2">

                    <div align="left" style="width: 62px">联系电话：</div>
                </td>
                <td height="23">
                    <input type="text" id="T_contactphone" name="T_contactphone" validate="{required:true}" ltype="text" ligerui="{width:180}" />
                </td>
            </tr>
            <tr>
                <td>
                    <div align="right" style="width: 61px">
                        联系地址：
                    </div>
                </td>
                <td colspan="5">
                    <input type="text" id="T_Adress" name="T_Adress" ltype="text" ligerui="{width:403}" /></td>
            </tr>
            <tr>
                <td>
                    <div align="right" style="width: 61px">
                        备注：
                    </div>
                </td>
                <td colspan="5">
                    <input type="text" id="T_Remark" name="T_Remark" ltype="text" ligerui="{width:403}" /></td>
            </tr>
        </table>
    </form>
</body>
</html>
