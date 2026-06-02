function PFComponent() {
    var item = {};
    item.ComponentCategory = $("#ComponentCategory").val();

    item.ComponentName = $("#ComponentName option:selected").text();
    item.ComponentPayrollId = $("#ComponentName").val();

    item.TransactionType = $("#TransactionType").val();

    item.PRComponentGroupID = $("#ComponentGroupName").val();

    item.ComponentType = $("#ComponentType").val();

    item.RatioBasedOn = $("#RatioBasedOn").val();

    item.ComponentAmount = $("#ComponentAmount").val();

    item.EmployeeTypeId = $("#EmployeeTypeId").val();

    item.EmployeeStatusIdList = $("#EmployeeStatusId").val();

    item.EmpDesignationIdList = $("#DesignationId").val();

    item.IsProductDependent = $("#IsProductDependent").val();

    item.IsProvidentFundComponent = $("#IsProvidentFundComponent").val();

//    item.EffectiveStartDateMsg = $("#EffectiveStartDateMsg").val();

//    item.ValidateDurtion = $("#ValidateDurtion").val();

  //  item.EffectiveEndDateMsg = $("#EffectiveEndDateMsg").val();

    item.OffLocationList = $("#typeFilterColumnOfficeLocationId").val();

    item.SalaryRoundType = $("#SalaryRoundType").val();

    item.MinimumLimit = $("#MinimumLimit").val();

    item.MaximumLimit = $("#MaximumLimit").val();

    item.MinDuration = $("#MinDuration").val();

    item.MaxDuration = $("#MaxDuration").val();

    item.IsAdjustable = $("#IsAdjustable").val();;

    item.InterestRate = $("#InterestRate").val();


    var loanid = $("#LoanCalculationId option:selected").val();
    if (loanid == "0") {
        item.LoanCalculationId = 0;
    }
    else {
        item.LoanCalculationId = $("#LoanCalculationId").val();;
    }


    item.SalaryAccCode = $("#SalaryAccCode").val();
    item.AccountName = $("#AccountName").val();

    item.SalaryChangesByComponent = $("#SalaryChangesByComponent").val();

    item.IsSalaryImpactProhibited = $("#IsSalaryImpactProhibited").val();

    if (item.SalaryChangesByComponent == "N/A") {
        item.SalaryEffect = false;
    }
    else {
        item.SalaryEffect = true;
    }

    item.PFTypeId = $("#PFTypeId").val();

    //item.DesignationId = $("#DesignationId").val();

    return item;

}

function ValidateInput(model) {
    if (model.ComponentCategory == "") {
        $.alert.open("Error", "Please Select Component Category");
        return false;
    }

    if (model.ComponentName == "Please Select") {
        $.alert.open("Error", "Please Select Component Name");
        return false;
    }
    if (model.TransactionType == "0") {
        $.alert.open("Error", "Please Provide Transaction Type");
        return false;
    }

    if (model.PRComponentGroupID == "") {
        $.alert.open("Error", "Please Provide Component Group");
        return false;
    }

    if (model.ComponentType == "") {
        $.alert.open("Error", "Please Select Component Type");
        return false;
    }
    if (model.RatioBasedOn == "") {
        $.alert.open("Error", "Please Provide Ration Based On");
        return false;
    }

    if (model.ComponentAmount == "") {
        $.alert.open("Error", "Please Provide Component Amount");
        return false;
    }
    if (model.EmployeeTypeId == "") {
        $.alert.open("Error", "Please Provide Employee Type");
        return false;
    }
    //if (model.DesignationId == "") {
    //    $.alert.open("Error", "Please select payroll designation!");
    //    return false;
    //}

    if (model.EmpDesignationIdList == null || model.EmpDesignationIdList.length == 0) {
        $.alert.open("Error", "Please select payroll designation!");
        return false;
    }


    if (model.EmployeeStatusIdList == null || model.EmployeeStatusIdList.length == 0) {
        $.alert.open("Error", "Please Provide Employee Status");
        return false;
    }

    if (model.Productdependent == "") {
        $.alert.open("Error", "Please Provide Product dependency");
        return false;
    }

    if (model.IsProvidentFundComponent == "") {
        $.alert.open("Error", "Please Provide Provident Fund Integration Required or Not");
        return false;
    }

    if (model.OffLocationList == null || model.OffLocationList.length == 0) {
        $.alert.open("Error", "Please Provide Office Location");
        return false;
    }

    if (model.SalaryRoundType == "") {
        $.alert.open("Error", "Please Provide Salary Round Type");
        return false;
    }

    if (model.MaximumLimit == "") {
        $.alert.open("Error", "Please Provide Maximum Limit");
        return false;
    }
    if (model.MinimumLimit == "") {
        $.alert.open("Error", "Please Provide Minimum Limit");
        return false;
    }

    if (model.MinDuration == "") {
        $.alert.open("Error", "Please Provide Minimum Duration");
        return false;
    }
    if (model.MaxDuration = "") {
        $.alert.open("Error", "Please Provide Maximum Duration");
        return false;
    }

    if (model.IsAdjustable == "") {
        $.alert.open("Error", "Please Provide Loan Configuration Changable");
        return false;
    }

    if (model.InterestRate == "") {
        $.alert.open("Error", "Please Provide Interest Rate");
        return false;
    }

    if (model.LoanCalculationId != "0" && model.LoanCalculationId == "") {
        $.alert.open("Error", "Please Provide Loan Calculation");
        return false;
    }


    if (model.SalaryAccCode == "") {
        $.alert.open("Error", "Please Provide Salary Integration Code");
        return false;
    }


    //if (model.EffectiveStartDateMsg == "") {
    //    $.alert.open("Error", "Please Provide Effective Start Date");
    //    return false;
    //}
    //if (model.EffectiveEndDateMsg == "") {
    //    $.alert.open("Error", "Please Provide Effective End Date");
    //    return false;
    //}


    if (model.ValidateDurtion == "") {
        $.alert.open("Error", "Please Provide Validate Durtion");
        return false;
    }

    if (model.SalaryChangesByComponent == "") {
        $.alert.open("Error", "Please Provide Changes in Regular Configured Salary");
        return false;
    }


    if (model.IsSalaryImpactProhibited == "") {
        $.alert.open("Error", "Deny Impact on Regular Salary Component");
        return false;
    }

    if (model.IsProvidentFundComponent == true && model.PFTypeId == '0') {
        $.alert.open("Error", "Provident Fund Type Required");
        return false;
    }
    return true;
}

