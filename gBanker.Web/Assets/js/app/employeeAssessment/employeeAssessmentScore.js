var promotionScoreManager = {
    Clearform: function () {
        $("#PromotionId").val('');
        $("#Year").val('');
        $("#employeecode").val('');
        $("#employeeid").val('');
        $("#employeename").val('');
        $("#Score").val('');        
    },

    // Load Grid
    LoadGrid: function () {     

        $('#grid').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            url: '/EmployeePromotion/GetAllPromotionScore?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
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

                PromotionId: {
                    key: true,
                    list: false,
                    create: false,
                    edit: false
                },
                EmployeeId: {
                    key: false,
                    list: false,
                    create: false,
                    edit: false
                },
                IsActive: {
                    key: false,
                    list: false,
                    create: false,
                    edit: false
                },
                EmployeeCode: {
                    width: '0.5%',
                    title: 'Employee Id',
                    filter: true
                },
                EmployeeName: {
                    width: '3%',
                    title: 'Employee Name',
                    filter: true
                },
                AssessmentYear: {
                    width: '1%',
                    title: 'Assessment Year',
                    filter: true
                },

                Score: {
                    width: '1%',
                    title: 'Score',
                    filter: true
                },         

                EditLink: {
                    title: "Edit",
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        return '<div class="text-center"><a href="#" OnClick="promotionScoreManager.EditGrid( '  + data.record.PromotionId + ',' + "'" + data.record.EmployeeId + "'" + ',' + "'" + data.record.AssessmentYear + "'" + ',' + "'" + data.record.Score + "'" + ',' + "'" + data.record.EmployeeCode + "'" + ' );"><i class="fa fa-pencil-square-o"></i></a></div>';
                    }
                },
                //Delete: {
                //    title: "Delete",
                //    width: '5%',
                //    display: function (data) {
                //        return '<div class="text-center"><a href="#" OnClick="employeeAllowanceListManager.InformationDelete(' + data.record.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                //    }
                //}
            }

        });       
        $('#grid').jtable('load');

        
    },

    // Load Grid
    LoadGridNotFound: function () {

        $('#grid2').jtable({
            paging: true,
            pageSize: 10,
            sorting: true,
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            url: '/EmployeePromotion/GetAllPromotionScoreNotFound?jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
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

                PromotionId: {
                    key: true,
                    list: false,
                    create: false,
                    edit: false
                },
                EmployeeId: {
                    key: false,
                    list: false,
                    create: false,
                    edit: false
                },
                IsActive: {
                    key: false,
                    list: false,
                    create: false,
                    edit: false
                },
                EmployeeCode: {
                    width: '0.5%',
                    title: 'Employee Id',
                    filter: true
                },
                EmployeeName: {
                    width: '3%',
                    title: 'Employee Name',
                    filter: true
                },
                AssessmentYear: {
                    width: '1%',
                    title: 'Assessment Year',
                    filter: true
                },

                Score: {
                    width: '1%',
                    title: 'Score',
                    filter: true
                },

                EditLink: {
                    title: "Edit",
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        return '<div class="text-center"><a href="#" OnClick="promotionScoreManager.EditGrid( ' + data.record.PromotionId + ',' + "'" + data.record.EmployeeId + "'" + ',' + "'" + data.record.AssessmentYear + "'" + ',' + "'" + data.record.Score + "'" + ',' + "'" + data.record.EmployeeCode + "'" + ' );"><i class="fa fa-pencil-square-o"></i></a></div>';
                    }
                },
                //Delete: {
                //    title: "Delete",
                //    width: '5%',
                //    display: function (data) {
                //        return '<div class="text-center"><a href="#" OnClick="employeeAllowanceListManager.InformationDelete(' + data.record.Id + ');"><i class="fa fa-trash-o"></i></a></div>';
                //    }
                //}
            }

        });
        $('#grid2').jtable('load');


    },

    // Edit Grid
    EditGrid: function (PromotionId, EmployeeId, AssessmentYear, Score, EmployeeCode ) {
   //     alert("Edit Grid");
        $("#PromotionId").val(PromotionId);
        $("#employeeid").val(EmployeeId);  
        $("#Year").val(AssessmentYear);
        $("#Score").val(Score);
        $("#employeecode").val(EmployeeCode).blur();
        $("#employeecode").attr('readonly', true);


        $("#btnSave").hide();
        $("#btnUpdate").show();
        $("#btnRestore").show();


    },

    toggleShowHide: function () {
        
        return;
    },

};


// Document Ready 

$(document).ready(function () {

    promotionScoreManager.LoadGrid();
    promotionScoreManager.toggleShowHide();

    $("#btnNotFound").click(function (e) {
        $('#grid').hide();
        promotionScoreManager.LoadGridNotFound();
        promotionScoreManager.toggleShowHide();
    });

    // Save
    $("#btnSave").click(function (e) {

        var assessmentYear = $("#Year").val();
        var employeeId = $("#employeeid").val();
        var score = $("#Score").val();

        var obj = {
            AssessmentYear: assessmentYear,
            employeeid: employeeId,
            Score: score
        }
        if (assessmentYear != "" && employeeId != "" && score != "") {
            e.preventDefault();
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                url: '/EmployeePromotion/EmployeeAssessmentScoreSave',
                dataType: 'json',
                data: JSON.stringify({ obj: obj }),
                async: true,
                success: function (data) {

                    if (data.result == 1) {
                        $.alert.open("Success", data.message);
                        promotionScoreManager.Clearform();
                        promotionScoreManager.LoadGrid();

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
    


        //        error: function (request, status, error) {
        //            $.alert.open("Message", "Error ..");
        //        }
        //    });
        //}
    });

    $("#btnUpdate").hide();
    $("#btnRestore").hide();

    // Edit / Update
    $("#btnUpdate").click(function () {
       // alert("Update");
        var promotionId = $("#PromotionId").val();
        var empId = $("#employeeid").val();
        var score = $("#Score").val();
        var assesYear = $("#Year").val();
        var empCode = $("#employeecode").val();

        var obj = {
            PromotionId: promotionId,
            EmployeeId: empId,
            AssessmentYear: assesYear,
            Score: score,
            employeecode: empCode
        }
        if (empId != "" && score != "" && score > 0 && promotionId != "" && assesYear != "" ) {
            // $('#AjaxLoader').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                async: true,
                cache: false,
                url: '/EmployeePromotion/EmployeeAssessmentScoreEdit',
                data: JSON.stringify({ obj: obj }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //   $('#AjaxLoader').hide();
                    if (data.result == 1) {
                        $.alert.open("Success", data.message);
                        promotionScoreManager.Clearform();
                        $("#employeecode").attr('readonly', false);
                        promotionScoreManager.LoadGrid();
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

    // Employee Load
    $("#employeecode").blur(function () {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/Employee/GetEmployeeByEmployeeCode',
            dataType: 'json',
            data: { employeeCode: $(this).val(),  },
            async: true,
            success: function (data) {
                $("#employeename").val(data.EmployeeName);
                $("#employeeid").val(data.EmployeeId);
            },
            error: function (request, status, error) {
                $.alert.open("Message", "Error ..");
            }
        });
    });

    // Button Restore
    $("#btnRestore").click(function () {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnRestore").hide();
        promotionScoreManager.Clearform();
        $("#employeecode").attr('readonly', false);

    });


});

