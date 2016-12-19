using System;
using System.Diagnostics;

namespace MDM.ImportSources.Lookup
{
    using System.Xml;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Imports.Interfaces;
    using MDM.Utility;

    /// <summary>
    /// This class implements the source data from a RS Lookup xml1.0 source.
    /// It reads the data in to a data set and process them as per the request from the Lookup import engine.
    /// </summary>
    public class RSLookupXml10: BaseLookupSource, ILookupImportSourceData
    {
        #region Fields

        private Object readerLock = new Object();

        private LookupCollection _lookupTables = null;

        private XmlReader _lookupXmlReader = null;

        #endregion

        #region Constructors

        public RSLookupXml10(String filePath)
            : base(filePath)
        {
            if (String.IsNullOrEmpty(base.SourceFile))
            {
                throw new ArgumentNullException("RSLookupXml10 file is not available");
            }
        }

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with configuration from the job.
        /// </summary>
        /// <param name="job">Indicates the Job</param>
        /// <param name="lookupImportProfile">Indicates the Import profile</param>
        /// <returns>RSLookupXml10 is initialized then will return true else false</returns>
        public Boolean Initialize(Job job, LookupImportProfile lookupImportProfile)
        {
            if (System.IO.File.Exists(base.SourceFile))
            {
                _lookupXmlReader = XmlReader.Create(base.SourceFile);
            }
            else
            {
                String errorMessage = String.Format("Rs XML file is not available in the specified location {0}", base.SourceFile);
                throw new ArgumentException(errorMessage);
            }

            return true;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get all the lookup data which are waiting to be processed.
        /// </summary>
        /// <param name="application">Indicates the MDM application</param>
        /// <param name="module">Indicates the MDM module</param>
        /// <returns>Return the lookup collection object</returns>
        LookupCollection ILookupImportSourceData.GetAllLookupTables(MDMCenterApplication application, MDMCenterModules module)
        {
            if (_lookupXmlReader == null)
            {
                String errorMessage = String.Format("RS Lookup Import reader is not available for processing. The file is : {0}", base.SourceFile);
                throw new ArgumentException(errorMessage);
            }

            if (_lookupTables == null)
            {
                //Since the batch type is single and at a time one thread will be reading this thread lock may not be 
                //Used but in future in the case of multi threading logic implemented it will be used.
                lock (readerLock)
                {
                    if (_lookupTables == null)
                    {
                        _lookupTables = new LookupCollection();
 
                        while ((_lookupXmlReader.ReadState == ReadState.Initial || _lookupXmlReader.ReadState == ReadState.Interactive))
                        {
                            if (_lookupXmlReader.NodeType == XmlNodeType.Element && _lookupXmlReader.Name == "Table")
                            {
                                String lookupXml = _lookupXmlReader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(lookupXml))
                                {
                                    _lookupTables.Add(new Lookup(lookupXml));
                                }
                            }
                            else
                            {
                                _lookupXmlReader.Read();
                            }
                        }
                        // if we reached the end of the file..close it
                        if (_lookupXmlReader.EOF)
                        {
                            _lookupXmlReader.Close();
                        }
                    }

                }
            }

            return _lookupTables;
        }

        #endregion
    }
}
