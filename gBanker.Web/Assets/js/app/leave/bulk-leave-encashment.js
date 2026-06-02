"use strict";

var Page = {
    Load: function () {
        DataGrid.Load();
        ExcludedGrid.Load();
    },
    Save: function () {
        $("#btnSave").prop("disabled", true);
        var data = JSON.stringify(ExcludedGrid.IdList);
        Req.POST.Save(data, function () {
            $.alert.open("Success", "Data Saved successfully!");
        }, function () {
            $("#btnSave").prop("disabled", false);
        });
    }
};

var DataGrid = {
    Obj: function () { return $("#grid"); },
    Load: function () {
        DataGrid.Obj().kendoGrid({
            toolbar: "<span class='grid-title'>Encashment Employees List</span>",
            dataSource: DataGrid.GetDataSource(),
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
                    width: "50px",
                    field: "Code",
                    title: "Code",
                    filterable: true
                },
                {
                    width: "100px",
                    field: "Name",
                    title: "Name",
                    filterable: true
                },
                {
                    width: "50px",
                    field: "Qty",
                    title: "Days",
                    filterable: true
                },
                {
                    width: "50px",
                    field: "Amt",
                    title: "Amount",
                    filterable: true
                },
                {
                    title: "",
                    width: "15px",
                    template: function (data) {
                        return '<div><span class="fa fa-sign-out" title="Exclude" onclick="DataGrid.Exclude(' + data.Id + ')" style="cursor: pointer;"></span></div>';
                    }
                }
            ]
        });
    },
    GetDataSource: function () {
        var DataList = BulkEncashmentDataList.filter(function (x) { return !ExcludedGrid.IdList.includes(x.Id); });
        return new kendo.data.DataSource({
            data: DataList,
            pageSize: 15
        });
    },
    Reload: function () {
        var Grid = DataGrid.Obj().data("kendoGrid");
        Grid.setDataSource(DataGrid.GetDataSource());
        Grid.dataSource.read();
        Grid.refresh();
    },
    Exclude: function (Id) {
        if (!ExcludedGrid.IdList.includes(Id)) {
            ExcludedGrid.IdList.push(Id);
            DataGrid.Reload();
            ExcludedGrid.Reload();
        }
    }
};

var ExcludedGrid = {
    IdList: [],
    Obj: function () { return $("#grid-excluded"); },
    Load: function () {
        ExcludedGrid.Obj().kendoGrid({
            toolbar: "<span class='grid-title'>Excluded List</span>",
            dataSource: ExcludedGrid.GetDataSource(),
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
                    title: "",
                    width: "15px",
                    template: function (data) {
                        return '<div><span class="fa fa-arrow-left" title="Include" onclick="ExcludedGrid.Include(' + data.Id + ')" style="cursor: pointer;"></span></div>';
                    }
                },
                {
                    width: "50px",
                    field: "Code",
                    title: "Code",
                    filterable: true
                },
                {
                    width: "100px",
                    field: "Name",
                    title: "Name",
                    filterable: true
                },
                {
                    width: "50px",
                    field: "Qty",
                    title: "Days",
                    filterable: true
                },
                {
                    width: "50px",
                    field: "Amt",
                    title: "Amount",
                    filterable: true
                }
            ]
        });
    },
    GetDataSource: function () {
        var DataList = BulkEncashmentDataList.filter(function (x) { return ExcludedGrid.IdList.includes(x.Id); });
        return new kendo.data.DataSource({
            data: DataList,
            pageSize: 15
        });
    },
    Reload: function () {
        var Grid = ExcludedGrid.Obj().data("kendoGrid");
        Grid.setDataSource(ExcludedGrid.GetDataSource());
        Grid.dataSource.read();
        Grid.refresh();
    },
    Include: function (Id) {
        var Index = ExcludedGrid.IdList.findIndex(function (x) { return x == Id; });
        if (-1 != Index) {
            ExcludedGrid.IdList.splice(Index, 1);
            DataGrid.Reload();
            ExcludedGrid.Reload();
        }
    }
};

var Req = {
    POST: {
        Save: function (Data, callback, err_callback) {
            $.ajax({
                url: '/LeaveEncashment/MakeBulkEncashment',
                type: 'Post',
                data: Data,
                dataType: 'json',
                async: true,
                contentType: 'application/json',
                success: function (response) {

                    if (!response.success) {
                        $.alert.open("Error", response.message);
                        if (null != err_callback) { err_callback(); }
                        return;
                    }
                    if (null != callback) { callback(); }
                },
                error: function (data, textStatus, jqXHR) {
                    $.alert.open("Error", "An Error Occured");
                    if (null != err_callback) { err_callback(); }
                }
            });
        }
    }
};

$(document).ready(function () {
    Page.Load();
});