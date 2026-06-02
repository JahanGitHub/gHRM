function getPRComponentListing_onload() {

    var employeeTypeId = $("#EmployeeTypeId").val();
    var employeeStatusId = $("#EmployeeStatusId").val();
    var designationId = $('#DesignationId').val();

    if (employeeTypeId === '' || employeeStatusId === '' || designationId === '') {
       // $.alert.open("Error", "Please select  all required ");
        return;
    }
}




function getPRComponentListing() {

    var employeeTypeId = $("#EmployeeTypeId").val();
    var employeeStatusId = $("#EmployeeStatusId").val();
    var designationId = $('#DesignationId').val();

    if ( employeeTypeId === '' || employeeStatusId === '' || designationId === '' ) {
        $.alert.open("Error", "Please select  all required ");
        return;
    }

    $('#gridKendo').html("");
    var dataSource = new kendo.data.DataSource({
        type: "aspnetmvc-ajax",
        pageSize: 25,
        schema: {
            data: "data",
            total: "total"
        },
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        transport: {
            read: {
                url: '/PRComponent/GetComponentList_designation',
                dataType: 'json',
                data: { employeeTypeId: employeeTypeId, employeeStatusId: employeeStatusId, designationId: designationId }
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
                field: "PRComponentID",
                hidden: true,
                filterable: false
            },
            {
                field: "DesignationName",
                title: "Designation",
                width: "16px",
                filterable: true,
            },
            {
                field: "OfficeLocationName",
                title: "Office",
                width: "12px",               
                filterable: true,
            },
            {
                field: "EmployeeTypeName",
                title: "Employee Type",
                width: "16px",               
                filterable: true,
            },
            {
                field: "EmployeeStatusName",
                title: "Employee Status",
                width: "13px",                            
                filterable: true,
            },
            {
                field: "ComponentName",
                title: "Component",
                width: "14px",               
                filterable: true,
            },
            //{
            //    field: "ComponentTypeInText",
            //    title: "Component Type",
            //    width: "12px",
            //    //locked: true,
            //    filterable: true,
            //},
             {
                field: "RatioBasedOnInText",
                title: "Ratio Based On",
                width: "15px",                
                filterable: true,
            },
            {
                field: "ComponentAmount",
                title: "Component Amount",
                width: "13px",               
                filterable: false,
                template: function (dataItem) {
                    return dataItem.ComponentAmount + " (" + dataItem.ComponentTypeInText + ")";
                }
            },
            {
                field: "TransactionTypeInText",
                title: "Transaction Type",
                width: "12px",
                filterable: true,
            },
            {
                field: "PFTypeInText",
                title: "PF Type",
                width: "15px",
                filterable: true,
            },
           
            //{
            //    field: "AccountCode",
            //    title: "Salary AccCode",
            //    width: "40px",
            //    locked: false,
            //    filterable: false,
            //},
            //{
            //    field: "EffectiveStartDate",
            //    title: "Start Date",
            //    width: "30px",
            //    locked: false,
            //    filterable: false,
            //},
            //{
            //    field: "EffectiveEndDate",
            //    title: "End Date",
            //    width: "50px",
            //    locked: false,
            //    filterable: false,
            //},
            {
                field: "IsProductDependent",
                title: "Is Product Dependent",
                width: "13px",
                filterable: false,
                template: function (dataItem) {
                    return dataItem.IsProductDependent==true?"Yes":"No";
                }
            },
            {
                field: "IsSalaryEffect",
                title: "Is Salary Effect",
                width: "12px",
                filterable: false,
                template: function (dataItem) {
                    return dataItem.IsSalaryEffect == true ? "Yes" : "No";
                }
            },      
            //{
            //    title: "Edit",
            //    width: "8px",
            //    template: function (dataItem) {
            //        return "<div class='text-center'><a href='/PRComponent/Edit?PRComponentID=" + dataItem.PRComponentID + "'><i class='fa fa-pencil-square-o'></i></a></div>";                    
            //    }
            //},
            {
                title: "Actions",
                width: "9px",
                template: function (dataItem) {
                    return "<div class='text-center'><a href='/PRComponent/Edit?PRComponentID=" + dataItem.PRComponentID + "'><i class='fa fa-pencil-square-o'></i></a>"
                        + "<a style='padding-left:10px' href='#' OnClick='Delete(" + dataItem.PRComponentID + ");'><i class='fa fa-trash-o'></i></a></div>";
                }
            }
        ]
    });
}

function deleteConfirm(status) {
    if (status == "true")
        return confirm('Are you sure you want to delete this record');

    $.alert.open("You cannot delete this record because it is already disabled.");
    return false;
}

function Delete(Id) {
    $.alert.open('confirm', 'Are you sure you want to delete this record?', function (button) {
        if (button == 'yes') {
            $.ajax({
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                url: '/PRComponent/Delete',
                data: { Id: Id },
                dataType: 'json',
                async: true,
                success: function (Result) {
                    //get prcomponent listings
                    getPRComponentListing();
                    $.alert.open("Message", Result);
                },
                error: function (request, status, error) {
                    $.alert.open("Message", "Error Occured");
                }
            });
            return true;
        }
        else {       
            return false;
        }
    });
}

$(document).ready(function () {
    $("#EmployeeTypeId").val('');  

    //get prcomponent listings
    getPRComponentListing_onload();

    $("#btnSearch").click(function () {
        //get prcomponent listings
        getPRComponentListing();
    });
});

