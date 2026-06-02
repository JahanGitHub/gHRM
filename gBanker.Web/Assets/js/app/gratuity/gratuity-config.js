var Page = {
    Load: function () {
        GridTable.Load();
    },
    Search: function () {
        GridTable.Load();
    }
};

var GridTable = {
    Load: function () {
        var EmployeeStatusId = $("#EmployeeStatus").val();
        EmployeeStatusId = [null, ""].includes(EmployeeStatusId) ? 0 : EmployeeStatusId;
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
                    url: '/GratuityConfig/LoadGratuityConfigList',
                    dataType: 'json',
                    data: { EmployeeStatusId: EmployeeStatusId }
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
            columns: [
                {
                    field: "Id",
                    hidden: true,
                    filterable: false
                },
                {
                    width: "120px",
                    field: "StatusName",
                    title: "Eligible Employee Type"
                },
                {
                    width: "80px",
                    field: "ServiceAgeFrom",
                    title: "Service Age From (Year)"
                },
                {
                    width: "80px",
                    field: "ServiceAgeTo",
                    title: "Service Age To (Year)"
                },
                {
                    width: "80px",
                    field: "GratuityTimes",
                    title: "Gratuity Basic"
                },
                {
                    title: "Eligible From",
                    width: "50px",
                    template: function (data) {
                        return "C" == data.EligibleFrom ? "Confirmation Date" : "Joining Date";
                    }
                },
                {
                    width: "50px",
                    field: "EffectiveStartDate",
                    title: "Effective Date"
                },
                {
                    width: "60px",
                    field: "EffectiveEndDate",
                    title: "Effective End Date"
                },
                {
                    title: "Delete",
                    width: "50px",
                    template: function (data) {
                        return '<div class="text-center delete-link"><a href="#" onclick="GridTable.Delete(' + data.Id + ', event)"><i class="fa fa-trash-o"></i></a></div>';
                    }
                }
            ]
        });
    },
    Reload: function () {
        $('#grid').data('kendoGrid').dataSource.read();
        $('#grid').data('kendoGrid').refresh();
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

var Req = {
    POST: {
        Delete: function (Data, callback, err_callback) {
            $.ajax({
                url: '/GratuityConfig/Delete',
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