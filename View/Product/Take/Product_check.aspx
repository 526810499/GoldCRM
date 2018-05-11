<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />
    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js?v=3" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    {
                        display: '�̵㵥��', name: 'id', align: 'left', width: 300, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=PView('" + item.id + "')>" + item.id + "</a>";
                            return html;
                        }
                    },
                    //{ display: '�̵�ֿ�', name: 'product_warehouse', align: 'left', width: 200 },
                    { display: '�̵㲿��', name: 'dep_name', align: 'left', width: 120 },
                    { display: '������', name: 'CreateName', align: 'left', width: 120 },
                    {
                        display: '����ʱ��', name: 'create_time', width: 150, align: 'left', render: function (item) {
                            return formatTime(item.create_time, 'yyyy-MM-dd HH:mm:ss');
                        }
                    },
                    {
                        display: '�޸�ʱ��', name: 'update_time', width: 150, align: 'left', render: function (item) {
                            return formatTime(item.update_time);
                        }
                    },
                    {
                        display: '״̬', name: 'status', width: 100, align: 'left', render: function (item) {
                            switch (item.status) {
                                case 0:
                                    return "<span style='color:#0066FF'> δ�ύ��� </span>";
                                case 1:
                                    return "<span style='color:#00CC66'> �ȴ���� </span>";
                                case 2:
                                    return "<span style='color:#009900'> ���ͨ�� </span>";
                                case 3:
                                    return "<span style='color:#FF3300'> ��˲�ͨ�� </span>";
                            }
                        }
                    },
                    { display: '��ע', name: 'remark', width: 120 }

                ],
                dataAction: 'server',
                url: "Product_TakeStock.grid.xhd?rnd=" + Math.random(),
                pageSize: 30,
                pageSizeOptions: [10, 20, 30, 40, 50, 60, 80, 100, 120],
                width: '100%',
                height: '100%',
                heightDiff: -8,
                detail: {
                    height: 'auto',
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        var grid = document.createElement('div');
                        $(p).append(grid);
                        $(grid).css('margin', 3).ligerGrid({
                            columns: [
                                { display: '��Ʒ����', name: 'product_name', align: 'left', width: 120 },
                                { display: '��Ʒ���', name: 'category_name', align: 'left', width: 120 },
                                { display: '������', name: 'BarCode', align: 'left', width: 160 },
                                {
                                    display: '����(��)', name: 'Weight', width: 50, align: 'left', render: function (item) {
                                        return toMoney(item.Weight);
                                    }
                                },
                                {
                                    display: '����С��(��)', name: 'CostsTotal', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.CostsTotal);
                                    }
                                },
                                 {
                                     display: '�̵�״̬', name: 'Status', align: 'left', width: 160, render: function (item) {
                                         switch (item.status) {
                                             case 1:
                                                 return "����";
                                                 break;
                                             case 2:
                                                 return "<span style='color:blue'>��ӯ</span>";
                                             case 3:
                                                 return "<span style='color:red'>�̿�</span>";
                                         }

                                     }
                                 },
                               { display: '��ע', name: 'remark', align: 'left', width: 180 }
                            ],
                            usePager: false,
                            checkbox: false,

                            url: "Product_TakeStock.gridDetail.xhd?takeid=" + r.id,
                            width: '99%', height: '180',
                            heightDiff: 0
                        })

                    }
                },
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#btn_serch").ligerButton({ text: "����", width: 60, click: doserch });

            toolbar();

        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=product_check&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);

                }

                items.push({
                    id: "sbtn",
                    type: 'serchbtn',
                    text: '����',
                    icon: '../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });



                //items.push({ type: 'textbox', id: 'sstatus', text: '״̬��' });
                //items.push({ type: 'textbox', id: 'sorderid', text: '���ţ�' });
                //items.push({ type: 'textbox', id: 'scode', text: '�����룺' });
                //items.push({ type: 'textbox', id: 'sck', text: '�̵�ֿ⣺' });
                //items.push({ type: 'button', text: '����', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items
                });
                $("#sorderid").ligerTextBox({ width: 180 });
                $("#scode").ligerTextBox({ width: 180 });
                $("#sstatus").ligerComboBox({
                    data: [
                    { text: '����', id: '' },
                    { text: 'δ�ύ���', id: '0' },
                    { text: '�ȴ����', id: '1' },
                    { text: '���ͨ��', id: '2' },
                    { text: '��˲�ͨ��', id: '3' }
                    ], valueFieldID: 'status',
                });
                //$("#swarehouse_id").ligerComboBox({
                //    width: 150,
                //    selectBoxWidth: 240,
                //    selectBoxHeight: 200,
                //    valueField: 'id',
                //    textField: 'text',
                //    treeLeafOnly: false,
                //    tree: {
                //        url: 'Product_warehouse.tree.xhd?zb=1&qxz=1&rnd=' + Math.random(),
                //        idFieldName: 'id',
                //        checkbox: false
                //    },
                //});
                $("#sbegtime").ligerDateEditor({ showTime: true, labelWidth: 100, labelAlign: 'left' });
                $("#sendtime").ligerDateEditor({ showTime: true, labelWidth: 100, labelAlign: 'left' });
                $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
                $('#serchform').ligerForm();
                $("div[toolbarid='sbtn']").click().hide();

                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }


        function serchpanel() {

            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
        }

        function onSelect(note) {
            doserch();
        }
        //��ѯ
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = "status=" + $("#status").val();
            serchtxt += "&sorderid=" + $("#sorderid").val();
            serchtxt += "&scode=" + $("#scode").val();
            //serchtxt += "&swarehouse_id=" + $("#swarehouse_id_val").val();
            serchtxt += "&sbegtime=" + $("#sbegtime").val();
            serchtxt += "&sendtime=" + $("#sendtime").val();
            serchtxt += sendtxt;
            var manager = $("#maingrid4").ligerGetGridManager();
            manager._setUrl("Product_TakeStock.grid.xhd?" + serchtxt);
        }

        //����
        function doclear() {

            $("#form1").each(function () {
                this.reset();
            });
        }

        function auth() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                var buttons = [];
                if (rows.status == 1) {
                    buttons.push({ text: '���ͨ��', onclick: f_saveYesAuth });
                    buttons.push({ text: '��˲�ͨ��', onclick: f_saveNoAuth });
                }
                f_openWindow2('product/Take/Product_CheckAdd.aspx?authbtn=1&id=' + rows.id + "&astatus=" + rows.status, "����̵㵥", 1050, 680, buttons);
            }
            else {
                $.ligerDialog.warn('��ѡ���̵㵥��');
            }
        }
        function PView(id) {
            f_openWindow2('product/Take/Product_CheckAdd.aspx?id=' + id, "�鿴�㵥", 1050, 680);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                var buttons = [];
                if (rows.status == 0) {
                    buttons.push({ text: '����', onclick: f_save });
                    buttons.push({ text: '���沢�ύ���', onclick: f_saveAuth });
                }
                f_openWindow2('product/Take/Product_CheckAdd.aspx?id=' + rows.id + "&astatus=" + rows.status, "�޸��̵㵥", 1050, 680, buttons);
            }
            else {
                $.ligerDialog.warn('��ѡ���̵㵥��');
            }
        }
        function add() {
            var buttons = [];
            buttons.push({ text: '����', onclick: f_save });
            //buttons.push({ text: '�����̵㵥', onclick: create_take });
            buttons.push({ text: '���沢�ύ���', onclick: f_saveAuth });
            f_openWindow2('product/Take/Product_CheckAdd.aspx?astatus=0', "�����̵㵥", 1050, 680, buttons);
        }

        function create_take() {


        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("�̵㵥ɾ�����ָܻ���ȷ��ɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "Product_TakeStock.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
                            dataType: 'json',
                            success: function (result) {
                                $.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_load();
                                }
                                else {
                                    $.ligerDialog.error(obj.Message);
                                }
                            },
                            error: function () {
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('ɾ��ʧ�ܣ�');
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn("��ѡ���̵㵥");
            }

        }

        function f_saveAuth(item, dialog) {
            Save(item, dialog, 1);
        }

        function f_save(item, dialog) {
            Save(item, dialog, 0);
        }

        function Save(item, dialog, auth) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "Product_TakeStock.save.xhd?auth=" + auth, type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            if (obj.Message != null && obj.Message != undefined) {
                                $.ligerDialog.warn(obj.Message);
                            }
                            f_load();
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }
                        //f_load();     
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }

        function f_saveYesAuth(item, dialog) {
            UserAuth(item, dialog, 2);
        }
        function f_saveNoAuth(item, dialog) {
            UserAuth(item, dialog, 3);
        }
        function UserAuth(item, dialog, auth) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "Product_TakeStock.Auth.xhd?auth=" + auth, type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            if (obj.Message != null && obj.Message != undefined) {
                                $.ligerDialog.warn(obj.Message);
                            }
                            f_load();
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }
                        //f_load();     
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }


        function f_load() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }

    </script>
