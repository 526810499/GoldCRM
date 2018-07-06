<!DOCTYPE html>
<html>
<head>
    <script src="../lib/jquery/jquery-1.11.3.min.js"></script>
    <script src="../JS/pringAreas/jquery.PrintArea.js"></script>
    <script src="../JS/XHD.js"></script>
    <meta charset="utf-8" />
    <style media="print">
        @page {
            size: auto; /* auto is the initial value */
            margin: 0mm; /* this affects the margin in the printer settings */
        }
    </style>
    <script type="text/javascript">



        // 设置页眉页脚为空
        function PageSetup_Null() {
            try {
                var hkey_root, hkey_path, hkey_key
                hkey_root = "HKEY_CURRENT_USER"
                hkey_path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";

                var RegWsh = new ActiveXObject("WScript.Shell");
                hkey_key = "header";
                RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
                hkey_key = "footer";
                RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
            }
            catch (e) { }
        }

        $(function () {

            var id = getparastr("id", "")

            loadForm();

            function loadForm() {
                if (id == "") { alert("参数为空"); return false; }
                $.ajax({
                    type: "get",
                    url: "Sale_order.form.xhd",
                    data: { id: id, rnd: Math.random() },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        if (result != null) {
                            $("#oid").text(result.Serialnumber);
                            $("#cuser").text((result.cus_name));
                            $("#oday").text(formatTimebytype(result.create_time, "yyyy-MM-dd"));
                            $("#otime").text(formatTimebytype(result.create_time, "yyyy-MM-dd hh:mm:ss"));
                            $("#semp_name").text(result.emp_name);
                            $("#fkfs").text(result.pay_type);
                            $("#yyy").text(result.emp_name);
                            $("#syy").text(result.cashiername);
                            $("#atotla").text(toMoney(result.total_amount));
                            LoadDetail(result)
                        }
                    }
                });
            }

            function LoadDetail(data) {
                $.ajax({
                    type: "get",
                    url: "Sale_order_details.grid.xhd",
                    data: { orderid: id, rnd: Math.random() },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        if (result != null && result.Rows.length > 0) {
                            var detail = result.Rows;
                            var datas = "<table>";
                            var datas2 = "";
                            $(detail).each(function (i, v) {
                                datas += "<tr>";
                                datas += "<td style='width:130px'>" + v.BarCode + "</td>";
                                datas += "<td style='width:265px;padding-left:20px'>" + v.product_name + "</td>";
                                datas += "<td style='width:110px'>1</td>";
                                datas += "<td style='width:150px'>" + toMoney(v.amount) + "</td>";

                                datas += "</tr>";
                            });
                            datas2 = datas;
                            datas += "</table>";
                            $("#odetail").append(datas);

                            datas2 += "<tr>";
                            datas2 += "<td  colspan='4'><div style='padding-top:10px'>销售人：" + $("#yyy").text() + "<span style='padding-left:30px'> </span>总金额：" + $("#atotla").text() + "<span style='padding-left:30px'> </span>收银员：" + $("#syy").text() + "</div></td></tr>";

                            datas2 += "</table>";
                            $("#other1").append(datas2);
                            $("#other2").append(datas2);
                        }
                    }
                });
            }

        });



    </script>
</head>
<body>

    <div id="divprint">

        <table style="height: 896px">
            <tr style="height: 196px; padding-left: 60px">
                <td style="height: 196px; padding-left: 60px">
                    <div id="other1">
                    </div>
                </td>
            </tr>
            <tr style="height: 196px; padding-left: 60px">
                <td style="height: 196px; padding-left: 60px">
                    <div id="other2">
                    </div>
                </td>
            </tr>
            <tr style="height: 85px; padding-left: 90px">
                <td style="width: 550px; padding-left: 90px">
                    <table style="height: 85px; padding-left: 100px">
                        <tr style="height: 15px">
                            <td colspan="2">
                                <div id="fkfs">现金</div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td colspan="2">
                                <div id="dwjg"></div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td colspan="2">
                                <div id="jl"></div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td colspan="2">
                                <div id="syy"></div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td style="width: 160px">
                                <div id="yyy"></div>
                            </td>
                            <td style="width: 350px; text-align: right; padding-bottom: 10px">
                                <div id="atotla">0</div>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr style="height: 168px; padding-left: 56px">
                <td style="padding-left: 56px">
                    <table>
                        <tr style="height: 8px">
                            <td></td>
                        </tr>
                        <tr style="height: 160px">
                            <td>
                                <div id="odetail">
                                </div>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr style="height: 56px">
                <td></td>
            </tr>

            <tr style="height: 82px; padding-left: 90px">
                <td style="padding-left: 100px">
                    <table>
                        <tr style="height: 15px">
                            <td>
                                <div id="onumber"></div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td>
                                <div id="otime"></div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td>
                                <div id="oday"></div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td>
                                <div id="oid"></div>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td>
                                <div id="cuser"></div>
                            </td>
                        </tr>
                    </table>

                </td>

            </tr>
        </table>

    </div>


    <script>
        $(document).ready(function () {
            PageSetup_Null();
            $("#divprint").printArea();
            PageSetup_Null();
            window.setTimeout(function () {
                window.close();
            }, 1000);
        });

    </script>
</body>
</html>
