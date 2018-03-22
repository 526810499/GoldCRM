<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <%--    <link href="../CSS/input.css" rel="stylesheet" />--%>
    <link href="../CSS/index/common.css" rel="stylesheet" />
    <link href="../CSS/index/index.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"> </script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js"></script>
    <script src="../JS/XHD.js" type="text/javascript"> </script>
    <script src="../JS/echarts-all.js" type="text/javascript"> </script>

    <script type="text/javascript">
        $(function () {
            f_menu();
            // f_message();
            remind();
            getUser();
            setInterval(remind, 30000);
            portal();

            //$("#a").ligerButton({ text: "客户管理", width: 80, click: notice, icon: "../images/icon/61.png" })
            //$("#b").ligerButton({ text: "跟进管理", width: 80, click: notice, icon: "../images/icon/3.png" })
            //$("#c").ligerButton({ text: "日程管理", width: 80, click: notice, icon: "../images/icon/29.png" })
            //$("#btn-set").ligerButton({ text: "设置", width: 60, click: queckmenu, icon: "../images/icon/77.png" })

            $("#queckmenu").ligerToolBar({
                items: [{
                    type: 'button',
                    text: '设置菜单',
                    icon: '../images/icon/77.png',
                    disable: true,
                    click: queckmenu
                }]
            });

            f_notice();
            f_news();
            f_note();
            f_calendar();
            f_todo();
        });

        function remind() {
            var now = new Date(), hour = now.getHours();
            if (hour > 4 && hour < 6) { $("#greetings").text("凌晨好！") }
            else if (hour < 9) { $("#greetings").text("早上好！") }
            else if (hour < 12) { $("#greetings").text("上午好！") }
            else if (hour < 14) { $("#greetings").text("中午好！") }
            else if (hour < 17) { $("#greetings").text("下午好！") }
            else if (hour < 19) { $("#greetings").text("傍晚好！") }
            else if (hour < 22) { $("#greetings").text("晚上好！") }
            else { $("#greetings").text("夜深了！") }
        }
        function getUser() {
            $.getJSON("Sys_base.GetUserInfo.xhd?rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                $("#username").text("【" + data.name + "】");

            });
        }

        function queckmenu() {
            f_openWindow("home/queckmenu.aspx", "快捷菜单", 600, 400, f_menu_set);
        }

        function f_menu_set(item, dialog) {
            var issave = dialog.frame.f_save();

            if (!issave) return;

            //alert(JSON.stringify(issave)); return;

            //if (!issave.length) return;

            var count = 0;
            $(issave).each(function (i, item) {
                //alert(this.__status);
                if (this.__status != 'delete')
                    count++;
            })
            if (count > 10) {
                var warn = "快捷方式不能超过10个，当前您选择的快捷方式有" + count + "个，请去掉您不需要的快捷方式！";
                top.$.ligerDialog.warn(warn, "警告", "", 9003);

                return;
            }

            dialog.close();
            $.ligerDialog.waitting('数据保存中,请稍候...');
            $.ajax({
                url: "Personal_queckmenu.set.xhd", type: "POST",
                data: { PostData: JSON.stringify(issave) },
                success: function (responseText) {
                    $.ligerDialog.closeWaitting();
                    f_menu();
                },
                error: function () {
                    $.ligerDialog.closeWaitting();
                    $.ligerDialog.error('操作失败！');
                }
            });

        }

        function f_menu() {
            $.ligerDialog.waitting('数据加载中,请稍候...');
            $.ajax({
                url: "Personal_queckmenu.get.xhd", type: "POST",
                dataType: 'json',
                success: function (result) {
                    var rows = result.Rows;

                    $.ligerDialog.closeWaitting();
                    var toolbarmanager = $("#queckmenu").ligerGetToolBarManager();
                    toolbarmanager.removeAll();

                    $(rows).each(function (i, item) {
                        toolbarmanager.addItem({
                            type: 'button',
                            id: item.menu_id,
                            text: item.Menu_name,
                            icon: '../' + item.Menu_icon,
                            disable: true,
                            click: function () {
                                f_tab(item.menu_id, item.Menu_name, item.Menu_url);
                            }
                        });
                    })
                },
                error: function () {
                    $.ligerDialog.closeWaitting();
                    $.ligerDialog.error('操作失败！');
                },
                complete: function () {
                    top.$.ligerDialog.closeWaitting();
                }
            });
        }

        function f_tab(id, text, url) {
            top.f_addTab(id, text, url);
        }


        function portal() {
            manager = $("#mainpanel").ligerPortal({
                draggable: false,
                rows: [
                    {
                        columns: [
                            {
                                width: '33%',
                                panels: [
                                    {
                                        title: '任务',
                                        width: '99%',
                                        height: 150,
                                        content_id: 'c_task'
                                    }
                                ]
                            },
                            {
                                width: '33%',
                                panels: [
                                    {
                                        title: '跟进',
                                        width: '99%',
                                        height: 150,
                                        content_id: 'c_follow'
                                    }
                                ]
                            },
                            {
                                width: '33%',
                                panels: [
                                    {
                                        title: '新闻',
                                        width: '99%',
                                        height: 150,
                                        content_id: 'c_news'
                                    }
                                ]
                            }
                        ]
                    }, {
                        columns: [
                            {
                                width: '33%',
                                panels: [{
                                    title: '便签',
                                    width: '99%',
                                    height: 150,
                                    content_id: 'c_note'
                                }
                                ]
                            },
                            {
                                width: '33%',
                                panels: [
                                    {
                                        title: '日程',
                                        width: '99%',
                                        height: 150,
                                        content_id: 'c_calendar'
                                    }
                                ]
                            },
                            {
                                width: '33%',
                                panels: [
                                    {
                                        title: '公告',
                                        width: '99%',
                                        height: 150,
                                        content_id: 'c_notice'
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });
        }

        function f_notice() { //公告提醒
            $.getJSON("public_notice.noticeremind.xhd?rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>");
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/18.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('message_notice','公告','Personal/Message/notice.aspx')\">" + obj.Rows[i].notice_title + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].notice_time, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_notice"));
            });
        }
        function f_news() { //新闻
            $.getJSON("public_news.newsremind.xhd?rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>");
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/10.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('message_news','新闻','personal/message/news.aspx')\">" + obj.Rows[i].news_title + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].news_time, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_news"));
            });

        }
        function f_note() { //便签
            $.getJSON("Personal_notes.notesremind.xhd?rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>");
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/35.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('mywork_note','我的便签','personal/personal/notes.aspx')\">" + obj.Rows[i].note_content
                        + "</a></div></td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_note"));
            });

        }
        function f_calendar() {
            //日程安排提醒
            $.getJSON("Personal_Calendar.Today.xhd?rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>");
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/31.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('mywork_calendar','日程安排','personal/personal/Calendar.aspx')\">" + obj.Rows[i].Subject + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].StartTime, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_calendar"));
            });
        }
        function f_todo() { //任务
            $.getJSON("Task.TaskRemind.xhd?rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>");
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/49.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('task_todo','待办任务','personal/Task/todo.aspx')\">" + obj.Rows[i].task_title + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].executive_time, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_task"));
            });
        }
    </script>
    <style type="text/css">
        .l-panel { box-shadow: 0px 2px 5px rgba(0,0,0,0.2); }
    </style>
</head>
<body style="overflow-y: scroll;">

    <div style="padding: 10px;">
        <div class="welcome">
            <strong><span id="greetings"></span><span id="username" style="font-size: 14px; font-weight: bolder;"></span></strong>
        </div>

        <fieldset class="l-group">
            <legend>快捷菜单</legend>
            <div id="queckmenu">
                <div id="a"></div>
                <div id="b"></div>
                <div id="c"></div>
                <div id="btn-set"></div>
            </div>
        </fieldset>

        <div style="margin-top: 10px; padding: 10px 0 0 10px; border: 1px solid #d0d0d0;" id="mainpanel">
        </div>
    </div>

    <%--<input type="button" value="test" onclick="test()" />--%>
</body>
</html>
