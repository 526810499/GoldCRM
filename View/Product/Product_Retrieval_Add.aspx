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
        var manager;
        function f_save() {
            manager = $("#maingrid4").ligerGetGridManager();

            var data = manager.getData();
            if (data == null || data.length == 0) {
                $.ligerDialog.warn('请添加订购产品');
                return false;
            }

            if ($(form1).valid()) {
                var sendtxt = "&id=" + getparastr("id");
                sendtxt += "&postdata=" + urlencode(JSON.stringify(GetPostData()));
                return $("form :input").fieldSerialize() + sendtxt;
            }

        }

        function GetPostData() {
            manager.endEdit(true);
            var data = manager.getData();
            var items = [];
            if (data != null) {
                $(data).each(function (i, v) {
                    var datas = { id: v.id, weight: parseFloat(v.weight), number: parseInt(v.number), category_id: v.category_id, __status: v.__status, categoryName: v.categoryName };
                    items.push(datas);
                });
            }
            return items;
        }

        function f_grid() {

            $("#maingrid4").ligerGrid({
                columns:
                    [
                { display: "克重", name: "weight", editor: { type: 'number' } },
                { display: "数量", name: "number", editor: { type: 'digits' } },
                {
                    display: "品类", name: "categoryName"
                }
                    ],
                allowHideColumn: false,
                title: '订购明细',
                usePager: false,
                enabledEdit: true,
                rownumbers: true,
                url: "SProduct_Retrieval.gridDetail.xhd?retrid=" + getparastr("id"),
                width: '100%',
                height: 500,
                heightDiff: -1,
                onLoaded: f_loaded
            });

        }

        function f_loaded() {

            $(".l-panel-header").append("<div id='headerBtn' style='width:250px;float:right;margin-bottom:2px;'><div id = 'btn_add' style='margin-top:2px;'></div><div id = 'btn_del' style='margin-top:2px;'></div></div>");
            $(".l-grid-loading").fadeOut();

            $("#btn_add").ligerButton({
                width: 80,
                text: "添加",
                icon: '../../images/icon/11.png',
                click: add
            });
            $("#btn_del").ligerButton({
                width: 80,
                text: "删除",
                icon: '../../images/icon/12.png',
                click: pro_remove
            });

            $("#maingrid4").ligerGetGridManager()._onResize();
        }
        function pro_remove() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.deleteSelectedRow();
        }
        function add() {
            f_openWindow("product/Product_Retrieval_AddDe.aspx?", "新增明细", 500, 400, f_getpost, 9003);
        }

        function f_getpost(item, dialog) {
            var data = dialog.frame.f_save();
            if (data != null) {
                $("#maingrid4").ligerGetGridManager().addRow(data);
            }
            dialog.close();
        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "SProduct_Retrieval.form.xhd", /* 注意后面的名字对应CS的方法名称 */
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
                    if (obj.category_id == null || obj.category_id == undefined) {
                        obj.category_id = "";
                    }
                    rows.push(
                            [
                             { display: "门店", name: "T_dep_id", type: "select", options: "{width:180,treeLeafOnly: false,tree:{url:'hr_department.tree.xhd?qxz=1',idFieldName: 'id',checkbox: false},value:'" + obj.createdep_id + "'}", validate: "{required:true}" }
                            ],
                            [
                                { display: "备注", name: "T_remark", type: "textarea", cols: 73, rows: 4, width: 465, cssClass: "l-textarea", initValue: obj.remark }
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
            });

            f_grid();
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
