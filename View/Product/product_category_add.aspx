<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/webuploader.css" rel="stylesheet" />
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
    <script src="../JS/webuploader/webuploader.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        //图标
        var winicons, currentComboBox;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            loadForm(getparastr("cid"));

        });



        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("cid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {

            f_uploader();
            $.ajax({
                type: "GET",
                url: "Product_category.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {

                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_category_name").val(obj.product_category);
                    $("#T_category_icon").val(obj.product_icon);
                    $("#T_CodingBegins").val(obj.CodingBegins);
                    if (!obj.product_icon)
                        obj.product_icon = 'images/icon/21.png ';
                    $("#menuicon").attr("src", "../" + obj.product_icon);
                    $("#T_category_parent").ligerComboBox({
                        width: 180,
                        selectBoxWidth: 180,
                        selectBoxHeight: 180,
                        valueField: 'id',
                        textField: 'text',
                        value: obj.parentid,
                        treeLeafOnly: false,
                        tree: {
                            url: 'Product_category.tree.xhd?qxzg=1&rnd=' + Math.random(),
                            idFieldName: 'id',
                            checkbox: false
                        }
                    })


                }
            });
        }


        function startup() {
            uploader.upload();
        }

        var uploader;
        function f_uploader() {
            uploader = WebUploader.create({
                swf: "../js/webuploader/uploader.swf",
                server: "CRM_contract_atta.uploadAtth.xhd?savefile=category",
                pick: "#uploadimg",
                resize: false,
                auto: true,
            });

            uploader.on('fileQueued', function (file) {

            });

            uploader.on('uploadSuccess', function (file, response) {
                var ico = response.Message;
                $("#T_category_icon").val(".." + ico);
                $("#menuicon").attr("src", ".." + ico);
            });

            uploader.on('uploadError', function (file) {
            });

            uploader.on('uploadFinished', function (file) {

            });
        }

    </script>


</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">
        <table border="0" cellpadding="3" cellspacing="1" style="background: #fff; width: 400px;" class="aztable">

            <tr>
                <td height="23" style="width: 85px" colspan="2">

                    <div align="left" style="width: 62px">类别名称：</div>
                </td>
                <td height="23">

                    <input type="text" id="T_category_name" name="T_category_name" ltype="text" ligerui="{width:180}" validate="{required:true}" />

                </td>
            </tr>
            <tr>
                <td height="23" colspan="2">

                    <div align="left" style="width: 62px">上级类别：</div>
                </td>
                <td height="23">
                    <input type="text" id="T_category_parent" name="T_category_parent" validate="{required:true}" />
                </td>
            </tr>
            <%--            <tr>
                <td height="23" colspan="2">
                    <div align="left" style="width: 62px">条形码头：</div>
                </td>
                <td height="23">
                    <input type="text" id="T_CodingBegins" name="T_CodingBegins" ltype="text" validate="{required:true}" />
                </td>
            </tr>--%>
            <tr>
                <td height="23" style="width: 62px">

                    <div align="left" style="width: 62px">类别图标：</div>
                </td>
                <td height="23" style="width: 27px" class="sbimg">
                    <img id="menuicon" style="width: 25px; height: 25px;" />
                </td>
                <td height="23" style="width: 27px">

                    <div id="uploadimg">请点击这个位置上传图标</div>

                    <input type="hidden" id="T_category_icon" name="T_category_icon" />
                </td>
            </tr>


        </table>
    </form>
</body>
</html>
