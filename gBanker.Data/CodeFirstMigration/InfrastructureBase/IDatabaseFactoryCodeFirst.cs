using gHRM.Data.CodeFirstMigration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.InfrastructureBase
{
   public interface IDatabaseFactoryCodeFirst
    {
        gHRMDBContext Get();
        //Asad added on 15.05.2017
        //gPFDBContext GetPF();

       //Previous Correct
      // gHRMDBContext Get();
    }
}
