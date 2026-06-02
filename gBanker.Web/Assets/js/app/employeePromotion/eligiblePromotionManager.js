
var eligiblePromotionManager = {
    init: function () {
        var currentYear = new Date().getFullYear();
        var currentMonth = new Date().getMonth();

        $('#Year').val(currentYear);
        $('#Month').val(currentMonth + 1);

        //overtime related default config
        $("#IsOverTime").val("false")
        $(".section-overtime").hide();
    },

    getPromotionDetail: function (promotionId, employeeCode) {

        if (!promotionId || promotionId <= 0 || !employeeCode) {
            var type = 'Warning';
            var message = 'Warning, Promotion or employee not found. Please try another!';
            app.showValidationAlert(type, message);
            return;
        }

        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/EmployeePromotion/GetExistingSalaryConfigurationListbyEmployeeCode',
            data: { employeeCode: employeeCode },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (data) {
                if (data.Result === "OK") {

                    $('#PromotionNavDiv').bPopup({
                        speed: 450,
                        transition: 'slideDown'
                    });

                    $("#PromotionId").val(promotionId);
                    $("#txtEmpName").val(employeeCode);                   
                    $("#txtEmpName").attr('Disabled', true);

                    if (data.dataList.length > 0) {
                        GenerateTableWithData(data.dataList);
                    }

                    if (data.dataList !== null) {
                        if (data.dataList[0].GradeId === 0) {
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

                        $("#EmployeeID").val(data.dataList[0].EmployeeID);
                        $("#EmployeeName").val(data.dataList[0].EmployeeName);
                        $("#EmployeeStatus").val(data.dataList[0].EmployeeStatus);
                        $("#txtEmpStatus").val(data.dataList[0].EmployeeStatusName);

                        if (data.dataList[0].EmployeeTypeId === 0) {
                            $("#EmployeeSalaryType").val("");
                        } else {
                            $("#EmployeeSalaryType").val(data.dataList[0].EmployeeTypeId);
                        }

                        if (data.dataList[0].Step == 0) {
                            $("#SalaryScaleList").val("");
                        } else {
                            $("#SalaryScaleList").val(data.dataList[0].Step);
                        }

                        var bankName = data.BankName;
                        var bankBranchName = data.BankBranchName;

                        $("#BankName").val(bankName);
                        $("#BankBranchName").val(bankBranchName);

                       
                        $("#GrossSalary").val(data.dataList[0].GrossSalary);
                        $("#EffectiveStartDate").val(data.dataList[0].EffectiveStartDate);
                        $("#EffectiveEndDate").val(data.dataList[0].EffectiveEndDate);
                                                
                        $("#IsOverTime").val("" + data.dataList[0].IsOverTime + "");
                        $("#MaxOvertimePerDay").val(data.dataList[0].MaxOvertimePerDay);
                        $("#MaxOvertimePerMonth").val(data.dataList[0].MaxOvertimePerMonth);

                        if (data.dataList[0].IsOverTime == true) {                           
                            $("#overtimeDuration").show();
                        } else {                           
                            $("#overtimeDuration").hide();
                        }
                        $("#LoginTime").val(data.dataList[0].LogInTime);
                        $("#LogoutTime").val(data.dataList[0].LogOutTime);
                        $("#LastLoginTime").val(data.dataList[0].LastLoginTime);                        
                    }
                }
            },
            error: function (request, status, error) {
            }
        });
    },

    /*
    promoteThisEmployee: function (promotionId, employeeCode) {
        

        var promotionInfo = `${employeeCode}@${promotionId}`;
        Cookies.remove(CookieCacheConstants.GHRM_PLUS_EMPLOYEE_PROMOTION);
        Cookies.set(CookieCacheConstants.GHRM_PLUS_EMPLOYEE_PROMOTION, JSON.stringify(promotionInfo));

        window.location.href = `'/promotionconfiguration/configure','_blank';return false;`;
      
    }
    */
};


function hideUnhideDiv() {
    var salType = $("#SalaryGenerationType").val();

    if (salType !== "") {
        if (salType === "PS") {
            $("#GradeList").val('');
            $("#SalaryScaleList").val('');
            $("#gradeScaleDiv").show();
            $("#grossSalaryDiv").show();
            $("#GrossSalary").attr("readonly", true);
        }
        else if (salType === "NPS") {
            $("#gradeScaleDiv").hide();
            $("#GrossSalary").val('');
            $("#GrossSalary").removeAttr("readonly");
            $("#grossSalaryDiv").show();
        }
    } else {
        return false;
    }
}


$(document).ready(function () {

    eligiblePromotionManager.init();

    GetDesignationDropdown();

    $("#filterColumn").change(function () {
        if ($(this).val() === "ViewAll") {
            $("#filterValue").val('');
        }
    });

    $("#txtNextPromotionReviewDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        changeYear: true,
        yearRange: "1920:2050"
    });

    $("#txtNextPromotionReviewDate").datepicker('setDate', new Date());

    $("#btnPromotionRejectionSave").click(function () {
        RejectionPromotion();
    });

    $("#btnPromotionCancel").click(function () {
        // Reset Fields
        $("#hdnEmployeeId").val("");
        SearchEligibleEmployees();
    });

    $("#SalaryGenerationType").change(function () {

        hideUnhideDiv();
        if (ISValidData()) {
            GenerateEmployeeSalaryConfiguration();
        }

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
});

function GenerateEmployeeSalaryConfiguration() {

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
            data: { EmpSalaryTypeId: empSalaryTypeId, EmployeeStatusId: employeeStatusId, GrossSalary: grossSalary, SalaryGenerationType: salaryGenerationType, OfficeLocationId: OfficeLocationId, PfTypeId: pfTypeId },
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

function fnPopupClose1() {
    var popup = $('#PromotionRejectionDiv').bPopup();
    popup.dispose = true;
    popup.close();
}

function GetDesignationDropdown() {
    var ddlDesignationID = $("#Designation");

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/EmployeePromotion/GetDesignationList',
        data: {},
        dataType: 'json',
        async: true,
        success: function (data) {
            ddlDesignationID.html('');
            $.each(data, function (id, option) {
                ddlDesignationID.append($('<option></option>').val(option.Value).html(option.Text));
            });
        },
        error: function (request, status, error) {
            $.alert.open(request.statusText + "/" + request.statusText + "/" + error);
        }
    });
}