function SavePRComponent() {
    var model = PFComponent();
    // && model.EffectiveStartDateMsg !== "" && model.EffectiveEndDateMsg !== ""
    if (ValidateInput(model)) {
        if (model.ComponentName !== "" && model.ComponentType !== "" && model.ComponentAmount !== "" && model.TransactionType !== "" && model.ComponentCategory !== "" && model.EmployeeTypeId !== "" && model.EmployeeStatusIdList.length > 0
            && model.RatioBasedOn !== "" && model.SalaryAccCode !== ""  && model.MaximumLimit !== "" && model.MinimumLimit !== "" && model.Productdependent !== "" && model.ValidateDurtion !== ""
            && model.OffLocationList.length > 0) {
            $('#AjaxLoader').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                async: true,
                cache: false,
                url: '/PRComponent/Create_designation',
                data: JSON.stringify({ model: model }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#AjaxLoader').hide();
                    // $.alert.open('Success', data);
                    $.alert.open("Message", data);
                },
                error: function (xhr, status, error) {
                    $('#AjaxLoader').hide();
                    alert(error);
                }
            });
        } else {
            $.alert.open("Error", "Please input required field");
        }
    }
}

function calculateEffectiveEndDate(duration) {

    var validation = parseFloat(duration) * 12;
    var validationYears = parseFloat(duration) * 12;
    var startDate = $("#EffectiveStartDateMsg").val();

    var effStartDate = new Date(MakeDate(startDate));
    var expireDate = effStartDate.setMonth(effStartDate.getMonth() + validationYears);
    var dd = new Date(expireDate - 1);
    var date = DateConversionToLongDate(dd);
    $("#EffectiveEndDateMsg").val(date);
}

function LoadComponentCategory(categoryName) {
    var ddlComponentName = $("#ComponentName");
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRComponent/GetComponentNamebyCategory',
        data: { categoryName: categoryName },
        dataType: 'json',
        async: true,
        success: function (data) {
            if (data.length > 0) {
                ddlComponentName.html("");
                $.each(data, function (sl, v) {
                    ddlComponentName.append($('<option></option>').val(v.Value).html(v.Text));
                });
            } else {
                ddlComponentName.html('<option value="">Please Select</option>');
            }
        },
    });
}

