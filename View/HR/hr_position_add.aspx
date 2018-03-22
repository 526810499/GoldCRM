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

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            if (getparastr("pid")) {
                loadForm(getparastr("pid"));
            }
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("pid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "hr_position.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {

                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_position").val(obj.position_name);
                    $("#T_order").val(obj.position_order);
                    $("#T_level").val(obj.position_level);
                }
            });
        }

    </script>
</head>
<body style="margin: 5px 5px 5px 5px">
    <form id="form1" onsubmit="return false">
        <div>
            <table align="left" border="0" cellpadding="3" cellspacing="1"
                style="background: #fff; width: 320px;" class="aztable">

                <tr>
                    <td width="65px">职务名称：</td>
                    <td>

                        <input type="text" id="T_position" name="T_position" ltype="text" ligerui="{width:180}" validate="{required:true}" />
                    </td>
                </tr>

                <tr>
                    <td width="65px">职务级别：</td>
                    <td>
                        <input type="text" id="T_level" name="T_level" ltype='spinner' ligerui="{type:'int',width:180}" value="20" validate="{required:true}" /></td>
                </tr>
                <tr>
                    <td>行号：</td>
                    <td>

                        <input type="text" id="T_order" name="T_order" ltype='spinner' ligerui="{type:'int',width:180}" value="20" validate="{required:true}" />

                    </td>
                </tr>


            </table>
        </div>
    </form>
</body>
</html>
