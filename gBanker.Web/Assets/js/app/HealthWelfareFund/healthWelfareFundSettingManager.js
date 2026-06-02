
var healthWelfareFundSettingManager = {
    clearform: function () {
        $("#DeductionAmount").val("");
        $("#IsPercentage").prop('checked', false);
        $(".input-validation-error").removeClass("input-validation-error");
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/HealthWelfareFundSetting/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            healthWelfareFundSettingManager.loadHealthWelfareFundSettingListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            healthWelfareFundSettingManager.clearform();
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
            healthWelfareFundSettingManager.clearform();
            return;
        }

        $.ajax({
            url: '/HealthWelfareFundSetting/GetHealthWelfareFundSetting/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {
            if (!result.isSuccess) {
                healthWelfareFundSettingManager.clearform();
                return;
            }
            $("#HealthWelfareFundSettingId").val(result.data.HealthWelfareFundSettingId);
            $("#DeductionAmount").val(result.data.DeductionAmount);
            $("#IsPercentage").prop('checked', result.data.IsPercentage);
        });
    },

    loadHealthWelfareFundSettingListing: function () {
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
                    url: '/HealthWelfareFundSetting/GetHealthWelfareFundSettingListing',
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
                     field: "HealthWelfareFundSettingId",
                     hidden: true,
                     filterable: false
                 },
                 {
                     field: "DeductionAmount",
                     title: "Deduction Amount",
                     width: "50px",
                     filterable: true,
                     template: function (dataItem) {
                         return (dataItem.IsPercentage == true) ?
                             dataItem.DeductionAmount + " %" : dataItem.DeductionAmount + " TK"
                     }
                 },
                 {
                     width: "150px",
                     field: "IsActive",
                     filterable: true,
                     title: "Is Active",
                     template: function (dataItem) {
                         return (dataItem.IsActive == true) ?
                             "Active" : "Inactive"
                     }
                 },
                 {
                     width: "150px",
                     field: "CreateDateInString",
                     filterable: true,
                     title: "Created On"
                 },
                 {
                     width: "30px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";
                         btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="healthWelfareFundSettingManager.populateEditableInfo(' + data.HealthWelfareFundSettingId + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                         btn += '<div class="text-center"><a href="#" OnClick="healthWelfareFundSettingManager.informationDelete(' + data.HealthWelfareFundSettingId + ');"><i class="fa fa-trash-o"></i></a></div>';
                         return btn;
                     }
                 },
            ]
        });
    }

}

$(document).ready(function () {
    //get listing
    $("#DeductionAmount").val("");
    healthWelfareFundSettingManager.loadHealthWelfareFundSettingListing();

    //submit to add/edit
    $('#add-or-edit-form').on('submit', function (event) {
        debugger;
        event.preventDefault();

        var form = $(this);

        //for form validation
        var isValid = app.validateForm('#add-or-edit-form');
        if (!isValid) return;

        var healthWelfareFundSettingId = $('#HealthWelfareFundSettingId').val();
        var action = healthWelfareFundSettingId && healthWelfareFundSettingId > 0 ?
            "/HealthWelfareFundSetting/Update" : form.attr('action');

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //get listing
                healthWelfareFundSettingManager.loadHealthWelfareFundSettingListing();
                //success alert
                $.alert.open("Success", response.message);
                //form clear
                healthWelfareFundSettingManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});
