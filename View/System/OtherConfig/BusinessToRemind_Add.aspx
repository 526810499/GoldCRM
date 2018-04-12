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
                url: "BusinessToRemindUserConfig.form.xhd", /* ע���������ֶ�ӦCS�ķ������� */
                data: { id: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);

                    var rows = [];

                    rows.push(
                                 [
                                        { display: "����Ա��", name: "T_userid", type: "select", options: "{width:180}", validate: "{required:true}", initValue: obj.userName },
                                 ],

                                     [
                                        { display: "����ҵ��", name: "T_remindType", type: "select", options: "{width:180,data:[ {id:'1',text:'�ͻ���������'}],selectBoxHeight:50, value: " + obj.remindType + "}", validate: "{required:true}" }
                                     ],
                                    [
                                        { display: "��ע", name: "T_Remark", type: "textarea", cols: 74, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                                    ]
                        );
                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '����', type: 'group', icon: '',
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
                zindex: 9005, title: 'ѡ��Ա��', width: 650, height: 300, url: '../../hr/getemp_auth.aspx?auth=3&config=1', buttons: [
                     { text: 'ȷ��', onclick: f_selectCashOK },
                     { text: 'ȡ��', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectCashOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('��ѡ����!');
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
