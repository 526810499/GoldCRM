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
    <script src="../../JS/XHD.js" type="text/javascript"></script>



    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 150, allowLeftResize: false, allowLeftCollapse: true, space: 2 });
            $("#tree1").ligerTree({
                url: 'Sys_App.GetAppList.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                usericon: 'App_icon',
                iconpath: '../../',
                checkbox: false,
                itemopen: false
            });

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            toolbar();

            $("#maingrid").ligerGrid({
                columns: [
                    { display: 'ID', name: 'Menu_id', type: 'int', width: 120, align: 'left' },
                    { display: '�˵���', name: 'Menu_name', align: 'left' },
                    { display: '���ӵ�ַ', name: 'Menu_url', align: 'left', width: 300 },
                    {
                        display: 'ͼ��', name: 'Menu_icon', width: 50, render: function (item) {
                            return "<img style='width:16px;height:16px;margin-top:8px;' src='../../" + item.Menu_icon + "'/>"
                        }
                    },
                    //{ display: '��Ӧ�¼�', name: 'Menu_handler' },
                    { display: '����', name: 'Menu_order', width: 50 }


                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                tree: { columnName: 'Menu_name' },
                url: "Sys_Menu.GetMenuV2.xhd?parentid=-1",
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                },
                width: '100%',
                height: '100%',
                heightDiff: -10,
                onRClickToSelect: true
            });

            $("#maingrid").ligerGetGridManager()._onResize();


        });
        function toolbar() {
            var items = [];
            items.push({ type: 'button', text: '����', icon: '../../images/icon/11.png', disable: true, click: add });
            items.push({ type: 'button', text: '�޸�', icon: '../../images/icon/33.png', disable: true, click: edit });
            items.push({ type: 'button', text: 'ɾ��', icon: '../../images/icon/12.png', disable: true, click: del });

            $("#toolbar").ligerToolBar({
                items: items

            });
        }

        function onSelect(note) {
            var manager = $("#maingrid").ligerGetGridManager();
            //manager.showData({ Rows: [], Total: 0 });
            var url = "Sys_Menu.GetMenuV2.xhd?appid=" + note.data.id + "&rnd=" + Math.random();
            manager._setUrl(url);
        }

        function edit() {
            var row = $("#maingrid").ligerGetGridManager().getSelectedRow();
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            if (row != null && row != undefined && notes != null && notes != undefined) {
                f_openWindow('System/sysbase/Sys_Menu_add.aspx?menuid=' + row.Menu_id + '&appid=' + notes.data.id, "�޸�Ŀ¼", 530, 380, f_save);
            }
            else {
                $.ligerDialog.warn('��ѡ��Ŀ¼��');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('System/sysbase/Sys_Menu_add.aspx?appid=' + notes.data.id, "����Ŀ¼", 530, 380, f_save);
            }
            else {
                $.ligerDialog.warn('��ѡ�����˵�Ŀ¼��');
            }
        }

        function del() {
            var manager = $("#maingrid").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("ɾ�����ָܻ���\n��ȷ��Ҫɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "Sys_Menu.del.xhd",
                            data: { menuid: row.Menu_id },
                            success: function (result) {
                                treereload();
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("��ѡ����");
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "Sys_Menu.save.xhd", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        treereload();

                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }

        function treereload() {
            var manager = $("#maingrid").ligerGetGridManager();
            manager.loadData(true);
        }
    </script>
</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">

        <div id="layout1" style="margin-top: -1px; margin-left: -1px">
            <div position="left" title="���˵�ģ��">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center" title="�Ӳ˵�">
                <div style="padding: 5px;">
                    <div id="toolbar"></div>
                    <div style="padding-top: 5px;">
                        <div id="maingrid" style=""></div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
