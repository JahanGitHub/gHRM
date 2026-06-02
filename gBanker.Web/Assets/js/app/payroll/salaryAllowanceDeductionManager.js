
var isProductDependentCheck = false;
var appAllawanceDeduction = {
    populateSalaryFromToDate: function () {
        var salaryYear = $('#SalaryYear').val();
        var salaryMonth = $("#SalaryMonth option:selected").text();
        var salaryMonthInValue = $("#SalaryMonth").val();

        if (!salaryYear || salaryYear === '' || !salaryMonth || salaryMonth === 'Please Select') return;

        salaryMonth = salaryMonth.substring(0, 3);
        var firstDateInString = `01-${salaryMonth}-${salaryYear}`;
        var fromDate = new Date(`${firstDateInString}`);
        var lastDateOfMonth = new Date(salaryYear, salaryMonthInValue, 0);
        var toDate = `${$.datepicker.formatDate('dd-M-yy', lastDateOfMonth)}`;

        $('#StartDate').val(firstDateInString);
        $('#EndDate').val(toDate);
    }
}

$(document).ready(function () {

    $('#SalaryYear,#SalaryMonth').on('change', function () {
        $('#StartDate').val('');
        $('#EndDate').val('');
        appAllawanceDeduction.populateSalaryFromToDate();
    });

    $("#OvertimeDiv").hide();
    $("#DeductionDaysDiv").hide();
    $("#visibilityComponent").hide();
     
    $("#EmployeeCode").blur(function (e) {
        var employeeCode = $("#EmployeeCode").val();

        if (employeeCode !== '') {
            //load employee basic info
            LoadEmpInfoByCode(employeeCode);
        }
        else {
            $("#EmployeeName").empty();
            $("#EmployeeName").val('');
        }      

        //load allowance,deduction and salary configurations
        getEmployeeSalaryConfigurations();
    });

    $("#SalaryYear").change(function (e) {       
        //load allowance,deduction and salary configurations
        getEmployeeSalaryConfigurations();
    });

    $("#SalaryMonth").change(function (e) {
        //load allowance,deduction and salary configurations
        getEmployeeSalaryConfigurations();
    });
    
    $("#ComponentCategory").change(function () {


        $("#OvertimeDiv").hide();
        $("#DeductionDaysDiv").hide();

        var ComponentCategory = $("#ComponentCategory").val();
        var employeeTypeId = $("#EmployeeTypeId").val();
        var EmployeeStatusId = $("#EmployeeStatusId").val();
        if (ComponentCategory !== "" && employeeTypeId !== "" && EmployeeStatusId !== "") {
            GetPRComponentList(employeeTypeId, EmployeeStatusId, ComponentCategory);
        } else if (ComponentCategory === "") {
            $("#PRComponentId").html('<option value="">Please Select</option>');
        }
        else {
            $.alert.open("Error", "Please Search Employee First");
        }
    });

    $("#PRComponentId").change(function () {
        var componentId = $("#PRComponentId").val();
    });

    $("#PRComponentId").change(function () {

        $("#PRComponentAmount").val(0.00);
        $("#DeductionDays").val('');
        $("#OvertimeHour").val('');

        var ComponentName = $("#PRComponentId option:selected").text();
        if (ComponentName === 'Overtime') {
            $("#OvertimeDiv").show();
            $("#DeductionDaysDiv").hide();
        } else if (ComponentName === 'Leave Without Payment') {
            $("#DeductionDaysDiv").show();
            $("#OvertimeDiv").hide();
        }
        else {
            $("#OvertimeDiv").hide();
            $("#DeductionDaysDiv").hide();
        }
        var prCompoId = $("#PRComponentId").val();
        if (prCompoId !== "") {
            CheckProductDependent(prCompoId);
        } else {
            $("#PRComponentId").html('<option value="">Please Select</option>');
        }
    });


    $("#OvertimeHour").blur(function () {

        var overtimeHour = $("#OvertimeHour").val();
        var maxOvertimePerMonth = $("#MaxOvertimePerMonth").val();
        if (parseFloat(overtimeHour) <= parseFloat(maxOvertimePerMonth)) {
            var overtimeRate = $("#OvertimeRate").val();
            var total = overtimeRate * overtimeHour;
            $("#PRComponentAmount").val(total);
        } else if (maxOvertimePerMonth === 0) {

        } else {
            $("#OvertimeHour").val('');
            $.alert.open("Error", "Overtime hour can't greater than configured hour " + maxOvertimePerMonth);
        }
    });


    $("#PRComponentAmount").blur(function () {

        var componentCategory = $("#ComponentCategory").val();
        if (componentCategory === "Deduction") {
            var amount = parseFloat($("#PRComponentAmount").val());
            var salaryAmt = parseFloat($("#salaryAmount").val());//$("#TotalSalaryPayable").text();

            if (amount > salaryAmt) {
                $("#PRComponentAmount").val('');
                $.alert.open("Error", "Deduction amount must be less than Salary amount");
            }
        }
    });

    $("#ProductGroup").change(function () {
        var prodGroupId = $("#ProductGroup").val();
        if (prodGroupId !== "") {
            GetProductTypeByGroup(prodGroupId);
        } else {
            $("#ProductGroup").html('<option value="">Please Select</option>');
            $("#ProductId").html('<option value="">Please Select</option>');
            //$("#prodQty").hide();
        }
    });

    $("#ProductType").change(function () {
        var prodGroupId = $("#ProductGroup").val();
        var prodTypeId = $("#ProductType").val();
        if (prodGroupId !== "" && prodTypeId !== "") {
            GetProductByProductType(prodGroupId, prodTypeId);
        } else {
            $("#ProductName").html('<option value="">Please Select</option>');
        }
    });

    $("#ProductName").change(function () {

        var prodGroupId = $("#ProductGroup").val();
        var prodTypeId = $("#ProductType").val();
        var prodName = $("#ProductName").val();
        if (prodGroupId !== "" && prodTypeId !== "" && prodName !== "") {
            GetSerialNoByProductId(prodName);
        } else {
            $("#SerialId").html('<option value="">Please Select</option>');
        }
    });
    
    $("#btnSave").click(function () {

        var StartDate = $("#StartDate").val();
        var EndDate = $("#EndDate").val();
        var DeductionDays = $("#DeductionDays").val();
        var EmployeeId = $("#EmployeeId").val();
        var PRComponentId = $("#PRComponentId").val();
        var PRComponentAmount = $("#PRComponentAmount").val();
        var OvertimeHour = $("#OvertimeHour").val();
        var conponentCategory = $("#ComponentCategory").val();
        var employeeCode = $("#EmployeeCode").val();

        var salaryYear = $("#SalaryYear").val();
        var salaryMonth = $("#SalaryMonth").val();


        var remark = $("#Remark").val();

        if (conponentCategory !== "") {
            if (PRComponentAmount === "") {
                $.alert.open("Message", "Please Give Amount.");
                return false;
            }
            if (PRComponentId === '') {
                $.alert.open("Message", "Please Give Component.");
                return false;
            }
            if (StartDate === '') {
                $.alert.open("Message", "Please Give Start Date.");
                return false;
            }
            if (EndDate === '') {
                $.alert.open("Message", "Please Give End Date.");
                return false;
            }

            if (salaryYear === '') {
                $.alert.open("Message", "Please Give Salary Year.");
                return false;
            }

            if (salaryMonth === '') {
                $.alert.open("Message", "Please Give Salary Month.");
                return false;
            }

            CreateSalaryIncentive(employeeCode, StartDate, EndDate, EmployeeId, PRComponentId,
                PRComponentAmount, OvertimeHour, DeductionDays, conponentCategory, remark,
                salaryYear, salaryMonth
                );

        } else {
            $.alert.open("Message", "Please Provide Component Category.");
        }
    });
});

