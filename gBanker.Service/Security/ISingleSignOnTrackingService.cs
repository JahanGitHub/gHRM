#region Using


#endregion

namespace gHRM.Service
{
    public interface ISingleSignOnTrackingService
    {
        /// <summary>
        /// Get single sign on identifier
        /// </summary>
        /// <returns></returns>
        string GetSingleSignOnIdentifier();

        /// <summary>
        /// Track single sign on
        /// </summary>
        /// <param name="loginIdentifier"></param>
        void TrackSingleSignOn(string loginIdentifier);

        /// <summary>
        /// Remove single sign on identifier
        /// </summary>        
        void RemoveSingleSignOnIdentifier();        
    }
}
