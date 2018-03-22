<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <style type="text/css">
        body, html { width: 100%; height: 100%; margin: 0; font-family: "微软雅黑"; font-family: "微软雅黑"; }
        #allmap { width: 100%; height: 100%; }
        p { margin-left: 5px; font-size: 14px; }
    </style>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=qBF1ENAhEgKANMrT9gikGXa9"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/library/SearchInfoWindow/1.5/src/SearchInfoWindow_min.js"></script>
    <link rel="stylesheet" href="http://api.map.baidu.com/library/SearchInfoWindow/1.5/src/SearchInfoWindow_min.css" />
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <title>地图单击事件</title>
</head>
<body style="">
    <div style="margin-top:10px;">
    <div id="toolbar" style="border-bottom:1px solid #ddd;"></div></div>
    <div id="allmap">

    </div>
</body>
</html>
<script type="text/javascript">
    $(function () {
        toolbar();
        resize();
        $(window).resize(function () {
            resize();
        });
        getPoint();
        getCity();
    })
    // 百度地图API功能
    var map = new BMap.Map("allmap");
    var point = new BMap.Point(112.991158, 28.191666);
    map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
    map.centerAndZoom(point, 15);

    // 添加带有定位的导航控件
    var navigationControl = new BMap.NavigationControl({
        // 靠左上角位置
        anchor: BMAP_ANCHOR_TOP_LEFT,
        // LARGE类型
        type: BMAP_NAVIGATION_CONTROL_LARGE
        // 启用显示定位
        //enableGeolocation: true
    });
    map.addControl(navigationControl);

    var mapType1 = new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP] });

    var overView = new BMap.OverviewMapControl();
    var overViewOpen = new BMap.OverviewMapControl({ isOpen: true, anchor: BMAP_ANCHOR_BOTTOM_RIGHT });
    //添加地图类型和缩略图
    //map.addControl(mapType1);          //2D图，卫星图
    map.addControl(overView);          //添加默认缩略地图控件
    map.addControl(overViewOpen);      //右下角，打开

    var size = new BMap.Size(10, 20);

    map.addControl(new BMap.CityListControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        offset: size,
        // 切换城市之间事件
        // onChangeBefore: function(){
        //    alert('before');
        // },
        // 切换城市之后事件
        // onChangeAfter:function(){
        //   alert('after');
        // }
    }));


    //    var label = new BMap.Label("{xx}软件集团"+i, { offset: new BMap.Size(20, -10) });
    //    marker.setLabel(label);
    //}
    //var city = new BMap.LocalSearch(map, {
    //    renderOptions: {
    //        map: map,
    //        autoViewport: true
    //    }

    //});

    function search(value) {
        //city.search(value);
        //setTimeout(function () {
        //    map.setZoom(14);
        //}, 1000)
        map.centerAndZoom(value, 12);
    }

    //工具条实例化
    function toolbar() {

        var items = [];
        items.push({ type: 'textbox', id: 'T_Provinces', text: '城市：' });
        items.push({ type: 'textbox', id: 'T_City' });
        items.push({ type: 'button', text: '设为默认', icon: '../../images/icon/67.png', disable: true, click: function () { defaultCity() } });

        $("#toolbar").ligerToolBar({
            items: items
        });

        b = $('#T_City').ligerComboBox({ width: 96, emptyText: '（空）', onSelected: function (newvalue, newtext) { search(newtext); } });
        c = $('#T_Provinces').ligerComboBox({
            width: 97,
            url: "Sys_Provinces.combo.xhd?rnd=" + Math.random(),
            onSelected: function (newvalue, newtext) {
                if (!newvalue)
                    newvalue = -1;
                $.get("Sys_City.combo.xhd?provincesid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                    b.setData(eval(data));
                });
                search(newtext);
            }, emptyText: '（空）'
        });


    }
    function resize() {
        var h = document.documentElement.clientHeight;
        $("#allmap").height(h - 47);
    }

    function defaultCity() {
        var city = $("#T_City").val() || $("#T_Provinces").val() || "";
        if (!city) {
            $.ligerDialog.warn('请选择城市再设置！');
            return;
        }

        $.ajax({
            url: "hr_employee.updateDefaultCity.xhd",
            type: "POST",
            data: { city: city },
            success: function (responseText) {
                $.ligerDialog.success('设置成功！');
            },
            error: function () {
                $.ligerDialog.error('操作失败！');
            }
        });

    }
    function getPoint() {
        $.ajax({
            url: "CRM_Customer.getMapList.xhd?type=map",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var rows = data.Rows;
                var arr = new Array();
                var point = new Array(); //存放标注点经纬信息的数组
                var marker = new Array(); //存放标注点对象的数组
                var info = new Array(); //存放提示信息窗口对象的数组
                var searchInfoWindow = new Array();//存放检索信息窗口对象的数组
                for (var i = 0; i < rows.length; i++) {
                    arr = rows[i].xy.split(",")

                    if (arr.length != 2) {
                        continue;
                    }

                    var p0 = arr[0]; //
                    var p1 = arr[1]; //按照原数组的point格式将地图点坐标的经纬度分别提出来
                    point[i] = new window.BMap.Point(p0, p1); //循环生成新的地图点
                    marker[i] = new window.BMap.Marker(point[i]); //按照地图点坐标生成标记
                    map.addOverlay(marker[i]);
                    //marker[i].setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
                    //显示marker的title，marker多的话可以注释掉
                    //var label = new window.BMap.Label(rows[i].Customer, { offset: new window.BMap.Size(20, -10) });
                    //marker[i].setLabel(label);
                    // 创建信息窗口对象
                    info[i] = "<div style='margin:0;line-height:20px;padding:2px;'>" + "【地址】：" + rows[i].cus_add + "</br> 【电话】：" + rows[i].cus_tel + "</div>";
                    //创建百度样式检索信息窗口对象                       
                    searchInfoWindow[i] = new BMapLib.SearchInfoWindow(map, info[i], {
                        title: "【客户】" + rows[i].cus_name,      //标题                        
                        panel: "panel",         //检索结果面板
                        enableAutoPan: true,     //自动平移
                        searchTypes: [
                            BMAPLIB_TAB_SEARCH,   //周边检索
                            BMAPLIB_TAB_TO_HERE,  //到这里去
                            BMAPLIB_TAB_FROM_HERE //从这里出发
                        ]
                    });
                    //添加点击事件
                    marker[i].addEventListener("click",
                        (function (k) {
                            // js 闭包
                            return function () {
                                //将被点击marker置为中心
                                map.centerAndZoom(point[k], 16);
                                //在marker上打开检索信息窗口
                                searchInfoWindow[k].open(marker[k]);
                            }
                        })(i)
                    );
                }
            }
        });
    }

    function getCity() {
        $.ajax({
            url: "hr_employee.getDefaultCity.xhd",
            type: "get",
            data: { rnd: Math.random() },
            success: function (data) {
                if (data)
                    search(data);
            }
        });
       
    }

</script>
