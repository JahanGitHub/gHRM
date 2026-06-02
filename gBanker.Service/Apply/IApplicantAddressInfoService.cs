using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Apply;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Apply
{
    public interface IApplicantAddressInfoService : IServiceBase<ApplicantAddressInfo>
    {
        
    }
    public class ApplicantAddressInfoService : IApplicantAddressInfoService
    {
        private readonly IApplicantAddressInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantAddressInfoService(IApplicantAddressInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ApplicantAddressInfo Create(ApplicantAddressInfo objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ApplicantAddressInfo Get(Expression<Func<ApplicantAddressInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicantAddressInfo> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantAddressInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantAddressInfo> GetAsync(Expression<Func<ApplicantAddressInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ApplicantAddressInfo GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicantAddressInfo> GetMany(Expression<Func<ApplicantAddressInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantAddressInfo>> GetManyAsync(Expression<Func<ApplicantAddressInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicantAddressInfo objectToUpdate)
        {
            throw new NotImplementedException();
        }
    }
  }
