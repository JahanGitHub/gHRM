
$(document).ready(function () {

    //submit to import employee salry configuration
    $('#import-salary-configuration-form').on('submit', function () {
        event.preventDefault();

        //for form validation
        var isValid = app.validateForm('#import-salary-configuration-form');
        if (!isValid) return;

        var buttonPreviousText = $('.btn-import-salary-configuration').html();

        $('.btn-import-salary-configuration').attr('disabled', 'disabled');
        $('.btn-import-salary-configuration').html('<i class="fa fa-spinner fa-spin fa-fw"></i>\r\n' +
            '<span>Processing...</span>');

        var form = $('#import-salary-configuration-form');
        var fd = new FormData();
        var inputs = $("#import-salary-configuration-form :input");
        for (i = 0; i < inputs.length; i++) {
            fd.append(inputs[i].name, inputs[i].value);
        }

        var fileUpload = $("#import-salary-configuration-form #file").get(0);
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
            $('.btn-import-salary-configuration').html('').html(buttonPreviousText).removeAttr('disabled');
            var result = new Object();
            result.message = data.message;
            result.type = data.type;
            //app.showConfirmation(result);
            if (result.type == 'success') {
                $.alert.open("Success", data.message);
                return;
            }

            $.alert.open("Error", data.message);
        });
    });
});

