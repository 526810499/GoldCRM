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
            $("#layout1").ligerLayout({ leftWidth: 150, allowLeftResize: false, allowLeftCollapse: true, space: 2, heightDiff:1 });
            $("#tree1").ligerTree({
                //url: '../data/Sys_Menu.GetSysApp&rnd=' + Math.random(),
                url: 'Sys_App.GetAppList.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                //usericon: 'App_icon',
                //iconpath: '../../',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return rowid + 1; } },
                    { display: '菜单名称', name: 'Menu_name', align: 'left', width: 190 },
                    {
                        display: '按钮权限', name: 'Menu_icon', align: 'left', width: 550, render: function (item) {
                            //var html = "<div style='vertical-align:middle;color: #FF0000;background:yellow'>";
                            var html = "<div style='vertical-align:middle;'>";
                            var returntxt = item.Sysroler;

                            var arrstr = new Array();
                            var arrstr1 = new Array();
                            var arrid = new Array();
                            var arrname = new Array();
                            arrstr = returntxt.split(",");

                            for (var j = 0; j < arrstr.length - 1; j++) {
                                arrstr1 = arrstr[j].split("|");
                                arrid = arrstr1[0];
                                arrname = arrstr1[1];
                                html += "<input type='checkbox' ";
                                html += "value='" + arrid + "' ";
                                html += " id='" + arrid + "'";
                                html += ">";
                                html += arrname;
                                html += "&nbsp;&nbsp;";
                            }
                            html += "</div>";
                            return html;
                        }
                    }

                ],
                rownumbers: true,
                allowHideColumn:false,
                rowid: "Menu_id",
                onAfterShowData: f_setbtn,
                onCheckRow: f_onCheckRow,
                onCheckAllRow: f_onCheckAllRow,
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                checkbox: true,
                url: "Sys_role.treegrid.xhd",
                width: '100%', height: '100%',
                usePager: false,
                tree: { columnName: 'Menu_name' },
                heightDiff: 29
            });


            var manager = $("#maingrid4").ligerGetGridManager();
            manager._onResize();
            $("tbody> :checkbox").prop("checked", false);


        });

        function onSelect(note) {
            $(" .l-checked", ".l-grid-header-table").removeClass("l-checked");
            //加载数据
            var manager = $("#maingrid4").ligerGetGridManager();
            var url = "Sys_role.treegrid.xhd?appid=" + note.data.id + '&rdm=' + Math.random();
            manager._setUrl(url);
            //执行操作
        }
        function f_setbtn() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.gridloading.hide();
            var note = treemanager.getSelected();
            if (!note) return;

            //获取权限
            var roleid = getparastr("Role_id");           

            $.ajax({
                type: 'get',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: "Sys_role.getauth.xhd?roleid=" + roleid + "&appid=" + note.data.id + '&rdm=' + Math.random(),
                success: function (data) {
                    //alert(data);   
                    var arrstr = new Array();
                    var arrmenu = new Array();
                    var arrbtn = new Array();

                    arrmenu = data.menus; 
                    for (var i = 0; i < arrmenu.length; i++) {
                        if (arrmenu[i].length > 0) {
                            manager.setCheck(arrmenu[i]);
                            //console.log(arrmenu[i]);
                        }
                    }
                    
                    arrbtn = data.btns;
                    for (var j = 0; j < arrbtn.length; j++) {
                            $("#" + arrbtn[j]).prop("checked", true);  
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log("f_setbtn err！");
                }
            });
        }

        function f_save() {
            var notes = treemanager.getSelected();
            if (notes != null && notes != undefined) {
                var app = notes.data.id;

                var manager = $("#maingrid4").ligerGetGridManager();
                var rows = manager.getCheckedRows();
                var menu = "";
                $(rows).each(function () {
                    menu += this.Menu_id + ",";
                });

                var btn = "";
                $(".l-grid-body2 input[type='checkbox']").each(function (i) {
                    if ($(this).prop("checked")) {
                        btn += $(this).attr("id") + ",";
                    }
                })


                var roleid = getparastr("Role_id");
                var savetext = "{role_id:'" + roleid + "',";
                savetext += "app:'" + app + "',";
                savetext += "menu:'" + menu + "',";
                savetext += "btn:'" + btn + "'}";

                //alert(savetext);

                f_saving();
                $.ajax({
                    type: 'post',
                    url: "Sys_role.saveauth.xhd?postdata=" + savetext + '&rdm=' + Math.random(),
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
            else {
                f_error("请选择模块！");
            }

        }
        function EditMenu() {
            $(":checkbox").prop("checked", true);
        }
        function DeleteMenu() {
            $(":checkbox").prop("checked", false);
            $("#_38").prop("checked", true);
        }
        function gridreload(Appid) {
            var manager = $("#maingrid4").ligerGetGridManager();
            //alert('onSelect:' + note.data.id);
            var url = "../../data/Sys_Menu_data.aspx?action=treegrid&appid=" + Appid;
            manager._setUrl(url);
        };

        function f_onCheckRow(checked, data) {
            var returntxt = data.Sysroler;
            var arrstr = new Array();
            var arrstr1 = new Array();
            var arrid = new Array();
            var arrname = new Array();
            arrstr = returntxt.split(",");

            if (checked) {
                for (var j = 0; j < arrstr.length; j++) {
                    arrstr1 = arrstr[j].split("|");
                    arrid = arrstr1[0];
                    arrid && $("#" + arrid).prop("checked", true);
                }
                //parent
                var manager = $("#maingrid4").ligerGetGridManager();
                if (manager.isLeaf(data)) {
                    var row = manager.getParent(data);
                    if (row) manager.setCheck(row.Menu_id);
                }
                else {
                    var rows = data.children;
                    if (!rows) return;
                    $(rows).each(function (i, item) {
                        manager.setCheck(rows[i].Menu_id);
                        var returntxt = item.Sysroler;
                        var arrstr = new Array();
                        var arrstr1 = new Array();
                        var arrid = new Array();
                        var arrname = new Array();
                        arrstr = returntxt.split(",");

                       
                        for (var j = 0; j < arrstr.length; j++) {
                            arrstr1 = arrstr[j].split("|");
                            arrid = arrstr1[0];
                            console.log(arrid);
                            if (arrid && checked) {
                                $("#" + arrid).prop("checked", true);
                            }
                            else {
                                $("#" + arrid).prop("checked", false);
                            }
                        }
                    })
                }
            }
            else {
                for (var j = 0; j < arrstr.length; j++) {
                    arrstr1 = arrstr[j].split("|");
                    arrid = arrstr1[0];
                    arrid && $("#" + arrid).prop("checked", false);
                }
                //children
                var manager = $("#maingrid4").ligerGetGridManager();
                if (!manager.isLeaf(data)) {
                    var rows = data.children;
                    $(rows).each(function (i) {
                        manager.setUnCheck(rows[i].Menu_id);
                    })
                }
            }


        }
        function f_onCheckAllRow(checked, grid) {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getData();
            if (rows.length > 0) {
                $(rows).each(function (i, item) {
                    //alert(i+";"+item.Sysroler)
                    var returntxt = item.Sysroler;
                    var arrstr = new Array();
                    var arrstr1 = new Array();
                    var arrid = new Array();
                    var arrname = new Array();
                    arrstr = returntxt.split(",");

                    for (var j = 0; j < arrstr.length; j++) {

                        arrstr1 = arrstr[j].split("|");
                        arrid = arrstr1[0];

                        if (arrid && checked) {
                            $("#" + arrid).prop("checked", true);
                        }
                        else {
                            $("#" + arrid).prop("checked", false);
                        }
                    }

                })
            }
        }
        function f_success() {
            setTimeout(function () {
                $.ligerDialog.confirm("是否继续编辑", "保存成功", function (ok) {
                    $.ligerDialog.closeWaitting();
                    if (!ok) {
                        dialog.close();
                    }
                });
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
    <div id="layout1" style="margin: -1px;">
        <div position="left" title="系统目录">
            <div id="treediv1" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto; padding-top: 5px;">
                <ul id="tree1"></ul>
            </div>
        </div>
        <div position="center" title="目录与按钮">

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </div>
</body>
</html>
