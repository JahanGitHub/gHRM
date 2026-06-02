
var healthWelfareFundConfigurationManager = {
    clearform: function () {
        $("#EmployeeCode").val("");
        $("#CollectionAmount").val("");
        $("#HealthWelfareFundSettingId").val("");
        $(".input-validation-error").removeClass("input-validation-error");
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/HealthWelfareFundConfiguration/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            healthWelfareFundConfigurationManager.loadhealthWelfareFundConfigurationListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            healthWelfareFundConfigurationManager.clearform();
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
            healthWelfareFundConfigurationManager.clearform();
            return;
        }

        $.ajax({
            url: '/HealthWelfareFundConfiguration/GetHealthWelfareFundConfiguration/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {

            if (!result.isSuccess) {
                healthWelfareFundConfigurationManager.clearform();
                return;
            }

            $("#EmployeeCode").val(result.data.EmployeeCode);
            $("#CollectionAmount").val(result.data.CollectionAmount);
            $("#HealthWelfareFundSettingId").val(result.data.HealthWelfareFundSettingId);
        });
    },

    loadhealthWelfareFundConfigurationListing: function () {
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
                    url: '/HealthWelfareFundConfiguration/GetHealthWelfareFundConfigurationListing',
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
                     field: "HealthWelfareFundConfigurationId",
                     hidden: true,
                     filterable: false
                 },
                 {
                     field: "EmployeeId",
                     title: "EmployeeId",
                     width: "50px",
                     filterable: true,
                 },
                 {
                     field: "Remark",
                     title: "Remark",
                     filterable: false,
                     width: "50px",
                     template: function (dataItem) {
                         if (dataItem.IsAmountPaid == true) {
                             return '<div class="text-center"><label>' + dataItem.Remark + '</label></div>';
                         } else {
                             return '<div class="text-center"><input type="text" id="txtRemarks' + dataItem.TADABillId + '"></input></div>';
                         }
                     }
                 },
                {
                    field: "Approve",
                    title: "Approve",
                    filterable: false,
                    width: "50px",
                    template: function (dataItem) {
                        if (dataItem.IsAmountPaid != true) {
                            return '<div class="text-center"><a title="Provide TADA Bill" href="#" OnClick="ProvideTADABill(' + dataItem.TADABillId + ');"><i class="fa fa-check"></i></a></div>';
                        }
                        else {
                            return "";
                        }
                    }
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
                     width: "30px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";
                         btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="healthWelfareFundConfigurationManager.populateEditableInfo(' + data.HealthWelfareFundConfigurationId + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                         btn += '<div class="text-center"><a href="#" OnClick="healthWelfareFundConfigurationManager.informationDelete(' + data.HealthWelfareFundConfigurationId + ');"><i class="fa fa-trash-o"></i></a></div>';
                         return btn;
                     }
                 },
            ]
        });
    }

}


$(document).ready(function () {
    //get listing
    $("#CollectionAmount").val("");

    $("#StaffEmployeeId").change(function () {
        $("$EmpId").val();
    });

    //submit to change userrole
    $('#add-or-edit-form').on('submit', function (event) {
        debugger;
        event.preventDefault();
        var form = $(this);

        //for form validation
        var isValid = app.validateForm('#add-or-edit-form');
        if (!isValid) return;

        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //success alert
                $.alert.open("Success", "Successfully Configured Health Welfare Fund Configuration.");
                //form clear
                staffWelfareFundConfigurationManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});