</head>
<body style="padding: 0px; overflow: hidden;">

    <form id="form1" onsubmit="return false">

        <div style="padding: 10px;">
            <div id="toolbar" style="margin-top: 10px;"></div>
            <div id="grid">
                <div id="maingrid4" style="margin: -1px;"></div>
            </div>
        </div>

    </form>

    <div class="az">
        <form id='serchform'>
            <table style='width: 720px'>
                <tr>

                    <td>
                        <div style='width: 40px; text-align: right; float: right'>���ţ�</div>
                    </td>
                    <td>
                        <div style='float: left'>
                            <input type='text' id='sorderid' name='sorderid' ltype='text' ligerui='{width:120}' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 80px; text-align: right; float: right'>�����룺</div>
                    </td>
                    <td>
                        <input id='scode' name="scode" type='text' /></td>
                    <%--                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�̵�ֿ⣺</div>
                    </td>
                    <td>
                        <input type='select' id='swarehouse_id' name='swarehouse_id' ltype='text' ligerui='{width:120}' />

                    </td>--%>

                    <td>
                        <div style='width: 80px; text-align: right; float: right'>״̬��</div>
                    </td>
                    <td>
                        <input type='select' id='sstatus' name='sstatus' ltype='text' ligerui='{width:120}' /></td>

                    <td>
                        <div style='width: 40px; text-align: right; float: right'>ʱ�䣺</div>
                    </td>
                    <td>
                        <input type='text' id='sbegtime' name='sbegtime' />

                    </td>
                    <td>~  
                    </td>
                    <td>
                        <input type='text' id='sendtime' name='sendtime' />
                    </td>

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
