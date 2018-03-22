<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'RoleID', type: 'int', width: 50 },
                    { display: 'ÐòºÅ', width: 50, render: function (item,i) { return item.n; } },
                    { display: '½ÇÉ«Ãû', name: 'RoleName' },
                    { display: '½ÇÉ«ÃèÊö', name: 'RoleDscript', width: 300 },
                    { display: 'ÅÅÐò', name: 'RoleSort', width: 50 }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                url: "Sys_role.grid.xhd",
                width: '100%',
                height: '100%',
                heightDiff: -5,
                onRClickToSelect: true,
                checkbox:true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });



            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            
        });
        
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            
            var url = "Sys_role.grid.xhd";
            manager._setUrl(url);
        };
        
    </script>
</head>
<body>
    <form id="mainform" onsubmit="return false">
       
            <div id="toolbar"></div>
            <div id="grid" style="">
            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>
</body>
</html>
