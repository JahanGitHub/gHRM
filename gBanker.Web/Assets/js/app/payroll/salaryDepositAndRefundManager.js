
var salaryDepositAndRefundTypeEnum = {
    DepositRequired: "DR",
    RefundRequired: "RR",
    Deposited: "D",
    Refunded: "R"
}

var salaryDepositAndRefundManager = {

    getSalaryDepositAndRefund: function () {
        var type = $('#SalaryDepositAndRefundType').val();
        var salaryYear = $('#SalaryYear').val();
        var salaryMonth = $('#SalaryMonth').val();

        var startDate = $('#StartDate').val();
        var endDate = $('#EndDate').val();

        if (type == salaryDepositAndRefundTypeEnum.DepositRequired || type == salaryDepositAndRefundTypeEnum.RefundRequired) {
            if (!salaryYear) {
                $.alert.open('Success', "Salary Year is Required"); return;
            }
            if (!salaryMonth) {
                $.alert.open('Success', "Salary Month is Required"); return;
            }
        }

        if (!startDate) {
            $.alert.open('Success', "Start Date is Required"); return;
        }
        if (!endDate) {
            $.alert.open('Success', "End Date is Required"); return;
        }

        if (!type) {
            $.alert.open('Success', "Deposit & Refund type is Required"); return;
        }

        //-----Get Requirede Deposit Info----------------
        if (type == salaryDepositAndRefundTypeEnum.DepositRequired) {
            salaryDepositAndRefundManager.GetRequiredeDepositInfo(salaryYear, salaryMonth);
            $('#gridDepositRequired').show();
            $("#gridRefundRequired").hide();
            $('#gridDeposited').hide();
            $('#gridRefunded').hide();
        }

        if (type == salaryDepositAndRefundTypeEnum.RefundRequired) {
            salaryDepositAndRefundManager.GetRequiredRefundInfo(salaryYear, salaryMonth);
            $('#gridDepositRequired').hide();
            $("#gridRefundRequired").show();
            $('#gridDeposited').hide();
            $('#gridRefunded').hide();
        }
        if (type == salaryDepositAndRefundTypeEnum.Deposited) {
            salaryDepositAndRefundManager.GetEmployeeDepositedInfo();
            $('#gridDeposited').show();
            $('#gridDepositRequired').hide();
            $("#gridRefundRequired").hide();
            $('#gridRefunded').hide();
        };
        if (type == salaryDepositAndRefundTypeEnum.Refunded) {
            salaryDepositAndRefundManager.GetEmployeeRefundedInfo();
            $('#gridRefunded').show();
            $('#gridDeposited').hide();
            $('#gridDepositRequired').hide();
            $("#gridRefundRequired").hide();
        }
    },

    initDatePicker: function () {
        $("#StartDate").datepicker(
        {
            dateFormat: "dd-M-yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2050"

        });

        $("#EndDate").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true,
                yearRange: "1920:2050"
            });
    },

    GetRequiredeDepositInfo: function (salaryYear, salaryMonth) {
        $("#gridDepositRequired").html('');

        var fromDate = $('#StartDate').val();
        var toDate = $('#EndDate').val();
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
                    url: '/PRDeposit/GetRequiredDepositInfo',
                    dataType: 'json',
                    data: {
                        FilterColumn: filterColumn, FilterValue: filterValue,
                        salaryYear: salaryYear, salaryMonth: salaryMonth,
                        fromDate: fromDate, toDate: toDate
                    }
                }
            }
        });

        $("#gridDepositRequired").kendoGrid({
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
                     field: "PRComponentId",
                     hidden: true,
                     filterable: false,
                     locked: false
                 },
                 {
                     field: "EmployeeId",
                     hidden: true,
                     filterable: false,
                     locked: false
                 },
                {
                    field: "EmployeeCode",
                    title: "Employee Code",
                    width: "120px",
                    filterable: true,
                    locked: true
                },
                 {
                     field: "EmployeeName",
                     title: "Employee Name",
                     width: "130px",
                     filterable: true,
                     locked: true
                 },
                 {
                     width: "140px",
                     field: "EmployeeType",
                     title: "EmployeeTypeId",
                     hidden: true,
                 },
                 {
                     width: "140px",
                     field: "EmployeeTypeName",
                     title: "Employee Type",
                     locked: true
                 },
                 {
                     width: "135px",
                     field: "EmployeeStatus",
                     title: "EmployeeStatusId",
                     hidden: true
                 },
                 {
                     width: "135px",
                     field: "EmployeeStatusName",
                     title: "Employee Status",
                     locked: true
                 },
                 {
                     width: "120px",
                     field: "DepositeType",
                     title: "Deposit Type",
                     locked: false
                 },
                 {
                     width: "120px",
                     field: "GrossSalary",
                     title: "Gross Salary",
                     locked: false
                 },
                 {
                     width: "135px",
                     field: "NoOfSalaryDays",
                     title: "No of Salary Days",
                     locked: false
                 },
                 {
                     width: "140px",
                     field: "DepositeAmount",
                     title: "Deposit Amount",
                     locked: false
                 },
                 {
                     width: "130px",
                     field: "TransactionType",
                     title: "Transaction Type",
                     locked: false
                 },
                {
                    width: "140px",
                    field: "ComponentGroup",
                    title: "Component Group",
                    locked: false
                },

                {
                    width: "140px",
                    field: "ComponentName",
                    title: "Component Name",
                    locked: false
                },
                 {
                     width: "50px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";
                         btn += '<div class="text-center"><a href="#" OnClick="ApproveDepositInfo( ' + data.PRComponentId + ',' + "'" + data.EmployeeCode + "'" + ',' + "'" + data.DepositeAmount + "'" + ',' + data.EmployeeId + ',' + "'" + data.TransactionType + "'" + ',' + "'" + data.ComponentGroup + "'" + ',' + "'" + data.EmployeeName + "'" + ',' + "'" + data.ComponentName + "'" + ',' + "'" + data.EmployeeType + "'" + ',' + "'" + data.EmployeeStatus + "'" + ',' + data.NoOfSalaryDays + ',' + data.OfficeLocationId + ',' + data.GrossSalary + ');"><i class="fa fa-thumbs-up"></i></a></div>';
                         return btn;
                     },
                     locked: false
                 },
            ]
        });
    },

    GetRequiredRefundInfo: function (salaryYear, salaryMonth) {
        $("#gridRefundRequired").html('');

        var fromDate = $('#StartDate').val();
        var toDate = $('#EndDate').val();
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
                    url: '/PRDeposit/GetRequiredRefundInfo',
                    dataType: 'json',
                    data: {
                        FilterColumn: filterColumn, FilterValue: filterValue,
                        salaryYear: salaryYear, salaryMonth: salaryMonth,
                        fromDate: fromDate, toDate: toDate
                    }
                }
            }
        });

        $("#gridRefundRequired").kendoGrid({
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
                     field: "EmployeeId",
                     hidden: true,
                     filterable: false,
                     //locked: true
                 },
                 {
                     field: "EmployeeCode",
                     title: "Employee Code",
                     width: "120px",
                     filterable: true,
                     //locked: true
                 },
                 {
                     field: "EmployeeName",
                     title: "Employee Name",
                     width: "130px",
                     filterable: true,
                     //locked: true
                 },
                 {
                     width: "140px",
                     field: "EmployeeType",
                     title: "EmployeeTypeId",
                     hidden: true,
                 },
                 {
                     width: "140px",
                     field: "EmployeeTypeName",
                     title: "Employee Type",
                     //locked: true
                 },
                 {
                     width: "135px",
                     field: "EmployeeStatusId",
                     title: "EmployeeStatus",
                     hidden: true
                 },
                 {
                     width: "135px",
                     field: "EmployeeStatusName",
                     title: "Employee Status",
                     //locked: true
                 },
                 {
                     width: "120px",
                     field: "GrossSalary",
                     title: "Gross Salary",
                     locked: false
                 },
                 {
                     width: "135px",
                     field: "RefundDays",
                     title: "Refund Days",
                     locked: false
                 },
                 {
                     width: "140px",
                     field: "RefundAmount",
                     title: "Refund Amount",
                     locked: false
                 },
                 {
                     width: "50px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";
                         btn += '<div class="text-center"><a href="#" OnClick="ApproveRefundInfo( ' + "'" + data.EmployeeCode + "'" + ',' + "'" + data.RefundAmount + "'" + ',' + data.EmployeeId + ',' + "'" + data.EmployeeName + "'" + ',' + "'" + data.EmployeeType + "'" + ',' + "'" + data.EmployeeStatusId + "'" + ',' + data.RefundDays + ',' + data.OfficeLocationId + ');"><i class="fa fa-thumbs-up"></i></a></div>';
                         return btn;
                     },
                     locked: false
                 },
            ]
        });
    },

    GetEmployeeDepositedInfo: function () {
        $("#gridDeposited").html('');

        var fromDate = $('#StartDate').val();
        var toDate = $('#EndDate').val();
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
                    url: '/PRDeposit/GetDepositedInfo',
                    dataType: 'json',
                    data: {
                        FilterColumn: filterColumn, FilterValue: filterValue,
                        fromDate: fromDate, toDate: toDate
                    }
                }
            }
        });

        $("#gridDeposited").kendoGrid({
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
                     field: "EmployeeCode",
                     title: "Employee Code",
                     width: "120px",
                     filterable: true
                 },
                 {
                     field: "EmployeeName",
                     title: "Employee Name",
                     width: "130px",
                     filterable: true
                 },
                 {
                     width: "140px",
                     field: "NoOfSalaryDays",
                     title: "No. of Salary Days",

                 },
                 {
                     width: "140px",
                     field: "DepositAmount",
                     title: "Deposit Amount",
                 }
            ]
        });
    },

    GetEmployeeRefundedInfo: function () {
        $("#gridRefunded").html('');

        var fromDate = $('#StartDate').val();
        var toDate = $('#EndDate').val();
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
                    url: '/PRDeposit/GetRefundInfo',
                    dataType: 'json',
                    data: {
                        FilterColumn: filterColumn, FilterValue: filterValue,
                        fromDate: fromDate, toDate: toDate
                    }
                }
            }
        });

        $("#gridRefunded").kendoGrid({
            dataSource: dataSource,            
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
                     field: "EmployeeCode",
                     title: "Employee Code",
                     width: "120px",
                     filterable: true
                 },
                 {
                     field: "EmployeeName",
                     title: "Employee Name",
                     width: "130px",
                     filterable: true
                 },
                 {
                     width: "140px",
                     field: "NoOfSalaryDays",
                     title: "No. of Salary Days",

                 },
                 {
                     width: "140px",
                     field: "DepositAmount",
                     title: "Refund Amount",
                 }
            ]
        });
    }
}

