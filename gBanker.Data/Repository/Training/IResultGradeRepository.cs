using System.Text;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
using gHRM.Data.Utility;
using System;
using System.Threading.Tasks;
using gHRM.Core.Filters.Offices;
using System.Data.Entity;

namespace gHRM.Data.Repository
{
    public interface IResultGradeRepository : IRepository<ResultGrade>
    {
        bool Save(ResultGrade Data, long LoggedInEmployeeId, out string Message);
        void DeleteResultGrade(int Id);
    }
    public class ResultGradeRepository : RepositoryBaseCodeFirst<ResultGrade>, IResultGradeRepository
    {
        public ResultGradeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool Save(ResultGrade Data, long LoggedInEmployeeId, out string Message)
        {
            if (!IsSaveValid(Data, out Message)) return false;
            ResultGrade _Region = Data.Id > 0 ? DataContext.ResultGrades.Find(Data.Id) : new ResultGrade();
            _Region.Name = Data.Name;
            _Region.IsActive = true;
            _Region.CreateDate = DateTime.Now;
            _Region.CreateUser = LoggedInEmployeeId;
            DataContext.ResultGrades.Add(_Region);
            DataContext.SaveChanges();
            return true;
        }

        public void DeleteResultGrade(int Id)
        {
            ResultGrade _ResultGrade = DataContext.ResultGrades.Find(Id);
            _ResultGrade.IsActive = false;
            DataContext.SaveChanges();
        }

        private bool IsSaveValid(ResultGrade Data, out string Message)
        {
            Message = "";
            string Name = null == Data.Name ? "" : Data.Name.Trim();

            if (Name == "")
            {
                Message = "Name is required";
                return false;
            }
            if ((Data.Id == 0 && DataContext.ResultGrades.Where(x => x.IsActive && x.Name == Name).Count() > 0)
                || (Data.Id > 0 && DataContext.ResultGrades.Where(x => x.IsActive && x.Id != Data.Id && x.Name == Name).Count() > 0))
            {
                Message = "Duplicate Name exists";
                return false;
            }
            return true;
        }
    }
}
