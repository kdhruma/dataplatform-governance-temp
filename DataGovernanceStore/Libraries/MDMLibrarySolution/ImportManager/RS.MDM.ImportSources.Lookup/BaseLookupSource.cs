using System;

namespace MDM.ImportSources.Lookup
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// This class implements the helper methods and properties required for Lookup source.
    /// </summary>
    public class BaseLookupSource 
    {
        #region Fields

        private ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        private Job _job = new Job();

        private String _sourceFile = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Base Lookup source parameter less constructor
        /// </summary>
        public BaseLookupSource()
        {
        
        }

        /// <summary>
        /// Constructor with File path as parameter
        /// </summary>
        /// <param name="filePath">Indicates the Base Lookup Source</param>
        public BaseLookupSource(String filePath)
        {
            this._sourceFile = filePath;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the batching mode the provider supports
        /// </summary>
        public ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }

        /// <summary>
        /// Indicates the Job.
        /// </summary>
        protected Job Job
        {
            get { return _job; }
            set { _job = value; }
        }

        /// <summary>
        /// Indicates Source file 
        /// </summary>
        protected String SourceFile
        {
            get { return _sourceFile; }
            set { _sourceFile = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the Lookup tables count. 
        /// </summary>
        /// <param name="application">Indicates the MDM Application</param>
        /// <param name="module">Indicates the MDM Module</param>
        /// <returns>Returns the lookup table count as Integer</returns>
        public Int64 GetLookupTableCount(MDMCenterApplication application, MDMCenterModules module)
        {
            return 0;
        }

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        /// <returns>Returns the ImportProviderBatchingType</returns>
        public ImportProviderBatchingType GetBatchingType()
        {
            return this._batchingType;
        }

        /// <summary>
        /// Get all the lookup data which are waiting to be processed.
        /// </summary>
        /// <param name="application">Indicates the MDM application</param>
        /// <param name="module">Indicates the MDM module</param>
        /// <returns>Return the lookup collection object</returns>
        public LookupCollection GetAllLookupTables(MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        /// <summary>
        /// Get the lookup based on the lookup table name.
        /// </summary>
        /// <param name="lookupTableName">Indicates the lookup table name</param>
        /// <param name="application">Indicates the MDM application</param>
        /// <param name="module">Indicates the MDM module</param>
        /// <returns>Return the lookup object</returns>
        public Lookup GetSingleLookupTable(String lookupTableName, MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        /// <summary>
        /// Gets the next available batch of lookup data for processing.
        /// </summary>
        /// <param name="application">Indicates the MDM application</param>
        /// <param name="module">Indicates the MDM module</param>
        /// <returns>Return the lookup object</returns>
        public Lookup GetNextLookupTable(MDMCenterApplication application, MDMCenterModules module)
        {
            return null;
        }

        /// <summary>
        /// Get the operation result if there are any error occur during Source data reader operation
        /// </summary>
        /// <returns>Returns the operation result</returns>
        public IOperationResult GetOperationResult()
        {
            return null;
        }

        #endregion
    }
}
