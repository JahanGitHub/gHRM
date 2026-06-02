
var performanceEvaluationHistoryManager = {
    init: function () {
        this.initDate();        
    },

    initDate: function () {
        $("#DateFrom").datepicker({
            //dateFormat: "mm/yy",
            dateFormat: "MM/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#DateFrom").datepicker('setDate', new Date());

        $("#DateTo").datepicker({
            //dateFormat: "mm/yy",
            dateFormat: "MM/yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#DateTo").datepicker('setDate', new Date());
    },

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
    },    
}

$(document).ready(function () {
    performanceEvaluationHistoryManager.init();

    $("#Ledger").change(function () {
        if ($(this).is(':not(:checked)'))
            $("#EmployeeCode").val('');          
    });

    $("#btnPrint").click(function () {
        debugger;
        var url;
        var officeTypeId = $("#OfficeTypeId").val();
        var officeId = performanceEvaluationHistoryManager.getOffice();
        var dateFrom = $("#DateFrom").val();
        var dateTo = $("#DateTo").val();
        var employeeCode = $("#EmployeeCode").val();
        var isLedger = $('#Ledger:checked').val() ? true : false;
        if (isLedger == true && employeeCode == "") {
            alert("Please enter Employee Code for Monthly Individual Performance Report");
            return false;
        }        
        if (dateFrom != '' || dateTo != '') {
            url = '/PerformanceEvaluationReport/PerformanceEvaluationHistoryReportPrint?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&officeId=' + officeId + '&employeeCode=' + employeeCode + '&isLedger=' + isLedger + '&officeTypeId=' + officeTypeId;
            PrintReport(url);
        }
    });
});
function PrintReport(printUrl) {
    window.open(printUrl, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}