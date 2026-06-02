
var staffWelfareFundConfigurationManager = {
    clearform: function () { 
        $("#Id").val("");
        $(".input-validation-error").removeClass("input-validation-error");
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to inactive this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/HealthFunding/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            staffWelfareFundConfigurationManager.loadStaffWelfareFundConfigListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            staffWelfareFundConfigurationManager.clearform();
                        } else {
                            $.alert.open("Error", data.message);
                        }
                    },
                    error: function (request, status, error) {
                        alert(request.statusText + "/" + request.statusText + "/" + error);
                    }
                });
                return true;
            }
            else {
                hiddenField.value = 'false';
                return false;
            }
        });
    },

    populateEditableInfo: function (id) {
        $(".input-validation-error").removeClass("input-validation-error");
        if (!id) {
            staffWelfareFundConfigurationManager.clearform();
            return;
        }

        $.ajax({
            url: '/HealthFunding/GetStaffWelfareFundConfiguration/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {

            if (!result.isSuccess) {
                staffWelfareFundConfigurationManager.clearform();
                return;
            }

            $("#Id").val(result.data.StaffWelfareFundSettingId);
           // $("#IsPercentage").prop('checked', result.data.IsPercentage);
        });       
    },
    
    loadStaffWelfareFundConfigListing: function () {
        debugger;
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
                    url: '/HealthFunding/GetHealthFundingListing',
                    dataType: 'json'
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
                    field: "EmpInfo",
                    title: "Employee",
                    width: "50px",
                    filterable: true
                },               
                {
                    field: "purposename",
                    title: "Purpose",
                    width: "50px",
                    filterable: true
                },
                {
                    field: "FundAmount",
                    title: "Fund Amount",
                    width: "50px",
                    filterable: true
                },
                {
                    width: "50px",
                    field: "IsActive",
                    filterable: true,
                    title: "Is Active",
                    template: function (dataItem) {
                        return (dataItem.IsActive == true) ?
                            "Active" : "Inactive"
                    }
                },
                {
                    width: "50px",
                    field: "CreateDateString",
                    filterable: true,
                    title: "Created On"
                },
                {
                    width: "70px",
                    title: 'Action',
                    template: function (data) {
                        var btn = "";
                       // btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="staffWelfareFundSettingManager.populateEditableInfo(' + data.Id + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                        btn += '<div class="text-center"><a href="#" OnClick="staffWelfareFundConfigurationManager.informationDelete(' + data.Id + ');"><i class="fa fa-close"></i></a></div>';
                        return btn;
                    }
                },
            ]
        });
    }

}



function GetPRComponentList(ComponentCategory) {

        var ddlComponent = $("#PRComponentId");
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/PRSalaryAllowance/GetPRComponentListFund',
            data: {  ComponentCategory: ComponentCategory },
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


function GetPRComponentListFund() {

    var ddlComponent = $("#PurposeId");
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/HealthFunding/GetPRpurposeListFund',
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

function LoadEmpInfoByCode(employee_code) {
    $("#EmployeeId").val(0);
    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: '/EmployeeProfileReport/GetEmpInfoByCode',
        data: { employee_code: employee_code },
        dataType: 'json',
        async: false,
        success: function (data) {
            debugger;
            console.log(data.length);
            console.log(data.result);

            if (data.result == 1) {
                $.each(data.data, function (index, data) { 
                    $("#EmployeeName").val(data.EmployeeName);
                    $('#EmployeeId').val(data.EmployeeId);
                });
                //LoadPreviousDataList($("#EmployeeId").val());
            }

            else {
                //ClearControl();
                $("#EmployeeCode").val('');
                $("#EmployeeName").val('');
                $('#EmployeeId').val('0');
                $.alert.open("Error", "Invalid code");
            }
        },
    });
}



$(document).ready(function () {

    $("#EmployeeCode").blur(function (e) {
        debugger;
        var employeeCode = $("#EmployeeCode").val();
        if (employeeCode != '') {
            LoadEmpInfoByCode(employeeCode);
        }
    });
    staffWelfareFundConfigurationManager.loadStaffWelfareFundConfigListing();
    GetPRComponentList("Deduction");
    GetPRComponentListFund();
    //get listing
    $("#DeductionAmount").val("");    

    //submit to change userrole
    $('#add-or-edit-form').on('submit', function (event) {
       
        event.preventDefault();
        var form = $(this);

        //for form validation
        var isValid = app.validateForm('#add-or-edit-form');
        if (!isValid) return;
        
        $("#AjaxLoader").show();
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') { 
                //success alert
                $("#AjaxLoader").hide();
                $.alert.open("Success", "Successfully Added Employee Fund.");

                staffWelfareFundConfigurationManager.loadStaffWelfareFundConfigListing();
                //form clear
                staffWelfareFundConfigurationManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});
