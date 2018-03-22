<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 200, allowLeftResize: false, allowLeftCollapse: true, space: 5, heightDiff: -10 });
            $("#tree1").ligerTree({
                url: 'hr_department.tree.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                usericon: 'd_icon',
                iconpath: "../images/icon/",
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first div:first").click();
                }

            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });



            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '岗位名称', name: 'post_name', width: 120 },
                    { display: '部门名称', name: 'dep_name', width: 120 },
                    { display: '职务级别', name: 'position_name', width: 120 },
                    { display: '姓名', name: 'emp_name', width: 120 },
                    { display: '备注', name: 'note', width: 120 }

                ],
                rownumbers: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                onSelectRow: function (row, index, data) {
                    //alert('onSelectRow:' + index + " | " + data.ProductName); 
                },
                url: "hr_post.grid.xhd?depid=0",
                width: '100%',
                height: '100%',
                heightDiff: -13,
                onRClickToSelect: true,

                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
            toolbar();
        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=hr_post&rnd=" + Math.random(), function (data, textStatus) {
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
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#maingrid4").ligerGetGridManager()._onResize();
            });
        }


        function onSelect(note) {
            var manager = $("#maingrid4").ligerGetGridManager();
            var url = "hr_post.grid.xhd?depid=" + note.data.id + "&rnd=" + Math.random();
            manager._setUrl(url);
        }



        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            if (rows && rows != undefined) {
                f_openWindow('hr/hr_post_add.aspx?depid=' + rows.dep_id + "&postid=" + rows.id, "修改岗位", 530, 380, f_save);
            }
            else {
                $.ligerDialog.warn('请选择岗位！');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('hr/hr_post_add.aspx?depid=' + notes.data.id, "新增岗位", 530, 380, f_save);
            }
            else {
                $.ligerDialog.warn('请选择部门！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("角色删除后不能恢复，\n您确定要移除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "hr_post.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
                            dataType: "json",
                            success: function (result) {
                                top.$.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_load();
                                }
                                else {
                                    top.$.ligerDialog.error(obj.Message);
                                }

                            },
                            error: function () {
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('删除失败！');
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "hr_post.save.xhd", type: "POST",
                    data: issave,
                    dataType: "json",
                    success: function (result) {
                        top.$.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_load();
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
        function f_load() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }

    </script>
</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">


        <div id="layout1" style="margin: 10px">
            <div position="left" title="组织架构">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;padding-top:5px;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center">
                <div id="toolbar" style="margin-top: 10px;"></div>
                <div id="maingrid4" style="margin: -1px;"></div>

            </div>
        </div>

    </form>
</body>
</html>
