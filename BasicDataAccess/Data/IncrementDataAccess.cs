using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;

namespace BasicDataAccess.Data
{
    public class IncrementDataAccess : DataAccessBase
    {
        public IncrementDataAccess()
        {

        }
        public string GetConnectionString()
        {
            //var connectionSrings = ConfigurationManager.ConnectionStrings["incrementDbContext"];

            //return connectionSrings.ConnectionString.ToString();

            var connectionSrings = @"data source=192.192.190.29;initial catalog=paperless;persist security info=True;user id=sa;password=Software@2012;MultipleActiveResultSets=True;App=EntityFramework";

            return connectionSrings;

        }

        protected override ConnectionStringSettings LoadConnectionStringSetting()
        {
            ////Get Employee List. gHRM GB .KHALID 26 August, 2020.
            
            //var connectionSrings = ConfigurationManager.ConnectionStrings["incrementDbContext"];
            // var connectionSrings = @"data source=192.192.190.29;initial catalog=paperless;persist security info=True;user id=sa;password=Software@2012;MultipleActiveResultSets=True;App=EntityFramework";
            // var connectionSrings =  @"Data Source=192.192.190.29;Initial Catalog=paperless;User ID=sa;Password=Software@2012;Connect Timeout=45;Pooling=false" , "System.Data.SqlClient";

            //TO DO: Load connection string values from web.config file, any other source.
            ConnectionStringSettings _dbConnectionStringSetting = new ConnectionStringSettings("paperless", @"Data Source=192.192.190.29;Initial Catalog=paperless;User ID=sa;Password=Software@2012;Connect Timeout=45;Pooling=false", "System.Data.SqlClient");// new ConnectionStringSettings("DCURDB", @"Data Source=10.10.10.40;Initial Catalog=DCURDB;User ID=sa;Password=sa1234;Connect Timeout=45;Pooling=false", "System.Data.SqlClient");

            return _dbConnectionStringSetting;
        }

    }// END Class
}// END Namespace
