
var absentEmployeeManager = {
    loadLeaveTypeDropDown: function (dataItem) {
        var leaveTypeDropDownHtml = "<select style='width:100%;' class='k-textbox form-control' id='LeaveType" + dataItem.rowSl + "' onchange='CheckAvailableLeaveByType(\"" + dataItem.EmployeeId + "\"," + dataItem.rowSl + ",\"" + dataItem.StartDate + "\",\"" + dataItem.EndDate + "\")'>";
                
        $.each(dataItem.LeaveTypeList, function (i, v) {
            leaveTypeDropDownHtml = leaveTypeDropDownHtml + `<option value='${v.Value}'>${v.Text}</option>`;
        });

        leaveTypeDropDownHtml = leaveTypeDropDownHtml + "</select>";

        return leaveTypeDropDownHtml;
    }
}

$(document).ready(function () {

    $("#Year").val((new Date).getFullYear());

    $("#btnSearch").click(function () {

        var searching = `<i class="fa fa-circle-o-notch fa-spin fa-fw"></i> Searching..`;
        $('#btnSearch').html(searching)
        $('#btnSearch').attr('disabled', 'disabled');

        loadSearchData();
    })
});

function loadSearchData() {
    var month = $("#Month").val();
    if (month == '')
        month = 0;
    var year = $("#Year").val();

    var office_TypeId = $("#OfficeTypeId").val();
    if (office_TypeId == 1) {
        OfficeTypeId = office_TypeId;
        officeId = $("#PVHeadOfficeId").val();
    } else if (office_TypeId == 3) {
        OfficeTypeId = office_TypeId;
        officeId = $("#PVProjectId").val();
    } else if (office_TypeId == 4) {
        OfficeTypeId = office_TypeId;
        officeId = $("#ZoneId").val();
    } else if (office_TypeId == 5) {
        OfficeTypeId = office_TypeId;
        officeId = $("#AreaId").val();
    } else if (office_TypeId == 6) {
        OfficeTypeId = office_TypeId;
        officeId = $("#UnitId").val();
    } else if (office_TypeId == null || office_TypeId == '') {
        OfficeTypeId = '';
        officeId = ''
    };

    if (month != "" && year != "") {
        GetEmployeesAbsentInfo(month, year, OfficeTypeId, officeId);
    } else {
        //$.alert.open("Error", "Please provide all required fields");
        //return false;

        GetEmployeesAbsentInfo(month, year, OfficeTypeId, officeId);
    }
}

function btnPrintinput(EmployeeCode) {
    var month = $("#Month").val();
    var year = $("#Year").val();
    if (EmployeeCode != "") {
        url = '/AttendanceReport/EmployeeCodeReportBySingleCode?EmployeeCode=' + EmployeeCode + '&month=' + month + '&year=' + year;
        PrintReport(url);
    } else {
        $.alert.open("Error", "Please Select Date");
        return false;
    }
}

