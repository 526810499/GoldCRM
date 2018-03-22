<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../lib/ligerUI/skins/touch/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });


            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    {
                        display: '客户', name: 'Customer_name', width: 140, align: 'left', render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(1,'" + item.Customer_id + "')>";
                            if (item.Customer_name)
                                html += item.Customer_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    {
                        display: '发票号码', name: 'invoice_num', width: 140, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(7,'" + item.order_id + "','" + item.id + "')>" + item.invoice_num + "</a>";
                            return html;
                        }
                    },
                    { display: '发票类型', name: 'invoice_type', width: 100 },
                    {
                        display: '发票金额(￥)', name: 'invoice_amount', width: 120, align: 'right', render: function (item) {
                            return toMoney(item.invoice_amount);
                        }
                    },
                    { display: '开票部门', name: 'C_depname', width: 90 },
                    { display: '开票人', name: 'C_empname', width: 90 },
                    {
                        display: '开票日期', name: 'invoice_date', width: 90, render: function (item) {
                            return formatTimebytype(item.invoice_date, 'yyyy-MM-dd');
                        }
                    },
                    { display: '录入人', name: 'create_name', width: 90 },
                {
                    display: '查看', width: 60, render: function (item) {
                        var html = "<a href='javascript:void(0)' onclick=view(7,'" + item.order_id + "','" + item.id + "')>查看</a>";
                        return html;
                    }
                }

                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "CRM_invoice.grid.xhd?rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -10,

                onRClickToSelect: true
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());


            $('form').ligerForm();
            initSerchForm();
            toolbar();
            //toolbar1();

        });

        function toolbar() {
            $("#toolbar").ligerToolBar({
                items: [
                    //{ type: 'button', text: '查看发票', icon: '../images/folder-open.gif', disable: true, click: function () { view(); } },
                    { type: 'serchbtn', text: '高级搜索', icon: '../images/search.gif', disable: true, click: function () { serchpanel() } }
                ]
            });
            menu = $.ligerMenu({
                width: 120, items:
                [
                    { text: '查看', click: view, icon: 'view' },
                ]
            });

            $("#maingrid4").ligerGetGridManager().onResize();

        }
        function initSerchForm() {
            var d = $('#pay_type').ligerComboBox({ width: 196, url: "C_Sys)Param.combo.xhd?type=pay_type&rnd=" + Math.random() });
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
                    parentIDFieldName: 'pid',
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
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.GetDataByURL("CRM_invoice.grid.xhd?" + serchtxt);
        }
        function doclear() {
            $("input:hidden", "#serchform").val("");
            $("input:text", "#serchform").val("");
            $(".l-selected").removeClass("l-selected");
        }

        function f_reload() {
            $("#maingrid4").ligerGetGridManager().loadData(true);
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
            <table style='width: 920px' class="bodytable1">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>客户名称：</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' />
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>开票人：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='department' name='department' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='employee' name='employee' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>开票日期：</div>
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
                        <div style='width: 60px; text-align: right; float: right'>发票号码：</div>
                    </td>
                    <td>
                        <input type='text' id='receive_num' name='receive_num' ltype='text' ligerui='{width:120}' />
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>发票类型：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='pay_type' name='pay_type' />
                        </div>
                    </td>
                    <td></td>
                    <td>
                        <input id='Button2' type='button' value='重置' style='width: 80px; height: 24px'
                            onclick="doclear()" />
                        <input id='Button1' type='button' value='搜索' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                </tr>

            </table>
        </form>
    </div>
</body>
</html>
