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
        $("#InformDate, #ResignDate").datepicker(
            {
                dateFormat: "dd-M-yy",
                showAnim: "scale",
                changeMonth: true,
                changeYear: true
            });
        $("#InformDate, #ResignDate").datepicker('setDate', new Date());
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
                Page.EmployeeId = 0;
                $("#btnSave").prop("disabled", false);
            });
        });
    },
    Save: function () {
        $("#btnSave").prop("disabled", true);
        var InformDate = $("#InformDate").val();
        var ResignDate = $("#ResignDate").val();
        var Remark = $("#Remark").val();

        /*if (!Page.IsValid()) {
            $("#btnSave").prop("disabled", false);
            return;
        }*/
        var fdata = new FormData();
        var token = $("input[name='__RequestVerificationToken']").val();
        fdata.append("__RequestVerificationToken", token);
        fdata.append("Data", JSON.stringify({
            InformDate: InformDate,
            ResignDate: ResignDate,
            EmployeeId: Page.EmployeeId,
            Remark: Remark
        }));
        Req.POST.Save(fdata, function () {
            $.alert.open("Success", "Data Saved successfully!");
            setTimeout(function () {
                window.location.href = "/ResignNotice/Index";
            }, 1000);
        }, function () {
            $("#btnSave").prop("disabled", false);
        });
    },
    Clear: function () {
        $('input[type="text"], select').val('');
    },
    IsValid: function () {
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
                url: '/ResignNotice/Save',
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