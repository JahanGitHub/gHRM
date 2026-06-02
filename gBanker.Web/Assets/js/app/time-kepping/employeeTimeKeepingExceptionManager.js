
$(document).ready(function () {

    var options = app.timePickerCommonOptions();
    $("#LoginTime").wickedpicker(options);
    $("#LoginTime").val("10 : 00");

    $("#LogoutTime").wickedpicker(options);
    $("#LogoutTime").val("18 : 00");

    $("#LastLoginTime").wickedpicker(options);
    $("#LastLoginTime").val("10 : 00");

    //enable timepicker as editable
    app.enableEditableTimepicker('.timepicker-editable');

    $("#LoginTime").keyup(function (e) {
        return timePickerValidation(e, '#LoginTime');
    });
    $("#LogoutTime").keyup(function (e) {
        return timePickerValidation(e, '#LogoutTime');
    });
    $("#LastLoginTime").keyup(function (e) {
        return timePickerValidation(e, '#LastLoginTime');
    });

    $("#LoginTime").keypress(function (e) {
        return checkNumeric(e); 
    });
    $("#LogoutTime").keypress(function (e) {
        return checkNumeric(e); 
    });
    $("#LastLoginTime").keypress(function (e) {
        return checkNumeric(e); 
    });
    
    $("#EmployeeCode").blur(function () {
        var empCode = $("#EmployeeCode").val();
        if (empCode != "") {
            $.ajax({
                type: "GET",
                dataType: "json",
                async: true,
                cache: false,
                url: '/EmployeeCommonInformation/GetEmpInfoByCode',
                data: { employee_code: empCode },
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (data.length > 0) {
                        $("#DepartmentId").val(data[0].DepartmentName);
                        $("#OfficeDesignationId").val(data[0].OfficeDesignationName);
                        $("#EmployeeName").val(data[0].EmployeeName);
                        $("#EmployeeId").val(data[0].EmployeeId);
                    } else {
                        $("#DepartmentId").val('');
                        $("#OfficeDesignationId").val('');
                        $("#EmployeeName").val('');
                    }

                },
                error: function (request, status, error) {
                    alert(request.statusText + "/" + request.statusText + "/" + error);
                }

            });
        }
    });

    $("#btnUpdate").hide();
    $("#btnReset").hide();

    $("#EventDate").datepicker(
    {
        dateFormat: "dd-M-yy",
        showAnim: "scale",
    });
    $("#EventDate").datepicker('setDate', new Date());

    $("#EventStartDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1950:2050"

    });
    $("#EventStartDate").datepicker('setDate', new Date());

    $("#EventEndDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1950:2050"

    });
    $("#EventEndDate").datepicker('setDate', new Date());

    
    $("#DepartmentId").change(function () {

        var DepartmentId = $("#DepartmentId").val();
        var dat = $("#OfficeDesignationId").val();
        if (DepartmentId != "" && dat == "") {
        } else if (DepartmentId != "" && dat != "") {
            GETDepartmentAndDesignation(DepartmentId);
        }
    });


    $("#OfficeDesignationId").change(function () {
        var OfficeDesignationId = $("#OfficeDesignationId").val();
        var DepartmentId = $("#DepartmentId").val();
        if (OfficeDesignationId != "" && DepartmentId == "") {
        } else if (OfficeDesignationId != "" && DepartmentId != "") {
            GETDesignationAndDepartment(OfficeDesignationId);
        }
    });

    GridLoad();
});

function checkNumeric(event) {
    var key = window.event ? event.keyCode : event.which;
    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 46
     || event.keyCode == 37 || event.keyCode == 39) {
        return true;
    }
    if (event.which === 13) {
        $(this).next().focus();
    }
    else if (key < 48 || key > 58) {
        return false;
    }
    else return true;
}

