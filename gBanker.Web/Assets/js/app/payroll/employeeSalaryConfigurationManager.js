
var empSalaryConfigManager = {
    getSalaryConfiguration: function () {
        var employee_code = $("#txtEmpName").val();
        if ($("#txtEmpName").val() !== '') {
            LoadEmployeeInformationbyCode(employee_code);
        }
        else {
            $("#EmployeeName").empty();
            $("#EmployeeName").val('');
            $("#OfficeID").val('');
            $("#GrossSalary").val('');
        }
    }
}

$(document).ready(function () {
    DesignTimePicker();
    $("#GrossSalary").attr('readonly', true);
    $("#gradeScaleDiv,#incomeTaxDiv,#dvFractionStep,.section-overtime").hide();
    $("#IsOverTime").val("false");

    $("#txtEmpName").blur(function (e) {
        empSalaryConfigManager.getSalaryConfiguration();
        if (ISValidData()) {
            if (system != 'NGF') {
                GenerateEmployeeSalaryConfiguration($("#EmployeeID").val());
            }
        }
    });

    $("#IncomeTax").blur(function () {
        var dtTable = $('#tblSalaryConfiguration');
        var incomeTax = $("#IncomeTax").val();

        var NewSl = 0;
        var rowValue = "";
        var rowCount = $('#tblSalaryConfiguration >tbody >tr').length;
        for (i = 1; i <= rowCount; i++) {
            NewSl++;
            var Sl = $('#tblSalaryConfiguration tr:eq(' + i + ') td:first input[type="text"]').val();
            var ComponentName = $("#ComponentName" + Sl).val();

            var PRComponentId = $("#txtPRComponentId" + Sl).val();
            var CalculatedAmount = $("#txtCalculatedAmount" + Sl).val();
            var ComponentCategory = $("#txtComponentCategory" + Sl).val();
            var TransactionType = $("#txtTransactionType" + Sl).val();

            if (ComponentName === "Income Tax") {
                CalculatedAmount = incomeTax;
            }
            rowValue += ('<tr id="tableRow' + NewSl + '">' +
                '<td><input type="text" value="' + NewSl + '" style="display:none;" name="NewSl" id="txtNewSl' + NewSl + '" /></td>' +
                '<td>' + ComponentName + '<input type="text" value="' + PRComponentId + '" style="display:none;" name="PRComponentId" id="txtPRComponentId' + NewSl + '" /><input type="text" value="' + ComponentName + '" style="display:none;" name="ComponentName" id="ComponentName' + NewSl + '" /></td>' +
                '<td>' + CalculatedAmount + '<input type="text" value="' + CalculatedAmount + '" style="display:none;" name="CalculatedAmount" id="txtCalculatedAmount' + NewSl + '" /></td>' +
                '<td>' + ComponentCategory + '<input type="text" value="' + ComponentCategory + '" style="display:none;" name="ComponentCategory" id="txtComponentCategory' + NewSl + '" /></td>' +
                '<td>' + TransactionType + '<input type="text" value="' + TransactionType + '" style="display:none;" name="TransactionType" id="txtTransactionType' + NewSl + '" /></td>' +
                '<td>' + TransactionTypeView + '<input type="text" value="' + TransactionTypeView + '" style="display:none;" name="TransactionTypeView" id="txtTransactionTypeView' + NewSl + '" /></td>' +
                '<td><input type="checkbox" id="chk' + NewSl + '" checked  /></td>' +
                '</tr>');
        }
        dtTable.find('tbody').html("");
        dtTable.find('tbody').html(rowValue);

    });

    $("#IncomeTax").keypress(function (e) {
        var isNumeric = checkNumeric(e);
        return isNumeric;

    });

    $("#IsOverTime").change(function () {
        var isOvertime = $("#IsOverTime").val();
        $("#IsOvertimeException").prop('checked', false);
        if (isOvertime === "true") {
            $(".section-overtime").show();
        } else {
            $(".section-overtime").hide();
            $("#MaxOvertimePerDay").val(0);
            $("#MaxOvertimePerMonth").val(0);
            $("#MaxOvertimePerMonth").val(0);
        }
    });

    function ISValidData() {
        if ($('#PFTypeId').val() == "") {
            alert("Please Select Provident Fund Type");
            return false;
        }
        if ($("#SalaryGenerationType").val() == "") {
            alert("Please Select Salary Generation Type");
            return false;
        }
        if ($("#EmployeeSalaryType").val() == "") {
            alert("Please Select Salary Type");
            return false;
        }

        return true;
    }

    $("#SalaryGenerationType").change(function () {

        hideUnhideDiv();
        if (ISValidData()) {
            GenerateEmployeeSalaryConfiguration();
        }

    });

    $("#EmployeeSalaryType").change(function () {
        $("#SalaryScaleList,#GradeList,#FractionStep").val('');
        if (ISValidData()) {
            GenerateEmployeeSalaryConfiguration();
        }
    });

    $("#GrossSalary").blur(function () {

        if (ISValidData()) {
            GenerateEmployeeSalaryConfiguration();
        }
    });


    $("#GradeList").change(function () {
        $("#SalaryScaleList,#FractionStep,#GrossSalary").val('');
        $("#grossSalaryDiv").hide();
        var stepHtm = ``;
        if ($(this).val()) {
            $.ajax({
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                url: '/PRSalaryConfiguration/GetSalaryStepXGrade',
                data: { gradeid: $(this).val() },
                dataType: 'json',
                async: false,
                cache: false,
                success: function (data) {
                    $.each(data, function (key, row) {
                        stepHtm += `<option value="${row.Value}">${row.Text}</option>`
                    })
                },
                error: function (request, status, error) {
                }
            });
        }
        else stepHtm = `<option value>Please Select</option>`;

        $("#SalaryScaleList").html(stepHtm)
        var dtTable = $('#tblSalaryConfiguration');
        dtTable.find('tbody').html("");
    });

    $("#SalaryScaleList").change(function () {

        var pfTypeId = $('#PFTypeId').val();
        if (pfTypeId == null || pfTypeId == '') {
            $.alert.open("Message", "Please Select Provident Fund Type");
            return false;
        }
        else if ($("#FractionStep").val() && $(this).val() > 0) {
            $.alert.open("Message", "Input Step check");
            $("#FractionStep").val('')
            return false;
        }
        var dtTable = $('#tblSalaryConfiguration');
        dtTable.find('tbody').html("");

        var scale = $("#SalaryScaleList").val();
        var EmployeeID = $('#EmployeeID').val();
        if (scale != "") {
            var grade = $("#GradeList").val();
            var empSalaryTypeId = $("#EmployeeSalaryType").val();
            var employeeStatus = $("#EmployeeStatusId").val();
            if (grade != "" && empSalaryTypeId != "" && empSalaryTypeId != "") {              
                if (system == 'GUP')
                    GenerateSalaryForEmployeeInPayScale(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId);
                else if (system == 'Prottyashi')
                    GenerateSalaryForEmployeeInPayScale_Prottashi(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId, EmployeeID);
                else
                GenerateSalaryForEmployeeInPayScale(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId);
                $("#incomeTaxDiv").show();
            } else {
                $.alert.open("Message", "Please Salary Grade");
                return false;
            }
        } else {
            return false;
        }
    });

    $("#FractionStep").change(function () {

        var pfTypeId = $('#PFTypeId').val();

        if (pfTypeId == null || pfTypeId == '') {
            $.alert.open("Message", "Please Select Provident Fund Type");
            return false;
        }
        var dtTable = $('#tblSalaryConfiguration');
        dtTable.find('tbody').html("");

        var scale = $("#FractionStep").val();
        var EmployeeID = $('#EmployeeID').val();
        if (scale != "") {
            $("#SalaryScaleList").val(0);
            $("#SalaryScaleList").trigger("change");
            var grade = $("#GradeList").val();
            var empSalaryTypeId = $("#EmployeeSalaryType").val();
            var employeeStatus = $("#EmployeeStatus").val();
            if (grade != "" && empSalaryTypeId != "" && empSalaryTypeId != "") {
                if (system == 'GUP')
                GenerateSalaryForEmployeeInPayScale_designation(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId);
                else
                GenerateSalaryForEmployeeInPayScale(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId);
                $("#incomeTaxDiv").show();
            } else {
                $.alert.open("Message", "Please Salary Grade");
                return false;
            }
        } else {
            return false;
        }
    });

    $("#PFTypeId").change(function () {
        $("#SalaryScaleList,#FractionStep,#GrossSalary,#SalaryGenerationType").val('');
        // $("#grossSalaryDiv").hide();
        var dtTable = $('#tblSalaryConfiguration');
        dtTable.find('tbody').html("");
    });


    $("#EffectiveStartDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2050"
    });

    $("#EffectiveStartDate").datepicker('setDate', new Date());

    $("#EffectiveEndDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2100"
    });

    $("#EffectiveStartDate").change(function () {
        var startdate = $("#EffectiveStartDate").val();
        var effStartDate = new Date(MakeDate(startdate));
        var expireDate = effStartDate.setMonth(effStartDate.getMonth() + 36);
        var dd = new Date(expireDate - 1);
        var date = DateConversionToLongDate(dd);
        $("#EffectiveEndDate").val(date);
    });

    //************************//

    $("#PromotionDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2050"
    });

    $("#NextReviewDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2100"
    });

    $("#PromotionDate").change(function () {
        var startdate = $("#PromotionDate").val();
        var effStartDate = new Date(MakeDate(startdate));
        var expireDate = effStartDate.setMonth(effStartDate.getMonth() + 36);
        var dd = new Date(expireDate - 1);
        var date = DateConversionToLongDate(dd);
        $("#NextReviewDate").val(date);
    });

    //************************//

    $("#btnSalaryConfigurationSave").click(function () {

        var officeId = $("#OfficeID").val();
        var employeeId = $("#EmployeeID").val();
        var employeeTypeId = $("#EmployeeSalaryType").val();
        var pfTypeId = $("#PFTypeId").val();

        var gradeId = $("#GradeList").val();
        var step = $("#SalaryScaleList").val();
        var FractionStep = $("#FractionStep").val();

        var grossSalary = $("#GrossSalary").val();

        var promotionId = $("#PromotionId").val();

        var promotionTypeId = 0;
        var newDesignationId = 0;

        if (promotionId) {
            newDesignationId = $("#NewDesignationId").val();
            if (!newDesignationId) {
                $("#NewDesignationId").addClass("errorClass");
                return;
            }

            promotionTypeId = $("#PromotionTypeId").val();
            if (!promotionTypeId) {
                $("#PromotionTypeId").addClass("errorClass");
                return;
            }
        }

        var effectiveStartDate = $("#EffectiveStartDate").val();
        var effectiveEndDate = $("#EffectiveEndDate").val();

        if (MakeDate(effectiveStartDate) > MakeDate(effectiveEndDate)) {
            $.alert.open("Message: Effective End date can not be Earlier than Effective Start Date.");
            return;
        }

        var promotionDate = $("#PromotionDate").val();
        var nextReviewDate = $("#NextReviewDate").val();

        var bankAccount = $("#BankAccountNo").val();
        var bankName = $("#BankName").val();
        var bankBranchName = $("#BankBranchName").val();

        var isOverTime = $("#IsOverTime").val();
        var isOvertimeException = $('#IsOvertimeException').is(':checked');
        var maxOvertimePerDay = $("#MaxOvertimePerDay").val();
        var maxOvertimePerMonth = $("#MaxOvertimePerMonth").val();

        var loginTime = $("#LoginTime").val();
        var logoutTime = $("#LogoutTime").val();
        var lastLoginTime = $("#LastLoginTime").val();

        var loginTime2 = loginTime.split(":");;
        var logoutTime2 = logoutTime.split(":");

        var logINTim = loginTime2[0] + "" + loginTime2[1];
        var logOUTim = logoutTime2[0] + "" + logoutTime2[1];

        if (loginTime == '' || logoutTime == '' || lastLoginTime == '') {
            $.alert.open("Message", "Please Fill All required data.");
            return;
        }

        if (logOUTim < logINTim) {
            $.alert.open("Message", "Please Fill All required data.");
            return;
        }

        var rowCount = $('#tblSalaryConfiguration >tbody >tr').length;
        var salaryConfiguration = new Array();

        for (i = 1; i <= rowCount; i++) {
            var Sl = $('#tblSalaryConfiguration tr:eq(' + i + ') td:first input[type="text"]').val();
            if ($("#chk" + i).is(":checked")) {
                var componentId = $("#txtPRComponentId" + Sl).val();
                var componentAmount = $("#txtCalculatedAmount" + Sl).val();
                var componentCategory = $("#txtComponentCategory" + Sl).val();
                var transactionType = $("#txtTransactionType" + Sl).val();
                var obj = {
                    PRComponentID: componentId,
                    ComponentAmount: componentAmount,
                    ComponentCategory: componentCategory,
                    TransactionType: transactionType
                };
                salaryConfiguration.push(obj);
            }
        }

        $('#AjaxLoader').show();
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryConfiguration/SalaryConfigurationSave',
            data: JSON.stringify({
                SalaryConfigurationList: salaryConfiguration
                , officeId: officeId
                , employeeId: employeeId
                , newDesignationId: newDesignationId
                , promotionId: promotionId ? promotionId : 0
                , promotionTypeId: promotionTypeId
                , employeeTypeId: employeeTypeId
                , pfTypeId: pfTypeId
                , grossSalary: grossSalary
                , gradeId: gradeId
                , step: step
                , FractionStep: FractionStep
                , isOverTime: isOverTime
                , isOvertimeException: isOvertimeException
                , maxOvertimePerDay: maxOvertimePerDay
                , maxOvertimePerMonth: maxOvertimePerMonth
                , loginTime: loginTime
                , logoutTime: logoutTime
                , lastLoginTime: lastLoginTime
                , bankAccount: bankAccount
                , bankName: bankName
                , bankBranchName: bankBranchName
                , promotionDate: promotionDate
                , nextReviewDate: nextReviewDate
                , effectiveStartDate: effectiveStartDate
                , effectiveEndDate: effectiveEndDate
            }),
            dataType: 'json',
            async: false,
            success: function (Data) {
                $('#AjaxLoader').hide();
                if (Data === "OK") {
                    $("#btnPromotionCancel").trigger('click');
                    $.alert.open("Success", "Saved Successfully");
                }
                else {
                    $.alert.open("Error", Data);
                }
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    });
});