function GetProductTypeByGroup(productGroupId) {
    var ddlProdType = $("#ProductType");

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryAllowance/GetProuductTypebyProductGroupId',
        cache: false,
        data: { productGroupId: productGroupId },
        dataType: 'json',
        async: false,
        success: function (data) {
            ddlProdType.html('');
            $.each(data.data, function (id, option) {
                ddlProdType.append($('<option></option>').val(option.Value).html(option.Text));
            });
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function GetProductByProductType(productGroupId, productTypeId) {
    var ddlProdName = $("#ProductName");
    var employeeId = $("#EmployeeId").val();

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",// GetProductByProductType
        url: '/PRSalaryAllowance/GetProductListByProductType',
        data: { productGroupId: productGroupId, productTypeId: productTypeId, productAssignId: employeeId },
        dataType: 'json',
        async: false,
        success: function (data) {
            ddlProdName.html('');
            $.each(data.data, function (id, option) {
                ddlProdName.append($('<option></option>').val(option.Value).html(option.Text));
            });
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function GetSerialNoByProductId(productId) {

    var ddlSerialNumber = $("#SerialId");
    var employeeIdNo = $("#EmployeeId").val();

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryAllowance/GetSerialNnumberByProductId',
        data: { productId: productId, employeeId: employeeIdNo },
        dataType: 'json',
        success: function (data) {
            ddlSerialNumber.html('');
            $.each(data.data, function (id, option) {
                ddlSerialNumber.append($('<option></option>').val(option.Value).html(option.Text));
            });
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function CheckProductDependent(componentId) {

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryAllowance/CheckProductDependent',
        data: { componentId: componentId },
        dataType: 'json',
        async: true,
        success: function (data) {

            if (data.data === "Y") {
                isProductDependentCheck = true;
                $("#visibilityComponent").show();
            } else {
                isProductDependentCheck = false;
                $("#visibilityComponent").hide();
            }
        },
        error: function (request, status, error) {
            $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function GetPRComponentList(EmployeeTypeId, EmployeeStatusId, ComponentCategory) {
    var EmployeeId = $("#EmployeeId").val();
    if (EmployeeTypeId !== "" && EmployeeStatusId !== "" && EmployeeId !== "") {
        var ddlComponent = $("#PRComponentId");

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryAllowance/GetPRComponentList',
            data: { EmployeeTypeId: EmployeeTypeId, EmployeeStatusId: EmployeeStatusId, ComponentCategory: ComponentCategory, EmployeeId: EmployeeId },
            dataType: 'json',
            async: true,
            success: function (data) {
                ddlComponent.html('');
                $.each(data, function (id, option) {
                    ddlComponent.append($('<option></option>').val(option.Value).html(option.Text));
                });
            },
            error: function (request, status, error) {
                $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function deleteAllowance(NewSl) {
    var SalaryIncentiveId = $("#txtDeleteComponent" + NewSl).val();
    var empCode = $("#EmployeeCode").val();
    var employeeId = $('#EmployeeId').val();
    var salaryYear = $('#SalaryYear').val();
    var salaryMonth = $('#SalaryMonth').val();

    if (!employeeId || employeeId <= 0) { $.alert.open('Employee is Required'); return; }    
    if (!empCode || empCode === '') { $.alert.open('Employee is Required'); return; }
    if (!salaryYear || salaryYear <=0) { $.alert.open('Salary Year is Required'); return; }
    if (!salaryMonth || salaryMonth <= 0) { $.alert.open('Salary Month is Required'); return; }

    if (PRComponentId !== "") {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryAllowance/DeleteSalaryAllowance',
            data: { SalaryIncentiveId: SalaryIncentiveId, salaryYear: salaryYear, salaryMonth: salaryMonth },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (data) {
                console.log(data);
                $.alert.open(data);
                LoadAllowanceTable(empCode, salaryYear, salaryMonth);
                GetTotalPayable();
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function deleteDeduction(NewSl) {
    var Id = $("#txtDeleteDeduction" + NewSl).val();
    var salaryMonth = $("#SalaryMonth").val();
    var salaryYear = $("#SalaryYear").val();

    if (!salaryYear || salaryYear === 0) return $.alert.open('Salary Year is Required');
    if (!salaryMonth || salaryMonth === 0) return $.alert.open('Salary Month is Required');

    var empCode = $("#EmployeeCode").val();
    if (PRComponentId !== "") {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryAllowance/DeleteSalaryDeduction',
            data: { Id: Id, salaryMonth: salaryMonth, salaryYear: salaryYear },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (data) {
                $.alert.open(data);               
                LoadDeductionTable(empCode, salaryYear, salaryMonth);                
                GetTotalPayable();
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    }
}

function GenerateTableWithData(dataTable) {
    $("#totalConfiguredSalary").html(0);
    var dtTable = $('#tblSalaryConfiguration');
    dtTable.find('tbody').html("");
    var NewSl = 0;
    var lastSl = 0;
    var rowValue = "";
    var prComponentId = dataTable[0].PRComponentId;
    var totalAmt = 0;
    if (prComponentId != "") {
        $.each(dataTable, function (id, option) {
            NewSl++;
            rowValue += ('<tr id="tableRow' + NewSl + '">' +
                '<td><input type="text" value="' + NewSl + '" style="display:none;" name="NewSl" id="txtNewSl' + NewSl + '" /></td>' +
                '<td>' + option.ComponentName + '<input type="text" value="' + option.PRComponentId + '" style="display:none;" name="PRComponentId" id="txtPRComponentId' + NewSl + '" /><input type="text" value="' + option.ComponentName + '" style="display:none;" name="ComponentName" id="ComponentName' + NewSl + '" /></td>' +
                '<td>' + option.TransactionTypeView + '<input type="text" value="' + option.TransactionTypeView + '" style="display:none;" name="CalculatedAmount" id="txtTransactionTypeView' + NewSl + '" /></td>' +
                '<td>' + option.CalculatedAmount + '<input type="text" value="' + option.CalculatedAmount + '" style="display:none;" name="CalculatedAmount" id="txtCalculatedAmount' + NewSl + '" /></td>' +
                //'<td>' + option.ComponentCategory + '<input type="text" value="' + option.ComponentCategory + '" style="display:none;" name="ComponentCategory" id="txtComponentCategory' + NewSl + '" /></td>' +
                //'<td>' + option.TransactionType + '<input type="text" value="' + option.TransactionType + '" style="display:none;" name="TransactionType" id="txtTransactionType' + NewSl + '" /></td>' +
                '</tr>');
            if (option.TransactionTypeView === 'Salary Addition') {
                totalAmt = totalAmt + option.CalculatedAmount;
            }
            else {
                totalAmt = totalAmt - option.CalculatedAmount;
            }
        });
        var tableBody = dtTable.find('tbody').html(rowValue);
        $("#totalConfiguredSalary").html(Math.round(totalAmt));
    }
}

function LoadConfigredSalaryTable(employeeCode, salaryYear, salaryMonth) {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryAllowance/GetExistingSalaryConfigurationListbyEmployeeCode',
        data: { employeeCode: employeeCode, salaryYear: salaryYear, salaryMonth: salaryMonth },
        dataType: 'json',
        async: false,
        cache: false,
        success: function (data) {
            if (data.dataList.length > 0) {
                GenerateTableWithData(data.dataList);
            }
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function LoadEmpInfoByCode(employeeCode) {
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryAllowance/GetEmployeeBasicInfo',
        data: { EmpCode: employeeCode },
        dataType: 'json',
        async: true,
        success: function (data) {
            $.each(data, function (index, data) {
                if (data !== "Error") {
                    // $("#OfficeID").val(data.OfficeID);
                    $("#EmployeeId").val(data.EmployeeId);
                    $("#EmployeeName").val(data.EmployeeName);
                    $("#EmployeeTypeId").val(data.EmployeeTypeId);
                    $("#EmployeeStatusId").val(data.EmployeeStatusId);
                    $("#MaxOvertimePerMonth").val(data.MaxOvertimePerMonth);
                    $("#OvertimeRate").val(data.OvertimeRate);
                    //GetPRComponentList(data.EmployeeTypeId, data.EmployeeStatus);

                } else {
                    //$("#OfficeID").val("");
                    $("#EmployeeId").val("");
                    $("#EmployeeName").val("");
                    $("#EmployeeTypeId").val('');
                    $("#EmployeeStatusId").val('');
                    $.alert.open("Wrong Employe Code");
                    //$("#OfficeDesignationId").val(0);
                }
            });
        },
        error: function (request, status, error) {
            $("#EmployeeName").val("");
            $("#EmployeeId").val("");
            //$("#OfficeID").val("");
            $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function CreateSalaryIncentive(employeeCode, StartDate, EndDate, EmployeeId, PRComponentId,
    PRComponentAmount, OvertimeHour, DeductionDays, conponentCategory, remark,
    salaryYear, salaryMonth
    ) {
    var saveCondition = true;
    var productId = 0;
    var serialId = 0;
    var isProductDependent = 0;

    if (isProductDependentCheck === false) {
        productId = 0;
        serialId = 0;
        isProductDependent = 0;
    }

    if (isProductDependentCheck === true) {
        productId = $("#ProductName").val();
        serialId = $("#SerialId").val();
        isProductDependent = 1;
    }

    if (isProductDependentCheck === true) {
        if (productId === '' || serialId === '') {
            $.alert.open("Message", "This Component Depends on Product, Please provide product before calculation, Save Denied");
            saveCondition = false;
        }
    }

    if (saveCondition === true) {
        $('#AjaxLoader').show();
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryAllowance/CreateIncentive',
            data: {
                employeeId: EmployeeId,
                dateStartFrom: StartDate,
                dateEndTo: EndDate,
                prComponentId: PRComponentId,
                prComponentAmount: PRComponentAmount,
                prComponentHour: OvertimeHour,
                deductionDays: DeductionDays,
                conponentCategory: conponentCategory,
                productId: productId,
                serialId: serialId,
                isProductDependent: isProductDependent,
                remark: remark,
                salaryYear,
                salaryMonth
            },
            dataType: 'json',
            async: true,
            success: function (data) {
                $('#AjaxLoader').hide();
                $.alert.open("Message", data);

                //load allowance,deduction and salary configurations
                getEmployeeSalaryConfigurations();               
            },
            error: function (request, status, error) {
                $.alert.open("Message", error);
            }
        });
    }
}

function LoadAllowanceTable(employeeCode, salaryYear, salaryMonth) {

    console.log('salaryYear, salaryMonth ' + salaryYear+' , '+ salaryMonth);
    $("#totalAllowance").html(0);     
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryAllowance/SalaryIncentiveListByEmployeeCode',
        data: { employeeCode: employeeCode, salaryYear: salaryYear, salaryMonth: salaryMonth },
        dataType: 'json',
        async: false,
        cache: false,
        success: function (data) {
            if (data.Result === "OK") {
                var dtTable = $('#tblAllowanceConfiguration');
                dtTable.find('tbody').html("");
                var NewSl = 0;
                var lastSl = 0;
                var rowValue = "";
                var dataTable = data.dataList;
                var prComponentId = data.PRComponentId;
                var totalAmt = 0;
                if (prComponentId !== "") {
                    $.each(dataTable, function (id, option) {
                        NewSl++;
                        rowValue += ('<tr id="tableRow' + NewSl + '">' +
                            '<td><input type="text" value="' + NewSl + '" style="display:none;" name="NewSl" id="txtNewSl' + NewSl + '" /></td>' +
                            '<td>' + option.ComponentName + '<input type="text" value="' + option.SalaryIncentiveId + '" style="display:none;" name="PRComponentId" id="txtPRComponentId' + NewSl + '" /><input type="text" value="' + option.ComponentName + '" style="display:none;" name="ComponentName" id="ComponentName' + NewSl + '" /></td>' +
                            '<td>' + option.PRComponentAmount + '<input type="text" value="' + option.PRComponentAmount + '" style="display:none;" name="CalculatedAmount" id="txtCalculatedAmount' + NewSl + '" /></td>' +
                            '<td>' + option.StartDateMsg + '<input type="text" value="' + option.StartDateMsg + '" style="display:none;" name="StartDateMsg" id="txtStartDate' + NewSl + '" /></td>' +
                            '<td>' + option.EndDateMsg + '<input type="text" value="' + option.EndDateMsg + '" style="display:none;" name="EndDateMsg" id="txtEndDateMsg' + NewSl + '" /></td>' +
                            '<td>' + option.Remark + '<input type="text" value="' + option.Remark + '" style="display:none;" name="CalculatedAmount" id="txtCalculatedAmount' + NewSl + '" /></td>' +
                            //'<td>' + option.ComponentCategory + '<input type="text" value="' + option.ComponentCategory + '" style="display:none;" name="ComponentCategory" id="txtComponentCategory' + NewSl + '" /></td>' +
                            //'<td>' + option.TransactionType + '<input type="text" value="' + option.TransactionType + '" style="display:none;" name="TransactionType" id="txtTransactionType' + NewSl + '" /></td>' +
                            '<td style="text-align:center;">' +
                                             '<a href="javascript:;" onclick="deleteAllowance(' + NewSl + ')"><i class="fa fa-trash-o fa-2x"></i></a>' +
                                             '<input type="text" value="' + option.SalaryIncentiveId + '" style="display:none;" name="NewSl" id="txtDeleteComponent' + NewSl + '" />' +
                                             '</td>' +
                            '</tr>');
                        totalAmt = totalAmt + option.PRComponentAmount;
                    });
                    var tableBody = dtTable.find('tbody').html(rowValue);
                    $("#totalAllowance").html(totalAmt);
                }
            }
            else {
                // ???
            }
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function LoadDeductionTable(employeeCode, salaryYear, salaryMonth) {
    $("#totalDeduction").html(0);

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/PRSalaryAllowance/SalaryDeductionListByEmployeeCode',
        data: { employeeCode: employeeCode, salaryYear: salaryYear, salaryMonth: salaryMonth },
        dataType: 'json',
        async: false,
        cache: false,
        success: function (data) {
            if (data.Result === "OK") {
                //GenerateTableWithData(data.dataList);
                var dtTable = $('#tblDeductionConfiguration');
                dtTable.find('tbody').html("");
                var NewSl = 0;
                var lastSl = 0;
                var rowValue = "";
                var dataTable = data.dataList;
                var prComponentId = data.PRComponentId;
                var totalAmt = 0;
                if (prComponentId !== "") {
                    $.each(dataTable, function (id, option) {
                        NewSl++;
                        rowValue += ('<tr id="tableRow' + NewSl + '">' +
                            '<td><input type="text" value="' + NewSl + '" style="display:none;" name="NewSl" id="txtNewSl' + NewSl + '" /></td>' +
                            '<td>' + option.ComponentName + '<input type="text" value="' + option.SalaryIncentiveId + '" style="display:none;" name="PRComponentId" id="txtPRComponentId' + NewSl + '" /><input type="text" value="' + option.ComponentName + '" style="display:none;" name="ComponentName" id="ComponentName' + NewSl + '" /></td>' +
                            '<td>' + option.PRComponentAmount + '<input type="text" value="' + option.PRComponentAmount + '" style="display:none;" name="CalculatedAmount" id="txtCalculatedAmount' + NewSl + '" /></td>' +
                            '<td>' + option.StartDateMsg + '<input type="text" value="' + option.StartDateMsg + '" style="display:none;" name="StartDateMsg" id="txtStartDate' + NewSl + '" /></td>' +
                            '<td>' + option.EndDateMsg + '<input type="text" value="' + option.EndDateMsg + '" style="display:none;" name="EndDateMsg" id="txtEndDateMsg' + NewSl + '" /></td>' +
                            '<td>' + option.Remark + '<input type="text" value="' + option.Remark + '" style="display:none;" name="CalculatedAmount" id="txtCalculatedAmount' + NewSl + '" /></td>' +
                            //'<td>' + option.ComponentCategory + '<input type="text" value="' + option.ComponentCategory + '" style="display:none;" name="ComponentCategory" id="txtComponentCategory' + NewSl + '" /></td>' +
                            //'<td>' + option.TransactionType + '<input type="text" value="' + option.TransactionType + '" style="display:none;" name="TransactionType" id="txtTransactionType' + NewSl + '" /></td>' +
                            '<td style="text-align:center;">' +
                                             '<a href="javascript:;" onclick="deleteDeduction(' + NewSl + ')"><i class="fa fa-trash-o fa-2x"></i></a>' +
                                             '<input type="text" value="' + option.SalaryIncentiveId + '" style="display:none;" name="NewSl" id="txtDeleteDeduction' + NewSl + '" />' +
                            '</td>' +

                            '</tr>');
                        totalAmt = totalAmt + option.PRComponentAmount;

                    });
                    var tableBody = dtTable.find('tbody').html(rowValue);
                    $("#totalDeduction").html(totalAmt);
                }
            }
            else {
                // ???
            }
        },
        error: function (request, status, error) {
            alert(request.statusText + "/" + request.statusText + "/" + error);
        }

    });
}

function GetTotalPayable() {
    var totalAllowance = parseFloat($("#totalAllowance").text());
    var totalDeduction = parseFloat($("#totalDeduction").text());
    var totalConfiguredSalary = parseFloat($("#totalConfiguredSalary").text());
    var totalPayable = totalConfiguredSalary + totalAllowance - totalDeduction;
    var totalPayableAfterRounding = Math.round(totalPayable);
    $("#salaryAmount").val(totalPayableAfterRounding);
    $("#TotalSalaryPayable").text("Total Payable Amount: " + totalPayableAfterRounding);
}

function getEmployeeSalaryConfigurations(){
    var employeeCode = $("#EmployeeCode").val();
    var salaryYear = $("#SalaryYear").val();
    var salaryMonth = $("#SalaryMonth").val();
    
    if (!(employeeCode !== '' && salaryYear > 0 && salaryMonth > 0)){
        $("#totalAllowance").html(0);
        var dtTable = $('#tblAllowanceConfiguration');
        dtTable.find('tbody').html("");

        $("#totalDeduction").html(0);
        var dtTable = $('#tblDeductionConfiguration');
        dtTable.find('tbody').html("");

        $("#totalConfiguredSalary").html(0);
        var dtTable = $('#tblSalaryConfiguration');
        dtTable.find('tbody').html("");
        
        $("#salaryAmount").val("0.00");
        $("#TotalSalaryPayable").text("Total Payable Amount: 0.00");

        return;
    }

    LoadAllowanceTable(employeeCode, salaryYear, salaryMonth);
    LoadDeductionTable(employeeCode, salaryYear, salaryMonth);
    LoadConfigredSalaryTable(employeeCode, salaryYear, salaryMonth);
    GetTotalPayable();
}
