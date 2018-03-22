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
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>

    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <title>地图单击事件</title>
</head>
<body>
    <div id="toolbar"></div>
    <div id="allmap"></div>
    <input type="hidden" id="T_xy" name="T_xy" />
</body>
</html>
<script type="text/javascript">
    var xy = getparastr("xy"), x, y;
    $(function () {
        //toolbar();
        resize();
        $(window).resize(function () {
            resize();
        });
    })

    // 百度地图API功能
    var map = new BMap.Map("allmap");
    map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
    x = 116.404, y = 39.915;

    var point = new BMap.Point(x, y)
    map.centerAndZoom(point, 8);

    
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
    map.addControl(new BMap.CityListControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT
        // 切换城市之间事件
        // onChangeBefore: function(){
        //    alert('before');
        // },
        // 切换城市之后事件
        // onChangeAfter:function(){
        //   alert('after');
        // }
    }));

    var mapType1 = new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP] });

    var overView = new BMap.OverviewMapControl();
    var overViewOpen = new BMap.OverviewMapControl({ isOpen: true, anchor: BMAP_ANCHOR_BOTTOM_RIGHT });
    //添加地图类型和缩略图
    //map.addControl(mapType1);          //2D图，卫星图
    map.addControl(overView);          //添加默认缩略地图控件
    map.addControl(overViewOpen);      //右下角，打开

    if (xy) {
        arr = xy.split(',');

        if (arr != "undefined") {
            //alert();
            x = arr[0], y = arr[1];
            $("#T_xy").val(xy); 

            point = new BMap.Point(x, y)
            setTimeout(function () {
                map.panTo(point);
                map.setZoom(16);
            }, 500)


            var marker = new BMap.Marker(point); // 创建点
            map.addOverlay(marker);    //增加点
        }
    }
    else {
        getCity();
    }

    function showInfo(e) {
        map.clearOverlays();
        xy = e.point.lng + ", " + e.point.lat;
        $("#T_xy").val(xy);
        var marker = new BMap.Marker(new BMap.Point(e.point.lng, e.point.lat)); // 创建点
        //marker.addEventListener("click",attribute);
        map.addOverlay(marker);    //增加点
    }
    map.addEventListener("click", showInfo);

    function search(value) {
        map.centerAndZoom(value, 12);
    }

    //工具条实例化
    function toolbar() {

        //alert(data);
        var items = [];

        items.push({
            type: 'textbox',
            id: 'T_Provinces',
            text: '城市：'
        });
        items.push({
            type: 'textbox',
            id: 'T_City'
        });
        items.push({
            type: 'text',text:'注意：请自行标注，然后点击保存，再点击客户信息的保存按钮。'
        });
        $("#toolbar").ligerToolBar({
            items: items

        });

        b = $('#T_City').ligerComboBox({ width: 96, emptyText: '（空）', onSelected: function (newvalue, newtext) { search(newtext); } });
        c = $('#T_Provinces').ligerComboBox({
            width: 97,
            url: "Param_City.combo1.xhd?rnd=" + Math.random(),
            onSelected: function (newvalue, newtext) {
                if (!newvalue)
                    newvalue = -1;
                $.get("Param_City.combo2.xhd?pid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                    b.setData(eval(data));
                });
                search(newtext);
            }, emptyText: '（空）'
        });

    }
    function resize() {
        var h = document.documentElement.clientHeight;
        $("#allmap").height(h - $("#toolbar").height());
    }
    function f_save() {
        var xy_val = $("#T_xy").val();
        if (xy_val)
            return xy_val
        else {
            alert('还未标注！')
            return false;
        }
    }
    function getCity() {
        $.get("hr_employee.getDefaultCity.xhd", Math.random(), function (data) {
            if (data)
                search(data);
        })
    }
</script>
