
function ProcessProfitDistribution(distributionYear, transDate) {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFProfitDistributionProcess/ProcessProfitDistribution',
        data: { distributionYear: distributionYear, transDate: transDate },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            if (data.status == 'ok' && data.IsProcessed == true) {
                $('#btnProcessProfitDistribution').prop('disabled', true);
            }
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Unable to Process");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
}

function clear() {
    $('#ProfitRate').val('');
    $('#DistributionYear').val('');
    $('#TransactionDate').val('');
}

function Distribute() {
    var distributionYear = $('#DistributionYear').val();
    var transDate = $('#TransactionDate').val();
    if (transDate == '') {
        alert('Check Transaction date');
        return;
    }

    ProcessProfitDistribution(distributionYear, transDate);
}

$(document).ready(function () {
    $("#TransactionDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "",
        changeYear: true
    });
    var isValidDistribution = $('#IsValidDistribution').val();

    if (isValidDistribution) {
        $('#btnProcessProfitDistribution').prop('disabled', false);       
    }
    else {
        $('#btnProcessProfitDistribution').prop('disabled', true);
    }

    $("#chkAll").change(function () {
        var ischecked = $(this).is(':checked');
        $("input:checkbox[name='chk']").prop('checked', ischecked)
    });

    $("#btnView").click(function (e) {
        e.preventDefault();
        var data = GenerateAjaxRequist('/PFProfitDistributionProcess/GetEmployeeWisePFDistribution', { declarationId: $("#DeclararionId").val() }, "GET");
        if (data.msg == "") {
            var htm = "",total=0;
            $.each(data.obj, function (i, item) {
                total += item.ProfitContribution;
                htm += `<tr><td><input type="checkbox" name="chk"/><input type="hidden" name="EmployeeId" value="${item.EmployeeId}"/></td><td>${item.EmployeeCode}</td><td>${item.EmployeeName}</td><td style="text-align:right">${item.TotalContribution}</td><td style="text-align:right">${item.ProfitContribution}</td></tr>`;
            });
            $("#tbContribution tbody").html(htm);
            $("#tfd").text(total);
            $("#btnProcessProfitDistribution").show()
        }
        else 
            $.alert.open("Message", data.msg);
    });
    $("#btnProcessProfitDistribution").click(function (e) {
        e.preventDefault();
        var arr = new Array();
        $.each($("#tbContribution tbody tr"), function () {
            if ($(this).closest("tr").find("td").eq(0).find("input:checkbox[name='chk']").is(':checked')) {
                arr.push({
                    EmployeeId: $(this).closest("tr").find("td").eq(0).find("input:hidden[name='EmployeeId']").val(),
                    TotalContribution: $(this).closest("tr").find("td").eq(3).text(),
                    ProfitContribution: $(this).closest("tr").find("td").eq(4).text(),

                });
            }
        });
        if (arr.length > 0) {
            var data = GenerateAjaxRequist('/PFProfitDistributionProcess/PostEmployeeDistributionData', JSON.stringify( { model: arr, transactionDate: $("#TransactionDate").val(), declarationId: $("#DeclararionId").val() }), "POST");
            $.alert.open("Message", data.Message);
            $("#tbContribution tbody").html('');
            $("#tfd").text('');
        } else $.alert.open("Message", "Data not found");
        
    });
});
