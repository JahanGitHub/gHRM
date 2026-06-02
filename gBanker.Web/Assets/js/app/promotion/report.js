var Page = {
    ControlList: ["DateContainer", "BasicOfficeTypeContainer", "DesignationContainer", "TotalServiceYearContainer", "ServiceYearFromLastPromotionContainer"],
    ReportControlConfig: {
        EFPOD: ["DateContainer", "BasicOfficeTypeContainer", "DesignationContainer", "TotalServiceYearContainer", "ServiceYearFromLastPromotionContainer"]
    },
    ReportControlConfigPR: {
        EPRINDate: ["DateContainer", "BasicOfficeTypeContainer", "DateToContainer"]
    },

    ReportControlConfigMousumiCode: {
        EPRCode: ["EmployeeCodeContainer"]
    },

    ReportControlConfigMousumiDate: {
        EPRDate: ["DateContainer", "DateToContainer"]
    },

    IsNumber: function (Value) {
        return ![null, ""].includes(Value) && !isNaN(Value);
    },
    Load: function () {
        this.BindEvents();
    },
    BindEvents: function () {
        $("#Date").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true
            });
        $("#Date").datepicker('setDate', new Date());


        $("#DateTo").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true
            });
        $("#DateTo").datepicker('setDate', new Date());



        $("#ReportType").change(function () {
            var ReportType = $(this).val();
          //  alert(ReportType);
            
            if (Page.ReportControlConfig.hasOwnProperty(ReportType)) {
                Page.ControlList.forEach(function (x) { $("." + x).hide(); });
                Page.ReportControlConfig[ReportType].forEach(function (x) {
                    $("." + x).show();
                });
            }

            if (Page.ReportControlConfigPR.hasOwnProperty(ReportType)) {
                Page.ControlList.forEach(function (x) { $("." + x).hide(); });
                Page.ReportControlConfigPR[ReportType].forEach(function (x) {
                    $("." + x).show();
                });
            }

            if (Page.ReportControlConfigMousumiCode.hasOwnProperty(ReportType)) {
                Page.ControlList.forEach(function (x) { $("." + x).hide(); });
                Page.ReportControlConfigMousumiCode[ReportType].forEach(function (x) {
                    $("." + x).show();
                });
            }

            if (Page.ReportControlConfigMousumiDate.hasOwnProperty(ReportType)) {
                Page.ControlList.forEach(function (x) { $("." + x).hide(); });
                Page.ReportControlConfigMousumiDate[ReportType].forEach(function (x) {
                    $("." + x).show();
                });
            }

        });
    },
    PrintReport: function (DownloadExcel) {

        var ReportType = $("#ReportType").val();
      //  alert(ReportType);

        if (ReportType == "EPRCode") {
            var DesignationId = $("#DesignationId").val();
            DesignationId = Page.IsNumber(DesignationId) ? DesignationId : 0;
            var url = '/EmployeePromotion/EmployeeForPromotionByEmployeeCodeReport?OfficeTypeId=' + $("#BasicOfficeTypeId").val() +
                '&Date=' + $("#Date").val() +
                '&DesignationId=' + DesignationId +
                '&TotalServiceYear=' + $("#TotalServiceYear").val() +
                '&ServiceYearFromLastPromotion=' + $("#ServiceYearFromLastPromotion").val() +
                '&EmployeeCode='+$("#EmployeeCode").val()+
                '&DownloadExcel=' + (DownloadExcel ? "true" : "false");
            window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
        }
        else if (ReportType == "EPRDate")
        {
            var DesignationId = $("#DesignationId").val();
            DesignationId = Page.IsNumber(DesignationId) ? DesignationId : 0;
            var url = '/EmployeePromotion/EmployeeForPromotionByEmployeeDateToDateReport?OfficeTypeId=' + $("#BasicOfficeTypeId").val() +
                '&Date=' + $("#Date").val() +
                '&DateTo=' + $("#DateTo").val() +
                '&DesignationId=' + DesignationId +
                '&TotalServiceYear=' + $("#TotalServiceYear").val() +
                '&ServiceYearFromLastPromotion=' + $("#ServiceYearFromLastPromotion").val() +
                '&EmployeeCode=' + $("#EmployeeCode").val() +
                '&DownloadExcel=' + (DownloadExcel ? "true" : "false");
            window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
        }
        else if (ReportType == "EPRINDate") {     
            var url = '/EmployeePromotion/EmployeeForPromotionIncrementByEmployeeDateToDateReport?OfficeTypeId=' + $("#BasicOfficeTypeId").val() +
                '&Date=' + $("#Date").val() +
                '&DateTo=' + $("#DateTo").val() +               
                '&DownloadExcel=' + (DownloadExcel ? "true" : "false");
            window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
        }
        else {
            var DesignationId = $("#DesignationId").val();
            DesignationId = Page.IsNumber(DesignationId) ? DesignationId : 0;
            var url = '/EmployeePromotion/EmployeeEligibleForPromotionOnDateReport?OfficeTypeId=' + $("#BasicOfficeTypeId").val() +
                '&Date=' + $("#Date").val() +
                '&DesignationId=' + DesignationId +
                '&TotalServiceYear=' + $("#TotalServiceYear").val() +
                '&ServiceYearFromLastPromotion=' + $("#ServiceYearFromLastPromotion").val() +
                '&DownloadExcel=' + (DownloadExcel ? "true" : "false");
            window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');
        }
    }
};

$(function () {
    Page.Load();
});