

var appADImport = {
    populateSalaryFromToDate: function () {
        var salaryYear = $('#SalaryYear').val();
        var salaryMonth = $("#SalaryMonth option:selected").text();
        var salaryMonthInValue = $("#SalaryMonth").val();

        if (!salaryYear || salaryYear === '' || !salaryMonth || salaryMonth === 'Please Select') return;

        salaryMonth = salaryMonth.substring(0,3);
        var firstDateInString = `01-${salaryMonth}-${salaryYear}`;
        var fromDate = new Date(`${firstDateInString}`);
        var lastDateOfMonth = new Date(salaryYear, salaryMonthInValue, 0);
        var toDate = `${$.datepicker.formatDate('dd-M-yy', lastDateOfMonth)}`;

        $('#StartDate').val(firstDateInString);
        $('#EndDate').val(toDate);
    }
}

$(document).ready(function () {

    $('#SalaryYear,#SalaryMonth').on('change', function () {   
        $('#StartDate').val('');
        $('#EndDate').val('');
        appADImport.populateSalaryFromToDate();
    });

    //submit to import contacts
    /*$('#import-allowance-deduction-form').on('submit', function () {
        event.preventDefault();

        //for form validation
        var isValid = app.validateForm('#import-allowance-deduction-form');
        if (!isValid) return;

        var buttonPreviousText = $('.btn-import-allowance-deduction').html();

        $('.btn-import-allowance-deduction').attr('disabled', 'disabled');
        $('.btn-import-allowance-deduction').html('<i class="fa fa-spinner fa-spin fa-fw"></i>\r\n' +
            '<span>Processing...</span>');

        var form = $('#import-allowance-deduction-form');
        var fd = new FormData();
        var inputs = $("#import-allowance-deduction-form :input");
        for (i = 0; i < inputs.length; i++) {
            fd.append(inputs[i].name, inputs[i].value);
        }

        var fileUpload = $("#import-allowance-deduction-form #file").get(0);
        var files = fileUpload.files;

        if (files.length > 0) {
            var file = files[0];
            fd.append("file", file);
        }

        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            processData: false,
            contentType: false,
            data: fd
        }).done(function (data) {
            $('.btn-import-allowance-deduction').html('').html(buttonPreviousText).removeAttr('disabled');
            var result = new Object();
            result.message = data.message;
            result.type = data.type;
            app.showConfirmation(result);
        });
    });*/
});

