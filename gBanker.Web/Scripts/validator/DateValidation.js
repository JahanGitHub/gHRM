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

function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return (true);
    } else {

        return (false);
    }
  
}

function LoadDropDown(url, data, type, targetField, optionFirstText) {

    var _type = type == null || type == ' undefined' || type.length == 0 ? 'GET' : 'POST';
    var _optionFirstText = optionFirstText == 'undefined' || optionFirstText == '' ? '' : optionFirstText;
    var selected = "";
    var haveSelectedItem = "";
    $.ajax({
        url: url,
        type:_type,
        data: data,
        async:false,
        dataType: 'json',
        success: function (data) {
            if (data.Data.length > 0) {
                $.each(data.Data, function (i, v) {
                    haveSelectedItem = v.Selected == 'undefined' || v.Selected == '' ? '' : v.Selected;
                    if (haveSelectedItem === true) {
                        selected = ' selected="selected"';
                    } else {
                        selected = "";
                    }
                    _optionFirstText = _optionFirstText + '<option value="' + v.Value + '" ' + selected + '>' + v.Text + '</option>';
                });
                $(targetField).html("");
                $(targetField).html(_optionFirstText);
            } else {
                $(targetField).html("");
            }
        }, error: function () {
            alert("Server Error.");
        }
    });
}


function LoadDropDownItem(url, data, type, optionFirstText) {
    var returnData = "";
    var _type = type == null || type == ' undefined' || type.length == 0 ? 'GET' : 'POST';
    var _optionFirstText = optionFirstText == 'undefined' || optionFirstText == '' ? '' : optionFirstText;
    var selected = "";
    var haveSelectedItem = "";
    $.ajax({
        url: url,
        type: _type,
        data: data,
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data.Data.length > 0) {
                $.each(data.Data, function (i, v) {
                    haveSelectedItem = v.Selected == 'undefined' || v.Selected == '' ? '' : v.Selected;
                    if (haveSelectedItem === true) {
                        selected = ' selected="selected"';
                    } else {
                        selected = "";
                    }
                    _optionFirstText = _optionFirstText + '<option value="' + v.Value + '" ' + selected + '>' + v.Text + '</option>';
                });
                returnData = _optionFirstText;
            } else {
                returnData = "";
            }
        }, error: function () {
            alert("Server Error.");
        }
    });
    return returnData;
}

function LoadDropDownWithSelected(url, data, type, targetField, optionFirstText,selected) {

    var _type = type == null || type == ' undefined' || type.length == 0 ? 'GET' : 'POST';
    var _optionFirstText = optionFirstText == 'undefined' || optionFirstText == '' ? '' : optionFirstText;
    var _selected="";
    $.ajax({
        url: url,
        type: _type,
        data: data,
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data.Data.length > 0) {
                $.each(data.Data, function (i, v) {
                    if (selected == v.Value) {
                        _selected = "selected='selected'";
                    } else {
                        _selected = "";
                    }

                    _optionFirstText = _optionFirstText + '<option value="' + v.Value + '"  '+ _selected +' >' + v.Text + '</option>';
                });
                $(targetField).html("");
                $(targetField).html(_optionFirstText);
            } else {
                $(targetField).html("");
            }
        }, error: function () {
            alert("Server Error.");
        }
    });
}

function GetURLParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}

function EnglishToBanlaNumber(engNumber) {
    var retValue = engNumber;
    var BanglaNumberArray = [
        '০','১','২','৩','৪','৫','৬','৭','৮','৯'
    ];
    try{
        var decValue = parseInt(engNumber);
        if (BanglaNumberArray[decValue] !== undefined) {
            retValue = BanglaNumberArray[decValue];
        } else {
            retValue = engNumber;
        }
    }catch(e){
        retValue = engNumber;
    }
    return retValue;
}

function checkDecimal(event) {
    debugger;
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

function ISAplhabetCharacter(event) {
    var key = window.event ? event.keyCode : event.which;//|| event.keyCode == 46
    if (key == 46 || key == 32 || key == 8 || (key >= 65 && key <= 122)) {
        return true;
    } else return false;    
}

function CheckFromAndToDate() {
    var statusFromDate = $("#StatusFromDate").val();
    var statusToDate = $("#StatusToDate").val();
    if (statusFromDate != null || statusFromDate != "" && statusToDate != null || statusToDate != "") {
        var FromDate = new Date(statusFromDate);
        var ToDate = new Date(statusToDate);
        var today = new Date();
        if (FromDate > ToDate) {
            $("#StatusFromDate").empty();
            var Msg = 'You Cannot Enter FromDate Large then ToDate!';
            $.alert.open("Success", Msg);
            return false;
        }
        return true;
    }
}

function ShowYesNoDiv(control, value) {
    if (value === "true") {
        $(control).show();
    } else {
        $(control).hide();
    }
}

function boolToint(value) {
    var d = "";
    if (value) {
        d = value.toLowerCase() == 'true' ? "1" : "0";
    }
    return d;
    
}

function intTobool(value) {
    var d = "";
    if (value) {
        d = value.toLowerCase() == 1 ? "true" : "false";
    }
    return d;

}

function boolTobool(value) {
    var d = "";
    if (value) {
        d = value.toLowerCase() == "true" ? true : false;
    }
    return d;

}
//function CheckInputLengthEqual(fieldId, length) {
//    $(fieldId).on('focusout', function () {
//        var msgLength = "";
//        var lengthArray = length.split(',');

//        var inputValue = $(fieldId).val();
//        var inputLength = inputValue.length;

//        var con1 = false;

//        var con_Count = 0;
//        for (var i = 0; i < lengthArray.length; i++) {
//            con1 = inputLength == lengthArray[i] ? true : false;
//            if (con1 == true) {
//                con_Count++;
//            }
//        }
//        if (lengthArray.length > 1) {
//            msgLength = length.replace(/,/g, " or ");
//        } else {
//            msgLength = length;
//        }

//        if (con_Count > 0) {
//            return;
//        } else {
//            $(fieldId).val("");
//            $.alert.open('alert', 'Input length must be ' + msgLength + ' characters.');
//            //$(fieldId).focus();
//        }
//    });
//}

/* Englsih to bangl Number */
function setMirrorValue(sourceId, targetid) {
    var _sourceId = sourceId.target.id;
    var sourceId = $('#' + _sourceId);
    var targetId = $('#' + targetid);
    var sourceValue = sourceId.val();
    var stringLength = sourceValue.length;

    var i = 0;
    var bNumber = "";
    for (i = 0; i < stringLength; i++) {
        var _convertValue = sourceValue[i];
        var convertValue = "";
        if (parseInt(_convertValue) >= 0) {
            convertValue = EnglishToBanlaNumber(_convertValue);
        } else {
            convertValue = _convertValue;
        }
        bNumber = bNumber + convertValue;
    }

    targetId.val(bNumber);
}
/* Englsih to bangl Number */



//Get PPT Download
//function GetPPT(EntId, NewBusinessProposalId) {
//    window.location.href = '@Url.Action("GetPPTReport", "NUDashboard")?EntId=' + EntId + '&NewBusinessProposalId=' + NewBusinessProposalId;
//}