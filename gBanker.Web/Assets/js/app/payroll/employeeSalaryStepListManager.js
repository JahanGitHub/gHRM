
var employeeSalaryStepListManager = {
    Clearform: function () {
        $("#Id,#StepFrom,#StepTo,#AmountOrPercent").val('');
    },
    checkNumeric: function (event) {
        var key = window.event ? event.keyCode : event.which;
        return (((key - 48) * (key - 57) <= 0) || ((key - 96) * (key - 106) <= 0) || key == 110 || key == 190);
    },
    LoadEmployeeStepListInGrid: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            url: '/PRSalaryConfiguration/GetEmployeeSalaryStepList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
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
                GradeId: {
                    key: true,
                    list: false,
                    create: false,
                    edit: false
                },
                GradeName: {
                    width: '10%',
                    title: 'Grade Name'
                },
                StepFrom: {
                    width: '15%',
                    title: 'From'
                },
                StepTo: {
                    width: '10%',
                    title: 'To'
                },
                RatioOn: {
                    width: '10%',
                    title: 'Ratio On',
                    display: function (data) {
                        var ratioOn = data.record.RatioOn;
                        return ratioOn;
                    }
                },

                AmountOrPercent: {
                    width: '10%',
                    title: 'AmountOrPercent'
                },
                EditLink: {
                    title: "Edit",
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        return `<div class="text-center"><a href="#" OnClick="employeeSalaryStepListManager.EditGrid(${data.record.Id},${data.record.GradeId},${data.record.StepFrom},${data.record.StepTo},${data.record.AmountOrPercent},'${data.record.RatioOn}');"><i class="fa fa-pencil-square-o"></i></a></div>`;
                    }
                },
                Delete: {
                    title: "Delete",
                    width: '5%',
                    display: function (data) {
                        return '<div class="text-center"><a href="#" OnClick="employeeSalaryStepListManager.InformationDelete(' + data.record.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                    }
                }
            }

        });
        $('#grid').jtable('load');

    },
    EditGrid: function (Id, GradeId, StepFrom, StepTo, AmountOrPercent, RatioOn) {
        $("#Id").val(Id);
        $("#GradeId").val(GradeId);
        $("#StepTo").val(StepTo);
        $("#StepFrom").val(StepFrom);
        $("#AmountOrPercent").val(AmountOrPercent);
        $("#RatioOn").val(RatioOn);

        $("#btnSave").hide();
        $("#btnUpdate").show();
        $("#btnRestore").show();
    },
    InformationDelete: function (Id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
                        if (button == 'yes') {
                $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    url: '/PRSalaryConfiguration/DeleteEmployeeSalaryStep',
                    data: JSON.stringify({ id: Id }),
                    dataType: 'json',
                    async: true,
                    cache: false,
                    success: function (data) {
                        if (data.result == 1) {
                            $.alert.open("Success", data.message);
                            employeeSalaryStepListManager.LoadEmployeeStepListInGrid();
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

};

$(document).ready(function () {

    $("#StepFrom,#StepTo,#AmountOrPercent").keyup(function (e) {
        var isNumeric = this.checkNumeric(e);
        if (!isNumeric)
            $(this).val('');
    })

    employeeSalaryStepListManager.LoadEmployeeStepListInGrid();
    $("#btnUpdate").hide();
    $("#btnRestore").hide();




    $("#btnSave,#btnUpdate").click(function () {


        var obj = {
            Id: $("#Id").val(),
            GradeId: $("#GradeId").val(),
            StepFrom: $("#StepFrom").val(),
            StepTo: $("#StepTo").val(),
            RatioOn: $("#RatioOn").val(),
            AmountOrPercent: $("#AmountOrPercent").val(),
            IsActive: true
        }

        if (obj.GradeId != "" && obj.StepFrom != "" && obj.StepTo != "" && obj.AmountOrPercent != "") {
            $('#AjaxLoader').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                async: true,
                cache: false,
                url: '/PRSalaryConfiguration/SaveEmployeeSalaryStep',
                data: JSON.stringify({ obj: obj }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#AjaxLoader').hide();
                    if (data.result == 1) {
                        $.alert.open("Success", data.message);
                        employeeSalaryStepListManager.LoadEmployeeStepListInGrid();
                        $("#btnRestore").trigger("click");
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
        employeeSalaryStepListManager.Clearform();
    });
});