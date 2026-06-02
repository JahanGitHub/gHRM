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
    public interface ITrainingAreaRepository : IRepository<TrainingArea>
    {
        bool Save(TrainingArea Data, long LoggedInEmployeeId, out string Message);
        void DeleteTrainingArea(int Id);
    }
    public class TrainingAreaRepository : RepositoryBaseCodeFirst<TrainingArea>, ITrainingAreaRepository
    {
        public TrainingAreaRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool Save(TrainingArea Data, long LoggedInEmployeeId, out string Message)
        {
            if (!IsSaveValid(Data, out Message)) return false;
            TrainingArea _Region = Data.Id > 0 ? DataContext.TrainingAreas.Find(Data.Id) : new TrainingArea();
            _Region.Name = Data.Name;

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
                DataContext.TrainingAreas.Add(_Region);
            }
            DataContext.SaveChanges();
            return true;
        }

        public void DeleteTrainingArea(int Id)
        {
            TrainingArea _TrainingArea = DataContext.TrainingAreas.Find(Id);
            _TrainingArea.IsActive = false;
            DataContext.SaveChanges();
        }

        private bool IsSaveValid(TrainingArea Data, out string Message)
        {
            Message = "";
            string Name = null == Data.Name ? "" : Data.Name.Trim();

            if (Name == "")
            {
                Message = "Name is required";
                return false;
            }
            if ((Data.Id == 0 && DataContext.TrainingAreas.Where(x => x.IsActive && x.Name == Name).Count() > 0)
                || (Data.Id > 0 && DataContext.TrainingAreas.Where(x => x.IsActive && x.Id != Data.Id && x.Name == Name).Count() > 0))
            {
                Message = "Duplicate Name exists";
                return false;
            }
            return true;
        }
    }
}
