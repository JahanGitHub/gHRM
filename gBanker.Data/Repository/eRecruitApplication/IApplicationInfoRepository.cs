using gHRM.Core.Filters;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.eRecruitApplication
{
    public interface IApplicationInfoRepository : IRepository<ApplicationInfo>
    {
        IEnumerable<ApplicationInfo> GetListingByFilter(BaseSearchFilter filter);
        ApplicationInfo GetActiveApplicationInfo(Int64 applicationInfo);
        ApplicationInfo GetByNID(string nId);
        ApplicationInfo GetByBirthRegistrationNo(string birthRegistrationNo);
        bool IsExistApplicationInfo(BaseSearchFilter filter);
    }
    public class ApplicationInfoRepository : RepositoryBaseCodeFirst<ApplicationInfo>, IApplicationInfoRepository
    {
        public ApplicationInfoRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }


        public ApplicationInfo GetActiveApplicationInfo(Int64 applicationInfo)
        {
            var single = DataContext.ApplicationInfo.FirstOrDefault(f => f.ApplicationId == applicationInfo);
            return single;
        }

        public ApplicationInfo GetByNID(string nId)
        {
            var single = DataContext.ApplicationInfo.FirstOrDefault(f => f.IsActive == true && f.NationalId == nId);
            return single;
        }
        public ApplicationInfo GetByBirthRegistrationNo(string birthRegistrationNo)
        {
            var single = DataContext.ApplicationInfo.FirstOrDefault(f => f.IsActive == true && f.BirthRegistrationNo == birthRegistrationNo);
            return single;
        }

        public bool IsExistApplicationInfo(BaseSearchFilter filter)
        {
            var isExist = true;

            using (var ts = new gHRMDBContext())
            {
                isExist = DataContext.ApplicationInfo.Any(
                               p => p.IsActive == true
                           && (p.ApplicationId != filter.ApplicationId)
                           && (filter.NationalId == null || filter.NationalId == string.Empty || p.NationalId == filter.NationalId)
                           && (filter.ApplicantName == null || filter.ApplicantName == string.Empty || p.ApplicantName.ToUpper().Trim() == filter.ApplicantName.ToUpper().Trim())
                       );
            }
            return isExist;
        }

        public IEnumerable<ApplicationInfo> GetListingByFilter(BaseSearchFilter filter)
        {
            var listings = DataContext.ApplicationInfo.Where(
                            p => p.IsActive == true
                        && (filter.NationalId == null || filter.NationalId == string.Empty || p.NationalId == filter.NationalId)
                        && (filter.ApplicantName == null || filter.ApplicantName == string.Empty || p.ApplicantName.ToUpper().Trim() == filter.ApplicantName.ToUpper().Trim())
                    ).AsParallel().ToList();
            return listings;
        }

    }
}
