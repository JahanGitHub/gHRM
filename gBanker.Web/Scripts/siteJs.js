function MakeDate(stringDate) {
    var monName = stringDate.substring(3, 6);
    var dd = stringDate.substring(0, 2);
    var yy = stringDate.substring(11, 7);
    var makeDt = '';
    var monSl;
    switch (monName) {
        case 'Jan':
            monSl = '01';
            break;
        case 'Feb':
            monSl = '02';
            break;
        case 'Mar':
            monSl = '03';
            break;
        case 'Apr':
            monSl = '04';
            break;
        case 'May':
            monSl = '05';
            break;
        case 'Jun':
            monSl = '06';
            break;
        case 'Jul':
            monSl = '07';
            break;
        case 'Aug':
            monSl = '08';
            break;
        case 'Sep':
            monSl = '09';
            break;
        case 'Oct':
            monSl = '10';
            break;
        case 'Nov':
            monSl = '11';
            break;
        case 'Dec':
            monSl = '12';
            break;
        default:
            monSl = '0';
    }
    if (monSl != '0') {
        makeDt = yy + ' ' + monSl + ' ' + dd;
    }
    return makeDt;
}


//
// Checks If the input is Numeric
//
function checkNumeric(event) {
    var key = window.event ? event.keyCode : event.which;
    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 46
     || event.keyCode == 37 || event.keyCode == 39) {
        return true;
    }
    if (event.which === 13) {
        $(this).next().focus();
    }
    else if (key < 48 || key > 57) {
        return false;
    }
    else return true;
}

function checkDecimal(event) {
    var key = window.event ? event.keyCode : event.which;//|| event.keyCode == 46
    if (event.keyCode == 8 || event.keyCode == 9
     || event.keyCode == 37 || event.keyCode == 39) {
        return true;
    }
    if (event.which === 13) {
        $(this).next().focus();
    }
    else if (key == 46) {
        var element = event.target.id;
        var findDecimal = $('#' + element).val();
        var isExist = ".";
        if (findDecimal.indexOf(isExist) != -1) {
            return false;
        } else {
            return true;
        }

    }
    else if (key != 46 && (key < 48 || key > 57)) {
        return false;
    }
    else return true;
}
//
//textbox input range validate
//
function checkInputRange(id, minValue, maxValue, event) {
    var inputValue = $("#" + id + "").val();
    if (inputValue >= minValue && inputValue <= maxValue) {
        return true;
    } else {
        $("#" + id + "").val(0);
        $.alert.open("Error", "Please insert valid data");
        return false;
    }
}


//
// Checks If the input is Decimal
//		
function validDecimalNumber(el, evt) {

    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var number = el.value.split('.');
    if (charCode == 8) {
        return true;
    }
    if (charCode == 9 || charCode === 13) {
        $(this).next().focus();
    }
    else if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}



//
// Checks If the test date is equal or greater than the standard Date 
//
function ValidDateEqualOrGreater(testDate, standardDate) {
    var _testDate = new Date(testDate);
    var _standardDate;
    if (standardDate) {
        _standardDate = new Date(standardDate);
    } else {
        var nd = new Date();
        var da = new Date(nd.getFullYear(), nd.getMonth(), nd.getDate());
        _standardDate = da;
    }
    if (_testDate >= _standardDate) {
        return true;
    } else {
        return false;
    }

}


//
// Checks If the test date is equal to the standard Date 
//
function ValidDateEqual(testDate, standardDate) {
    var _testDate = new Date(testDate);
    var _standardDate;
    if (standardDate) {
        _standardDate = new Date(standardDate);
    } else {
        var nd = new Date();
        var da = new Date(nd.getFullYear(), nd.getMonth(), nd.getDate());
        _standardDate = da;
    }
    if (_testDate == _standardDate) {
        return true;
    } else {
        return false;
    }

}



//
// Checks If the test date is equal or less than the standard Date 
//
function ValidDateEqualOrLess(testDate, standardDate) {
    var _testDate = new Date(testDate);
    var _standardDate;
    if (standardDate) {
        _standardDate = new Date(standardDate);
    } else {
        var nd = new Date();
        var da = new Date(nd.getFullYear(), nd.getMonth(), nd.getDate());
        _standardDate = da;
    }
    if (_testDate <= _standardDate) {
        return true;
    } else {
        return false;
    }
}


//
// Checks If the test date is greater than the standard Date 
//
function ValidDateGether(testDate, standardDate) {
    var _testDate = new Date(testDate);
    var _standardDate;
    if (standardDate) {
        _standardDate = new Date(standardDate);
    } else {
        var nd = new Date();
        var da = new Date(nd.getFullYear(), nd.getMonth(), nd.getDate());
        _standardDate = da;
    }
    if (_testDate > _standardDate) {
        return true;
    } else {
        return false;
    }

}


//
// Checks If the test date is less than the standard Date 
//
function ValidDateLess(testDate, standardDate) {
    var _testDate = new Date(testDate);
    var _standardDate;
    if (standardDate) {
        _standardDate = new Date(standardDate);
    } else {
        var nd = new Date();
        var da = new Date(nd.getFullYear(), nd.getMonth(), nd.getDate());
        _standardDate = da;
    }
    if (_testDate < _standardDate) {
        return true;
    } else {
        return false;
    }
}


//
// Checks If the Input email is valid 
//
function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return (true);
    } else {
        return (false);
    }

}

// Converts jquerydate to dd-MM-yyyy formate '1-Jan-2017'
function DateConversionToLongDate(dt) {
    var monthNames = [
      "Jan", "Feb", "Mar",
      "Apr", "May", "Jun", "Jul",
      "Aug", "Sep", "Oct",
      "Nov", "Dec"
    ];

    var date = dt;
    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();

    var result = day + '-' + monthNames[monthIndex] + '-' + year;
    return result;

}

// Returns the Month difference between two date

function monthDiff(d1, d2) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth();// + 1;
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
}


// Checks input length of a text input (fieldId= id of desired field; length= acceptable lengths(if more than 1 length then seperate length by ,like "6,10" else "6"))
function CheckInputLengthEqual(fieldId, length) {
    $(fieldId).on('focusout', function () {
        var msgLength = "";
        var lengthArray = length.split(',');

        var inputValue = $(fieldId).val();
        var inputLength = inputValue.length;

        var con1 = false;

        var con_Count = 0;
        for (var i = 0; i < lengthArray.length; i++) {
            con1 = inputLength == lengthArray[i] ? true : false;
            if (con1 == true) {
                con_Count++;
            }
        }
        if (lengthArray.length > 1) {
            msgLength = length.replace(/,/g, " or ");
        } else {
            msgLength = length;
        }

        if (con_Count > 0) {
            return;
        } else {
            $(fieldId).val("");
            $.alert.open('alert', 'Input length must be ' + msgLength + ' characters.');
            //$(fieldId).focus();
        }
    });
}

function CheckInputLengthLessOrEqual(fieldId, length) {
    $(fieldId).on('focusout', function () {
        var lengthArray = length.split(',');

        var inputValue = $(fieldId).val();
        var inputLength = inputValue.length;
        var con1 = false;

        var con_Count = 0;
        for (var i = 0; i < lengthArray.length; i++) {
            con1 = inputLength <= lengthArray[i] ? true : false;
            if (con1 == true) {
                con_Count++;
            }
        }

        //var inputValue = $(fieldId).val();
        //var inputLength = inputValue.length;///inputLength != length
        if (con_Count > 0) {
            return;
        } else {
            $(fieldId).val("");
            $.alert.open('alert', 'Input length must be ' + length + ' characters.');
            //$(fieldId).focus();
        }
    });
}