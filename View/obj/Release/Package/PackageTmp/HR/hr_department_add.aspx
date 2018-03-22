<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();
            loadForm(getparastr("depid"));
            //if (getparastr("depid")) {

            //}
            //else {
            //    $("#T_parent").ligerComboBox({
            //        width: 150,
            //        selectBoxWidth: 150,
            //        selectBoxHeight: 150,
            //        valueField: 'id',
            //        textField: 'text',
            //        treeLeafOnly: false,
            //        tree: {
            //            onBeforeSelect: function (node) {
            //                if (node.data['d_type'] == '部门') {
            //                    $("#T_deptype").ligerGetComboBoxManager()._setReadonly(1);
            //                    $("#T_deptype").ligerGetComboBoxManager().selectValue("部门");
            //                }
            //                else if (node.data['d_type'] == '公司')
            //                {                               
            //                    $("#T_deptype").ligerGetComboBoxManager()._setReadonly(0);
            //                }
            //                else {
            //                    $("#T_deptype").ligerGetComboBoxManager()._setReadonly(1);
            //                    $("#T_deptype").ligerGetComboBoxManager().selectValue("公司");
            //                }
            //            },
            //            url: 'hr_department.deptree.xhd?rnd=' + Math.random(),
            //            usericon: 'd_icon',
            //            idFieldName: 'id',
            //            //parentIDFieldName: 'pid',
            //            checkbox: false
            //        }
            //    })
            //}


        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&id=" + getparastr("depid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "hr_department.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == null || obj[n] == "null")
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_depname").val(obj.dep_name);
                    $("#T_sort").val(obj.dep_order);
                    $("#T_leader").val(obj.dep_chief);
                    $("#T_tel").val(obj.dep_tel);
                    $("#T_email").val(obj.dep_email);
                    $("#T_fax").val(obj.dep_fax);
                    $("#T_add").val(obj.dep_add);
                    $("#T_descript").val(obj.dep_descript);

                    
                    //$("#T_parent").ligerGetComboBoxManager().selectValue(obj.parentid);
                    $("#T_parent").ligerComboBox({
                        width: 150,
                        selectBoxWidth: 150,
                        selectBoxHeight: 150,
                        valueField: 'id',
                        textField: 'text',
                        treeLeafOnly: false,
                        value: obj.parentid,
                        onBeforeSelect: function (node) {
                            if (node.data['d_type'] == '部门') {
                                $("#T_deptype").ligerGetComboBoxManager()._setReadonly(1);
                                $("#T_deptype").ligerGetComboBoxManager().selectValue("部门");
                            }
                            else if (node.data['d_type'] == '公司') {
                                $("#T_deptype").ligerGetComboBoxManager()._setReadonly(0);
                            }
                            else {
                                $("#T_deptype").ligerGetComboBoxManager()._setReadonly(1);
                                $("#T_deptype").ligerGetComboBoxManager().selectValue("公司");
                            }

                        },
                        tree: {
                            onSuccess: function () {
                                setTimeout(function () {
                                    var manager = $(".l-tree").ligerGetTreeManager();
                                    manager.remove($("#" + oaid));
                                }, 100);
                            },
                            
                            url: 'hr_department.deptree.xhd?rnd=' + Math.random(),
                            usericon: 'd_icon',
                            idFieldName: 'id',
                            iconpath: "../images/icon/",
                            checkbox: false
                        }
                    })
                    if (obj.parentid == 'root')
                        $("#T_deptype").ligerGetComboBoxManager()._setReadonly(1);
                    
                    $("#T_deptype").ligerGetComboBoxManager().selectValue(obj.dep_type);
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">

        <table align="left" border="0" cellpadding="3" cellspacing="1" style="width: 462px;" class="aztable">

            <tr>
                <td>
                    <div align="left" style="width: 60px">机构名称：</div>
                </td>
                <td>
                    <input type='text' id="T_depname" name="T_depname" ltype="text" ligerui="{width:150}" validate="{required:true}" /></td>
                <td>
                    <div align="left" style="width: 60px">上级机构：</div>
                </td>
                <td>
                    <input type="text" id="T_parent" name="T_parent" validate="{required:true}"  />
                </td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 60px">机构类型：</div>
                </td>
                <td>
                    <input type="text" id="T_deptype" name="T_deptype" ltype="select" ligerui="{width:150,data:[{id:'部门',text:'部门'},{id:'公司',text:'公司'}]}"  validate="{required:true}" /></td>
                <td>
                    <div align="left" style="width: 60px">部门排序：</div>
                </td>
                <td>
                    <input type='text' id="T_sort" name="T_sort" ltype='spinner' ligerui="{type:'int',width:150}" value="20"  validate="{required:true}" /></td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 60px">负责人：</div>
                </td>
                <td>
                    <input type='text' id="T_leader" name="T_leader" ltype="text" ligerui="{width:150}" /></td>
                <td>
                    <div align="left" style="width: 60px">电话：</div>
                </td>
                <td>
                    <input type='text' id="T_tel" name="T_tel" ltype="text" ligerui="{width:150}" /></td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 60px">邮箱：</div>
                </td>
                <td>
                    <input type='text' id="T_email" name="T_email" ltype="text" ligerui="{width:150}" validate="{required:false,email:true}" /></td>
                <td>
                    <div align="left" style="width: 70px">传真：</div>
                </td>
                <td>
                    <input type='text' id="T_fax" name="T_fax" ltype="text" ligerui="{width:150}" /></td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 54px">地址：</div>
                </td>
                <td colspan="3">
                    <input type='text' id="T_add" name="T_add" ltype="text" ligerui="{width:390}" /></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <div align="left" style="width: 54px">描述：</div>
                </td>
                <td colspan="3">
                    <input type="text" id="T_descript" name="T_descript" ltype="text" ligerui="{width:390}" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
