

var leaveSellManualManager = {

    LoadEmpInfo: function (employee_Code) {
        $('#AjaxLoader').show();
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/LeaveEncashment/GetLeaveSellAdviseInfo',
            data: { employee_Code: employee_Code },
            dataType: 'json',
            async: true,
            success: function (data) {
                $('#AjaxLoader').hide();
                if (data.result == 0) {
                    $.alert.open('Error', data.message);
                    leaveSellManualManager.ClearFormData();                   
                    return;
                }

                $("#EmployeeId").val(data.model.EmployeeId);
                $("#EmployeeName").val(data.model.EmployeeName);
                $('#Zone').val(data.model.Zone);
                $('#DMC').val(data.model.DMC);
                $('#DepartmentName').val(data.model.DepartmentName);
                $('#DesignationName').val(data.model.Designation);
                $('#LeaveSellNo').val(data.model.LeaveSellNo);
                $('#EncashedAmount').val(data.model.EncashedAmount);
                $("#TotalDays").val(data.model.TatalDays);
                $('#SaleDate').val(data.model.SaleDate);
                $('#RequestDate').val(data.model.RequestDate);
                $('#ApprovedDate').val(data.model.ApprovedDate);
                $('#Remarks').val(data.model.Remarks);

                //leaveSellManualManager.GetLeaveSellListByEmployee();

            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },
        
    ClearFormData: function () {
        $("#EmployeeId").val('');
        $("#EmployeeName").val('');
        $('#Zone').val('');
        $('#DMC').val('');
        $('#DepartmentName').val('');
        $('#DesignationName').val('');
        $('#LeaveSellNo').val('');
        $('#EncashedAmount').val('');
        $("#TotalDays").val('');
        $('#SaleDate').val('');
        $('#RequestDate').val('');
        $('#ApprovedDate').val('');
        $('#Remarks').val('');
    },    
}

$(function () {

    $("#RequestDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2025"
    });

    $("#SaleDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2025"
    });

    $("#EmployeeCode").keyup(function (e) {
        var keycode = e.keyCode ? e.keyCode : e.which;
        if (keycode == 8) {
            leaveSellManualManager.ClearFormData();
        }
    });

    $("#EmployeeCode").blur(function () {
        var employee_Code = $("#EmployeeCode").val();
        if (employee_Code != "") {
            leaveSellManualManager.LoadEmpInfo(employee_Code);
        }
        else {
            $("#OfficeName").empty();
            $("#DepartmentName").empty();
            $("#DesignationName").empty();
        }
    });  

    //submit to change userrole
    $('#add-leave-sell-advise-form').on('submit', function (event) {

        event.preventDefault();
        var form = $(this);

        //for form validation
        var isValid = app.validateForm('#add-leave-sell-advise-form');
        if (!isValid) return;

        $("#AjaxLoader").show();
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize()
        }).done(function (data) {
            //success alert
            $("#AjaxLoader").hide();
            if (data.result == 1) {
                
                $.alert.open("Success", data.message);
                //form clear
                leaveSellManualManager.ClearFormData();
            }
            else {
                $.alert.open("Error", data.message);
            }
        });

    });
});