function StyleForComponentCategory(categoryName) {
    if (categoryName == "Allowance") {
        $("#TransactionType").attr("disabled", true);
        $("#TransactionType").val('Cr');

        $("#ComponentGroupName").attr("disabled", true);
        $("#ComponentGroupName").val('1');

        $("#ComponentType").attr("disabled", true);
        $("#ComponentType").val('F');

      //  $("#RatioBasedOn").attr("disabled", true);
      //  $("#RatioBasedOn").val('NR');

        $("#IsProductDependent").attr("disabled", false);
        $("#IsProductDependent").val("");

        $("#IsProvidentFundComponent").attr("disabled", true);
        $("#IsProvidentFundComponent").val("false");

        $("#SalaryChangesByComponent").attr("disabled", false);

        $("#IsSalaryImpactProhibited").attr("disabled", true);
        $("#IsSalaryImpactProhibited").val("true");

        $("#MinimumLimit").val("0");

        $("#MaximumLimit").val("0");

        $("#InterestRate").attr("disabled", true);
        $("#InterestRate").val("0");

        $("#MinDuration").attr("disabled", true);
        $("#MinDuration").val("0");

        $("#MaxDuration").attr("disabled", true);
        $("#MaxDuration").val("0");

        $("#IsAdjustable").attr("disabled", true);
        $("#IsAdjustable").val("false");

        $("#LoanCalculationId").attr("disabled", true);
        $("#LoanCalculationId").val("0");

        //$("#ComponentAmount").attr("disabled", true);
        //$("#ComponentAmount").val("0");

        $("#PFTypeId").attr("disabled", true);
        $('#PFTypeId').val('0');

        return false;
    }
    if (categoryName == "Deduction") {
        $("#TransactionType").attr("disabled", true);
        $("#TransactionType").val('Dr');

        $("#ComponentGroupName").attr("disabled", true);
        $("#ComponentGroupName").val('6');

        $("#ComponentType").attr("disabled", true);
        $("#ComponentType").val('F');

        $("#RatioBasedOn").attr("disabled", true);
        $("#RatioBasedOn").val('NR');

        $("#IsProductDependent").attr("disabled", false);
        $("#IsProductDependent").val("");

        $("#IsProvidentFundComponent").attr("disabled", true);
        $("#IsProvidentFundComponent").val("false");

        $("#SalaryChangesByComponent").attr("disabled", false);

        $("#IsSalaryImpactProhibited").attr("disabled", true);
        $("#IsSalaryImpactProhibited").val("true");

        $("#MinimumLimit").val("0");

        $("#MaximumLimit").val("0");

        $("#InterestRate").attr("disabled", true);
        $("#InterestRate").val("0");

        $("#MinDuration").attr("disabled", true);
        $("#MinDuration").val("0");

        $("#MaxDuration").attr("disabled", true);
        $("#MaxDuration").val("0");

        $("#IsAdjustable").attr("disabled", true);
        $("#IsAdjustable").val("false");

        $("#LoanCalculationId").attr("disabled", true);
        $("#LoanCalculationId").val("0");

        $("#ComponentAmount").attr("disabled", true);
        $("#ComponentAmount").val("0");

        $("#PFTypeId").attr("disabled", true);
        $('#PFTypeId').val('0');

        return false;
    }
    if (categoryName == "Bonus") {
        $("#TransactionType").attr("disabled", true);
        $("#TransactionType").val('Cr');

        $("#ComponentGroupName").attr("disabled", true);
        $("#ComponentGroupName").val('1');

        $("#ComponentType").attr("disabled", false);
        $("#ComponentType").val("");

        $("#RatioBasedOn").attr("disabled", false);
        $("#RatioBasedOn").val("");

        $("#IsProvidentFundComponent").attr("disabled", true);
        $("#IsProvidentFundComponent").val("false");

        $("#SalaryChangesByComponent").attr("disabled", true);
        $("#SalaryChangesByComponent").val('N/A');

        $("#IsSalaryImpactProhibited").attr("disabled", true);
        $("#IsSalaryImpactProhibited").val("true");

        $("#MinimumLimit").val("0");

        $("#MaximumLimit").val("0");

        $("#InterestRate").attr("disabled", true);
        $("#InterestRate").val("0");

        $("#MinDuration").attr("disabled", true);
        $("#MinDuration").val("0");

        $("#MaxDuration").attr("disabled", true);
        $("#MaxDuration").val("0");

        $("#IsAdjustable").attr("disabled", true);
        $("#IsAdjustable").val("false");

        $("#LoanCalculationId").attr("disabled", true);
        $("#LoanCalculationId").val("0");

        $("#ComponentAmount").attr("disabled", false);
        $("#ComponentAmount").val("0");

        $("#IsProductDependent").attr("disabled", true);
        $("#IsProductDependent").val("false");

        $("#PFTypeId").attr("disabled", true);
        $('#PFTypeId').val('0');
        return false;
    }
    if (categoryName == "Loan") {
        $("#TransactionType").attr("disabled", true);
        $("#TransactionType").val('Dr');

        $("#ComponentGroupName").attr("disabled", true);
        $("#ComponentGroupName").val('6');

        $("#ComponentType").attr("disabled", true);
        $("#ComponentType").val('F');

        $("#RatioBasedOn").attr("disabled", true);
        $("#RatioBasedOn").val('NR');

        $("#IsProductDependent").attr("disabled", false);

        $("#IsProvidentFundComponent").attr("disabled", true);
        $("#IsProvidentFundComponent").val("false");

        $("#SalaryChangesByComponent").attr("disabled", true);
        $("#SalaryChangesByComponent").val('N/A');

        $("#IsSalaryImpactProhibited").attr("disabled", true);
        $("#IsSalaryImpactProhibited").val("true");
        $("#InterestRate").attr("disabled", false);

        $("#MinDuration").attr("disabled", false);
        $("#MaxDuration").attr("disabled", false);

        $("#IsAdjustable").attr("disabled", false);
        $("#IsAdjustable").val("false");

        $("#LoanCalculationId").attr("disabled", false);
        $("#LoanCalculationId").val("0");

        $("#ComponentAmount").attr("disabled", true);
        $("#ComponentAmount").val("0");

        $("#MinimumLimit").val("0");
        $("#MaximumLimit").val("0");
        $("#MinDuration").val("0");
        $("#MaxDuration").val("0");

        $("#PFTypeId").attr("disabled", true);
        $('#PFTypeId').val('0');
        return false;
    }
    else {
        $("#TransactionType").attr("disabled", false);
        $("#TransactionType").val('0');

        $("#ComponentGroupName").attr("disabled", true);
        $("#ComponentGroupName").val("");

        $("#ComponentType").attr("disabled", false);
        $("#ComponentType").val("");

        $("#RatioBasedOn").attr("disabled", false);
        $("#RatioBasedOn").val("");

        $("#IsProductDependent").attr("disabled", true);
        $("#IsProductDependent").val("false");

        $("#IsProvidentFundComponent").attr("disabled", false);
        $("#IsProvidentFundComponent").val("");

        $("#SalaryChangesByComponent").attr("disabled", true);
        $("#SalaryChangesByComponent").val('N/A');

        $("#IsSalaryImpactProhibited").attr("disabled", false);
        $("#IsSalaryImpactProhibited").val("false");

        $("#MinimumLimit").val("0");

        $("#MaximumLimit").val("0");

        $("#InterestRate").attr("disabled", true);
        $("#InterestRate").val("0");

        $("#MinDuration").attr("disabled", true);
        $("#MinDuration").val("0");

        $("#MaxDuration").attr("disabled", true);
        $("#MaxDuration").val("0");

        $("#IsAdjustable").attr("disabled", true);
        $("#IsAdjustable").val("false");

        $("#LoanCalculationId").attr("disabled", true);
        $("#LoanCalculationId").val("0");

        $("#ComponentAmount").attr("disabled", false);
        $("#ComponentAmount").val("0");

        $("#PFTypeId").attr("disabled", true);
        $('#PFTypeId').val('0');
    }
}

