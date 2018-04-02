<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>

    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            loadForm(getparastr("id"));

        });

        function f_save() {

            f_check();

            if (f_postnum() == 0) {
                $.ligerDialog.warn("请添加产品！");
                return;
            }
            var msg = checkPrice();
            if (msg.length > 0) {
                if (!confirm(msg)) { return false; }
            }

            var manager = $("#maingrid4").ligerGetGridManager();
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");
                sendtxt += "&PostData=" + JSON.stringify(manager.getChanges());
                sendtxt += "&customer_id=" + getparastr("customer_id");
                return $("form :input").fieldSerialize() + sendtxt;
            }

        }

        function checkPrice() {
            var T_total = parseFloat($("#T_total").val());
            var T_receive = parseFloat($("#T_receive").val());
            var T_arrears = T_total - T_receive;
            $("#T_arrears").val(toMoney(T_arrears));
            var msg = "";

            if (T_receive <= 0) {
                msg = ("已收金额为0确认要提交订单?");
            }
            else if (T_arrears != 0) {
                msg = ("还有订单金额未支付确认要提交订单?");
            }

            return msg;
        }




        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "Sale_order.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    var rows = [];

                    var customer_id = obj.Customer_id || getparastr("customer_id");
                    if (!customer_id)
                        rows.push([{ display: "客户", name: "T_customer", validate: "{required:true}", width: 465 }])

                    rows.push(
                            [
                                { display: "成交时间", name: "T_date", type: "date", options: "{width:180}", validate: "{required:true}", initValue: formatTimebytype(obj.Order_date, "yyyy-MM-dd") },
                                { display: "订单金额", name: "T_amount", type: "text", options: "{width:180,disabled:true,onChangeValue:function(){ getAmount(); }}", validate: "{required:true}", initValue: toMoney(obj.Order_amount) }
                            ],
                            [
                                { display: "订单状态", name: "T_status", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=order_status',value:'" + obj.Order_status_id + "'}", validate: "{required:true}" },
                                { display: "优惠金额", name: "T_discount", type: "text", options: "{width:180,onChangeValue:function(){ getAmount(); }}", validate: "{required:true}", initValue: toMoney(obj.discount_amount) }
                            ],
                            [
                                { display: "支付方式", name: "T_paytype", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=pay_type',value:'" + obj.pay_type_id + "'}", validate: "{required:true}", initValue: formatTimebytype(obj.import_time, "yyyy-MM-dd") },
                                { display: "应收金额", name: "T_total", type: "text", options: "{width:180,disabled:true}", validate: "{required:true}", initValue: toMoney(obj.total_amount) }
                            ],
                            [
                                { display: "已收金额", name: "T_receive", type: "text", options: "{width:180,onChangeValue:function(){ arrearsAmount(); }}", validate: "{required:true}", initValue: toMoney2(obj.receive_money) },
                                { display: "待收金额", name: "T_arrears", type: "text", options: "{width:180,disabled:true}", validate: "{required:true}", initValue: toMoney(obj.arrears_money) }
                            ],
                            [
                                { display: "成交人员", name: "T_emp", validate: "{required:true}" },
                                { display: "收银员", name: "T_cashier", validate: "{required:true}" }
                            ],
                            [
                                { display: "备注", name: "T_details", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.Order_details }
                            ]
                        );

                    if (!obj.discount_amount)
                        obj.discount_amount = 0;

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '订单', type: 'group', icon: '',
                                rows: rows

                            }
                        ]
                    });
                    f_grid();

                    $("#T_customer").ligerComboBox({
                        width: 465,
                        onBeforeOpen: f_selectCustomer
                    });


                    $("#T_customer").val(obj.Customer_name);
                    $("#T_customer_val").val(obj.customer_id);


                    $("#T_emp").ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectEmp
                    });

                    $("#T_cashier").ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectCash
                    })

                    $("#T_emp").val(obj.emp_name);
                    $("#T_emp_val").val(obj.emp_id);

                    $("#T_cashier").val(obj.emp_name);
                    $("#T_cashier_val").val(obj.emp_id);
                }
            });
        }
        function f_selectCustomer() {
            $.ligerDialog.open({
                zindex: 9005, title: '选择客户', width: 650, height: 300, url: '../crm/customer/getCustomer.aspx', buttons: [
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

            $("#T_customer").val(data.cus_name);
            $("#T_customer_val").val(data.id);
            dialog.close();
        }

        function f_selectEmp() {
            $.ligerDialog.open({
                zindex: 9005, title: '选择员工', width: 650, height: 300, url: '../hr/getemp_auth.aspx?auth=3', buttons: [
                     { text: '确定', onclick: f_selectEmpOK },
                     { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectCash() {
            $.ligerDialog.open({
                zindex: 9005, title: '选择员工', width: 650, height: 300, url: '../hr/getemp_auth.aspx?auth=3', buttons: [
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

            $("#T_cashier").val(data.name);
            $("#T_cashier_val").val(data.id);

            dialog.close();
        }

        function f_selectEmpOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }

            $("#T_emp").val(data.name);
            $("#T_emp_val").val(data.id);

            if ($("#T_cashier").val().length <= 0) {
                $("#T_cashier").val(data.name);
                $("#T_cashier_val").val(data.id);
            }
            dialog.close();
        }

        function getAmount() {
            var T_amount = $("#T_amount").val();
            var T_discount = $("#T_discount").val();
            var T_total = $("#T_total").val();

            $("#T_discount").val(toMoney(T_discount));
            $("#T_total").val(toMoney(parseFloat(T_amount.replace(/\$|\,/g, '')) - parseFloat(T_discount.replace(/\$|\,/g, ''))));
        }



        function f_grid() {
            $("#maingrid4").ligerGrid({
                columns: [

                    {
                        display: '产品', name: 'product_name', width: 120
                    },
                    {
                        display: '单位', name: 'unit', width: 40
                    },
                    {
                        display: '规格', name: 'specifications', width: 100
                    },
                    {
                        display: '销售价', name: 'agio', width: 80, type: 'float', align: 'right', render: function (item) {
                            return toMoney(item.agio);
                        }
                        , editor: { type: 'float' }
                    },
                    {
                        display: '数量', name: 'quantity', width: 60, type: 'int',
                        editor: { type: 'int', isNegative: false }
                    },
                    {
                        display: '总价', name: 'amount', width: 100, type: 'float', align: 'right', render: function (item) {
                            return toMoney(item.amount);
                        }
                    }
                ],
                allowHideColumn: false,
                onAfterEdit: f_onAfterEdit,
                title: '产品明细',
                usePager: false,
                enabledEdit: true,
                url: "Sale_order_details.grid.xhd?orderid=" + getparastr("id"),
                width: '100%',
                height: 170,
                heightDiff: -1,
                onLoaded: f_loaded
            });

        }

        function f_loaded() {
            if ($("#btn_add").length > 0)
                return;

            $(".l-panel-header").append("<div style='width:150px;float:right'><div id = 'btn_add' style='margin-top:2px;'></div><div id = 'btn_del' style='margin-top:2px;'></div></div>");
            $(".l-grid-loading").fadeOut();
            $("#btn_add").ligerButton({
                width: 60,
                text: "添加",
                icon: '../../images/icon/11.png',
                click: add
            })

            $("#btn_del").ligerButton({
                width: 60,
                text: "删除",
                icon: '../../images/icon/12.png',
                click: pro_remove
            })
            $("#maingrid4").ligerGetGridManager()._onResize();
        }

        function f_onAfterEdit(e) {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.updateCell('amount', e.record.agio * e.record.quantity, e.record);
            $("#T_amount").val(toMoney(manager.getColumnDateByType('amount', 'sum') * 1.0));
            getAmount();
        }

        function add() {
            f_openWindow("product/GetProduct.aspx", "选择产品", 1000, 600, f_getpost, 9003);
        }
        function pro_remove() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.deleteSelectedRow();
            setTimeout(function () {
                $("#T_amount").val(toMoney(manager.getColumnDateByType('amount', 'sum') * 1.0));
                getAmount();
            }, 50)

        }
        function f_getpost(item, dialog) {
            var rows = null;
            if (!dialog.frame.f_select()) {
                alert('请选择产品!');
                return;
            }
            else {
                rows = dialog.frame.f_select();

                //过滤重复
                var manager = $("#maingrid4").ligerGetGridManager();
                var data = manager.getData();

                for (var i = 0; i < rows.length; i++) {
                    rows[i].product_id = rows[i].id;
                    var add = 1;
                    for (var j = 0; j < data.length; j++) {
                        if (rows[i].product_id == data[j].product_id) {
                            add = 0;
                        }
                    }
                    if (add == 1) {
                        //price
                        rows[i].quantity = 1;
                        rows[i]["amount"] = rows[i].agio * rows[i].quantity;
                        manager.addRow(rows[i]);
                    }
                }
                dialog.close();
            }
            $("#T_amount").val(toMoney(manager.getColumnDateByType('amount', 'sum') * 1.0));
            getAmount();
        }

        function f_postnum() {
            var manager = $("#maingrid4").ligerGetGridManager();
            return manager.getColumnDateByType('amount', 'count') * 1.0;
        }

        function f_check() {
            var g = $("#maingrid4").ligerGetGridManager().endEdit(true);
        }
        function remote() {
            var url = "PB_BicycleType.Exist.xhd?id=" + getparastr("id") + "&rnd=" + Math.random();
            return url;
        }

        function toMoney2(values) {
            if (parseFloat(values) > 0) {
                return toMoney(values);
            }
            return "";
        }

        function arrearsAmount() {
            var T_total = parseFloat($("#T_total").val());
            var T_receive = parseFloat($("#T_receive").val());
            var T_arrears = T_total - T_receive;
            $("#T_arrears").val(toMoney(T_arrears));
        }

    </script>

</head>
<body style="overflow: hidden;">
    <form id="form1" onsubmit="return false">
    </form>
    <div style="padding: 5px 4px 5px 2px;">

        <div id="maingrid4">
        </div>
    </div>
</body>
</html>
