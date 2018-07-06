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
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">

        var uploader;
        $(function () {

            $("#imtypes").ligerComboBox({
                data: [
                { text: '请选择类型', id: '' },
                { text: '珠宝类', id: '0' },
                { text: '黄金类', id: '1' },
                { text: 'K进类', id: '2' },
                ], valueFieldID: 'simtypes', initValue: ''
            });
            $("#ctlBtn").click(function () { startup(); });
        });


        function startup() {

            var stype = $("#simtypes").val();
            if (stype.length <= 0) {
                $.ligerDialog.warn('请选择上传类型！');
                return false;
            }
            var fileObj = document.getElementById("excel").files[0];
            if (fileObj == null) {
                $.ligerDialog.warn('请选择上传文件！');
                return false;
            }

            $.ligerDialog.confirm("确认上传类型为【" + $("#imtypes").val() + "】&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br\><br\> 如果有新增商品分类请在分类列表中新增后在上传！！！", function (yes) {

                if (!yes) { return false;}
              
                var formFile = new FormData();
                formFile.append("excelFile", fileObj);

                var data = formFile;
                $.ajax({
                    url: "Product.ImportProduct.xhd?rand" + Math.random() + "&pid=" + getparastr("pid", "") + "&isAddTemp=" + getparastr("isAddTemp", 0) + "&addtype=" + stype,
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
        <fieldset style="border-style: solid; margin: 10px; padding: 5px;">
             
            <table border="0" cellpadding="3" cellspacing="1" style="background: #fff; width: 400px;" class="aztable">

                <tr>
                    <td height="23">

                        <div align="left">上传模板：</div>
                    </td>
                    <td colspan="2">

                        <a href="../file/template/珠宝类_入库模板.xlsx" target="_blank" style="padding-right: 10px">珠宝类</a>
                        <a href="../file/template/黄金类_入库模板.xlsx" target="_blank" style="padding-right: 10px">黄金类</a>
                        <a href="../file/template/K金类_入库模板.xlsx" target="_blank" style="padding-right: 10px">K金类</a>
                    </td>
                </tr>
                <tr>
                    <td height="23">

                        <div align="left">上传类型：</div>
                    </td>
                    <td colspan="2">

                        <input id='imtypes' name="imtypes" type='text' />

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
        </fieldset>

    </form>
</body>
</html>
