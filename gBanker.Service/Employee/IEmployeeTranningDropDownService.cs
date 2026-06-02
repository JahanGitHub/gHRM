using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Service
{
    public interface IEmployeeTranningDropDownService
    {
        IEnumerable<EmployeeTranningDropDown> GetAll();
        EmployeeTranningDropDown GetById(int id);
        GlobalResponse<EmployeeTranningDropDown> Create(EmployeeTranningDropDown objectToCreate);
        GlobalResponse<EmployeeTranningDropDown> Update(EmployeeTranningDropDown objectToUpdate);
        GlobalResponse<EmployeeTranningDropDown> Delete(EmployeeTranningDropDown staffWelfareFundSetting);
        bool IsExistEmployeeTrainingDropDownByTitle(string trainingTitle);
    }
    public class EmployeeTranningDropDownService : IEmployeeTranningDropDownService
    {
        private readonly IEmployeeTranningDropDownRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeTranningDropDownService(IEmployeeTranningDropDownRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeTranningDropDown> GetAll()
        {
            var listing = new List<EmployeeTranningDropDown>();
            using (var db = new gHRMDBContext())
            {
                listing = db.EmployeeTranningDropDowns.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public EmployeeTranningDropDown GetById(int id)
        {
            var single = new EmployeeTranningDropDown();
            using (var db = new gHRMDBContext())
            {
                single = db.EmployeeTranningDropDowns
                    .FirstOrDefault(f => f.EmployeeTrainingDropDownId == id);
            }

            return single;
        }
        public GlobalResponse<EmployeeTranningDropDown> Create(EmployeeTranningDropDown objectToCreate)
        {
            var response = new GlobalResponse<EmployeeTranningDropDown>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.EmployeeTranningDropDowns.Add(objectToCreate);

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Employee Tranning DropDown";
                    response.Result = objectToCreate;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = objectToCreate;
            }

            return response;
        }
        public GlobalResponse<EmployeeTranningDropDown> Update(EmployeeTranningDropDown objectToUpdate)
        {
            var response = new GlobalResponse<EmployeeTranningDropDown>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateEmployeeTranningDropDown = db.EmployeeTranningDropDowns
                        .FirstOrDefault(f => f.EmployeeTrainingDropDownId == objectToUpdate.EmployeeTrainingDropDownId);

                    if (updateEmployeeTranningDropDown == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Employee Tranning DropDown not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateEmployeeTranningDropDown.IsActive = objectToUpdate.IsActive;
                        updateEmployeeTranningDropDown.UpdateBy = objectToUpdate.UpdateBy;
                        updateEmployeeTranningDropDown.UpdateDate = currentDate;

                        db.SaveChanges();


                        response.IsSuccess = true;
                        response.Message = "Success, Updated Employee Tranning DropDown";
                        response.Result = objectToUpdate;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = objectToUpdate;
            }

            return response;
        }

        public bool IsExistEmployeeTrainingDropDownByTitle(string trainingTitle)
        {
            var isExist = false;
            using (var db = new gHRMDBContext())
            {
                isExist = db.EmployeeTranningDropDowns
                    .Any(f => f.EmployeeTrainingDropDownName.Trim().ToLower() == trainingTitle.Trim().ToLower());
            }

            return isExist;
        }

        public GlobalResponse<EmployeeTranningDropDown> Delete(EmployeeTranningDropDown objectToUpdate)
        {
            var response = new GlobalResponse<EmployeeTranningDropDown>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteEmployeeTranningDropDown = db.EmployeeTranningDropDowns
                        .FirstOrDefault(f => f.EmployeeTrainingDropDownId == objectToUpdate.EmployeeTrainingDropDownId);

                    if (deleteEmployeeTranningDropDown == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Employee Tranning DropDown not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deleteEmployeeTranningDropDown.IsActive = false;
                        deleteEmployeeTranningDropDown.UpdateBy = objectToUpdate.UpdateBy;
                        deleteEmployeeTranningDropDown.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Employee Training DropDown";
                        response.Result = objectToUpdate;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = objectToUpdate;
            }

            return response;
        }
    }
}
