
function LoadEmpInfo(employeeCode) {
    var ddl = $("#LoanTypeId");
    ddl.html('');
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/EmployeeLoan/GetEmployeeInfoByCode',
        data: { employeeCode: employeeCode },
        dataType: 'json',
        async: false,
        success: function (data) {//klm
            if (data.result != 0) {
                $("#EmployeeId").val(data.data.EmployeeId);
                $("#EmployeeName").val(data.data.EmployeeName);
                $("#OfficeName").val(data.data.OfficeName);
                $("#DepartmentName").val(data.data.DepartmentName);
                $("#DesignationName").val(data.data.DesignationName);

                $.each(data.data.LoanComponentList, function (id, option) {
                    ddl.append($('<option></option>').val(option.Value).html(option.Text));
                });

            } else {
                $.alert.open("Error", "Sorry invalid Employee Code. Please check again");
            }
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}
function ClearForm() {
    $("#LoanTypeId").val("");
    $("#EmployeeCode").val("");
    $("#EmployeeName").val("");
    $("#OfficeName").val("");
    $("#DepartmentName").val("");
    $("#DesignationName").val('');
    $("#InstallmentAmount").val(0.00);
    $("#TotalLoanAmt").val(0.00);
    $("#LoanStartDate").datepicker('setDate', new Date());
    $("#InstallmentDate").datepicker('setDate', new Date());
    $("#LoanEndDate").datepicker('setDate', new Date());
    $("#EmployeeId").val("");
}

var employeeLoanCreateManager = {
    populateNoOfInstallment: function () {
        var totalLoanAmount = $('#TotalLoanAmt').val();
        var installmentAmount = $('#InstallmentAmount').val();
        if (totalLoanAmount <= 0 || installmentAmount <= 0) {
            $('#NoOfInstallMent').val(0);
        }

        var noOfInstallment = (parseFloat(totalLoanAmount) / parseFloat(installmentAmount)).toFixed(2);

        $('#NoOfInstallMent').val(noOfInstallment);
    },
    populateLoanEndDate: function (installmentDate) {
        var noOfInstallment = $('#NoOfInstallMent').val();
        $("#LoanEndDateMsg").val('');
        if (noOfInstallment && parseInt(noOfInstallment) > 0) {
            var insFormattedDate = new Date(MakeDate(installmentDate));
            var loanEndDate = insFormattedDate.setMonth(insFormattedDate.getMonth() + parseInt(noOfInstallment)-1);
            var formattedLoanEndDate = new Date(loanEndDate);
            var loanLastDate = new Date(formattedLoanEndDate.getFullYear(), formattedLoanEndDate.getMonth() + 1, 0);
            var loanEndDateMsg = DateConversionToLongDate(loanLastDate);
            $("#LoanEndDate").val(loanEndDateMsg);
        }
    }
}

$(document).ready(function () {

    //populate no of installment
    employeeLoanCreateManager.populateNoOfInstallment();

    $('#InstallmentAmount,#TotalLoanAmt').on('blur', function () {
        //populate no of installment
        employeeLoanCreateManager.populateNoOfInstallment();
    })

    $("#btnSave").click(function () {
        var prComponentId = $("#LoanTypeId").val();
        var installmentAmount = $("#InstallmentAmount").val();
        var totalLoanAmt = $("#TotalLoanAmt").val();
        var loanStartDate = $("#LoanStartDate").val();
        var installmentDate = $("#InstallmentDate").val();
        var loanEndDate = $("#LoanEndDate").val();
        var employeeId = $("#EmployeeId").val();
        if (prComponentId > 0 && installmentAmount > 0 && totalLoanAmt > 0 && loanStartDate != "" && employeeId > 0 && installmentDate != "" && loanEndDate != "") {
            var newObj = {
                EmployeeId: employeeId,
                PRComponentId: prComponentId,
                TotalAmount: totalLoanAmt,
                LoanStartDate: loanStartDate,
                InstallmentDate: installmentDate,
                LoanEndDate: loanEndDate,
                InstallmentAmount: installmentAmount
            };
            if (newObj != null) {
                $('#AjaxLoader').show();
                $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    url: '/EmployeeLoan/SaveEmployeeShortLoanInfo',
                    data: JSON.stringify({ LoanObject: newObj }),
                    dataType: 'json',
                    async: false,
                    success: function (data) {//klm
                        $('#AjaxLoader').hide();
                        if (data.result === 1) {
                            ClearForm();
                            $.alert.open("Success", data.message);
                        } else {
                            $.alert.open("Error", data.message);
                        }
                    },
                    error: function (request, status, error) {
                        alert(request.statusText + "/" + request.statusText + "/" + error);
                    }
                });
            }

        } else {
            $.alert.open("Error", "Please provide all required information");
        }
    });

    $("#EmployeeCode").blur(function () {
        var employeeCode = $("#EmployeeCode").val();
        if (employeeCode != "") {
            LoadEmpInfo(employeeCode);
        }

    });
    $("#LoanStartDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true,
        //minDate:new Date()
    });
    $("#LoanStartDate").datepicker('setDate', new Date());//
    $("#LoanEndDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true,
        //minDate: new Date()
    });
    $("#LoanEndDate").datepicker('setDate', new Date());
    $("#InstallmentDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true,
        onSelect: function (installmentDate) {
            employeeLoanCreateManager.populateLoanEndDate(installmentDate);
        }
    });
    $("#InstallmentDate").datepicker('setDate', new Date());
});