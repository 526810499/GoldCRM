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
                var sendtxt = "&id=" + getparastr("id");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "BusinessToRemindUserConfig.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);

                    var rows = [];

                    rows.push(
                                 [
                                        { display: "提醒员工", name: "T_userid", type: "select", options: "{width:180}", validate: "{required:true}", initValue: obj.userName },
                                 ],

                                     [
                                        { display: "提醒业务", name: "T_remindType", type: "select", options: "{width:180,data:[ {id:'1',text:'客户生日提醒'}],selectBoxHeight:50, value: " + obj.remindType + "}", validate: "{required:true}" }
                                     ],
                                    [
                                        { display: "备注", name: "T_Remark", type: "textarea", cols: 74, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                                    ]
                        );
                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '配置', type: 'group', icon: '',
                                rows: rows

                            }
                        ]
                    });

                    $("#T_userid").ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectEmp
                    });

                    $("#T_userid").val(obj.userName);
                    $("#T_userid_val").val(obj.userid);
                }

            });
        }


        function f_selectEmp() {
            $.ligerDialog.open({
                zindex: 9005, title: '选择员工', width: 650, height: 300, url: '../../hr/getemp_auth.aspx?auth=3&config=1', buttons: [
                     { text: '确定', onclick: f_selectCashOK },
                     { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectCashOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }

            $("#T_userid").val(data.name);
            $("#T_userid_val").val(data.id);

            dialog.close();
        }

    </script>

</head>
<body>
    <form id="form1" onsubmit="return false">
    </form>
</body>
</html>
