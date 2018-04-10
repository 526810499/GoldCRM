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
            loadForm();
        });

        function f_save() {

            return { id: "", "weight": $("#T_weight").val(), "number": $("#T_number").val(), "category_id": $("#T_category_val").val(), "categoryName": $("#T_category").val() };

        }



        function loadForm() {

            var rows = [];

            rows.push(
                    [
                       { display: "克重", name: "T_weight", type: "number", validate: "{required:true}" },
                    ],
                    [
                        { display: "数量", name: "T_number", type: "digits", validate: "{required:true}" }
                    ],
                    [
                     { display: "品类", name: "T_category", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'Product_category.tree.xhd?qxz=1',idFieldName: 'id', checkbox: false},value:''}", validate: "{required:true}" }
                    ]
                );


            $("#form1").ligerAutoForm({
                labelWidth: 80, inputWidth: 180, space: 20,
                fields: [
                    {
                        display: '订购补货', type: 'group', icon: '',
                        rows: rows

                    }
                ]
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
