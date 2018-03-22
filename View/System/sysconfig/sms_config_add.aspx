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
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            loadForm(getparastr("id"));
        });

        function f_save() {
            if ($(form1).valid()) {
                return $("form :input").fieldSerialize()+"&rnd="+Math.random();
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "Task.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, labelWidth: 80, space: 20,
                        fields: [
                            {
                                display: '系统注册', type: 'group', icon: '',
                                rows: [
                                    [{ display: "序列号", name: "T_SerialNo", type: "text", options: "{width:180}", validate: "{required:true}" }],
                                    [{ display: "密码", name: "T_key", type: "text", options: "{width:180}", validate: "{required:true}" }]
                                ]
                            }
                        ]
                    });
                }
            });
        }

        function f_selectCustomer() {
            $.ligerDialog.open({
                title: '选择联系人', width: 650, height: 300, url: '../../crm/customer/getcustomer.aspx', buttons: [
                    { text: '确定', onclick: f_selectCustomerOK },
                    { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }
        function f_selectCustomerOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }
            $("#T_customer").val(data.Customer);
            $("#T_customer_val").val(data.id);
            dialog.close();
        }
        function f_selectContact() {
            $.ligerDialog.open({
                title: '选择联系人', width: 650, height: 300, url: '../../hr/getemp_auth.aspx?auth=1', buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }
            $("#T_executive").val(data.name);
            $("#T_executive_val").val(data.ID);
            dialog.close();
        }


    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
    </form>
</body>
</html>
