
var employeeReportManager = {
    getOffice: function () {
        var officeTypeId = $("#OfficeTypeId").val();
        var officeId = 0;
        if (officeTypeId != "") {
            if (officeTypeId == 1) {
                officeId = $("#PVHeadOfficeId").val();
            }
            else if (officeTypeId == 3) {
                officeId = $("#PVProjectId").val();
            }
            else if (officeTypeId == 4) {
                officeId = $("#ZoneId").val();
            }
            else if (officeTypeId == 5) {
                officeId = $("#AreaId").val();
            }
            else if (officeTypeId == 6) {
                officeId = $("#UnitId").val();
            }
        }
        return officeId;
    }
}

function HideAll() {
    $(".hideEmpCode").hide();
    $(".hideBloodGroup").hide();
    $(".employeeStatus").hide();
    $(".reportDate").hide();
    $(".exelPrint").hide();
    $(".officeNavigation").hide();
    $(".employeeServiceBook").hide();
}
function HideAllOtherReportField() {
    $(".hideReason").hide();
    $(".hideOfficeType").hide();
    $(".hideDateFrom").hide();
    $(".hideDateTo").hide();
}
function ShowAllOtherReportField() {
    $(".hideReason").show();
    $(".hideOfficeType").show();
    $(".hideDateFrom").show();
    $(".hideDateTo").show();
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


$(document).ready(function () {


    $("#OfficeTypeIdNew").change(function () {
        var OfficeTypeId = $("#OfficeTypeIdNew").val();
        GetRelatedOfficeXOfficeType($(this).val())
    });


    HideAll();
    HideAllOtherReportField();
    $("#DateFromNew").datepicker(
        {
            dateFormat: "dd-M-yy",
            showAnim: "scale",
        });

    $("#DateToNew").datepicker(
        {
            dateFormat: "dd-M-yy",
            showAnim: "scale",
        });


    $("#DateFrom").datepicker(
        {
            dateFormat: "dd-M-yy",
            showAnim: "scale",
        });

    $("#DateTo").datepicker(
        {
            dateFormat: "dd-M-yy",
            showAnim: "scale",
        });


    $("#ReportType").change(function () {
        var type = $("#ReportType").val();

        if (!type || type == 0) HideAll();

        if (type == "1") {
            $(".hideEmpCode").show();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "2") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "3") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "4") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "5") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "6") {
            HideAll();
        }
        if (type == "7") {
            HideAll();
            $(".employeeServiceBook").hide();
        }
        if (type == "8") {
            HideAll();
            $(".employeeServiceBook").hide();
        }
        if (type == "9") {
            HideAll();
            $(".employeeServiceBook").hide();
        }
        if (type == "10") {
            HideAll();
            $(".employeeServiceBook").hide();
        }
        if (type == "11") {
            HideAll();
            $(".employeeServiceBook").hide();
        }
        if (type == "12") {
            $(".reportDate").show();
            $(".exelPrint").show();
            $(".officeNavigation").hide();
            $(".hideBloodGroup").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "13") {
            $(".reportDate").hide();
            $(".exelPrint").hide();
            $(".hideEmpCode").hide();
            $(".officeNavigation").hide();
            $(".hideBloodGroup").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "14") {
            $(".reportDate").hide();
            $(".exelPrint").hide();
            $(".hideEmpCode").hide();
            $(".officeNavigation").hide();
            $(".hideBloodGroup").hide();
            $(".employeeServiceBook").hide();
        }
        //Employee Pay Slip
        if (type == "15") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".reportDate").hide();
            $(".exelPrint").hide();
            $(".officeNavigation").hide();
        }
        //Employee Service Book
        if (type === EmployeeReportConstants.Employee_Service_Book) {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".reportDate").hide();
            $(".exelPrint").hide();
            $(".officeNavigation").hide();
            $(".employeeServiceBook, .digitalIDCardSection").show();
        }

        //Digital ID Card
        if (type === EmployeeReportConstants.Digital_ID_Card) {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".reportDate").hide();
            $(".exelPrint").hide();
            $(".officeNavigation").hide();
            $(".employeeServiceBook, .digitalIDCardSection").show();
        }


        //Digital ID Card mousumi
        if (type === EmployeeReportConstants.Digital_ID_Card_Mousumi) {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".reportDate").hide();
            $(".exelPrint").hide();
            $(".officeNavigation").hide();
            $(".employeeServiceBook, .digitalIDCardSection").show();
        }


        if (type == "26") {
            $(".hideEmpCode").show();
            $(".hideBloodGroup").hide();
            $(".reportDate").hide();
            $(".exelPrint").hide();
            $(".officeNavigation").hide();
            $(".employeeServiceBook, .digitalIDCardSection").show();
            $(".hideEmpCode2").hide();
        }


        if (type == "18") {
            $(".hideEmpCode").show();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }



        if (type == "19" || type == "22") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
            $(".ManualOfficeTypeDiv").show();
        }
        if (type == "20") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
            $(".blood-group-label").addClass("required");
        }
        if (type == "21") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
            $(".blood-group-label").addClass("required");
        }
        if (type == "23") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "24") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "25") {
            $(".hideEmpCode").show();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }

        //if (type == "27") {
        //    $(".hideEmpCode").show();
        //    $(".hideBloodGroup").show();
        //    $(".officeNavigation").hide();
        //    $(".reportDate").hide();
        //    $(".employeeServiceBook").hide();         
        //}

        if (type == "27") {
            $(".hideEmpCode").show();
            $(".employeeServiceBook").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
        }

        if (type == "29") {
            $(".hideEmpCode").show();
            $(".employeeServiceBook").hide();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
        }
        if (type == "30" || type == "31" || type == "32" || type == "33" || type == "34" || type == "35" || type == "36" || type == "37" || type == "38" || type == "38" || type == "39" || type == "40" || type == "41" || type == "42" || type == "43" || type == "44" || type == "45" || type == "46" || type == "47" || type == "48" || type == "49" || type == "50") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").show();
            $(".employeeServiceBook").hide();
            $(".reportDate").show();

        }

    });

    $("#btnPrint").click(function () {
        var url;
        var officeId = 0;
        var employeeCode = 0;
        var type = $("#ReportType").val();

        if (type === "1") {
            var empCode = $("#EmployeeCode").val();
            if (empCode != "") {
                url = '/EmployeeReport/EmployeeWiseReportPrint?empCode=' + empCode;
                PrintReport(url);
            } else {
                $.alert.open("Error", "Please Select Employee Code");
                return false;
            }
        }

        if (type === "2") {
            var bloodGroup = $("#BloodGroup").val();
            if (bloodGroup != "" && bloodGroup != "0") {
                if (bloodGroup == "AG") {
                    url = '/EmployeeReport/BloodGroupWiseAllEmployeeReportPrint?bloodGroup=' + encodeURIComponent(bloodGroup) + '&qType=' + 0;
                    PrintReport(url);
                }
                else {
                    url = '/EmployeeReport/BloodGroupWiseAllEmployeeReportPrint?bloodGroup=' + encodeURIComponent(bloodGroup) + '&qType=' + 1;
                    PrintReport(url);
                }
            } else {
                $.alert.open("Error", "Please Select Blood Group");
                return false;
            }
        }

        if (type === "3") {
            url = '/EmployeeReport/ChartOfBloodSummaryReportPrint?';
            PrintReport(url);
        }
        if (type === "4") {
            url = '/EmployeeReport/OfficeNameWiseEmployeeCount?';
            PrintReport(url);
        }
        if (type === "5") {
            url = '/EmployeeReport/OfficeTypeWiseEmployeeCount?';
            PrintReport(url);
        }
        if (type === "6") {
            url = '/EmployeeReport/GenderWiseEmployeeCount?';
            PrintReport(url);
        }
        if (type === "7") {
            url = '/EmployeeReport/AllDepartmentWiseEmployeeCount?';
            PrintReport(url);
        }
        if (type === "8") {
            url = '/EmployeeReport/DepartmentWiseTotalEmployeeCount?';
            PrintReport(url);
        }
        if (type === "9") {
            url = '/EmployeeReport/DepartmentWiseTotalEmployeeGraphicalView?';
            PrintReport(url);

        }
        if (type === "10") {
            url = '/EmployeeReport/PayrollDesignationWiseEmployee?';
            PrintReport(url);
        }
        if (type === "11") {
            url = '/EmployeeReport/EmployementTypeWiseEmployeeCount';
            PrintReport(url);
        }

        if (type === "12") {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();
            if (dateFrom != "" && dateTo != "") {
                url = '/EmployeeReport/PayrollDesignationWiseInsuranceReport?DateFrom=' + dateFrom + '&DateTo=' + dateTo;
                PrintReport(url);
            } else {
                $.alert.open("Error", "Please Select Date");
                return false;
            }
        }

        // Employee Experience
        if (type === "13") {
            url = '/EmployeeReport/EmployeeExperienceReport?';
            PrintReport(url);
        }

        // Employee Demographic
        if (type === "14") {
            url = '/EmployeeReport/EmployeeDemographicReport?';
            PrintReport(url);
        }

        // Employee Pay Slip
        if (type === "15") {
            url = '/EmployeeReport/EmployeeSignatureReportList';
            PrintReport(url);

        }

        // Employee Service Book
        if (type === "16") {
            officeId = employeeReportManager.getOffice();
            departmentId = $('#DepartmentId').val();
            employeeCode = $('.employeeCode').val();
            url = '/EmployeeReport/EmployeeServiceBookReport?employeeCode=' + employeeCode + '&officeId=' + officeId + '&departmentId=' + departmentId;
            PrintReport(url);
        }
        // Employee Service Book
        if (type === EmployeeReportConstants.Digital_ID_Card) {
            debugger;
            officeId = employeeReportManager.getOffice();
            departmentId = $('#DepartmentId').val();
            employeeCode = $('.employeeCode').val();
            url = '/EmployeeReport/DigitalIDCard?employeeCode=' + employeeCode + '&officeId=' + officeId + '&departmentId=' + departmentId;
            PrintReport(url);
        }
    });

    $("#btnExelPrint").click(function () {
        debugger;
        var type = $("#ReportType").val();
        if (type == "12") {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();
            if (dateFrom != "" && dateTo != "") {
                url = '/EmployeeReport/PayrollDesignationWiseInsuranceReportExcel?DateFrom=' + dateFrom + '&DateTo=' + dateTo;
                PrintReport(url);
            } else {
                $.alert.open("Error", "Please Select Date");
                return false;
            }
        }
    });

    $("#ReportTypeOther").change(function () {
        var reportTypeOther = $("#ReportTypeOther").val();
        if (reportTypeOther == "DropoutByReason")
            ShowAllOtherReportField();
        if (reportTypeOther == "OfficeWiseActiveEmployeeByDesignation") {
            $(".hideReason").hide();
            $(".hideOfficeType").show();
            $(".hideDateFrom").show();
            $(".hideDateTo").show();
        }
        if (reportTypeOther == "PersonalInfo") {
            $(".hideReason").hide();
            $(".hideOfficeType").show();
            $(".hideDateFrom").hide();
            $(".hideDateTo").hide();
        }        
        if (reportTypeOther == "MonthWiseConfirmationList") {
            $(".hideOfficeType").show();
            $(".hideDateFrom").show();
            $(".hideDateTo").show();
        }
    });
    $("#btnViewOtherReport").click(function () {
        var otherReportURL;
        var reportTypeOther = $("#ReportTypeOther").val();
        var reasonId = $("#ReasonId").val();
        var officeTypeId = $("#OfficeTypeIdNew").val();
        var OfficeId = $("#OfficeId").val();
        var dateFrom = $("#DateFromNew").val();
        var dateTo = $("#DateToNew").val();
        var format = 'pdf';

        if (reportTypeOther == 'DropoutByReason') {
            if (reasonId == '' || dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                otherReportURL = '/CommonReportGenerator/EmployeeDropoutByReasonReport?reasonId=' + reasonId + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&officeTypeId=' + officeTypeId;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'OfficeWiseActiveEmployeeByDesignation') {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                otherReportURL = '/CommonReportGenerator/ActiveEmployeeInfoByDesignationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&officeTypeId=' + officeTypeId;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'PersonalInfo') {
            otherReportURL = '/CommonReportGenerator/EmployeePersonalInfoReport2?format=' + format + '&officeTypeId=' + officeTypeId + '&OfficeId=' + OfficeId;
            PrintReport(otherReportURL);
        }
        if (reportTypeOther == "MonthWiseConfirmationList") {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            } else {
                otherReportURL = '/CommonReportGenerator/MonthWiseConfirmationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&officeTypeId=' + officeTypeId;
                PrintReport(otherReportURL);
            }
        }           
        
    });
    $("#btnExcel").click(function () {
        debugger;
        var otherReportURL;
        var reportTypeOther = $("#ReportTypeOther").val();
        var reasonId = $("#ReasonId").val();
        var officeTypeId = $("#OfficeTypeIdNew").val();
        var dateFrom = $("#DateFromNew").val();
        var dateTo = $("#DateToNew").val();

        var format = 'excel';

        if (reportTypeOther == 'DropoutByReason') {
            if (reasonId == '' || dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                otherReportURL = '/CommonReportGenerator/EmployeeDropoutByReasonReport?reasonId=' + reasonId + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&officeTypeId=' + officeTypeId;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'OfficeWiseActiveEmployeeByDesignation') {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                otherReportURL = '/CommonReportGenerator/ActiveEmployeeInfoByDesignationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&officeTypeId=' + officeTypeId;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'PersonalInfo') {
            otherReportURL = '/CommonReportGenerator/EmployeePersonalInfoReport2?format=' + format + '&officeTypeId=' + officeTypeId + '&OfficeId=' + OfficeId;
            PrintReport(otherReportURL);
        }
        if (reportTypeOther == "MonthWiseConfirmationList") {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            } else {
                otherReportURL = '/CommonReportGenerator/MonthWiseConfirmationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&officeTypeId=' + officeTypeId;
                PrintReport(otherReportURL);
            }
        }

    });
});
function PrintReport(printUrl) {
    window.open(printUrl, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}