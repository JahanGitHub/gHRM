using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IPFTypeRepository : IRepository<PFType>
    {
        IEnumerable<PFType> GetPFTypeByName(string pfTypeName);
        bool UpdatePFType(PFType objPFType);
        IEnumerable<PFType> GetPFTypes(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    
    public class PFTypeRepository : RepositoryBaseCodeFirst<PFType>, IPFTypeRepository
    {
        public PFTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<PFType> GetPFTypeByName(string pfTypeName)
        {
            IQueryable<PFType> results = null;
            results = DataContext.PFType.Where(x => x.FullName == pfTypeName);
            return results;
        }

        public bool UpdatePFType(PFType objPFType)
        {
            bool isSuccess = true;
            try
            {
                // DataContext.PFType.Attach(objPFType);

                //  DataContext.Entry(objPFType).Property("UpdateDate").IsModified = true;
                DataContext.Entry(objPFType).Property(x => x.UpdateDate).IsModified = true;
                //DataContext.Entry(objPFType).Property("CreateDate").IsModified = false;
                //DataContext.Entry(objPFType).Property("InActiveDate").IsModified = false;
                //DataContext.Entry(objPFType).Property("IsActive").IsModified = false;
                Update(objPFType);
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public IEnumerable<PFType> GetPFTypes(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            //Asad added for temp solution
            TotCount = 0;

            IQueryable<PFType> results = null;

            //if (filterColumnName == "PFTypeShortName")
            //    results = DataContext.PFType.OrderBy(x => x.PFTypeName);
            //else if (filterColumnName == "PFTypeFullName")
            //{
            //    results = results = DataContext.PFType.Skip(startRowIndex).Take(pageSize).OrderBy(x => x.PFTypeName);
            //}
            //else if (filterColumnName == "IsActive")
            //{
            //    results = results = DataContext.PFType.Skip(startRowIndex).Take(pageSize).OrderBy(x => x.IsActive);
            //}
            //else if (filterColumnName == "InActiveDate")
            //{
            //    results = results = DataContext.PFType.Skip(startRowIndex).Take(pageSize).OrderBy(x => x.InActiveDate); 
            //}
            //else if (filterColumnName == "CreateUser")
            //{
            //    results = results = DataContext.PFType.Skip(startRowIndex).Take(pageSize).OrderBy(x => x.CreateUser); 
            //}
            //else if (filterColumnName == "CreateDate")
            //{
            //    results = results = DataContext.PFType.Skip(startRowIndex).Take(pageSize).OrderBy(x => x.CreateDate); 
            //}
            //else
            //    results = results = DataContext.PFType.Skip(startRowIndex).Take(pageSize).OrderBy(x => x.PFTypeId);

            //TotCount = results.LongCount();

            ////var obj = results.Skip(startRowIndex).Take(pageSize).Select(s => new PFType()
            ////{
            ////    PFTypeId = s.PFTypeId,
            ////    PFTypeShortName = s.PFTypeShortName,
            ////    PFTypeFullName = s.PFTypeFullName,
            ////    IsActive = s.IsActive,
            ////    InActiveDate = s.InActiveDate,
            ////    CreateUser = s.CreateUser,
            ////    CreateDate = s.CreateDate,  
            ////});

            ////if (!string.IsNullOrWhiteSpace(jtSorting))
            ////{
            ////    if (jtSorting == "OrderId ASC")
            ////        return obj.OrderBy(o => o.OrderId);
            ////    else if (jtSorting == "OrderId DESC")
            ////        return obj.OrderByDescending(o => o.OrderId);
            ////    else if (jtSorting == "OrderNo ASC")
            ////        return obj.OrderBy(o => o.OrderNo);
            ////    else if (jtSorting == "OrderNo DESC")
            ////        return obj.OrderByDescending(o => o.OrderNo);
            ////    else if (jtSorting == "EmployeeId ASC")          
            ////        return obj.OrderBy(o => o.EmployeeId);
            ////    else if (jtSorting == "EmployeeId DESC")
            ////        return obj.OrderByDescending(o => o.EmployeeId);
            ////    else if (jtSorting == "EmployeeName ASC")        
            ////        return obj.OrderBy(o => o.EmployeeName);
            ////    else if (jtSorting == "EmployeeName DESC")
            ////        return obj.OrderByDescending(o => o.EmployeeName);
            ////    else if (jtSorting == "CurrentOfficeName ASC")
            ////        return obj.OrderBy(o => o.CurrentOfficeName);
            ////    else if (jtSorting == "CurrentOfficeName DESC")
            ////        return obj.OrderByDescending(o => o.CurrentOfficeName);
            ////    else if (jtSorting == "CurrentDepartmentName ASC")
            ////        return obj.OrderBy(o => o.CurrentDepartmentName);
            ////    else if (jtSorting == "CurrentDepartmentName DESC")
            ////        return obj.OrderByDescending(o => o.CurrentDepartmentName);
            ////    else if (jtSorting == "JoiningDate ASC")
            ////        return obj.OrderBy(o => o.JoiningDate);
            ////    else if (jtSorting == "JoiningDate DESC")
            ////        return obj.OrderByDescending(o => o.JoiningDate);
            ////    else if (jtSorting == "NewDepartmentId DESC")
            ////        return obj.OrderByDescending(o => o.NewDepartmentId);
            ////    else if (jtSorting == "NewDepartmentId ASC")
            ////        return obj.OrderBy(o => o.NewDepartmentId);
            ////    else if (jtSorting == "NewDesignationID DESC")
            ////        return obj.OrderByDescending(o => o.NewDesignationID);
            ////    else if (jtSorting == "NewDesignationID DESC")
            ////        return obj.OrderByDescending(o => o.NewDesignationID);
            ////    else if (jtSorting == "officeID DESC")
            ////        return obj.OrderByDescending(o => o.officeID);
            ////    else if (jtSorting == "officeID DESC")
            ////        return obj.OrderByDescending(o => o.officeID);
            ////    else
            ////        return obj.OrderBy(o => o.OrderId);
            ////}
            ////else
            ////    return obj.OrderBy(o => o.OrderId);

            return results;
        }
    }
}
