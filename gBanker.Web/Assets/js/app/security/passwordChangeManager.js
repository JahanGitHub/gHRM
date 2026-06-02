

$(document).ready(function () {  
    //submit to change userrole
    $('#change-passord-form').on('submit', function (event) {
        event.preventDefault();
        var form = $(this);

        //for form validation
        var isValid = app.validateForm('#change-passord-form');
        if (!isValid) return;
        
        $("#AjaxLoader").show();
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') { 
                //success alert
                $("#AjaxLoader").hide();
                $.alert.open("Success", response.message);
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});
