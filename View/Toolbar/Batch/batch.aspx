<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />

    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    //{
                    //    display: '操作类型', name: 'C_name', width: 100, render: function (item) {
                    //        switch (item.batch_type)
                    //        {
                    //            case "customer": return "批量转客户"; break;
                    //            default: return "未知类型"; break;
                    //        }
                    //    }
                    //},
                     {
                         display: '转出人', width: 200, render: function (item, i) {
                             return "【" + item.o_dep_name + "】" + item.o_emp_name;
                         }
                     },
                     {
                         display: '→', width: 50, render: function (item, i) {
                             return "→";
                         }
                     },
                      {
                          display: '转入人', width: 200, render: function (item, i) {
                              return "【" + item.c_dep_name + "】" + item.c_emp_name;
                          }
                      },
                    { display: '数量', name: 'b_count', width: 50 },
                    { display: '操作人', name: 'create_name', width: 100 },
                    {
                        display: '操作时间', name: 'create_time', width: 180, render: function (item) {
                            return formatTimebytype(item.create_time, 'yyyy-MM-dd hh:mm:ss');
                        }
                    }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "Tool_batch.grid.xhd",
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -10,

                onRClickToSelect: true,
                detail: {
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        $(p).append(
                            "<table class='bodytable0'  style='width:99%;margin:5px'>" +                                
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>条件：</td><td height='23'  >" + r.batch_filter + "</td>" +
                                "</tr>" +
                                
                            "</table>");
                    }
                }

            });


            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

           
            toolbar();

        });

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=tools_batch&rnd=" + Math.random(), function (data, textStatus) {
                var data = eval('(' + data + ')');;
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

                manager = $("#maingrid4").ligerGetGridManager();
                manager._onResize();
            });
        }

        function batch_cus() {
            f_openWindow("toolbar/batch/batch_add.aspx?type=customer", "批量转客源", 720, 400, f_save);
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            var check = dialog.frame.f_check();

            if (check == true) {
                if (issave) {
                    dialog.close();
                    $.ligerDialog.confirm("批量转出后无法恢复，确定操作？", function (yes) {
                        if (yes) {
                            $.ligerDialog.waitting('数据保存中,请稍候...');
                            $.ajax({
                                url: "Tool_batch.save.xhd", type: "POST",
                                data: issave,
                                success: function (responseText) {
                                    $.ligerDialog.closeWaitting();
                                    f_reload();
                                },
                                error: function () {
                                    $.ligerDialog.closeWaitting();
                                    $.ligerDialog.error('操作失败！');
                                }
                            });
                        }
                    })
                }
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
        <div style="padding: 10px">
            <div id="toolbar"></div>

            <div id="grid">
                <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            </div>
        </div>

    </form>
</body>
</html>
