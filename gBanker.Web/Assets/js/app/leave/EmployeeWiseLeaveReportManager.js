
var employeeWiseLeaveReportManager = {
    init: function () {
        this.initDate();        
    },

    initDate: function () {
        $("#DateFrom").datepicker({
            dateFormat: "dd-M-yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#DateFrom").datepicker('setDate', new Date());

        $("#DateTo").datepicker({
            dateFormat: "dd-M-yy",       
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1920:2100"

        });
        $("#DateTo").datepicker('setDate', new Date());
    },   
}

$(document).ready(function () {
    employeeWiseLeaveReportManager.init();
    
    $("#btnPrint").click(function () {
        debugger;
        var url;        
        var dateFrom = $("#DateFrom").val();
        var dateTo = $("#DateTo").val();
        var employeeCode = $("#EmployeeCode").val();

        if (dateFrom == "" || dateTo == "" || employeeCode == "") {
            alert("Please enter Required fields");
            return false;
        }        
        else {
            url = '/LeaveReport/EmployeeWiseLeaveReportPrint?dateFrom=' + dateFrom + '&dateTo=' + dateTo + '&employeeCode=' + employeeCode;
            PrintReport(url);
        }
    });
});
function PrintReport(printUrl) {
    window.open(printUrl, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}