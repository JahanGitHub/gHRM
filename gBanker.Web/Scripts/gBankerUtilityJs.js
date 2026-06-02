

function gHRMDatePicker(controlid) {
    if (controlid == null) {
        return;
    }
    $("#" + controlid).datepicker(
           {
               dateFormat: "dd-M-yy",
               showAnim: "scale",
           });
}


function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,10})+$/.test(mail)) {
        return (true)
    }
    $.alert.open("You have entered an invalid email address!")
    return (false)
}
