<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../../CSS/input.css" rel="stylesheet" /> 
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/json2.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            if (getparastr("id")) {
                loadForm(getparastr("id"));
            }
        });


        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "Personal_Calendar.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { calendarid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        
                    }
                    //alert(obj.constructor); //String 构造函数
                    //alert(obj.IsAllDayEvent);
                    $("input[type=radio][value=" + obj.IsAllDayEvent + "]").attr("checked", 'checked');
                    $("#T_starttime").val(formatTimebytype(obj.StartTime, 'yyyy-MM-dd hh:mm'));
                    $("#T_endtime").val(formatTimebytype(obj.EndTime, 'yyyy-MM-dd hh:mm'));

                    $("#T_content").val(obj.Subject);
                }
            });
        }

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&calendarid=" + getparastr("id");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
    </script>

</head>
<body style="margin: 0">
    <form id="form1" onsubmit="return false">
        <div>
            <table style="width: 520px; margin: 5px;" class='aztable'>
                <tr>
                    <td class="table_title1">日程信息</td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 450px">
                            <tr>
                                <td>
                                    <div align="right" style="width: 61px">
                                        全天日程：
                                    </div>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="radio" value="true" name="allday"/></td>
                                            <td>是 </td>
                                            <td>
                                                <input type="radio" value="false" name="allday" checked="checked" /></td>
                                            <td>否 </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <div align="right" style="width: 61px">
                                        开始时间：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_starttime" name="T_starttime" ltype="date" ligerui="{showTime: true,width:150}" validate="{required:true}" />
                                </td>
                                <td>
                                    <div align="right" style="width: 61px">
                                        结束时间：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_endtime" name="T_endtime" ltype="date" validate="{required:true}" ligerui="{showTime: true,width:150}" />

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div align="right" style="width: 61px">
                                        日程内容：
                                    </div>
                                </td>
                                <td colspan="3">

                                    <textarea cols="100" id="T_content" name="T_content" rows="6" class="l-textarea" style="width: 376px" validate="{required:true}"></textarea></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>






        </div>
    </form>

</body>
</html>
