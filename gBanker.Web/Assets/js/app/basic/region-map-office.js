var Page = {
    IsNumber: function (Value) {
        return ![null, ""].includes(Value) && !isNaN(Value);
    },
    Load: function () {
        GridTable.Load();
    }
};

var RegionForm = {
    EditId: 0,
    Create: function (e) {
        e.preventDefault();
        RegionForm.EditId = 0;
    },
    Save: function () {
        $("#btnSave").prop("disabled", true);
        var OfficeId = $("#Office").val();
        OfficeId = Page.IsNumber(OfficeId) ? OfficeId : 0;
        var fdata = new FormData();
        var token = $("input[name='__RequestVerificationToken']").val();
        fdata.append("__RequestVerificationToken", token);
        fdata.append("Data", JSON.stringify({
            RegionId: RegionId,
            OfficeId: OfficeId
        }));
        Req.POST.Save(fdata, function () {
            $.alert.open("Success", "Data Saved successfully!");
            $("#btnSave").prop("disabled", false);
            GridTable.Reload();
        }, function () {
            $("#btnSave").prop("disabled", false);
        });
    },
    Delete: function (Id, e) {
        e.preventDefault();
        $.alert.open('confirm', 'Are you sure you want to delete this record ?', function (button) {
            if (button == 'yes') {
                var fdata = new FormData();
                var token = $("input[name='__RequestVerificationToken']").val();
                fdata.append("__RequestVerificationToken", token);
                fdata.append("Id", Id);
                Req.POST.Delete(fdata, function () {
                    $.alert.open("Success", "Data Deleted successfully!");
                    GridTable.Reload();
                });
                return true;
            }
            else {
                return false;
            }
        });
    }
};

var GridTable = {
    Load: function () {
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
                    url: '/Region/LoadRegionOfficeList',
                    dataType: 'json',
                    data: { RegionId: RegionId }
                }
            }
        });
        $("#grid").kendoGrid({
            dataSource: dataSource,
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
            noRecords: {
                template: "<span style='padding: 10px;display: block;'>No data available</span>"
            },
            columns: [
                {
                    field: "Id",
                    hidden: true,
                    filterable: false
                },
                {
                    width: "120px",
                    field: "Name",
                    title: "Office Name"
                },
                {
                    width: "50px",
                    template: function (data) {
                        return '<div class="text-center"><a href="#" title="Delete" onclick="RegionForm.Delete(' + data.Id + ', event)" style="padding: 0 5px;"><i class="fa fa-trash-o"></i></a></div>';
                    }
                }
            ]
        });
    },
    Reload: function () {
        $('#grid').data('kendoGrid').dataSource.read();
        $('#grid').data('kendoGrid').refresh();
    }
};

var Req = {
    POST: {
        Save: function (Data, callback, err_callback) {
            $.ajax({
                url: '/Region/SaveMapOffice',
                type: 'Post',
                data: Data,
                async: true,
                contentType: false,
                processData: false,
                success: function (response) {

                    if ("Error" == response.Result) {
                        $.alert.open("Error", response.Message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("Error", data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
                    if (null != err_callback) { err_callback(); }
                }
            });
        },
        Delete: function (Data, callback, err_callback) {
            $.ajax({
                url: '/Region/DeleteMapOffice',
                type: 'Post',
                data: Data,
                async: true,
                contentType: false,
                processData: false,
                success: function (response) {

                    if ("Error" == response.Result) {
                        $.alert.open("Error", response.Message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("Error", data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
                    if (null != err_callback) { err_callback(); }
                }
            });
        }
    }
};

$(function () {
    Page.Load();
});