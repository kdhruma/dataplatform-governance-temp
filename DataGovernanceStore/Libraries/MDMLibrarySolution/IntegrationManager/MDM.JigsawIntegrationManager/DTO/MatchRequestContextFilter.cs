using System;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Represents class for MatchRequestContextFilter.
    /// </summary>
    public class MatchRequestContextFilter
    {
        /// <summary>
        /// Initializes a new instance of the MatchRequestContextFilter class.
        /// </summary>
        public MatchRequestContextFilter()
        {

        }
        
        /// <summary>
        /// Property denoting the path.
        /// </summary>
        public String Path
        {
            get; set;
        }

        /// <summary>
        /// Property denoting the comparator.
        /// </summary>
        public String Comparator
        {
            get; set;
        }

        /// <summary>
        /// Property denoting the value.
        /// </summary>
        public String[] Value
        {
            get; set;
        }

        /// <summary>
        /// Property denoting the extension.
        /// </summary>
        public String Extension
        {
            get; set;
        }
    }
}
