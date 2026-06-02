var Page = {
    List: [],
    IsNumber: function (Value) {
        return ![null, ""].includes(Value) && !isNaN(Value);
    },
    Load: function () {
        this.BindEvents();
        if (Edit_ReportName != "") {
            $("#ReportName").val(Edit_ReportName);
            $("#SignEmp_Code1").val(Edit_SignEmp_Id1);
            Page.SignEmp_Code1Blur();
            $("#SignEmp_Code2").val(Edit_SignEmp_Id2);
            Page.SignEmp_Code2Blur();
            $("#SignEmp_Code3").val(Edit_SignEmp_Id3);
            Page.SignEmp_Code3Blur();
            $("#SignEmp_Code4").val(Edit_SignEmp_Id4);
            Page.SignEmp_Code4Blur();
            $("#SignEmp_Code5").val(Edit_SignEmp_Id5);
            Page.SignEmp_Code5Blur();
            $("#SignEmp_Code6").val(Edit_SignEmp_Id6);
            Page.SignEmp_Code6Blur();
        }
    },
    SignEmp_Code1Blur: function () {
        $("#SignEmp_Detail1").html("");
        var data = JSON.stringify({ Code: $("#SignEmp_Code1").val() });
        Req.POST.GetEmployeeShortInfoByCode(data, function (response) {
            if (response.EmployeeId == 0) {
                $("#SignEmp_Id1").val(0);
                $("#SignEmp_Detail1").html("");
                return;
            }
            $("#SignEmp_Id1").val(response.EmployeeId);
            var Html = "Name: <b>" + response.EmployeeName + "</b>, " +
                "Office: <b>" + response.OfficeName + "</b>, " +
                "Department: <b>" + response.DepartmentName + "</b>, " +
                "DesignationName: <b>" + response.DesignationName + "</b>";
            $("#SignEmp_Detail1").html(Html);
        }, function () {
            $("#SignEmp_Id1").val(0);
        });
    },
    SignEmp_Code2Blur: function () {
        $("#SignEmp_Detail2").html("");
        var data = JSON.stringify({ Code: $("#SignEmp_Code2").val() });
        Req.POST.GetEmployeeShortInfoByCode(data, function (response) {
            if (response.EmployeeId == 0) {
                $("#SignEmp_Id2").val(0);
                $("#SignEmp_Detail2").html("");
                return;
            }
            $("#SignEmp_Id2").val(response.EmployeeId);
            var Html = "Name: <b>" + response.EmployeeName + "</b>, " +
                "Office: <b>" + response.OfficeName + "</b>, " +
                "Department: <b>" + response.DepartmentName + "</b>, " +
                "DesignationName: <b>" + response.DesignationName + "</b>";
            $("#SignEmp_Detail2").html(Html);
        }, function () {
            $("#SignEmp_Id2").val(0);
        });
    },
    SignEmp_Code3Blur: function () {
        $("#SignEmp_Detail3").html("");
        var data = JSON.stringify({ Code: $("#SignEmp_Code3").val() });
        Req.POST.GetEmployeeShortInfoByCode(data, function (response) {
            if (response.EmployeeId == 0) {
                $("#SignEmp_Id3").val(0);
                $("#SignEmp_Detail3").html("");
                return;
            }
            $("#SignEmp_Id3").val(response.EmployeeId);
            var Html = "Name: <b>" + response.EmployeeName + "</b>, " +
                "Office: <b>" + response.OfficeName + "</b>, " +
                "Department: <b>" + response.DepartmentName + "</b>, " +
                "DesignationName: <b>" + response.DesignationName + "</b>";
            $("#SignEmp_Detail3").html(Html);
        }, function () {
            $("#SignEmp_Id3").val(0);
        });
    },
    SignEmp_Code4Blur: function () {
        $("#SignEmp_Detail4").html("");
        var data = JSON.stringify({ Code: $("#SignEmp_Code4").val() });
        Req.POST.GetEmployeeShortInfoByCode(data, function (response) {
            if (response.EmployeeId == 0) {
                $("#SignEmp_Id4").val(0);
                $("#SignEmp_Detail4").html("");
                return;
            }
            $("#SignEmp_Id4").val(response.EmployeeId);
            var Html = "Name: <b>" + response.EmployeeName + "</b>, " +
                "Office: <b>" + response.OfficeName + "</b>, " +
                "Department: <b>" + response.DepartmentName + "</b>, " +
                "DesignationName: <b>" + response.DesignationName + "</b>";
            $("#SignEmp_Detail4").html(Html);
        }, function () {
            $("#SignEmp_Id4").val(0);
        });
    },

    SignEmp_Code5Blur: function () {
        $("#SignEmp_Detail5").html("");
        var data = JSON.stringify({ Code: $("#SignEmp_Code5").val() });
        Req.POST.GetEmployeeShortInfoByCode(data, function (response) {
            if (response.EmployeeId == 0) {
                $("#SignEmp_Id5").val(0);
                $("#SignEmp_Detail5").html("");
                return;
            }
            $("#SignEmp_Id5").val(response.EmployeeId);
            var Html = "Name: <b>" + response.EmployeeName + "</b>, " +
                "Office: <b>" + response.OfficeName + "</b>, " +
                "Department: <b>" + response.DepartmentName + "</b>, " +
                "DesignationName: <b>" + response.DesignationName + "</b>";
            $("#SignEmp_Detail5").html(Html);
        }, function () {
            $("#SignEmp_Id5").val(0);
        });
    },


    SignEmp_Code6Blur: function () {
        $("#SignEmp_Detail6").html("");
        var data = JSON.stringify({ Code: $("#SignEmp_Code6").val() });
        Req.POST.GetEmployeeShortInfoByCode(data, function (response) {
            if (response.EmployeeId == 0) {
                $("#SignEmp_Id6").val(0);
                $("#SignEmp_Detail6").html("");
                return;
            }
            $("#SignEmp_Id6").val(response.EmployeeId);
            var Html = "Name: <b>" + response.EmployeeName + "</b>, " +
                "Office: <b>" + response.OfficeName + "</b>, " +
                "Department: <b>" + response.DepartmentName + "</b>, " +
                "DesignationName: <b>" + response.DesignationName + "</b>";
            $("#SignEmp_Detail6").html(Html);
        }, function () {
            $("#SignEmp_Id6").val(0);
        });
    },

    BindEvents: function () {
        $("#SignEmp_Code1").blur(function () {
            Page.SignEmp_Code1Blur();
        });
        $("#SignEmp_Code2").blur(function () {
            Page.SignEmp_Code2Blur();
        });
        $("#SignEmp_Code3").blur(function () {
            Page.SignEmp_Code3Blur();
        });
        $("#SignEmp_Code4").blur(function () {
            Page.SignEmp_Code4Blur();
        });

        $("#SignEmp_Code5").blur(function () {
            Page.SignEmp_Code5Blur();
        });

        $("#SignEmp_Code6").blur(function () {
            Page.SignEmp_Code6Blur();
        });

    },
    Save: function () {
        $("#btnSave").prop("disabled", true);
        var ReportCode = $("#ReportName").val();
        var ReportDes = $("#ReportName option:selected").html();
        var SignEmp_Id1 = $("#SignEmp_Id1").val();
        var SignEmp_Id2 = $("#SignEmp_Id2").val();
        var SignEmp_Id3 = $("#SignEmp_Id3").val();
        var SignEmp_Id4 = $("#SignEmp_Id4").val();
        var SignEmp_Id5 = $("#SignEmp_Id5").val();
        var SignEmp_Id6 = $("#SignEmp_Id6").val();

        SignEmp_Id1 = Page.IsNumber(SignEmp_Id1) ? parseInt(SignEmp_Id1) : 0;
        SignEmp_Id2 = Page.IsNumber(SignEmp_Id2) ? parseInt(SignEmp_Id2) : 0;
        SignEmp_Id3 = Page.IsNumber(SignEmp_Id3) ? parseInt(SignEmp_Id3) : 0;
        SignEmp_Id4 = Page.IsNumber(SignEmp_Id4) ? parseInt(SignEmp_Id4) : 0;
        SignEmp_Id5 = Page.IsNumber(SignEmp_Id5) ? parseInt(SignEmp_Id5) : 0;
        SignEmp_Id6 = Page.IsNumber(SignEmp_Id6) ? parseInt(SignEmp_Id6) : 0;

        var Data = {
            Id: Edit_Id,
            ReportCode: ReportCode,
            ReportDes: ReportDes,
            SignEmp_Id1: SignEmp_Id1,
            SignEmp_Id2: SignEmp_Id2,
            SignEmp_Id3: SignEmp_Id3,
            SignEmp_Id4: SignEmp_Id4,
            SignEmp_Id5: SignEmp_Id5,
            SignEmp_Id6: SignEmp_Id6
        };
        Req.POST.Save(JSON.stringify(Data), function () {
            $.alert.open("Success", "Data Saved successfully!");
            setTimeout(function () {
                window.location.href = "/ReportSignature/Index";
            }, 1000);
        }, function () {
            $("#btnSave").prop("disabled", false);
        });
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
                        $.alert.open("error", response.message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(response.data); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("error", data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
                    if (null != err_callback) { err_callback(); }
                }
            });
        },
        Save: function (Data, callback, err_callback) {
            $.ajax({
                url: '/ReportSignature/Save',
                type: 'Post',
                data: Data,
                dataType: 'json',
                async: true,
                contentType: 'application/json',
                success: function (response) {

                    if ("Error" == response.Result) {
                        $.alert.open("error", response.Message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("error", data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
                    if (null != err_callback) { err_callback(); }
                }
            });
        }
    }
};

$(function () {
    Page.Load();
});