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
            $("#layout1").ligerLayout({ leftWidth: 150, allowLeftResize: false, allowLeftCollapse: true, space: 5, heightDiff: -10 });
            $("#tree1").ligerTree({
                url: 'Sys_Param.GetApp.xhd?rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                parentIDFieldName: 'pid',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first div:first").click();
                }
            });

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    //{ display: 'pid', name: 'parentid', width: 50 },
                    { display: '参数名', name: 'params_name', width: 200 },
                    { display: '标识Key', name: 'KeyID', width: 50 },
                    { display: '排序', name: 'params_order', width: 50 }

                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],

                url: "Sys_Param.GetParams.xhd?parentid=-1",
                width: '100%',
                height: '100%',
                heightDiff: -13,
                onRClickToSelect: true,
                rownumbers: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
            toolbar();
            


        });
        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=sys_params&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                
                $("#maingrid").ligerGetGridManager()._onResize();
            });

        }


        function onSelect(note) {
            var manager = $("#maingrid").ligerGetGridManager();            
            var url = "Sys_Param.GetParams.xhd?id=" + note.data.id + "&rnd=" + Math.random();
            manager._setUrl(url);
        }

        function edit() {
            var row = $("#maingrid").ligerGetGridManager().getSelectedRow();
            if (row != null && row != undefined) {
                f_openWindow('System/sysmanager/Param_SysParam_add.aspx?paramid=' + row.id, "修改参数", 430, 280,f_save);
            }
            else {
                $.ligerDialog.warn('请选择参数！');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('System/sysmanager/Param_SysParam_add.aspx?parentid=' + notes.data.id, "新增参数", 430, 280,f_save);
            }
            else {
                $.ligerDialog.warn('请选择参数类别！');
            }
        }

        function del() {
            var manager = $("#maingrid").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("删除后不能恢复，\n您确定要删除？", function (yes) {
                    top.$.ligerDialog.waitting('数据保存中,请稍候...');
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "Sys_Param.del.xhd",
                            data: { paramid: row.id, parentid: row.params_type },
                            dataType:'json',
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
                            error: function ()
                            {
                                top.$.ligerDialog.closeWaitting();
                                top.$.ligerDialog.error('操作失败！');
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择行");
            }

        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "Sys_Param.save.xhd", type: "POST",
                    data: issave,
                    dataType:'json',
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
            var manager = $("#maingrid").ligerGetGridManager();
            manager.loadData(true);
        }
    </script>
</head>
<body style="padding: 0px;overflow:hidden;">
    <form id="form1" onsubmit="return false">
        
        <div id="layout1" style="margin:10px">
            <div position="left" title="参数类别">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;padding-top:5px;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center" title="参数信息" style="padding-top:10px;">
                <div id="toolbar"></div>
                <div id="maingrid" style="margin-top: -1px; margin-left: -1px"></div>
            </div>
        </div>
    </form>
</body>
</html>
