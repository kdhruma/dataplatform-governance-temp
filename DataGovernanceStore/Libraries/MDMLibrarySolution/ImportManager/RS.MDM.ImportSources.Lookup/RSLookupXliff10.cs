//using System;
//using System.Diagnostics;

//namespace MDM.ImportSources.Lookup
//{
//    using System.Xml;
//    using MDM.BusinessObjects;
//    using MDM.BusinessObjects.Imports;
//    using MDM.BusinessObjects.Jobs;
//    using MDM.Core;
//    using MDM.Imports.Interfaces;
//    using MDM.Utility;
//    using MDM.TranslationUtility;
//    using System.Xml.Schema;
//    using System.IO;

//    /// <summary>
//    /// This class implements the source data from a RS Lookup xliff1.0 source.
//    /// It reads the data in to a data set and process them as per the request from the Lookup import engine.
//    /// </summary>
//    public class RSLookupXliff10: BaseLookupSource, ILookupImportSourceData
//    {
//        #region Fields

//        private Object readerLock = new Object();

//        private LookupCollection _lookupTables = null;

//        private XmlReader _rsXliffReader = null;     

//        private String _schemaFilePath = String.Empty;

//        private String errorMessage = String.Empty;

//        private Int32 errorCount = 0;

//        private Int32 warningCount = 0;

//        #endregion

//        #region Constructors

//        public RSLookupXliff10(String filePath)
//            : base(filePath)
//        {
//            if (String.IsNullOrEmpty(base.SourceFile))
//            {
//                throw new ArgumentNullException("RSLookupXliff10 file is not available");
//            }
//        }

//        /// <summary>
//        /// Provide an opportunity for the source data to initializes itself with configuration from the job.
//        /// </summary>
//        /// <param name="job">Indicates the Job</param>
//        /// <param name="lookupImportProfile">Indicates the Import profile</param>
//        /// <returns>RSLookupXliff10 is initialized then will return true else false</returns>
//        public Boolean Initialize(Job job, LookupImportProfile lookupImportProfile)
//        {
//            //lookupImportProfile.LookupJobProcessingOptions.
//            if (System.IO.File.Exists(base.SourceFile))
//            {
//                if (lookupImportProfile.LookupJobProcessingOptions.ValidateSchema)
//                {
//                    JobParameter schemaFilePathParameter = job.JobData.JobParameters["SchemaFilePath"];

//                    if (schemaFilePathParameter == null)
//                    {
//                        throw new ArgumentNullException("Rs Xliff Schema file path parameter is not set by the import engine.");
//                    }

//                    _schemaFilePath = schemaFilePathParameter.Value;

//                    if (String.IsNullOrEmpty(_schemaFilePath))
//                    {
//                        throw new ArgumentNullException("Rs Xliff Schema file path parameter set by import engine is empty.");
//                    }

//                    FileInfo fileInfo = new FileInfo(_schemaFilePath);

//                    if (!fileInfo.Exists)
//                    {
//                        throw new ArgumentNullException("Rs Xliff Schema file path parameter set by import engine is not valid.");
//                    }

//                    if (!ValidateSchema(base.SourceFile))
//                        return false;
//                }
//                _rsXliffReader = XmlReader.Create(base.SourceFile);
//            }
//            else
//            {
//                String errorMessage = String.Format("Rs Xliff file is not available in the specified location {0}", base.SourceFile);
//                throw new ArgumentException(errorMessage);
//            }

//            return true;
//        }

//        #endregion

//        #region Properties

//        #endregion

//        #region Methods

//        /// <summary>
//        /// Get all the lookup data which are waiting to be processed.
//        /// </summary>
//        /// <param name="application">Indicates the MDM application</param>
//        /// <param name="module">Indicates the MDM module</param>
//        /// <returns>Return the lookup collection object</returns>
//        LookupCollection ILookupImportSourceData.GetAllLookupTables(MDMCenterApplication application, MDMCenterModules module)
//        {
           
//            if (_rsXliffReader == null)
//            {
//                String errorMessage = String.Format("RS Lookup Import reader is not available for processing. The file is : {0}", base.SourceFile);
//                throw new ArgumentException(errorMessage);
//            }
            
//            if (_lookupTables == null)
//            {
//                //Since the batch type is single and at a time one thread will be reading this thread lock may not be 
//                //Used but in future in the case of multi threading logic implemented it will be used.
//                lock (readerLock)
//                {
//                    if (_lookupTables == null)
//                    {
//                        _lookupTables = new LookupCollection();
//                        Lookup lookup = null;

//                        while ((_rsXliffReader.ReadState == ReadState.Initial || _rsXliffReader.ReadState == ReadState.Interactive))
//                        {
//                            if (_rsXliffReader.NodeType == XmlNodeType.Element && _rsXliffReader.Name == "file")
//                            {
//                                String lookupXliff = _rsXliffReader.ReadOuterXml();

//                                if (!String.IsNullOrWhiteSpace(lookupXliff))
//                                {
//                                    lookup = TranslationHelper.ConvertRSXliff1_0ToLookup(lookupXliff);

//                                    if (lookup != null)
//                                    {
//                                        _lookupTables.Add(lookup);
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                _rsXliffReader.Read();
//                            }
//                        }
//                        // if we reached the end of the file..close it
//                        if (_rsXliffReader.EOF)
//                        {
//                            _rsXliffReader.Close();
//                        }
//                    }

//                }
//            }

//            return _lookupTables;
//        }

//        #region Validation Methods

//        private bool ValidateSchema(String filePath)
//        {
//            XmlReaderSettings settings = new XmlReaderSettings();
//            settings.Schemas.Add("http://www.riversand.com/schemas", _schemaFilePath);
//            settings.Schemas.Compile();
//            settings.ValidationType = ValidationType.Schema;
//            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
//            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(rsXliff_ValidationEventHandler);

//            //Create the schema validating reader.
//            XmlReader schemaReader = null;
//            try
//            {
//                schemaReader = XmlReader.Create(filePath, settings);
//            }
//            catch
//            {
//                throw;
//            }

//            // just read through the document...
//            while (schemaReader.Read()) { }

//            //Close the reader.
//            schemaReader.Close();

//            if (warningCount > 0)
//            {
//                // the schema information is missing.
//                throw new Exception(String.Format("Schema validation was requested but the RSXliff file {0} does not have the schema header information.", filePath));
//            }

//            if (errorCount > 0)
//            {
//                // the schema information is missing.
//                throw new Exception(String.Format("The RSXliff file {0} failed schema validation. The errors are {1}.", filePath, errorMessage));
//            }
//            return true;
//        }

//        private void rsXliff_ValidationEventHandler(object sender, ValidationEventArgs args)
//        {
//            if (args.Severity == XmlSeverityType.Warning)
//            {
//                warningCount++;
//            }
//            else
//            {
//                errorMessage = errorMessage + args.Message + "\r\n";
//                errorCount++;
//            }
//        }

//        #endregion
//        #endregion
//    }
//}
