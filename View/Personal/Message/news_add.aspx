<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../../JS/XHD.js" type="text/javascript"></script>

    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <script type="text/javascript" charset="utf-8" src="../../ueditor/ueditor.config.js"></script>
    <script src="../../ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <script src="../../ueditor/lang/zh-cn/zh-cn.js" type="text/javascript"></script>
    <link href="../../ueditor/themes/default/css/ueditor.css" rel="stylesheet" />

    <script type="text/javascript">
        var editor;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("form").ligerForm();
            editor = UE.getEditor('editor', {
                //initialFrameWidth: null,
                //initialFrameHeight: 293,
                toolbars: [
                   ['fullscreen', 'source', '|', 'undo', 'redo', '|',
                    'bold', 'italic', 'underline', 'fontborder', 'strikethrough', 'superscript', 'subscript', 'removeformat', 'formatmatch', 'autotypeset', 'blockquote', 'pasteplain', '|',
                    'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
                    'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
                    'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
                    'directionalityltr', 'directionalityrtl', 'indent', '|',
                    'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|', 'touppercase', 'tolowercase', '|',
                    'link', 'unlink', 'anchor', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|',
                    'simpleupload', 'insertimage', 'emotion', 'scrawl', 'music', 'map', 'gmap', 'insertcode', 'pagebreak', 'template', 'background', '|',
                    'horizontal', 'date', 'time', 'spechars', '|',
                    'inserttable', 'deletetable', 'insertparagraphbeforetable', 'insertrow', 'deleterow', 'insertcol', 'deletecol', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols', '|',
                    'print', 'preview', 'searchreplace', 'help', 'drafts'
                   ]
                ],
                autoHeightEnabled: false
            });
            if (getparastr("nid")) {
                loadForm(getparastr("nid"));
            }

            onResize();
            $(window).resize(function () {
                onResize();
            });
        })

        function onResize()
        {
            var winH = $(window).height();
            var winW = $(window).width();            

            editor.ready(function () {
                $('.edui-editor').width(winW - 20);
                $('#edui1_iframeholder').width(winW - 20);
                editor.setHeight(winH - 170);
            });            
        }

        function f_save() {
            if ($(form1).valid()) {
                var arr = [];
                arr.push(UE.getEditor('editor').getContent());
                var sendtxt = "&nid=" + getparastr("nid") + "&T_content=" + escape(arr);
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "public_news.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {

                    }

                    //alert(obj.constructor); //String 构造函数
                    $("#T_title").val(obj.news_title);

                    //editor_a.setContent($('#content').val());  //赋值给UEditor
                    editor.ready(function () {
                        editor.setContent(myHTMLDeCode(obj.news_content));
                    });

                }
            });

        }
    </script>
   
</head>
<body style="overflow: hidden;">
    <form id="form1" onsubmit="return false">
        <div style="position: relative; z-index: 900;">


            <table style="width: 650px"  class="aztable">
                <tr>
                    <td style="width: 85px">
                        <div style="width: 80px; text-align: right;">新闻标题：</div>
                    </td>
                    <td>
                        <input type="text" id="T_title" name="T_title" ltype="text" ligerui="{width:300}" validate="{required:true}" /></td>
                </tr>
            </table>
             <script id="editor"></script>
        </div>

        

    </form>
</body>

</html>
