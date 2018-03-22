<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/Gray2014/css/all.css" rel="stylesheet" />
    <link href="../CSS/webuploader.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>

    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../JS/XHD.js" type="text/javascript"></script>
    <script src="../JS/webuploader/webuploader.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        var contract_id = null;
        var dialog = frameElement.dialog;

        $(function () {
            $.metadata.setType("attr", "validate");
            XHD.validate($(form1));

            loadForm(getparastr("id"));
        });

        function f_save() {
            if ($(form1).valid()) {
                

                var sendtxt = "&id=" + contract_id;
                sendtxt += "&customer_id=" + getparastr("customer_id");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "CRM_contract.form.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
           
                    if (typeof (obj.Pay_cycle) == "undefined")
                        obj.Pay_cycle = 1;
                    
                    contract_id = obj.id;

                    var rows = [];  

                    var customer_id = obj.Customer_id || getparastr("customer_id");
                    if (!customer_id)
                        rows.push([{ display: "客户", name: "T_customer", validate: "{required:true}", width: 465 }])

                    rows.push(
                        [{ display: "合同名称", name: "T_name", type: "text", options: "{width:465}", validate: "{required:true}", width: 465, initValue: obj.Contract_name }],
                        [
                            { display: "合同编号", name: "T_no", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.Serialnumber },
                            { display: "合同金额", name: "T_amount", type: "text", options: "{width:180,onChangeValue:function(){ getAmount(); }}", validate: "{required:true}", initValue: toMoney(obj.Contract_amount) }
                        ],
                        [
                            { display: "付款期数", name: "T_Pay_cycle", type: "spinner", options: "{width:180,type:'int',value:'" + obj.Pay_cycle + "'}", validate: "{required:true}" },
                            { display: "开始时间", name: "T_Start_date", type: "date", options: "{width:180}", validate: "{required:true}", initValue: formatTimebytype(obj.Start_date, "yyyy-MM-dd") }
                        ],
                        [
                            { display: "对方签约", name: "T_Customer_Contractor", type: "text", options: "{width:180}", validate: "{required:true}", initValue: obj.Customer_Contractor },
                            { display: "结束时间", name: "T_End_date", type: "date", options: "{width:180}", validate: "{required:true}", initValue: formatTimebytype(obj.End_date, "yyyy-MM-dd") }
                        ],
                        [
                            { display: "我方签约", name: "T_Our_Contractor", validate: "{required:true}" },
                            { display: "签订时间", name: "T_Sign_date", type: "date", options: "{width:180}", validate: "{required:true}", initValue: formatTimebytype(obj.Sign_date, "yyyy-MM-dd") }
                        ],
                        [
                            { display: "主要条款", name: "T_Main_Content", type: "textarea", cols: 77, rows: 4, width: 475, cssClass: "l-textarea", initValue: obj.Main_Content }
                        ],
                        [
                            { display: "备注", name: "T_Remarks", type: "textarea", cols: 77, rows: 2, width: 475, cssClass: "l-textarea", initValue: obj.Remarks }
                        ]
                    );

                    if (!obj.discount_amount)
                        obj.discount_amount = 0;

                    $("#form1").ligerAutoForm({
                        labelWidth: 80, inputWidth: 180, space: 20,
                        fields: [
                            {
                                display: '合同', type: 'group', icon: '',
                                rows: rows

                            }
                        ]
                    });
                    f_grid();

                    $("#T_customer").ligerComboBox({
                        width: 465,
                        onBeforeOpen: f_selectCustomer
                    })

                    $("#T_customer").val(obj.Customer_name);
                    $("#T_customer_val").val(obj.customer_id);


                    $("#T_Our_Contractor").ligerComboBox({
                        width: 180,
                        onBeforeOpen: f_selectEmp
                    })

                    $("#T_Our_Contractor").val(obj.Our_Contractor);
                    $("#T_Our_Contractor_val").val(obj.Our_Contractor_id);
                }
            });
        }
        function f_selectCustomer() {
            $.ligerDialog.open({
                zindex: 9005, title: '选择客户', width: 650, height: 300, url: '../crm/customer/getCustomer.aspx', buttons: [
                     { text: '确定', onclick: f_selectCustomerOK },
                     { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectCustomerOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }

            $("#T_customer").val(data.cus_name);
            $("#T_customer_val").val(data.id);
            dialog.close();
        }

        function f_selectEmp() {
            $.ligerDialog.open({
                zindex: 9005, title: '选择员工', width: 650, height: 300, url: '../hr/getemp_auth.aspx?auth=3', buttons: [
                     { text: '确定', onclick: f_selectEmpOK },
                     { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
            return false;
        }

        function f_selectEmpOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }

            $("#T_Our_Contractor").val(data.name);
            $("#T_Our_Contractor_val").val(data.id);
            dialog.close();
        }

        function getAmount() {
            var T_amount = $("#T_amount").val();

            $("#T_amount").val(toMoney(T_amount));
        }

        function f_grid() {
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '文件名', name: 'file_name', width: 300, align: 'left' },
                    {
                        display: '文件大小', name: 'file_size', width: 80, align: 'right', render: function (item, i) {
                            return formatUnits(item.file_size);
                        }
                    },
                    {
                        display: '上传进度', name: 'fileprogress', width: 150, render: function (item, i) {

                            if (item.fileprogress) {
                                var html = "<div style='background:#0f0;white-space:nowrap;";
                                html += "width:" + item.fileprogress + "%;'>";
                                html += item.fileprogress;
                                html += "%</div>";
                                return html;
                            }
                            return "";
                        }
                    },
                    {
                        display: '操作', width: 120, render: function (item,i) {
                            var div = "";


                            if (!item.filenew)
                                div += ' <a href="javascript:void(0)" onclick="down_atta(\'' + item.real_name + '\')">下载</a>    ';

                            div += ' <a href="javascript:void(0)" onclick="removefile(\'' + item.id + '\',\' ' + (item.filenew==1) + '\');" >删除</a>     ';
                            return div;
                        }
                    }                   

                ],
                allowHideColumn: false,
                rowid:'id',
                title: '附件',
                usePager: false,
                enabledEdit: true,
                url: "CRM_contract_atta.grid.xhd?contract_id=" + getparastr("id"),
                width: '100%',
                height: 'auto',
                heightDiff: -1,
                onLoaded: f_loaded
            });

        }

        function removefile(id, type) {
            console.log(typeof(type));
            if (type=="true") {
                uploader.removeFile(id);

                var manager = $("#maingrid4").ligerGetGridManager();
                var row = manager.getRow($("[growid = " + id + "]")[0]);
                
                //manager.updateCell("fileprogress",parseInt( Math.random().toFixed(2) * 100), row);
                //console.log(JSON.stringify(row))
                manager.deleteRow(row);               
            }
            else
            {
                //后台删除
                $.get("CRM_contract_atta.del.xhd?id=" + id, function (data, textStatus) {
                    var manager = $("#maingrid4").ligerGetGridManager();
                    var row = manager.getRow($("[growid = " + id + "]")[0]);

                    //console.log(JSON.stringify(row))
                    manager.deleteRow(row);
                });
            }
        }

        function down_atta(filename) {
            var url = "../../file/contract/" + getparastr("id") + "/" + filename;

            var a = filename.IsPicture()
            if (a) {
                window.open(url);
            }
            else
                $("#toexcel").attr("Action", url).submit();
        }

        function f_loaded() {
            if ($("#btn_add").length > 0)
                return;

            $(".l-panel-header").append("<div style='width:80px;float:right'><div id = 'btn_add' style='margin-top:2px;'>选择文件</div>");
            $(".l-grid-loading").fadeOut();

            $("#maingrid4").ligerGetGridManager()._onResize();

            f_uploader();
        }

        function startup()
        {
            uploader.upload();
        }

        var uploader;
        function f_uploader() {
            uploader = WebUploader.create({
                swf: "../js/webuploader/uploader.swf",
                server: "CRM_contract_atta.upload.xhd?contract_id=" + contract_id,
                pick: "#btn_add",
                resize: false
            });
            var manager = $("#maingrid4").ligerGetGridManager();
            uploader.on('fileQueued', function (file) {                
                var row = {
                    file_name: file.name,
                    file_size: file.size,
                    id: file.id,
                    filenew: 1,
                    fileprogress:0//Math.random().toFixed(2)*100
                }
                manager.addRow(row);
            });

            // 文件上传过程中创建进度条实时显示。
            uploader.on('uploadProgress', function (file, percentage) {
                var manager = $("#maingrid4").ligerGetGridManager();
                var row = manager.getRow($("[growid = " + file.id + "]")[0]);

                manager.updateCell("fileprogress", parseInt(percentage * 100), row);
            });

            uploader.on('uploadSuccess', function (file) {
            });

            uploader.on('uploadError', function (file) {
            });

            uploader.on('uploadFinished', function (file) {
                top.$.ligerDialog.closeWaitting();
                dialog.close();
            });
        }





    </script>

</head>
<body style="overflow-y: scroll;">
    <form id="form1" onsubmit="return false">
    </form>
    <div style="padding: 5px 4px 5px 2px;">
        <div id="thelist" class="uploader-list"></div>
        <div id="maingrid4">
        </div>
    </div>
    <form id='toexcel'></form>
</body>
</html>
