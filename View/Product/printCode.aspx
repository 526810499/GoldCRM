<!DOCTYPE html>
<html>
<head>
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="chrome=1" />
    <script src="../lib/jquery/jquery-1.11.3.min.js"></script>
    <script src="../JS/pringAreas/jquery.PrintArea.js"></script>
    <script src="../JS/XHD.js"></script>
    <meta charset="utf-8" />
    <script type="text/javascript">
        $(function () {

            var id = getparastr("id", "061be71f-5395-4c90-8b03-f023a78cb3d9")

            loadForm();

            function loadForm() {
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
                            $(detail).each(function (i, v) {
                                datas += "<tr>";
                                datas += "<td style='width:130px'>" + v.BarCode + "</td>";
                                datas += "<td style='width:265px;padding-left:20px'>" + v.product_name + "</td>";
                                datas += "<td style='width:110px'>1</td>";
                                datas += "<td style='width:150px'>" + toMoney(v.amount) + "</td>";

                                datas += "</tr>";
                            });
                            datas += "</table>";
                            $("#odetail").append(datas);
                            $("#cw").append(datas);
                            $("#other").append(datas);
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
            <div style="padding-top: 15px; padding-left: 120px" id="cuser"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="oid"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="oday"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="otime"></div>
            <div style="padding-top: 5px; padding-left: 120px" id="onumber"></div>

            <div style="padding-top: 80px; padding-left: 40px; height: 215px" id="odetail">
            </div>

            <div style="padding-left: 120px; float: left;" id="yyy"></div>
            <div style="float: right; padding-right: 150px;" id="atotla">0</div>

            <div style="padding-left: 120px; padding-top: 20px;" id="syy"></div>
            <div style="padding-top: 5px; padding-left: 120px;" id="jl">&nbsp; </div>
            <div style="padding-top: 5px; padding-left: 120px;" id="dwjg">&nbsp; </div>
            <div style="padding-top: 5px; padding-bottom: 10px; padding-left: 120px;" id="fkfs">ож╫П</div>
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
