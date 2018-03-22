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

            loadForm();
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "";
                sendtxt += "&followid=" + getparastr("followid");
                sendtxt += "&taskid=" + getparastr("taskid");
                sendtxt += "&statuid=" + getparastr("statu");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm() {
            $("#form1").ligerAutoForm({
                labelWidth: 80, inputWidth: 180, labelWidth: 80, space: 20,
                fields: [
                    {
                        display: '任务详情', type: 'group', icon: '',
                        rows: [
                            [
                                { display: "任务名称", name: "T_task_name", type: "text", options: "{width:465,disabled:true}",  width: 465 }
                            ],
                             [
                                { display: "任务类别", name: "T_task_type", type: "text", options: "{width:180,disabled:true}" },
                                { display: "相关客户", name: "T_customer", type: "text", options: "{width:180,disabled:true}" }
                             ],
                            [
                                { display: "执行人", name: "T_executive", type: "text", options: "{width:180,disabled:true}" },
                                { display: "执行时间", name: "T_executive_time", type: "text", options: "{width:180,disabled:true}" }
                            ],
                            [
                                { display: "任务状态", name: "T_status", type: "select", options: "{width:180,readonly:true,data:[{id:0,text:'进行中'},{id:1,text:'已完成'},{id:2,text:'已终止'}],initValue:0 }" },
                                { display: "优先级", name: "T_priority", type: "select", options: "{width:180,readonly:true,data:[{id:0,text:'高'},{id:1,text:'中'},{id:2,text:'低'}],initValue:1 }" }
                            ],
                            [
                                { display: "任务说明", name: "T_content", type: "textarea", cols: 74, rows: 6, width: 470, cssClass: "l-textarea" }
                            ]
                        ]
                    },
                    {
                        display: '任务跟进', type: 'group', icon: '',
                        rows: [
                            [
                                { display: "任务跟进", name: "T_follow", type: "textarea", cols: 74, rows: 6, width: 470, cssClass: "l-textarea", validate: "{required:true}" }
                            ]
                        ]
                    }
                ]
            }); 
            $.ajax({
                type: "get",
                url: "Task.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: getparastr("taskid"), rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    $("#T_task_name").val(obj.task_title);
                    $("#T_task_type").val(obj.task_type);
                    $("#T_executive_time").val(formatTimebytype(obj.executive_time, "yyyy-MM-dd"));                   
                    $("#T_content").val(obj.task_content).attr("disabled", "disabled");

                    $('#T_customer').val(obj.customer);
                    $('#T_executive').val(obj.executive);

                    $("#T_priority").ligerGetComboBoxManager().selectValue(obj.Priority_id)
                    $("#T_status").ligerGetComboBoxManager().selectValue(obj.task_status_id);
                }
            });
            $.ajax({
                type: "get",
                url: "Task_follow.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: getparastr("followid"), rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    $("#T_follow").val(obj.follow_content);
                }
            });
        }

       

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
    </form>
</body>
</html>
