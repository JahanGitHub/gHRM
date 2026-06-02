
var componentConstants = {
    SalaryDeposit: 'Salary Deposit',
    SalaryDepositRefund: 'Salary Deposit Refund'
};

var transactionConstants = {
    Addition: 'Cr',
    Deduction: 'Dr'
};

var componentGroupConstants = {
    Salary: 1,
    SalaryDeduction: 6
};

var prDepositManager = {

    Clearform: function () {
        $("#PRComponentId").val('');
        $("#ComponentGroupId").val('');
        $("#EmployeeType").val('');
        $("#EmployeeStatusId").val('');
        $("#IsDepositRequired").val('');
        $("#DepositeType").val('');
        $("#ReturnDepositeOnEmployeeStatusId").val('');
        $("#TransactionType").val('');
        $("#ComponentGroup").val('');
        $("#MaximumLimit").val('');
        $("#MinimumLimit").val('');
        $("#NoOfSalaryDays").val('');
        $("#EffectiveStartDate").val('');
        $("#EffectiveEndDate").val('');
    },

    GetPRDepositInfo: function () { 
        var filterColumn = $("#filterColumn").val();
        var filterValue = $("#filterValue").val();
        if (filterColumn != "" && filterValue == "") {
            $.alert.open("Error", "Please Provide Filter Value");
            return false;
        }

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
                    url: '/PRDeposit/GetPRDepositInfo',
                    dataType: 'json',
                    data: { FilterColumn: filterColumn, FilterValue: filterValue }
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
                     field: "PRComponentId",
                     hidden: true,
                     filterable: false
                 },
                {
                    field: "OfficeLocationName",
                    title: "Office Location",
                    width: "120px",
                    filterable: true,
                    locked: true
                },
                 {
                     field: "ComponentName",
                     title: "Component",
                     width: "120px",
                     filterable: true,
                     locked: true
                 },
                 {
                     width: "140px",
                     field: "ComponentGroup",
                     filterable: true,
                     title: "Component Group",
                     locked: true
                 },
                 {
                     width: "140px",
                     field: "EmployeeTypeName",
                     title: "Employee Type",
                     locked: true
                 },
                 {
                     width: "140px",
                     field: "EmployeeStatusName",
                     title: "Employee Status",
                     locked: true
                 },
                 {
                     width: "140px",
                     field: "IsDepositRequired",
                     title: "Is Deposit Required",
                     template: function (data) {
                         if (data.IsDepositRequired == '1') {
                             return "Yes";
                         } else {
                             return "No";
                         }
                     },
                     //locked: true
                 },
                 {
                     width: "120px",
                     field: "DepositeType",
                     title: "Deposit Type",
                     //locked: true
                 },
                 {
                     width: "110px",
                     field: "NoOfSalaryDays",
                     title: "No. Salary Days",
                     //locked: true
                 },

                 {
                     width: "160px",
                     field: "EffectiveDate",
                     title: "Effective Date",
                     //locked: true
                 },

                 {
                     width: "50px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";
                         btn += '<div class="text-center"><a href="#" OnClick="prDepositManager.DeleteGrid(' + data.Id + ',' + data.PRComponentId + ',' + "'" + data.EmployeeType + "'" + ',' + "'" + data.EmployeeStatusId + "'" + ');"><i class="fa fa-trash-o"></i></a></div>';
                         return btn;
                     }                     
                 },
            ]
        });     

        //remove kendo unwanted height
        prDepositManager.remoteKendoUnwantedHeight();
       
    },
    initDatePicker: function () {
        $("#EffectiveStartDate").datepicker(
      {
          dateFormat: "dd-M-yy",
          showAnim: "scale",
          changeMonth: true,
          changeYear: true,
          yearRange: "1920:2050"

      });

        $("#EffectiveEndDate").datepicker(
          {
              dateFormat: "dd-M-yy",
              showAnim: "scale",
              changeMonth: true,
              changeYear: true,
              yearRange: "1920:2050"
          });
    },

    DeleteGrid: function (Id, PRComponentId, EmployeeType, EmployeeStatusId) {
        var obj = {
            Id:Id,
            PRComponentId: PRComponentId,           
            EmployeeType: EmployeeType,
            EmployeeStatusId: EmployeeStatusId
        }
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    url: '/PRDeposit/DeletePRDepositInfo',
                    data: JSON.stringify({ obj: obj }),
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.result == 1) {
                            $.alert.open("Success", data.message);
                            prDepositManager.GetPRDepositInfo();
                        } else {
                            $.alert.open("Error", data.message);
                        }
                    },
                });
                return true;
            }
            else {
                hiddenField.value = 'false';
                return false;
            }
        });
    },

    defaultSetup: function () {
        $("#btnUpdate").hide();
        $("#btnReset").hide();
        $(".hideLimit").hide();
        $("#IsDepositRequired").val('1').trigger('change');
        $("#DepositeAmount").val('0');
        $("#DepositeAmount").attr('Disabled', true);
        $("#MaximumLimit").val('0');
        $("#MaximumLimit").attr('Disabled', true);
        $("#MinimumLimit").val('0');
        $("#MinimumLimit").attr('Disabled', true);
        $("#NoOfSalaryDays").val('0');
        $("#NoOfSalaryDays").removeAttr('Disabled');
    },

    remoteKendoUnwantedHeight:function(){
        //remove kendo unwanted height
        setTimeout(function () {
            $('.k-grid-content-locked').removeAttr('style');
            $(".k-grid-content-locked").css({ "width": "660px" });
        }, 300);
    }
};