function PrintReport(printUrl) {
    window.open(printUrl, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}
function CheckAvailableLeaveByType(employeeId, rowSl, StartDate, EndDate) {

    var leaveType = $("#LeaveType" + rowSl).val();
    $.ajax({
        type: "GET",
        dataType: "json",
        async: false,
        cache: false,
        url: '/AttendancePenalty/CheckAvailableLeaveByType',
        data: { employeeId: employeeId, leaveTypeId: leaveType, StartDate: StartDate, EndDate: EndDate },
        contentType: "application/json; charset=utf-8",
        success: function (result) {

            if (!result) {
                $("#LeaveType" + rowSl).val('');
                $.alert.open("Error", "No of leave days overlaps available days");
                return false;
            }
        }
    });
}

function GetEmployeesAbsentInfo(month, year, OfficeTypeId, officeId) {
    $('#gridKendo').html("");
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
                url: '/AttendancePenalty/GetEmployeesAbsentInfo',
                dataType: 'json',
                data: { Month: month, Year: year, OfficeTypeId: OfficeTypeId, officeId: officeId }
            }
        }
    });

    var grid = $("#gridKendo").kendoGrid({     

        dataSource: dataSource,
        dataBound: function (e) {
            $('#btnSearch').html('Search');
            $('#btnSearch').removeAttr('disabled');
        },
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
                filterable: false
            },
            {
                field: "rowSl",
                title: "Sl",
                width: "15px",
                filterable: false,
            },
            {
                field: "EmployeeCode",
                title: "Code",
                width: "15px",
                filterable: true,
            },
            {
                field: "EmployeeName",
                title: "Employee Name",
                width: "35px",
                filterable: true,
            },
            {
                field: "OfficeName",
                title: "Office",
                width: "30px",
                filterable: true,
            },
            {
                field: "DepartmentName",
                title: "Department",
                width: "35px",
                filterable: true,
            },
            {
                field: "OfficeDesignation",
                title: "Designation",
                width: "35px",
                filterable: true,
            },
            {
                field: "AttendanceDate",
                title: "Absent Date",
                width: "40px",
                filterable: true,
            },
            {
                title: "Leave Type",
                width: "30px",
                filterable: false,
                template: function (dataItem) { 
                    return absentEmployeeManager.loadLeaveTypeDropDown(dataItem);;
                }
            },
            {
                title: "Print PDF",
                width: "20px",
                filterable: false,
                template: function (dataItem) {
                    return "<a href='#' OnClick='btnPrintinput(\"" + dataItem.EmployeeCode + "\");'><i class='fa fa-print '></i></a>";
                }
            },

            {
                title: "leave Apply",
                width: "20px",
                template: function (dataItem) {
                    //return "<a href='#'><i class='fa fa-check '></i></a>";
                    return "<a href='#' OnClick='ApproveLeaveDeduction(" + dataItem.EmployeeId + ",\"" + dataItem.EmployeeCode + "\",\"" + dataItem.StartDate + "\",\"" + dataItem.EndDate + "\"," + dataItem.rowSl + ");'><i class='fa fa-check '></i></a>";
                }
            },
        ],
        toolbar: ["excel"],
        excel: {
            fileName: "Leave Deduction For Absent.xlsx",
            allPages: true
        }

    }).data("kendoGrid");
}

function ApproveLeaveDeduction(EmployeeId, EmployeeCode, StartDate, EndDate, rowSl) {

    var leaveType = $("#LeaveType" + rowSl).val();
    var entity = {
        EmployeeId: EmployeeId,
        LeaveStartDate: StartDate,
        LeaveEndDate: EndDate,
        LeaveTypeId: leaveType,
        IsAbsentLeave: 1,
        LeaveDayDuration: "Full"
    }
    if (leaveType != "") {
        $.alert.open('confirm', 'Are you sure you want to apply this leave?', function (button) {
            if (button == 'yes') {
                $('#AjaxLoader').show();
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    async: false,
                    cache: false,
                    url: '/LeaveHistoryNew/LeaveEntry',
                    data: JSON.stringify({ model: entity }),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $('#AjaxLoader').hide();
                        if (data.Result == 1) {
                            
                            $.alert.open("Success", data.Message);

                            var month = $("#Month").val();
                            var year = $("#Year").val();

                            var office_TypeId = $("#OfficeTypeId").val();
                            if (office_TypeId == 1) {
                                OfficeTypeId = office_TypeId;
                                officeId = $("#PVHeadOfficeId").val();
                            } else if (office_TypeId == 3) {
                                OfficeTypeId = office_TypeId;
                                officeId = $("#PVProjectId").val();
                            } else if (office_TypeId == 4) {
                                OfficeTypeId = office_TypeId;
                                officeId = $("#ZoneId").val();
                            } else if (office_TypeId == 5) {
                                OfficeTypeId = office_TypeId;
                                officeId = $("#AreaId").val();
                            } else if (office_TypeId == 6) {
                                OfficeTypeId = office_TypeId;
                                officeId = $("#UnitId").val();
                            } else if (office_TypeId == null || office_TypeId == '') {
                                OfficeTypeId = '';
                                officeId = ''
                            };

                            if (month != "" && year != "") {
                                GetEmployeesAbsentInfo(month, year, OfficeTypeId, officeId);
                            }
                        } else {
                            $.alert.open("Error", data.Message);
                        }
                    },
                    error: function (request, status, error) {
                        alert(request.statusText + "/" + request.statusText + "/" + error);
                    }

                });
            } else {
                return false;
            }
        });
    } else {
        $.alert.open("Error", "Please fill up required fields");
    }
}