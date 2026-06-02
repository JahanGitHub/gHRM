using BasicDataAccess;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System.Data;

namespace gHRM.Service.ReportServies
{
    public interface IUltimateReportService
    {        
        DataSet GetDataWithParameterFixedAsset<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;        
        DataSet GetDataWithoutParameterFixedAsset(string storeProcedureName);       
    }
    public class UltimateReportService : IUltimateReportService
    {
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DataSet GetDataWithParameterFixedAsset<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataWithoutParameterFixedAsset(string storeProcedureName)
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }        
    }
}



    