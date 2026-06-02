

function RoleDelete(Id) {
    $.alert.open('confirm', 'Are you sure you want to delete this Role?', function (button) {
        if (button == 'yes') {
            if (Id > 0) {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/Security/UserRoleDelete',
                    data: { Id: Id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.result === 1) {
                            userRoleManager.GetUserRoles();
                            $.alert.open("Success", data.message);
                        } else {
                            $.alert.open("Error", data.message);
                        }
                    },
                    error: function (request, status, error) {
                        alert(request.statusText + "/" + request.statusText + "/" + error);
                    }
                });
            }
        }
    });
}


var userRoleManager = {
    GetUserRoles: function () {
        $('#gridKendo').html("");
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
                    //url: baseURL + '/ProcessCategory/GetProcessCategoryList?QueryOption=' + 1,
                    url: '/Security/GetUserRoles',
                    dataType: 'json',
                    data: {}
                }
            }
        });
        $("#gridKendo").kendoGrid({
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
                    field: "rowSl",
                    title: "SL",
                    width: "20px",
                    filterable: false,
                },//OfficeTypeName
                {
                    field: "Name",
                    title: "Role Name",
                    width: "50px",
                    filterable: true,
                },
                //{
                //    title: "Delete",
                //    width: "25px",
                //    template: function (dataItem) {
                //        if (dataItem.OfficeTypeId !== 1 && dataItem.Name !== "Super Admin") {
                //            return "<a href='#' OnClick='RoleDelete(" + dataItem.Id + ");'><i class='fa fa-trash-o'></i></a>";
                //        } else {
                //            return "<span class='label label-danger'>Read Only</span>";
                //        }
                //    }
                //}
            ]
        });
    }
}

$(document).ready(function () {
    userRoleManager.GetUserRoles();

    $("#user-role-submit-form").on("submit", function (event) {
        event.preventDefault();

        var _currentForm = $(this).closest('form');
        if (_currentForm.valid()) {
            $("#loading").show();
            var url = $(this).attr("action");
            var formData = $(this).serialize();
            $.ajax({
                url: url,
                type: "POST",
                data: formData,
                dataType: "json",
                success: function (data) {
                    if (data.result === 1) {
                        $("#Name").val('');
                        userRoleManager.GetUserRoles();
                        $.alert.open("Success", data.message);
                    } else {
                        $.alert.open("Error", data.message);
                    }
                },
                error: function (err) {
                    alert(request.statusText + "/" + request.statusText + "/" + error);
                }
            })
        }
    });
});