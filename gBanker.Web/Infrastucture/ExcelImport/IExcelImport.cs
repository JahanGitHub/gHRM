using gHRM.Web.Controllers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Web.Infrastucture.ExcelImport
{
    public interface IExcelImport
    {
        void ProcessData(ExcelWorksheets Worksheets, ExcelImportController _Controller);

        void SalaryProcessData(ExcelWorksheets Worksheets, SalaryExcelImportController _Controller);

        void ChallanProcessData(ExcelWorksheets Worksheets, SalaryExcelImportController _Controller);
        string GetAllErrorMsg();
    }
}
