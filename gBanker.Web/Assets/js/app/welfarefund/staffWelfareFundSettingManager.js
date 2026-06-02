
var staffWelfareFundSettingManager = {
    clearform: function () { 
        $("#DeductionAmount").val("");
        $("#IsPercentage").prop('checked', false);
        $(".input-validation-error").removeClass("input-validation-error");
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to inactive this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/StaffWelfareFundSetting/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            staffWelfareFundSettingManager.loadStaffWelfareFundSettingListing();
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
            staffWelfareFundSettingManager.clearform();
            return;
        }

        $.ajax({
            url: '/StaffWelfareFundSetting/GetStaffWelfareFundSetting/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {

            if (!result.isSuccess) {
                staffWelfareFundSettingManager.clearform();
                return;
            }

            
            $("#StaffWelfareFundSettingId").val(result.data.StaffWelfareFundSettingId);
            $("#DeductionAmount").val(result.data.DeductionAmount);
            $("#IsPercentage").prop('checked', result.data.IsPercentage);
        });       
    },

    loadStaffWelfareFundSettingListing: function () {
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
                    url: '/StaffWelfareFundSetting/GetStaffWelfareFundSettingListing',
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
                     field: "StaffWelfareFundSettingId",
                     hidden: true,
                     filterable: false
                 },
                 {
                     field: "FundType",
                     title: "Fund Type",
                     width: "100px",
                     filterable: true
                },
                {
                    field: "ComponentType",
                    title: "Component Type",
                    width: "100px",
                    filterable: true
                },
                {
                    field: "ComponentAmount",
                    title: "Component Amount",
                    width: "100px",
                    filterable: true
                },
                {
                    field: "RatioBasedOn",
                    title: "Ratio BasedOn",
                    width: "100px",
                    filterable: true
                },
                {
                    field: "ComponentName",
                    title: "Component Name",
                    width: "50px",
                    filterable: true
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
                     width: "100px",
                     field: "CreateDateString",
                     filterable: true,
                     title: "Created On"
                 },
                 {
                     width: "50px",
                     title: 'Action',
                     template: function (data) {
                         var btn = "";                        
                         btn += '<div class="text-center"><a href="#" OnClick="staffWelfareFundSettingManager.informationDelete(' + data.StaffWelfareFundSettingId + ');"><i class="fa fa-ban"></i></a></div>';
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
        data: { ComponentCategory: ComponentCategory },
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

$(document).ready(function () {
    //get listing  
    staffWelfareFundSettingManager.loadStaffWelfareFundSettingListing();
    GetPRComponentList("Deduction");
    //submit to change userrole
    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();
        var form = $('#add-or-edit-form');
        var staffWelfareFundSettingId = $('#StaffWelfareFundSettingId').val();
        var action = staffWelfareFundSettingId && staffWelfareFundSettingId > 0 ?
            "/StaffWelfareFundSetting/Update" : form.attr('action');

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //get listing
                staffWelfareFundSettingManager.loadStaffWelfareFundSettingListing();
                //success alert
                $.alert.open("Success", response.message);
                //form clear
                staffWelfareFundSettingManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });
});
