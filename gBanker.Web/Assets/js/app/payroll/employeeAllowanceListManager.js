var employeeAllowanceListManager = {
    Clearform: function () {
        $("#EmpGradeId").val('');
        $("#EmpStatusId").val('');
        $("#ComponentId").val('');
        $("#Allowance").val('');
    },

    // Load
    LoadEmployeeAllowanceListInGrid: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            //url: '/EmployeeAllowence/GetEmployeeAllowanceList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            url: '/EmployeeAllowence/GetAllAllowance?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            data: postData,
                            success: function (data) {
                                $dfd.resolve(data);
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                }
            },
            fields: {

                Id: {
                    key: true,
                    list: false,
                    create: false,
                    edit: false
                },
                GradeName: {
                    width: '15%',
                    title: 'Grade'
                },
                StatusName: {
                    width: '15%',
                    title: 'Status'
                },                
                ComponentName: {
                    width: '10%',
                    title: 'Component'
                },
                RatioOn: {
                    width: '15%',
                    title: 'RatioOn'
                },
                Allowance: {
                    width: '15%',
                    title: 'Allowance'
                },
                EditLink: {
                    title: "Edit",
                    width: '15%',
                    sorting: false,
                    display: function (data) {
                        return `<div class="text-center"><a href="#" OnClick="employeeAllowanceListManager.EditGrid(${data.record.Id},${data.record.EmpGradeId},${data.record.EmpStatusId},${data.record.ComponentId},${data.record.Allowance},'${data.record.RatioOn}');"><i class="fa fa-pencil-square-o"></i></a></div>`;
                        //'<div class="text-center"><a href="#" OnClick="employeeAllowanceListManager.EditGrid( ' + data.record.Id + ',' + "'" + data.record.EmpGradeId + "'" + ',' + "'" + data.record.EmpTypeId + "'" + ',' + data.record.EmpStatusId + ',' + "'" + data.record.ComponentId + "'" + ',' + "'" + data.record.AllowancePercent + "'" + ' );"><i class="fa fa-pencil-square-o"></i></a></div>';
                        
                    }
                },
                Delete: {
                    title: "Delete",
                    width: '5%',
                    display: function (data) {
                        return '<div class="text-center"><a href="#" OnClick="employeeAllowanceListManager.InformationDelete(' + data.record.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                    }
                }
            }

        });
        $('#grid').jtable('load');

    },

    // Edit
    EditGrid: function (Id, EmpGradeId,  EmpStatusId, ComponentId, AllowancePercent,ratioOn ) {
        // alert("Edit Grid");
        $("#Id").val(Id);
        $("#EmpGradeId").val(EmpGradeId);
        $("#EmpStatusId").val(EmpStatusId);
        $("#ComponentId").val(ComponentId);
        $("#Allowance").val(AllowancePercent);
        $("#RatioOn").val(ratioOn);
        
        $("#btnSave").hide();
        $("#btnUpdate").show();
        $("#btnRestore").show();

    },

    // Delete
    InformationDelete: function (Id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
           
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/EmployeeAllowence/DeleteEmpAllowance',
                    data: { Id: Id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {

                        if (data.result == 1) {
                            $.alert.open("Success", data.message);
                            employeeAllowanceListManager.LoadEmployeeAllowanceListInGrid();
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

    toggleShowHide: function () {

        return;
    },
};

// Document Ready

$(document).ready(function () {
    employeeAllowanceListManager.LoadEmployeeAllowanceListInGrid();
    employeeAllowanceListManager.toggleShowHide();

    $("#btnUpdate").hide();

    $("#btnRestore").hide();

    $("#btnSave").click(function () {
        var empGradeId = $("#EmpGradeId").val();
        var empStatusId = $("#EmpStatusId").val();
        var componentNameId = $("#ComponentId").val();
        var allowanceAmount = $("#Allowance").val();

        var obj = {
            GradeId: empGradeId,
            EmployeeStatusId: empStatusId,
            ComponentId: componentNameId,
            Allowance: allowanceAmount,
            RatioOn: $("#RatioOn").val()
        }

        if (empGradeId != "" &&  empStatusId != "" && componentNameId != "" && allowanceAmount != "" ) {
            $('#AjaxLoader').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                async: true,
                cache: false,
                url: '/EmployeeAllowence/SaveEmployeeAllowance',
                data: JSON.stringify({ obj: obj }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#AjaxLoader').hide();
                    if (data.result == 1) {
                        $.alert.open("Success", data.message);
                        employeeAllowanceListManager.LoadEmployeeAllowanceListInGrid();
                        employeeAllowanceListManager.Clearform();
                    } else {
                        $.alert.open("Error", data.message);
                    }

                },
                error: function (xhr, status, error) {
                    alert(error);
                }

            });

        } else {
            $.alert.open("Error", "Please insert required data");
        }
    });

    // Edit/Update

    $("#btnUpdate").click(function () {
        //  alert("Update");
        var id = $("#Id").val();
        var empGradeId = $("#EmpGradeId").val();
        var empStatusId = $("#EmpStatusId").val();
        var componentNameId = $("#ComponentId").val();
        var allowanceAmount = $("#Allowance").val();

        var obj = {
            Id: id,
            GradeId: empGradeId,
            EmployeeStatusId: empStatusId,
            ComponentId: componentNameId,
            Allowance: allowanceAmount,
            RatioOn: $("#RatioOn").val()
        }
        if (empGradeId != "" && empStatusId != "") {
            $('#AjaxLoader').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                async: true,
                cache: false,
                url: '/EmployeeAllowence/UpdateEmployeeAllowance',
                data: JSON.stringify({ obj: obj }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#AjaxLoader').hide();
                    if (data.result == 1) {
                        $.alert.open("Success", data.message);
                        employeeAllowanceListManager.LoadEmployeeAllowanceListInGrid();
                        employeeAllowanceListManager.Clearform();
                        $(function () {
                            $("#btnRestore").click();
                        });
                    } else {
                        $.alert.open("Error", data.message);
                    }

                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });

        } else {
            $.alert.open("Error", "Please insert required data");
        }
    });

    $("#btnRestore").click(function () {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnRestore").hide();
        employeeAllowanceListManager.Clearform();
    });


});