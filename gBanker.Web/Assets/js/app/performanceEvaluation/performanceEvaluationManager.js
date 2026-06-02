
var performanceEvaluationManager = {
    clearform: function () {
        $("#EvaluationYear").val("");
        $("#EvaluationMonth").val("");
        $("#TotalSamity").val("0");
        $("#TotalMember").val("0");
        $("#TotalLoanee").val("0");
        $("#OSP").val("0.00");
        $("#SpecialSavings").val("0.00");
        $("#GeneralSavings").val("0.00");
        $("#LoanDisburse").val("0.00");
        $("#LoanRepaid").val("0.00");
        $("#LoanOutstanding").val("0.00");
        $("#CurrentDueNo").val("");
        $("#CurrentDue").val("");
        $("#OverDueNo").val("");
        $("#OverDue").val("");       
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/PerformanceEvaluation/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            performanceEvaluationManager.loadPerformanceEvaluationListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            performanceEvaluationManager.clearform();
                        } else {
                            $.alert.open("Error", data.message);
                        }
                    },
                    error: function (request, status, error) {
                        alert(request.statusText + "/" + request.statusText + "/" + error);
                    }
                });
                return true;
            }
            else {
                hiddenField.value = 'false';
                return false;
            }
        });
    },

    populateEditableInfo: function (id) {
        $(".input-validation-error").removeClass("input-validation-error");
        if (!id) {
            performanceEvaluationManager.clearform();
            return;
        }

        $.ajax({
            url: '/PerformanceEvaluation/GetPerformanceEvaluation/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {

            if (!result.isSuccess) {
                performanceEvaluationManager.clearform();
                return;
            }

            $("#DeductionAmount").val(result.data.DeductionAmount);
            $("#IsPercentage").prop('checked', result.data.IsPercentage);
        });
    },

    loadPerformanceEvaluationListing: function () {

        var year = $('#Year').val();
        var month = $('#Month').val();
        var employeeCode = $('#SearchTerm').val();

        var filterColumn = $("#filterColumn").val();
        var filterValue = $("#filterValue").val();
        if (filterColumn != "" && filterValue == "") {
            $.alert.open("Error", "Please Provide Filter Value");
            return false;
        }

        var dataSource = new kendo.data.DataSource({
            type: "aspnetmvc-ajax",
            pageSize: 25,
            schema: {
                data: "data", // records are returned in the "data" field of the response
                total: "total" // total number of records is in the "total" field of the response
            },
            serverPaging: true,   // enable server paging
            serverSorting: true,
            serverFiltering: true,
            transport: {
                read: {
                    url: '/PerformanceEvaluation/GetPerformanceEvaluationListing',
                    dataType: 'json',
                    data: {
                        year: year, month: month, employeeCode: employeeCode,
                        FilterColumn: filterColumn,
                        FilterValue: filterValue
                    }
                }
            }
        });
        $("#grid").kendoGrid({
            dataSource: dataSource,
            //height: 600,
            groupable: false,
            reorderable: true,
            filterable: true,
            sortable: true,

            selectable: false,
            resizable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            columns: [
                {
                    field: "PerformanceEvaluationId",
                    hidden: true,
                    filterable: false
                },
                {
                    field: "EvaluationYear",
                    title: "Year",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "EvaluationMonthInText",
                    title: "Month",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "EvaluationOn",
                    title: "Evaluation On",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "EmployeeCode",
                    title: "Employee Code",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "EmployeeName",
                    title: "Employee Name",
                    width: "50px",
                    filterable: true
                },

                {
                    field: "TotalSamity",
                    title: "Total Samity",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "TotalLoanee",
                    title: "Total Loanee",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "SavingsTotals",
                    title: "SavingsTotals",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "LoanTotals",
                    title: "Loan Totals",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "DueTotals",
                    title: "Due Totals",
                    width: "50px",
                    filterable: true
                },
                {
                    width: "30px",
                    title: 'Action',
                    template: function (data) {
                        var btn = "";
                        btn += '<div class="text-center" style="float:left;"><a href="/PerformanceEvaluation/manage?performanceEvaluationId=' + data.PerformanceEvaluationId + '&employeecode=' + data.EmployeeCode + '"><i class="fa fa-pencil-square-o"></i></a></div>';
                        btn += '<div class="text-center"><a href="#" OnClick="performanceEvaluationManager.informationDelete(' + data.PerformanceEvaluationId + ');"><i class="fa fa-trash-o"></i></a></div>';
                        return btn;
                    }
                },
            ]
        });
    },
    loadEmployeeInformationByCode: function (employeeCode) {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PerformanceEvaluation/GetEmployeeInfoByEmployeeCode',
            data: { employeeCode: employeeCode },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (response) {
                if (response.type == "success") {
                    console.log(response);
                    // populate employee information
                    var imployeeInfo = response.employeeInfo;
                    if (imployeeInfo) {
                        $("#EmployeeId").val(imployeeInfo.EmployeeId);
                        $("#EmployeeName").val(imployeeInfo.EmployeeName);
                        $("#EmployeeEmployeeStatus").val(imployeeInfo.EmployeeEmployeeStatus);
                        $("#EmployeeDesignationStatus").val(imployeeInfo.EmployeeDesignationStatus);
                        $("#EmployeeDepartment").val(imployeeInfo.EmployeeDepartment);

                    }
                    debugger;
                    var officeInfo = response.officeInfo;
                    if (imployeeInfo) {

                        $("#OfficeTypeId").val(officeInfo.OfficeTypeId).trigger('change');
                        $("#PVHeadOfficeId").val(officeInfo.PVHeadOfficeId).trigger('change');
                        $("#PVProjectId").val(officeInfo.PVProjectId).trigger('change');
                        $("#ZoneId").val(officeInfo.ZoneId).trigger('change');
                        $("#AreaId").val(officeInfo.AreaId).trigger('change');
                        $("#UnitId").val(officeInfo.UnitId);

                    }
                    else {
                        //clear employee information
                        performanceEvaluationManager.clearEmployeeInfo();
                    }
                    return;
                }
                $.alert.open("Error", response.message);
            },
            error: function (request, status, error) {
                $.alert.open("Error", "There was an error while fatching employee information. Please try again!");
            }
        });
    },
    clearEmployeeInfo: function () {
        $("#EmployeeId").val('');
        $("#EmployeeName").val('');
        $("#EmployeeEmployeeStatus").val('');
        $("#EmployeeDesignationStatus").val('');
        $("#EmployeeDepartment").val('');
    },
    calculateLoanOutstanding: function () {
        var loanOutstanding; 
        var loanDisburse = $("#LoanDisburse").val();
        var loanRepaid = $("#LoanRepaid").val();        
        loanOutstanding = parseFloat(loanDisburse) - parseFloat(loanRepaid);
       
        $("#LoanOutstanding").val(loanOutstanding);
    }
}

