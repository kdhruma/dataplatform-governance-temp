using System;

namespace MDM.ImportSources.Lookup
{
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Imports.Interfaces;

    /// <summary>
    /// This class implements the source data from a RS Lookup Generic10  source.
    /// It reads the data in to a data set and process them as per the request from the Lookup import engine.
    /// </summary>
    public class RSLookupGeneric10 : BaseLookupSource, ILookupImportSourceData
    {
        #region Fields

        private String _sourceFile = String.Empty;

        #endregion

        #region Constructors

        public RSLookupGeneric10(String filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("RSLookupGeneric10 file is not available");
            }

            _sourceFile = filePath;
        }

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with configuration from the job.
        /// </summary>
        /// <param name="job">Indicates the Job</param>
        /// <param name="lookupImportProfile">Indicates the Lookup Import profile</param>
        /// <returns>RSLookupGeneric10 is initialized then will return true else false</returns>
        public Boolean Initialize(Job job, LookupImportProfile lookupImportProfile)
        {
            return true;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion
    }
}
