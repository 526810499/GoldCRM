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
                    //{ display: '序号', width: 50, name: 'n' },
                    { display: '编号', name: 'Receive_num', width: 180 },
                    {
                        display: '应收单', name: 'receivable_no', width: 180, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('receivable','" + item.Receivable_id + "')>";
                            if (item.receivable_no)
                                html += item.receivable_no;
                            html += "</a>";
                            return html;
                        }
                    },                    
                    {
                        display: '收款金额', name: 'Receive_amount', width: 100, align: 'right', render: function (item, i) {
                            return toMoney(item.Receive_amount);
                        }, totalSummary: { type: 'sum_money' }
                    },                    
                    {
                        display: '收款时间', name: 'Receive_date', width: 100, render: function (item, i) {
                            return formatTimebytype(item.Receive_date, "yyyy-MM-dd");
                        }
                    },
                    { display: '收款人', name: 'payee', width: 100}

                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "Finance_Receive.grid.xhd",
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -10,
                onRClickToSelect: true,

                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            $('#serchform').ligerForm();
            initSerchForm();
            toolbar();

            $("#btn_serch").ligerButton({ text: "搜索", width: 60, click: doserch })
            $("#btn_reset").ligerButton({ text: "重置", width: 60, click: doclear })
        });

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=finance_receive&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
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
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });


                manager = $("#maingrid4").ligerGetGridManager();
                manager._onResize();
            });
        }
        function initSerchForm() {

        }
        function serchpanel() {
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                manager._onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                manager._onResize();
            }
        }
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;

            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("Finance_Receive.grid.xhd?" + serchtxt);
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

        function add() {
            f_openWindow("finance/Receive_add.aspx", "新增收款", 730, 450, f_save);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('finance/Receive_add.aspx?id=' + row.id, "修改收款", 730, 450, f_save);
            } else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (!row) {
                $.ligerDialog.warn("请选择数据");
                return;
            }
            $.ligerDialog.confirm("数据删除无法恢复，确定删除？", function (yes) {
                if (yes) {
                    $.ajax({
                        url: "Finance_Receive.del.xhd", type: "POST",
                        data: { id: row.id, rnd: Math.random() },
                        dataType: "json",
                        success: function (result) {

                            top.$.ligerDialog.closeWaitting();

                            var obj = eval(result);

                            if (obj.isSuccess) {
                                f_reload();
                            }
                            else {
                                top.$.ligerDialog.error(obj.Message);
                            }


                        },
                        error: function () {
                            top.$.ligerDialog.error('删除失败！');
                        }
                    });
                }
            })

        }


        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Finance_Receive.save.xhd", type: "POST",
                    data: issave,
                    dataType: "json",
                    success: function (result) {
                        top.$.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_reload();
                        }
                        else {
                            top.$.ligerDialog.error(obj.Message);
                        }
                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('操作失败！');
                    }
                });

            }
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

        function toimport() {
            f_openWindow('crm/customer/contact_import.aspx', '联系人导入', 540, 295);
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
    <div class="az" style="padding-left: 10px;">
        <form id='serchform'>
           <table style='width: 730px'>               
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>收款时间：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>收款人：</div>
                    </td>
                    <td>
                        <input id='T_emp' name="T_emp" type='text' ltype='text' ligerui='{width:120}' />

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
