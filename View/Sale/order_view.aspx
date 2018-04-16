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
    <script src="../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js?v=1.0" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            loadForm(getparastr("id"));

        });

        function f_save() {

            var manager = $("#maingrid4").ligerGetGridManager();
            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");
                sendtxt += "&PostData=" + JSON.stringify(manager.getChanges());
                sendtxt += "&customer_id=" + getparastr("customer_id");
                return $("form :input").fieldSerialize() + sendtxt;
            }

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
                    if (!customer_id) {
                        rows.push([{ display: "客户", name: "T_customer", validate: "{required:true}", width: 465 }]);
                    }

                    rows.push(
                              [
                                { display: "会员卡号", name: "T_vipcard", type: "text", initValue: obj.vipcard },
                                { display: "销售门店", name: "T_dept_id", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'hr_department.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.createdep_id + "'}", validate: "{required:true}" }
                              ],
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
                                { display: "已收金额", name: "T_receive", type: "text", options: "{width:180,onChangeValue:function(){ arrearsAmount(); }}", validate: "{required:true}", initValue: toMoney(obj.receive_money) },
                                { display: "待收金额", name: "T_arrears", type: "text", options: "{width:180,disabled:true}", validate: "{required:true}", initValue: toMoney(obj.arrears_money) }
                            ],
                            [
                                { display: "成交人员", name: "T_emp", validate: "{required:true}", initValue: obj.emp_name },
                                { display: "收银员", name: "T_cashier", validate: "{required:true}", initValue: obj.cashiername }
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
                    });


                    $("#T_customer").val(obj.Customer_name);
                    $("#T_customer_val").val(obj.customer_id);


                    $("#T_emp").ligerComboBox({
                        width: 180,
                    });

                    $("#T_cashier").ligerComboBox({
                        width: 180,
                    })


                    $("#T_emp").val(obj.emp_name);
                    $("#T_emp_val").val(obj.emp_id);

                    $("#T_cashier").val(obj.cashiername);
                    $("#T_cashier_val").val(obj.cashier_id);

                }
            });
        }


        function f_grid() {
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '商品名称', name: 'product_name', align: 'left', width: 150 },
                    { display: '商品类别', name: 'category_name', align: 'left', width: 150 },
                    { display: '条形码', name: 'BarCode', align: 'left', width: 180 },
                    {
                        display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }
                    },
                    {
                        display: '销售工费(￥)', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesCostsTotal);
                        }
                    },
                    {
                        display: '销售价格(￥)', name: 'SalesTotalPrice', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.SalesTotalPrice);
                        }
                    }
                ],
                allowHideColumn: false,
                //onAfterEdit: f_onAfterEdit,
                title: '商品明细',
                usePager: false,
                //enabledEdit: true,
                url: "Sale_order_details.grid.xhd?orderid=" + getparastr("id"),
                width: '100%',
                height: 350,
                heightDiff: -1,
            });

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