function StyleForTransactionType(transactionType) {
    if (transactionType == "Cr") {
        $("#ComponentGroupName").attr("disabled", true);
        $("#ComponentGroupName").val('1');
        return false;
    }

    if (transactionType == "Dr") {
        $("#ComponentGroupName").attr("disabled", true);
        $("#ComponentGroupName").val('6');
        return false;
    }
    else {
        $("#ComponentGroupName").val('6');
        $("#ComponentGroupName").val("");
    }
}

function StyleForComponentType(componentType) {

    $("select option[value*='NR']").prop('disabled', false);
    $("select option[value*='G']").prop('disabled', false);
    $("select option[value*='B']").prop('disabled', false);

    if (componentType === "F") {
        $("#RatioBasedOn").val('NR');
        $("select option[value*='NR']").prop('disabled', false);
        $("select option[value*='G']").prop('disabled', true);
        $("select option[value*='B']").prop('disabled', true);
        return;
    }

   //  $("select option[value*='NR']").prop('disabled', true);
    return;
}

function ValidateDurationEffectiveDate() {
    var duration = $("#ValidateDurtion").val();
    var startDate = $("#EffectiveStartDateMsg").val();
    if (duration != "" && startDate != "") {
        if (duration != "Other") {
            $("#EffectiveEndDateMsg").attr("readonly", true);
            calculateEffectiveEndDate(duration);
        } else {
            $("#EffectiveEndDateMsg").removeAttr("readonly");
        }
    } else {
        if (startDate == "") {
            $.alert.open("Error", "Please enter effective start date first");
        }
        $("#EffectiveEndDateMsg").attr("readonly", true);
        $("#EffectiveEndDateMsg").val("");
        return false;
    }
}

