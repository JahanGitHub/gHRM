
var employeeManager = {
    importValidation: function () {

        var $modal = $('#ImportValidationErrorModalPartial');
        $modal.modal('show');
        $modal.on('hidden.bs.modal', function (e) {
        });

        setTimeout(function () { $('.modal-backdrop.fade.in').last().remove(); }, 3000);        
    }
}


$(function () {
    //submit to import employees
    $('#import-employee-form').on('submit', function (e) {
        e.preventDefault();       

        var buttonPreviousText = $('.btn-import-employee').html();

        $('.btn-import-employee').attr('disabled', 'disabled');
        $('.btn-import-employee').html('<i class="fa fa-spinner fa-spin fa-fw"></i>\r\n' +
            '<span>Processing...</span>');

        var form = $('#import-employee-form');
        var fd = new FormData();
       
        var fileUpload = $("#import-employee-form #file").get(0);
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
            $('.btn-import-employee').html('').html(buttonPreviousText).removeAttr('disabled');
            if (data.type.exceptionErro) {

                $('#eceptionError').html(data.type.exceptionErro);
                return;
            }

            if (data.type === 'success') {
                return window.location = '/EmployeeImport/importconfirmation';
            }
            else {
                if (data.errorLisings) {
                    $('.import-employee-notifier').html(data.importEmployeeErrorList);
                    employeeManager.importValidation();
                    return;
                }

                var result = new Object();
                result.message = data.message;
                result.type = data.type;
                app.showConfirmation(result);

            }
        });
    });
})