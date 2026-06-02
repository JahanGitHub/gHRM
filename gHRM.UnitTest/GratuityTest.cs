using gHRM.Data.CodeFirstMigration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace gHRM.UnitTest
{
    [TestClass]
    public class GratuityTest
    {
        [TestMethod]
        [DataRow(2, 5, 6, 1.4, "2022-4-2", true)]
        [DataRow(2, 5, 6, 1.2, "2022-2-1", false)]
        public void ValidateGratuitConfig(int EmployeeStatusId, int ServiceAgeFrom, int ServiceAgeTo, double GratuityTimes, string StrEffectiveStartDate, bool IsValid)
        {
            string Message = "";
            List<GratuityGlobalConfig> ConfigList = new List<GratuityGlobalConfig>();
            ConfigList.Add(new GratuityGlobalConfig { EmployeeStatusId = 2, ServiceAgeFrom = 5, ServiceAgeTo = 6, GratuityTimes = 1, EffectiveStartDate = Convert.ToDateTime("2022-3-4") });
            ConfigList.Add(new GratuityGlobalConfig { EmployeeStatusId = 2, ServiceAgeFrom = 7, ServiceAgeTo = 9, GratuityTimes = 1.2, EffectiveStartDate = Convert.ToDateTime("2022-3-6") });
            GratuityGlobalConfig Config = new GratuityGlobalConfig
            {
                EmployeeStatusId = EmployeeStatusId,
                ServiceAgeFrom = ServiceAgeFrom,
                ServiceAgeTo = ServiceAgeTo,
                GratuityTimes = GratuityTimes,
                EffectiveStartDate = Convert.ToDateTime(StrEffectiveStartDate)
            };
            gHRM.Data.Utility.Gratuity.GratuityHelper _Helper = new gHRM.Data.Utility.Gratuity.GratuityHelper();
            Assert.AreEqual(IsValid, _Helper.IsValid(Config, ConfigList, out Message));
        }
    }
}
