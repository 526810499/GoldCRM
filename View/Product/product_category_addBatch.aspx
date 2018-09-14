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
    <script src="../JS/XHD.js?v=21" type="text/javascript"></script>
    <script src="../JS/webuploader/webuploader.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        //图标
        var winicons, currentComboBox;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("form").ligerForm();

            loadForm();
            $("#ctlBtn").click(function () { startup(); });
        });



        function f_save() {
            var category_attr = $("#category_attr").val();
            if (category_attr == null || category_attr.length <= 0) {
                $.ligerDialog.warn("分类属性必须关联");
                return false;
            }

            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("cid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm() {

            $("#T_category_parent").ligerComboBox({
                width: 180,
                selectBoxWidth: 180,
                selectBoxHeight: 180,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: 'Product_category.tree.xhd?qxzg=1&rnd=' + Math.random(),
                    idFieldName: 'id',
                    checkbox: false
                }
            });
 
        }

        function startup() {

            var stype = $("#T_category_parent_val").val();
            if (stype.length <= 0) {
                $.ligerDialog.warn('请选择上传主类！');
                return false;
            }

            var fileObj = document.getElementById("excel").files[0];
            if (fileObj == null) {
                $.ligerDialog.warn('请选择上传文件！');
                return false;
            }

            $.ligerDialog.confirm("确认上传主类型为【" + $("#T_category_parent").val() + "】 ！！！", function (yes) {

                if (!yes) { return false; }

                var formFile = new FormData();
                formFile.append("excelFile", fileObj);

                var data = formFile;
                $.ajax({
                    url: "Product_category.ImportCategory.xhd?rand" + Math.random() + "&pid=" + stype,
                    data: data,
                    async: true,
                    type: "Post",
                    dataType: "json",
                    cache: false,
                    processData: false,
                    contentType: false,
                    success: function (result) {
                        var msg = result.Message;
                        $("#urs").text(msg);
                    }, error: function (jqXHR, textStatus, errorThrown) {
                        $.ligerDialog.warn('上传出错请重试！' + jqXHR.responseText);
                    }
                });
            });
        }

    </script>


</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">
        <table border="0" cellpadding="3" cellspacing="1" style="background: #fff; width: 400px;" class="aztable">

            <tr>
                <td height="23">

                    <div align="left">上传模板：</div>
                </td>
                <td>

                    <a href="../file/template/分类_批量模板.xlsx" target="_blank" style="padding-right: 10px">分类批量模板</a>

                </td>
            </tr>
            <tr>
                <td height="23" >

                    <div align="left" style="width: 62px">上级类别：</div>
                </td>
                <td height="23">
                    <input type="text" id="T_category_parent" name="T_category_parent" validate="{required:true}" />
                </td>
            </tr>
 
            <tr>
                <td height="23" style="width: 62px">

                    <div align="left" style="width: 62px">选择文件：</div>
                </td>
                <td>

                    <table>
                        <tr>

                            <td>
                                <input id="excel" type="file" name="excelFile" accept=".xls,.xlsx" />

                            </td>
                            <td style="width: 10px"></td>
                            <td>
                                <button id="ctlBtn" class="btn btn-default">开始上传</button>
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
            <tr style="padding-top: 10px">
                <td>上传结果：
                </td>
                <td>
                    <textarea rows="28" cols="130" id="urs">


                    </textarea>


                </td>

            </tr>


        </table>
    </form>
</body>
</html>
