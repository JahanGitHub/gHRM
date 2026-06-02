
function PrintSalaryBeforeApprovalReportPDF(year, month, officeTypeId, salaryDate, officeID, w_o_HO) {
    url = '/PRSalaryReport/PrintSalaryBeforeApprovalReportPDF2?Year=' + year + '&Month=' + month + '&officeTypeId=' + officeTypeId + '&salaryDate=' + salaryDate + '&officeID=' + officeID + '&w_o_HO=' + w_o_HO;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintSalaryBeforeApprovalReportPDF2(year, month, officeTypeId, salaryDate, officeID, w_o_HO, reportId ) {
    url = '/PRSalaryReport/PrintSalaryBeforeApprovalReportPDF3?Year=' + year + '&Month=' + month + '&officeTypeId=' + officeTypeId + '&salaryDate=' + salaryDate + '&officeID=' + officeID + '&w_o_HO=' + w_o_HO + '&reportId=' + reportId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintSalaryBeforeApprovalReportExel(year, month, officeTypeId, salaryDate, officeID) {
    url = '/PRSalaryReport/PrintSalaryBeforeApprovalReportExel?Year=' + year + '&Month=' + month + '&officeTypeId=' + officeTypeId + '&salaryDate=' + salaryDate + '&officeID=' + officeID;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}


function PrintSalaryBeforeApprovalReportExel2(year, month, officeTypeId, salaryDate, officeID, reportId) {
    url = '/PRSalaryReport/PrintSalaryBeforeApprovalReportExel2?Year=' + year + '&Month=' + month + '&officeTypeId=' + officeTypeId + '&salaryDate=' + salaryDate + '&officeID=' + officeID + '&reportId=' + reportId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintSalaryBeforeApprovalReportExel3(year, month, officeTypeId, salaryDate, officeID, reportId) {
    url = '/PRSalaryReport/PrintSalaryBeforeApprovalReportExel3?Year=' + year + '&Month=' + month + '&officeTypeId=' + officeTypeId + '&salaryDate=' + salaryDate + '&officeID=' + officeID + '&reportId=' + reportId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintRejectedEmployeesSalaryReportPDF(year, month) {
    url = '/PRSalaryReport/PrintRejectedEmployeesSalaryReportPDF?Year=' + year + '&Month=' + month;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintApprovedSalaryReportPDF(year, month, officeTypeId) {
    url = '/PRSalaryReport/PrintApprovedSalaryReportPDF?Year=' + year + '&Month=' + month + '&OfficeTypeId=' + officeTypeId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintApprovedSalaryReportExel(year, month, officeTypeId) {
    url = '/PRSalaryReport/PrintApprovedSalaryReportExel?Year=' + year + '&Month=' + month + '&OfficeTypeId=' + officeTypeId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintSalaryReportAfterApprovalGroupByOfficePDF(year, month, officeTypeId) {
    url = '/PRSalaryReport/PrintSalaryReportAfterApprovalGroupByOfficePDF?Year=' + year + '&Month=' + month + '&OfficeTypeId=' + officeTypeId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintSalaryReportAfterApprovalGroupByOfficeExel(year, month, officeTypeId) {
    url = '/PRSalaryReport/PrintSalaryReportAfterApprovalGroupByOffice?Year=' + year + '&Month=' + month + '&OfficeTypeId=' + officeTypeId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintSalaryReportAfterApprovalGroupByZoneAreaPDF(year, month, officeId) {
    url = '/PRSalaryReport/PrintSalaryReportAfterApprovalGroupByZoneAreaPDF?Year=' + year + '&Month=' + month + '&OfficeId=' + officeId;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintPFReportBeforeApproval(year, month, officeTypeId, type) {
    url = '/PRSalaryReport/PrintPFReportBeforeApproval?Year=' + year + '&Month=' + month + '&OfficeTypeId=' + officeTypeId + '&reportType=' + type;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function PrintEmployeeSalaryStatementDetailsBeforeApprovalPDF(year, month, officeTypeId,  officeID) {
    //url = '/PRSalaryReport/PrintPFReportBeforeApproval?Year=' + year + '&Month=' + month + '&OfficeTypeId=' + officeTypeId + '&reportType=' + type;
    url = `/PRSalaryReport/PrintEmployeeSalaryStatementDetailsBeforeApprovalPDF?Year=${year}&Month=${month}&OfficeTypeId=${officeTypeId}&OfficeId=${officeID}`;
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function GetRelatedOfficeXOfficeType(OfficeTypeId) {
    if (OfficeTypeId > 0) {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRProcess/GetRelatedOfficeXOfficeType',
            data: { OfficeTypeId: OfficeTypeId },
            dataType: 'json',
            async: false,
            success: function (data) {
                var htm = "";
                for (var i = 0; i < data.length; i++) {

                    htm += "<option " + (data[1].Selected ? "Selected='Selected'" : "") + " value='" + data[i].Value + "'>" + data[i].Text + "</option>"
                }
                $("#OfficeId").html(htm)

            },
            error: function (request, status, error) {
                $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}
function ProcessMonthlySalary(empType, month) {
    $("#btnProcessMonthlySalary").hide();
    var salaryYear = $("#Year option:selected").val();
    var officeTypeId = $("#OfficeTypeId").val();
    $("#AjaxLoader").show();

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRProcess/MonthlySalaryProcess',
        data: {
            empType: empType,
            month: month,
            salaryYear: salaryYear,
            OfficeTypeId: officeTypeId
        },
        dataType: 'json',
        async: true,
        success: function (data) {
            $("#btnProcessMonthlySalary").show();
            $("#AjaxLoader").hide();
            $.alert.open("Message", data);
        },
        error: function (request, status, error) {
            $("#AjaxLoader").hide();
            $.alert.open("Message", error);
        }
    });
}

function SalarySendForApproval(year, month) {

    if (year != "" && month != "") {
        $("#AjaxLoader").show();
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRProcess/SalarySendForApproval',
            data: { Year: year, Month: month },
            dataType: 'json',
            async: true,
            success: function (data) {
                $("#AjaxLoader").hide();
                $.alert.open("Message", data);
            },
            error: function (request, status, error) {
                $("#AjaxLoader").hide();
                $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function getSalarySummaryBeforeSendForApproval() {
    var year = $("#Year").val();
    var month = $("#Month").val();
    var officeTypeId = $("#OfficeTypeId").val();
    if (year != "" && year != '0' && month != "" && month != '0') {
        SummaryPreview(year, month);
    } else {
        $.alert.open("Error", "Please Insert Required Fields");
    }
}

function HoldSalary(EmployeeId) {

    var year = $("#Year").val();
    var month = $("#Month").val();
    if (EmployeeId > 0 && year != "" && month > 0) {
        $.alert.open('confirm', "Are you sure you want to Hold this Employee's salary ?", function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/PRProcess/HoldSalary',
                    data: { EmployeeId: EmployeeId, Year: year, Month: month },
                    dataType: 'json',
                    async: false,
                    success: function (data) {

                        if (data.result == 1) {
                            getSalarySummaryBeforeSendForApproval();
                        } else {
                            $.alert.open("Error", data.message);
                        }
                    },
                    error: function (request, status, error) {
                        $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
                    }
                });
            }
            return true;
        });
    } else {
        $.alert.open("Error", "Invalid Employee Id");
    }
}

function SummaryPreview(year, month) {

    $("#beforeApprovalLbl").show();
    $('#beforeApprovalGridKendo').html("");
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
                url: '/PRProcess/SalarySummaryPreviewBeforeSendForApproval',
                dataType: 'json',
                data: { year: year, month: month }
            }
        }
    });

    $("#beforeApprovalGridKendo").kendoGrid({
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
                field: "EmployeeId",
                hidden: true,
                filterable: false
            },
            {
                field: "EmployeeCode",
                title: "Code",
                width: "40px",
                filterable: true,
            },
            {
                field: "EmployeeName",
                title: "Name",
                width: "100px",
                filterable: true,
            },
            {
                field: "Department",
                title: "Department",
                width: "100px",
                filterable: true,
            },
            {
                field: "Designation",
                title: "Designation",
                width: "100px",
                filterable: true,
            },
            {
                field: "TotalEarning",
                title: "Total Earning",
                width: "50px",
                filterable: false,
            },
            {
                field: "TotalDeduction",
                title: "Total Deduction",
                width: "50px",
                filterable: false,
            },
            {
                field: "NetPayable",
                title: "Net Payable",
                width: "50px",
                filterable: false,
            },
            {
                title: "HoldSalary",
                width: "50px",
                template: function (dataItem) {
                    return "<a href='#' title='Hold Salary' OnClick='HoldSalary(" + dataItem.EmployeeId + ");'><i class='fa fa-pause'></i></a>";
                }
            }
        ]
    });
}

function getGeneratedSalaryDate() {
    var month = $("#Month").val();
    var year = $("#Year").val();
    if (!(month > 0 && year > 0)) {
        return;
    }

    var salaryDay = $("#SalaryDay").val();
    var monthName = $("#Month option:selected").text();

    if (monthName != "" && monthName != "Please Select") {
        monthName = monthName.substring(0, 3);
    }

    var generatedDate = salaryDay + "-" + monthName + "-" + year;
    $("#ProcessDate").val(generatedDate);
}





$(document).ready(function () {
    $("#beforeApprovalLbl").hide();
    $("#ZoneDiv").hide();

    $("#OfficeTypeId").change(function () {

        var OfficeTypeId = $("#OfficeTypeId").val();
        //if (OfficeTypeId != 1)
        //    $("#dvButton").hide();
        //else $("#dvButton").show();
        if (OfficeTypeId == 4) {
            $("#ZoneDiv").show();
        } else {
            $("#ZoneDiv").hide();
        }
        GetRelatedOfficeXOfficeType($(this).val())
    });

    $("#ProcessDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2100"
    });

    $("#ProcessDate").datepicker('setDate', new Date());

    $("#btnSummaryPreview").click(function () {
        getSalarySummaryBeforeSendForApproval();
    });

    $("#btnProcessMonthlySalary").click(function () {

        var officeTypeId = $("#OfficeTypeId").val();
        var Year = $("#Year option:selected").val();
        var Month = $('#Month option:selected').val();
        var ProcessDate = $('#ProcessDate').val();
        var emptype = 0;

        if (Year == "" || Month == 0 || ProcessDate == "" || officeTypeId == "") {
            $.alert.open("Message", "Please Fill All required Data.");
            return false;
        }
        $.alert.open('confirm', "Are you sure you want to perform salary process?", function (button) {
            if (button == 'yes') {
                ProcessMonthlySalary(emptype, Month);
            }
        });
    });// End Of Process1

    $("#btnSendForApproval").click(function () {
        var year = $("#Year").val();
        var month = $("#Month").val();
        var officeTypeId = $("#OfficeTypeId").val();
        if (year != "" && year != '0' && month != "" && month != '0') {
            $.alert.open('confirm', "Are you sure you want to send salary for approval?", function (button) {
                if (button == 'yes') {
                    SalarySendForApproval(year, month);
                }
            });
        } else {
            $.alert.open("Error", "Please Insert Required Fields");
        }
    });

    $("#btnPrint").click(function () {
        var type = $("#ReportType").val();
        var year = $("#Year").val();
        var month = $("#Month").val();
        var officeTypeId = $("#OfficeTypeId").val();
        var officeID = $("#OfficeId").val();
        var salaryDate = $("#ProcessDate").val();
        var w_o_HO = $("#chkwoHO").is(':checked');
        var reportId = $('#ReportType').val();

        var system = $('#system').val();

        if (system == 'GTT') {
            if (type == "1" || type == "4") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryBeforeApprovalReportPDF2(year, month, officeTypeId, salaryDate, officeID, w_o_HO, reportId );
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            if (type == "11" || type == "111") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryBeforeApprovalReportPDF2(year, month, officeTypeId, salaryDate, officeID, w_o_HO, reportId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }


            else if (type == "2") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryBeforeApprovalReportExel(year, month, officeTypeId, salaryDate, officeID, w_o_HO );
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if ( type == "222") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryBeforeApprovalReportExel3(year, month, officeTypeId, salaryDate, officeID, reportId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }



            else if (type == "3") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintRejectedEmployeesSalaryReportPDF(year, month);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            /*else if (type == "4") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintApprovedSalaryReportPDF(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }*/

            else if (type == "5") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintApprovedSalaryReportExel(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if (type == "6") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryReportAfterApprovalGroupByOfficePDF(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if (type == "7") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryReportAfterApprovalGroupByOfficeExel(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if (type == "8") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryReportAfterApprovalGroupByZoneAreaPDF(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }
            else if (type == "9" || type == "10" || type == "11") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintPFReportBeforeApproval(year, month, officeTypeId, type);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }


            else if (type == "99") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintEmployeeSalaryStatementDetailsBeforeApprovalPDF(year, month, officeTypeId, officeID);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }
        }
        else {

            if (type == "1" || type == "4") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryBeforeApprovalReportPDF(year, month, officeTypeId, salaryDate, officeID, w_o_HO);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if (type == "2") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryBeforeApprovalReportExel(year, month, officeTypeId, salaryDate, officeID);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }


            else if (type == "3") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintRejectedEmployeesSalaryReportPDF(year, month);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            /*else if (type == "4") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintApprovedSalaryReportPDF(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }*/

            else if (type == "5") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintApprovedSalaryReportExel(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if (type == "6") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryReportAfterApprovalGroupByOfficePDF(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if (type == "7") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryReportAfterApprovalGroupByOfficeExel(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

            else if (type == "8") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintSalaryReportAfterApprovalGroupByZoneAreaPDF(year, month, officeTypeId);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }
            else if (type == "9" || type == "10" || type == "11") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintPFReportBeforeApproval(year, month, officeTypeId, type);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }


            else if (type == "99") {
                if (year != "" && year != '0' && month != "" && month != '0') {
                    PrintEmployeeSalaryStatementDetailsBeforeApprovalPDF(year, month, officeTypeId, officeID);
                } else {
                    $.alert.open("Error", "Please Insert Required Fields");
                }
            }

        }

    });

    $("#Year").change(function () {
        getGeneratedSalaryDate();
    })

    $("#Month").change(function () {
        getGeneratedSalaryDate();
    })
    
});
