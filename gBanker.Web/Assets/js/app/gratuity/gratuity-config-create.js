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
        var EmployeeStatus = $("#EmployeeStatus").val();
        var EffectiveStartDate = $("#EffectiveStartDate").val();
        var ServiceAgeFrom = $("#ServiceAgeFrom").val();
        var ServiceAgeTo = $("#ServiceAgeTo").val();
        var GratuityTimes = $("#GratuityTimes").val();
        var EligibleFrom = $("#EligibleFrom").val();
        GratuityTimes = Page.IsNumber(GratuityTimes) ? parseFloat(GratuityTimes) : 0;



        // Validation Conditions
        if (!EmployeeStatus || !EffectiveStartDate || !ServiceAgeFrom || !ServiceAgeTo || !GratuityTimes || !EligibleFrom) {
            // If any of the fields are empty, show an error message and return
            $.alert.open("Error", "Please fill in all fields.");
            $("#btnSave").prop("disabled", false);
            return;
        }


        var fdata = new FormData();
        var token = $("input[name='__RequestVerificationToken']").val();
        fdata.append("__RequestVerificationToken", token);
        fdata.append("Data", JSON.stringify({
            EmployeeStatusId: EmployeeStatus,
            ServiceAgeFrom: ServiceAgeFrom,
            ServiceAgeTo: ServiceAgeTo,
            GratuityTimes: GratuityTimes,
            EffectiveStartDate: EffectiveStartDate,
            EligibleFrom: EligibleFrom
        }));
        Req.POST.Save(fdata, function () {
            $.alert.open("Success", "Data Saved successfully!");
            setTimeout(function () {
                window.location.href = "/GratuityConfig/Index";
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
                url: '/GratuityConfig/Save',
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