function SearchEligibleEmployees() {
    var year = $("#Year option:selected").val();
    var month = $("#Month option:selected").text();
    var promotionTypeId = $("#PromotionTypeId1 option:selected").val();
    var designationId = $("#Designation option:selected").val();

    var reqFieldsName = new Array('#Year', '#Month', '#PromotionTypeId1');
    var isRequiredFiledValid = app.requiredFieldValidate(reqFieldsName);
    if (!isRequiredFiledValid || isRequiredFiledValid <= 0)
        return;

    $('#grid').html("");
    var dataSource = new kendo.data.DataSource({
        type: "aspnetmvc-ajax",
        pageSize: 25,
        schema: {
            data: "data", // records are returned in the "data" field of the response
            total: "total" // total number of records is in the "total" field of the response
        },
        serverPaging: true,   // enable server paging
        serverSorting: true,
        serverFiltering: true,
        transport: {
            read: {
                url: '/EmployeePromotion/GetPromotionEligibleEmployees',
                dataType: 'json',
                data: { Year: year, MonthName: month, PromotionTypeId: promotionTypeId, DesignationId: designationId }
            }
        }
    });

    $("#grid").kendoGrid({
        dataSource: dataSource,
        //height: 600,
        groupable: false,
        reorderable: true,
        filterable: true,
        sortable: true,

        selectable: false,
        resizable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [
             {
                 field: "Id",
                 hidden: true,
                 filterable: false
             },

             {
                 field: "rowSl",
                 title: "Sl",
                 width: "20px",
                 filterable: true
             },
            {
                field: "PromotionId",
                hidden: true,
                filterable: false
            },
             {

                 field: "EmployeeCode",
                 title: "Employee Code",
                 width: "50px",
                 filterable: true
                 //locked: true
             },
             {
                 field: "EmployeeName",
                 title: "Employee Name",
                 width: "80px",
                 filterable: true
                 //locked: true
             },
            //{
            //    field: "EmployeeTypeName",
            //    title: "Emp Type",
            //    width: "50px",
            //    filterable: true
            //    //locked: true
            //},

            //{
            //    field: "FirstJoiningDate",
            //    title: "Joining Date",
            //    width: "50px",
            //    filterable: true
            //    //locked: true
            //},
            {
                field: "DesignationName",
                title: "Current Desig Name",
                width: "80px",
                filterable: true
                //locked: true
            },
            {
                field: "DepartmentName",
                title: "Curr. Dept Name",
                width: "80px",
                filterable: true
                //locked: true
            },
            {
                field: "OfficeName",
                title: "Curr. Office Name",
                width: "50px",
                filterable: true
                //locked: true
            },
            {
                field: "PromotionTypeName",
                title: "Promotion Type",
                width: "50px",
                filterable: true
                //locked: true
            },
            {
                title: "Action",
                width: "50px",
                template: function (dataItem) {
                    var actions = ` <div class="text-center"> 
                                        <a title="Promote this Employee" target="_blank" href="/promotionconfiguration/configure?employeeCode=${dataItem.EmployeeCode}&promotionId=${dataItem.PromotionId} "><i class="fa fa-thumbs-up"></i></a>
                                        <a title="Reject this Promotion" href="javascript:void(0);" OnClick="Rejected('${dataItem.PromotionId}','${dataItem.NextReviewDateMsg}');"><i class="fa fa-thumbs-down"></i></a>
                                    </div>`;
                    return actions;
                }
            }
        ]
    });
}

function popupPromotionNavDiv(promotionId, employeeCode) {
    eligiblePromotionManager.getPromotionDetail(promotionId, employeeCode); 
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
                    '</tr>');
            });
            var tableBody = dtTable.find('tbody').html(rowValue);
            $("#incomeTaxDiv").show();
        } else {
            $("#incomeTaxDiv").hide();
        }
    }
}

function Rejected(PromotionId, NextReviewDateMsg) {

    $('#PromotionRejectionDiv').bPopup({
        speed: 450,
        transition: 'slideDown'
    });
    $("#pid").val(PromotionId);
    $("#NRDM").val(NextReviewDateMsg);
    $("#txtNextPromotionReviewDate").val('');
}

function RejectionPromotion()
{
    var PromotionId = $("#pid").val();    
    var NextReviewDate = $("#txtNextPromotionReviewDate").val();
    var Remarks = $("#txtRemarks").val();

    if (NextReviewDate !== "" && Remarks !== "") {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/EmployeePromotion/RejectPromotion',
            data: { PromotionId: PromotionId, NextReviewDate: NextReviewDate, Remarks: Remarks },
            dataType: 'json',
            async: true,
            success: function (data) {
                $.alert.open("Success", "Update Successfully");
                fnPopupClose1();
                $("#txtRemarks").val('');
                SearchEligibleEmployees();
            },
            error: function (request, status, error) {
                $.alert.open("Message", "Error in Process..");
            }
        });
    } else {        
        $('#txtNextPromotionReviewDate').addClass('errorClass');
        $('#txtRemarks').addClass('errorClass');
    }
}

