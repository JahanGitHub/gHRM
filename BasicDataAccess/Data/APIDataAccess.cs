using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;


namespace BasicDataAccess.Data
{
    public class APIDataAccess : DataAccessBase
    {
        public APIDataAccess()
        {

        }

        public string GetConnectionString()
        {
            var connectionSrings = ConfigurationManager.ConnectionStrings["APIDataContext"];
            return connectionSrings.ConnectionString.ToString();

            //var connectionSrings = @"data source=192.192.190.29;initial catalog=paperless;persist security info=True;user id=sa;password=Software@2012;MultipleActiveResultSets=True;App=EntityFramework";

            //return connectionSrings;

        }

        protected override ConnectionStringSettings LoadConnectionStringSetting()
        {
             var connectionSrings = ConfigurationManager.ConnectionStrings["APIDataContext"];
            //TO DO: Load connection string values from web.config file, any other source.
            ConnectionStringSettings _dbConnectionStringSetting =  connectionSrings;// new ConnectionStringSettings("DCURDB", @"Data Source=10.10.10.40;Initial Catalog=DCURDB;User ID=sa;Password=sa1234;Connect Timeout=45;Pooling=false", "System.Data.SqlClient");

            return _dbConnectionStringSetting;
        }


    }// END CLASS
}// END NAMESPACE
