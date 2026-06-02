
////////
function takeManualAttendance() {

    var EmployeeName = $("#EmployeeName").val();
    var EmployeeId = $("#EmployeeId").val();
    var Clock = $("#Clock").val();
    var remark = $("#Remarks").val();
    var AttOfficeDayTypeId = 1;

    if (AttOfficeDayTypeId == 0) {
        $.alert.open("Message", "Please Select Office Day Type.")

        return false;
    }

    if (EmployeeName == '' || EmployeeId == '') {
        $(".danger .create-content").show(700).fadeToggle(2000);
        return false;
    }

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Attendance/CreateTime',
        data: { EmployeeId: EmployeeId, remark: remark, Clock: Clock, AttOfficeDayTypeId: AttOfficeDayTypeId, },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.Message);
        },
        error: function (request, status, error) {
            //$.alert.open(request.statusText + "/" + request.statusText + "/" + error);
            $.alert.open("Message", "Data not Saved.");

        }
        ,
    });

};
//// End of Button


//Start Check If Employee On Leave
function LoadEmpLeaveInfoByCode(employee_code) {
    var result = 'true';
    return $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Attendance/GetEmpLeaveInfoByCode', //'@Url.Action("GetEmpLeaveInfoByCode", "Attendance", "http")',
        data: { employeeID: employee_code },
        dataType: 'json',
        async: false,
        success: function (data) {
            result = 'false';
            $("#isInLeave").val('true');
            $.alert.open("Message", "Employee On Leave.");
        },
        error: function (request, status, error) {
            $("#isInLeave").val('true');
            //alert("error: True");
            result = 'true';

        }
    });

    return result;
}

//End Check If Employee On Leave
function LoadEmpInfoByCode(employee_code) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Attendance/GetEmpInfoByCode', //'@Url.Action("GetEmpInfoByCode", "Attendance", "http")',
        data: { employeeID: employee_code },
        dataType: 'json',
        async: false,
        success: function (data) {

            $.each(data, function (index, data) {

                if (data != "Error") {
                    $("#EmployeeName").val(data.EmployeeName);
                    $("#txtEmpName").val(data.EmployeeCode);
                    $("#EmployeeId").val(data.EmployeeId);
                    var validEmployee = data.ValidOfficeEmployee;
                     
                    if (validEmployee == 'Yes') {
                        $("#btnEntrySave").show();

                    }
                    else {
                        $("#btnEntrySave").hide();
                    }
                    if (data.EmployeeName == '') {
                        $("#EmployeeName").val('');
                        $("#EmployeeId").val('');
                    }
                }
                else {
                    $("#EmployeeName").val('');
                    //alert("Wrong Employe Code");
                    $("#txtEmpName").focus();
                    $("#EmployeeId").val('');
                }
            });
        },
        error: function (request) {
            var Err = $("#errFound").val();
            if (Err == '') {
                if (request.statusText == 'Not Found') {
                    $.alert.open("Message", "Employee Not Found.");

                }
                else {
                    $.alert.open("Message", request.statusText + "/" + request.statusText + "/" + error);

                }
                $("#errFound").val(1);
            }
            $("#errFound").val('');
            $("#EmployeeName").val('');
            //alert("Wrong Employe Code");
            $("#txtEmpName").focus();
            $("#EmployeeId").val('');
        }
    });
}

function LoadClock() {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Attendance/GetClock', //'@Url.Action("GetClock", "Attendance", "http")',
        data: {},
        dataType: 'json',
        async: true,
        success: function (data) {
            $.each(data, function (index, data) {
                if (data != "Error") {
                    $("#Clock").val(data.Clock);
                    $("#CurrentDate").val(data.CurrentDate);
                    document.getElementById("DigiClock").innerHTML = data.Clock;
                    document.getElementById("DigiDate").innerHTML = data.CurrentDate;
                }
                else {
                    $("#hour").empty();
                    // alert("Wrong ");
                }
            });
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Server Error.");
        }
    });
}


function GetOfficeDayTypeDropdown() {
    var ddlOfficeDayD = $("#AttOfficeDayTypeId");


    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/Attendance/GetOfficeDayTypeList',//'@Url.Action("GetOfficeDayTypeList", "Attendance", "http")',
        data: {},
        dataType: 'json',
        async: true,
        success: function (data) {
            ddlOfficeDayD.html('');
            $.each(data, function (id, option) {
                ddlOfficeDayD.append($('<option></option>').val(option.Value).html(option.Text));
            });
        },
        error: function (request, status, error) {
            $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

$(document).ready(function () {

    var initialEmployeeCode = $("#EmployeeCode").val();
    var loggedInCompanyCode = $("#CompanyCode").val();

    $("#btnEntrySave").hide();

    if (initialEmployeeCode != '' && loggedInCompanyCode == 'GT') {
        LoadEmpInfoByCode(initialEmployeeCode);
        $("#txtEmpName").prop('readonly', true);
    }
    

    
    //GetOfficeDayTypeDropdown();
    $("#AttenDate").datepicker(
       {
           dateFormat: "dd-M-yy",
           showAnim: "scale",
           changeMonth: true,
           changeYear: true,
           yearRange: "1920:2050"

       });
    $("#AttenDate").datepicker(
  'setDate', new Date());

    $("input[type='text']").change(function () {
        // your code
        $("#EmployeeName").val('');
        $("#EmployeeId").val('');

      

        var employee_code = $("#txtEmpName").val();
        if ($("#txtEmpName").val().trim() != '') {

            var result = '';
            LoadEmpLeaveInfoByCode(employee_code);
            result = $("#isInLeave").val();

            // var isInLeave = $("#isInLeave").val();
            //alert(result);
            if (result != '' & result == 'true') {
                LoadEmpInfoByCode(employee_code); //Employee Not in Leave.
            }
        }
        else {
            $("#EmployeeName").val('');
        }
    });
});

///CLOCK
setInterval(function () {
    LoadClock();
}, 1000);

///End CLOCK