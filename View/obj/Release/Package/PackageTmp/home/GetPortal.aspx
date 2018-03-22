<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/touch/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>    
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: 'ÐòºÅ', width: 30, render: function (item, i) { return i + 1; } },                    
                    { display: 'Ä£¿éÃû³Æ', name: 'item_title', width: 150 }

                ],                
                checkbox: true,
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../data/C_Sys_portal.except&rnd=" + Math.random(),
                width: '100%',
                height: '100%',

                heightDiff: 0
            });
                        
        });
        
        
        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getCheckedRows();
            return rows;
        }


    </script>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div>
            <div id="serchbar1"></div>
            
            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
