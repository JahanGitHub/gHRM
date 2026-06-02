using System.Web.Configuration;

namespace gHRM.Core.Utilities.eRecruitUtilities
{
    public static partial class Globals
    {
        public readonly static Section Settings = WebConfigurationManager.GetSection("Cortex") as Section;
        public readonly static Section MRMSettings = WebConfigurationManager.GetSection("HR") as Section;

        static Globals()
        {

        }
    }
}
