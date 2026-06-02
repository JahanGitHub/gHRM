
function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function SaveLoanCollection(loanId, employeeId, amount, loanInstallment, interestInstallment, interestCharge, comment) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFLoanCollection/SaveLoanCollection',
        data: { loanId: loanId, employeeId: employeeId, amount: amount, loanInstallment: loanInstallment, interestInstallment: interestInstallment, interestCharge: interestCharge, comment: comment },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            clear();
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Data Not Saved");
        }
    });
}

function clear() {
    $("input[type=text]:visible:enabled").first().focus();

    $('#EmployeeCode').val('');

    $('#LoanTypeList').empty();
    $('#EmployeeId').val('');
    $('#Comment').val('');
    $('#LoanInstallment').val('');
    $('#InterestInstallment').val('');
    $('#LoanId').val('');

    $('#EmployeeName').val('');
    $('#Amount').val('');
    $('#LoanInstallment').val('');
    $('#InterestInstallment').val('');
    $('#InterestCharge').val('');


    //New
    $('#DisburseAmount').val('');
    $('#IntersetRate').val('');
    $('#NoOfInstallment').val('');

    $('#MonthlyInstallment').val('');
    $('#LoanPaid').val('');
    $('#InterestPaid').val('');

    $('#PrincipalDue').val('');
    $('#InterestDue').val('');
    $('#TotalDue').val('');
    $('#DisburseDate').val('');
}

function ClearOnLoanTypeChange() {
    $('#LoanInstallment').val('');
    $('#InterestInstallment').val('');
    $('#LoanId').val('');

    $('#Amount').val('');
    $('#LoanInstallment').val('');
    $('#InterestInstallment').val('');
    $('#InterestCharge').val('');
}

function ClearOnEmployeeChange() {
    $('#EmployeeId').val('');
    $('#LoanTypeList').val('');
    $('#LoanInstallment').val('');
    $('#InterestInstallment').val('');
    $('#LoanId').val('');

    $('#EmployeeName').val('');
    $('#Amount').val('');
    $('#LoanInstallment').val('');
    $('#InterestInstallment').val('');
    $('#InterestCharge').val('');
}

function ClearOnAmountChange() {
    $('#LoanInstallment').val('');
    $('#InterestInstallment').val('');
    $('#InterestCharge').val('');
}

function GetEmployeeByEmployeeCode(employeeCode) {
    var employeeId = '';
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Employee/GetEmployeeByEmployeeCode',
        data: { employeeCode: employeeCode },
        dataType: 'json',
        async: false,
        success: function (data) {

            $('#EmployeeId').val(data.EmployeeId);
            $('#EmployeeName').val(data.EmployeeName);

            employeeId = data.EmployeeId;
        },
        error: function (request, status, error) {
        }
    });
    return employeeId;
}

function GetEmployeeByLoanType(loanTypeId, employeeId) {

    var employeeName = '';
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFLoanCollection/GetEmployeeByLoanType',
        data: { loanTypeId: loanTypeId, employeeId: employeeId },
        dataType: 'json',
        async: false,
        success: function (data) {
            employeeName = data.EmployeeName;
            $('#LoanId').val(data.LoanId);
            if (data.status == 'nok') {
                alert(data.message);
            }
        },
        error: function (request, status, error) {
        }
    });
    return employeeName;
}

function GetDisburseAndTodaysCollectionDetail(loanId) {

    var employeeName = '';
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFLoanCollection/GetDisburseAndTodaysCollectionDetail',
        data: { loanId: loanId },
        dataType: 'json',
        async: false,
        success: function (result) {

            $('#DisburseAmount').val(result.data.DisburseAmount);
            $('#IntersetRate').val(result.data.IntersetRate);
            $('#NoOfInstallment').val(result.data.NoOfInstallment);

            $('#MonthlyInstallment').val(result.data.MonthlyInstallment);
            $('#LoanPaid').val(result.data.LoanPaid);
            $('#InterestPaid').val(result.data.InterestPaid);

            $('#PrincipalDue').val(result.data.PrincipalDue);
            $('#InterestDue').val(result.data.InterestDue);
            $('#TotalDue').val(result.data.TotalDue);
            $('#DisburseDate').val(result.data.DisburseDate);
        },
        error: function (request, status, error) {
        }
    });
    return employeeName;
}

function GetLoanInfoByLoanId(loanId, amount) {
    var result = true;
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFLoanCollection/GetLoanInfoByLoanId',
        data: { loanId: loanId, amount: amount },
        dataType: 'json',
        async: false,
        success: function (data) {
            $('#LoanInstallment').val(data.LoanInstallment);
            $('#InterestInstallment').val(data.InterestInstallment);
            $('#InterestCharge').val(data.InterestCharge);
        },
        error: function (request, status, error) {
        }
    });
    return result;
}