function StyleForProvidentFund(pfRequired) {
    if (pfRequired == null || pfRequired == '') {
        $("#PFTypeId").attr("disabled", true);
        $('#PFTypeId').val('0');
    }
    if (pfRequired == 'false') {
        $("#PFTypeId").attr("disabled", true);
        $('#PFTypeId').val('0');

    }
    if (pfRequired == 'true') {
        $("#PFTypeId").attr("disabled", false);
        $('#PFTypeId').val('1');

    }
}

function LoadSalaryAccountDetail(salaryAccountCode) {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRComponent/GetAccountData',
        data: { AccCode: salaryAccountCode },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.each(data, function (index, data) {
                if (data != "Error") {
                    $("#SalaryAccCode").val(data.SalaryAccCode);
                    $("#AccountName").val(data.AccountName);
                }
                else {
                    $("#SalaryAccCode").val('');
                    $.alert.open("Wrong Account Code");
                    $("#SalaryAccCode").focus();
                    $("#AccountName").val('');
                }
            });
        },
        error: function (request, status, error) {
            $("#SalaryAccCode").val('');
            $.alert.open("Wrong Account Code");
            $("#SalaryAccCode").focus();
            $("#AccountName").val('');
        }
    });
}

$(document).ready(function () {
    $("#PFTypeId").attr("disabled", true);
    $('#PFTypeId').val('0');

    $("#ComponentCategory").change(function () {
        var categoryName = $("#ComponentCategory").val();
        if (categoryName == "Loan")
            $("#LoanCalculationId").val(0).attr('readonly', true);
        LoadComponentCategory(categoryName);
        StyleForComponentCategory(categoryName);
    });

    $("#TransactionType").change(function () {
        var transactionType = $("#TransactionType").val();
        StyleForTransactionType(transactionType);
    });

    $("#ComponentType").change(function () {
        var componentType = $("#ComponentType").val();
        StyleForComponentType(componentType);
    });

    $("#EffectiveStartDateMsg").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true
    });

    $("#ValidateDurtion").change(function () {
        ValidateDurationEffectiveDate();
    });

    $("#EffectiveEndDateMsg").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        onClose: function () {
            if ($("#EffectiveEndDateMsg").val() != '') {
                var issueDate = $("#EffectiveStartDateMsg").val();
                if (issueDate == "") {
                    $("#EffectiveEndDateMsg").val("");
                    $.alert.open("Error", "Please Insert Issue Date First");
                    return;
                } else {
                    var standardDate = new Date(MakeDate(issueDate));
                    var testDate = new Date(MakeDate($("#EffectiveEndDateMsg").val()));
                    if (ValidDateGether(testDate, standardDate) == false) {
                        $("#EffectiveEndDateMsg").val("");
                        $.alert.open("Error", "End date must be greater than the start date");
                    }
                }
            }
        }
    });

    $("#EffectiveEndDateMsg").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true
    });

    $("#SalaryAccCode").blur(function (e) {
        var salaryAccountCode = $("#SalaryAccCode").val();
        if ($("#SalaryAccCode").val() != '') {
            LoadSalaryAccountDetail(salaryAccountCode);
        }
        else {
            $("#SalaryAccCode").empty();
            $("#SalaryAccCode").val('');
            $("#AccountName").val('');
        }
    });

    $("#IsProvidentFundComponent").change(function () {
        var pfRequired = $("#IsProvidentFundComponent").val();
        StyleForProvidentFund(pfRequired);
    });

    $("#btnShowDetailChangesinRegular").click(function () {
        $.alert.open("Message", "It is required if any allowance or deduction impact with regular salary, changes regular configured Salary amount");

    }); 

});



// Test Mizan

$(".fieldOne").hide();

$('#EffectiveStartDateMsg').val('');
$('#EffectiveEndDateMsg').val('');

$('#enterStartDate').on('change', function () {
    if (this.checked) {
        $(".fieldOne").show();
    }
    else {
        $(".fieldOne").hide();
        //alert("One");
        $('#EffectiveStartDateMsg').val('');
        $('#EffectiveEndDateMsg').val('');
    };



});



// Test End Mizan 