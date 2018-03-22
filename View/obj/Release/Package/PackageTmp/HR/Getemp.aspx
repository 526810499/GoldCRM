<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {   
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID', type: 'int', width: 50 },
                    { display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    { display: '名字', name: 'name' },
                    { display: '性别', name: 'sex', width: 50 },
                    { display: '部门', name: 'dname' },
                    { display: '职务', name: 'zhiwu' }
                ],
                checkbox: false,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "hr_employee.grid.xhd",
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -1
            });
            toolbar();
        });

        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            //alert(rows);
            return rows;
        }

        function toolbar() {            
                var items = [];                
                items.push({ type: 'button', text: '新增员工', icon: '../images/icon/11.png', disable: true, click: function () { addemp() } });
                items.push({ type: 'textbox', id: 'stext', text: '姓名：' });
                items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items

                });  
            
                $("#stext").ligerTextBox({ width: 200, nullText: "输入姓名搜索" })
                $("#maingrid4").ligerGetGridManager()._onResize();
           
        }
        //查询
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("hr_employee.grid.xhd?" + serchtxt);
            manager.loadData(true);
        }
       
        function addemp() {
            f_openWindow("hr/QuickAddEmp.aspx", "新增账号", 730, 430,f_save,9005);
        }
    
        function f_save(item, dialog) {             
                var issave = dialog.frame.f_save();                 
                if (issave) {
                    dialog.close();
                    $.ajax({
                        url: "hr_employee.save.xhd", type: "POST",
                        data: issave + "&PostData=[]",
                        success: function (responseText) {                            
                            f_reload();
                        },
                        error: function () {                            
                            alert('操作失败！');
                        }
                    });

                }
            
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };
    </script>
   
</head>
<body> 

  <form id="form1" onsubmit="return false"> 
    <div>
        <div id="toolbar" style="margin-top:10px;"></div> 
        <div id="maingrid4" style=" "></div>   
    </div>     
  </form>

 
</body>
</html>
