
$(function () {
    //submit to import promotion backlogs
    $('#import-transfer-backlog-form').on('submit', function (e) {
        e.preventDefault();       

        var buttonPreviousText = $('.btn-import-transfer-backlog').html();

        $('.btn-import-transfer-backlog').attr('disabled', 'disabled');
        $('.btn-import-transfer-backlog').html('<i class="fa fa-spinner fa-spin fa-fw"></i>\r\n' +
            '<span>Processing...</span>');

        var form = $('#import-transfer-backlog-form');
        var fd = new FormData();
       
        var fileUpload = $("#import-transfer-backlog-form #file").get(0);
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
            $('.btn-import-transfer-backlog').html('').html(buttonPreviousText).removeAttr('disabled');            
            if (data.type === 'success') {
                return window.location = '/EmployeeTransferImport/importconfirmation';
            }
            else { 
                var result = new Object();
                result.message = data.message;
                result.type = data.type;
                app.showConfirmation(result);
            }
        });
    });
})