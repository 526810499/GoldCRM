<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../CSS/input.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID', type: 'int', width: 50 },
                    //{ display: '序号', width: 50, render: function (item, i) { return item.n; } },
                    {
                        display: '名字', name: 'name', width: 120, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view('emp','" + item.id + "')>";
                            if (item.name)
                                html += item.name;
                            html += "</a>";
                            return html;
                        }
                    },
                    { display: '入职日期', name: 'EntryDate' },
                    { display: '生日', name: 'birthday' },
                    { display: '性别', name: 'sex', width: 50 },
                    { display: '部门', name: 'dep_name' },
                    { display: '默认岗位', name: 'post_name' },
                    { display: '默认职务', name: 'position_name' },
                    {
                        display: '可登陆', width: 50, render: function (item) {
                            var html = "<div style='margin-top:5px;'><input type='checkbox'";
                            if (item.canlogin == 1) html += "checked='checked'  ";
                            html += " disabled='disabled' /></div>";
                            return html;
                        }
                    },
                    { display: '状态', name: 'status' }
                ],
                rownumbers:true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "hr_employee.grid.xhd",
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -11,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                },
                //onDblClickRow: function (data, rowindex, rowobj) {
                //    edit();
                //},
                rowtype: "status",
                onAfterShowData: function (grid) {
                    $("tr[rowtype='离职']").addClass("l-leaving").removeClass("l-grid-row-alt");
                }
            });



            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            toolbar();
        });

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=hr_employee&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'stext', text: '姓名：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#stext").ligerTextBox({ width: 200, nullText: "输入姓名搜索" })
                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }
        //查询
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("hr_employee.grid.xhd?" + serchtxt);
        }

        function add() {
            f_openWindow("hr/hr_employee_add.aspx", "新增账号", 730, 490, f_save);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('hr/hr_employee_add.aspx?empid=' + row.id, "修改账号", 730, 490, f_save);
            } else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function changepwd() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            //alert(row.uid);
            if (row) {
                f_openWindow('hr/hr_employee_changpwd.aspx?empid=' + row.id, "修改密码", 400, 200, function (item, dialog) { dialog.frame.f_save(); })
            }
            else {
                top.$.ligerDialog.error('请选择行!');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("员工删除后不能恢复，且员工所在岗位都将清除，\n您确定要移除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "hr_employee.del.xhd", type: "POST",
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
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('删除失败！', "", null, 9003);
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择员工");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();

            //alert(postnum + " @ " + postdata);
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "hr_employee.save.xhd", type: "POST",
                    data: issave,
                    dataType: "json",
                    success: function (result) {
                        $.ligerDialog.closeWaitting();
                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_reload();
                        }
                        else {
                            top.$.ligerDialog.error(obj.Message);
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
    </script>
    <style type="text/css">
        .l-leaving { background: #eee; color: #999; }
    </style>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div style="padding: 10px;">
            <div id="toolbar"></div>

            <div id="maingrid4" style="background: #fff;"></div>


        </div>
    </form>


</body>
</html>
