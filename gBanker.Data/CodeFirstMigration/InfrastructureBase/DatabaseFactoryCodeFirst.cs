using gHRM.Data.CodeFirstMigration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.InfrastructureBase
{
   public class DatabaseFactoryCodeFirst: Disposable,IDatabaseFactoryCodeFirst
    {
        private gHRMDBContext dataContext;
        //1. Asad added on 15.05.2017
        //private gPFDBContext pfDataContext;
        public gHRMDBContext Get()
        {
            return dataContext ?? (dataContext = new gHRMDBContext());
        }

        //2. Asad added on 15.05.2017
        //public gPFDBContext GetPF()
        //{
        //    return pfDataContext ?? (pfDataContext = new gPFDBContext());
        //}
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
            //3. Asad added on 15.05.2017
            //if (pfDataContext != null)
            //    pfDataContext.Dispose();
        }

       //Previous Correct
       //private gHRMDBContext dataContext;
       //public gHRMDBContext Get()
       // {
       //     return dataContext ?? (dataContext = new gHRMDBContext());
       // }
       // protected override void DisposeCore()
       // {
       //     if (dataContext != null)
       //         dataContext.Dispose();
       // }
    }
}
