<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />

    <link href="../../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../../CSS/input.css" rel="stylesheet" />

    <script src="../../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <script src="../../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../../JS/XHD.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

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
                        display: '删除时间', name: 'Delete_time', width: 180, render: function (item) {
                            return formatTimebytype(item.Delete_time, 'yyyy-MM-dd hh:mm:ss');
                        }
                    }

                ],
                rowtype: "CustomerType",
                checkbox: true,
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "CRM_Customer.grid.xhd?isdel=1&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -10,
                onAfterShowData: function (grid) {
                    $("tr[rowtype='已成交']").addClass("l-treeleve2").removeClass("l-grid-row-alt");
                },

                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }

            });
            $('#serchform').ligerForm();
            initSerchForm();
            toolbar();
            //toolbar1();

        });

        function toolbar() {
            $("#btn_serch").ligerButton({ text: "搜索", width: 60, click: doserch });
            $("#btn_reset").ligerButton({ text: "重置", width: 60, click: doclear });
            $.get("toolbar.GetSys.xhd?mid=tools_customer&rnd=" + Math.random(), function (result, textStatus) {
                var data = eval('(' + result + ')');;
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../../" + arr[i].icon;
                    items.push(arr[i]);
                }

                items.push({
                    type: 'serchbtn',
                    text: '高级搜索',
                    icon: '../../../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
                $("#maingrid4").ligerGetGridManager()._onResize();

            });
        }
        function initSerchForm() {
            var a = $('#T_City').ligerComboBox({ width: 96, emptyText: '（空）' });
            var b = $('#T_Provinces').ligerComboBox({
                width: 97,

                url: "Sys_Provinces.combo.xhd?rnd=" + Math.random(),
                onSelected: function (newvalue) {
                    $.get("Sys_City.combo.xhd?provincesid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        a.setData(eval(data));
                    });
                },
                emptyText: '（空）'
            });
            $('#customertype').ligerComboBox({ width: 97, emptyText: '（空）', url: "Sys_Param.combo.xhd?type=cus_type&rnd=" + Math.random() });
            $('#customerlevel').ligerComboBox({ width: 96, emptyText: '（空）', url: "Sys_Param.combo.xhd?type=cus_level&rnd=" + Math.random() });
            $('#cus_sourse').ligerComboBox({ width: 196, emptyText: '（空）', url: "Sys_Param.combo.xhd?type=cus_source&rnd=" + Math.random() });
            $('#industry').ligerComboBox({ width: 120, emptyText: '（空）', url: "Sys_Param.combo.xhd?type=cus_industry&rnd=" + Math.random() });
            var e = $('#employee').ligerComboBox({ width: 96, emptyText: '（空）' });
            var f = $('#department').ligerComboBox({
                width: 97,
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
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager()._onResize();
            }
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });
        function doserch() {
            var sendtxt = "&isdel=1&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            $.ligerDialog.waitting('数据查询中,请稍候...');
            var manager = $("#maingrid4").ligerGetGridManager();

            manager._setUrl("CRM_Customer.grid.xhd?" + serchtxt);
            manager.loadData(true);
            $.ligerDialog.closeWaitting();
        }
        function doclear() {
            $("input:hidden", "#serchform").val("");
            $("input:text", "#serchform").val("");
            $(".l-selected").removeClass("l-selected");
        }

        function regain() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length > 0) {
                var rowid = "";
                for (var i = 0; i < row.length; i++) {
                    if (i == (row.length - 1)) {
                        rowid += row[i].id;
                    }
                    else {
                        rowid += row[i].id + ",";
                    }
                }
                $.ajax({
                    url: "CRM_Customer.regain.xhd", type: "POST",
                    data: { idlist: rowid, rnd: Math.random() },
                    dataType:'json',
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);

                        if (obj.isSuccess) {
                            $.ligerDialog.warn(obj.Message);
                            f_reload();
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }
                    },
                    error: function () {
                        top.$.ligerDialog.error('恢复失败！');
                    }
                });

            }
            else {
                $.ligerDialog.warn("请选择数据");
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length > 0) {
                $.ligerDialog.confirm("彻底删除后不能恢复，请谨慎操作！<br/>您确定要删除？", function (yes) {
                    if (yes) {
                        var rowid = "";
                        for (var i = 0; i < row.length; i++) {                            
                                rowid += row[i].id + ",";
                        }
                        //alert(rowid);
                        $.ajax({
                            url: "CRM_Customer.del.xhd", type: "POST",
                            data: { idlist: rowid, rnd: Math.random() },
                            dataType:'json',
                            success: function (result) {
                                $.ligerDialog.closeWaitting();

                                var obj = eval(result);

                                if (obj.isSuccess) {
                                    $.ligerDialog.warn(obj.Message);
                                    f_reload();
                                }
                                else {
                                    $.ligerDialog.error(obj.Message);
                                }
                            },
                            error: function () {
                                $.ligerDialog.error('系统错误，删除失败！');
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn("请选择数据");
            }
        }

        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

    </script>
    <style type="text/css">
        .l-treeleve1 { background: orange; }

        .l-treeleve2 { background: yellow; }

        .l-treeleve3 { background: #eee; }
    </style>
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
            <div style="">
                <table style='width: 960px' class="aztable">
                    <tr>
                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>客户名称：</div>
                        </td>
                        <td>
                            <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' /></td>

                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>客户类型：</div>
                        </td>
                        <td>
                            <div style='float: left; width: 100px;'>
                                <input type='text' id='customertype' name='customertype' />
                            </div>
                            <div style='float: left; width: 98px;'>
                                <input type='text' id='customerlevel' name='customerlevel' />
                            </div>
                        </td>
                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>录入时间：</div>
                        </td>
                        <td>
                            <div style='float: left; width: 100px;'>
                                <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                            </div>
                            <div style='float: left; width: 98px;'>
                                <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                            </div>
                        </td>
                        <td>
                            <input id='keyword' name="keyword" type='text' ltype='text' ligerui='{width:196, nullText: "输入关键词搜索地址、描述、备注"}' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>所属行业：</div>
                        </td>
                        <td>
                            <input id='industry' name="industry" type='text' /></td>

                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>所属地区：</div>
                        </td>
                        <td>
                            <div style='float: left; width: 100px;'>
                                <input type='text' id='T_Provinces' name='T_Provinces' />
                            </div>
                            <div style='float: left; width: 98px;'>
                                <input type='text' id='T_City' name='T_City' />
                            </div>
                        </td>
                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>最后跟进：</div>
                        </td>
                        <td>
                            <div style='float: left; width: 100px;'>
                                <input type='text' id='startfollow' name='startfollow' ltype='date' ligerui='{width:97}' />
                            </div>
                            <div style='float: left; width: 98px;'>
                                <input type='text' id='endfollow' name='endfollow' ltype='date' ligerui='{width:96}' />
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>电话：</div>
                        </td>
                        <td>
                            <input type='text' id='tel' name='tel' ltype='text' ligerui='{width:120}' />
                        </td>

                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>客户来源：</div>
                        </td>
                        <td>
                            <input type='text' id='cus_sourse' name='cus_sourse' />
                        </td>
                        <td>
                            <div style='float: right; text-align: right; width: 60px;'>业务员：</div>
                        </td>
                        <td>
                            <div style='float: left; width: 100px;'>
                                <input type='text' id='department' name='department' />
                            </div>
                            <div style='float: left; width: 98px;'>
                                <input type='text' id='employee' name='employee' />
                            </div>
                        </td>
                        <td>
                            <div id="btn_serch"></div>
                            <div id="btn_reset"></div>
                            <%--<input id='Button2' type='button' value='重置' style='height: 24px; width: 80px;' onclick=" doclear() " />
                            <input id='Button1' type='button' value='搜索' style='height: 24px; width: 80px;' onclick=" doserch() " />--%>
                        </td>
                    </tr>
                </table>

            </div>
        </form>
    </div>
</body>
</html>