function LoadEmployeeInformationbyCode(employee_code) {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryConfiguration/GetExistingSalaryConfigurationListbyEmployeeCode',
        data: { employeeCode: employee_code },
        dataType: 'json',
        async: false,
        cache: false,
        success: function (data) {
            if (data.Result === "OK") {
                if (data.dataList.length > 0) {
                    GenerateTableWithData(data.dataList);
                }

                if (data.dataList !== null) {
                    if (data.dataList.length > 0 && data.dataList[0].GradeId == 0) {
                        $("#SalaryGenerationType").val("NPS");
                        $("#GradeList").val("");
                        $("#GrossSalary").attr('Disabled', false);
                    } else {
                        $("#SalaryGenerationType").val("PS");
                    }

                    //toggle div show hide on basis of SalaryGenerationType
                    hideUnhideDiv();

                    $("#NewDesignationId").val(data.DesignationId);
                    $("#EmployeeStatusId").val(data.EmployeeStausId);

                    $("#JoiningDate").val(data.JoiningDate);
                    $("#ConfirmationDate").val(data.ConfirmationDate);
                    $("#PromotionDate").val(data.PromotionDate);
                    $("#NextReviewDate").val(data.NextReviewDate);
                    $("#BankAccountNo").val(data.BankAccountNo);
                    $("#DepartmentName").val(data.DepartmentName);
                    $("#DesignationName").val(data.DesignationName);
                    $("#OfficeLocationId").val(data.OfficeLocationId);

                    $("#OfficeId").val(data.OfficeId);
                    $("#PFTypeId").val(data.PFTypeId);

                    $("#GradeList").val(data.GradeId);
                    if (data.GradeId > 0)
                    $("#GradeList").trigger("change");
                    $("#EmployeeID").val(data.dataList[0].EmployeeID);
                    $("#EmployeeName").val(data.dataList[0].EmployeeName);
                    $("#EmployeeStatus").val(data.dataList[0].EmployeeStatus);
                    $("#txtEmpStatus").val(data.dataList[0].EmployeeStatusName);
                    $("#IsOvertimeException").prop('checked', data.IsOvertimeException);

                    if (data.dataList[0].EmployeeTypeId === 0) {
                        $("#EmployeeSalaryType").val("");
                    } else {
                        $("#EmployeeSalaryType").val(data.dataList[0].EmployeeTypeId);
                    }

                    if (data.dataList[0].Step === 0) {
                        $("#SalaryScaleList").val("");
                    } else {

                        $("#SalaryScaleList").val(data.dataList[0].Step);
                    }

                    if (data.dataList[0].FractionStep === 0) {
                        $("#FractionStep").val("");
                    } else {
                        $("#FractionStep").val(data.dataList[0].FractionStep);
                    }

                    var bankName = data.BankName;
                    var bankBranchName = data.BankBranchName;

                    $("#BankName").val(bankName);
                    $("#BankBranchName").val(bankBranchName);
                    
                    var payrollConfigurationType = data.PayrollConfigurationType;

                    var grossOrBasicSalary = payrollConfigurationType === payrollConfigurationTypeEnum.GrossSalary
                        ? data.dataList[0].GrossSalary : data.dataList[0].BasicSalary

                    $("#GrossSalary").val(grossOrBasicSalary);
                    $("#EffectiveStartDate").val(data.dataList[0].EffectiveStartDate);
                    $("#EffectiveEndDate").val(data.dataList[0].EffectiveEndDate);

                    $("#IsOverTime").val("" + data.dataList[0].IsOverTime + "");
                    $("#MaxOvertimePerDay").val(data.dataList[0].MaxOvertimePerDay);
                    $("#MaxOvertimePerMonth").val(data.dataList[0].MaxOvertimePerMonth);

                    if (data.dataList[0].IsOverTime === true) {
                        $(".section-overtime").show();
                    } else {
                        $(".section-overtime").hide();
                    }
                    $("#LoginTime").val(data.dataList[0].LogInTime);
                    $("#LogoutTime").val(data.dataList[0].LogOutTime);
                    $("#LastLoginTime").val(data.dataList[0].LastLoginTime);
                }

            }
            else {
                $("#EmployeeID,#EmployeeName,#EmployeeStatusId,#OfficeLocationId,#DepartmentName,#DesignationName,#JoiningDate,#ConfirmationDate,#PFTypeId,#EmployeeSalaryType,#txtEmpStatus").val("");
                $('#tblSalaryConfiguration').find('tbody').html("");
            }
        },
        error: function (request, status, error) {
        }
    });
}

