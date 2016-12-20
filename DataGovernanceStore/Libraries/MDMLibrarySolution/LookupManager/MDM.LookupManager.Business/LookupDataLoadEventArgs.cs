using System;

namespace MDM.LookupManager.Business
{
    using MDM.Interfaces;
    using MDM.BusinessObjects;

    /// <summary>
    /// Specifies arguments for events raised for Lookup data load
    /// </summary>
    public class LookupDataLoadEventArgs : EventArgs
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the lookup data
        /// </summary>
        public Lookup LookupData { get; private set; }

        /// <summary>
        /// Property denoting the current context of the application
        /// </summary>
        public ApplicationContext ApplicationContext { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the object
        /// </summary>
        /// <param name="lookupData"></param>
        /// <param name="applicationContext"></param>
        public LookupDataLoadEventArgs(Lookup lookupData, ApplicationContext applicationContext)
        {
            if (lookupData == null)
                throw new ArgumentNullException("lookupData");

            if (applicationContext == null)
                throw new ArgumentNullException("applicationContext");

            LookupData = lookupData;
            ApplicationContext = applicationContext;
        }

        #endregion
    }
}
