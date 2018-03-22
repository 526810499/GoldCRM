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
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (item, i) { return i + 1; } },
                    {
                        display: '发票号码', name: 'invoice_num', width: 140, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('invoice','" + item.id + "')>" + item.invoice_num + "</a>";
                            return html;
                        }
                    },
                    { display: '发票类型', name: 'invoice_type', width: 100 },
                    {
                        display: '发票金额(￥)', name: 'invoice_amount', width: 120, align: 'right', render: function (item) {
                            return toMoney(item.invoice_amount);
                        }
                    },
                    //{ display: '开票部门', name: 'C_dep_name', width: 90 },
                    { display: '开票人', name: 'name', width: 90 },
                    {
                        display: '开票日期', name: 'invoice_date', width: 90, render: function (item) {
                            return formatTimebytype(item.invoice_date, 'yyyy-MM-dd');
                        }
                    },
                    {
                        display: '录入时间', name: 'create_time', width: 150, render: function (item) {
                            return formatTime(item.create_time);
                        }
                    },
                    { display: '录入人', name: 'create_name', width: 90 }


                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "Finance_Invoice.grid.xhd?rnd=" + Math.random(),
                width: '100%', height: '100%',
                //title: "发票信息",
                heightDiff: -10,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu1.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            $('#serchform').ligerForm();

            toolbar();
        });

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=finance_invoice&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu1 = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#maingrid4").ligerGetGridManager()._onResize();
            });
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
                $("#maingrid4").ligerGetGridManager().onResize();
                $("#maingrid5").ligerGetGridManager().onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager().onResize();
                $("#maingrid5").ligerGetGridManager().onResize();
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
            manager.GetDataByURL("CRM_order.grid.xhd?" + serchtxt);
        }
        function doclear() {
            $("input:hidden", "#serchform").val("");
            $("input:text", "#serchform").val("");
            $(".l-selected").removeClass("l-selected");
        }


        function add() {

            f_openWindow("finance/invoice_add.aspx", "新开发票", 770, 490, f_save);

        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (!row) {
                $.ligerDialog.warn('请选择发票！');
                return;
            }
            f_openWindow("finance/invoice_add.aspx?id=" + row.id, "修改发票", 770, 490, f_save);

        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("发票删除不能恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Finance_Invoice.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
                            dataType: 'json',
                            success: function (result) {
                                $.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_reload();
                                }
                                else {
                                    $.ligerDialog.error(obj.Message);
                                }

                            },
                            error: function () {
                                top.$.ligerDialog.error('删除失败！');
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn("请选择发票");
            }
        }


        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Finance_Invoice.save.xhd", type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_reload();
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('操作失败！');
                    }
                });

            }

        }
        function f_reload() {
            $("#maingrid4").ligerGetGridManager().loadData(true);
            $("#maingrid5").ligerGetGridManager().loadData(true);
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
            <table style='width: 720px' class="bodytable1">
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
                        <input id='Button2' type='button' value='重置' style='width: 80px; height: 24px' onclick="doclear()" />
                        <input id='Button1' type='button' value='搜索' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                </tr>

            </table>
        </form>
    </div>
</body>
</html>
