
var companyWisePayrollConfigManager = {
    clearform: function () {
        $("#Id").val('');
        $("#CompanyCode").val('');
        $("#PayrollType").val('');
        $("#Description").val('');
        $(".input-validation-error").removeClass("input-validation-error");
        $("#PayrollType").removeAttr('disabled');
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/CompanyWisePayrollConfig/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            companyWisePayrollConfigManager.loadCompanyWisePayrollConfigListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            companyWisePayrollConfigManager.clearform();
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
            companyWisePayrollConfigManager.clearform();
            return;
        }

        $.ajax({
            url: '/CompanyWisePayrollConfig/GetCompanyWisePayrollConfig/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {

            if (!result.isSuccess) {
                companyWisePayrollConfigManager.clearform();
                return;
            }

            $("#Id").val(result.data.Id);
            $("#CompanyCode").val(result.data.CompanyCode);
            $("#PayrollType").val(result.data.PayrollType);
            $("#Description").val(result.data.Description);

            $("#PayrollType").prop('disabled', 'disabled');
        });
    },

    loadCompanyWisePayrollConfigListing: function () {        
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
                    url: '/CompanyWisePayrollConfig/GetCompanyWisePayrollConfigListing',
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
                     field: "CompanyName",
                     title: "Company",
                     width: "100px",
                     filterable: true
                 },
                 {
                     field: "PayrollTypeInText",
                     title: "Payroll Type",
                     width: "100px",
                     filterable: true
                 },                
                 {
                     width: "55px",
                     field: "IsActive",
                     filterable: true,
                     title: "Is Active",
                     template: function (dataItem) {
                         return (dataItem.IsActive == true) ?
                             "Yes" : "No"
                     }
                 },
                 {
                     width: "50px",
                     field: "CreateDateInString",
                     filterable: true,
                     title: "Created On"
                 },
                 {
                     width: "30px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";
                         btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="companyWisePayrollConfigManager.populateEditableInfo(' + data.Id + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                         btn += '<div class="text-center"><a href="#" OnClick="companyWisePayrollConfigManager.informationDelete(' + data.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                         return btn;
                     }
                 },
            ]
        });
    }

}

$(document).ready(function () {
    //get listing   
    companyWisePayrollConfigManager.loadCompanyWisePayrollConfigListing();

    //submit to change userrole
    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();

        var form = $('#add-or-edit-form');
        
        //for form validation
        var isValid = app.validateForm('#add-or-edit-form');
        if (!isValid) return;

        var Id = $('#Id').val();
        var action = Id && Id > 0 ?
            "/CompanyWisePayrollConfig/Update" : form.attr('action');

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //get listing
                companyWisePayrollConfigManager.loadCompanyWisePayrollConfigListing();
                //success alert
                $.alert.open("Success", response.message);
                //form clear
                companyWisePayrollConfigManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});
