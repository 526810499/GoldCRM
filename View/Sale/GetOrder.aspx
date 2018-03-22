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
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    { display: '订单编号', name: 'Serialnumber', width: 180 },
                    { display: '客户', name: 'cus_name', width: 200, align: 'left' },
                    { display: '成交部门', name: 'F_dep_id', width: 80, render: function (item, i) { return item.dep_name; } },
                    { display: '成交人员', name: 'emp_id', width: 80, render: function (item, i) { return item.emp_name; } },
                    {
                        display: '订单状态', name: 'Order_status_id', width: 70, render: function (item, i) {
                            return item.Order_status;
                        },
                        totalSummary: { type: 'total' }
                    },
                    {
                        display: '订单金额（￥）', name: 'total_amount', width: 100, align: 'right', render: function (item) {
                            return "<div style='color:#135294'>" + toMoney(item.total_amount) + "</div>";
                        }, totalSummary: { type: 'sum_money' }
                    },
                   {
                       display: '已收总额（￥）', name: 'receive_money', width: 100, align: 'right', render: function (item) {
                           return "<div style='color:#135294'>" + toMoney(item.receive_money) + "</div>";
                       }, totalSummary: { type: 'sum_money' }
                   },
                   {
                       display: '未收余额（￥）', name: 'arrears_money', width: 100, align: 'right', render: function (item) {
                           return "<div style='color:#135294'>" + toMoney(item.arrears_money) + "</div>";
                       }, totalSummary: { type: 'sum_money' }
                   },
                   
                    {
                        display: '成交时间', name: 'Order_date', width: 90, render: function (item) {
                            return formatTimebytype(item.Order_date, 'yyyy-MM-dd');
                        }
                    }

                ],
                //groupColumnName: 'Customer_name', groupColumnDisplay: '客户',
                //groupRender: function (column, display, data) {
                //    return display + ":" + column + "  （共" + data.length + "条记录。）";
                //},
                //defaultCloseGroup: true,
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "Sale_order.grid.xhd?customerid=" + getparastr("customer_id"),
                width: '100%', height: '100%',
                heightDiff: -10,

                detail: {
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        var grid = document.createElement('div');
                        $(p).append(grid);
                        $(grid).css('margin', 3).ligerGrid({
                            columns: [
                                    //{ display: '序号', width: 30, render: function (item, i) { return i + 1; } },
                                    { display: '产品名', name: 'product_name', width: 120 },
                                    {
                                        display: '单价', name: 'price', width: 80, type: 'float', align: 'right', render: function (item) {
                                            return toMoney(item.price);
                                        }
                                    },
                                    { display: '数量', name: 'quantity', width: 40, type: 'int' },
                                    { display: '单位', name: 'unit', width: 40 },
                                    {
                                        display: '总价', name: 'amount', width: 100, type: 'float', align: 'right', render: function (item) {
                                            return toMoney(item.amount) + "元";
                                        }
                                    }

                            ],
                            //selectRowButtonOnly: true,
                            usePager: false,
                            checkbox: true,
                            url: "Sale_order_details.grid.xhd?orderid=" + r.id,
                            width: '99%', height: '100',
                            heightDiff: 0
                        })

                    }
                },
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
            $('#serchform').ligerForm();
            toolbar();

            $("#btn_serch").ligerButton({ text: "搜索", width: 60, click: doserch });
            $("#btn_reset").ligerButton({ text: "重置", width: 60, click: doclear });
        });

        function toolbar() {
           
                var items = [];
               
                items.push({
                    type: 'serchbtn',
                    text: '高级搜索',
                    icon: '../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });
                $("#toolbar").ligerToolBar({
                    items: items
                });
                
                $("#maingrid4").ligerGetGridManager()._onResize();
           
        }
        
        function initSerchForm() {
            var d = $('#contact').ligerComboBox({ width: 120, url: "Param_SysParam.combo.xhd?parentid=6&rnd=" + Math.random() });
            var e = $('#employee').ligerComboBox({ width: 96 });
            var f = $('#department').ligerComboBox({
                width: 97,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: 'hr_department.tree.xhd?rnd=' + Math.random(),
                    idFieldName: 'id',
                    //parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue) {
                    $.get("hr_employee.combo.xhd?did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        e.setData(eval(data));
                    });
                }
            });
        }
        function serchpanel() {
            initSerchForm();
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
        }
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("CRM_order.grid.xhd?" + serchtxt);
        }
        function doclear() {
            $("input:hidden", "#serchform").val("");
            $("input:text", "#serchform").val("");
            $(".l-selected").removeClass("l-selected");
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });


        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            //alert(rows);
            return rows;
        }
    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div style="padding: 10px;">
            <div id="toolbar"></div>

            <div id="grid">
                <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            </div>
        </div>

    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 720px' class="aztable" >
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>客户名称：</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' /></td>


                    <td>
                        <div style='width: 60px; text-align: right; float: right'>成交时间：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>订单状态：</div>
                    </td>
                    <td>
                        <input id='contact' name="contact" type='text' /></td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>成交人员：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='department' name='department' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='employee' name='employee' />
                        </div>
                    </td>
                    <td></td>
                    <td>
                         <div id="btn_serch"></div>
                        <div id="btn_reset"></div>
                    </td>
                </tr>

            </table>
        </form>
    </div>
</body>
</html>
