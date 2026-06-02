using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Company GetCompanyInfo();
        IEnumerable<DBCompanyDetailModel> GetCompanyDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        string GetCompanyNameOtherAndWebsite(out string WebsiteUrl);
    }
    public class CompanyRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.Company>, ICompanyRepository
    {
        public CompanyRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public Company GetCompanyInfo()
        {
            var single = new Company();
            try
            {
                single = DataContext.Companies.FirstOrDefault();

                return single;
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<DBCompanyDetailModel> GetCompanyDetail(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<Company> results = null;
            if (filterColumnName == "CompanyName")
                results = DataContext.Companies.Where(x => x.IsActive == true && x.CompanyName.Contains(filterValue));
            else if (filterColumnName == "CompanyType")
                results = DataContext.Companies.Where(x => x.IsActive == true && x.CompanyType.Contains(filterValue));
            else
                results = DataContext.Companies.Where(x => x.IsActive == true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.CompanyId).Skip(startRowIndex).Take(pageSize).Select(s => new DBCompanyDetailModel()
            {
                CompanyType = s.CompanyType,
                CompanyId = s.CompanyId,
                CompanyName = s.CompanyName,
                CompanyAddress = s.CompanyAddress,
                CompanyPhone = s.CompanyPhone,
                CompanyCode = s.CompanyCode,
                // DataFilter               
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "CompanyId ASC")
                    return obj.OrderBy(o => o.CompanyId);
                else if (jtSorting == "CompanyId DESC")
                    return obj.OrderByDescending(o => o.CompanyId);
                else if (jtSorting == "CompanyName ASC")
                    return obj.OrderBy(o => o.CompanyName);
                else if (jtSorting == "CompanyName DESC")
                    return obj.OrderByDescending(o => o.CompanyName);
                else if (jtSorting == "CompanyAddress ASC")                       //DataSorting
                    return obj.OrderBy(o => o.CompanyAddress);
                else if (jtSorting == "CompanyAddress DESC")
                    return obj.OrderByDescending(o => o.CompanyAddress);
                else if (jtSorting == "CompanyType ASC")                                           //DataSorting
                    return obj.OrderBy(o => o.CompanyType);
                else if (jtSorting == "CompanyType DESC")
                    return obj.OrderByDescending(o => o.CompanyType);
                else if (jtSorting == "CompanyPhone ASC")
                    return obj.OrderBy(o => o.CompanyPhone);
                else if (jtSorting == "CompanyPhone DESC")
                    return obj.OrderByDescending(o => o.CompanyPhone);
                else if (jtSorting == "CompanyCode ASC")
                    return obj.OrderBy(o => o.CompanyCode);
                else if (jtSorting == "CompanyCode DESC")
                    return obj.OrderByDescending(o => o.CompanyCode);
                else
                    return obj.OrderBy(o => o.CompanyId);
            }
            else
                return obj.OrderBy(o => o.CompanyId);
        }

        public string GetCompanyNameOtherAndWebsite(out string WebsiteUrl)
        {
            WebsiteUrl = "";
            var _Company = DataContext.Companies.Select(x => new { x.CompanyNameOther, x.WebsiteUrl }).FirstOrDefault();
            if (null == _Company) return "";
            WebsiteUrl = _Company.WebsiteUrl;
            return _Company.CompanyNameOther;
        }
    }
}
