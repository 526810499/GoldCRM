<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">
        var xNode = null, xtype = 'select';
        UE = parent.UE, xform = parent.xform;
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            $("#maingrid4").ligerGrid({
                columns: [
                        { display: '序号', width: 50, render: function (item,i) { return item.n; } },
                    { display: '联系人', name: 'C_name', width: 100 },
                    { display: '职务', name: 'C_position', width: 100 },
                    { display: '性别', name: 'C_sex', width: 50 },
                    { display: '所属客户', name: 'customer', width: 180 },
                    { display: '手机', name: 'C_mob', width: 120 }

                ],
                //usePager: false,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                checkbox: true,
                url: "CRM_Contact.grid.xhd?smstype=1&smsid=" + getparastr("id"),
                width: '100%', height: 160,
                heightDiff: -1
            })
            loadForm(getparastr("id"));
            toolbar();
        });

        function f_save() {
            if ($(form1).valid()) {
                var manager = $("#maingrid4").ligerGetGridManager();
                var data = manager.getData();

                var ids = "", mobiles = "";
                for (var i = 0; i < data.length; i++) {
                    ids += data[i].id + ",";
                    mobiles += data[i].C_mob + ",";
                }
                return $("form :input").fieldSerialize() + "&ids=" + ids + "&mobiles=" + mobiles;
            }
            else {
                $.ligerDialog.warn("您有字段未通过验证！", "警告！");
            }
        }        

        function f_num()
        {
            var manager = $("#maingrid4").ligerGetGridManager();
            var data = manager.getData();
            var len=data.length;
            if (len < 1)
            {
                $.ligerDialog.warn("请选择联系人", "警告！");
            }
            return len;
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "SMS.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 480, space: 20,
                        fields: [
                            {
                                display: '发送短信', type: 'group', icon: '',
                                rows: [
                                    [{ display: "短信主题", name: "T_title", type: "text", options: "{width:480}", validate: "{required:true}" ,initValue:obj.sms_title}],
                                    [{ display: "短信内容", name: "T_content", type: "textarea", cols: 76, rows: 7, cssClass: 'l-textarea', validate: "{required:true}",initValue:obj.sms_content }]
                                ]
                            }
                        ]
                    });

                }
            });
            
        }

        function toolbar() {
            var items = [];

            items.push({ type: 'button', text: '新增', icon: '../../images/icon/11.png', disable: true, click: function () { add() } });
            items.push({ type: 'button', text: '删除', icon: '../../images/icon/12.png', disable: true, click: function () { deleteRow() } });
            items.push({ type: 'text', text: ' 注：收件人如果未能添加，请检测手机号码是否正确' });

            $("#toolbar").ligerToolBar({
                items: items
            });
        }
        function add() {
            f_openWindow("crm/customer/getcontactmult.aspx", "选择联系人", 580, 400, f_getcontact, 9003);
        }
        function deleteRow() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.deleteSelectedRow();
        }
        function f_getcontact(item, dialog) {
            var rows = dialog.frame.f_select();
            if (!rows || rows == "") {
                alert('请选择联系人!');
                return;
            }

            dialog.close();

            var manager = $("#maingrid4").ligerGetGridManager();
            var data = manager.getData();

            if (data.length + rows.length >= 200) {
                $.ligerDialog.error("最多添加200个号码");
                return;
            }

            for (var i = 0; i < rows.length; i++) {
                var add = 1;

                //手机验证
                var telNum = rows[i].C_mob;

                var length = telNum.length;
                var isTel = (length == 11 && /^(1\d{10})$/.test(telNum))

                if (!isTel) add = 0;

                //过滤重复
                if (add) {
                    for (var j = 0; j < data.length; j++) {
                        if (rows[i].id == data[j].id) {
                            add = 0;
                        }
                    }
                }

                if (add == 1) {
                    manager.addRow(rows[i]);
                }
            }
        }



    </script>
</head>
<body style="overflow:hidden;">
    <form id="form1" onsubmit="return false">
    </form>
    <form id="form2" onsubmit="return false">
        <div style="margin: 2px;margin-top:5px; ">
            <div id="toolbar"></div>
            <div id="maingrid4" style="margin: -1px;"></div>

        </div>
    </form>
</body>
</html>
