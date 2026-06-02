using gHRM.Data.CodeFirstMigration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Utility.Gratuity
{
    public class GratuityHelper
    {
        public bool IsValid(GratuityGlobalConfig Config, List<GratuityGlobalConfig> ConfigList, out string Message)
        {
            Message = "";
            ConfigList = ConfigList.Where(x => x.ServiceAgeFrom <= Config.ServiceAgeFrom && x.ServiceAgeTo >= Config.ServiceAgeTo).ToList();

            if (0 == Config.EmployeeStatusId)
            {
                Message = "Eligible Employee Type is Required";
                return false;
            }
            if (DuplicateConfigExists(Config, ConfigList))
            {
                Message = "Gratuity Configuration already exists";
                return false;
            }
            if (Config.ServiceAgeFrom > Config.ServiceAgeTo)
            {
                Message = "Service Age From can not be greater than Service Age To";
                return false;
            }
            int MinServiceAgeFrom = ConfigList.Select(x => x.ServiceAgeFrom).DefaultIfEmpty(0).Min();
            int MaxServiceAgeTo = ConfigList.Select(x => x.ServiceAgeTo).DefaultIfEmpty(0).Max();
            int AllowedServiceAgeFrom = MaxServiceAgeTo + 1;
            int AllowedServiceAgeTo = MinServiceAgeFrom - 1;

            /*if (MaxServiceAgeTo != 0 && Config.ServiceAgeFrom != AllowedServiceAgeFrom && Config.ServiceAgeTo != AllowedServiceAgeTo)
            {
                Message = "Service Age From must be " + AllowedServiceAgeFrom + " OR Service Age To must be " + AllowedServiceAgeTo;
                return false;
            }*/
            /*if (ConfigList.Where(x => x.EffectiveEndDate != null && x.EffectiveStartDate > Config.EffectiveStartDate).Count() > 0)
            {
                Message = 
            }*/
            return true;
        }

        public bool DuplicateConfigExists(GratuityGlobalConfig Config, List<GratuityGlobalConfig> ConfigList)
        {
            return ConfigList.Where(x => x.ServiceAgeFrom == Config.ServiceAgeFrom
                && x.ServiceAgeTo == Config.ServiceAgeTo
                && (
                    (x.EffectiveEndDate != null && x.EffectiveEndDate >= Config.EffectiveStartDate)
                    || (x.EffectiveEndDate == null && x.EffectiveStartDate >= Config.EffectiveStartDate)
                )).Count() > 0;
        }
    }
}
