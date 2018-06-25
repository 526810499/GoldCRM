﻿<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<%--    <title>永坤金行-CRM</title>--%>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" />
    <link rel="shortcut icon" type="image/x-icon" href="images/logo/favicon.ico" />
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="lib/ligerUI/skins/Silvery/css/all.css" rel="stylesheet" />
    <link href="CSS/index/main.css?v=2" rel="stylesheet" />

    <script src="lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="JS/jquery.jclock.js" type="text/javascript"></script>
    <script src="JS/XHD.js" type="text/javascript"></script>
    <style>
        .scroll_div {
            width: 400px;
            height: 44px;
            line-height: 44px;
            margin-left: 200px;
            margin-top: 5px;
            overflow: hidden;
            white-space: nowrap;
        }

        .bli {
            font-family: arial, sans-serif;
            color: #ffc600;
            font-size: 18px;
            text-transform: uppercase;
            letter-spacing: 0.8pt;
            word-spacing: 0pt;
            line-height: 1.1;
        }

        #scroll_begin, #scroll_end, #scroll_begin ul, #scroll_end ul, #scroll_begin ul li, #scroll_end ul li {
            display: inline;
        }
    </style>
</head>
<body>


    <div id="header"  style="display: none">


        <div class="logoContent">
            <img src="css/index/logo.png" width="150" style="float: left; margin-top: 4px;" />
            <marquee direction="left" behavior="scroll" scrollamount="5" scrolldelay="100" scrolldelay="0" width="600" height="50" style="margin-left: 500px;">
             <div id="scroll_begin"  ></div> </marquee>
        </div>
        <div class="navright">


            <div id="userinfo" class="item">

                <div style="position: relative; float: left;">
                    <div id="Username" style="font-size: 14px; padding-right: 5px; width: 60px; text-align: right; float: left; height: 50px; line-height: 50px;"></div>
                </div>
                <div id="portrait">
                    <img id="userheader" width="40" style="border-radius: 100%;" />
                </div>
            </div>


        </div>
    </div>
    <div id="layout" style="width: 100%; margin-top: 4px;">

        <div position="left" title="功能菜单" id="accordion1">
            <%--<ul class="nav" id="mainmenu">
              
            </ul>--%>
        </div>
        <div position="center" id="framecenter">
            <div tabid="home" title="桌面" style="height: 300px">
                <iframe frameborder="0" name="home" id="home"></iframe>
            </div>
        </div>


        <div position="bottom">

            <div style="text-align: center; font-size: 12px;">©2015 <a href="#" target="_blank">永坤金行-CRM</a> 版权所有 v2.0</div>

        </div>
    </div>


    <script type="text/javascript">
        var tab;
        var accordion;
        function onResize() {
            var winH = $(window).height(), winW = $(window).width();
            $("#pageloading").height(winH);
            initLayout();
        }

        function checkbrowse() {
            if ($.browser.msie) {
                var ver = $.browser.version;

                if (ver == "6.0" || ver == "7.0" || ver == "8.0" || ver == "9.0") {
                    $.ligerDialog.warn("检测到您的浏览器版本较低，为了使系统得到最佳体验效果，建议您使用高级浏览器。")
                }
            }
        }
        var mwaitDialog;
        $(function () {
            mwaitDialog = $.ligerDialog.waitting("系统加载中...");
            //$("#home").attr("src", "home/home.aspx");
            f_user();

            //布局
            $("#layout").ligerLayout({ leftWidth: 190, rightWidth: 190, bottomHeight: 25, space: 4, allowBottomResize: false, allowLeftResize: false, allowRightResize: false, height: '100%', onHeightChanged: f_heightChanged, isRightCollapse: true });

            var height = $(".l-layout-center").height();

            //Tab
            tab = $("#framecenter").ligerTab({
                height: height,
                dblClickToClose: true,
                showSwitch: true,       //显示切换窗口按钮
                showSwitchInTab: true //切换窗口按钮显示在最后一项 ,


            });


            $("#userinfo").click(function (e) { f_dropdown(e) });
            onResize();
            $(window).resize(function () {
                onResize();
            });
            accordion = $("#accordion1").ligerAccordion({ height: height - 32 });
            menu();

            checkbrowse();
            setTimeout("getuserinfo()", 3000);
        });


        function usertree() {
            menu = $.ligerMenu({
                top: 100, left: 100, width: 120,
                items:
                [
                    { text: '刷新', click: flushtree, img: 'images/icon/97.png' }
                ]
            });

            $("#tree1").ligerTree({
                url: 'Sys_base.getUserTree.xhd?rnd=' + Math.random(),
                idFieldName: 'id',
                iconpath: 'images/icon/',
                usericon: 'd_icon',
                checkbox: false,
                itemopen: false,
                onError: function () { javascript: location.replace("login.aspx"); },
                onContextmenu: function (node, e) {
                    actionNodeID = node.data.text;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
        }


        function flushtree() {
            treemanager = $("#tree1").ligerGetTreeManager();
            treemanager.reload();

        }

        function f_heightChanged(options) {
            if (tab)
                tab.addHeight(options.diff);
            if (accordion && options.middleHeight - 32 > 0)
                accordion.setHeight(options.middleHeight - 32);

        }


        function f_user() {
            $("#userinfo").hover(
                function () {
                    $(this).addClass("userover");
                },
                function () {
                    $(this).removeClass("userover");
                }
            )

        }

        function menu() {
            var mainmenu = $("#accordion1");
            $.getJSON("Sys_base.GetAllMenus.xhd?rnd=" + Math.random(), function (data, textStatus) {
                $(data).each(function (i, app) {
                    var appmenu = $("<div title='" + app.text + "'><ul class='sidebar-menu'></ul></div>");

                    $(app.children).each(function (gi, group) //包括分组的部分
                    {
                        if (group.children) {
                            var groupmenu = $("<li class='sub-menu'><a ><img /> " + group.Menu_name + "<span class='arrow'></span></a><ul class='sub'></ul><li>");
                            groupmenu.find("img").attr("src", group.Menu_icon);

                            $(group.children).each(function (i, submenu) {
                                var subitem = $('<li><a class="menulink" ><img /> ' + submenu.Menu_name + '</a></li>');
                                subitem.find("img").attr("src", submenu.Menu_icon);
                                subitem.find("a").attr({
                                    tabid: submenu.Menu_id,
                                    tabtext: submenu.Menu_name,
                                    taburl: submenu.Menu_url
                                });
                                $("ul:first", groupmenu).append(subitem);
                            });
                            $("ul:first", appmenu).append(groupmenu);
                        }
                        else {
                            var subitem = $('<li><a class="menulink" ><img />' + group.Menu_name + '</a></li>');
                            subitem.find("img").attr("src", group.Menu_icon);
                            subitem.find("a").attr({
                                tabid: group.Menu_id,
                                tabtext: group.Menu_name,
                                taburl: group.Menu_url
                            });
                            $("ul:first", appmenu).append(subitem);
                        }

                    });
                    $(mainmenu).append(appmenu);
                })

                accordion._render();
                onResize();

                $('.sub-menu > a').click(function () {
                    var last = $('.sub-menu.open');
                    last.removeClass("open");
                    $('.sub').slideUp(200);

                    var sub = jQuery(this).next();
                    if (sub.is(":visible")) {
                        $(this).parent().removeClass("open");
                        sub.slideUp(200);
                    } else {
                        $(this).parent().addClass("open");
                        sub.slideDown(200);
                    }
                });

                mainmenu.find("a.menulink").click(function () {
                    var tabid = $(this).attr('tabid'),
                       url = $(this).attr("taburl"),
                       text = $(this).attr('tabtext');

                    if (!url) return;

                    f_addTab(tabid, text, url);
                });

                //$(".l-accordion-header").css({"border-top":"1px solid #fff","line-height":"29px"})
                $("#home").attr("src", "home/home.aspx");
                //$("#pageloading").fadeOut(800);

                mwaitDialog.close();
            });
        }



        function f_dropdown(e) {
            var sysitem = [];
            var windowsswitch;
            if ($(".l-userinfo-panel").length == 0) {
                windowsswitch = $("<div class='l-userinfo-panel'><ul class='userinfolist'></ul></div>").appendTo($("#userinfo"));
                sysitem.push({ icon: 'images/icon/37.png', title: "个人设置", click: function () { personalinfoupdate(); } });
                sysitem.push({ icon: 'images/icon/77.png', title: "修改密码", click: function () { changepwd(); } });
                sysitem.push({ icon: 'images/icon/1.png', title: "退出系统", click: function () { logout(); } });

                //if ($(".l-userinfo-panel").length)
                //    var windowsswitch = $("<div class='l-userinfo-panel'><ul class='userinfolist'></ul></div>").appendTo($("#userinfo"));
                $(sysitem).each(function (i, item) {
                    var subitem = $('<li><img/><span></span></li>');

                    $("img", subitem).attr("src", item.icon);
                    $("span", subitem).html(item.title);
                    $("ul:first", windowsswitch).append(subitem);
                    //windowsswitch.append(subitem);
                    subitem.click(function () { item.click(item); });
                })
            }
            else
                windowsswitch = $(".l-userinfo-panel");

            $("li", windowsswitch).live('click', function () {
                $(".l-userinfo-panel").hide();
            }).live('mouseover', function () {
                var jitem = $(this);
                jitem.addClass("over");
            }).live('mouseout', function () {
                var jitem = $(this);
                jitem.removeClass("over");
            });
            windowsswitch.css({
                top: $("#userinfo").offset().top + $("#userinfo").height() + 10,
                //left:$("#userinfo").offset().left,
                width: $("#userinfo").width()
            });

            if ($(".l-userinfo-panel").css('display') == 'none')
                $(".l-userinfo-panel").show();
            else
                $(".l-userinfo-panel").hide();

            $(document).one("click", function () {
                $(".l-userinfo-panel").hide();
            });

            e.stopPropagation();
        }

        function getuserinfo() {
            $.getJSON("Sys_base.GetUserInfo.xhd?rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                $("#Username").html("<div style='cursor:pointer'>" + data.name + "</div>");
                if (data.title) {
                    $("#userheader").attr("src", "file/header/" + data.title);
                }
                else {
                    $("#userheader").attr("src", "/images/noheadimage.jpg");
                }
            });
            GetTodayBroadcast();
        }



        function getUser() {
            $.ajax({
                type: 'post',
                dataType: 'json',
                url: 'Sys_base.getUserTree.xhd',
                data: { rnd: Math.random() },
                success: function (result) {

                },
                error: function () {
                    javascript: location.replace("login.aspx");
                }

            });
        }
        function logout() {
            $.ligerDialog.confirm('您确认要退出系统？', function (yes) {
                if (yes) {
                    $.ajax({
                        type: 'post',
                        //dataType: 'json',
                        url: 'login.logout.xhd',
                        //data: { type: "login", method: 'logout' },
                        success: function (result) {
                            javascript: location.replace("login.aspx");
                        },
                        error: function ()
                        { alert() }

                    });
                }
            });
        }
        function changepwd() {
            var dialog = $.ligerDialog.open({
                url: "hr/hr_changepwd.aspx", width: 480, height: 250, title: "修改密码", buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                dialog.frame.f_save();
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            });
        }
        function personalinfoupdate() {
            var dialog = $.ligerDialog.open({
                url: "hr/emp_personal_update.aspx", width: 760, height: 300, title: "个人信息", buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                dialog.frame.f_save();
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            });
        }

        function getsysinfo() {
            $.ajax({
                type: "GET",
                url: "Sys_info.grid.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var rows = obj.Rows;

                    var sysinfo = {};
                    for (var i = 0; i < rows.length; i++) {
                        if (rows[i].sys_value == "null" || rows[i].sys_value == null) {
                            rows[i].sys_value = " ";
                        }
                        sysinfo[rows[i].sys_key] = rows[i].sys_value;
                    }

                    document.title = sysinfo["sys_name"] + "-永坤金行-CRM";
                    $("#logo").attr("src", sysinfo["sys_logo"]);
                }
            });
        }


        function GetTodayBroadcast() {
            $.getJSON("STodayBroadcast.GetTodayBroadcast.xhd?rnd=" + Math.random(), function (data, textStatus) {
                if (data != null) {
                    var temp = "<li class='bli'>当前单价:<b style='color:red;padding-left:5px'>" + toMoney(data.TodayGlodPrice) + "</b></li><li class='bli'>&nbsp;&nbsp;" + data.OtherBrodcast + "</li>";
                    var c = temp;
                    c += "<li>&nbsp;&nbsp;</li>";
                    c += "<li>&nbsp;&nbsp;</li>";
                    c += temp;
                    c += "<li>&nbsp;&nbsp;</li>";
                    c += "<li>&nbsp;&nbsp;</li>";
                    c += temp;

                    $("#scroll_begin").html("<ul>" + c + "</ul>");
                    // ScrollImgLeft();
                }
            });
        }

        function ScrollImgLeft() {
            var speed = 20
            var scroll_begin = document.getElementById("scroll_begin");
            var scroll_end = document.getElementById("scroll_end");
            var scroll_div = document.getElementById("scroll_div");
            scroll_end.innerHTML = scroll_begin.innerHTML
            function Marquee() {
                if (scroll_end.offsetWidth - scroll_div.scrollLeft <= 0) {
                    scroll_div.scrollLeft -= scroll_begin.offsetWidth;
                }
                else {
                    scroll_div.scrollLeft++;
                }
            }
            var MyMar = setInterval(Marquee, speed);
            scroll_div.onmouseover = function () { clearInterval(MyMar); }
            scroll_div.onmouseout = function () { MyMar = setInterval(Marquee, speed); }
        }

    </script>

</body>
</html>
