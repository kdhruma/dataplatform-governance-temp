using System;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace MDM.ImportSources.Lookup
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;

    /// <summary>
    /// This class implements the source data from a Lookup Data Xml source. It has one forward only reader.
    /// </summary>
    public class LookupSource10 :BaseLookupSource, ILookupImportSourceData
    {
        #region Fields

        /// <summary>
        /// Single forward only reader needs to be synchronized.
        /// </summary>
        private Object readerLock = new Object();

        LookupCollection lookupTables = null;

        private XmlReader lookupXmlReader = null;

        ImportProviderBatchingType _batchingType = ImportProviderBatchingType.Single;

        string _sourceFile = string.Empty;

        string _schemaFilePath = String.Empty;

        string errorMessage = string.Empty;

        Int32 currentLookupIndex = 0;

        List<Lookup> lookupList = new List<Lookup>();

        #endregion Fields

        #region Constructors

        public LookupSource10(string filePath)
        {
            _sourceFile = filePath;
        }

        #endregion Constructors

        #region Public Properties
        public new ImportProviderBatchingType BatchingType
        {
            get { return _batchingType; }
            set { _batchingType = value; }
        }
        #endregion

        #region IImportSourceData Methods For Entity

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="lookupImportProfile"></param>
        /// <returns></returns>
        public Boolean Initialize(Job job, LookupImportProfile lookupImportProfile)
        {
            if (string.IsNullOrEmpty(_sourceFile))
            {
                throw new ArgumentNullException("Lookup Xml file is not available");
            }

            // If validation is required...
            ExecutionStep coreStep = null;
            foreach (ExecutionStep executionStep in lookupImportProfile.ExecutionSteps)
            {
                if (executionStep.StepType == ExecutionStepType.Core)
                {
                    coreStep = executionStep;
                }
            }

            //TODO:: Check if we need schema validation in lookup
            //if (coreStep.StepConfiguration.ProcessingSpecifications.ValidateSchema)
            //{
            //    JobParameter schemaFilePathParameter = job.JobData.JobParameters["SchemaFilePath"];

            //    if (schemaFilePathParameter == null)
            //    {
            //        throw new ArgumentNullException("Rs XML Schema file path parameter is not set by the import engine.");
            //    }

            //    _schemaFilePath = schemaFilePathParameter.Value;

            //    if (String.IsNullOrEmpty(_schemaFilePath))
            //    {
            //        throw new ArgumentNullException("Rs XML Schema file path parameter set by import engine is empty.");
            //    }

            //    FileInfo fileInfo = new FileInfo(_schemaFilePath);

            //    if (!fileInfo.Exists)
            //    {
            //        throw new ArgumentNullException("Rs XML Schema file path parameter set by import engine is not valid.");
            //    }

            //    if (!ValidateSchema(_sourceFile))
            //        return false;
            //}

            if (System.IO.File.Exists(_sourceFile))
            {
                lookupXmlReader = XmlReader.Create(_sourceFile);
            }
            else
            {
                throw new ArgumentException(String.Format("Lookup XML file is not available in the specified location {0}", _sourceFile));
            }
            return true;
        }

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        /// <returns></returns>
        public new ImportProviderBatchingType GetBatchingType()
        {
            return BatchingType;
        }

        public new Int64 GetLookupTableCount(MDMCenterApplication application, MDMCenterModules module)
        {
            Int32 count = 0;

            this.GetAllLookupTables(application, module);

            if (lookupTables != null)
            {
                count = lookupTables.Count;
            }
            return count;
        }
        /// <summary>
        /// Gets all lookup table objects
        /// </summary>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public new BusinessObjects.LookupCollection GetAllLookupTables(MDMCenterApplication application, MDMCenterModules module)
        {
            if (lookupTables == null)
            {
                lock (readerLock)
                {
                    if (lookupTables == null)
                    {
                        if (lookupXmlReader.ReadState == ReadState.Initial)
                        {
                            lookupXmlReader.Read();
                        }
                        String lookupXml = lookupXmlReader.ReadOuterXml();
                        lookupTables = new LookupCollection(lookupXml);
                        lookupList = lookupTables.ToList();
                    }
                }
            }
            return lookupTables;
        }

        public new BusinessObjects.Lookup GetSingleLookupTable(String lookupTableName, MDMCenterApplication application, MDMCenterModules module)
        {
            Lookup lookup = null;

            this.GetAllLookupTables(application, module);

            if (lookupTables != null)
            {
                lookup = lookupTables.FirstOrDefault(lk => lk.Name.Equals(lookupTableName));
            }

            return lookup;
        }

        public new BusinessObjects.Lookup GetNextLookupTable(MDMCenterApplication application, MDMCenterModules module)
        {
            Lookup lookup = null;

            this.GetAllLookupTables(application, module);

            if (lookupList != null)
            {
                if (lookupList.Count > currentLookupIndex)
                {
                    lookup = lookupList[currentLookupIndex];
                    currentLookupIndex++;
                }
            }

            return lookup;
        }

        #endregion

        #region Validation Methods


        #endregion
    }
}
