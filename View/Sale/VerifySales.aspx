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
        var user = 0;
        $(function () {
            initLayout();

            $(window).resize(function () {
                initLayout();
            });
            $("#maingrid4").ligerGrid({
                columns: [
                    {
                        display: '�ɽ�ʱ��', name: 'Order_date', width: 150, render: function (item) {
                            return formatTime(item.create_time);
                        }
                    },
                    { display: '�ɽ�����', name: 'F_dep_id', width: 80, render: function (item, i) { return item.dep_name; } },
                    {
                        display: '���۶���', name: 'Serialnumber', width: 150, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=PView('" + item.Serialnumber + "','" + item.id + "')>" + item.Serialnumber + "</a>";
                            return html;
                        }
                    },
                     {
                         display: '��Ա����', name: 'VipCardType', width: 80, render: function (item) {
                             var VipCardType = item.VipCardType;
                             return GetVIPCardType(VipCardType);
                         }
                     },
                      { display: '������', name: 'BarCode', align: 'left', width: 120 },
                    {
                        display: '�ͻ�', name: 'Customer_id', width: 150, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('customer','" + item.Customer_id + "')>";
                            if (item.cus_name)
                                html += item.cus_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    { display: '��Ʒ����', name: 'product_name', align: 'left', width: 150 },
                    { display: '��Ʒ���', name: 'category_name', align: 'left', width: 150, totalSummary: { type: 'total', render: function (item) { return "<span id='tspan'>�ϼ�:</span>"; } } },
                    {
                        display: '����(��)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                            return toMoney(item.Weight);
                        }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='Weight'>" + item.sum + "</span>"; } }
                    },
                    {
                        display: '�����ܼ�', name: 'amount', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.amount);
                        }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='amount'>" + item.sum + "</span>"; } }
                    },
                    {
                        display: '�Ż�(��)', name: 'Discounts', width: 80, align: 'right', render: function (item) {
                            return toMoney(item.Discounts);
                        }, totalSummary: { type: 'sum', render: function (item, i) { return "<span id='Discounts'>" + item.sum + "</span>"; } }
                    },
                    {
                        display: '״̬', name: 'VerifyStatus', width: 80, align: 'right', render: function (item) {
                            if (item.VerifyStatus == 1) { return "�Ѻ���"; } else { return "������"; }

                        }
                    },

                    { display: '�ɽ���Ա', name: 'emp_id', width: 80, render: function (item, i) { return item.emp_name; } }
                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [10, 20, 30, 40, 50, 60, 80, 100, 120],
                url: "Sale_order.gridData.xhd?cwVerify=1&user=" + user + "&datacount=1&rnd=" + Math.random() + "&startdate=<%=DateTime.Now.AddDays(-1).Date%>",
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

            $("#btn_serch").ligerButton({ text: "����", width: 60, click: doserch });
            $("#btn_reset").ligerButton({ text: "����", width: 60, click: doclear });
        });

        function toolbar() {

            $.get("toolbar.GetSys.xhd?mid=verifysales&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');

                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                $("#sstatus").ligerComboBox({
                    data: [
                    { text: '����', id: '' },
                    { text: '�Ѻ���', id: '1' },
                    { text: '������', id: '0' },
                    ], valueFieldID: 'vstatus',
                });
                items.push({
                    id: "sbtn",
                    type: 'serchbtn',
                    text: '�߼�����',
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
            $("#T_OrderID").ligerTextBox({ width: 190 });

            if (user == 0) {

                var e = $('#employee').ligerComboBox({ width: 96 });
                var f = $('#department').ligerComboBox({
                    width: 97,
                    selectBoxWidth: 240,
                    selectBoxHeight: 200,
                    valueField: 'id',
                    textField: 'text',
                    treeLeafOnly: false,
                    tree: {
                        url: 'hr_department.tree.xhd?qxz=1&rnd=' + Math.random(),
                        idFieldName: 'id',
                        checkbox: false
                    },
                    onSelected: function (newvalue) {
                        $.get("hr_employee.combo.xhd?qxz=1&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                            e.setData(eval(data));
                        });
                    }
                });
            } else {
                $(".truser").hide();
            }

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
            var sendtxt = "&cwVerify=1&user=" + user + "&datacount=1&rnd=" + Math.random();
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

                var buttons = [];

                buttons.push({ text: '����ͨ��', onclick: f_save });


                f_openWindow2('sale/VerifyOrder_view.aspx?id=' + row.id, "�鿴����" + row.Serialnumber, 1200, 700, buttons);
            }
            else {
                $.ligerDialog.warn('��ѡ�񶩵���');
            }
        }

        function PView(oid, id) {

            f_openWindow('sale/VerifyOrder_view.aspx?id=' + id, "�鿴����" + oid, 1200, 700);

        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();

            if (!issave) {
                return;
            }

            dialog.close();
            $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
            $.ajax({
                url: "Sale_order.VerifySave.xhd?vstatus=1", type: "POST",
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
                    $.ligerDialog.error('����ʧ�ܣ�');
                }
            });
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };




        function exports() {

            var sendtxt = "&etype=1&datacount=1&user=2&rnd=" + Math.random();
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
            <table>
                <tr>
                    <td class="truser">
                        <div style='width: 60px; text-align: right; float: right'>�����ŵ꣺</div>
                    </td>
                    <td class="truser">

                        <div style='width: 100px; float: left'>
                            <input type='text' id='department' name='department' />
                        </div>
                    </td>
                    <td class="truser">
                        <div style='width: 60px; text-align: right; float: right'>������Ա��</div>
                    </td>
                    <td class="truser">

                        <div style='width: 98px; float: left'>
                            <input type='text' id='employee' name='employee' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�����ţ�</div>
                    </td>
                    <td style="width: 200px">

                        <div style='width: 98px; float: left'>
                            <input type='text' id='T_OrderID' name='T_OrderID' />
                        </div>
                    </td>
                    <td style="width: 65px">
                        <div style='width: 60px;'>����ʱ�䣺</div>
                    </td>
                    <td style="width: 215px">
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' value="<%=(DateTime.Now.AddDays(-7).Date).ToString("yyyy-MM-dd") %>" ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' value="<%=(DateTime.Now.AddDays(1).Date).ToString("yyyy-MM-dd") %>" ligerui='{width:96}' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>״̬��</div>
                    </td>
                    <td>
                        <input type='select' id='sstatus' name='sstatus' ltype='text' ligerui='{width:120}' /></td>
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
