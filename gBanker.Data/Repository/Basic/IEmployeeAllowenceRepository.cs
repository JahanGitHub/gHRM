using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Basic
{
    public interface IEmployeeAllowenceRepository : IRepository<EmployeeAllowence>
    {
        IEnumerable<EmployeeAllowanceCommonClass> GetAllAllowanceCollection();
    }

    public class EmployeeAllowenceRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeAllowence>, IEmployeeAllowenceRepository
    {


        public EmployeeAllowenceRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<EmployeeAllowanceCommonClass> GetAllAllowanceCollection()
        {
            var results = (

                from A in DataContext.EmployeeAllowence
                join G in DataContext.EmployeeGradeList on A.GradeId equals G.GradeId
                join C in DataContext.ComponentPayroll on A.ComponentId equals C.Id
                join E in DataContext.EmployeeStatus on A.EmployeeStatusId equals E.StatusId 

                where A.IsActive && G.IsActive

                select new EmployeeAllowanceCommonClass
                {
                    Id = A.Id,
                    Allowance = A.Allowance,
                    GradeName = G.GradeName,
                    StatusName = E.StatusName,
                    ComponentName = C.ComponentName,
                    ComponentId = A.ComponentId,
                    EmpGradeId = A.GradeId,
                    EmpStatusId = A.EmployeeStatusId,
                    RatioOn = A.RatioOn
                }).ToList();



            return results;

        }



    }


    public class EmployeeAllowanceCommonClass
    {
        public int Id { get; set; }
        //  public int? ComponentId { get; set; }
        public string GradeName { get; set; }
        public string StatusName { get; set; }
        public int EmpTypeId { get; set; }
        public string TypName { get; set; }
        public bool IsActive { get; set; }
        public decimal? Allowance { get; set; }
        public string ComponentName { get; set; }
        public int ? ComponentId { get; set; }
        public int? EmpGradeId { get; set; }        
        public int? EmpStatusId { get; set; }
        public string RatioOn { get; set; }

    }


}
