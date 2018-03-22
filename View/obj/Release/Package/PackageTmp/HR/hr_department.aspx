<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">

        var manager = "";
        var treemanager;
        var tab;
        $(function () {
            tab = $("#layout1").ligerLayout({ leftWidth: 250, allowLeftResize: false, allowLeftCollapse: true, space: 5, heightDiff: -1 });
            $("#tree1").ligerTree({
                url: 'hr_department.tree.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                //parentIDFieldName: 'pid',
                usericon: 'd_icon',
                iconpath:"../images/icon/",
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


            toolbar();
            $("#pageloading").fadeOut(800);
        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=hr_department&rnd=" + Math.random(), function (data, textStatus) {
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
                tab._onResize();
            });
        }


        function onSelect(note) {
            $.getJSON("hr_department.form.xhd?id=" + note.data.id + '&rdm=' + Math.random(), function (data, textStatus) {
                //alert(data);
                var obj = eval(data);
                for (var n in obj) {
                    if (obj[n] == null || obj[n] == "null")
                        obj[n] = "";
                }

                $("#Label1").text(obj.dep_name);
                $("#Label2").text(obj.dep_chief);
                $("#Label3").text(obj.dep_tel);
                $("#Label4").text(obj.dep_email);
                $("#Label5").text(obj.dep_fax);
                $("#Label6").text(obj.dep_add);
                $("#Label7").text(obj.dep_descript);
                $("#Label8").text(obj.dep_order);
            });

        }



        function edit() {
            var notes = treemanager.getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('hr/hr_department_add.aspx?depid=' + notes.data.id, "修改部门", 620, 280, f_save);
            }
            else {
                $.ligerDialog.warn('请选择部门！');
            }
        }
        function add() {
            f_openWindow('hr/hr_department_add.aspx', "新增部门", 620, 280, f_save);
        }

        function del() {
            var manager = $("#tree1").ligerGetTreeManager();
            var node = manager.getSelected();
            if (!node) {
                $.ligerDialog.warn("请选择部门！");
                return;
            }

            if (manager.hasChildren(node.data)) {
                $.ligerDialog.error("含有下级部门，不允许删除！");
                return;
            }

            $.ligerDialog.confirm("部门删除后不能恢复，\n您确定要删除？", function (yes) {
                if (yes) {
                    $.ajax({
                        url: "hr_department.del.xhd", type: "POST",
                        data: { id: node.data.id, rnd: Math.random() },
                        dataType: "json",
                        success: function (result) {
                            top.$.ligerDialog.closeWaitting();

                            var obj = eval(result);

                            if (obj.isSuccess) {
                                treereload();
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

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "hr_department.save.xhd",
                    type: "POST",
                    data: issave,
                    dataType: "json",
                    success: function (result) {
                        top.$.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            treereload();
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

        function treereload() {
            treemanager = $("#tree1").ligerGetTreeManager();
            treemanager.reload();
        }
    </script>
</head>
<body style="padding: 0px; overflow: hidden;">
    <form id="form1" onsubmit="return false">
        <div style="padding: 10px; padding-bottom: 0;">
            <div id="toolbar"></div>
        </div>

        <div id="layout1" style="margin: -1px">
            <div position="left" title="组织架构">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;padding-top:5px;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center">

                <table class="bodytable0" style="width: 100%; margin: -1px">

                    <tr>
                        <td height="27" width="20%" class="table_label">名称：</td>
                        <td height="27">
                            <span id="Label1"></span>
                        </td>
                    </tr>
                    <tr>
                        <td height="27" class="table_label">负责人：</td>
                        <td height="27">
                            <span id="Label2"></span>
                        </td>
                    </tr>
                    <tr>
                        <td height="27" class="table_label">电话：</td>
                        <td height="27">
                            <span id="Label3"></span>
                        </td>
                    </tr>
                    <tr>
                        <td height="27" class="table_label">邮箱：</td>
                        <td height="27">
                            <span id="Label4"></span>
                        </td>
                    </tr>
                    <tr>
                        <td height="27" class="table_label">传真：</td>
                        <td height="27">
                            <span id="Label5"></span>
                        </td>
                    </tr>
                    <tr>
                        <td height="27" class="table_label">地址：</td>
                        <td height="27">
                            <span id="Label6"></span>
                        </td>
                    </tr>
                    <tr>
                        <td height="27" class="table_label">描述：</td>
                        <td height="27">
                            <span id="Label7"></span>
                        </td>
                    </tr>
                    <tr>
                        <td height="27" class="table_label">排序：</td>
                        <td height="27">
                            <span id="Label8"></span>
                        </td>
                    </tr>
                </table>


            </div>
        </div>



    </form>
</body>
</html>