$(document).ready(function () {

    salaryDepositAndRefundManager.initDatePicker();

    $('#salary-depositand-refund-form').on('submit', function (event) {
        event.preventDefault();

        var form = $('#salary-depositand-refund-form');

        //for form validation
        var isValid = app.validateForm(form);
        if (!isValid) return;

        //get Salary Deposit And Refund
        salaryDepositAndRefundManager.getSalaryDepositAndRefund();
    })
});

function ApproveDepositInfo(PRComponentId, EmployeeCode, DepositeAmount, EmployeeId,
    TransactionType, ComponentGroup, EmployeeName, ComponentName, EmployeeType, EmployeeStatus, NoOfSalaryDays, OfficeLocationId, GrossSalary) {

    var salaryYear = $('#SalaryYear').val();
    var salaryMonth = $('#SalaryMonth').val();

    var obj = {
        PRComponentId: PRComponentId,
        EmployeeCode: EmployeeCode,
        DepositeAmount: DepositeAmount,
        GrossSalary: GrossSalary,
        EmployeeId: EmployeeId,
        TransactionType: TransactionType,
        ComponentGroup: ComponentGroup,
        EmployeeName: EmployeeName,
        ComponentName: ComponentName,
        EmployeeType: EmployeeType,
        EmployeeStatusId: EmployeeStatus,
        NoOfSalaryDays: NoOfSalaryDays,
        SalaryYear: salaryYear,
        SalaryMonth: salaryMonth,
        OfficeLocationId: OfficeLocationId
    }
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: '/PRDeposit/ApproveDepositInfo',
        data: JSON.stringify({ obj: obj }),
        dataType: 'json',
        async: true,
        success: function (data) {
            if (data.result == 1) {
                $(".panel-body .create-success").show(800).fadeToggle(3000);
                $.alert.open('Success', data.message);
                salaryDepositAndRefundManager.GetRequiredeDepositInfo();
            } else {
                $.alert.open('Error', data.message);
            }
        },
    });
}

