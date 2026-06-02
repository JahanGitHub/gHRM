using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Employee;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IEmployeeDocumentService
    {
        IEnumerable<EmployeeDocument> GetAll();
        Task<IEnumerable<EmployeeDigitalIDModel>> GetEmployeeDigitalIDInfo(EmployeeSearchFilter filter);
        EmployeeDocument GetById(int id);
        EmployeeDocument GetByEmployeeId(int employeeId, string documentType);
        GlobalResponse<EmployeeDocument> Create(EmployeeDocument objectToCreate);
        GlobalResponse<EmployeeDocument> Update(EmployeeDocument objectToUpdate);
        GlobalResponse<EmployeeDocument> Delete(EmployeeDocument staffWelfareFundSetting);
    }
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeDocumentService(IEmployeeDocumentRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmployeeDigitalIDModel>> GetEmployeeDigitalIDInfo(EmployeeSearchFilter filter)
        {
            return await repository.GetEmployeeDigitalIDInfo(filter);
        }

        public IEnumerable<EmployeeDocument> GetAll()
        {
            var listing = new List<EmployeeDocument>();
            using (var db = new gHRMDBContext())
            {
                listing = db.EmployeeDocuments.Where(f => f.IsActive)
                    .AsParallel().ToList();
            }

            return listing;
        }
        public EmployeeDocument GetById(int id)
        {
            var single = new EmployeeDocument();
            using (var db = new gHRMDBContext())
            {
                single = db.EmployeeDocuments
                    .FirstOrDefault(f=>f.EmployeeDocumentId==id);
            }

            return single;
        }

        public EmployeeDocument GetByEmployeeId(int employeeId, string documentType)
        {
            var single = new EmployeeDocument();
            using (var db = new gHRMDBContext())
            {
                single = db.EmployeeDocuments
                    .FirstOrDefault(f => f.IsActive && f.EmployeeId == employeeId && f.DocumentType== documentType);
            }

            return single;
        }
        public GlobalResponse<EmployeeDocument> Create(EmployeeDocument objectToCreate)
        {
            var response = new GlobalResponse<EmployeeDocument>();
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    objectToCreate.CreateDate = currentDate;
                    db.EmployeeDocuments.Add(objectToCreate);                    

                    db.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Success, Added Employee Document";
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
        public GlobalResponse<EmployeeDocument> Update(EmployeeDocument objectToUpdate)
        {
            var response = new GlobalResponse<EmployeeDocument>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateEmployeeDocument = db.EmployeeDocuments
                        .FirstOrDefault(f=>f.EmployeeDocumentId==objectToUpdate.EmployeeDocumentId);

                    if (updateEmployeeDocument == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Employee Document not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateEmployeeDocument.DocumentType = objectToUpdate.DocumentType;
                        updateEmployeeDocument.DocumentUrl = objectToUpdate.DocumentUrl;                        
                        updateEmployeeDocument.DocumentRemarks = objectToUpdate.DocumentRemarks;
                        updateEmployeeDocument.IsActive = objectToUpdate.IsActive;
                        updateEmployeeDocument.UpdateUser = objectToUpdate.UpdateUser;
                        updateEmployeeDocument.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Updated Employee Document";
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
        public GlobalResponse<EmployeeDocument> Delete(EmployeeDocument staffWelfareFundSetting)
        {
            var response = new GlobalResponse<EmployeeDocument>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deleteEmployeeDocument = db.EmployeeDocuments
                        .FirstOrDefault(f => f.EmployeeDocumentId == staffWelfareFundSetting.EmployeeDocumentId);

                    if (deleteEmployeeDocument == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Employee Document not exist";
                        response.Result = null ;
                    }

                    if (isOperationSuccess)
                    {
                        deleteEmployeeDocument.IsActive = false;
                        deleteEmployeeDocument.UpdateUser = staffWelfareFundSetting.UpdateUser;
                        deleteEmployeeDocument.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted Employee Document";
                        response.Result = staffWelfareFundSetting;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = staffWelfareFundSetting;
            }

            return response;
        }        
    }
}
