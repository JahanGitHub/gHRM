
var dashboardManager = {
    redirectPreviousPage: function () {
        var redirectUrl = $('#RedirectUrl').attr("RedirectUrl");
        if (!redirectUrl || redirectUrl==='/')
            return;
                
        $('#RedirectUrl').attr('RedirectUrl', '');
        setTimeout(function () {            
            window.location = redirectUrl;
            return;
        }, 3000);       
    }
}

$(function () {
    //redirect previous page
    dashboardManager.redirectPreviousPage();
})

function loadDashboardItems() {
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: '/Home/GetDashboardItems',
        dataType: 'json',
        async: true,
        success: function (result) {
        },
        error: function (request, status, error) {
            alert("Error occured.");
        }
    });
}
