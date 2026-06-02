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
    public interface ITrainingTitleRepository : IRepository<TrainingTitle>
    {
        bool Save(TrainingTitle Data, long LoggedInEmployeeId, out string Message);
        void DeleteTrainingTitle(int Id);
    }
    public class TrainingTitleRepository : RepositoryBaseCodeFirst<TrainingTitle>, ITrainingTitleRepository
    {
        public TrainingTitleRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool Save(TrainingTitle Data, long LoggedInEmployeeId, out string Message)
        {
            if (!IsSaveValid(Data, out Message)) return false;
            TrainingTitle _Region = Data.Id > 0 ? DataContext.TrainingTitles.Find(Data.Id) : new TrainingTitle();
            _Region.Title = Data.Title;

            if (_Region.Id > 0)
            {
                _Region.UpdateDate = DateTime.Now;
                _Region.UpdateUser = LoggedInEmployeeId;
            }
            else
            {
                _Region.IsActive = true;
                _Region.CreateDate = DateTime.Now;
                _Region.CreateUser = LoggedInEmployeeId;
                DataContext.TrainingTitles.Add(_Region);
            }
            DataContext.SaveChanges();
            return true;
        }

        public void DeleteTrainingTitle(int Id)
        {
            TrainingTitle _TrainingTitle = DataContext.TrainingTitles.Find(Id);
            _TrainingTitle.IsActive = false;
            DataContext.SaveChanges();
        }

        private bool IsSaveValid(TrainingTitle Data, out string Message)
        {
            Message = "";
            string Name = null == Data.Title ? "" : Data.Title.Trim();

            if (Name == "")
            {
                Message = "Title is required";
                return false;
            }
            if ((Data.Id == 0 && DataContext.TrainingTitles.Where(x => x.IsActive && x.Title == Name).Count() > 0)
                || (Data.Id > 0 && DataContext.TrainingTitles.Where(x => x.IsActive && x.Id != Data.Id && x.Title == Name).Count() > 0))
            {
                Message = "Duplicate Title exists";
                return false;
            }
            return true;
        }
    }
}
