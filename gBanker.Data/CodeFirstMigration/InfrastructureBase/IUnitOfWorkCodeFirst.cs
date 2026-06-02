using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.InfrastructureBase
{
   public interface IUnitOfWorkCodeFirst
    {
        void Commit();
       // void CommitPF();

       //Previous Correct
       //void Commit();
    }
}
