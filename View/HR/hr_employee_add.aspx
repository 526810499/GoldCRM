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

    <script src="../lib/ligerUI/js/common.js" type="text/javascript">></script>
    <script src="../lib/jquery.form.js" type="text/javascript">></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>

    <script type="text/javascript">
        var manager;
        $(function () {

            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            if (getparastr("empid")) {
                loadForm(getparastr("empid"));
            }
            var empid = getparastr("empid") ? getparastr("empid") : 0;

            $("#maingrid4").ligerGrid({
                columns: [
                        //{ display: '公司', name: 'name', align: 'left', width: 150 },
                        { display: '岗位', name: 'post_name', width: 100 },
                        { display: '部门', name: 'dep_name', width: 100 },
                        { display: '职务', name: 'position_name', width: 100 },
                        {
                            display: '默认', name: 'default_post', width: 40, render: function (item, i) {
                                var html = "<div style='margin-top:8px;'><input type='radio' name='default_post_check' onclick='check()' value='" + i + "'";
                                if (item.default_post == 1) html += " checked='checked' ";
                                else html += " /></div>";
                                return html;
                            }
                        }
                ],
                selectRowButtonOnly: true,
                allowHideColumn: false,
                //onAfterShowData: onAfterShowData,
                usePager: false,
                //checkbox: true,
                url: "hr_post.getpostbyempid.xhd?id=" + empid,
                width: 405, height: 150,
                heightDiff: 0
            });
            manager = $("#maingrid4").ligerGetGridManager();
            $("#uploadimg").ligerButton({ text: "上传头像", width: 130, click: uploadimg });
            $("#btn_add").ligerButton({ text: "新增", width: 60, click: add });
            $("#btn_del").ligerButton({ text: "删除", width: 60, click: removepost });

            $("#T_uid").attr("validate", "{ required: true, username: true, remote: remote, messages: { required: '请输入账号名', remote: '此账户存在!' } }");
        });

        function check(evt) {
            var evt = evt || window.event;
            var e = evt.srcElement || evt.target;

            var data = manager.getData();

            for (var i = 0; i < data.length; i++) {
                manager.updateCell('default_post', 0, i);
            }
            manager.updateCell('default_post', 1, e.value);

            console.log(data);
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "hr_employee.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == null || obj[n] == "null" || obj[n] == undefined)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_uid").val(obj.uid);
                    $("#T_email").val(obj.email);
                    $("#T_name").val(obj.name);
                    $("#T_birthday").val(obj.birthday);
                    $("#T_idcard").val(obj.idcard);
                    $("#T_tel").val(obj.tel);
                    $("#T_entryDate").val(obj.EntryDate);
                    $("#T_Adress").val(obj.address);
                    $("#T_school").val(obj.schools);
                    $("#T_edu").val(obj.education);
                    $("#T_professional").val(obj.professional);
                    $("#T_remarks").val(obj.remarks);

                    $("#T_sex").ligerGetComboBoxManager().selectValue(obj.sex);
                    //$("#T_dep").ligerGetComboBoxManager().selectValue(obj.d_id);
                    //$("#T_Position").ligerGetComboBoxManager().selectValue(obj.zhiwuid);
                    $("#T_status").ligerGetComboBoxManager().selectValue(obj.status);

                    // $("#T_uid").ligerGetTextBoxManager().setDisabled();
                    // $("#T_uid").attr("validate", "{required:true}");
                    $("input[type=radio][value=" + obj.canlogin + "]").attr("checked", 'checked');

                    if (!obj.title)
                        obj.title = "noheader.png";

                    $("#headurl").val(obj.title);
                    $("#userheadimg").attr("src", "../file/header/" + obj.title);

                }
            });
        }

        function remote() {
            var url = "hr_employee.Exist.xhd?emp_id=" + getparastr("empid") + "&rnd=" + Math.random();
            return url;
        }

        function uploadimg() {
            top.$.ligerDialog.open({
                zindex: 9004,
                width: 850, height: 600,
                title: '上传头像',
                url: 'hr/headimage.aspx',
                buttons: [
                {
                    text: '保存', onclick: function (item, dialog) {
                        saveheadimg(item, dialog);
                    }
                },
                {
                    text: '关闭', onclick: function (item, dialog) {
                        dialog.close();
                    }
                }
                ],
                isResize: true
            });
        }

        function saveheadimg(item, dialog) {
            var upfile = dialog.frame.f_save();
            //alert(upfile);
            if (upfile) {
                dialog.close();
                $.ligerDialog.waitting('数据保存中,请稍候...');

                $.ajax({
                    url: "hr_employee.Base64Upload.xhd", type: "POST",
                    data: upfile,
                    dataType: "json",
                    success: function (result) {
                        $.ligerDialog.closeWaitting();

                        var obj = eval(result);
                        if (obj.isSuccess) {
                            $("#headurl").val(obj.Message);
                            $("#userheadimg").attr("src", "../file/header/" + obj.Message);
                        }
                        else {
                            $.ligerDialog.error(obj.Message);
                        }


                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('操作失败！');
                    }
                });
            }
        }

        function add() {
            f_openWindow("hr/hr_getpost.aspx", "选择岗位", 650, 400, f_getpost, 9003);
        }
        function removepost() {
            manager.deleteSelectedRow();
        }
        function f_getpost(item, dialog) {
            var rows = null;
            if (!dialog.frame.f_select()) {
                alert('请选择岗位!');
                return;
            }
            else {
                rows = dialog.frame.f_select();
                rows.default_post = "0";
                var manager = $("#maingrid4").ligerGetGridManager();

                var data = manager.getData();

                var canadd = 1;
                for (var i = 0; i < data.length; i++) {
                    if (rows.id == data[i].id)
                        canadd = 0;
                }

                if (canadd)
                    manager.addRow(rows);

                dialog.close();
                //onAfterShowData()
            }
        }
        function f_save() {
            if ($("input[name=default_post_check]").length == 0) {
                $.ligerDialog.warn("请添加岗位！");
                return;
            }

            if (!f_checkdefault()) {
                $.ligerDialog.warn("请选择默认岗位！");
                return;
            }

            if ($(form1).valid()) {
                var data = manager.getData();

                var sendtxt = "&id=" + getparastr("empid");
                sendtxt += "&PostData=" + JSON.stringify(data);
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
        function f_postnum() {
            return $("input[name=default_post_check]").length;
        }
        function f_postdata() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var data = manager.getData();
            return JSON.stringify(data);
        }
        function f_checkdefault() {
            var checked = false;

            $("input[name=default_post_check]").each(function () {
                if (this.checked == true) {
                    checked = true;
                }
            })

            return checked;
        }

    </script>

</head>
<body style="margin: 0; overflow: hidden">
    <form id="form1" onsubmit="return false">
        <div>
            <table class="aztable" width="647px">
                <tr>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            账号：
                        </div>
                    </td>
                    <td width="182px" colspan="3">
                        <input type="text" id="T_uid" name="T_uid" ltype="text" ligerui="{width:160}" />
                    </td>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            Email：
                        </div>
                    </td>
                    <td width="182px">
                        <input type="text" id="T_email" name="T_email" ltype="text" ligerui="{width:160}" <%-- validate="{required:false,email:true}"--%> /></td>
                    <td width="132px" rowspan="5">
                        <img id="userheadimg" src="a.gif" onerror="noheadimg()" style="border-radius: 4px; box-shadow: 1px 1px 3px #111; width: 120px; height: 120px; margin-left: 5px; background: #d2d2f2; border: 3px solid #fff; behavior: url(../css/pie.htc);" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 62px">姓名：</div>
                    </td>
                    <td colspan="3">
                        <input type="text" id="T_name" name="T_name" ltype="text" ligerui="{width:160}" validate="{required:true,minlength:1,maxlength:50,messages:{required:'请输入姓名',maxlength:'你的名字有这么长嘛？'}}" />
                    </td>
                    <td>
                        <div align="right" style="width: 62px">生日：</div>
                    </td>
                    <td>
                        <input type="text" id="T_birthday" name="T_birthday" ltype="date" ligerui="{width:160}" /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 62px">性别：</div>
                    </td>
                    <td width="45px">
                        <input type="text" id="T_sex" name="T_sex" ltype="select" ligerui="{width:40,data:[{id:'男',text:'男'},{id:'女',text:'女'}]}"  validate="{required:true}"  />
                    </td>
                    <td width="42">
                        <div align="right">状态：</div>
                    </td>
                    <td width="80px">
                        <input type="text" id="T_status" name="T_status" style="width: 60px" ltype="select" ligerui="{width:65,data:[{id:'在职',text:'在职'},{id:'离职',text:'离职'}]}"  validate="{required:true}"  /></td>
                    <td>
                        <div align="right" style="width: 61px">
                            身份证：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_idcard" name="T_idcard" ltype="text" ligerui="{width:160}" <%--validate="{required:false,IdNumber:true,messages:{required:'请输入身份证号码'}}"--%> /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            手机：
                        </div>
                    </td>
                    <td colspan="3">
                        <input type="text" id="T_tel" name="T_tel" ltype="text" ligerui="{width:160}"  validate="{required:true,cellphone:true,messages:{required:'请输入手机号'}}" />
                    </td>
                    <td>
                        <div align="right" style="width: 62px">入职日期：</div>
                    </td>
                    <td>
                        <input type="text" id="T_entryDate" name="T_entryDate" ltype="date" ligerui="{width:160}"  validate="{required:true}"  /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            学历：
                        </div>
                    </td>
                    <td colspan="3">
                        <input type="text" id="T_edu" name="T_edu" ltype="text" ligerui="{width:160}" /></td>
                    <td>
                        <div align="right" style="width: 62px">专业：</div>
                    </td>
                    <td>
                        <input type="text" id="T_professional" name="T_professional" ltype="text" ligerui="{width:160}" /></td>
                    <td>
                        <input type="hidden" id="headurl" name="headurl" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            地址：
                        </div>
                    </td>
                    <td colspan="5">
                        <input type="text" id="T_Adress" name="T_Adress" ltype="text" ligerui="{width:403}" /></td>
                </tr>

                <tr>
                    <td>
                        <div align="right" style="width: 62px">毕业院校：</div>
                    </td>
                    <td colspan="5">
                        <input type="text" id="T_school" name="T_school" ltype="text" ligerui="{width:403}" /></td>
                    <td>
                        <div style="text-align: center">
                            <div id="uploadimg"></div>
                            <%--<input type="button" value="上传头像" style="width: 80px; height: 22px;" onclick="uploadimg()" />--%>
                        </div>

                    </td>
                </tr>

                <tr>
                    <td>
                        <div align="right" style="width: 62px">备注：</div>
                    </td>
                    <td colspan="6">
                        <textarea cols="100" id="T_remarks" name="T_remarks" rows="2" class="l-textarea" style="width: 399px"></textarea></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 62px">岗位：</div>
                    </td>
                    <td colspan="3">
                        <div id="btn_add"></div>
                        <div id="btn_del"></div>
                        <%-- <input id="Button1" type="button" value="新增" style="height: 21px" onclick="add()" />
                        <input id="Button2" type="button" value="删除" style="height: 21px" onclick="removepost()" />--%>

                    </td>
                    <td>
                        <div align="right" style="width: 62px">可登陆：</div>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <input type="radio" value="1" name="canlogin" checked="checked" /></td>
                                <td>是 </td>
                                <td>
                                    <input type="radio" value="0" name="canlogin" /></td>
                                <td>否 </td>

                            </tr>
                        </table>
                    </td>
                    <td></td>
                </tr>

            </table>
        </div>
    </form>

    <form id="form2">
        <div>
            <table>
                <tr>
                    <td width="62px">&nbsp;</td>
                    <td colspan="5">
                        <div id="maingrid4" style=""></div>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
