
var employeeGradeListManager = {
    Clearform: function () {
        $("#GradeName").val('');
        $("#GradeDescription").val('');
        $("#InitialAmount").val('');
        $("#AmountPerIncrement").val('');
        $("#EffectiveDateFrom").val('');
        $("#EffectiveDateTo").val('');
        $("#Percentage").val('0');
    },
    LoadEmployeeGradeListInGrid: function () {
        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            url: '/PRSalaryConfiguration/GetEmployeeGradeList?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
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
                GradeDescription: {
                    width: '15%',
                    title: 'Grade Description'
                },               
                InitialAmount: {
                    width: '10%',
                    title: 'InitialAmount'
                },
                //RatioOn: {
                //    width: '10%',
                //    title: 'Ratio On',
                //    display: function (data) {
                //        var ratioOn = gradeRatioOnConstants.Percentage === data.record.RatioOn ? `${data.record.Percentage}%` : data.record.RatioOn;
                //        return ratioOn;
                //    }
                //},

                //AmountPerIncrement: {
                //    width: '10%',
                //    title: 'AmountPerIncrement'
                //},
                EffectiveDateFrom: {
                    width: '20%',
                    title: 'EffectiveDateFrom'
                },
                EffectiveDateTo: {
                    width: '20%',
                    title: 'EffectiveDateTo'
                },
                EditLink: {
                    title: "Edit",
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        return '<div class="text-center"><a href="#" OnClick="employeeGradeListManager.EditGrid( ' + data.record.Id + ',' + "'" + data.record.GradeName + "'" + ',' + "'" + data.record.GradeDescription + "'" + ',' + data.record.InitialAmount + ',' + "'" + data.record.AmountPerIncrement + "'" + ',' + "'" + data.record.EffectiveDateFrom + "'" + ',' + "'" + data.record.EffectiveDateTo + "'"
                            + ',' + "'" + data.record.RatioOn + "'" + ',' + data.record.Percentage
                            + ' );"><i class="fa fa-pencil-square-o"></i></a></div>';
                    }
                },
                Delete: {
                    title: "Delete",
                    width: '5%',
                    display: function (data) {
                        return '<div class="text-center"><a href="#" OnClick="employeeGradeListManager.InformationDelete(' + data.record.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                    }
                }
            }

        });
        $('#grid').jtable('load');

    },
    EditGrid: function (Id, GradeName, GradeDescription, InitialAmount,
        AmountPerIncrement, EffectiveDateFrom, EffectiveDateTo, RatioOn, Percentage) {
        $("#Id").val(Id);
        $("#GradeName").val(GradeName);
        $("#GradeDescription").val(GradeDescription);
        $("#InitialAmount").val(InitialAmount);
        $("#EffectiveDateFrom").val(EffectiveDateFrom);
        $("#EffectiveDateTo").val(EffectiveDateTo);

        $("#btnSave").hide();
        $("#btnUpdate").show();
        $("#btnRestore").show();
        $("#RatioOn").val(RatioOn);

        employeeGradeListManager.toggleShowHide();

        $("#Percentage").val(Percentage);
        $("#AmountPerIncrement").val(AmountPerIncrement);
    },
    InformationDelete: function (Id) {
        $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
            if (button == 'yes') {
                $.ajax({
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    url: '/PRSalaryConfiguration/DeleteSalaryGrade',
                    data: { Id: Id },
                    dataType: 'json',
                    async: true,
                    success: function (data) {
                        if (data.result == 1) {
                            $.alert.open("Success", data.message);
                            employeeGradeListManager.LoadEmployeeGradeListInGrid();
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
        var ratioOn = $('#RatioOn').val();
        $('#AmountPerIncrement').val('');
        $('#AmountPerIncrement').removeAttr('readonly');
        $('#Percentage').val('0.00');

        if (ratioOn === gradeRatioOnConstants.Fixed) {
            $('.section-percentage').hide();
            return;
        }

        $('.section-percentage').show();
        $('#AmountPerIncrement').attr('readonly', 'readonly');
        return;
    },
};

$(document).ready(function () {
    employeeGradeListManager.LoadEmployeeGradeListInGrid();
    employeeGradeListManager.toggleShowHide();
    $("#btnUpdate").hide();
    $("#btnRestore").hide();

    $("#EffectiveDateFrom").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true
    });
    $("#EffectiveDateTo").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "1980:2050",
        changeYear: true
    });

    $('#RatioOn').on('change', function () {
        employeeGradeListManager.toggleShowHide();
    });

    $('#Percentage').on('focusout', function () {
        var percentage = $(this).val();
        var initialAmount = $('#InitialAmount').val();
        $('#AmountPerIncrement').val(0.00);
        if ((!initialAmount || initialAmount <= 0) || !percentage || percentage <= 0) { return; }

        var amountPerIncrement = ((Number(percentage) * Number(initialAmount)) / 100);

        amountPerIncrement = amountPerIncrement.toFixed(2);
        $('#AmountPerIncrement').val(amountPerIncrement);
    });

    $("#btnSave").click(function () {
        var gradeName = $("#GradeName").val();
        var gradeDes = $("#GradeDescription").val();
        var initialAmt = $("#InitialAmount").val();
        var amtPerInc = $("#AmountPerIncrement").val();
        var dateFrom = $("#EffectiveDateFrom").val();
        var dateTo = $("#EffectiveDateTo").val();
        var ratioOn = $("#RatioOn").val();
        var percentage = $("#Percentage").val();       

        var obj = {
            GradeName: gradeName,
            GradeDescription: gradeDes,
            InitialAmount: initialAmt,
            AmountPerIncrement: amtPerInc,
            EffectiveDateFrom: dateFrom,
            EffectiveDateTo: dateTo,
            RatioOn: ratioOn,
            Percentage: percentage            
        }

        if (gradeName != "" && gradeDes != "" && initialAmt != "" && amtPerInc != "" && dateFrom != "" && dateTo != "") {
            $('#AjaxLoader').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                async: true,
                cache: false,
                url: '/PRSalaryConfiguration/SaveEmployeeSalaryGrade',
                data: JSON.stringify({ obj: obj }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#AjaxLoader').hide();
                    if (data.result == 1) {
                        $.alert.open("Success", data.message);
                        employeeGradeListManager.LoadEmployeeGradeListInGrid();
                        employeeGradeListManager.Clearform();
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

    $("#btnUpdate").click(function () {
        var id = $("#Id").val();
        var gradeName = $("#GradeName").val();
        var gradeDes = $("#GradeDescription").val();
        var initialAmt = $("#InitialAmount").val();
        var amtPerInc = $("#AmountPerIncrement").val();
        var dateFrom = $("#EffectiveDateFrom").val();
        var dateTo = $("#EffectiveDateTo").val();
        var ratioOn = $("#RatioOn").val();
        var percentage = $("#Percentage").val();       

        var obj = {
            Id: id,
            GradeName: gradeName,
            GradeDescription: gradeDes,
            InitialAmount: initialAmt,
            AmountPerIncrement: amtPerInc,
            EffectiveDateFrom: dateFrom,
            EffectiveDateTo: dateTo,
            RatioOn: ratioOn,
            Percentage: percentage,            
        }
        if (gradeName != "" && gradeDes != "" && initialAmt != "" && amtPerInc != "" && dateFrom != "" && dateTo != "") {
            $('#AjaxLoader').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                async: true,
                cache: false,
                url: '/PRSalaryConfiguration/UpdateEmployeeSalaryGrade',
                data: JSON.stringify({ obj: obj }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#AjaxLoader').hide();
                    if (data.result == 1) {
                        $.alert.open("Success", data.message);
                        employeeGradeListManager.LoadEmployeeGradeListInGrid();
                        employeeGradeListManager.Clearform();
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
        employeeGradeListManager.Clearform();
    });
});