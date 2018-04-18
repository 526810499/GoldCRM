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
        var ExtenData;
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '序号', width: 50, render: function (item, i) { return item.n; } },
                    {
                        display: '销售订单', name: 'Serialnumber', width: 250, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('order','" + item.id + "')>" + item.Serialnumber + "</a>";
                            return html;
                        }
                    },
                    {
                        display: '客户', name: 'Customer_id', width: 260, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('customer','" + item.Customer_id + "')>";
                            if (item.cus_name)
                                html += item.cus_name;
                            html += "</a>";
                            return html;
                        }
                    },
                     { display: '商品名称', name: 'product_name', align: 'left', width: 150 },
                    { display: '商品类别', name: 'category_name', align: 'left', width: 150 },
                    { display: '条形码', name: 'BarCode', align: 'left', width: 180, totalSummary: { type: 'total', render: function (item) { return "<span id='tspan'>合计:</span>"; } } },
                    {
                        display: '重量(克)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='Weight'>" + item.sum + "</span>"; } }
                    },
                    {
                        display: '工费小计(￥)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.CostsTotal);
                        }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='CostsTotal'>" + item.sum + "</span>"; } }
                    },
                    {
                        display: '销售总价', name: 'amount', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.amount);
                        }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='amount'>" + item.sum + "</span>"; } }
                    },
                    { display: '成交部门', name: 'F_dep_id', width: 80, render: function (item, i) { return item.dep_name; } },
                    { display: '成交人员', name: 'emp_id', width: 80, render: function (item, i) { return item.emp_name; } },
                    {
                        display: '成交时间', name: 'Order_date', width: 150, render: function (item) {
                            return formatTime(item.create_time);
                        }
                    }
                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [10, 20, 30, 40, 50, 60, 80, 100, 120],
                url: "Sale_order.gridData.xhd?datacount=1&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -10,
                onSuccess: function (data, grid) {
                    if (data != null && data.Exten != null) {
                        ExtenData = data.Exten[0];
                    }
                },
                onAfterShowData: function (grid) {
                    for (var n in ExtenData) {
                        $("#" + n).text(toMoney(ExtenData[n]));
                    }
                },
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
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
            $.get("toolbar.GetSys.xhd?mid=saleorderdata&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');

                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }

                items.push({
                    id: "sbtn",
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
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                $("div[toolbarid='sbtn']").click().hide();
                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }

        function initSerchForm() {
            $("#T_OrderID").ligerTextBox({ width: 200 });
            $("#T_cus").ligerTextBox({ width: 200 });
            var d = $('#T_status').ligerComboBox({ width: 120, url: "Sys_Param.combo.xhd?type=order_status&rnd=" + Math.random() });
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
            var sendtxt = "&datacount=1&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Sale_order.gridData.xhd?" + serchtxt);
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



        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('sale/order_add.aspx?id=' + row.id, "查看订单", 800, 700);
            }
            else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function exports() {
            var sendtxt = "&etype=1&datacount=1&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;

            var url = ("../Product/ExportProduct.aspx?" + serchtxt);
            location.href = url;
        }

        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

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
            <table style='width: 720px'>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>客户名称：</div>
                    </td>
                    <td>
                        <input type='text' id='T_cus' name='T_cus' ltype='text' ligerui='{width:120}' /></td>


                    <td>
                        <div style='width: 120px; text-align: right; float: right'>成交时间：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' value="<%=(DateTime.Now.AddDays(-1).Date).ToString("yyyy-MM-dd") %>" ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' value="<%=(DateTime.Now.AddDays(1).Date).ToString("yyyy-MM-dd") %>" ligerui='{width:96}' />
                        </div>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr style="height: 10px">
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>订单号：</div>
                    </td>
                    <td>
                        <input id='T_OrderID' name="T_OrderID" type='text' /></td>
                    <td>
                        <div style='width: 120px; text-align: right; float: right'>成交(部门/人员)：</div>
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
