
#region Using
using System.Collections.Generic;


#endregion

namespace gHRM.Core.Utilities.eRecruitUtilities
{
    public static class ErecruitmentCacheKeyConstants
    {
        public const string COUNTRY = "CK_ERECRUITMENT_COUNTRY";
        public const string STATEORPROVINCE = "CK_ERECRUITMENT_STATEORPROVINCE";
        public const string DISTRICT = "CK_ERECRUITMENT_DISTRICT";
        public const string LGTHANA = "CK_ERECRUITMENT_LGTHANA";
        public const string LGUNION = "CK_ERECRUITMENT_LGUNION";
        public const string EDUCATIONDEGREE = "CK_ERECRUITMENT_EDUCATIONDEGREE";
    }
    public static class DegreeTitleConstants
    {
        public const string SSC = "3";
    }

    public static class ApplicantConfirmationConstants
    {
        public const string ApplicantConfirmationInfoCookieKey = "ApplicantConfirmationInfo";
        public const string ApplicantSubmittedInfoCookieLabel = "application-submitted-info";
    }
}
