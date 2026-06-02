
var processLogManager = {

}

$(function () {
    if ($("#IsOpen").val()==='True') {
        $("#StartDate").prop('disabled', true);
        $("#btnSaveProcessLog").prop('disabled', true);
        $("#btnReset").prop('disabled', true);
    }
    else {
        $("#StartDate").prop('disabled', false);
        $("#btnSaveProcessLog").prop('disabled', false);
        $("#btnReset").prop('disabled', false);
    }

    $("#StartDate").datepicker(
      {
          dateFormat: "dd-M-yy",
          showAnim: "scale",
          changeMonth: true,
          changeYear: true
      });

    $("#btnReset").click(function () {
        clear();
    });

    $("#btnSaveProcessLog").click(function () {

        $.alert.open('confirm', 'Are you sure you want to add?', function (button) {
            if (button == 'yes') {
                Add();
                return true;
            }
            else {
                return false;
            }
        });

    });

    //loading single pf
    $('#grid2').jtable({
        title: 'Day Initialization List',
        paging: true,
        pageSize: 5,
        sorting: false,
        defaultSorting: 'Name ASC',
        actions: {
            listAction: '/PFProcessLog/GetProcessLogList'
        },
        fields: {
            ProcessLogId: {
                key: true,
                list: false,
                create: false,
                edit: false
            },
            StartDate: {
                title: 'Start Date',
                width: '30%'
            },
            SystemDateAtDayStart: {
                title: 'System Date [Day Open]',
                width: '30%'
            },
            SystemDateAtDayEnd: {
                title: 'System Date [Day Close]',
                width: '30%'
            },
            IsOpen: {
                title: 'Day Status',
                width: '10%',
                display: function (data) {
                    if (data.record.IsOpen == true)
                        return "Open";
                    if (data.record.IsOpen == false)
                        return "Closed";
                }
            }
        }
    });

    reloadGrid2();

    $("#filterColumn").change(function () {
        if ($(this).val() === "ViewAll") {
            $("#filterValue").val('');
        }
    });

});

function IsValidStartDate(startdate) {
    var result = '';
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFProcessLog/IsValidStartDate',
        data: { startdate: startdate },
        dataType: 'json',
        async: false,
        success: function (data) {
            result = data.message;
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Not Saved");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
    return result;
}

function SaveProcessLog(startDate) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFProcessLog/SaveProcessLog',
        data: { startDate: startDate },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
            if (data.status == 'ok') {
                $("#DayStatus").val(data.DayStatus);
                $("#TransactionDate").val(data.TransactionDate);
                $("#StartDate").prop('disabled', true);
                $("#btnSaveProcessLog").prop('disabled', true);
                $("#btnReset").prop('disabled', true);

                clear();
            }
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Data Not Saved");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
}

function clear() {
    $("#StartDate").val('');
}

function Add() {
    var startDate = $("#StartDate").val();
    if (startDate == '') {
        $.alert.open("Please enter Start Date");
        return;
    }
    var result = IsValidStartDate(startDate);
    if (result != '') {
        $.alert.open("Message", result);
        return;
    }
    SaveProcessLog(startDate);

}

function reloadGrid2() {
    $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
}
