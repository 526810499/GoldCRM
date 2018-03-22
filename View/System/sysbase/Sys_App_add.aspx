<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        //图标
        var jiconlist, winicons, currentComboBox;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("form").ligerForm();
            $("#menuicon").bind("click", f_selectContact);
            $("#T_menu_icon").bind("click", f_selectContact);
            var id = getparastr("id");
            if (id) {
                $("#hid").val(id);
                loadForm(id);
            }


            jiconlist = $("body > .iconlist:first");
            if (!jiconlist.length) jiconlist = $('<ul class="iconlist"></ul>').appendTo('body');
        });

        function f_save() {
            if ($(form1).valid()) {
                return $("form :input").fieldSerialize();
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "Sys_App.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);

                    //alert(obj.constructor); //String 构造函数
                    $("#T_menu_name").val(obj.App_name);

                    $("#T_menu_icon").val(obj.App_icon);
                    $("#T_menu_order").val(obj.App_order);
                    $("#T_menu_id").val(obj.id);
                    $("#menuicon").attr("src", "../../" + obj.App_icon);


                }
            });
        }

        function f_selectContact() {
            if (winicons) {
                winicons.show();
                //return;
            }
            winicons = $.ligerDialog.open({
                title: '选取图标',
                target: jiconlist,
                width: 400, height: 250, modal: true
            });
            if (!jiconlist.attr("loaded")) {
                $.ajax({
                    url: "Sys_base.GetIcons.xhd", type: "get",
                    data: { icontype: "1", rnd: Math.random() },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var obj = eval(data);
                        //alert(obj.length);
                        for (var i = 0, l = obj.length; i < l; i++) {
                            var src = obj[i];
                            var reg = /(images\\icon)(.+)/;
                            var match = reg.exec(src);
                            jiconlist.append("<li><img src='../../images/icon/" + src.filename + "' onclick=\"ClickImg(this)\" /></li>");
                            if (!match) continue;
                        }

                        $(".iconlist li").live('mouseover', function () {
                            $(this).addClass("over");
                        }).live('mouseout', function () {
                            $(this).removeClass("over");
                        });

                        jiconlist.attr("loaded", true);
                    }
                });
            }
        }

        function ClickImg(v) {
            var src = $(v).attr("src");
            $("#menuicon").attr("src", "../../images/icon/" + src.split('images/icon/')[1]);
            $("#T_menu_icon").val("images/icon/" + src.split('images/icon/')[1]);

            winicons.close();
        }
    </script>
    <style type="text/css">
        .iconlist {
            width: 360px;
            padding: 3px;
        }

            .iconlist li {
                border: 1px solid #FFFFFF;
                float: left;
                display: block;
                padding: 2px;
                cursor: pointer;
            }

                .iconlist li.over {
                    border: 1px solid #516B9F;
                }

                .iconlist li img {
                    height: 16px;
                    height: 16px;
                }
    </style>

</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">
        <table border="0" cellpadding="3" cellspacing="1" style="background: #fff; width: 400px;">

            <tr>
                <td height="23" style="width: 85px" colspan="2">

                    <div align="left" style="width: 62px">AppID：</div>
                </td>
                <td height="23">
                    <input type="hidden" id="hid" name="hid" />
                    <input type="text" id="T_menu_id" name="T_menu_id" ltype="text" ligerui="{width:300}" validate="{required:true}" /></td>
            </tr>

            <tr>
                <td height="23" style="width: 85px" colspan="2">

                    <div align="left" style="width: 62px">名称：</div>
                </td>
                <td height="23">

                    <input type="text" id="T_menu_name" name="T_menu_name" ltype="text" ligerui="{width:300}" validate="{required:true}" />

                </td>
            </tr>


            <tr>
                <td height="23" style="width: 62px">

                    <div align="left" style="width: 62px">图标：</div>
                </td>
                <td height="23" style="width: 27px">
                    <img id="menuicon" style="width: 16px; height: 16px;" /></td>
                <td height="23">
                    <input type="text" id="T_menu_icon" name="T_menu_icon" ltype="text" ligerui="{width:300}" />
                </td>
            </tr>
            <tr>
                <td height="23" colspan="2">

                    <div align="left" style="width: 62px">排序号：</div>
                </td>
                <td height="23">
                    <input type="text" id="T_menu_order" name="T_menu_order" value="20" ltype='spinner' ligerui="{type:'int',width:300}" /></td>
            </tr>



        </table>
    </form>
</body>
</html>
