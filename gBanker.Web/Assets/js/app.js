

if (typeof jQuery === "undefined") { throw new Error("This application requires jQuery"); }

var app = {
    init: function () {
        this.initGlobalControls();
        this.initDatePicker();
    },
    initGlobalControls: function () {

        var url = window.location;

        // for sidebar menu entirely but not cover treeview
        $('ul.sidebar-menu a').filter(function () {
            return this.href == url;
        }).parent().addClass('active');

        // for treeview
        $('ul.treeview-menu a').filter(function () {
            return this.href == url;
        }).parentsUntil(".sidebar-menu > .treeview-menu").addClass('active');
    },
    showConfirmation: function (result) {
        var type = this.returnAlertClasses(result.type);
        $("#dvMessage").attr('class', type);
        $("#dvMessage").html(result.message);
        $("#dvMessage").show();
        $("#dvMessage").toggle('fade', 9000);
    },
    showValidationAlert: function (type, message) {
        $.alert.open(type, message);
    },
    returnAlertClasses: function (alertType) {

        if (alertType == "success") {
            return "success"
        }
        else if (alertType == "warning") {
            return "failed"
        }
        else if (alertType == "failed") {
            return "failed"
        }
        return "";
    },
    addDynamicValidation: function (id) {
        $(id).removeData("validator");
        $(id).removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(id);
    },
    validateForm: function (form) {
        if (!form)
            return false;

        return $(form).valid();
    },
    enableEditableTimepicker: function (selector) {
        $(selector).attr("onkeypress", "return true;");
    },
    timePickerCommonOptions: function () {
        var options = {// now: "12:35", //hh:mm 24 hour format only, defaults to current time
            twentyFour: true, //Display 24 hour format, defaults to false
            upArrow: 'wickedpicker__controls__control-up', //The up arrow class selector to use, for custom CSS
            downArrow: 'wickedpicker__controls__control-down', //The down arrow class selector to use, for custom CSS
            close: 'wickedpicker__close', //The close class selector to use, for custom CSS
            hoverState: 'hover-state', //The hover state class to use, for custom CSS
            title: 'Timepicker', //The Wickedpicker's title,
            showSeconds: null, //Whether or not to show seconds,
            secondsInterval: 1, //Change interval for seconds, defaults to 1  ,
            minutesInterval: 1, //Change interval for minutes, defaults to 1
            beforeShow: null, //A function to be called before the Wickedpicker is shown
            show: null, //A function to be called when the Wickedpicker is shown
            clearable: false, //Make the picker's input clearable (has clickable "x")
        };

        return options;
    },
    requiredFieldValidate: function (reqFieldsName) {
        var flag = 1;
        if (!reqFieldsName)
            return flag;

        for (var i = 0; i < reqFieldsName.length; i++) {
            var fieldName = reqFieldsName[i];

            if ($(fieldName).val() == '' || $(fieldName).val() == '0') {
                $(fieldName).addClass("errorClass");
                flag = 0;
            }
        }

        return flag;
    },
    initDatePicker: function () {

        if ($('.dt-datepicker').length <= 0) return;

        $(".dt-datepicker").datepicker({
            dateFormat: "dd-M-yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1950:2050"
        });

        var dateToday = new Date();
        $(".ddl-datepicker").datepicker({
            dateFormat: "dd-M-yy",
            showAnim: "scale",
            changeMonth: true,
            changeYear: true,
            yearRange: "1950:2050"
            //minDate: dateToday

        });
        $(".ddl-datepicker").datepicker('setDate', new Date());
    },
    checkStartDateIsSmallerThanOrEqualToEndDate: function (startDate, endDate) {
        return (app.MakeDate(startDate) <= app.MakeDate(endDate)) ? true : false;
    },
    checkStartDateIsSmallerThanEndDate: function (startDate, endDate) {
        return (app.MakeDate(startDate) < app.MakeDate(endDate)) ? true : false;
    },
    MakeDate: function (stringDate) {
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
    },
    validateEmail: function (mail) {
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
            return (true)
        }
        $.alert.open("You have entered an invalid email address!")
        return (false)
    },
    checkNumeric: function (event) {
        var key = window.event ? event.keyCode : event.which;
        return (((key - 48) * (key - 57) <= 0) || ((key - 96) * (key - 106) <= 0) || key == 110 || key == 190);
    },
    getBaseUrl: function () {
        var fullPath = window.location;
        var baseUrl = `${fullPath.protocol}//${fullPath.host}`;
        return baseUrl;
    },
    logoutForSSOLogin: function () {
        window.location.href = `${app.getBaseUrl()}/Account/logoff`; return;
    },
    ajaxCall: function (_url, param, method_type) {
            $.ajax({
                type: method_type,
                dataType: "json",
                async: true,
                cache: false,
                url: _url,
                data: param,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    return data;
                },
                error: function (xhr, status, error) {
                    $.alert.open("error", error);
                }
            });
    }
};

$(function () {
    app.init();
});

var CookieCacheConstants = {
    GHRM_PLUS_EMPLOYEE_PROMOTION: "GHRM_PLUS_EMPLOYEE_PROMOTION::COOKIES"
}


var enumUserRole = {
    SuperAdmin: 'Super Admin'
}

var enumPFReportType = {
    IndividualLoanLedger: "1",
    LoanAndInterestCollectionfortheMonth: "2",
    LoanVoucherDetails: "3",
    LoanWiseCollectionList: "4",
    OfficeWiseLoanSummary: "5",
    LoanCollectionDetails: "6",
    LoanStatistics: "7",
    LoanDisbursementSummary: "8",
}

var payrollConfigurationTypeEnum = {
    GrossSalary: 'GR',
    BasicSalary: "BC"
}

var loanStatusConstants = {
    Running: 'Running',
    Closed: "Closed"
}

var gradeRatioOnConstants = {
    Fixed: 'Fixed',
    Percentage: "Percentage"
}

var EmployeeReportConstants = {
    Employee_Wise_Product: '1',
    Blood_Group_Wise_All_Employee: '2',
    Chart_Of_Blood_Summary: '3',
    Office_Name_Wise_Employee_Count_Summary: '4',
    Office_Type_Wise_Employee_Count_Summary: '5',
    Gender_Wise_Employee: '6',
    All_Department_Wise_Employee: '7',
    Department_Wise_Total_Employee_: '8',
    Department_Wise_Total_employee_Graphical_View: '9',
    Payroll_Designation_Wise_Employee: '10',
    Employment_Type_Wise_Count: '11',
    Payroll_Designation_Wise_Insurance: '12',
    Employee_experience: '13',
    Employee_Demographic_Info: '14',
    Employee_Signature_Report: '15',
    Employee_Service_Book: '16',
    Digital_ID_Card: '17',
    Digital_ID_Card_Mousumi: '28',
}


var ApiRoutesConstants = {
    AUTH_PATH: 'http://103.26.136.30:8080/auth',
}

var CookieConstants = {
    CURRENT_LOGGED_IN_ACCESSTOKEN: 'COOKIE_CURRENT_LOGGED_IN_ACCESSTOKEN',
}