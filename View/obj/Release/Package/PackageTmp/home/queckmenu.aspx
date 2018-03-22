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
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var str1 = getparastr("rid");
            $("#maingrid4").ligerGrid({
                columns: [
                    {
                         display: '图标', name: 'Menu_icon', width: 50, render: function (item) {
                             return "<img style='width:16px;height:16px;margin-top:8px;' src='../" + item.Menu_icon + "'/>"
                         }
                    },                  
                    { display: '菜单名', name: 'Menu_name', align: 'left',width:200 }
                     
                ],
                checkbox: true,
                usePager: false,
                //dataAction: 'server',
                //pageSize: 30,
                //pageSizeOptions: [20, 30, 50, 100],
                url: "Personal_queckmenu.get.xhd?rnd=" + Math.random(),
                width: '100%',
                height: '100%',
                heightDiff: 31
            });
            var items = [];
            items.push({ type: 'button', text: '添加', icon: '../images/icon/11.png', disable: true, click: function () { add(); } });
            items.push({ type: 'button', text: '移除', icon: '../images/icon/12.png', disable: true, click: function () { remove(); } });
            
            $("#serchbar1").ligerToolBar({
                items: items
            })
            
        });


        function add() {
            top.$.ligerDialog.open({
                title: '选择目录', width: 600, height: 500, url: 'home/getMenu.aspx', buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ], zindex: 9003
            });
            return false;
        }

        function f_selectContactOK(item, dialog) {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = dialog.frame.f_select();
            if (rows.length < 1) {                
                return;
            }

            //过滤重复

            var data = manager.getData();

            for (var i = 0; i < rows.length; i++) {

                var add = 1;
                for (var j = 0; j < data.length; j++) {
                    if (rows[i].Menu_id == data[j].menu_id) {
                        add = 0;
                    }
                }
                if (add == 1) {
                    //price
                    rows[i].menu_id = rows[i].Menu_id;
                    manager.addRow(rows[i]);
                }
            }
            dialog.close();
        }

        function remove() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();

            manager.deleteSelectedRow();           
        }
       
        function f_add()
        {
            var manager = $("#maingrid4").ligerGetGridManager();
            return manager.getAdded();
        }

        function f_del()
        {
            var manager = $("#maingrid4").ligerGetGridManager();
            return manager.getDeleted();
        }

        function f_save()
        {
            var manager = $("#maingrid4").ligerGetGridManager();
            return manager.getChanges();
        }

    </script>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div style="padding-top:10px;">
            <div id="serchbar1"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>

</body>
</html>
