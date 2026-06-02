
var salaryDateConfigManager = {
    clearform: function () {
        $("#DayOfMonthlySalary").val("");
        $("#IsCurrentlyUsing").prop('checked', false);
        $(".input-validation-error").removeClass("input-validation-error");
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/SalaryDateConfig/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            salaryDateConfigManager.loadSalaryDateConfigListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            salaryDateConfigManager.clearform();
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
            salaryDateConfigManager.clearform();
            return;
        }

        $.ajax({
            url: '/SalaryDateConfig/GetSalaryDateConfig/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {

            if (!result.isSuccess) {
                salaryDateConfigManager.clearform();
                return;
            }

            $("#Id").val(result.data.Id);
            $("#DayOfMonthlySalary").val(result.data.DayOfMonthlySalary);
            $("#IsCurrentlyUsing").prop('checked', result.data.IsCurrentlyUsing);
        });
    },

    loadSalaryDateConfigListing: function () {        
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
                    url: '/SalaryDateConfig/GetSalaryDateConfigListing',
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
                     field: "DayOfMonthlySalary",
                     title: "Day Of Monthly Salary",
                     width: "100px",
                     filterable: true
                 },
                 {
                     width: "55px",
                     field: "IsCurrentlyUsing",
                     filterable: true,
                     title: "Is Current",
                     template: function (dataItem) {
                         return (dataItem.IsCurrentlyUsing == true) ?
                             "<h5><span class='label label-success'> Yes </label></h5>" : "<h5><span class='label label-warning'> No </label></h5>"
                     }
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
                         btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="salaryDateConfigManager.populateEditableInfo(' + data.Id + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                         btn += '<div class="text-center"><a href="#" OnClick="salaryDateConfigManager.informationDelete(' + data.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                         return btn;
                     }
                 },
            ]
        });
    }

}

$(document).ready(function () {
    //get listing
    $("#DayOfMonthlySalary").val("");
    salaryDateConfigManager.loadSalaryDateConfigListing();

    //submit to change userrole
    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();
        var form = $('#add-or-edit-form');
        var Id = $('#Id').val();
        var action = Id && Id > 0 ?
            "/SalaryDateConfig/Update" : form.attr('action');

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //get listing
                salaryDateConfigManager.loadSalaryDateConfigListing();
                //success alert
                $.alert.open("Success", response.message);
                //form clear
                salaryDateConfigManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});
