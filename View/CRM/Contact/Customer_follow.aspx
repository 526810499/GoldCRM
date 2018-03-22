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
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            grid();
            toolbar();
            $("#btn_serch").ligerButton({ text: "搜索", width: 60, click: doserch });
            $("#btn_reset").ligerButton({ text: "重置", width: 60, click: doclear });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            $("#serchform").ligerForm();
        });

        function grid() {
            $("#maingrid4").ligerGrid({
                columns: [
                        //{ display: '序号', name: 'n', width: 50 },
                        {
                            display: '客户名', name: 'customer_id', width: 150, render: function (item) {
                                var html = "<a href='javascript:void(0)' onclick=view('customer','" + item.customer_id + "')>";
                                if (item.Customer_name)
                                    html += item.Customer_name;
                                html += "</a>";
                                return html;
                            }
                        },
                        {
                            display: '联系人', name: 'contact_id', width: 100, render: function (item) {
                                var html = "<a href='javascript:void(0)' onclick=view('contact','" + item.contact_id + "')>";
                                if (item.contact_name)
                                    html += item.contact_name;
                                html += "</a>";
                                return html;
                            }
                        },
                        {
                            display: '跟进内容', name: 'follow_content', align: 'left', width: 300, render: function (item) {
                                var html = "<div class='abc'><a href='javascript:void(0)' onclick=view('follow','" + item.id + "')>";
                                if (item.follow_content)
                                    html += item.follow_content;
                                html += "</a></div>";
                                return html;
                            }
                        },
                         { display: '跟进目的', name: 'Follow_aim_id', width: 80, render: function (item, i) { return item.Follow_aim; } },
                        {
                            display: '跟进时间', name: 'follow_time', width: 140, render: function (item) {
                                return formatTimebytype(item.follow_time, 'yyyy-MM-dd hh:mm');
                            }
                        },
                        { display: '跟进方式', name: 'Follow_Type_id', width: 80, render: function (item, i) { return item.Follow_Type; } },
                        { display: '跟进部门', name: 'Employee_id', width: 80, render: function (item, i) { return item.Department; } },
                        { display: '跟进人', name: 'Employee_id', width: 80, render: function (item, i) { return item.Employee; } }
                ],
                onAfterShowData: function (grid) {
                    $(".abc").hover(function (e) {
                        $(this).ligerTip({ content: $(this).text(), width: 300, distanceX: event.clientX - $(this).offset().left - $(this).width() + 15 });
                    }, function (e) {
                        $(this).ligerHideTip(e);
                    });
                },
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "CRM_follow.grid.xhd",
                width: '100%', height: '100%',
                //title: "跟进信息",
                heightDiff: -10,
                onRClickToSelect: true
            });
        }

        function toolbar() {
            $.get("toolbar.GetSys.xhd?mid=contact_follow&rnd=" + Math.random(), function (result, textStatus) {
                var data = eval('(' + result + ')');;
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({
                    type: 'serchbtn',
                    text: '高级搜索',
                    icon: '../../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });
                
                $("#toolbar").ligerToolBar({
                    items: items
                });

                manager = $("#maingrid4").ligerGetGridManager();
                manager._onResize();
            });
        }

        function initSerchForm() {
            var c = $("#followtype").ligerComboBox({ width: 194, url: "Sys_Param.combo.xhd?type=follow_type&rnd=" + Math.random() });
            var e = $('#employee').ligerComboBox({ width: 96, emptyText: '（空）' });
            var f = $('#department').ligerComboBox({
                width: 98,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: 'hr_department.tree.xhd?rnd=' + Math.random(),
                    idFieldName: 'id',
                    //parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue) {
                    $.get("hr_employee.combo.xhd?did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        e.setData(eval(data));
                    });
                }
            });
        }
        function serchpanel() {
            initSerchForm();
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                manager._onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                manager._onResize();
            }
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });
        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);

            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("CRM_follow.grid.xhd?" + serchtxt);

        }
        function doclear() {
            $("input:hidden", "#serchform").val("");
            $("input:text", "#serchform").val("");
            $(".l-selected").removeClass("l-selected");
            $("#T_smart").focus();
        }

        function add() {

            f_openWindow("CRM/contact/Customer_follow_add.aspx", "新增跟进", 630, 400, f_save);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('CRM/contact/Customer_follow_add.aspx?id=' + row.id, "修改跟进", 630, 400, f_save);
            } else {
                $.ligerDialog.warn('请选择跟进！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("跟进删除无法恢复，确定删除？", function (yes) {
                    if (yes) {
                        $.ligerDialog.waitting('数据删除中,请稍候...');
                        $.ajax({
                            url: "CRM_follow.del.xhd",
                            type: "POST",
                            data: { id: row.id, rnd: Math.random() },
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
                                $.ligerDialog.closeWaitting();
                                $.ligerDialog.error('删除失败！');
                            }
                        });
                    }
                });
            } else {
                $.ligerDialog.warn("请选择跟进");
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "CRM_follow.save.xhd",
                    type: "POST",
                    data: issave,
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
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('操作失败！');
                    }
                });

            }
        }

        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }



    </script>
</head>
<body>

    <form id="form1" onsubmit="return false">
        <div style="padding: 10px;">
            <div id="toolbar"></div>

            <div id="grid">
                <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            </div>
        </div>
    </form>

    <div class="az">
        <form id='serchform'>
            <table style='width: 780px' class="aztable">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>客户名称：</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:197}' />
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>跟进员工：</div>
                    </td>
                    <td>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='department' name='department' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='employee' name='employee' />
                        </div>
                    </td>
                    <td>
                        <input type='text' id='T_smart' name='T_smart' ltype='text' ligerui='{width:160,nullText:"输入关键词智能搜索跟进内容"}' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>跟进时间：</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>跟进方式：</div>

                    </td>
                    <td>
                        <div style='width: 196px; float: left'>
                            <input type='text' id='followtype' name='followtype' />
                        </div>

                    </td>
                    <td>
                        <div id="btn_serch"></div>
                        <div id="btn_reset"></div>
                    </td>
                </tr>

            </table>
        </form>
    </div>
</body>
</html>
