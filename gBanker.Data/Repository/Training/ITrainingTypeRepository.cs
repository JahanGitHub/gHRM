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
    public interface ITrainingTypeRepository : IRepository<TrainingType>
    {
        bool Save(TrainingType Data, long LoggedInEmployeeId, out string Message);
        void DeleteTrainingType(int Id);
    }
    public class TrainingTypeRepository : RepositoryBaseCodeFirst<TrainingType>, ITrainingTypeRepository
    {
        public TrainingTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool Save(TrainingType Data, long LoggedInEmployeeId, out string Message)
        {
            if (!IsSaveValid(Data, out Message)) return false;
            TrainingType _Region = Data.Id > 0 ? DataContext.TrainingTypes.Find(Data.Id) : new TrainingType();
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
                DataContext.TrainingTypes.Add(_Region);
            }
            DataContext.SaveChanges();
            return true;
        }

        public void DeleteTrainingType(int Id)
        {
            TrainingType _TrainingType = DataContext.TrainingTypes.Find(Id);
            _TrainingType.IsActive = false;
            DataContext.SaveChanges();
        }

        private bool IsSaveValid(TrainingType Data, out string Message)
        {
            Message = "";
            string Name = null == Data.Name ? "" : Data.Name.Trim();

            if (Name == "")
            {
                Message = "Name is required";
                return false;
            }
            if ((Data.Id == 0 && DataContext.TrainingTypes.Where(x => x.IsActive && x.Name == Name).Count() > 0)
                || (Data.Id > 0 && DataContext.TrainingTypes.Where(x => x.IsActive && x.Id != Data.Id && x.Name == Name).Count() > 0))
            {
                Message = "Duplicate Name exists";
                return false;
            }
            return true;
        }
    }
}
