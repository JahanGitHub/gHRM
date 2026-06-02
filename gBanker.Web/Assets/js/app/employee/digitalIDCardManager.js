
var digitalIDCardManager = {
    printDigitalIDCard: function () {
        $(".print-section").printThis({
            header: '',
            footer: '',
            importCSS: true,
            loadCSS: "/Assets/css/digitalIDCard.css",
        });
    }
}


$(document).ready(function () {
    $('#btnPrint').on('click', function () {
        digitalIDCardManager.printDigitalIDCard();
    });    
});

