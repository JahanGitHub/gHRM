
function ProcessVoucher(transDate) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFVoucherProcess/ProcessVoucher',
        data: { transDate: transDate },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Unable to Process");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
}

function VerifyVoucher(transDate) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PFVoucherProcess/VerifyVoucher',
        data: { transDate: transDate },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        },
        error: function (request, status, error) {
            $.alert.open("Message", "Unable to Process");
            $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
        }
    });
}

function Process() {
    var transDate = $('#TransactionDate').val();
    if (transDate == '') {
        alert('Check Transaction date');
        return;
    }
    ProcessVoucher(transDate);
    reloadGrid2();
}

function Verify() {
    var transDate = $('#TransactionDate').val();
    if (transDate == '') {
        alert('Check Transaction date');
        return;
    }
    VerifyVoucher(transDate);
}

$(document).ready(function () {   
    if ($("#IsOpen").val() == "True") {
        $("#btnProcessVoucher").prop('disabled', false);
        $("#btnVerifyVoucher").prop('disabled', false);
    }
    else {
        $("#btnProcessVoucher").prop('disabled', true);
        $("#btnVerifyVoucher").prop('disabled', true);
    }

    $("#btnProcessVoucher").click(function () {
        $.alert.open('confirm', 'Are you sure you want to process voucher?', function (button) {
            if (button == 'yes') {
                Process();
                return true;
            }
            else {
                return false;
            }
        });
    });

    $("#btnVerifyVoucher").click(function () {

        $.alert.open('confirm', 'Are you sure you want to verify voucher?', function (button) {
            if (button == 'yes') {
                Verify();
                return true;
            }
            else {
                return false;
            }
        });
    });


    //Asad added today
    $('#grid2').jtable({
        //title: 'Employee Drop List',
        paging: true,
        pageSize: 5,
        sorting: false,
        defaultSorting: 'Name ASC',
        actions: {
            listAction: '/PFVoucherProcess/GetVoucherList'
        },
        fields: {
            SerialNo: {
                key: true,
                list: false,
                create: false,
                edit: false
            },
            SerialNo: {
                title: 'Serial No',
                width: '10%'
            },
            TransactionDate: {
                title: 'Transaction Date',
                width: '15%'
            },
            VoucherNo: {
                title: 'VoucherNo',
                width: '10%'
            },
            AccountCode: {
                title: 'AccountCode',
                width: '10%'
            },
            Dr: {
                title: 'Dr',
                width: '10%'
            },
            Cr: {
                title: 'Cr',
                width: '15%'
            },
            TransactionType: {
                title: 'Transaction Type',
                width: '10%'
            },
            Particulars: {
                title: 'Particulars',
                width: '20%'
            }
        }
    });

    //reloadGrid2();
    $("#filterColumn").change(function () {
        if ($(this).val() === "ViewAll") {
            $("#filterValue").val('');
        }
    });

    function reloadGrid2() {
        $('#grid2').jtable('load', { filterColumn: $('#filterColumn').val(), filterValue: $('#filterValue').val() });
    }
});
