function IsNumber(Value) {
    return ![null, ""].includes(Value) && !isNaN(Value);
}

var Page = {
    Load: function () {
        $("#ApproveDate").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true
            });
        $("#ApproveDate").datepicker('setDate', new Date());
    },
    SummaryPreview: function () {
        if (!Page.IsValid()) return;
        $("#btnSummaryBeforeSendForApproval").prop("disabled", true);
        setTimeout(function () {
            $("#btnSummaryBeforeSendForApproval").prop("disabled", false);
        }, 1000);
        $(".before-approval-container").show();
        var Year = $("#FromYear").val();
        var Month = $("#FromMonth").val();
        Year = IsNumber(Year) ? Year : 0;
        Month = IsNumber(Month) ? Month : 0;
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
                    url: '/NoticePayProcess/NoticePaySummaryPreviewBeforeApproval',
                    dataType: 'json',
                    data: {
                        Year: Year,
                        Month: Month
                    }
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
            noRecords: {
                template: "<span style='padding: 10px;display: block;'>No data available</span>"
            },
            columns: [
                {
                    field: "Code",
                    title: "Code",
                    width: "40px",
                    filterable: true,
                },
                {
                    field: "Name",
                    title: "Name",
                    width: "100px",
                    filterable: true,
                },
                {
                    field: "InformDate",
                    title: "Inform Date",
                    width: "50px",
                    filterable: false,
                },
                {
                    field: "ResignDate",
                    title: "Resign Date",
                    width: "50px",
                    filterable: false,
                },
                {
                    field: "NoticePeriod",
                    title: "Notice Period",
                    width: "50px",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "NoticeGiven",
                    title: "Notice Given",
                    width: "50px",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "SalaryType",
                    title: "Salary Type",
                    width: "40px",
                    filterable: false,
                },
                {
                    field: "SalaryAmount",
                    title: "Salary Amount",
                    width: "50px",
                    format: "{0:n2}",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "SalaryPer",
                    title: "Salary Percentage",
                    width: "50px",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "Amount",
                    title: "Notice Pay Amount",
                    width: "50px",
                    format: "{0:n2}",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                }
            ]
        });
    },
    IsValid: function () {
        var Year = $("#FromYear").val();
        var Month = $("#FromMonth").val();
        Year = IsNumber(Year) ? Year : 0;
        Month = IsNumber(Month) ? Month : 0;

        if (0 == Year) {
            $.alert.open("Error", "From Year is required");
            return false;
        }
        if (0 == Month) {
            $.alert.open("Error", "From Month is required");
            return false;
        }
        return true;
    },
    ApproveNoticePaySendForApproval: function () {
        if (!Page.IsValid()) return;
        $("#btnApprove").prop("disabled", true);
        $(".before-approval-container").hide();
        var Year = $("#FromYear").val();
        var Month = $("#FromMonth").val();
        Year = IsNumber(Year) ? Year : 0;
        Month = IsNumber(Month) ? Month : 0;
        var Data = JSON.stringify({
            Year: Year,
            Month: Month,
            ApproveDate: $("#ApproveDate").val()
        });
        Req.POST.ApproveNoticePaySendForApproval(Data, function () {
            $("#btnApprove").prop("disabled", false);
            $.alert.open("Success", "Notice Pay Approved");
        }, function () {
            $("#btnApprove").prop("disabled", false);
        });
    },
    RejectNoticePaySendForApproval: function () {
        if (!Page.IsValid()) return;
        $("#btnReject").prop("disabled", true);
        $(".before-approval-container").hide();
        var Year = $("#FromYear").val();
        var Month = $("#FromMonth").val();
        Year = IsNumber(Year) ? Year : 0;
        Month = IsNumber(Month) ? Month : 0;
        var Data = JSON.stringify({
            Year: Year,
            Month: Month
        });
        Req.POST.RejectNoticePaySendForApproval(Data, function () {
            $("#btnReject").prop("disabled", false);
            $.alert.open("Success", "Notice Pay Rejected");
        }, function () {
            $("#btnReject").prop("disabled", false);
        });
    },
    ReportView: function () {
        $.alert.open("Info", "This report is not available.");
    }
};

var Req = {
    POST: {
        ApproveNoticePaySendForApproval: function (Data, callback, err_callback) {
            $.ajax({
                url: '/NoticePayProcess/ApproveNoticePaySendForApproval',
                type: 'Post',
                data: Data,
                async: true,
                contentType: 'application/json',
                success: function (response) {
                    if (!response.success) {
                        $.alert.open("Success", response.message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("Error", data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
                    if (null != err_callback) { err_callback(); }
                }
            });
        },
        RejectNoticePaySendForApproval: function (Data, callback, err_callback) {
            $.ajax({
                url: '/NoticePayProcess/RejectNoticePaySendForApproval',
                type: 'Post',
                data: Data,
                async: true,
                contentType: 'application/json',
                success: function (response) {
                    if (!response.success) {
                        $.alert.open("Success", response.message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("Error", data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
                    if (null != err_callback) { err_callback(); }
                }
            });
        }
    }
};

$(function () {
    Page.Load();
});