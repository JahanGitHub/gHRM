function IsNumber(Value) {
    return ![null, ""].includes(Value) && !isNaN(Value);
}

var Page = {
    Load: function () {
        $("#OfficeTypeId").val(LoggedInOfficeTypeId);
        $("#OfficeTypeId").trigger("change");
        $("#ProcessDate").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true
            });
        $("#ProcessDate").datepicker('setDate', new Date());
    },
    GetOfficeId: function () {
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
    SummaryPreview: function () {
        if (!Page.IsValid()) return;
        $("#btnSummaryBeforeSendForApproval").prop("disabled", true);
        setTimeout(function () {
            $("#btnSummaryBeforeSendForApproval").prop("disabled", false);
        }, 1000);
        $(".before-approval-container").show();
        var officeId = Page.GetOfficeId();
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
                    url: '/GratuityProcess/GratuitySummaryPreviewBeforeApproval2',
                    dataType: 'json',
                    data: {
                        Year: Year,
                        Month: Month,
                        OfficeId: IsNumber(officeId) ? officeId : 0,
                        OfficeTypeId: $('#OfficeTypeId').val()
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
                    title: "Eligible From",
                    width: "40px",
                    filterable: true,
                    template: function (data) {
                        return "C" == data.EligibleFrom ? "Confirmation Date" : "Joining Date";
                    }
                },
                {
                    field: "JoinOrConfirmationDate",
                    title: "Join or Confirmation Date",
                    width: "50px",
                    filterable: false,
                },
                {
                    field: "SalaryDate",
                    title: "Salary Date",
                    width: "50px",
                    filterable: false,
                },
                {
                    field: "BasicSalary",
                    title: "Basic Salary",
                    width: "50px",
                    format: "{0:n2}",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "SerMonth",
                    title: "Serv Month",
                    width: "50px",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "CurGratuity",
                    title: "Cur Gratuity",
                    width: "50px",
                    format: "{0:n2}",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "CumGratuity",
                    title: "Cum Gratuity",
                    width: "50px",
                    format: "{0:n2}",
                    headerAttributes: { style: 'text-align: right' },
                    attributes: { style: 'text-align: right' },
                    filterable: false,
                },
                {
                    field: "GratuityTimes",
                    title: "Gratuity Times",
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
       // var OfficeId = Page.GetOfficeId();
       // $("#OfficeId").val(OfficeId);
        var Year = $("#FromYear").val();
        var Month = $("#FromMonth").val();
        Year = IsNumber(Year) ? Year : 0;
        Month = IsNumber(Month) ? Month : 0;

        //if (0 == OfficeId) {
        //    $.alert.open("Error", "Please select an Office");
        //    return false;
        //}
        if (0 == Year) {
            $.alert.open("Error", "From Year is required");
            return false;
        }
        //if (0 == Month) {
        //    $.alert.open("Error", "From Month is required");
        //    return false;
        //}
        if ([null, ""].includes($("#ProcessDate").val())) {
            $.alert.open("Error", "Date is required");
            return false;
        }
        return true;
    },
    SendGeneratedGratuityForApproval: function () {
        if (!Page.IsValid()) return;
        $("#btnSendForApproval").prop("disabled", true);
        $(".before-approval-container").hide();
        var officeId = Page.GetOfficeId();
        var Year = $("#FromYear").val();
        var Month = $("#FromMonth").val();
        Year = IsNumber(Year) ? Year : 0;
        Month = IsNumber(Month) ? Month : 0;
        var Data = JSON.stringify({
            OfficeId: 0,
            Year: Year,
            Month: Month,
            OfficeTypeId: $('#OfficeTypeId').val()
        });
        Req.POST.SendGeneratedGratuityForApproval(Data, function () {
            $("#btnSendForApproval").prop("disabled", false);
            $.alert.open("Success", "Gratuity Sent For Approval");
        }, function () {
            $("#btnSendForApproval").prop("disabled", false);
        });
    },
    GratuityForApproval: function () {
        if (!Page.IsValid()) return;
        $("#btnSendForApproval").prop("disabled", true);
        $(".before-approval-container").hide();
        var officeId = Page.GetOfficeId();
        var Year = $("#FromYear").val();
        var Month = $("#FromMonth").val();
        Year = IsNumber(Year) ? Year : 0;
        Month = IsNumber(Month) ? Month : 0;
        var Data = JSON.stringify({
            //OfficeId: 0,
            Year: Year,
            Month: Month,
            ApproveDate: $('#ProcessDate').val()
           // OfficeTypeId: $('#OfficeTypeId').val()
        });
        Req.POST.ApproveGratuitySendForApproval(Data, function () {
            $("#btnSendForApproval").prop("disabled", false);
            $.alert.open("Success", "Gratuity Sent For Approval");
        }, function () {
            $("#btnSendForApproval").prop("disabled", false);
        });
    },

    ReportView: function () {
        $.alert.open("Info", "This report is not available.");
    }
};

var Req = {
    POST: {
        SendGeneratedGratuityForApproval: function (Data, callback, err_callback) {
            $.ajax({
                url: '/GratuityProcess/SendGeneratedGratuityForApproval2',
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
        , ApproveGratuitySendForApproval: function (Data, callback, err_callback) {
            $.ajax({
                url: '/GratuityProcess/ApproveGratuitySendForApproval',
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

//$(function () {
//    Page.Load();
//});