$(document).ready(function () {

    var employeeCode = $('#EmployeeCode').val();

    if (employeeCode && employeeCode.length > 0) {
        //load employee basic info
        performanceEvaluationManager.loadEmployeeInformationByCode(employeeCode);
    }

    $('#search-evaluation-listing-form').on('submit', function (event) {
        event.preventDefault();

        //for form validation
        var isValid = app.validateForm('#search-evaluation-listing-form');
        if (!isValid) return;

        //get listing    
        performanceEvaluationManager.loadPerformanceEvaluationListing();
    });

    $("#EmployeeCode").blur(function (e) {        
        var employeeCode = $("#EmployeeCode").val();
        if (employeeCode == '' || employeeCode == null || employeeCode == 0) {
            //clear employee information
            performanceEvaluationManager.clearEmployeeInfo();
            $.alert.open("Error", "Please enter valid employee code.");
        }

        performanceEvaluationManager.loadEmployeeInformationByCode(employeeCode);
    });

    //submit to change userrole
    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();

        var form = $(this);

        //for form validation
        var isValid = app.validateForm('#add-or-edit-form');
        if (!isValid) return;

        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                $('#PerformanceEvaluationId').val(response.performanceEvaluationId);
                performanceEvaluationManager.clearform();
                //success alert
                $.alert.open("Success", response.message);
            }
            else {
                $('#PerformanceEvaluationId').val("0");
                $.alert.open("Error", response.message);
            }
        });

    });

    $('.validateText').keyup(function () {
        this.value = this.value.replace(/[^0-9\.\-]/g, '');
    });   

    //$("#LoanDisburse").keyup(function () {
    //    performanceEvaluationManager.calculateLoanOutstanding();
    //});
    //$("#LoanRepaid").keyup(function () {
    //    performanceEvaluationManager.calculateLoanOutstanding();
    //});
});
