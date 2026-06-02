using System;
using System.Linq;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEducationDegreeRepository : IRepository<EducationDegree>
    {
        List<Dictionary<string, object>> GetDropdownList();
    }
    public class EducationDegreeRepository : RepositoryBaseCodeFirst<EducationDegree>, IEducationDegreeRepository
    {
        public EducationDegreeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<Dictionary<string, object>> GetDropdownList()
        {
            List<Dictionary<string, object>> _List = new List<Dictionary<string, object>>();
            var DataList = DataContext.EducationDegree.Where(x => x.IsActive != null && x.IsActive.Value && x.CompanyId == 1)
                .OrderBy(x => x.DegreeLevelId).Select(x => new
                {
                    x.DegreeLevelId,
                    x.DegreeLevel
                }).Distinct().ToList();
            foreach (var DataItem in DataList)
            {
                Dictionary<string, object> DItem = new Dictionary<string, object>();
                DItem["DegreeLevelId"] = DataItem.DegreeLevelId;
                DItem["DegreeLevel"] = DataItem.DegreeLevel;
                _List.Add(DItem);
            }
            return _List;
        }
    }
}
