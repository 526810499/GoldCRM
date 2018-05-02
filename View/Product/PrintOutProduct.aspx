<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>
<html>
<head>
    <script src="../lib/jquery/jquery-1.11.3.min.js"></script>
    <script src="../JS/pringAreas/jquery.PrintArea.js"></script>
    <script src="../JS/XHD.js"></script>
    <meta charset="utf-8" />
 
    <script>
        $(function () {

            $.ajax({
                type: "get",
                url: "Product_out.ExportPrint.xhd", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', outids: getparastr("outids", ""), rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result == null) { return; }
                    var obj = eval(result);
                    var datas = "";
                    var count = 0;
                    var CostsTotal = 0;
                    var Totals = 0;
                    var Weight = 0;
                    $(result).each(function (i, v) {
                        count++;
                        CostsTotal += v.CostsTotal;
                        Totals += v.Totals;
                        Weight += v.Weight;
                        datas += "<tr> ";
                        datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + (i + 1) + "</td>";
                        datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + v.product_name + "</td>";
                        datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + toMoney(v.Weight) + "</td>";
                        datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + toMoney(v.CostsTotal) + "</td>";
                        datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">1</td>";
                        datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + v.BarCode + "</td>";
                        datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + toMoney(v.CostsTotal) + "</td>";
                        datas += "</tr> ";
                    });

                    datas += "<tr> ";
                    datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\"></td>";
                    datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">合计：</td>";
                    datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + toMoney(Weight) + "</td>";
                    datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\"></td>";
                    datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + count + "</td>";
                    datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\"></td>";
                    datas += "<td style=\"border: solid #999;border-width: 0 1px 1px 0;padding: 2px;\">" + toMoney(CostsTotal) + "</td>";
                    datas += "</tr> ";
                    $("#stime").text(formatTime(getparastr("stime", "")));
                    $("#sorderid").text(getparastr("outids", ""));
                    $("#tbody").append(datas);

                    $("#divprint").printArea();
                    window.setTimeout(function () {
                        window.close();
                    }, 1000);
                }
            });
        });

    </script>
</head>
<body>

    <div id="divprint">
        <div style="text-align: center">
            <b>货品出库单</b>
        </div>
        <div>
            时间：<span id="stime" style="padding-right:50px"></span>  单号：<span id="sorderid"></span>
        </div>
        <div>
            <table style=" border-collapse: collapse;border: solid #999; border-width: 1px 0 0 1px;">
                <thead>
                    <th style="border: solid #999;border-width: 0 1px 1px 0;padding: 2px;">序号</th>
                    <th style="border: solid #999;border-width: 0 1px 1px 0;padding: 2px;">产品</th>
                    <th style="border: solid #999;border-width: 0 1px 1px 0;padding: 2px;">克重</th>
                    <th style="border: solid #999;border-width: 0 1px 1px 0;padding: 2px;">工费</th>
                    <th style="border: solid #999;border-width: 0 1px 1px 0;padding: 2px;">数量</th>
                    <th style="border: solid #999;border-width: 0 1px 1px 0;padding: 2px;">条形码</th>
                    <th style="border: solid #999;border-width: 0 1px 1px 0;padding: 2px;">工费小计</th>
                </thead>
                <tbody id="tbody">
                </tbody>
            </table>
        </div>

    </div>


 
</body>
</html>
