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
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js?v=1.0" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));
            loadForm();
            $("#btn_upname").ligerButton({ text: "保存", width: 60, click: f_save });

        });

        function f_save() {

            if ($(form1).valid()) {

                var data = $("form :input").fieldSerialize() + "&id=" + $("#hid").val();
                $.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "STodayBroadcast.save.xhd?" + data,
                    type: "get",
                    dataType: "json",
                    success: function (result) {
                        $.ligerDialog.closeWaitting();
                    }, error: function (result) {
                        $.ligerDialog.error('操作失败！');
                        $.ligerDialog.closeWaitting();
                    }
                });
            }

        }

        function loadForm() {
            $.ajax({
                type: "get",
                url: "STodayBroadcast.from.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);

                    var rows = [];
                    $("#hid").val(obj.id);
                    rows.push(
                            [
                               { display: "今日金价(克)", name: "T_TodayGlodPrice", type: "number", validate: "{required:true}", initValue: toMoney(obj.TodayGlodPrice, "") },
                            ],
                            [
                                { display: "其他播报", name: "T_OtherBrodcast", type: "textarea", cols: 73, rows: 6, width: 600, cssClass: "l-textarea", initValue: obj.OtherBrodcast }
                            ],
                           [
                                { display: "备注", name: "T_Remark", type: "textarea", cols: 73, rows: 2, width: 600, cssClass: "l-textarea", initValue: obj.Remark }
                           ]
                        );


                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '今日播报', type: 'group', icon: '',
                                rows: rows
                            }
                        ]
                    });

                }
            });
        }


    </script>

</head>
<body style="overflow: hidden;">
    <form id="form1" onsubmit="return false">
    </form>
    <div style="padding: 5px 4px 5px 2px;">
        <input type="hidden" id="hid" name="hid" />
        <div id="maingrid4">
        </div>
        <div id="btn_upname"></div>
    </div>
</body>
</html>