function GenerateSalaryForEmployeeInPayScale_designation(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId, EmployeeID ) {

    if (grade != "" && scale != "" && empSalaryTypeId != "" && pfTypeId != '') {

        var employeeStatusId = $("#EmployeeStatusId").val();
        var OfficeLocationId = $("#OfficeLocationId").val();

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryConfiguration/GenerateSalaryForEmployeeInPayScale_designation',
            data: { empSalaryTypeId: empSalaryTypeId, grade: grade, scale: scale, EmployeeStatusId: employeeStatusId, OfficeLocationId: OfficeLocationId, providentFundTypeId: pfTypeId, EmployeeID: EmployeeID },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (Data) {
                if (Data.Message == "OK") {
                    $("#GrossSalary").val(Data.grossSalary);
                    $("#GrossSalary").attr("readonly", true);
                    $("#grossSalaryDiv").show();
                    GenerateTableWithData(Data.dataTable);
                }
            },
            error: function (request, status, error) {
                //alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function GenerateSalaryForEmployeeInPayScale(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId) {

    if (grade != "" && scale != "" && empSalaryTypeId != "" && pfTypeId != '') {

        var employeeStatusId = $("#EmployeeStatusId").val();
        var OfficeLocationId = $("#OfficeLocationId").val();
        
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryConfiguration/GenerateSalaryForEmployeeInPayScale',
            data: { empSalaryTypeId: empSalaryTypeId, grade: grade, scale: scale, EmployeeStatusId: employeeStatusId, OfficeLocationId: OfficeLocationId, providentFundTypeId: pfTypeId },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (Data) {
                if (Data.Message == "OK") {
                    $("#GrossSalary").val(Data.grossSalary);
                    $("#GrossSalary").attr("readonly", true);
                    $("#grossSalaryDiv").show();
                    GenerateTableWithData(Data.dataTable);
                }
            },
            error: function (request, status, error) {
                //alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}


function GenerateSalaryForEmployeeInPayScale_Prottashi(empSalaryTypeId, grade, scale, employeeStatus, pfTypeId) {

    if (grade != "" && scale != "" && empSalaryTypeId != "" && pfTypeId != '') {

        var employeeStatusId = $("#EmployeeStatusId").val();
        var OfficeLocationId = $("#OfficeLocationId").val();

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryConfiguration/GenerateSalaryForEmployeeInPayScale_Prottashi',
            data: { empSalaryTypeId: empSalaryTypeId, grade: grade, scale: scale, EmployeeStatusId: employeeStatusId, OfficeLocationId: OfficeLocationId, providentFundTypeId: pfTypeId },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (Data) {
                if (Data.Message == "OK") {
                    $("#GrossSalary").val(Data.grossSalary);
                    $("#GrossSalary").attr("readonly", true);
                    $("#grossSalaryDiv").show();
                    GenerateTableWithData(Data.dataTable);
                }
            },
            error: function (request, status, error) {
                //alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}



function GenerateEmployeeSalaryConfiguration(empid = null) {
    
    var empSalaryTypeId = $("#EmployeeSalaryType").val();
    var salaryGenerationType = $("#SalaryGenerationType").val();
    var pfTypeId = $("#PFTypeId").val();
    var grossSalary = $("#GrossSalary").val();

    var employeeStatusId = $("#EmployeeStatusId").val();
    var OfficeLocationId = $("#OfficeLocationId").val();

    if (empSalaryTypeId != "" && salaryGenerationType != "" && pfTypeId != '' && grossSalary != "" && employeeStatusId != "" && OfficeLocationId != "") {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            cache: false,
            url: '/PRSalaryConfiguration/GenerateEmployeeSalary',
            data: { EmpSalaryTypeId: empSalaryTypeId, EmployeeStatusId: employeeStatusId, GrossSalary: grossSalary, SalaryGenerationType: salaryGenerationType, OfficeLocationId: OfficeLocationId, PfTypeId: pfTypeId, empid: empid },
            dataType: 'json',
            async: false,
            success: function (Data) {
                var data = Data;
                GenerateTableWithData(Data);
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function GenerateTableWithData(dataTable) {
    var dtTable = $('#tblSalaryConfiguration');
    dtTable.find('tbody').html("");
    var NewSl = 0;
    var lastSl = 0;
    var rowValue = "";
    if (dataTable[0] != null) {
        var prComponentId = dataTable[0].PRComponentId;

        if (prComponentId != "") {
            $.each(dataTable, function (id, option) {    
                NewSl++;
                if (option.ComponentName == "Income Tax") {
                    $("#IncomeTax").val(option.CalculatedAmount);
                }
                rowValue += ('<tr id="tableRow' + NewSl + '">' +
                    '<td><input type="text" value="' + NewSl + '" style="display:none;" name="NewSl" id="txtNewSl' + NewSl + '" /></td>' +
                    '<td>' + option.ComponentName + '<input type="text" value="' + option.PRComponentId + '" style="display:none;" name="PRComponentId" id="txtPRComponentId' + NewSl + '" /><input type="text" value="' + option.ComponentName + '" style="display:none;" name="ComponentName" id="ComponentName' + NewSl + '" /></td>' +
                    '<td>' + option.CalculatedAmount + '<input type="text" value="' + option.CalculatedAmount + '" style="display:none;" name="CalculatedAmount" id="txtCalculatedAmount' + NewSl + '" /></td>' +
                    '<td>' + option.ComponentCategory + '<input type="text" value="' + option.ComponentCategory + '" style="display:none;" name="ComponentCategory" id="txtComponentCategory' + NewSl + '" /></td>' +
                    '<td style="display:none">' + option.TransactionType + '<input type="text" value="' + option.TransactionType + '" style="display:none;" name="TransactionType" id="txtTransactionType' + NewSl + '" /></td>' +
                    '<td>' + option.TransactionTypeView + '<input type="text" value="' + option.TransactionTypeView + '" style="display:none;" name="TransactionTypeView" id="txtTransactionTypeView' + NewSl + '" /></td>' +
                    '<td><input type="checkbox" id="chk' + NewSl + '" checked  /></td>' +
                    '</tr>');
            });

            var tableBody = dtTable.find('tbody').html(rowValue);
            $("#incomeTaxDiv").show();
        } else {
            $("#incomeTaxDiv").hide();
        }
    }
}

function hideUnhideDiv() {
    var salType = $("#SalaryGenerationType").val();
    if (salType != "") {
        if (salType == "PS") {
            $("#GradeList,#SalaryScaleList,#dvFractionStep").val('');
            $("#gradeScaleDiv,#dvFractionStep,#grossSalaryDiv").show();
            $("#GrossSalary").attr("readonly", true);
        }
        else if (salType == "NPS") {
            $("#gradeScaleDiv,#dvFractionStep").hide();
            $("#GrossSalary").val('');
            $("#GrossSalary").removeAttr("readonly");
            $("#grossSalaryDiv").show();
        }
    } else {
        return false;
    }
}

function DesignTimePicker() {
    var options = {// now: "12:35", //hh:mm 24 hour format only, defaults to current time
        twentyFour: true, //Display 24 hour format, defaults to false
        upArrow: 'wickedpicker__controls__control-up', //The up arrow class selector to use, for custom CSS
        downArrow: 'wickedpicker__controls__control-down', //The down arrow class selector to use, for custom CSS
        close: 'wickedpicker__close', //The close class selector to use, for custom CSS
        hoverState: 'hover-state', //The hover state class to use, for custom CSS
        title: 'Timepicker', //The Wickedpicker's title,
        showSeconds: false, //Whether or not to show seconds,
        secondsInterval: 1, //Change interval for seconds, defaults to 1  ,
        minutesInterval: 1, //Change interval for minutes, defaults to 1
        beforeShow: null, //A function to be called before the Wickedpicker is shown
        show: null, //A function to be called when the Wickedpicker is shown
        clearable: false, //Make the picker's input clearable (has clickable "x")
    };

    $("#LoginTime").wickedpicker(options);
    $("#LogoutTime").wickedpicker(options);
    $("#LastLoginTime").wickedpicker(options);
    $("#LoginTime").val("10 : 00");
    $("#LogoutTime").val("18 : 00");
    $("#LastLoginTime").val("18 : 00");
}