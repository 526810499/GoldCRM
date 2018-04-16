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
                            var html = "<a href='javascript:void(0)' onclick=view('ptake','" + item.id + "')>" + item.id + "</a>";
                            return html;
                        }
                    },
                    { display: '�̵�ֿ�', name: 'product_warehouse', align: 'left', width: 200 },
                    { display: '�̵㲿��', name: 'dep_name', align: 'left', width: 120 },
                    { display: '������', name: 'CreateName', align: 'left', width: 120 },
                    {
                        display: '����޸�ʱ��', name: 'update_time', width: 120, align: 'left', render: function (item) {
                            return formatTime(item.update_time);
                        }
                    },
                    {
                        display: '״̬', name: 'status', width: 100, align: 'left', render: function (item) {
                            switch (item.status) {
                                case 0:
                                    return "<span style='color:#0066FF'> δ�����̵㵥 </span>";
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
                                    display: '���۹���(��)', name: 'SalesCostsTotal', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.CostsTotal);
                                    }
                                },
                                {
                                    display: '���ۼ۸�(��)', name: 'SalesTotalPrice', width: 80, align: 'right', render: function (item) {
                                        return toMoney(item.SalesTotalPrice);
                                    }
                                }, {
                                    display: '�̵�״̬', name: 'Status', align: 'left', width: 160, render: function (item) {
                                        switch (status) {
                                            case 1:
                                                return "����";
                                                break;
                                            case 2:
                                                return "<span style='color:blue'>��ӯ</span>";
                                            case 3:
                                                return "<span style='color:red'>�̿�</span>";
                                        }

                                    }
                                }
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
                items.push({ type: 'textbox', id: 'sstatus', text: '״̬��' });
                items.push({ type: 'textbox', id: 'sorderid', text: '���ţ�' });
                items.push({ type: 'textbox', id: 'scode', text: '�����룺' });
                items.push({ type: 'textbox', id: 'sck', text: '�̵�ֿ⣺' });
                items.push({ type: 'button', text: '����', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items
                });
                $("#sorderid").ligerTextBox({ width: 200 });
                $("#scode").ligerTextBox({ width: 250 });
                $("#sstatus").ligerComboBox({
                    data: [
                    { text: '����', id: '' },
                    { text: 'δ�����̵㵥', id: '0' },
                    { text: '�ȴ����', id: '1' },
                    { text: '���ͨ��', id: '2' },
                    { text: '��˲�ͨ��', id: '3' }
                    ], valueFieldID: 'status',
                });
                $("#sck").ligerComboBox({
                    width: 150,
                    selectBoxWidth: 240,
                    selectBoxHeight: 200,
                    valueField: 'id',
                    textField: 'text',
                    treeLeafOnly: false,
                    tree: {
                        url: 'Product_warehouse.tree.xhd?zb=1&qxz=1&rnd=' + Math.random(),
                        idFieldName: 'id',
                        checkbox: false
                    },
                });
                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }


        function onSelect(note) {
            doserch();
        }
        //��ѯ
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;

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
                $.ligerDialog.warn('��ѡ����Ʒ��');
            }
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
                $.ligerDialog.warn('��ѡ����Ʒ��');
            }
        }
        function add() {
            var buttons = [];
            buttons.push({ text: '����', onclick: f_save });
            buttons.push({ text: '�����̵㵥', onclick: create_take });
            buttons.push({ text: '�����̵㵥���ύ���', onclick: f_saveAuth });
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
    <div style="padding: 5px 10px 0px 5px;">
        <form id="form1" onsubmit="return false">
            <div id="layout1" style="">

                <div position="center">
                    <div id="toolbar" style="margin-top: 10px;"></div>
                    <div id="maingrid4" style="margin: -1px;"></div>

                </div>
            </div>
        </form>

    </div>
</body>
</html>
