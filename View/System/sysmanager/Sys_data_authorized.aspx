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
        var dialog = frameElement.dialog;
        var manager = "";
        var treemanager;
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [                    
                    { display: '组织架构', name: 'dep_name', align: 'left', width: 300 }
                ],
                rownumbers:true,
                rowid: "id",
                onAfterShowData: f_setbtn,
                title:"当角色权限设置为【指定部门】时有效。",
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                checkbox: true,
                url: "hr_department.treegrid.xhd",
                width: '100%', height: '100%',
                usePager: false,
                tree: { columnName: 'dep_name' },
                heightDiff: -11
            });


            var manager = $("#maingrid4").ligerGetGridManager();
            manager._onResize();
            $("tbody> :checkbox").attr("checked", false);


        });

        function f_setbtn() {
            var manager = $("#maingrid4").ligerGetGridManager();
            

            $.ajax({
                type: 'get',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: "Sys_data_authority.get.xhd?Role_id=" + getparastr("Role_id") + '&rdm=' + Math.random(),
                success: function (data) {
                    manager.gridloading.hide();
                    //alert(data);   
                    var arrstr = new Array();

                    arrmenu = data.Rows;
                    for (var i = 0; i < arrmenu.length; i++) {
                        if (arrmenu[i].dep_id) {
                            manager.setCheck(arrmenu[i].dep_id);
                        }
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    manager.gridloading.hide();
                    //f_error("角色还未授权！");
                }
            });
        }

        function f_save() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getCheckedRows();

            var depids = "";
            for (var i = 0; i < rows.length; i++)
            {
                depids += rows[i].id + ",";
            }

            f_saving();
            $.ajax({
                type: 'post',
                url: "Sys_data_authority.save.xhd",
                data: { id: getparastr("Role_id"), depids: depids, rnd: Math.random() },
                success: function (data) {
                    //alert(data);
                    setTimeout(function () {
                        f_success();
                    }, 10);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    f_error("授权失败！");
                }
            });


        }

        function f_success() {
            setTimeout(function () {

                dialog.close();
            }, 200);
        }
        function f_error(message) {
            $.ligerDialog.closeWaitting();
            $(document).ready(function () {
                $.ligerDialog.closeWaitting();
                $.ligerDialog.error(message);
            });
        }
        function f_saving() {
            $.ligerDialog.waitting("正在保存中...");
        }

    </script>

</head>
<body style="padding: 0px; overflow: hidden;">


    <div id="maingrid4" style="margin: -1px;"></div>
</body>
</html>
