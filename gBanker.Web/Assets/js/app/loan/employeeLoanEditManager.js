
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

function LoadEmpInfoEdit(LoanId, employeeCode) {
    var ddl = $("#LoanTypeId");
    ddl.html('');
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/EmployeeLoan/GetEmployeeInfoByCodeEdit',
        data: { LoanId: LoanId, employeeCode: employeeCode },
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

var employeeLoanEditManager = {
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
            $("#LoanEndDateMsg").val(loanEndDateMsg);
        }
    }
}

$(document).ready(function () {
    var employeeCode = $('#EmployeeCode').val();
    var LoanId = $("#LoanId").val();
    if (LoanId > 0 ) {
        if (employeeCode != "") {
            LoadEmpInfoEdit(LoanId, employeeCode );
            $("#LoanTypeId").val($('#HiddenLoanTypeId').val());
        }
    }
    else {
        if (employeeCode != "") {
            LoadEmpInfo(employeeCode);
            $("#LoanTypeId").val($('#HiddenLoanTypeId').val());
        }
    }

    $("#btnSave").click(function () {
        var prComponentId = $("#LoanTypeId").val();
        var installmentAmount = $("#InstallmentAmount").val();
        var totalLoanAmt = $("#TotalLoanAmt").val();
        var loanStartDate = $("#LoanStartDateMsg").val();
        var installmentDate = $("#InstallmentDateMsg").val();
        var loanEndDate = $("#LoanEndDateMsg").val();
        var employeeId = $("#EmployeeId").val();
        var loanId = $("#LoanId").val();
        var loanStatus = $("#LoanStatus").val();
        if (loanId > 0 && prComponentId > 0 && installmentAmount > 0 && totalLoanAmt > 0 && loanStartDate != "" && employeeId > 0 && installmentDate != "" && loanEndDate != "" && loanStatus!=="") {
            var newObj = {
                LoanId: loanId,
                EmployeeId: employeeId,
                PRComponentId: prComponentId,
                TotalAmount: totalLoanAmt,
                LoanStartDate: loanStartDate,
                InstallmentDate: installmentDate,
                LoanEndDate: loanEndDate,
                InstallmentAmount: installmentAmount,
                LoanStatus: loanStatus,
            };
            if (newObj != null) {
                $('#AjaxLoader').show();
                $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    url: '/EmployeeLoan/EditEmployeeShortLoanInfo',
                    data: JSON.stringify({ LoanObject: newObj }),
                    dataType: 'json',
                    async: false,
                    success: function (data) {//klm
                        $('#AjaxLoader').hide();
                        if (data.result == 1) {

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

    //populate no of installment
    employeeLoanEditManager.populateNoOfInstallment();

    $('#InstallmentAmount,#TotalLoanAmt').on('blur', function () {
        //populate no of installment
        employeeLoanEditManager.populateNoOfInstallment();
    })
        
    $("#LoanStartDateMsg").datepicker(
   {
       dateFormat: "dd-M-yy",
       showAnim: "scale",
       changeMonth: true,
       yearRange: "1980:2050",
       changeYear: true,
       //minDate:new Date()
   });

    $("#LoanEndDateMsg").datepicker( {
       dateFormat: "dd-M-yy",
       showAnim: "scale",
       changeMonth: true,
       yearRange: "1980:2050",
       changeYear: true,
       //minDate: new Date()
   });

    $("#InstallmentDateMsg").datepicker({
      dateFormat: "dd-M-yy",
      showAnim: "scale",
      changeMonth: true,
      yearRange: "1980:2050",
      changeYear: true,
      //minDate: new Date()
      onSelect: function (installmentDate) {
          employeeLoanEditManager.populateLoanEndDate(installmentDate);
      }
  });

});