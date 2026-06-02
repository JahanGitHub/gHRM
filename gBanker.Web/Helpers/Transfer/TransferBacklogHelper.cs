using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Helpers.Transfer
{
    public class TransferBacklogHelper
    {
        public IEmployeeTransferService _EmployeeTransferService;
        public long LoggedInEmployeeId;
        public bool IsCurrentOfficeReleaseDate = false;

        public bool Save(EmployeeTransfer model, out string Message)
        {
            Message = "";
            var operationMood = "Create";
            if (!IsSaveValid(model, out Message)) return false;

            var entity = new EmployeeTransfer();
            if (model.Id > 0)
            {
                entity = _EmployeeTransferService.Get(x => x.IsActive == true && x.IsApproved == true && x.Id == model.Id);
                entity.EmployeeId = model.EmployeeId;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.UtcNow;
                Message = "Update Successfull";

                operationMood = "Edit";
            }

            //get last employee transfer
            var getLastTransfer = _EmployeeTransferService.GetLastTranserByEmployeeId(model.EmployeeId);

            entity.EmployeeId = model.EmployeeId;
            entity.OfficeDesignationId = model.OfficeDesignationId;
            entity.OfficeId = model.OfficeId;
            entity.DepartmentId = model.DepartmentId;
            entity.SectionId = model.SectionId;
            entity.OrderNo = model.OrderNo;
            entity.OrderDate = Convert.ToDateTime(model.OrderDate);
            entity.IsTADAApplicable = model.IsTADAApplicable;
            entity.IsMutual = model.IsMutual;
            entity.JoiningDate = model.JoiningDate;
            entity.ReleaseDate = model.ReleaseDate;

            if (model.Id == 0)
            {
                entity.IsActive = true;
                entity.IsPlanned = false;
                entity.IsApproved = true;
                if (!IsCurrentOfficeReleaseDate) entity.ReleaseDate = null;
                entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.CreateDate = DateTime.UtcNow;
                Message = "Save Successfull";
                _EmployeeTransferService.Create(entity);
            }

            if (model.Id > 0)
                _EmployeeTransferService.Update(entity);

            //let's update last transter release date of this employee
            if (!IsCurrentOfficeReleaseDate && getLastTransfer != null && getLastTransfer.Id > 0 && operationMood == "Create" && getLastTransfer.Id != entity.Id)
            {
                getLastTransfer.ReleaseDate = model.ReleaseDate != null ? model.ReleaseDate : ((DateTime)model.JoiningDate).AddDays(-1);
                getLastTransfer.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                getLastTransfer.UpdateDate = DateTime.UtcNow;
                _EmployeeTransferService.Update(getLastTransfer);
            }
            return true;
        }

        private bool IsSaveValid(EmployeeTransfer _Transfer, out string Message)
        {
            Message = "";
            var checkDuplicateEntry = new List<EmployeeTransfer>();

            if (_Transfer.EmployeeId == 0)
            {
                Message = "Employee is not valid";
                return false;
            }
            if (_Transfer.OfficeId <= 0)
            {
                Message = "Employee Office is Required";
                return false;
            }
            if (_Transfer.DepartmentId <= 0)
            {
                Message = "Employee Department is Required";
                return false;
            }
            if (_Transfer.OfficeDesignationId <= 0)
            {
                Message = "Employee Designation is Required";
                return false;
            }
            if (_Transfer.Id == 0)
            {
                checkDuplicateEntry = _EmployeeTransferService.GetMany(p =>
                                          p.EmployeeId == _Transfer.EmployeeId && p.OfficeId == _Transfer.OfficeId
                                         && p.OrderNo == _Transfer.OrderNo).ToList();
            }
            else if (_Transfer.Id > 0)
            {
                checkDuplicateEntry = _EmployeeTransferService.GetMany(p => p.Id != _Transfer.Id &&
                                          p.EmployeeId == _Transfer.EmployeeId && p.OfficeId == _Transfer.OfficeId
                                         && p.OrderNo == _Transfer.OrderNo).ToList();
            }
            if (checkDuplicateEntry.Any())
            {
                if (SessionHelper.CompanyInfo.CompanyShortName == "ADI")
                {
                    return true;
                }
                else
                {
                    Message = "This Order No already exists";
                    return false;
                }
            }
            return true;
        }
    }
}