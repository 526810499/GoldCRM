<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />
    
   <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        var sys_id = "", sys_name = "", sys_version = "";
        $(function () {
            $.ajax({
                type: "GET",
                url: "S_Sys_info.grid.xhd", /* 注意后面的名字对应CS的方法名称 */
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
                    sys_id = sysinfo["sys_guid"];
                    sys_name = sysinfo["sys_name"];
                    sys_version = sysinfo["sys_version"];
                    //alert(obj.constructor); //String 构造函数
                    $("#Label1").text(sys_version);
                    $("#Label2").attr(sysinfo["sys_vinfo"]);
                }
            });
        });
        function checkup() {
            var T_name = "";
            if ($("#T_send").attr("checked")) {
                T_name = sys_name;
            }
            $.ligerDialog.waitting('正在获取服务器版本,请稍候...');
            $.ajax({
                type: "GET",
                url: "xhd_Service.getVersion.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'getversion', T_guid: sys_id, T_name: T_name, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var server_version = result.version_name;

                    server_version = server_version.toLowerCase().replace('v', '');
                    sys_version = sys_version.toLowerCase().replace('v', '');

                    var a, b = new Array();
                    a = server_version.split('.');
                    b = sys_version.split('.');

                    if (a[3] * 1 < 10000) a[3] = a[3] * 10;
                    if (b[3] * 1 < 10000) b[3] = b[3] * 10;

                    var va = a[3] * 1 + a[2] * 100000 + a[1] * 10000000 + a[0] * 1000000000;
                    var vb = b[3] * 1 + b[2] * 100000 + b[1] * 10000000 + b[0] * 1000000000;

                    //alert(va + ":" + vb)
                    $.ligerDialog.closeWaitting();
                    if (vb < va) {
                        $.ligerDialog.question("有 " + result.version_name + " 版本可以更新，请前往<br/>    <a href='http://www.xhdcrm.com/down' target='_blank'>{xx}CRM官网</a>   下载更新。", "提示");
                    }
                    else {
                        $.ligerDialog.success("已经是最新版本，无需升级！");
                    }
                }
                ,error: function ()
                {
                    $.ligerDialog.closeWaitting();
                }
            })
        }

    </script>
    </head>
<body style="padding: 0px;overflow-y:scroll;">
    <form id="form1">
        <table class="bodytable0" style="width: 100%; margin: -1px">

            <tr>
                <td height="23" width="150" class="title" colspan="2" style="border-top: none;">系统信息</td>
            </tr>

            <tr>
                <td height="23" width="150" class="table_label">当前版本号：</td>
                <td height="23">
                    <span id="Label1">1</span>
                </td>
            </tr>
             <tr>
                <td height="23" class="table_label">用户体验计划：</td>
                <td height="23">
                    <input id="T_send" type="checkbox" checked="checked" />发送企业名</td>
            </tr>
            <tr>
                <td height="23" class="table_label">&nbsp;</td>
                <td height="23">
                    <span id="Label4">
                        <input type="button" value="检查更新" style="width: 80px; height: 22px;" onclick="checkup()"></span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">当前版本信息：</td>
                <td height="23">
                    <span id="Label2">1、【优化】全新的系统架构。 
                    <br />
                    2、【新增】任务管理系统。 
                    <br />
                    3、【新增】短信功能。 
                    <br />
                    4、【新增】客户地图及标记功能。 
                    <br />
                    5、【新增】内置帮助系统。 
                    <br />
                    6、【新增】联系人导入。 
                    <br />
                    7、【新增】客户查重功能。 
                    <br />
                    </span>8、【优化】参数按中文排序。 
                    <br />
                    9、【优化】客户列表里，联系人显示电话和部门列。 
                    <br />
                    10、【优化】对所有金额列添加统计功能。 
                    <br />
                    11、【优化】跟进加入跟进目的与跟进对象。 
                    <br />
                    12、【优化】加强验证，提示验证码过期。 
                    <br />
                    13、【优化】优化上传组件，减少错误。 
                    <br />
                    14、【优化】优化导入，减少错误。 
                    <br />
                    15、【优化】优化导出，生成EXCLE表。 
                    <br />
                    16、【优化】下载不再需要在新页面打开。 
                    <br />
                    17、【优化】客户详情页需要检查权限。 
                    <br />
                    18、【优化】新增跟进之后不再取消客户的选择。 
                    <br />
                    19、【优化】便签的删除按钮显示手型图标。 
                    <br />
                    20、【优化】部分窗口加入最大化功能。 
                    <br />
                    21、【优化】合同加入到期查询。 
                    <br />
                    22、【优化】日志加入详细修改记录。 
                    <br />
                    23、【优化】字段加强验证。 
                    <br />
                    24、【修复】表格编辑问题。 
                    <br />
                    25、【修复】表格数据行数为0时显示undefined问题。 
                    <br />
                    26、【修复】表格只有一行数据不刷新问题。 
                    <br />
                    27、【修复】表格序号显示。 
                    <br />
                    28、【修复】跟进列表右键错误问题。 
                    <br />
                    29、【修复】右键菜单图标问题。 <br />
                    30、【修复】联系人多次加载问题。</td>
            </tr>
           
        </table>
    </form>
</body>
</html>