function GETDepartmentAndDesignation(DepartmentId) {
    var ddlEASSProfile = $("#EmployeeId");
    var OfficeDesignationId = $("#OfficeDesignationId").val();
    if (DepartmentId > 0 && OfficeDesignationId > 0) {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/EmployeeTimeKeepingException/GETDepartmentAndDesignation)',
            data: { DepartmentId: DepartmentId, OfficeDesignationId: OfficeDesignationId },
            dataType: 'json',
            success: function (data) {
                ddlEASSProfile.html('');
                $.each(data.data, function (id, option) {
                    ddlEASSProfile.append($('<option></option>').val(option.Value).html(option.Text));
                });
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function GETDesignationAndDepartment(OfficeDesignationId) {
    var ddlEASSProfile = $("#EmployeeId");
    var DepartmentId = $("#DepartmentId").val();
    if (OfficeDesignationId > 0 && DepartmentId > 0) {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/EmployeeTimeKeepingException/GETDesignationAndDepartment)',
            data: { OfficeDesignationId: OfficeDesignationId, DepartmentId: DepartmentId },
            dataType: 'json',
            success: function (data) {
                ddlEASSProfile.html('');
                $.each(data.data, function (id, option) {
                    ddlEASSProfile.append($('<option></option>').val(option.Value).html(option.Text));
                });
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function GenerateETK() {
    var item = {};
    item.EmployeeId = $("#EmployeeId").val();
    item.EventStartDate = $("#EventStartDate").val();
    item.EventEndDate = $("#EventEndDate").val();
    item.LoginTime = $("#LoginTime").val();
    item.LogoutTime = $("#LogoutTime").val();
    item.LastLoginTime = $("#LastLoginTime").val();
    item.AttendenceTypeId = $("#AttendenceTypeId").val();
    item.Justification = $("#Justification").val();
    return item;
}

function GenerateUpdateETK() {
    var item = {};
    item.Id = $("#Id").val();
    item.EmployeeId = $("#EmployeeId").val();
    item.EventDate = $("#EventStartDate").val();
    item.LoginTime = $("#LoginTime").val();
    item.LogoutTime = $("#LogoutTime").val();
    item.LastLoginTime = $("#LastLoginTime").val();
    item.AttendenceTypeId = $("#AttendenceTypeId").val();
    item.Justification = $("#Justification").val();
    return item;
}

function ClearControl() {
    $("#Id").val('');
    $("#EmployeeId").val('');
    $("#EventDate").val('');
    $("#AttendenceTypeId").val('');
    $("#DepartmentId").val('');
    $("#OfficeDesignationId").val('');
    $("#Justification").val('');
    $("#EmployeeName").val('');
    $("#EmployeeCode").val('');
    $("#LoginTime").val("10 : 00");
    $("#LogoutTime").val("18 : 00");
    $("#LastLoginTime").val("10 : 00");

    $("#EventStartDate").attr("disabled", false).val('');
    $("#EventEndDate").attr("disabled", false).val('');
}

function SaveEmployeeTimeKeepingException() {
    var employeeTimeKeepingException = GenerateETK();

    var EventEndDate = $("#EventEndDate").val();
    if (EventEndDate != "" && EventEndDate != null) {
        $.ajax({
            type: "POST",
            dataType: "json",
            async: true,
            cache: false,
            url: '/EmployeeTimeKeepingException/SaveEmployeeTimeKeepingException',
            data: JSON.stringify({ employeeTimeKeepingException: employeeTimeKeepingException }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#AjaxLoader').hide();
                if (data.result == 1) {
                    GridLoad();
                    $.alert.open("Success", data.message);

                    //Referred By Biplob Vai.
                    //ClearControl();
                } else {
                    $.alert.open("Error", data.message);
                }
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }

        });
    } else {
        $.alert.open("Error", "Please select Event End Date");
    }
}

function UpdateEmployeeTimeKeepingException() {

    var employeeTimeKeepingException = GenerateUpdateETK();
    $.ajax({
        type: "POST",
        dataType: "json",
        async: true,
        cache: false,
        url: '/EmployeeTimeKeepingException/UpdateEmployeeTimeKeepingException',
        data: JSON.stringify({ employeeTimeKeepingException: employeeTimeKeepingException }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#AjaxLoader').hide();
            if (data.result == 1) {
                GridLoad();
                $.alert.open("Success", data.message);
                ClearControl();
            } else {
                $.alert.open("Error", data.message);
            }
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function InformationDelete(Id) {
    $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
        if (button == 'yes') {
            $.ajax({
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                url: '/EmployeeTimeKeepingException/InformationDeleteEmployeeTimeKeepingException',
                data: { Id: Id },
                dataType: 'json',
                async: true,
                success: function (data) {
                    if (data.result == 1) {
                        GridLoad();
                        $.alert.open("Error", data.message);
                    } else {
                        $.alert.open("Error", data.message);
                    }

                },
                error: function (request, status, error) {
                    alert(request.statusText + "/" + request.statusText + "/" + error);
                }
            });
            return true;
        }
        else {
            hiddenField.value = 'false';
            return false;
        }
    });
}

//edit using jtable
function ResetEASSManagment() {
    $("#btnUpdate").hide();
    $("#btnSave").show();
    $("#btnReset").hide();
    ClearControl();
}

function EditTada(a, b, c, d, e, f, g, h, i, j, k) {
    $("#Id").val(a);
    $("#EmployeeCode").val(b);
    $("#EventStartDate").attr("disabled", "disabled").val(c);
    $("#AttendenceTypeId").val(d);
    $("#DepartmentId").val(e);
    $("#OfficeDesignationId").val(f);
    $("#Justification").val(g);
    $("#EmployeeName").val(h);
    $("#EmployeeId").val(i);
    
    $("#LoginTime").val(j);
    $("#LogoutTime").val(k);
    $("#LastLoginTime").val(k);

    $("#EventEndDate").attr("disabled", "disabled").val('');
    $("#btnUpdate").show();
    $("#btnReset").show();
    $("#btnSave").hide();
}

function GridLoad() {
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
                url: '/EmployeeTimeKeepingException/ListEmployeeTimeKeepingException',
                dataType: 'json'
            }
        }
    });

    $("#grid").kendoGrid({
        dataSource: dataSource,
        //height: 600,
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
                 field: "Id",
                 hidden: true,
                 filterable: false
             },            
            {
                 field: "EmployeeName",
                 title: "Employee Name",
                 width: "50px",
                 filterable: true
            },
             {
                 field: "EmployeeCode",
                 title: "Employee Code",
                 width: "40px",
                 filterable: true
             },
             {
                 width: "80px",
                 field: "AttenTypeFullName",
                 filterable: true,
                 title: "Attend Type Full Name",
                 //locked: true
             },
             {
                 width: "40px",
                 field: "ED",
                 title: "Event Date",
                 //locked: true
             },
             {
                 width: "40px",
                 field: "LT",
                 title: "Login Time",
                 //locked: true
             },             
             {
                 width: "40px",
                 field: "LOutT",
                 title: "Logout Time",
                 //locked: true
             },
             {
                 width: "40px",
                 field: "LastLoginTime",
                 title: "Last Login Time",
                 //locked: true
             },
             {
                 width: "80px",
                 field: "Justification",
                 title: "Justification",
                 //locked: true
             },

             {
                 width: "30px",
                 title: 'Edit',
                 template: function (data) {
                     var btn = "";
                     btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="EditTada(' + "'" + data.Id + "'" + ',' + "'" + data.EmployeeCode + "'" + ',' + "'" + data.ED + "'" + ',' + "'" + data.AttendenceTypeId + "'" + ',' + "'" + data.DepartmentName + "'" + ',' + "'" + data.OffcDesignName + "'" + ',' + "'" + data.Justification + "'" + ',' + "'" + data.EmployeeName + "'" + ',' + "'" + data.EmployeeId + "'" + ',' + "'" + data.LT + "'" + ',' + "'" + data.LOutT + "'" + ');"><i class="fa fa-pencil-square-o"></i></a></div>';                     
                     return btn;
                 }
            },
            {
                width: "30px",
                title: 'Delete',
                template: function (data) {

                    return '<div class="text-center" style="float:left;"><a href="#" OnClick="InformationDelete(' + "'" + data.Id + "'" + ');"><i class="fa fa-trash"></i></a></div>';

                }
            },


        ]
    });
}

function timePickerValidation(event, selector) {
    var selectedTime = $(selector).val();
    if (selectedTime) {
        var fragmentedTime = selectedTime.split(':');
        if (fragmentedTime.length === 2 && $.trim(fragmentedTime[1].length > 0)) {
            var hour = $.trim(fragmentedTime[0]);
            var minute = $.trim(fragmentedTime[1]);

            if (hour.length > 2 || hour > 24 || hour < 0) {
                var type = 'alert';
                var message = 'Hour must be 00 to 24';
                app.showValidationAlert(type, message);
                $(selector).val("");
                return;
            }
            if (minute.length > 2 || minute > 60 || minute < 0) {
                var type = 'alert';
                var message = 'Minute must be 00 to 60';
                app.showValidationAlert(type, message);
                $(selector).val("");
                return;
            }
        }
    }
}
