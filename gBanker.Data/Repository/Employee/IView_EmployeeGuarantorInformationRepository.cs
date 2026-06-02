using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IView_EmployeeGuarantorInformationRepository : IRepository<View_EmployeeGuarantorInformation>
    {

    }
    public class View_EmployeeGuarantorInformationRepository : RepositoryBaseCodeFirst<View_EmployeeGuarantorInformation>, IView_EmployeeGuarantorInformationRepository
    {
        public View_EmployeeGuarantorInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
