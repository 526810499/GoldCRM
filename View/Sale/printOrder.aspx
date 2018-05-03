<!DOCTYPE html>
<html>
<head>
    <script src="../lib/jquery/jquery-1.11.3.min.js"></script>
    <script src="../JS/pringAreas/jquery.PrintArea.js"></script>
    <script src="../JS/XHD.js"></script>
    <meta charset="utf-8" />
    <script type="text/javascript">
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
                            datas2 += "<td  colspan='4'><div style='padding-top:10px'>销售人：" + $("#yyy").text() + "&nbsp;&nbsp;收银员：" + $("#syy").text() + "</div></td></tr>";

                            datas2 += "</table>";
                            $("#cw").append(datas2);
                            $("#other").append(datas2);
                        }
                    }
                });
            }

        });



    </script>
</head>
<body>

    <div id="divprint">
        <div>
            <div style="padding-top: 12px; padding-left: 120px" id="cuser"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="oid"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="oday"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="otime"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="onumber"></div>

            <div style="padding-top: 80px; padding-left: 40px; height: 210px" id="odetail">
            </div>

            <div style="padding-left: 120px; float: left;" id="yyy"></div>
            <div style="float: right; padding-right: 150px;" id="atotla">0</div>

            <div style="padding-left: 120px; padding-top: 20px;" id="syy"></div>
            <div style="padding-top: 5px; padding-left: 120px;" id="jl">&nbsp; </div>
            <div style="padding-top: 5px; padding-left: 120px;" id="dwjg">&nbsp; </div>
            <div style="padding-top: 5px; padding-bottom: 10px; padding-left: 120px;" id="fkfs">现金</div>
        </div>
        <div style="height: 280px" id="cw">
        </div>
        <div id="other">
        </div>

    </div>


    <script>
        $(document).ready(function () {
            $("#divprint").printArea();
            window.setTimeout(function () {
                window.close();
            }, 1000);
        });

    </script>
</body>
</html>
