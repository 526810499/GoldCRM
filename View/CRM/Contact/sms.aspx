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
        var manager;
        var manager1;
        $(function () {
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '序号', width: 50, name: 'n' },
                    {
                        display: '短信主题', name: 'sms_title', width: 200, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('sms','" + item.id + "')>";
                            if (item.sms_title)
                                html += item.sms_title;
                            html += "</a>";
                            return html;
                        }
                    },
                    {
                        display: '创建人', name: 'create_id', width: 80, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('emp','" + item.create_id + "')>";
                            if (item.create_name)
                                html += item.create_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    {
                        display: '创建时间', name: 'create_time', width: 140, render: function (item) {
                            return formatTimebytype(item.create_time, 'yyyy-MM-dd hh:mm');
                        }
                    },
                    {
                        display: '审核', width: 50, render: function (item) {
                            var html = "<div style='margin-top:5px;'><input type='checkbox'";
                            if (item.isSend == 1) html += "checked='checked'  ";
                            html += " disabled='disabled' /></div>";
                            return html;
                        }
                    },

                    {
                        display: '审核人', name: 'check_id', width: 80, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(8," + item.check_id + ")>";
                            if (item.check_name)
                                html += item.check_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    {
                        display: '审核时间', name: 'sendtime', width: 140, render: function (item) {
                            return formatTimebytype(item.sendtime, 'yyyy-MM-dd hh:mm');
                        }
                    }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "SMS.grid.xhd",
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
          
            toolbar();
        });

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=sms&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({
                    type: 'serchbtn',
                    text: '高级搜索',
                    icon: '../../images/search.gif',
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
       

        function add() {
            f_openWindow("CRM/contact/sms_add.aspx", "新增短信", 730, 450, f_save);
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (!row) {
                $.ligerDialog.warn('请选择短信！');
                return;
            }

            if (row.isSend == 1) {
                $.ligerDialog.warn('此短信已发送，不能删除！');
                return;
            }

            $.ligerDialog.confirm("短信删除无法恢复，确定删除？", function (yes) {
                if (yes) {
                    $.ligerDialog.waitting('数据删除中,请稍候...');
                    $.ajax({
                        url: "SMS.del.xhd",
                        type: "POST",
                        data: { id: row.id, rnd: Math.random() },
                        dataType: "json",
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
                            $.ligerDialog.error('删除失败！');
                        }
                    });
                }
            });
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            var num = dialog.frame.f_num();

            if (num < 1) return;

            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "SMS.save.xhd", type: "POST",
                    data: issave,
                    dataType: "json",
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
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

        function check() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (!row) { $.ligerDialog.warn('请选择行！'); return; }
            if (row.isSend == 1) { $.ligerDialog.warn('该信息已经审核并发送！'); return; }

            $.ligerDialog.confirm("审核之后，短信将发送并相应扣费，是否继续？", function (yes) {
                if (yes) {
                    $.ajax({
                        url: "SMS.send.xhd",
                        type: "get",
                        data: { id: row.id, rnd: Math.random() },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
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
                            $.ligerDialog.error('系统错误，发送失败！');
                        }
                    });
                }
            });

        }
        function status() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (!row) { $.ligerDialog.warn('请选择行！'); return; }

            f_openWindow("crm/contact/sms_report.aspx?id=" + row.id, '发送状态', 500, 300)
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
  
</body>
</html>
