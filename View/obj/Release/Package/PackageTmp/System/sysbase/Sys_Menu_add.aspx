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

            //$("#T_Contract_name").focus();
            $("form").ligerForm();
            $("#menuicon").bind("click", f_selectContact);
            $("#T_menu_icon").bind("click", f_selectContact);

            if (getparastr("menuid")) {
                loadForm(getparastr("menuid"));
            }
            else {
                initcomb();
            }

            jiconlist = $("body > .iconlist:first");
            if (!jiconlist.length) jiconlist = $('<ul class="iconlist"></ul>').appendTo('body');
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&menutype=sys&menuid=" + getparastr("menuid") + "&appid=" + getparastr("appid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "Sys_Menu.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {

                    }

                    if (!obj.isMobile)
                        obj.isMobile = 0;
                    //alert(obj.constructor); //String 构造函数
                    $("#T_menu_name").val(obj.Menu_name);
                    $("#T_menu_url").val(obj.Menu_url);
                    $("#T_menu_icon").val(obj.Menu_icon);
                    $("#T_menu_order").val(obj.Menu_order);
                    $("#T_menu_id").val(obj.Menu_id);
                    $("#menuicon").attr("src", "../../" + obj.Menu_icon);

                    initcomb(obj.parentid);
                }
            });
        }
        function initcomb(value) {
            var appid = getparastr("appid")

            $("#T_menu_parent").ligerComboBox({
                width: 300,
                selectBoxWidth: 300,
                selectBoxHeight: 180,
                valueField: 'id',
                textField: 'text',
                initValue: value,
                treeLeafOnly: true,
                tree: {
                    url: 'Sys_Menu.SysTreeV2.xhd?appid=' + appid + '&rnd=' + Math.random(),
                    idFieldName: 'id',
                    checkbox: false
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

                    <div align="left" style="width: 62px">目录ID：</div>
                </td>
                <td height="23">

                    <input type="text" id="T_menu_id" name="T_menu_id" ltype="text" ligerui="{width:300}" validate="{required:true}" /></td>
            </tr>

            <tr>
                <td height="23" style="width: 85px" colspan="2">

                    <div align="left" style="width: 62px">目录名称：</div>
                </td>
                <td height="23">

                    <input type="text" id="T_menu_name" name="T_menu_name" ltype="text" ligerui="{width:300}" validate="{required:true}" />

                </td>
            </tr>
            <tr>
                <td height="23" colspan="2">

                    <div align="left" style="width: 62px">上级目录：</div>
                </td>
                <td height="23">

                    <input type="text" id="T_menu_parent" name="T_menu_parent" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td height="23" colspan="2">

                    <div align="left" style="width: 62px">链接地址：</div>
                </td>
                <td height="23">

                    <input type="text" id="T_menu_url" name="T_menu_url" ltype="text" ligerui="{width:300}" />


                </td>
            </tr>
            <tr>
                <td height="23" style="width: 62px">

                    <div align="left" style="width: 62px">目录图标：</div>
                </td>
                <td height="23" style="width: 27px">
                    <img id="menuicon" style="width: 16px; height: 16px;" /></td>
                <td height="23">
                    <input type="text" id="T_menu_icon" name="T_menu_icon" ltype="text" ligerui="{width:300}" />
                </td>
            </tr>
            <tr>
                <td height="23" colspan="2">

                    <div align="left" style="width: 62px">排序行号：</div>
                </td>
                <td height="23">
                    <input type="text" id="T_menu_order" name="T_menu_order" value="20" ltype='spinner' ligerui="{type:'int',width:300}" /></td>
            </tr>



        </table>
    </form>
</body>
</html>
