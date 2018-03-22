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
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    {
                        display: '公告标题', name: 'notice_title', width: 460, align: 'left', render: function (item) {
                            return "<a href='javascript:void(0)' onclick=view('" + item.id + "')>" + item.notice_title + "</a>";
                        }
                    },
                    { display: '发布部门', name: 'dep_name', width: 100 },
                    { display: '发布人', name: 'create_name', width: 100 },

                    {
                        display: '发布时间', name: 'create_time', width: 90, render: function (item) {
                            return formatTimebytype(item.create_time, 'yyyy-MM-dd');
                        }
                    },
                    {
                        display: '查看', render: function (item) {
                            return "<a href='javascript:void(0)' onclick=view('" + item.id + "')>查看</a>";
                        }
                    }

                ],
                //fixedCellHeight:false,
                rownumbers:true,
                onSelectRow: function (data, rowindex, rowobj) {
                    $("#framecenter").html(myHTMLDeCode(data.notice_content));
                },
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "public_notice.grid.xhd?rnd=" + Math.random(),
                //title:'公告列表',
                width: '100%', height: '100%',
                heightDiff: -10,
                onRClickToSelect: true,

                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            toolbar();
        });

        function toolbar() {
            $.ajax({
                url: "toolbar.GetSys.xhd",
                type: "POST",
                data: { mid: 'message_notice', rnd: Math.random() },
                //dataType: "json",
                success: function (result) {

                    var data = eval('(' + result + ')');;

                    var items = [];
                    var arr = data.Items;
                    for (var i = 0; i < arr.length; i++) {
                        arr[i].icon = "../../" + arr[i].icon;
                        items.push(arr[i]);
                    }

                    items.push({ type: 'textbox', id: 'sstart', text: '时间：' });
                    items.push({ type: 'textbox', id: 'sdend', text: '' });
                    items.push({ type: 'textbox', id: 'stext', text: '标题：' });
                    items.push({ type: 'button', text: '搜索', icon: '../../images/search.gif', disable: true, click: function () { doserch() } });
                    items.push({ type: 'button', text: '重置', icon: '../../images/edit.gif', disable: true, click: function () { doclear() } });

                    $("#toolbar").ligerToolBar({
                        items: items

                    });
                    menu = $.ligerMenu({
                        width: 120, items: getMenuItems(data)
                    });

                    $("#maingrid4").ligerGetGridManager()._onResize();
                    $("#sstart").ligerDateEditor({ width: 100 })
                    $("#sdend").ligerDateEditor({ width: 100 })
                    $("#stext").ligerTextBox({ width: 200, nullText: "输入关键词搜索" })
                },
                error: function () {
                    $.ligerDialog.error('！');
                }
            });
           
        }

        function doserch() {
            var sendtxt = "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("public_notice.grid.xhd?" + serchtxt);
        }
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#form1").each(function () {
                this.reset();
            });
        }

        //查看
        function view(title_id) {
            var dialogOptions = {
                width: 770, height: 510, title: "查看公告", url: 'personal/message/notice_view.aspx?nid=' + title_id, buttons: [
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }

        function add() {
            f_openWindow("personal/message/notice_add.aspx", "新增公告", 770, 550,f_save);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('personal/message/notice_add.aspx?nid=' + row.id, "修改公告", 770, 550,f_save);
            }
            else {
                $.ligerDialog.warn('请选择行！');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("删除后不能恢复，\n您确定要删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "public_notice.del.xhd", type: "POST",
                            data: { id: row.id, rnd: Math.random() },
                            dataType:'json',
                            success: function (result) {
                                top.$.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    f_reload();
                                }
                                else {
                                    top.$.ligerDialog.error(obj.Message);
                                }

                            },
                            error: function () {
                                top.$.ligerDialog.error('删除失败！');
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn("请选择数据");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "public_notice.save.xhd", type: "POST",
                    data: issave,
                    dataType: 'json',
                    success: function (result) {
                        top.$.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            f_reload();
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

        function f_reload() {
            $("#maingrid4").ligerGetGridManager().loadData(true);
        };


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

</body>
</html>
