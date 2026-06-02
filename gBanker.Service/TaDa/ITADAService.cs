using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.TaDa;
using gHRM.Data.Repository.TaDa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.TaDa
{
    public interface ITADAPurposeService : IServiceBase<TADAPurpose>
    {
        BaseResponse IsValidTADAPurpose(TADAPurpose model);
        GlobalResponse<TADAPurpose> Create(TADAPurpose objectToCreate);
        GlobalResponse<TADAPurpose> Update(TADAPurpose objectToUpdate);
        GlobalResponse<TADAPurpose> Delete(TADAPurpose tADAPurpose);
    }
    public class TADAPurposeService : ITADAPurposeService
    {
        private readonly ITADAPurposeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TADAPurposeService(ITADAPurposeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<TADAPurpose> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public TADAPurpose GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        //public TADAPurpose Create(TADAPurpose objectToCreate)
        //{
        //    repository.Add(objectToCreate);
        //    Save();
        //    return objectToCreate;
        //}

        //public void Update(TADAPurpose objectToUpdate)
        //{
        //    repository.Update(objectToUpdate);
        //    Save();
        //}

        //public void Delete(int id)
        //{
        //    var entity = repository.GetById(id);
        //    repository.Delete(entity);
        //    Save();
        //}

        public void Save()
        {
            unitOfWork.Commit();
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public TADAPurpose Get(Expression<Func<TADAPurpose, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<TADAPurpose> GetMany(Expression<Func<TADAPurpose, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        public BaseResponse IsValidTADAPurpose(TADAPurpose model)
        {
            var response = new BaseResponse();
            var isFound = true;
            using (var db = new gHRMDBContext())
            {
                if (model.Id > 0)
                {
                    isFound = db.TADAPurposes
                            .Any(f =>f.IsActive && f.Id!=model.Id && f.Purpose.ToLower().Trim() == model.Purpose.ToLower().Trim());
                }
                else
                {
                    isFound = db.TADAPurposes
                            .Any(f => f.IsActive && f.Purpose.ToLower().Trim() == model.Purpose.ToLower().Trim());
                }
                response = new BaseResponse
                {
                    IsSuccess = !isFound, //not valid
                    Message = isFound ? $" Already exist. Please try again" : "Sucess"
                };
            }

            return response;
        }

        public GlobalResponse<TADAPurpose> Create(TADAPurpose objectToCreate)
        {
            var response = new GlobalResponse<TADAPurpose>();
            var currentDate = DateTime.Now;
            var isOperationSuccess = true;
            try
            {
                using (var db = new gHRMDBContext())
                {

                    if (isOperationSuccess)
                    {
                        objectToCreate.CreateDate = currentDate;
                        db.TADAPurposes.Add(objectToCreate);

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Added TADA Purpose";
                        response.Result = objectToCreate;
                    }
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

        public GlobalResponse<TADAPurpose> Update(TADAPurpose objectToUpdate)
        {
            var response = new GlobalResponse<TADAPurpose>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var updateTADAPurpose = db.TADAPurposes
                        .FirstOrDefault(f => f.Id == objectToUpdate.Id);

                    if (updateTADAPurpose == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, TADA Purpose not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        updateTADAPurpose.Purpose = objectToUpdate.Purpose;
                        updateTADAPurpose.Remarks = objectToUpdate.Remarks;
                        updateTADAPurpose.IsActive = objectToUpdate.IsActive;
                        updateTADAPurpose.UpdateUser = objectToUpdate.UpdateUser;
                        updateTADAPurpose.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Updated TADA Purpose";
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
        public GlobalResponse<TADAPurpose> Delete(TADAPurpose tADAPurpose)
        {
            var response = new GlobalResponse<TADAPurpose>();
            var isOperationSuccess = true;
            var currentDate = DateTime.Now;
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var deletetADAPurpose = db.TADAPurposes
                        .FirstOrDefault(f => f.Id == tADAPurpose.Id);

                    if (deletetADAPurpose == null)
                    {
                        isOperationSuccess = false;
                        response.IsSuccess = false;
                        response.Message = "Warning, Salary Date Config not exist";
                        response.Result = null;
                    }

                    if (isOperationSuccess)
                    {
                        deletetADAPurpose.IsActive = false;
                        deletetADAPurpose.UpdateUser = tADAPurpose.UpdateUser;
                        deletetADAPurpose.UpdateDate = currentDate;

                        db.SaveChanges();

                        response.IsSuccess = true;
                        response.Message = "Success, Deleted TADA Purpose";
                        response.Result = tADAPurpose;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Result = tADAPurpose;
            }

            return response;
        }
        #region Asyc
        public virtual async Task<IEnumerable<TADAPurpose>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<TADAPurpose>> GetManyAsync(Expression<Func<TADAPurpose, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<TADAPurpose> GetAsync(Expression<Func<TADAPurpose, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        TADAPurpose IServiceBase<TADAPurpose>.Create(TADAPurpose objectToCreate)
        {
            throw new NotImplementedException();
        }

        void IServiceBase<TADAPurpose>.Update(TADAPurpose objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