function ApproveRefundInfo(EmployeeCode, RefundAmount, EmployeeId, EmployeeName, EmployeeType, EmployeeStatus, RefundDays, OfficeLocationId, GrossSalary) {

    var salaryYear = $('#SalaryYear').val();
    var salaryMonth = $('#SalaryMonth').val();
    if (!salaryYear) {
        $.alert.open('Error', "Salary Year not found!");
    }

    if (!salaryMonth) {
        $.alert.open('Error', "Salary Month not found!");
    }

    var obj = {
        //PRComponentId: PRComponentId,
        EmployeeCode: EmployeeCode,
        RefundAmount: RefundAmount,
        EmployeeId: EmployeeId,
        //TransactionType: TransactionType,
        //ComponentGroup: ComponentGroup,
        EmployeeName: EmployeeName,
        //ComponentName: ComponentName,
        EmployeeType: EmployeeType,
        EmployeeStatusId: EmployeeStatus,
        RefundDays: RefundDays,
        SalaryYear: salaryYear,
        SalaryMonth: salaryMonth,
        OfficeLocationId: OfficeLocationId
    }
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: '/PRDeposit/ApproveRefundInfo',
        data: JSON.stringify({ obj: obj }),
        dataType: 'json',
        async: true,
        success: function (data) {
            if (data.result == 1) {
                $(".panel-body .create-success").show(800).fadeToggle(3000);
                $.alert.open('Success', data.message);
                salaryDepositAndRefundManager.GetRequiredRefundInfo();
            } else {
                $.alert.open('Error', data.message);
            }
        },
    });
}
