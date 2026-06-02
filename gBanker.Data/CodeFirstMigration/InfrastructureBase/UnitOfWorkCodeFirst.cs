using gHRM.Data.CodeFirstMigration;


namespace gHRM.Data.CodeFirstMigration.InfrastructureBase
{
    public class UnitOfWorkCodeFirst : IUnitOfWorkCodeFirst
    {

        private readonly IDatabaseFactoryCodeFirst databaseFactory;
        private gHRMDBContext dataContext;
        //1. Asad added on 15.05.2017
        //private gPFDBContext pfDataContext;

        public UnitOfWorkCodeFirst(IDatabaseFactoryCodeFirst databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected gHRMDBContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        //2. Asad added on 15.05.2017
        //protected gPFDBContext PFDataContext
        //{
        //    get { return pfDataContext ?? (pfDataContext = databaseFactory.GetPF()); }
        //}

        public void Commit()
        {
            DataContext.SaveChanges();
        }

        //3. Asad added on 15.05.2017
        //public void CommitPF()
        //{
        //    PFDataContext.SaveChanges();
        //}

        //Previous Correct
    //    private readonly IDatabaseFactoryCodeFirst databaseFactory;
    //    private gHRMDBContext dataContext;

    //    public UnitOfWorkCodeFirst(IDatabaseFactoryCodeFirst databaseFactory)
    //    {
    //        this.databaseFactory = databaseFactory;
    //    }

    //    protected gHRMDBContext DataContext
    //    {
    //        get { return dataContext ?? (dataContext = databaseFactory.Get()); }
    //    }

    //    public void Commit()
    //    {
    //        DataContext.SaveChanges();
    //    }
    }
}
