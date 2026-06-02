
function IsFinalized(employeeId) {

    var isFinalized;

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFWithdrawan/IsFinalized',
        data: { employeeId: employeeId },
        dataType: 'json',
        async: false,
        success: function (data) {
            isFinalized = data.isFinalized;
        },
        error: function (request, status, error) {
        }
    });
    return isFinalized;
}

function Withdraw(employeeId, calculationDate) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFWithdrawan/Withdraw',
        data: { employeeId: employeeId, calculationDate: calculationDate },
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

//Old : delete after workable
function WithdrawanPF(employeeId, selfContribution, orgContribution, selfInterestAmount, orgInterestAmount) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFWithdrawan/WithdrawanPF',
        data: { employeeId: employeeId, selfContribution: selfContribution, orgContribution: orgContribution, selfInterestAmount: selfInterestAmount, orgInterestAmount: orgInterestAmount },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            clear();
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Data Not Saved");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
}

function GetEmployeeNameByEmpId(employeeId) {
    var employeeName = '';
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Employee/GetEmployeeNameByEmpId',
        data: { employeeId: employeeId },
        dataType: 'json',
        async: false,
        success: function (data) {
            employeeName = data.EmployeeName;
        },
        error: function (request, status, error) {
        }
    });
    return employeeName;
}

//New
function GetWithdrawnInfo(employeeId, calculationDate) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFWithdrawan/GetWithdrawnInfo',
        data: { employeeId: employeeId, calculationDate: calculationDate },
        dataType: 'json',
        async: false,
        success: function (data) {

            if (data.message == '') {

                $('#SelfContribution').val(data.model.SelfContribution);
                $('#OrgContribution').val(data.model.OrgContribution);
                $('#Contribution').val(data.model.Contribution);

                $('#SelfInterestUptoInterim').val(data.model.SelfInterestUptoInterim);
                $('#OrgInterestUptoInterim').val(data.model.OrgInterestUptoInterim);
                $('#InterestUptoInterim').val(data.model.InterestUptoInterim);

                $('#SelfInterestAftInterim').val(data.model.SelfInterestAftInterim);
                $('#OrgInterestAftInterim').val(data.model.OrgInterestAftInterim);
                $('#InterestAftInterim').val(data.model.InterestAftInterim);

                $('#PrincipalBalance').val(data.model.PrincipalBalance);
                $('#InterestBalance').val(data.model.InterestBalance);
                $('#OutStanding').val(data.model.OutStanding);

                $('#InterestIncome').val(data.model.InterestIncome);
                $('#Fund').val(data.model.Fund);
                $('#Payable').val(data.model.Payable);

                $('#btnWithdrawanPF').prop('disabled', false);
            }
            else {
                alert(data.message);
                $('#btnWithdrawanPF').prop('disabled', true);
            }
        },
        error: function (request, status, error) {
        }
    });
}

//Old
function GetPFWithdrawanInfo(employeeId) {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFWithdrawan/GetPFWithdrawanInfo',
        data: { employeeId: employeeId },
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data.message == '') {

                $('#SelfContribution').val(data.SelfContribution);
                $('#OrgContribution').val(data.OrgContribution);

                $('#SelfInterestAmount').val(data.SelfInterestAmount);
                $('#OrgInterestAmount').val(data.OrgInterestAmount);
                $('#TotalPayable').val(data.TotalPayable);
                $('#LoanDue').val(data.LoanDue);
                $('#btnWithdrawanPF').prop('disabled', false);
            }
            else {
                alert(data.message);
                $('#btnWithdrawanPF').prop('disabled', true);
            }
        },
        error: function (request, status, error) {
        }
    });
}

function clear() {
    $("#EmployeeCode").val('');
    $("#EmployeeId").val('');
    $("#EmployeeName").val('');
    $("#TotalPayable").val('');
    $("#LoanDue").val('');
    $("#SelfContribution").val('');
    $("#OrgContribution").val('');
    $("#SelfInterestAmount").val('');
    $("#OrgInterestAmount").val('');
    $("#SelfInterestUptoInterim").val('');
    $("#OrgInterestUptoInterim").val('');
    $("#InterestUptoInterim").val('');
    $("#InterestIncome").val('');
    $("#OutStanding").val('');

    $("#PrincipalBalance").val('');
    $("#InterestBalance").val('');
    $("#Fund").val('');
    $("#Payable").val('');
    $("#Contribution").val('');
}

