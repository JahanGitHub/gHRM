using BasicDataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.StoredProcedure
{
    public interface IAPISPService
    {
        DataSet GetDataWithoutParameter(string storeProcedureName);

        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;


    //API Added For gHRM GB .KHALID 26 August, 2020.
    }// END Interface
    public class APISPService : IAPISPService
    {

        public DataSet GetDataWithoutParameter(string storeProcedureName)
        {
            using (var gbData = new APIDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new APIDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

    }// END Class
}// END Namespace
