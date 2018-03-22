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

            $('#T_employee1').ligerComboBox({ width: 196, onBeforeOpen: f_selectContact1 });
            $('#T_employee2').ligerComboBox({ width: 196, onBeforeOpen: f_selectContact2 });
            $('#T_customertype').ligerComboBox({ width: 97, url: "Sys_Param.combo.xhd?type=cus_type&rnd=" + Math.random(), emptyText: '（空）' });
            $('#T_customerlevel').ligerComboBox({ width: 96, url: "Sys_Param.combo.xhd?type=cus_level&rnd=" + Math.random(), emptyText: '（空）' });
            $('#T_CustomerSource').ligerComboBox({ width: 196, url: "Sys_Param.combo.xhd?type=cus_source&rnd=" + Math.random(), emptyText: '（空）' });
        });
        function f_selectContact1() {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '选择员工', width: 850, height: 400, url: "../../hr/Getemp_Auth.aspx?auth=0", buttons: [
                    { text: '确定', onclick: function (item, dialog) { f_selectContactOK(item, dialog, 1) } },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContact2() {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '选择员工', width: 850, height: 400, url: "../../hr/Getemp_Auth.aspx?auth=0", buttons: [
                    { text: '确定', onclick: function (item, dialog) { f_selectContactOK(item, dialog, 2) } },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog, type) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择员工!');
                return;
            }
            switch (type) {
                case 1:
                    $("#T_employee1").val("【" + data.dep_name + "】" + data.name);
                    $("#T_employee1_val").val(data.id);
                    break;
                case 2:
                    $("#T_employee2").val("【" + data.dep_name + "】" + data.name);
                    $("#T_employee2_val").val(data.id);
                    break;
            }
            dialog.close();
        }
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }
        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save." + getparastr("type");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }


        function f_check() {
            if ($(form1).valid()) {
                var emp1 = $("#T_employee1_val").val();
                var emp2 = $("#T_employee2_val").val();

                if (emp1 == emp2) {
                    $.ligerDialog.error("转出人与转入人不能是同一个人！");
                    return false;
                }

                return true;
            }
        }

        function f_count() {
            if (!$(form1).valid()) return;

            var sendtxt = $("form :input").fieldSerialize() + "&rnd=" + Math.random();

            $.ajax({
                url: "CRM_Customer.c_count.xhd", type: "get",
                data: sendtxt,
                beforeSend: function () {
                    $.ligerDialog.waitting('数据查询中,请稍候...');
                },
                success: function (responseText) {
                    $.ligerDialog.closeWaitting();
                    $("#Label_count").text(responseText);
                },
                error: function () {
                    $.ligerDialog.closeWaitting();
                    $.ligerDialog.error('操作失败！');
                }
            });


        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <table align="left" border="0" cellpadding="3" cellspacing="1" class="bodytable2"
            style="background: #fff; width: 640px; margin: 5px;">

            <tr>
                <td width="65px" colspan="4" class="table_title">人员</td>

            </tr>

            <tr>
                <td width="65px">转出人：</td>
                <td>
                    <input type="text" id="T_employee1" name="T_employee1" validate="{required:true}" />
                </td>
                <td width="65px">转入人：</td>
                <td>
                    <input type="text" id="T_employee2" name="T_employee2" validate="{required:true}" />
                </td>

            </tr>

            <tr>
                <td width="65px" colspan="4" class="table_title">条件</td>

            </tr>

            <tr>
                <td width="65px">创建时间：</td>
                <td>

                    <div style='width: 100px; float: left'>
                        <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                    </div>
                    <div style='width: 98px; float: left'>
                        <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                    </div>
                </td>
                <td width="65px">客户类型：</td>
                <td>
                    <div style="width: 100px; float: left">
                        <input id="T_customertype" name="T_customertype" type="text" style="width: 96px" />
                    </div>
                    <div style="width: 98px; float: left">
                        <input id="T_customerlevel" name="T_customerlevel" type="text" style="width: 96px" />
                    </div>
                </td>

            </tr>

            <tr>
                <td width="65px">最后跟进：</td>
                <td>
                    <div style='width: 100px; float: left'>
                        <input type='text' id='startfollow' name='startfollow' ltype='date' ligerui='{width:97}' />
                    </div>
                    <div style='width: 98px; float: left'>
                        <input type='text' id='endfollow' name='endfollow' ltype='date' ligerui='{width:96}' />
                    </div>
                </td>
                <td width="65px">客户来源：</td>
                <td>
                    <input id="T_CustomerSource" name="T_CustomerSource" type="text" /></td>

            </tr>

            <tr>
                <td width="65px" colspan="4" class="table_title">统计</td>

            </tr>

            <tr>
                <td width="65px">统计数：</td>
                <td>
                    <span id="Label_count"></span>
                </td>
                <td width="65px">&nbsp;</td>
                <td>

                    <input type="button" style="width: 100px; height: 21px;" value="统计" onclick="f_count()" />
                    <input type="button" style="width: 100px; height: 21px;" value="重置" onclick="f_cancel()" />
                </td>

            </tr>

        </table>
    </form>
</body>
</html>