$(document).ready(function () {
    //load listing
    prDepositManager.GetPRDepositInfo();

    //init datepicker
    prDepositManager.initDatePicker();

    //submit form
    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();
        var prComponentName = $("#PRComponentId option:selected").text();

        var noOfSalaryDays = $('#NoOfSalaryDays').val()
        $('#NoOfSalaryDays').removeClass("input-validation-error");
        $('#DepositeType').removeClass("input-validation-error");
        if (prComponentName == componentConstants.SalaryDeposit && (!noOfSalaryDays || noOfSalaryDays <= 0)) {
            $('#DepositeType').addClass("input-validation-error");
            $('#NoOfSalaryDays').addClass("input-validation-error");
            return;
        } 

        var form = $('#add-or-edit-form');

        var componentName = $("#PRComponentId option:selected").text();
        var componentGroupId = $("#ComponentGroupId").val();
        var componentGroup = $("#ComponentGroupId option:selected").text();
        var employeeStatusName = $("#EmployeeStatusId option:selected").text();
        var transactionType = $("#TransactionType").val();
        var depositeType = $("#DepositeType").val();        
        var officeLocationId = $("#OfficeLocationId").val();

        var form = $('#add-or-edit-form');

        //for form validation
        var isValid = app.validateForm('#add-or-edit-form');
        if (!isValid) return;

        var serializedForm = form.serialize() + '&componentName=' + componentName + '&componentGroup=' + componentGroup
                                + '&employeeStatusName=' + employeeStatusName + '&componentGroupId=' + componentGroupId
                                + '&transactionType=' + transactionType + '&depositeType=' + depositeType + '&officeLocationId=' + officeLocationId;

        var action = form.attr('action');
        $('#AjaxLoader').show();

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: serializedForm
        }).done(function (data) {
            $('#AjaxLoader').hide();
            if (data.result == 1) {
                $(".panel-body .create-success").show(800).fadeToggle(3000);
                $.alert.open('Success', data.message);
                prDepositManager.GetPRDepositInfo();
                //prDepositManager.Clearform();
            } else {
                $.alert.open('Error', data.message);
            }
        });
    });

    //default setup
    prDepositManager.defaultSetup();
    
    $("#PRComponentId").change(function () {
        var prComponent = $("#PRComponentId").val();
        var prComponentName = $("#PRComponentId option:selected").text();

        $('#TransactionType').val('');
        $('#ComponentGroupId').val('');
        $('#NoOfSalaryDays').val('');
        $('#IsDepositRequired').val('1');

        $('.no-of-salary-days').removeClass('required');
        $('#NoOfSalaryDays').removeAttr('disabled');

        $('.deposite-type').removeClass('required');
        $('#DepositeType').removeAttr('disabled');

        if (prComponentName == componentConstants.SalaryDeposit) {
            $('#TransactionType').val(transactionConstants.Deduction);
            $('#ComponentGroupId').val(componentGroupConstants.SalaryDeduction);
            $('.no-of-salary-days').addClass('required');

            $('#DepositeType').val('Salary');
            $('.deposite-type').addClass('required');
        }
        else if (prComponentName == componentConstants.SalaryDepositRefund) {
            $('#TransactionType').val(transactionConstants.Addition);
            $('#ComponentGroupId').val(componentGroupConstants.Salary);
            
            $('#NoOfSalaryDays').val('');
            $('#NoOfSalaryDays').attr('disabled','disabled');
            $('#NoOfSalaryDays').removeClass("input-validation-error");

            $('#DepositeType').val('');
            $('#DepositeType').attr('disabled', 'disabled');
            $('#DepositeType').removeClass("input-validation-error");

            $('#IsDepositRequired').val('0');
        }
    });

    $("#EffectiveStartDate").val('');
    $("#EffectiveEndDate").val('');

 
    //resolving kendo grid ui loading issue
    $('.k-link').on('click', function () {
        prDepositManager.remoteKendoUnwantedHeight();
    });
   
    var grid = $("#grid").data("kendoGrid");
    grid.bind("filter", function (e) {
        if (e.filter != null) {
            //remove kendo unwanted height
            prDepositManager.remoteKendoUnwantedHeight();
        }
    });
});