
var cooperativeConfigurationManager = {
    clearform: function () {
        $(".input-validation-error").removeClass("input-validation-error");
    },
    EditGrid: function (id, eid, cid, installment, startdate, eCode, eName) {
        $("#Id").val(id)
        $("#EmployeeId").val(eid)
        $("#ComponentId").val(cid)

        $("#EmployeeCode").val(eCode)
        $("#EmployeeName").val(eName)
        $("#MonthlyInstallment").val(installment)
        $("#StartDate").val(startdate)
    },
    InformationDelete: function (id) {
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: '/CooperativeConfiguration/InfoDelete',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            async: true,
            success: function (data) {
                $.alert.open(data.status, data.message);
                cooperativeConfigurationManager.Reload();
            }
        });
    },
    LoadGrid: function () {

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
                    url: '/CooperativeConfiguration/GetCooperativeConfigurationListing',
                    dataType: 'json',
                    data: {}//{ collectionYear: collectionYear, collectionMonth: collectionMonth }
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
                    title: "EmployeeId",
                    hidden: true,
                    filterable: false,
                },
                {
                    field: "ComponentId",
                    title: "ComponentId",
                    hidden: true,
                    filterable: false,
                },
                {
                    field: "EmployeeCode",
                    title: "Employee Code",
                    width: "50px",
                    filterable: true,
                },
                {
                    field: "EmployeeName",
                    title: "Employee Name",
                    width: "50px",
                    filterable: true,
                },
                {
                    field: "ComponentName",
                    title: "Component",
                    width: "50px",
                    filterable: false,
                },
                {
                    field: "StartDate",
                    title: "StartDate",
                    width: "50px",
                    filterable: false,
                },
                {
                    field: "MonthlyInstallment",
                    title: "Installment",
                    width: "50px",
                    filterable: true,
                },

                {
                    title: "Action",
                    filterable: false,
                    width: "30px",
                    template: function (dataItem) {
                        return  `<div class="text-center"><a href="#" OnClick="cooperativeConfigurationManager.EditGrid(${dataItem.Id},${dataItem.EmployeeId},${dataItem.ComponentId},${dataItem.MonthlyInstallment},'${dataItem.StartDate}','${dataItem.EmployeeCode}','${dataItem.EmployeeName}')"><i class="fa fa-pencil-square-o"></i></a> | <a href="#" OnClick="cooperativeConfigurationManager.InformationDelete(${dataItem.Id});"><i class="fa fa-trash-o"></i></a></div>`;
                    }
                },
            ]
        });
    },
    Reload: function () {
        $('#grid').data('kendoGrid').dataSource.read();
        $('#grid').data('kendoGrid').refresh();
    }
}

$(document).ready(function () {
    cooperativeConfigurationManager.LoadGrid();
    $("#StartDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true
    });

    $("#EmployeeCode").blur(function () {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/Employee/GetEmployeeInfoByEmployeeCode',
            data: { employeeCode: $(this).val() },
            dataType: 'json',
            async: true,
            success: function (data) {
                if (data.type == "warning") {
                    $.alert.open("Error", data.message);
                    $("#EmployeeName,#EmployeeCode,#EmployeeId").val('')
                } else {
                    $("#EmployeeName").val(data.employeeInfo.EmployeeName);
                    $("#EmployeeId").val(data.employeeInfo.EmployeeId);

                }
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    })
    $("#btnSave").click(function (e) {
        e.preventDefault();
        var obj = {
            Id: $("#Id").val(),
            ComponentId: $("#ComponentId").val(),
            EmployeeId: $("#EmployeeId").val(),
            EmployeeCode: $("#EmployeeCode").val(),
            MonthlyInstallment: $("#MonthlyInstallment").val(),
            StartDate: $("#StartDate").val(),
        };
        if (obj.ComponentId && obj.EmployeeId && obj.EmployeeId > 0 && obj.EmployeeCode && obj.MonthlyInstallment > 0 && obj.StartDate) {
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                url: '/CooperativeConfiguration/AddCooperativeConfiguration',
                data: JSON.stringify({ model: obj }),
                dataType: 'json',
                async: true,
                success: function (data) {
                    $.alert.open(data.status, data.message);
                    $("input[type='text'],[type='hidden']").val('')
                    cooperativeConfigurationManager.Reload();
                },
                error: function (request, status, error) {
                    alert(request.statusText + "/" + request.statusText + "/" + error);
                }
            });
        }
        else $.alert.open("warning", "Please fill up all required field.");
    });
});
