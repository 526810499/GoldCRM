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

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            loadForm(getparastr("id"));


        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&id=" + getparastr("id");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "Sys_City.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {

                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_City").val(obj.City);
                    $("#T_order").val(obj.City_order);
                    //$("#T_Parent").ligerGetComboBoxManager().selectValue(obj.parentid);
                    $("#T_Parent").ligerComboBox({
                        width: 180,
                        url: 'Sys_Provinces.combo.xhd',
                        value: obj.Provinces_id?obj.Provinces_id:getparastr("pid")
                    });
                }
                    
            });
        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">

        <table align="left" border="0" cellpadding="3" cellspacing="1"
            style="background: #fff; width: 320px;" class="aztable">

            <tr>
                <td width="65px">
                   
                        省份：
                </td>
                <td>
                   
                        <input type="text" id="T_Parent" name="T_Parent" validate="{required:true}" />
                    
                </td>
            </tr>
            <tr>
                <td>

                    <div align="left" style="width: 62px">城市名：</div>
                </td>
                <td>
                    
                        <input type="text" id="T_City" name="T_City" ltype='text' ligerui="{width:180}" validate="{required:true}" />
                    
                </td>
            </tr>
            <tr>
                <td>

                    <div align="left" style="width: 62px">排序：</div>
                </td>
                <td>
                    <input type="text" id="T_order" name="T_order" value="10" ltype='spinner' ligerui="{width:180,type:'int'}" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
        </table>

    </form>
</body>
</html>
