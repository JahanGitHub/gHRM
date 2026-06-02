

var aspNetUserManager = {

}

$(function () {

    $(".user-edit-submit-form").on("submit", function (event) {
        var _currentForm = $(this).closest('form');
        if (_currentForm.valid()) {

            event.preventDefault();
            $("#loading").show();
            var url = $(this).attr("action");
            var formData = $(this).serialize();
            $.ajax({
                url: url,
                type: "POST",
                data: formData,
                dataType: "json",
                success: function (resp) {
                    $("#loading").hide();
                    $.alert.open(resp.Message);

                    $(window).scrollTop(0);
                },
                error: function (err) {
                    $("#loading").hide();
                    var msg = err.responseText;
                    $.alert.open(msg);
                    $(window).scrollTop(0);
                }

            })
        }
    });

})