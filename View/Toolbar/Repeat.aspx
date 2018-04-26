<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"> </script>
    <script src="../lib/ligerUI/js/ligerui.all.js" type="text/javascript"> </script>
    <script src="../lib/jquery.form.js" type="text/javascript"> </script>
    <script src="../JS/XHD.js" type="text/javascript"> </script>
    <script type="text/javascript">
        if (top.location == self.location) top.location = "../default.aspx";
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            grid();
            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
           
            toolbar();


            //var tt = test(jsonObj.Rows, 3);
            //alert(JSON.stringify(tt));
        });

        function grid() {
            $("#maingrid4").ligerGrid({
                columns: [
                   //{ display: '序号', name: 'id', width: 50, render: function (item, i) { return item.n; } },
                    {
                        display: '客户', name: 'cus_name', width: 200, align: 'left', render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(1,'" + item.id + "')>";
                            if (item.cus_name)
                                html += item.cus_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    { display: '电话', name: 'cus_tel', width: 120, align: 'right' },
                    { display: '客户类型', name: 'cus_type_id', width: 80, render: function (item, i) { return item.cus_type } },
                    { display: '客户类别', name: 'cus_level_id', width: 80, render: function (item, i) { return item.cus_level } },
                    { display: '客户来源', name: 'cus_source_id', width: 80, render: function (item, i) { return item.cus_source } },
                    { display: '省份', name: 'Provinces_id', width: 80, render: function (item, i) { return item.Provinces } },
                    { display: '城市', name: 'City_id', width: 80, render: function (item, i) { return item.City } },
                    { display: '所属行业', name: 'cus_industry_id', width: 80, render: function (item, i) { return item.cus_industry } },
                    { display: '部门', name: 'emp_id', width: 80, render: function (item, i) { return item.department } },
                    { display: '员工', name: 'emp_id', width: 80, render: function (item, i) { return item.employee } },
                    {
                        display: '客源状态', name: 'isPrivate', width: 60, render: function (item, i) {
                            if (item.isPrivate == 1) return "公客";
                            else
                                return "私客";
                        }
                    },
                    {
                        display: '最后跟进', name: 'lastfollow', width: 90, render: function (item) {
                            var lastfollow = formatTimebytype(item.lastfollow, 'yyyy-MM-dd');
                            if (lastfollow == "1900-01-01")
                                lastfollow = "";
                            return lastfollow;
                        }
                    },
                    {
                        display: '删除时间',
                        name: 'Delete_time',
                        width: 90,
                        render: function (item) {
                            var Delete_time = formatTimebytype(item.Delete_time, 'yyyy-MM-dd');
                            return Delete_time;
                        }
                    },
                    {
                        display: '创建时间',
                        name: 'create_time',
                        width: 90,
                        render: function (item) {
                            var create_time = formatTimebytype(item.create_time, 'yyyy-MM-dd');
                            return create_time;
                        }
                    }
                ],
                onBeforeShowData: function (grid, data) {
                    startTime = new Date();
                },
                rowtype: "CustomerType",
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "CRM_Customer.grid.xhd?type=repeat&rnd=" + Math.random(),
                width: '100%',
                height: '100%',
                heightDiff: -10,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                },
                onAfterShowData: function (grid) {
                    $("tr[rowtype='已成交']").addClass("l-treeleve1").removeClass("l-grid-row-alt");
                    var nowTime = new Date();
                    //alert('加载数据耗时：' + (nowTime - startTime));
                },
                groupColumnName: 'cus_name',
                groupColumnDisplay: '客户'

            });

        }

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=tools_repeat&rnd=" + Math.random(), function (data, textStatus) {
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
                    width: 120,
                    items: getMenuItems(data)
                });
                $("#maingrid4").ligerGetGridManager()._onResize();
            });

        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("确定删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "CRM_Customer.del.xhd",
                            type: "POST",
                            data: { idlist: row.id, rnd: Math.random() },
                            dataType: "json",
                            success: function (result) {
                                $.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_reload();
                                }
                                else {
                                    $.ligerDialog.error(obj.Message);
                                }
                            },
                            error: function () {
                                $.ligerDialog.error('删除失败！');
                            }
                        });
                    }
                });
            } else {
                $.ligerDialog.warn("请选择数据");
            }
        }



        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }


        function help(parameters) {
            $.ligerDialog.question("此界面是永坤金行-CRM的客户管理界面，上面部分是客户列表，下面部分是跟进。点击客户，可以加载跟进信息。具体请访问官方教程：<br/><br/><a href='http://www.xhdcrm.com/khgl/8.html' target='_blank'>客户管理：客户列表教程</a>", "提示");
            //$.ligerDialog.open({ height: 200, url: 'http://wwww.xhdcrm.com', width: null });
        }
    </script>
    <style type="text/css">
        .l-treeleve1 { background: yellow; }

        .l-treeleve2 { background: yellow; }

        .l-treeleve3 { background: #eee; }
    </style>
</head>
<body>
    <form id="form1" onsubmit=" return false ">
        <div style="padding: 10px;">
            <div id="toolbar"></div>

            <div id="grid">
                <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            </div>
        </div>


    </form>

</body>
</html>
