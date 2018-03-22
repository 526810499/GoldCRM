<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../lib/json2.js" type="text/javascript"></script>

    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();

            loadForm(getparastr("id"));
        });

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "Finance_Invoice.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数                    

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '发票', type: 'group', icon: '',
                                rows: [
                                    [
                                        { display: "订单编号", name: "T_order", validate: "{required:true}" },
                                        { display: "关联客户", name: "T_customer", type: "text", options: "{width:180,disabled:true}", initValue: obj.cus_name }
                                    ],
                                    [
                                        { display: "订单金额", name: "T_order_amount", type: "text", options: "{width:180,disabled:true}", validate: "{required:true}", initValue: toMoney(obj.Order_amount) },
                                        { display: "开票时间", name: "T_date", type: "date", options: "{width:180}", validate: "{required:true}", initValue: formatTimebytype(obj.invoice_date, "yyyy-MM-dd") }
                                    ],
                                    [
                                        { display: "发票金额", name: "T_amount", type: "text", options: "{width:180,onChangeValue:function(){ getAmount(); }}", validate: "{required:true}", initValue: toMoney(obj.invoice_amount) },
                                        { display: "发票类型", name: "T_type", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=invoice_type',value:'" + obj.invoice_type_id + "'}", validate: "{required:true}" }
                                    ],
                                    [
                                        { display: "发票号码", name: "T_no", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.invoice_num },
                                        { display: "开票人", name: "T_emp", type: "select", options: "{width:180}", validate: "{required:true}" }
                                    ],
                                    [
                                        { display: "开票内容", name: "T_Remark", type: "textarea", cols: 74, rows: 4, width: 465, cssClass: "l-textarea", validate: "{required:true}", initValue: obj.invoice_content }
                                    ]
                                ]
                            }
                        ]
                    });
                    $('#T_order').ligerComboBox({ width: 180, onBeforeOpen: f_selectorder });

                    $("#T_order").val(obj.order_no);
                    $("#T_order_val").val(obj.order_id);

                    $('#T_emp').ligerComboBox({ width: 180, onBeforeOpen: f_selectContact });
                    if (obj.empid) {
                        $("#T_emp_val").val(obj.empid);
                        $("#T_emp").val(obj.name);
                    }
                    else {
                        $.getJSON("hr_employee.form.xhd?id=epu&rnd=" + Math.random(), function (result) {
                            var obj = eval(result);
                            for (var n in obj) {
                                if (obj[n] == null)
                                    obj[n] = "";
                            }
                            $("#T_emp_val").val(obj.id);
                            $("#T_emp").val(obj.name);
                        })
                    }
                }
            });
        }

        function set_tomoney(value) {
            $("#T_invoice_amount").val(toMoney(value));
        }

        function getAmount() {
            $("#T_amount").val(toMoney($("#T_amount").val()));
        }

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
        function f_selectorder() {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '选择订单', width: 850, height: 400, url: "sale/getorder.aspx", buttons: [
                    { text: '确定', onclick: f_selectOrderOK },
                    { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }
        function f_selectOrderOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择订单!');
                return;
            }

            $("#T_order").val(data.Serialnumber);
            $("#T_order_val").val(data.id);
            $("#T_customer").val(data.cus_name);
            $("#T_order_amount").val(toMoney(data.Order_amount));
            dialog.close();
        }

        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }

        function f_selectContact() {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '选择员工', width: 850, height: 400, url: "hr/Getemp_Auth.aspx?auth=1", buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择员工!');
                return;
            }

            $("#T_emp").val("【" + data.dep_name + "】" + data.name);
            $("#T_emp_val").val(data.id);

            dialog.close();
        }
       
    </script>

</head>
<body style="margin: 0">
    <form id="form1" onsubmit="return false">
    </form>

</body>
</html>
