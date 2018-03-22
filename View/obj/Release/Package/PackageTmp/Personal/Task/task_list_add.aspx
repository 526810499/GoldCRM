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
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    

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
                                display: '任务指派', type: 'group', icon: '',
                                rows: [
                                    [
                                        { display: "任务名称", name: "T_task_name", type: "text", options: "{width:465}", validate: "{required:true}", initValue: obj.task_title, width: 465 }
                                    ],
                                     [
                                        { display: "任务类别", name: "T_task_type", type: "select", options: "{width:180, url: 'Sys_Param.combo.xhd?type=task_type',initValue:'"+obj.task_type_id+"'}", validate: "{required:true}" },
                                        { display: "相关客户", name: "T_customer" }
                                     ],
                                    [
                                        { display: "执行人", name: "T_executive",  validate: "{required:true}" },
                                        { display: "执行时间", name: "T_executive_time", type: "date", options: "{width:180}", validate: "{required:true}", initValue: formatTimebytype( obj.executive_time,"yyyy-MM-dd") }
                                    ],
                                    [
                                        { display: "任务状态", name: "T_status", type: "select", options: "{width:180,data:[{id:0,text:'进行中'},{id:1,text:'已完成'},{id:2,text:'已终止'}],initValue:0 }", validate: "{required:true}", initValue: obj.task_status_id },
                                        { display: "优先级", name: "T_priority", type: "select", options: "{width:180,data:[{id:0,text:'高'},{id:1,text:'中'},{id:2,text:'低'}],initValue:1 }" }
                                    ],
                                    [
                                        { display: "任务说明", name: "T_content", type: "textarea", cols: 74, rows: 8, width: 470, cssClass: "l-textarea", validate: "{required:true}", initValue: obj.task_content }
                                    ]
                                ]
                            }
                        ]
                    });
                    $('#T_customer').ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectCustomer
                    });
                    $('#T_executive').ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectContact
                    });

                    $('#T_customer').val(obj.customer);
                    $('#T_customer_val').val(obj.customer_id);

                    $('#T_executive').val(obj.executive);
                    $('#T_executive_val').val(obj.executive_id);

                    $("#T_priority").ligerGetComboBoxManager().selectValue(obj.priority_id)
                    $("#T_status").ligerGetComboBoxManager().selectValue(obj.task_status_id);
                   
                }
            });
        }

        function f_selectCustomer() {
            $.ligerDialog.open({
                title: '选择联系人', width: 650, height: 300, url: '../../crm/customer/getcustomer.aspx', buttons: [
                    { text: '确定', onclick: f_selectCustomerOK },
                    { text: '取消', onclick: function (item, dialog) { dialog.close();} }
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
            $("#T_customer").val(data.cus_name);
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
        function f_selectContactOK(item,dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }
            $("#T_executive").val(data.name);
            $("#T_executive_val").val(data.id);
            dialog.close();
        }


    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
       
    </form>
</body>
</html>
