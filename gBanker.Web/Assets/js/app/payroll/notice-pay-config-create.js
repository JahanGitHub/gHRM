"use strict";

var Page = {
    List: [],
    IsNumber: function (Value) {
        return ![null, ""].includes(Value) && !isNaN(Value);
    },
    Load: function () {
        this.BindEvents();
    },
    BindEvents: function () {
        $("#EffectiveStartDate").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true
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
    },
    Save: function () {
        $("#btnSave").prop("disabled", true);
        var NoticePeriod = $("#NoticePeriod").val();
        var EffectiveStartDate = $("#EffectiveStartDate").val();
        var CalculationFrom = $("#CalculationFrom").val();
        var SalaryPer = $("#SalaryPer").val();
        SalaryPer = Page.IsNumber(SalaryPer) ? parseFloat(SalaryPer) : 0;

        if (0 == NoticePeriod) {
            $.alert.open("Error", "Notice Period is required!");
            return;
        }
        if ("" == EffectiveStartDate) {
            $.alert.open("Error", "Effective Date is required!");
            return;
        }
        if ("" == CalculationFrom) {
            $.alert.open("Error", "Calculation From is required!");
            return;
        }
        if (0 == SalaryPer) {
            $.alert.open("Error", "Salary Percentage is required!");
            return;
        }
        var fdata = new FormData();
        var token = $("input[name='__RequestVerificationToken']").val();
        fdata.append("__RequestVerificationToken", token);
        fdata.append("Data", JSON.stringify({
            NoticePeriod: NoticePeriod,
            IsCalcFromBasic: "B" == CalculationFrom,
            SalaryPer: SalaryPer,
            EffectiveStartDate: EffectiveStartDate
        }));
        Req.POST.Save(fdata, function () {
            $.alert.open("Success", "Data Saved successfully!");
            setTimeout(function () {
                window.location.href = "/NoticePayConfig/Index";
            }, 1000);
        }, function () {
            $("#btnSave").prop("disabled", false);
        });
    }
};

var Req = {
    POST: {
        Save: function (Data, callback, err_callback) {
            $.ajax({
                url: '/NoticePayConfig/Save',
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