
var employeeTranningDropDownManager = {
    clearform: function () { 
        $("#EmployeeTrainingDropDownName").val("");
        $(".input-validation-error").removeClass("input-validation-error");
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/EmployeeTranningDropDown/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            employeeTranningDropDownManager.loadEmployeeTranningDropDownListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            staffWelfareFundSettingManager.clearform();
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
            employeeTranningDropDownManager.clearform();
            return;
        }

        $.ajax({
            url: '/EmployeeTranningDropDown/GetEmployeeTranningDropDown/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {

            if (!result.isSuccess) {
                employeeTranningDropDownManager.clearform();
                return;
            }

            $("#EmployeeTrainingDropDownName").val(result.data.EmployeeTrainingDropDownName);
            $("#IsPercentage").prop('checked', result.data.IsPercentage);
        });       
    },

    loadEmployeeTranningDropDownListing: function () {
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
                    url: '/EmployeeTranningDropDown/GetEmployeeTranningDropDownListing',
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
                     field: "EmployeeTrainingDropDownId",
                     title: "Training Title Id",
                     width: "10px",
                     hidden: false,
                     filterable: false
                 },
                 {
                     field: "EmployeeTrainingDropDownName",
                     title: "Employee Training Title",
                     width: "50px",
                     filterable: true,
                 },
                 {
                     width: "20px",
                     field: "IsActive",
                     filterable: true,
                     title: "Is Active",
                     template: function (dataItem) {
                         return (dataItem.IsActive == true) ?
                             "Active" : "Inactive"
                     }
                 },
                 {
                     width: "30px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";
                         btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="employeeTranningDropDownManager.populateEditableInfo(' + data.EmployeeTrainingDropDownId + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                         btn += '<div class="text-center"><a href="#" OnClick="employeeTranningDropDownManager.informationDelete(' + data.EmployeeTrainingDropDownId + ');"><i class="fa fa-trash-o"></i></a></div>';
                         return btn;
                     }
                 },
            ]
        });
    }
    
}

$(document).ready(function () {
    //get listing
    employeeTranningDropDownManager.loadEmployeeTranningDropDownListing();

    //submit to change userrole
    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();
        var form = $('#add-or-edit-form');
        var EmployeeTrainingDropDownId = $('#EmployeeTrainingDropDownId').val();
        var action = EmployeeTrainingDropDownId && EmployeeTrainingDropDownId > 0 ?
            "/EmployeeTranningDropDown/Update" : form.attr('action');

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //get listing
                employeeTranningDropDownManager.loadEmployeeTranningDropDownListing();
                //success alert
                $.alert.open("Success", response.message);
                //form clear
                employeeTranningDropDownManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});