function ClearOnEmployeeChange() {
    $("#EmployeeId").val('');
    $("#EmployeeName").val('');
    $("#TotalPayable").val('');
    $("#LoanDue").val('');
    $("#SelfContribution").val('');
    $("#OrgContribution").val('');
    $("#SelfInterestAmount").val('');
    $("#OrgInterestAmount").val('');
}

function DisableAll(flag) {
    $("#EmployeeCode").prop('disabled', flag);
    $("#btnReset").prop('disabled', flag);
}

function Add() {

    var employeeId = $("#EmployeeId").val();
    var calculationDate = $("#CalculationDate").val();

    if (employeeId == '' || calculationDate == '') {
        $.alert.open("Enter valid Information");
        return;
    }

    var isFinalized = IsFinalized(employeeId);
    if (isFinalized == true) {
        $.alert.open("Info", "Already Finalized");
        return;
    }
    if (isFinalized == 'undefined') {
        $.alert.open("Info", "Sorry for inconvenience! please try again later");
        return;
    }
            
    Withdraw(employeeId, calculationDate);
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

$(document).ready(function () {

    $("#CalculationDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: '1980:2050'
    });

    $('#btnSearch').prop('disabled', true);
    $('#btnWithdrawanPF').prop('disabled', true);

    if ($('#IsOpen').val() === 'True') {
        DisableAll(false);
    }
    else {
        DisableAll(true);
    }

    $('#btnSearch').click(function () {
        //ClearOnEmployeeChange();

        var employeeCode = $('#EmployeeCode').val();
        if (employeeCode == '') {
            $.alert.open("Info", "Enter Employee Code");
            $('#EmployeeCode').focus();
            return;
        }

        var employeeId = $('#EmployeeId').val();
        if (employeeId == '') {
            $.alert.open("Info", "Enter Valid Employee Code");
            $('#EmployeeCode').focus();
            return;
        }

        var calculationDate = $('#CalculationDate').val();
        if (calculationDate == '') {
            $.alert.open("Info", "Enter Calculation Date");
            $('#CalculationDate').focus();
            return;
        }

        GetWithdrawnInfo(employeeId, calculationDate);
    });

    $('#EmployeeCode').change(function () {
        var employeeCode = $('#EmployeeCode').val();
        if (employeeCode == '') {
            $.alert.open("Info", "Enter Employee Code");
            $('#EmployeeCode').focus();
            return;
        }
        var employeeId = GetEmployeeByEmployeeCode(employeeCode);
        if (employeeId == '' || employeeId == 'undefined') {
            $.alert.open("Info", "Employee does not esist");
            $('#EmployeeCode').val('');
            $('#EmployeeCode').focus();
            return;
        }

        var employeeName = GetEmployeeNameByEmpId(employeeId);
        if (employeeName == '' || employeeName == 'undefined') {
            $.alert.open("Info", "Unable to fetch Employee information");
            return;
        }
        $('#EmployeeName').val(employeeName);


        //New Block
        var isFinalized = IsFinalized(employeeId);
        //alert(isFinalized);
        if (isFinalized == true) {
            $.alert.open("Info", "Already Finalized");

            $('#btnSearch').prop('disabled', true);

            $('#EmployeeId').val('');
            $('#EmployeeCode').val('');
            $('#EmployeeName').val('');
            $('#EmployeeCode').focus();
            return;
        }
        if (isFinalized == 'undefined') {
            $.alert.open("Info", "Sorry for inconvenience! please try again later");
            $('#btnSearch').prop('disabled', true);
            $('#EmployeeId').val('');
            $('#EmployeeCode').val('');
            $('#EmployeeName').val('');
            $('#EmployeeCode').focus();
            return;
        }
        if (isFinalized == false) {
            $('#btnSearch').prop('disabled', false);
        }

    });

    $("#btnReset").click(function () { // Reset form
        clear();
    });

    $("#btnWithdrawanPF").click(function () {
        $.alert.open('confirm', 'Are you sure you want to withdraw?', function (button) {
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