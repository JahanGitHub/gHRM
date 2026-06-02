using System;

namespace gHRM.Data.DBDetailModels.Promotions
{
    public class TransferBackLogImportModel
    {
        public string EmployeeCode { get; set; }
        public string OfficeZone { get; set; }
        public string OfficeArea { get; set; }
        public string OfficeDesignation { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public bool IsActive { get; set; }
        public Int64? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
