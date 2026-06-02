
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

var Page = {
    SelectedTab: "typewise",
    IsNumber: function (Value) {
        return ![null, ""].includes(Value) && !isNaN(Value);
    },
    Load: function () {
        this.BindEvents();
    },
    BindEvents: function () {
    },
    GetData: function () {
        var OfficeIdBlood = "";
        var DeptIdBlood = "";

        var officeTypeIdBlood = $("#OfficeTypeId").val();

        if (officeTypeIdBlood != "") {
            if (officeTypeIdBlood == 1) {
                OfficeIdBlood = $("#PVHeadOfficeId").val();

            } else if (officeTypeIdBlood == 3) {
                OfficeIdBlood = $("#PVProjectId").val();
            }
            else if (officeTypeIdBlood == 4) {
                OfficeIdBlood = $("#ZoneId").val();
            }
            else if (officeTypeIdBlood == 5) {
                OfficeIdBlood = $("#AreaId").val();
            }
            else if (officeTypeIdBlood == 6) {
                OfficeIdBlood = $("#UnitId").val();
            }
        }
        var DeptIdBlood = $("#DepartmentId").val();
        if (DeptIdBlood != "") {
            DeptIdBlood = DeptIdBlood;
        } else {
            DeptIdBlood = "";
        }
        var payRollDesignationBlood = $("#DesignationId").val();
        var responsibilityBlood = $("#OfficeDesignationId").val();
        var statusBlood = $('#typeFilterColumn').val();
        var filterColumnBlood = $("#filterColumn").val();
        var filterValueBlood = $("#filterValue").val();
        if (filterColumnBlood != "" && filterValueBlood == "") {
            $.alert.open("Error", "Please Provide Filter Value");
            return false;
        }

        var SectionBlood = $("#Section").val();
        if (SectionBlood == "0") {
            SectionBlood = "";
        }
        var DegreeLevelId = $("#DegreeLevel").val();
        DegreeLevelId = Page.IsNumber(DegreeLevelId) ? parseInt(DegreeLevelId) : 0;
        var DegreeTitle = $("#DegreeTitle").val();
        if (null == DegreeTitle) DegreeTitle = "";
        var Concentration = $("#Concentration").val();
        if (null == Concentration) Concentration = "";
        return {
            OfficeTypeId: Page.IsNumber(officeTypeIdBlood) ? parseInt(officeTypeIdBlood) : 0,
            OfficeId: Page.IsNumber(OfficeIdBlood) ? parseInt(OfficeIdBlood) : 0,
            DesignationId: Page.IsNumber(payRollDesignationBlood) ? parseInt(payRollDesignationBlood) : 0,
            ResponsibilityId: Page.IsNumber(responsibilityBlood) ? parseInt(responsibilityBlood) : 0,
            DeptId: Page.IsNumber(DeptIdBlood) ? parseInt(DeptIdBlood) : 0,
            SectionId: Page.IsNumber(SectionBlood) ? parseInt(SectionBlood) : 0,
            Status: statusBlood,
            DegreeLevelId: DegreeLevelId,
            DegreeTitle: DegreeTitle,
            Concentration: Concentration
        };
    },
    CRReportTypeChange: function () {
        $(".emp-status-container").show();
        $(".blood-group-label").removeClass("required");
        $(".education-control").hide();
        var ReportType = $("#CRReportType").val();

        if (["1", "2"].includes(ReportType)) {
            $(".education-control").show();
        }

        $(".hideBloodGroup").show();
        $(".BloodGroupControlContainer").hide();
        $(".hideDateFromForCompany").hide();
        $(".hideDateToForCompany").hide();


        if (ReportType == 15) {
            $(".hideEmpCode").show();
            $(".hideBloodGroup").hide();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
            $('#FinalSattlementGridList').show();
            GetFinalSattlementList("0");
        }

        if (ReportType == 16 || ReportType == 17 || ReportType == 18 || ReportType == 19 || ReportType == 100 ) {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").show();
            $(".employeeServiceBook").hide();
        }

    },
    ShowCRReport: function () {
        var ReportType = $("#CRReportType").val();
        if (0 == ReportType) {
            $.alert.open("Error", "Please Select Report Type");
            return;
        }
        var Data = Page.GetData();
        if (false == Data) return;
        if (1 == ReportType) {
            var url = '/EmployeeReport/EmployeeEducationInformation' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status +
                "&DegreeLevelId=" + Data.DegreeLevelId +
                "&DegreeTitle=" + Data.DegreeTitle +
                "&Concentration=" + Data.Concentration;
            PrintReport(url);
        }
        else if (2 == ReportType) {
            var url = '/EmployeeReport/EmployeeEducationInformationSummary' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status +
                "&DegreeLevelId=" + Data.DegreeLevelId +
                "&DegreeTitle=" + Data.DegreeTitle +
                "&Concentration=" + Data.Concentration;
            PrintReport(url);
        }
        else if (3 == ReportType) {
            var url = '/EmployeeReport/EmployeeDistrictInformationReport' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (4 == ReportType) {
            var url = '/EmployeeReport/EmployeeDistrictInformationSummaryReport' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (5 == ReportType) {
            var url = '/EmployeeReport/DepartmentWiseEmployeeInformationReport' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (6 == ReportType) {
            var url = '/EmployeeReport/DelayConfirmationEmployeeListReport' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (7 == ReportType) {
            var url = '/EmployeeReport/ConfirmationEligibleEmployeeList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (8 == ReportType) {
            var url = '/EmployeeReport/ConfirmationEligibleEmployeeSummary' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (9 == ReportType) {
            var url = '/EmployeeReport/TransferableEmployeeList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (10 == ReportType) {
            var url = '/EmployeeReport/TransferableEmployeeSummary' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (11 == ReportType) {
            var url = '/EmployeeReport/IncrementEligibleEmployeeList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (12 == ReportType) {
            var url = '/EmployeeReport/IncrementEligibleEmployeeSummary' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (13 == ReportType) {
            var url = '/EmployeeReport/PromotionEligibleEmployeeList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (14 == ReportType) {
            var url = '/EmployeeReport/PromotionEligibleEmployeeSummary' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (15 == ReportType) {
            var EmployeeCode =  $('#EmployeeCode').val();

            if (EmployeeCode != "") {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/Employee/GetResignDeathEmployee',
                    data: { EmployeeCode: EmployeeCode },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data =="Valid") {
                           // window.location.href = '/EmployeeReport/FinalSattlement?EmpCode=' + //$('#EmployeeCode').val();
                            var url = '/EmployeeReport/FinalSattlement?EmpCode=' + $('#EmployeeCode').val();
                            window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');

                        }
                        else if (data == "AllReadyExist")
                        {
                            $.alert.open('Sorry this employee final sattlement already created! ');
                        }
                        else
                        {
                            $.alert.open('Sorry this employee not resigend or death ');
                        }
                    },
                });
            } else {

                $.alert.open('Please type employee code first ');
                $('#EmployeeCode').focus();
            }


        }
        else if (16 == ReportType) {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();

            var url = '/EmployeeReport/GcConfirmationEligibleEmployeeList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
               /* "&OfficeId=" + Data.PVHeadOfficeId +*/
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status +
                "&DateFrom=" + dateFrom +
                "&DateTo=" + dateTo;
            PrintReport(url);
        }
        else if (17 == ReportType) {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();

            var url = '/EmployeeReport/GcConfirmationEligibleEmployeeList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status +
                "&DateFrom=" + dateFrom +
                "&DateTo=" + dateTo;
            PrintReport(url);
        }
        else if (18 == ReportType) {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();

            var url = '/EmployeeReport/GcHoEmployeeSeparationList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status +
                "&DateFrom=" + dateFrom +
                "&DateTo=" + dateTo;
            PrintReport(url);
        }
        else if (19 == ReportType) {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();

            var url = '/EmployeeReport/GcHoEmployeeSeparationList' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status +
                "&DateFrom=" + dateFrom +
                "&DateTo=" + dateTo;
            PrintReport(url);
        }
        else if (20 == ReportType) {
            var url = '/EmployeeReport/EmployeeHighestEducationReport' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }
        else if (21 == ReportType) {
            var url = '/EmployeeReport/GenderWiseDetailsReport' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status;
            PrintReport(url);
        }

        // join and resign 
        else if (100 == ReportType) {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();
            var url = '/EmployeeReport/EmployeeJoiningAndResigningReport_DateRange' +
                "?OfficeTypeId=" + Data.OfficeTypeId +
                "&OfficeId=" + Data.OfficeId +
                "&DesignationId=" + Data.DesignationId +
                "&ResponsibilityId=" + Data.ResponsibilityId +
                "&DeptId=" + Data.DeptId +
                "&SectionId=" + Data.SectionId +
                "&Status=" + Data.Status +
                "&DateFrom=" + dateFrom +
                "&DateTo=" + dateTo;
            PrintReport(url);
        }

    }
}

function FinalSattlementFilter(obj) {
    if (obj.value.length >= 4) {
       // alert(obj.value);
        GetFinalSattlementList(obj.value);
    }   

}

function HideAll() {
    $(".hideEmpCode").hide();
    $(".hideBloodGroup").hide();
    $(".employeeStatus").hide();
    $(".reportDate").hide();
    $(".exelPrint").hide();
    /*  $(".officeNavigation").hide();*/
    $(".employeeServiceBook").hide();
    $(".ManualOfficeTypeDiv").hide();
}
function HideAllOtherReportField() {
    $(".hideReason").hide();
    $(".hideOfficeType").hide();
    $(".hideDateFrom").hide();
    $(".hideDateTo").hide();
    $(".hideReportTypeOther").hide();
    $(".hidePrintExcelView").hide();
}
function ShowAllOtherReportField() {
    $(".hideReason").show();
    $(".hideOfficeType").show();
    $(".hideDateFrom").show();
    $(".hideDateTo").show();
}
$(document).ready(function () {

    HideAll();
    HideAllOtherReportField();

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var type = $(e.target).attr("data-id") // activated tab
        Page.SelectedTab = type;
        $(".ManualOfficeTypeDiv").hide();

        if ("othersreport" == type) {
            $(".hideReportType").hide();
            $(".hideEmpCode").hide();
            $("#btnPrint").hide();
            $(".hideReportTypeOther").show();
            $(".hidePrintExcelView").show();
            $(".hideBloodGroup").show();
            $("#ReportTypeOther").val("");
            $(".companyreport-control").hide();
            $(".BloodGroupControlContainer").show();
        } else if ("companyreport" == type) {
            $(".hideReportType").hide();
            $(".hideReportTypeOther").hide();
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").hide();
            $(".hideDateFromForCompany").hide();
            $(".hideDateToForCompany").hide();
            HideAllOtherReportField();
            $("#btnPrint").show();
            $("#CRReportType").val(0);
            $(".companyreport-control").show();
        } else {
            $(".hideReportType").show();
            $(".hideReportTypeOther").hide();
            HideAllOtherReportField();
            $("#btnPrint").show();
            $("#ReportType").val(0);
            $(".companyreport-control").hide();
            $(".BloodGroupControlContainer").show();
        }
    });

    HideAll();
    HideAllOtherReportField();
    $("#DateFrom").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1950:2050"
    });
    $("#DateFrom").datepicker('setDate', new Date());
    $("#DateTo").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1950:2050"
    });
    $("#DateTo").datepicker('setDate', new Date());
    $("#DateFromNew").datepicker(
        {
            dateFormat: "dd-M-yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true
        });

    $("#DateToNew").datepicker(
        {
            dateFormat: "dd-M-yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true
        });


    $("#ReportType").change(function () {
        var type = $("#ReportType").val();
        $(".blood-group-label").removeClass("required");

        HideAll();

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
            $(".blood-group-label").addClass("required");
        }
        if (type == "3") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "4") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "5") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "6") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "7") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "8") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "9") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "10") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "11") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "12") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").show();
            $(".employeeServiceBook").hide();
            $(".exelPrint").show();
        }
        if (type == "13") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        if (type == "14") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        //Employee Pay Slip
        if (type == "15") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
        }
        //Employee Service Book
        if (type === EmployeeReportConstants.Employee_Service_Book) {
            $(".hideEmpCode").show();
            /*   $(".hideBloodGroup").show();*/
            $(".officeNavigation").hide();
            $(".reportDate").hide();
            $(".employeeServiceBook").hide();
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
        if (type == "30" || type == "31" || type == "32" || type == "33" || type == "34" || type == "35" || type == "36" || type == "37" || type == "38" || type == "38" || type == "39" || type == "40" || type == "41" || type == "42" || type == "43" || type == "44" || type == "45" || type == "46" || type == "47" || type == "48" || type == "49" || type == "50" || type == "100") {
            $(".hideEmpCode").hide();
            $(".hideBloodGroup").show();
            $(".hideOfficeType").hide();
            $(".employeeServiceBook").hide();
            $(".reportDate").show();

        }

    });


    $("#btnPrint").click(function () {
        if ("companyreport" == Page.SelectedTab) {

            Page.ShowCRReport();
            return;
        }

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

        //if (["2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "20", "21", "23", "24"].includes(type)) {
        //if (["2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "20", "21", "26"].includes(type)) {
        if (["2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "20", "21", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "41", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "100"].includes(type)) {

            var bloodGroup = $("#BloodGroup").val();
            if (type != "2" || (bloodGroup != "" && bloodGroup != "0")) {


                // NEW Search Option
                var OfficeIdBlood = "";
                var DeptIdBlood = "";

                var officeTypeIdBlood = $("#OfficeTypeId").val();

                if (officeTypeIdBlood != "") {
                    if (officeTypeIdBlood == 1) {
                        OfficeIdBlood = $("#PVHeadOfficeId").val();

                    } else if (officeTypeIdBlood == 3) {
                        OfficeIdBlood = $("#PVProjectId").val();
                    }
                    else if (officeTypeIdBlood == 4) {
                        OfficeIdBlood = $("#ZoneId").val();
                    }
                    else if (officeTypeIdBlood == 5) {
                        OfficeIdBlood = $("#AreaId").val();
                    }
                    else if (officeTypeIdBlood == 6) {
                        OfficeIdBlood = $("#UnitId").val();
                    }
                }
                var DeptIdBlood = $("#DepartmentId").val();
                if (DeptIdBlood != "") {
                    DeptIdBlood = DeptIdBlood;
                } else {
                    DeptIdBlood = "";
                }
                var payRollDesignationBlood = $("#DesignationId").val();
                var responsibilityBlood = $("#OfficeDesignationId").val();
                var statusBlood = $('#typeFilterColumn').val();
                var filterColumnBlood = $("#filterColumn").val();
                var filterValueBlood = $("#filterValue").val();
                if (filterColumnBlood != "" && filterValueBlood == "") {
                    $.alert.open("Error", "Please Provide Filter Value");
                    return false;
                }

                var SectionBlood = $("#Section").val();
                if (SectionBlood == "0") {
                    SectionBlood = "";
                }



                // END New Search Option
                if (type === "2") {
                    if (bloodGroup == "AG") {
                        url = '/EmployeeReport/BloodGroupWiseAllEmployeeReportPrint?bloodGroup=' + encodeURIComponent(bloodGroup) + '&qType=' + 0 + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + '&filterColumn=' + filterColumnBlood + '&filterValue=' + filterValueBlood;
                        PrintReport(url);
                    }
                    else {
                        url = '/EmployeeReport/BloodGroupWiseAllEmployeeReportPrint?bloodGroup=' + encodeURIComponent(bloodGroup) + '&qType=' + 1 + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + '&filterColumn=' + filterColumnBlood + '&filterValue=' + filterValueBlood;
                        PrintReport(url);
                    }
                } else if (type === "3") {
                    url = '/EmployeeReport/ChartOfBloodSummaryReportPrint?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "4") {
                    url = '/EmployeeReport/OfficeNameWiseEmployeeCount?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "5") {
                    url = '/EmployeeReport/OfficeTypeWiseEmployeeCount?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "6") {
                    url = '/EmployeeReport/GenderWiseEmployeeCount?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "7") {
                    url = '/EmployeeReport/AllDepartmentWiseEmployeeCount?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "8") {
                    url = '/EmployeeReport/DepartmentWiseTotalEmployeeCount?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "9") {
                    url = '/EmployeeReport/DepartmentWiseTotalEmployeeGraphicalView?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "10") {
                    url = '/EmployeeReport/PayrollDesignationWiseEmployee?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "11") {
                    url = '/EmployeeReport/EmployementTypeWiseEmployeeCount?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "12") {
                    var dateFrom = $("#DateFrom").val();
                    var dateTo = $("#DateTo").val();
                    if (dateFrom != "" && dateTo != "") {
                        url = '/EmployeeReport/PayrollDesignationWiseInsuranceReport?DateFrom=' + dateFrom + '&DateTo=' + dateTo + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                        PrintReport(url);
                    } else {
                        $.alert.open("Error", "Please Select Date");
                        return false;
                    }
                } else if (type === "13") {// Employee Experience
                    url = '/EmployeeReport/EmployeeExperienceReport?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "14") {// Employee Demographic
                    url = '/EmployeeReport/EmployeeDemographicReport?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "15") {// Employee Pay Slip
                    url = '/EmployeeReport/EmployeeSignatureReportList?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "16") {// Employee Service Book
                    var EmployeeCode = $("#EmployeeCode").val();
                    url = '/EmployeeReport/EmployeeServiceBookReport?employeeCode=' + EmployeeCode + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "20") {
                    url = '/EmployeeReport/EmployeeGuarantorInformation?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "21") {

                    url = '/EmployeeReport/EmployeePreviousWorkExperience?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                } else if (type === "23") {
                    url = '/EmployeeReport/AllDepartmentWiseEmployeeCountForMousumi?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                }

                else if (type === "24") {
                    url = '/EmployeeReport/PayrollDesignationWiseEmployeeForMousumi?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                }

                else if (type === "27") {// Employee Experience
                    var empCode = $("#EmployeeCode").val();
                    url = '/EmployeeReport/EmployeeStatusBranchWiseForMousumi?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + "&EmployeeCode=" + empCode;
                    PrintReport(url);
                }
                else if (type === "30" || type === "31" || type === "32" || type === "33" || type === "34" || type === "35" || type === "36" || type === "37" || type === "38" || type === "39" || type === "40" || type === "41") {
                    var dateFrom = $("#DateFrom").val();
                    var dateTo = $("#DateTo").val();
                    url = '/EmployeeReport/EmployeeETinInfoReport?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + '&type=' + type + '&DateFrom=' + dateFrom + '&DateTo=' + dateTo;
                    PrintReport(url);
                }
                else if (type === "42") {
                    url = '/EmployeeReport/PayrollDesignationWiseEmployee_Payroll?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                    PrintReport(url);
                }

            } else {
                $.alert.open("Error", "Please Select Blood Group");
                return false;
            }
        }
        if (type === "17") {// Employee Service Book
            debugger;
            officeId = employeeReportManager.getOffice();
            departmentId = $('#DepartmentId').val();
            employeeCode = $('.employeeCode').val();
            url = '/EmployeeReport/DigitalIDCard?employeeCode=' + employeeCode + '&officeId=' + officeId + '&departmentId=' + departmentId;
            PrintReport(url);
        }

        if (type === "28") {// Employee Service Book
            debugger;
            officeId = employeeReportManager.getOffice();
            departmentId = $('#DepartmentId').val();
            employeeCode = $('.employeeCode').val();
            url = '/EmployeeReport/DigitalIDCardMousumi?employeeCode=' + employeeCode + '&officeId=' + officeId + '&departmentId=' + departmentId;
            PrintReport(url);
        }



        if (type === "25") {// Employee Experience
            var empCode = $("#EmployeeCode").val();
            url = '/EmployeeReport/EmployeeExperienceReportForMousumi?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + "&EmployeeCode=" + empCode;
            PrintReport(url);
        }


        if (type === "26") {
            var empCode = $("#EmployeeCode").val();
            url = '/EmployeeReport/EmployeeNomineeReport?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + "&EmployeeCode=" + empCode;
            PrintReport(url);
        }


        //if (type === "27") {
        //    var empCode = $("#EmployeeCode").val();
        //    url = '/EmployeeReport/EmployeeStatusBranchWiseForMousumi?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + "&EmployeeCode=" + empCode;
        //    PrintReport(url);
        //}

        if (type === "29") {
            var empCode = $("#EmployeeCode").val();
            url = '/EmployeeReport/ReportOfJoiningLetterAfterTransfer?bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood + "&EmployeeCode=" + empCode;
            PrintReport(url);
        }


        if (type === "18") {
            var empCode = $("#EmployeeCode").val();
            if (empCode != "") {
                url = '/EmployeeReport/EmployeeWiseLeaveDetailsPrint?empCode=' + empCode;
                PrintReport(url);
            } else {
                $.alert.open("Error", "Please Select Employee Code");
                return false;
            }
        }

        if (type === "19") {
            url = '/EmployeeReport/EmployeeRegisterPrint?ManualOfficeType=' + $("#ManualOfficeType").val();
            PrintReport(url);
        }

        if (type === "22") {
            url = '/EmployeeReport/EmployeeRegisterOfficeWisePrint';
            PrintReport(url);
        }
    });

    $("#btnExelPrint").click(function () {
        var url;
        var type = $("#ReportType").val();
        var bloodGroup = $("#BloodGroup").val();

        // NEW Search Option
        var OfficeIdBlood = "";
        var DeptIdBlood = "";

        var officeTypeIdBlood = $("#OfficeTypeId").val();

        if (officeTypeIdBlood != "") {
            if (officeTypeIdBlood == 1) {
                OfficeIdBlood = $("#PVHeadOfficeId").val();

            } else if (officeTypeIdBlood == 3) {
                OfficeIdBlood = $("#PVProjectId").val();
            }
            else if (officeTypeIdBlood == 4) {
                OfficeIdBlood = $("#ZoneId").val();
            }
            else if (officeTypeIdBlood == 5) {
                OfficeIdBlood = $("#AreaId").val();
            }
            else if (officeTypeIdBlood == 6) {
                OfficeIdBlood = $("#UnitId").val();
            }
        }
        var DeptIdBlood = $("#DepartmentId").val();
        if (DeptIdBlood != "") {
            DeptIdBlood = DeptIdBlood;
        } else {
            DeptIdBlood = "";
        }
        var payRollDesignationBlood = $("#DesignationId").val();
        var responsibilityBlood = $("#OfficeDesignationId").val();
        var statusBlood = $('#typeFilterColumn').val();
        var filterColumnBlood = $("#filterColumn").val();
        var filterValueBlood = $("#filterValue").val();
        if (filterColumnBlood != "" && filterValueBlood == "") {
            $.alert.open("Error", "Please Provide Filter Value");
            return false;
        }

        var SectionBlood = $("#Section").val();
        if (SectionBlood == "0") {
            SectionBlood = "";
        }


        if (type == "12") {
            var dateFrom = $("#DateFrom").val();
            var dateTo = $("#DateTo").val();
            if (dateFrom != "" && dateTo != "") {
                url = '/EmployeeReport/PayrollDesignationWiseInsuranceReportExcel?DateFrom=' + dateFrom + '&DateTo=' + dateTo + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(url);
            } else {
                $.alert.open("Error", "Please Select Date");
                return false;
            }
        }
    });

    $("#ReportTypeOther").change(function () {
        $(".blood-group-label").removeClass("required");
        var reportTypeOther = $("#ReportTypeOther").val();
        $(".emp-status-container").show();
        if (reportTypeOther == "DropoutByReason") {
            ShowAllOtherReportField();
            $(".emp-status-container").hide();
        }

        if (reportTypeOther == "DropoutByReasonForMousumi") {
            ShowAllOtherReportField();
            $(".hideEmpCode").show();
            $(".emp-status-container").hide();
            
        }

        if (reportTypeOther == "OfficeWiseActiveEmployeeByDesignation") {
            $(".hideReason").hide();
            $(".hideOfficeType").show();
            $(".officeNavigation").hide();
            $(".hideDateFrom").show();
            $(".hideDateTo").show();
        }
        if (reportTypeOther == "PersonalInfo") {
            $(".hideReason").hide();
            $(".hideOfficeType").show();
            $(".hideDateFrom").hide();
            $(".hideDateTo").hide();
            $(".officeNavigation").show();

        }
        if (reportTypeOther == "MonthWiseConfirmationList") {
            $(".hideOfficeType").show();
            $(".hideDateFrom").show();
            $(".hideDateTo").show();
        }
        if (reportTypeOther == "MonthWiseConfirmationDueList") {
            $(".hideOfficeType").show();
            $(".hideDateFrom").show();
            $(".hideDateTo").show();
        }

    });
    $("#btnViewOtherReport").click(function () {
        var otherReportURL;
        var reportTypeOther = $("#ReportTypeOther").val();
        var reasonId = $("#ReasonId").val();
        /*    var officeTypeId = $("#OfficeTypeIdNew").val();*/
        var dateFrom = $("#DateFromNew").val();
        var dateTo = $("#DateToNew").val();
        var format = 'pdf';

        //New Filter

        var bloodGroup = $("#BloodGroup").val();

        // NEW Search Option
        var OfficeIdBlood = "";
        var DeptIdBlood = "";

        var officeTypeIdBlood = $("#OfficeTypeId").val();

        if (officeTypeIdBlood != "") {
            if (officeTypeIdBlood == 1) {
                OfficeIdBlood = $("#PVHeadOfficeId").val();

            } else if (officeTypeIdBlood == 3) {
                OfficeIdBlood = $("#PVProjectId").val();
            }
            else if (officeTypeIdBlood == 4) {
                OfficeIdBlood = $("#ZoneId").val();
            }
            else if (officeTypeIdBlood == 5) {
                OfficeIdBlood = $("#AreaId").val();
                if (OfficeIdBlood == '') {
                    OfficeIdBlood = $("#ZoneId").val();
                }
            }
            else if (officeTypeIdBlood == 6) {
                OfficeIdBlood = $("#UnitId").val();
                if (OfficeIdBlood == '') {
                    OfficeIdBlood = $("#AreaId").val();
                    if (OfficeIdBlood == '') {
                        OfficeIdBlood = $("#ZoneId").val();
                    }
                }
            }
        }
        var DeptIdBlood = $("#DepartmentId").val();
        if (DeptIdBlood != "") {
            DeptIdBlood = DeptIdBlood;
        } else {
            DeptIdBlood = "";
        }
        var payRollDesignationBlood = $("#DesignationId").val();
        var responsibilityBlood = $("#OfficeDesignationId").val();
        var statusBlood = $('#typeFilterColumn').val();
        var filterColumnBlood = $("#filterColumn").val();
        var filterValueBlood = $("#filterValue").val();
        if (filterColumnBlood != "" && filterValueBlood == "") {
            $.alert.open("Error", "Please Provide Filter Value");
            return false;
        }

        var SectionBlood = $("#Section").val();
        if (SectionBlood == "0") {
            SectionBlood = "";
        }

        //End new Filter

        if (reportTypeOther == 'DropoutByReason') {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                reasonId = [null, ""].includes(reasonId) ? 0 : reasonId;
                otherReportURL = '/CommonReportGenerator/EmployeeDropoutByReasonReport?reasonId=' + reasonId + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'OfficeWiseActiveEmployeeByDesignation') {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                otherReportURL = '/CommonReportGenerator/ActiveEmployeeInfoByDesignationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'PersonalInfo') {
            otherReportURL = '/CommonReportGenerator/EmployeePersonalInfoReport?format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
            PrintReport(otherReportURL);
        }
        if (reportTypeOther == "MonthWiseConfirmationList") {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            } else {
                otherReportURL = '/CommonReportGenerator/MonthWiseConfirmationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == "MonthWiseConfirmationDueList") {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            } else {
                otherReportURL = '/CommonReportGenerator/MonthWiseConfirmationDueReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'DropoutByReasonForMousumi') {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                reasonId = [null, ""].includes(reasonId) ? 0 : reasonId;
                otherReportURL = '/CommonReportGenerator/EmployeeDropoutByReasonReportForMousumi?reasonId=' + reasonId + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }

    });
    $("#btnExcel").click(function () {
        

        var otherReportURL;
        var reportTypeOther = $("#ReportTypeOther").val();
        var reasonId = $("#ReasonId").val();
        /*    var officeTypeId = $("#OfficeTypeIdNew").val();*/
        var dateFrom = $("#DateFromNew").val();
        var dateTo = $("#DateToNew").val();

        var format = 'excel';

        //New Filter

        var bloodGroup = $("#BloodGroup").val();

        // NEW Search Option
        var OfficeIdBlood = "";
        var DeptIdBlood = "";

        var officeTypeIdBlood = $("#OfficeTypeId").val();

        if (officeTypeIdBlood != "") {
            if (officeTypeIdBlood == 1) {
                OfficeIdBlood = $("#PVHeadOfficeId").val();

            } else if (officeTypeIdBlood == 3) {
                OfficeIdBlood = $("#PVProjectId").val();
            }
            else if (officeTypeIdBlood == 4) {
                OfficeIdBlood = $("#ZoneId").val();
            }
            else if (officeTypeIdBlood == 5) {
                OfficeIdBlood = $("#AreaId").val();
            }
            else if (officeTypeIdBlood == 6) {
                OfficeIdBlood = $("#UnitId").val();
            }
        }
        var DeptIdBlood = $("#DepartmentId").val();
        if (DeptIdBlood != "") {
            DeptIdBlood = DeptIdBlood;
        } else {
            DeptIdBlood = "";
        }
        var payRollDesignationBlood = $("#DesignationId").val();
        var responsibilityBlood = $("#OfficeDesignationId").val();
        var statusBlood = $('#typeFilterColumn').val();
        var filterColumnBlood = $("#filterColumn").val();
        var filterValueBlood = $("#filterValue").val();
        if (filterColumnBlood != "" && filterValueBlood == "") {
            $.alert.open("Error", "Please Provide Filter Value");
            return false;
        }


        var SectionBlood = $("#Section").val();
        if (SectionBlood == "0") {
            SectionBlood = "";
        }

        //End new Filter

        if (reportTypeOther == 'DropoutByReason') {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                reasonId = (reasonId || reasonId=="" ? "0" : reasonId);
                otherReportURL = '/CommonReportGenerator/EmployeeDropoutByReasonReport?reasonId=' + reasonId + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'OfficeWiseActiveEmployeeByDesignation') {
            if (dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            }
            else {
                otherReportURL = '/CommonReportGenerator/ActiveEmployeeInfoByDesignationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }
        if (reportTypeOther == 'PersonalInfo') {
            otherReportURL = '/CommonReportGenerator/EmployeePersonalInfoReport?format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
            PrintReport(otherReportURL);
        }
        if (reportTypeOther == "MonthWiseConfirmationList") {
            if (reasonId == '' || dateFrom == '' || dateTo == '') {
                alert("Please fill up all required fields");
                return false;
            } else {
                otherReportURL = '/CommonReportGenerator/MonthWiseConfirmationReport?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&format=' + format + '&bloodGroup=' + encodeURIComponent(bloodGroup) + '&officeTypeId=' + officeTypeIdBlood + '&OfficeId=' + OfficeIdBlood + '&DeptId=' + DeptIdBlood + '&payRollDesignation=' + payRollDesignationBlood + '&responsibility=' + responsibilityBlood + '&Section=' + SectionBlood + '&status=' + statusBlood;
                PrintReport(otherReportURL);
            }
        }

    });
});
function PrintReport(printUrl) {
    window.open(printUrl, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}


function GetFinalSattlementList(EmployeeCode) {
    $('#grid').jtable({
        paging: true,
        pageSize: 10,
        sorting: true,
        actions: {
            listAction: function (postData, jtParams) {
                return $.Deferred(function ($dfd) {
                    var EmployeeCode = $("#filterInput").val();
                    $.ajax({
                        url: '/EmployeeReport/GetFinalSattlementReportList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting + "&EmployeeCode=" + EmployeeCode,
                        type: 'POST',
                        dataType: 'json',
                        data: postData,
                        success: function (data) {
                            $dfd.resolve(data);
                        },
                        error: function () {
                            $dfd.reject();
                        }
                    });
                });
            }
        },
        fields: {

            Id: {
                key: true,
                list: false,
                create: false,
                edit: false
            },
            HeadOfficeId: {
                width: '5%',
                title: 'SL'
            },
            EmployeeCode : {
                width: '5%',
                title: 'ID'
            },
            EmployeeName : {
                width: '15%',
                title: 'NAME '
            },
            BatchNo : {
                width: '10%',
                title: 'Report Date'
            },
            FirstJoiningDateMsg : {
                width: '10%',
                title: 'Joining Date'
            },
            kMessage: {
                width: '10%',
                title: 'Confirmation Date'
            },
            BirthPlace: {
                width: '10%',
                title: 'Last Working Day'
            },
            EditLink: {
                title: "Edit",
                width: '5%',
                sorting: false,
                display: function (data) {
                    return "<div class='text-center'><a href='#' OnClick=\"EditGridSattlement( '" + data.record.EmployeeCode + "');\"><i class='fa fa-pencil-square-o'></i></a></div>";

                }
            },
            Delete: {
                title: "Print",
                width: '5%',
                display: function (data) {
                    return "<div class='text-center'><a href='#' OnClick=\"PrintFinalSattlement('" + data.record.EmployeeCode  + "');\"><i class='fa fa-print'></i></a></div>";
                }
            }
        }

    });
    $('#grid').jtable('load');
}

function PrintFinalSattlement( EmployeeCode) {
    var url = "/EmployeeReport/FinalSattlementReport?EmployeeCode=" + EmployeeCode
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function EditGridSattlement(EmployeeCode) {
    //window.location.href = '/EmployeeReport/FinalSattlement?EmpCode=' + EmployeeCode + "&Edit=True";

    var url = '/EmployeeReport/FinalSattlement?EmpCode=' + EmployeeCode + "&Edit=True";
    window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');

}