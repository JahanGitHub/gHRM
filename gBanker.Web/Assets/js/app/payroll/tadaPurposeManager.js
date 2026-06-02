
$(document).ready(function () {
    tadaPurposeManager.loadtadaPurposeListing();

    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();
        var form = $('#add-or-edit-form');
        var id = $('#Id').val();
        var action = id && id > 0 ?
            "/TADAPurpose/UpdateTADAPurpose" : form.attr('action');

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //get listing
                tadaPurposeManager.loadtadaPurposeListing();
                //success alert
                $.alert.open("Success", response.message);
                //form clear
                tadaPurposeManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });

});
var tadaPurposeManager = {
    clearform: function () {
        $("#Purpose").val("");
        $("#Remarks").val("");
        $("#btnSave").text('Save');
        $("#Id").val(0);
    },

    loadtadaPurposeListing: function () {
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
                    url: '/TADAPurpose/GetTADAPurposeListing',
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
                    field: "Purpose",
                    title: "Purpose",
                    width: "100px",
                    filterable: true
                },
                {
                    field: "Remarks",
                    title: "Remarks",
                    width: "100px",
                    filterable: true
                },
                {
                    width: "30px",
                    title: 'Action',
                    template: function (data) {
                        var btn = "";
                        btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="tadaPurposeManager.populateEditableInfo(' + data.Id + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                        btn += '<div class="text-center"><a href="#" OnClick="tadaPurposeManager.informationDelete(' + data.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                        return btn;
                    }
                },
            ]
        });
    },

    populateEditableInfo: function (id) {
        $("#btnSave").text('Update');
        $(".input-validation-error").removeClass("input-validation-error");
        if (!id) {
            tadaPurposeManager.clearform();
            return;
        }
        $.ajax({
            url: '/TADAPurpose/GetTADAPurposeList/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {
            if (!result.isSuccess) {
                tadaPurposeManager.clearform();
                return;
            }
            $("#Id").val(result.data.Id);
            $("#Purpose").val(result.data.Purpose);
            $("#Remarks").val(result.data.Remarks);
        });   
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/TADAPurpose/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            tadaPurposeManager.loadtadaPurposeListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            tadaPurposeManager.clearform();
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
}