$(document).ready(function () {
    $("#btnSearch").click(function (e) {
        e.preventDefault();
        LoadGrid()
    });
    $("input[name='btnReport']").click(function (e) {
        e.preventDefault();
        var year = $("#Year option:selected").val();
        var from = $("#FromDateStr").val();
        var to = $("#ToDateStr").val();
        var officetype = $("#BasicOfficeTypeId").val();
        if (year && from && to) {
            var url = `/EmployeePromotion/ReportEmployeeAssessment?year=${year}&from=${from}&to=${to}&format=${e.target.value}&officetype=${officetype}`;
            window.open(url, 'mywindow', 'fullscreen=yes, scrollbars=auto');

        }
    });

    $("#FromDateStr,#ToDateStr").datepicker({
        dateFormat: "dd-M",
        showAnim: "scale",
        changeMonth: true,
        changeYear: false,
    });
});
function LoadGrid() {
    var year = $("#Year option:selected").val();
    var from = $("#FromDateStr").val();
    var to = $("#ToDateStr").val();
    var officetype = $("#BasicOfficeTypeId").val();
    $("#grid").html("")

    if (year && from && to) {
        var htmproType = "", digLst;
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/EmployeePromotion/Map_type_Dig_DropDown',
            dataType: 'json',
            async: true,
            success: function (data) {
                digLst = data.digLst;
                jQuery.each(data.promoTypeLst, function (index, item) {
                    htmproType += `<option value="${item.Value}">${item.Text}</option>`;
                });

            },
            error: function (request, status, error) {
                $.alert.open("Message", "Error ..");
            }
        });


        ////////////////////////////// Grid
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
                    url: '/EmployeePromotion/GetEmployeeAssessmentByJoinDate',
                    dataType: 'json',
                    data: {
                        from: from, to: to, year: year, officetype: officetype
                    }
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
                    field: "EmployeeId",
                    hidden: true,
                    filterable: false
                }, {

                    field: "EmployeeCode",
                    title: "Emp. id",
                    width: "20px",
                    filterable: false
                    //locked: true
                }, {
                    field: "EmployeeName",
                    title: "Employee Name",
                    width: "60px",
                    filterable: false
                    //locked: true
                }, {
                    field: "FirstJoiningDate",
                    title: "Join Date",
                    template: "#= kendo.toString(kendo.parseDate(FirstJoiningDate, 'yyyy-MM-dd'), 'dd-MMM-yyyy') #",
                    width: "40px",
                    filterable: false
                    //locked: true
                }, {
                    field: "DepartmentName",
                    title: "Department",
                    width: "50px",
                    filterable: false
                    //locked: true
                }, {
                    field: "DesignationName",
                    title: "Designation",
                    width: "50px",
                    filterable: false
                    //locked: true
                }, {
                    field: "TotalService",
                    title: "Total Service",
                    width: "25px",
                    filterable: false
                    //locked: true
                }, {
                    field: "LastPromotionYear",
                    title: "Last Pro. Years",
                    width: "25px",
                    filterable: false
                },
                {
                    title: "Assessment Type",
                    width: "35px",
                    template: function (dataItem) {
                        return `<div class="text-center"><select class="form-control" name="ddlassType_${dataItem.EmployeeId}" onChange="AssessmentTypeOnChange(this)">${htmproType}</select></div>`
                    }
                }, {
                    title: "Designation",
                    width: "35px",
                    template: function (dataItem) {
                        htmdig = ""
                        jQuery.each(digLst, function (index, item) {
                            htmdig += `<option value="${item.Value}" ${(dataItem.DesignationId == item.Value ? "selected" : "")}>${item.Text}</option>`;
                        });
                        return `<div class="text-center"><select class="form-control" disabled="disabled" name="ddlDig_${dataItem.EmployeeId}">${htmdig}</select></div>`
                    }
                }, {
                    title: "Remark",
                    width: "35px",
                    template: function (dataItem) {
                        return `<div class="text-center"><textarea style="width:77%;resize:none;" class="form-control" name="txtRemark_${dataItem.EmployeeId}"></textarea></div>`
                    }
                },

                {
                    title: "Action",
                    width: "20px",
                    template: function (dataItem) {
                        return ` <div class="text-center">
                                        ${dataItem.PromotionStatus =="approved"  ?
                            `<a title="Salary Configure" onClick="SalaryConfigure(${dataItem.EmployeeId},${dataItem.PromotionId})"><i class="fa fa-cogs"></i></a>`
                            : dataItem.PromotionStatus == "Pending" ? `<a title="Approve this Assessment" onClick="Approved(${dataItem.PromotionId},${dataItem.EmployeeId},'${dataItem.PromotionDateMsg}')"><i class="fa fa-thumbs-up"></i></a>&nbsp;&nbsp;
                                           <a title="Reject this Assessment" onClick="Rejected(${dataItem.PromotionId},${dataItem.EmployeeId},'${dataItem.PromotionDateMsg}')" href="javascript:void(0);" ><i class="fa fa-thumbs-down"></i></a>&nbsp;` : ""}
                                    </div>`;
                    }
                }
            ]

        });
    }
}
function AssessmentTypeOnChange(e) {
    var ass_type = e.options[e.selectedIndex].text;
    var eid = e.name.replace("ddlassType_", "")

    if (ass_type == "Promotion")
        $("[name='ddlDig_" + eid + "']").removeAttr("disabled");
    else $("[name='ddlDig_" + eid + "']").attr("disabled", "disabled");
}

function Approved(promotionId,eid, promotiondate) {
    var dig = $("[name='ddlDig_" + eid + "']").val();
    var assType = $("[name='ddlassType_" + eid + "']").val();
    var remarks = $("[name='txtRemark_" + eid + "']").val();
    if (!assType)
        $.alert.open("Message", "Assessment type is required");
    else {
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: '/EmployeePromotion/AssessmentApprovalorReject',
            dataType: 'json',
            data: JSON.stringify({ PromotionId: promotionId, eid: eid, proTypeID: assType, digID: dig, remark: remarks, promotiondate: promotiondate, status: "approved" }),
            async: true,
            success: function (data) {
                $.alert.open("Message", data.message);
                if (data.result == 1)
                    LoadGrid();

            },
            error: function (request, status, error) {
                $.alert.open("Message", "Error ..");
            }
        });
    }
    
}
function Rejected(promotionId,eid, promotiondate) {
    var remarks = $("[name='txtRemark_" + eid + "']").val()
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: '/EmployeePromotion/AssessmentApprovalorReject',
        dataType: 'json',
        data: JSON.stringify({ PromotionId: promotionId,eid: eid, status: "rejected", promotiondate: promotiondate, remark: remarks, }),
        async: true,
        success: function (data) {
            $.alert.open("Message", data.message);
            if (data.result == 1)
                LoadGrid();

        },
        error: function (request, status, error) {
            $.alert.open("Message", "Error ..");
        }
    });

}

function SalaryConfigure(eid, PromotionId) {
    window.open(`/PromotionConfiguration/EmployeeSalaryConfigurationAfterAssesment?eid=${eid}&pid=${PromotionId}`, 'mywindow', 'height=' + screen.height + ',width=' + screen.width , 'fullscreen=yes, scrollbars=auto');
}