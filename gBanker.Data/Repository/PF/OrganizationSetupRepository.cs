using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IOrganizationSetupRepository : IRepository<OrganizationSetup>
    {
        IEnumerable<OrganizationSetup> GetOrganization();
        IEnumerable<OrganizationSetup> GetOrgSetupByName(string orgName);
        //bool UpdatePFType(PFType objPFType);
        //IEnumerable<PFType> GetPFTypes(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }

    public class OrganizationSetupRepository : RepositoryBaseCodeFirst<OrganizationSetup>, IOrganizationSetupRepository
    {
        public OrganizationSetupRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }

        public IEnumerable<OrganizationSetup> GetOrgSetupByName(string orgName)
        {
            IQueryable<OrganizationSetup> results = null;
            results = DataContext.OrganizationSetup.Where(x => x.OrgName == orgName);

            return results;
        }

        public IEnumerable<OrganizationSetup> GetOrganization()
        {
            IQueryable<OrganizationSetup> results = null;
            results = (from s in DataContext.OrganizationSetup.Include("PFType")
                       select s).AsQueryable();

            //(from s in DataContext.EmployeeConfiguration.Include("OfficeSetup").Include("EmployeeDropType")
            //            where s.EmployeeId == 1
            //            select s).AsQueryable();
            
            //
            //-----------------
            //result = (from s in DataContext.OrganizationSetup.Include("PFType")
            //                   // where s.PFTypeId == 1
            //                    select s).FirstOrDefault<OrganizationSetup>();
            return results;
        }
    }
}
