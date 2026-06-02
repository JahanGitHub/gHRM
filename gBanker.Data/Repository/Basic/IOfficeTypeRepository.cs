using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Utility;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface IOfficeTypeRepository : IRepository<OfficeType>
    {
        IEnumerable<DropDownAttribute> getOfficeTypeList();
    }
    public class OfficeTypeRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.OfficeType>, IOfficeTypeRepository
    {
       public OfficeTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

       public IEnumerable<DropDownAttribute> getOfficeTypeList()
       {
           var List = DataContext.OfficeTypes.Where(b => b.IsActive == true)
               .Select(b => new DropDownAttribute
               {
                   Id = b.OfficeTypeId,
                   Name = b.OfficeTypeName,
                   NameOther = b.OfficeShortName,
                   OtherString = b.OfficeTypeCode
               });
           return List;
       }
    }
}
