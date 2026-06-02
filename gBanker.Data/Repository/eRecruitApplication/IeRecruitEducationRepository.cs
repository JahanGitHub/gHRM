using gHRM.Core.Filters;
using gHRM.Core.Filters.eRecruit;
using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.eRecruitApplication
{
    
    public interface IeRecruitEducationRepository : IRepository<eRecruitEmployeeEducation>
    {
        IEnumerable<eRecruitEmployeeEducation> GetEmployeeEducationsByFilterByFilter(BaseSearchFilter filter);
        eRecruitEmployeeEducation GetEmployeeEducationInfoByFilter(BaseSearchFilter filter);

    }
    public class eRecruitEducationRepository : RepositoryBaseCodeFirst<eRecruitEmployeeEducation>, IeRecruitEducationRepository
    {
        public eRecruitEducationRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public eRecruitEmployeeEducation GetEmployeeEducationInfoByFilter(BaseSearchFilter filter)
        {
            var single = DataContext.eRecruitEmployeeEducations
                .FirstOrDefault(p => p.IsActive
                                && p.DegreeTitle == filter.DegreeTitle
                                && p.RollNo == filter.RollNoVerify
                                && p.BoardName == filter.BoardName
                                && p.PassingYear == filter.PassingYear
                                );
            return single;
        }
        public IEnumerable<eRecruitEmployeeEducation> GetEmployeeEducationsByFilterByFilter(BaseSearchFilter filter)
        {
            var listings = DataContext.eRecruitEmployeeEducations
                .Where(p => p.IsActive
                                && (filter.ApplicantId == null || filter.ApplicantId == 0 || p.EmployeeId == filter.ApplicantId)
                                ).AsParallel().ToList();
            return listings;
        }
    }
}