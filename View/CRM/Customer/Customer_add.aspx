<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../CSS/input.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />

    <meta http-equiv="X-UA-Compatible" content="ie=edge chrome=1" />
    <script src="../../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../JS/XHD.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript">></script>
    <script src="../../lib/jquery.form.js" type="text/javascript">></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($("#form1"));
            //$("form").ligerForm();

            loadForm(getparastr("cid"));

        })


        function f_save() {
            if ($(form1).valid()) {
                var extendform = $('#formextend').serializeForm();
                var extendjson = JSON.stringify(extendform);

                var sendtxt = "&Action=save&id=" + getparastr("cid");
                sendtxt += "&extendjson=" + extendjson;

                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
        var a; var b; var c; var d; var e; var f; var g; var h; var i;

        function loadForm(oaid) {
            //var dialog = frameElement.dialog;
            //dialog.setShowToggle(0);
            $.ajax({
                type: "GET",
                url: "CRM_Customer.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, labelWidth: 80, space: 20,
                        fields: [
                            {
                                display: '客户', type: 'group', icon: '',
                                rows: [
                                    [
                                        { display: "客户名称", name: "T_customer", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.cus_name },
                                        { display: "网址", name: "T_Website", type: "text", options: "{width:180}", initValue: obj.cus_website }
                                    ],
                                    [
                                        { display: "电话", name: "T_tel", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.cus_tel },
                                        { display: "传真", name: "T_fax", type: "text", options: "{width:180}", initValue: obj.cus_fax }
                                    ],
                                    [
                                        { display: "省份", name: "T_Provinces" },
                                        { display: "城市", name: "T_City" }
                                    ],
                                    [
                                        { display: "地址", name: "T_address", type: "text", options: "{width:378}", width: 378, initValue: obj.cus_add },
                                        { type: "html", width: 70, html: "<div id='btn_map'></div>" }
                                    ],
                                    [
                                        { display: "类型", name: "T_type", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=cus_type',emptyText: '（空）',value:'" + obj.cus_type_id + "'}" },
                                        { display: "级别", name: "T_level", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=cus_level',emptyText: '（空）',value:'" + obj.cus_level_id + "'}" }
                                    ],
                                    [
                                        { display: "来源", name: "T_source", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=cus_source',emptyText: '（空）',value:'" + obj.cus_source_id + "'}" },
                                        { display: "行业", name: "T_industry", type: "select", options: "{width:180,url:'Sys_Param.combo.xhd?type=cus_industry',emptyText: '（空）',value:'" + obj.cus_industry_id + "'}" }
                                    ],
                                    [
                                        { display: "描述", name: "T_descript", type: "text", options: "{width:465}", width: 469, initValue: obj.DesCripe },
                                    ],
                                    [
                                        { display: "备注", name: "T_remarks", type: "text", options: "{width:465}", width: 469, initValue: obj.Remarks },
                                    ],
                                    [
                                        { display: "状态", name: "T_private", type: "select", options: "{width:180,data:[{id:0,text:'私客'},{id:1,text:'公客'}],selectBoxHeight:50,value:'" + obj.isPrivate + "'}", validate: "{required:true}" },
                                        { display: "业务员", name: "T_employee", validate: "{required:true}" }
                                    ]
                                ]
                            }
                        ]
                    });
                    $("#btn_map").ligerButton({ text: "地图", width: 60, click: map })
                    $("#T_xy").val(Trim(obj.xy, "g"));
                    $('#T_employee').ligerComboBox({ width: 180, onBeforeOpen: f_selectContact });
                    $("#T_customer").attr("validate", "{ required: true, remote: remote, messages: { required: '请输入客户名', remote: '此客户已存在!' } }");

                    if (obj.emp_id) {
                        $("#T_employee_val").val(obj.emp_id);
                        $("#T_employee").val("【" + obj.department + "】" + obj.employee);
                    }
                    else {
                        $.getJSON("hr_employee.form.xhd?id=epu&rnd=" + Math.random(), function (result) {
                            var obj = eval(result);
                            for (var n in obj) {
                                if (obj[n] == null)
                                    obj[n] = "";
                            }
                            $("#T_employee_val").val(obj.id);
                            $("#T_employee").val("【" + obj.dep_name + "】" + obj.name);
                        })
                    }
                    //$('#T_City').val(obj.City);
                    //$('#T_City_val').val(obj.City_id);

                    //$('#T_Provinces').val(obj.Provinces);
                    //$('#T_Provinces_val').val(obj.Provinces_id);
                    //obj.isPrivate && $("#T_private").ligerGetComboBoxManager().selectValue(obj.isPrivate);

                    b = $('#T_City').ligerComboBox({
                        width: 180,
                        url: "Sys_City.combo.xhd?provincesid=" + obj.Provinces_id + "&rnd=" + Math.random(),
                        value: obj.City_id,
                        emptyText: '（空）'
                    });
                    c = $('#T_Provinces').ligerComboBox({
                        width: 180,
                        value: obj.Provinces_id,
                        url: "Sys_Provinces.combo.xhd?rnd=" + Math.random(),
                        onBeforeSelect: function (newvalue) {
                            //if (!newvalue)
                            //    newvalue = -1;
                            //$.get("Sys_City.combo.xhd?provincesid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                            //    b.setData(eval(data));
                            //});
                            url= "Sys_City.combo.xhd?provincesid=" + newvalue + "&rnd=" + Math.random(),
                            b.setUrl(url);
                        }, emptyText: '（空）'
                    });

                }
            });
        }
        function f_selectContact() {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '选择员工', width: 850, height: 400, url: "hr/Getemp_Auth.aspx?auth=1", buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择员工!');
                return;
            }

            $("#T_employee").val("【" + data.dep_name + "】" + data.name);
            $("#T_employee_val").val(data.id);

            dialog.close();
        }

        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }
        function remote() {
            var url = "CRM_Customer.validate.xhd?type=cus&T_cid=" + getparastr("cid") + "&rnd=" + Math.random();
            return url;
        }
        function map() {
            var url = "crm/customer/map_mark.aspx?type=mark&xy=" + $("#T_xy").val();
            f_openWindow(url, "客户地图标记【" + $("#T_customer").val() + "】", 800, 500, savemap, 9003);
        }
        function savemap(item, dialog) {
            var xy = dialog.frame.f_save();
            if (xy) {
                $("#T_xy").val(xy);
                dialog.close();
            }
        }

    </script>
</head>
<body style="">
    <div style="padding: 0px 10px;">
        <form id="form1" onsubmit="return false">
            <input type="hidden" id="T_xy" name="T_xy" />
        </form>
    </div>
</body>
</html>
