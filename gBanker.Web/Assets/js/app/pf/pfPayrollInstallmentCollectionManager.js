
function clearPrSalaryStatus() {
    $('#isProcessed').val('');
    $('#selfContribution').val('');
    $('#selfContributor').val('');
    $('#orgContribution').val('');
    $('#orgContributor').val('');
    $('#loanAmount').val('');
    $('#loanee').val('');
}

function VerifyPayrollInstallment(monthId, year) {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFPayrollInstallmentCollection/VerifyPayrollInstallment',
        data: { monthId: monthId, year: year },
        dataType: 'json',
        async: true,
        success: function (data) {
            $('#selfContribution').val(data.selfContribution);
            $('#selfContributor').val(data.selfContributor);
            $('#orgContribution').val(data.orgContribution);
            $('#orgContributor').val(data.orgContributor);
            $('#loanAmount').val(data.loanAmount);
            $('#loanee').val(data.loanee);

            if (data.isProcessed == true) {
                $.alert.open("Message", "PF Contribution Deducted, You can Process");
                $('#isProcessed').val("PF Contribution Deducted");
                $('#isProcessed').css("color", "green");
            }
            else {
                $.alert.open("Message", "PF Contribution has not been Deducted");
                $('#isProcessed').val("PF Contribution has not been Deducted");
                $('#isProcessed').css("color", "red");
            }

        },
        error: function (request, status, error) {
            $.alert.open("Message", "Unable to Process");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
}

function SavePayrollInstallment(monthId, year) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFPayrollInstallmentCollection/SavePayrollInstallment',
        data: { monthId: monthId, year: year },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
            clear();
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Unable to Process");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
}

function clear() {
    $('#MonthList').val('');
    $('#YearList').val('');
}

$(document).ready(function () {

    if ($('#IsOpen').val() ==='True') {
        $("#btnSavePayrollInstallment").prop('disabled', false);
        $("#btnReset").prop('disabled', false);
        $("#YearList").prop('disabled', false);
        $("#MonthList").prop('disabled', false);
    } else {
        $("#btnSavePayrollInstallment").prop('disabled', true);
        $("#btnReset").prop('disabled', true);
        $("#YearList").prop('disabled', true);
        $("#MonthList").prop('disabled', true);
    }

    //btnVerify
    $("#btnVerify").click(function () {
        clearPrSalaryStatus();

        var monthId = $('#MonthList option:selected').val();
        var year = $('#YearList option:selected').text();
        if (monthId == '') {
            alert('Select Month');
            return;
        }
        if (year == '') {
            alert('Select Year');
            return;
        }
        //clear();

        $.alert.open('confirm', 'Are you sure you want to Verify Payroll Installment?', function (button) {
            if (button == 'yes') {
                VerifyPayrollInstallment(monthId, year);
                return true;
            }
            else {
                return false;
            }
        });
    });

    $("#btnSavePayrollInstallment").click(function () {
        clearPrSalaryStatus();

        var monthId = $('#MonthList option:selected').val();
        var year = $('#YearList option:selected').text();
        if (monthId == '') {
            alert('Select Month');
            return;
        }
        if (year == '') {
            alert('Select Year');
            return;
        }

        $.alert.open('confirm', 'Are you sure you want to Retrieve Contribution and Loan Installment?', function (button) {
            if (button == 'yes') {
                SavePayrollInstallment(monthId, year);
                return true;
            }
            else {
                return false;
            }
        });
    });

    //From Index
    $('#grid2').jtable({
        title: 'Payroll Installment Collection Log',
        paging: true,
        pageSize: 5,
        sorting: false,
        defaultSorting: 'Name ASC',
        actions: {
            listAction: '/PFPayrollInstallmentCollection/GetPayrollInstallmentLogList'
        },
        fields: {
            ProcessId: {
                key: true,
                list: false,
                create: false,
                edit: false
            },
            Year: {
                title: 'Year',
                width: '30%'
            },
            Month: {
                title: 'Month',
                width: '30%'
            },
            IsProcessed: {
                title: 'Processed',
                width: '30%',
                display: function (data) {
                    if (data.record.IsProcessed == true)
                        return "Yes";
                    if (data.record.IsProcessed == false)
                        return "No";
                }
            },
            CDate: {
                title: 'Create Date',
                width: '10%'
            }
        }
    });
    reloadGrid2();
    $("#filterColumn").change(function () {
        if ($(this).val() === "ViewAll") {
            $("#filterValue").val('');
        }
    });
    function reloadGrid2() {
        $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
    }

});
