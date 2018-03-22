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
                    { display: '序号', width: 50, name: 'n' },
                    { display: '编号', name: 'receivable_no', width: 180 },
                    { display: '订单', name: 'Order_no', width: 180},                   
                    { display: '所属客户', name: 'cus_name', width: 200, align: 'left'},
                    {
                        display: '应收金额', name: 'receivable_amount', width: 100,align:'right', render: function (item, i) {
                            return toMoney(item.receivable_amount);
                        }, totalSummary: { type: 'sum_money' }
                    },
                    {
                        display: '已收金额', name: 'received_amount', width: 100, align: 'right', render: function (item, i) {
                            return toMoney(item.received_amount);
                        }, totalSummary: { type: 'sum_money' }
                    },
                    {
                        display: '未收余额', name: 'arrears_amount', width: 100, align: 'right', render: function (item, i) {
                            return toMoney(item.arrears_amount);
                        }, totalSummary: { type: 'sum_money' }
                    },
                    {
                        display: '应收时间', name: 'receivable_time', width: 100, render: function (item, i) {
                            return formatTimebytype(item.receivable_time, "yyyy-MM-dd");
                        }
                    }
                    
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "Finance_Receivable.grid.xhd",
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

            $('form').ligerForm();
            initSerchForm();
            toolbar();

            $("#btn_serch").ligerButton({ text: "搜索", width: 60, click: doserch })
            $("#btn_reset").ligerButton({ text: "重置", width: 60, click: doclear })
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
               
                manager = $("#maingrid4").ligerGetGridManager();
                manager.onResize();
        }
        function initSerchForm() {

        }
        function serchpanel() {
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                manager.onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                manager.onResize();
            }
        }
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;

            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("CRM_Contact.grid.xhd?" + serchtxt);
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
    <div class="az" style="padding-left: 10px;">
        <form id='serchform'>
            <table style='width: 730px'>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>客户名称：</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:196}' />
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>联系人：</div>
                    </td>
                    <td>
                        <input id='contact' name="contact" type='text' ltype='text' ligerui='{width:120}' />
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>手机：</div>
                    </td>
                    <td>
                        <input type='text' id='tel' name='tel' ltype='text' ligerui='{width:163}' />
                    </td>

                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>录入时间：</div>
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
                        <div style='width: 60px; text-align: right; float: right'>QQ：</div>
                    </td>
                    <td>
                        <input id='qq' name="qq" type='text' ltype='text' ligerui='{width:120}' />

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
