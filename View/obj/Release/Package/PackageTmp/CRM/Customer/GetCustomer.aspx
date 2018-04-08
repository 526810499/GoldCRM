<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var str1 = getparastr("rid");
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '���', width: 50, render: function (item, i) { return item.n; } },
                    { display: '�ͻ�����', name: 'cus_name', width: 160, align: 'left' },

                    {
                        display: '�ͻ�����', name: '', width: 120, render: function (item) {
                            return "��" + item.department + "��" + item.employee;
                        }
                    },

                    {
                        display: '������', name: 'lastfollow', width: 90, render: function (item) {
                            return formatTimebytype(item.lastfollow, 'yyyy-MM-dd');
                        }
                    },
                    { display: '�绰', name: 'cus_tel', width: 150 }

                ],
                onAfterShowData: function (grid) {
                    $("tr[rowtype='�ѳɽ�']").addClass("l-treeleve2").removeClass("l-grid-row-alt");
                },
                checkbox: false,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "CRM_Customer.grid.xhd?rnd=" + Math.random(),
                width: '100%',
                height: '100%',
                title: "�ͻ��б�",
                heightDiff: -2,
                onLoaded: f_loaded
            });
            toolbar();

            $(document).keydown(function (e) {
                if (e.keyCode == 13 && e.target.applyligerui) {
                    doserch();
                }
            });
        });
        function toolbar() {
            var items = [];
            items.push({ type: 'textbox', id: 'company', text: '������' });
            items.push({ type: 'button', text: '����', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

            $("#serchbar1").ligerToolBar({
                items: items

            });
            $("#company").ligerTextBox({ width: 200, nullText: "����ؼ������������ͻ�" });
            $("#maingrid4").ligerGetGridManager()._onResize();


        }
        function f_loaded() {

            $(".l-panel-header").append("<div id='headerBtn' style='width:100px;float:right;margin-bottom:2px;'><div id = 'btn_add' style='margin-top:2px;'></div></div>");
            $(".l-grid-loading").fadeOut();

            $("#btn_add").ligerButton({
                width: 80,
                text: "����¿ͻ�",
                icon: '../../images/icon/11.png',
                click: addCustomer
            });

            $("#maingrid4").ligerGetGridManager()._onResize();
        }

        function addCustomer() {
            f_openWindow('CRM/Customer/Customer_add.aspx', "�����ͻ�", 770, 490, f_saveCustomer);
        }


        function f_saveCustomer(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();

                $.ajax({
                    url: "CRM_Customer.save.xhd",
                    type: "POST",
                    data: issave,
                    dataType: "json",
                    beforesend: function () {
                        $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                    },
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            doserch();
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }

                        //f_openWindow('CRM/Customer/Customer_add.aspx?cid=' + row.id, "�����ϵ��", 770, 490, f_save);
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }

        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            $.ligerDialog.waitting('���ݲ�ѯ��,���Ժ�...');
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("CRM_Customer.grid.xhd?" + serchtxt);
            manager.loadData(true);
            $.ligerDialog.closeWaitting();
        }
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
        <div>
            <div id="serchbar1" style="margin-top: 10px;"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
