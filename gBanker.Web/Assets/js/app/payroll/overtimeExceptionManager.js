
$(document).ready(function () {
    $('#EffectiveStartDate').val('');
    $('#EffectiveEndDate').val('');
    overtimeExceptionManager.loadOvertimeExceptionConfigListing();
    $('#EmployeeCode').blur(function () {
        //overtimeExceptionManager.clearform();
        var employeeCode = $('#EmployeeCode').val();
        if (employeeCode == '') {
            return;
        }
        var employeeId = overtimeExceptionManager.getEmployeeByEmployeeCode(employeeCode);
        if (employeeId == '' || employeeId == undefined) {
            alert('Does not exist');
            $('#EmployeeCode').focus();
            return;
        }
    });
    $('#EffectiveStartDate').change(function () {
        var startDate = $('#EffectiveStartDate').val();
        var endDate = $('#EffectiveEndDate').val();
        if (new Date(startDate) > new Date(endDate))
            $('#EffectiveStartDate').val('')
        return false;
    });

    $('#EffectiveEndDate').change(function () {
        var startDate = $('#EffectiveStartDate').val();
        var endDate = $('#EffectiveEndDate').val();
        if (new Date(startDate) > new Date(endDate))
            $('#EffectiveEndDate').val('');
        return false;
    });

    $('#add-or-edit-form').on('submit', function (event) {
        event.preventDefault();
        var form = $('#add-or-edit-form');
        var Id = $('#Id').val();
        var action = Id && Id > 0 ?
            "/OvertimeException/UpdateOvertimeConfiguration" : form.attr('action');

        $.ajax({
            type: form.attr('method'),
            url: action,
            data: form.serialize()
        }).done(function (response) {
            if (response.type == 'success') {
                //get listing
                overtimeExceptionManager.loadOvertimeExceptionConfigListing();
                //success alert
                $.alert.open("Success", response.message);
                //form clear
                overtimeExceptionManager.clearform();
            }
            else {
                $.alert.open("Error", response.message);
            }
        });

    });

});
var overtimeExceptionManager = {
    clearform: function () {
        $("#EmployeeCode").val("");
        $("#EmployeeName").val("");
        $("#ExceptionType").val("");
        $("#EffectiveStartDate").val("");
        $("#EffectiveEndDate").val("");
        $("#btnSave").text('Save');
        $("#Id").val(0);
    },

    clearOnEmployeeChange: function () {
        $('#EmployeeId').val('');
        $('#EmployeeName').val('');        
    },

    getEmployeeByEmployeeCode: function (employeeCode) {
        var employeeId = '';
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/Employee/GetEmployeeByEmployeeCode',
            data: { employeeCode: employeeCode },
            dataType: 'json',
            async: false,
            success: function (data) {
                $('#EmployeeId').val(data.EmployeeId);
                $('#EmployeeName').val(data.EmployeeName);
                employeeId = data.EmployeeId;
            },
            error: function (request, status, error) {
            }
        });
        return employeeId;
    },

    loadOvertimeExceptionConfigListing: function () {
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
                    url: '/OvertimeException/GetOvertimeExceptionConfigListing',
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
                    field: "EmployeeId",
                    hidden: true,
                    filterable: false
                },
                {
                    field: "EmployeeCode",
                    title: "Employee Code",
                    width: "100px",
                    filterable: true
                },
                {
                    field: "EmployeeName",
                    title: "Employee Name",
                    width: "100px",
                    filterable: true
                },
                {
                    field: "ExceptionType",
                    title: "Exception Type",
                    width: "100px",
                    filterable: true
                },
                {
                    field: "EffectiveStartDate",
                    title: "Effective Start Date",
                    width: "100px",
                    filterable: true
                },                
                {
                    field: "EffectiveEndDate",
                    title: "Effective End Date",
                    width: "100px",
                    filterable: true
                },
                {
                    width: "30px",
                    title: 'Action',
                    template: function (data) {
                        var btn = "";
                        btn += '<div class="text-center" style="float:left;"><a href="#" OnClick="overtimeExceptionManager.populateEditableInfo(' + data.Id + ');"><i class="fa fa-pencil-square-o"></i></a></div>';
                        btn += '<div class="text-center"><a href="#" OnClick="overtimeExceptionManager.informationDelete(' + data.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
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
            overtimeExceptionManager.clearform();
            return;
        }
        $.ajax({
            url: '/OvertimeException/GetOvertimeExceptionList/' + id,
            method: 'GET',
            cache: false,
            dataType: 'json'
        }).done(function (result) {
            if (!result.isSuccess) {
                overtimeExceptionManager.clearform();
                return;
            }
            $("#Id").val(result.data.Id);
            $("#EmployeeCode").val(result.data.EmployeeCode).trigger('blur');            
            $("#ExceptionType").val(result.data.ExceptionType);
            $("#EffectiveStartDate").val(result.data.EffectiveStartDate);
            $("#EffectiveEndDate").val(result.data.EffectiveEndDate);            
        });
    },

    informationDelete: function (id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/OvertimeException/Delete',
                    data: { id: id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.type == 'success') {
                            //get listing
                            overtimeExceptionManager.loadOvertimeExceptionConfigListing();
                            //success alert
                            $.alert.open("Success", data.message);
                            //form clear
                            overtimeExceptionManager.clearform();
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