function DisableAll(flag) {
    $('#EmployeeId').prop('disabled', flag);
    $('#LoanTypeList').prop('disabled', flag);
    $('#Amount').prop('disabled', flag);
    $('#btnSaveLoanCollection').prop('disabled', flag);
    $('#btnReset').prop('disabled', flag);
}

function Add() {
    var loanId = $("#LoanId").val();
    var loanTypeId = $("#LoanTypeList option:selected").val();
    var employeeId = $("#EmployeeId").val();
    var employeeName = $("#EmployeeName").val();
    var amount = $("#Amount").val();
    var loanInstallment = $("#LoanInstallment").val();
    var interestInstallment = $("#InterestInstallment").val();
    var interestCharge = $("#InterestCharge").val();

    //New
    var comment = $("#Comment").val();
    //debugger;

    if (loanId == '' || loanTypeId == '' || employeeId == '' || employeeName == '' || amount == '' || loanInstallment == '' || interestInstallment == '' || interestCharge == '') {
        alert('Enter valid information.');
        return;
    }

    if (parseFloat(amount) <= 0) {
        alert('Enter more than zero (0) as Amount');
        return;
    }
    SaveLoanCollection(loanId, employeeId, amount, loanInstallment, interestInstallment, interestCharge, comment);

}

function isUndefinedOrNull(val) {
    return (typeof val === 'undefined' || val === undefined || val === null);
}

function generateDropdown(selector, url, selectedValue, defaultText, defaultValue) {

    $(selector).empty();
    if (!isUndefinedOrNull(defaultValue))
        $(selector).append('<option value="' + defaultValue + '">' + defaultText + '</option>');
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: 'json',
        async: false,
        success: function (data) {
            $.each(data, function (index, option) {
                if (option.Value == selectedValue)
                    $(selector).append('<option value="' + option.Value + '" ' + " selected='selected' " + '>' + option.Text + '</option>');
                else
                    $(selector).append('<option value="' + option.Value + '">' + option.Text + '</option>');
            });
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

$(function () {

    $("input[type=text]:visible:enabled").first().focus();

    //Day Status
    if ($("#IsOpen").val()) {
        DisableAll(false);
    }
    else {
        DisableAll(true);
    }

    $('#Amount').keypress(function (event) {
        return isNumber(event, this)
    });

    //Added today [27.02.2018]
    $('#EmployeeCode').change(function () {
        ClearOnEmployeeChange();
        var employeeCode = $('#EmployeeCode').val();

        if (employeeCode == '') {
            return;
        }

        var employeeId = GetEmployeeByEmployeeCode(employeeCode);

        if (employeeId == '' || employeeId == undefined) {
            alert('Does not exist');
            $('#EmployeeCode').focus();
            return;
        }

        var url = '';
        var selectedValue = '';
        url = '/PFLoanCollection/GetUnpaidLoanTypeByEmployeeId?employeeId=' + employeeId;
        selectedValue = "";
        generateDropdown("#LoanTypeList", url, selectedValue, "Select Loan Type", "");
    });

    $('#LoanTypeList').change(function () {
        ClearOnLoanTypeChange();

        var loanTypeId = $('#LoanTypeList option:selected').val();
        if (loanTypeId == '') {
            $('#LoanTypeList').focus();
            return;
        }

        var employeeId = $('#EmployeeId').val();
        if (employeeId == '') {
            alert('Enter valid Employee Id');
            $('#EmployeeId').focus();
            return;
        }

        var employeeName = GetEmployeeByLoanType(loanTypeId, employeeId)
        if (employeeName == '' || employeeName == undefined) {
            return;
        }
        $('#EmployeeName').val(employeeName);

        GetDisburseAndTodaysCollectionDetail($('#LoanId').val());
    });

    $('#Amount').change(function () {
        ClearOnAmountChange();

        var amount = $('#Amount').val();
        if (amount == '') {
            $('#Amount').focus();
            return;
        }

        if (parseFloat(amount) <= 0) {
            alert('Enter valid amount');
            $('#Amount').val('');
            $('#Amount').focus();
            return;
        }

        var employeeId = $('#EmployeeId').val();

        if (employeeId == '') {
            alert('Select employee id');
            $('#EmployeeId').focus();
            return;
        }

        var loanTypeId = $('#LoanTypeList option:selected').val();
        if (loanTypeId == '') {
            alert('Select loan Type');
            $('#LoanTypeList').focus();
            return;
        }

        var loanId = $('#LoanId').val();

        if (loanId == '') {
            alert('Enter valid data.');
            return;
        }
        var result = GetLoanInfoByLoanId(loanId, amount);
    });

    $("#btnReset").click(function () {
        clear();
    });

    $("#btnSaveLoanCollection").click(function () {

        $.alert.open('confirm', 'Are you sure you want to add?', function (button) {
            if (button == 'yes') {
                Add();
                return true;
            }
            else {
                return false;
            }
        });
    });

});