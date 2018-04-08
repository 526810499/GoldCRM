<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI2/js/plugins/ligerTree.js"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js?v=1.0" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            loadForm(getparastr("id"));

        });

        function f_save() {


            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");

                return $("form :input").fieldSerialize() + sendtxt;
            }

        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "SProduct_OldChangeNew.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    var rows = [];

                    if (obj.createdep_id == null || obj.createdep_id == undefined) {
                        obj.createdep_id = "";
                    }
        
                    rows.push(
                            [
                               { display: "旧金重", name: "T_oldWeight", type: "number", validate: "{required:true}", initValue: toMoney(obj.oldWeight,"") },
                               { display: "旧金价值", name: "T_oldTotalPrice", type: "number", validate: "{required:true}", initValue: toMoney(obj.oldTotalPrice, "") }
                            ],
                            [
                                { display: "旧金折旧费", name: "T_oldCharge", type: "number", validate: "{required:true}", initValue: toMoney(obj.oldCharge, "") },
                                { display: "新金重", name: "T_newWeight", type: "number", validate: "{required:true}", initValue: toMoney(obj.newWeight, "") }
                            ],
                           [
                                { display: "新金价值", name: "T_newTotalPrice", type: "number", validate: "{required:true}", initValue: toMoney(obj.newTotalPrice, "") },
                                { display: "工费", name: "T_costsTotalPrice", type: "number", validate: "{required:true}", initValue: toMoney(obj.costsTotalPrice, "") }
                           ],
                           [
                            { display: "折扣", name: "T_discount", type: "number", validate: "{required:true}", initValue: toMoney(obj.discount, "") },
                            { display: "需补差额", name: "T_difTotalPrice", type: "number", validate: "{required:true}", initValue: toMoney(obj.difTotalPrice, "") }
                           ],
                            [
                                { display: "备注", name: "T_remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                            ]
                        );


                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '以旧换新', type: 'group', icon: '',
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

        <div id="maingrid4">
        </div>
    </div>
</body>
</html>
