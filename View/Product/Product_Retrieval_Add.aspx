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
    <script src="../JS/XHD.js" type="text/javascript"></script>

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
                url: "SProduct_Retrieval.form.xhd", /* ע���������ֶ�ӦCS�ķ������� */
                data: { id: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    var rows = [];

                    if (obj.dep_id == null || obj.dep_id == undefined) {
                        obj.dep_id = "";
                    }
                    if (obj.category_id == null || obj.category_id == undefined) {
                        obj.category_id = "";
                    }
                    rows.push(
                            [
                               { display: "����", name: "T_weight", type: "number", validate: "{required:true}", initValue: toMoney(obj.weight) },
                            ],
                            [
                                { display: "����", name: "T_number", type: "digits", validate: "{required:true}", initValue: (obj.number) }
                            ],
                            [
                             { display: "Ʒ��", name: "T_category", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_category.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.category_id + "'}", validate: "{required:true}" }
                            ],
                            [
                             { display: "�ŵ�", name: "T_dep_id", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'hr_department.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.dep_id + "'}", validate: "{required:true}" }
                            ],
                            [
                                { display: "��ע", name: "T_remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
                            ]
                        );


                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '��������', type: 'group', icon: '',
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
