"use strict";

var Page = {
    EmployeeId: 0,
    List: [],
    IsNumber: function (Value) {
        return ![null, ""].includes(Value) && !isNaN(Value);
    },
    Load: function () {
        this.BindEvents();
    },
    GetData: function (Key) {
        return $("#page-data").attr("data-" + Key);
    },
    BindEvents: function () {
        $("#EffectiveStartDate").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
            });
        $("#EffectiveStartDate").datepicker('setDate', new Date());
        $(document).on("keydown", ".number-value", function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        $("[name='ConfigType']").change(function () {
            var ConfigType = $(this).val();
            if ("PayrollDesignation" == ConfigType) {
                $(".payroll-designation-control").show();
                $(".employee-code-control").hide();
            } else if ("EmployeeCode" == ConfigType) {
                $(".payroll-designation-control").hide();
                $(".employee-code-control").show();
            }
        });
        $("#EmployeeCode").blur(function () {
            var data = JSON.stringify({ Code: $("#EmployeeCode").val() });
            Req.POST.GetEmployeeShortInfoByCode(data, function (response) {
                Page.EmployeeId = response.EmployeeId;
                $("#EmployeeName").val(response.EmployeeName);
                $("#OfficeName").val(response.OfficeName);
                $("#DepartmentName").val(response.DepartmentName);
                $("#DesignationName").val(response.DesignationName);
                $("#OffcDesignName").val(response.ResponsibilityName);
            }, function () {
                $("#btnSave").prop("disabled", false);
            });
        });
    },
    Save: function () {
        $("#btnSave").prop("disabled", true);
        var ConfigType = $("[name='ConfigType']:checked").val();
        var PayrollDesignationId = $("#PayrollDesignationId").val();
        var EmployeeCode = $("#EmployeeCode").val();
        var EffectiveStartDate = $("#EffectiveStartDate").val();
        var WorkingDayMax = $("#WorkingDayMax").val();
        var HolidayMax = $("#HolidayMax").val();
        var MonthlyMax = $("#MonthlyMax").val();
        var ManualOvertimeOnly = "y" == $("[name='ManualOvertimeOnly']:checked").val();
        PayrollDesignationId = Page.IsNumber(PayrollDesignationId) ? parseInt(PayrollDesignationId) : 0;
        WorkingDayMax = Page.IsNumber(WorkingDayMax) ? parseFloat(WorkingDayMax) : 0;
        HolidayMax = Page.IsNumber(HolidayMax) ? parseFloat(HolidayMax) : 0;
        MonthlyMax = Page.IsNumber(MonthlyMax) ? parseFloat(MonthlyMax) : 0;

        if (!Page.IsValid()) {
            $("#btnSave").prop("disabled", false);
            return;
        }
        var fdata = new FormData();
        var token = $("input[name='__RequestVerificationToken']").val();
        fdata.append("__RequestVerificationToken", token);
        fdata.append("Data", JSON.stringify({
            EmployeeDesignationId: PayrollDesignationId,
            EmployeeId: Page.EmployeeId,
            WorkingDayMax: WorkingDayMax,
            HolidayMax: HolidayMax,
            MonthlyMax: MonthlyMax,
            ManualOvertimeOnly: ManualOvertimeOnly,
            EffectiveStartDate: EffectiveStartDate
        }));
        Req.POST.Save(fdata, function () {
            $.alert.open("Success", "Data Saved successfully!");
            setTimeout(function () {
                window.location.href = "/Overtime/ManualConfig";
            }, 1000);
        }, function () {
            $("#btnSave").prop("disabled", false);
        });
    },
    Clear: function () {
        $('input[type="text"], select').val('');
    },
    IsValid: function () {
        var ConfigType = $("[name='ConfigType']:checked").val();
        var PayrollDesignationId = $("#PayrollDesignationId").val();
        var EmployeeCode = $("#EmployeeCode").val();
        var EffectiveStartDate = $("#EffectiveStartDate").val();
        var WorkingDayMax = $("#WorkingDayMax").val();
        var HolidayMax = $("#HolidayMax").val();
        var MonthlyMax = $("#MonthlyMax").val();
        PayrollDesignationId = Page.IsNumber(PayrollDesignationId) ? parseInt(PayrollDesignationId) : 0;
        WorkingDayMax = Page.IsNumber(WorkingDayMax) ? parseFloat(WorkingDayMax) : 0;
        HolidayMax = Page.IsNumber(HolidayMax) ? parseFloat(HolidayMax) : 0;
        MonthlyMax = Page.IsNumber(MonthlyMax) ? parseFloat(MonthlyMax) : 0;

        if ("PayrollDesignation" == ConfigType && 0 == PayrollDesignationId) {
            $.alert.open("Error", "Payroll Designation is Required");
            return false;
        }
        if ("EmployeeCode" == ConfigType && [null, ""].includes(EmployeeCode)) {
            $.alert.open("Error", "Employee Code is Required");
            return false;
        }
        if ([null, ""].includes(EffectiveStartDate)) {
            $.alert.open("Error", "Effective Start Date is Required");
            return false;
        }
        if ([null, ""].includes($("#WorkingDayMax").val()) || WorkingDayMax < 0) {
            $.alert.open("Error", "Working Day Max is Required");
            return false;
        }
        if (WorkingDayMax > parseInt(WorkingDayMax)) {
            $.alert.open("Error", "Working Day Max must be an integer number");
            return false;
        }
        if ([null, ""].includes($("#HolidayMax").val()) || HolidayMax < 0) {
            $.alert.open("Error", "Holiday Max is Required");
            return false;
        }
        if (HolidayMax > parseInt(HolidayMax)) {
            $.alert.open("Error", "Holiday Max must be an integer number");
            return false;
        }
        if ([null, ""].includes($("#MonthlyMax").val()) || MonthlyMax < 0) {
            $.alert.open("Error", "Monthly Max is Required");
            return false;
        }
        if (MonthlyMax > parseInt(MonthlyMax)) {
            $.alert.open("Error", "Monthly Max must be an integer number");
            return false;
        }
        return true;
    }
};

var Req = {
    POST: {
        GetEmployeeShortInfoByCode: function (Data, callback, err_callback) {
            $.ajax({
                url: '/Employee/GetEmployeeShortInfoByCode',
                type: 'Post',
                data: Data,
                dataType: 'json',
                async: true,
                contentType: 'application/json',
                success: function (response) {

                    if (!response.success) {
                        $.alert.open("Error", response.message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(response.data); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("Error", data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
                    if (null != err_callback) { err_callback(); }
                }
            });
        },
        Save: function (Data, callback, err_callback) {
            $.ajax({
                url: '/Overtime/ManualConfigSave',
                type: 'Post',
                data: Data,
                async: true,
                contentType: false,
                processData: false,
                success: function (response) {

                    if ("Error" == response.Result) {
                        $.alert.open("Error", response.Message);
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