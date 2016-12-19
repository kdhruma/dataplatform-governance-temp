using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the Lookup Export context.
    /// It contains the details about the lookup Export context details.
    /// Such as FileType, Group by , Locale list.
    /// </summary>
    [DataContract]
    public class LookupExportContext : MDMObject, ILookupExportContext
    {
        #region Fields

        /// <summary>
        /// Field denoting the collection of locale list.
        /// </summary>
        private Collection<LocaleEnum> _localeList = new Collection<LocaleEnum>();

        /// <summary>
        /// Field denoting the lookup export group by order.
        /// </summary>
        private LookupExportGroupOrder _groupBy = LookupExportGroupOrder.ColumnName;  

        /// <summary>
        /// Field denoting the lookup export file format.
        /// </summary>  
        private LookupExportFileFormat _fileFormat = LookupExportFileFormat.Unknown;

        /// <summary>
        /// Filed denoting the maximum number of lookups could be exported per file.
        /// By default value will be 50.
        /// Maximum limit is (Excel work sheet limit per file).
        /// Minimum limit is 1.
        /// </summary>
        private Int32 _maxNoOfLookups = 10;

        /// <summary>
        /// Field denoting the maximum number of lookup records could be exported per lookup.
        /// Maximum limit could be 1 million record per lookup
        /// </summary>
        private Int32 _maxRecordsPerLookup = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Represents the parameter less Constructor
        /// </summary>
        public LookupExportContext()
            : base()
        {
        }

        /// <summary>
        /// Represent the Lookup export constructor with 'Lookup export context' as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates the lookup export context as xml format</param>
        public LookupExportContext(String valuesAsXml)
            : base()
        {
            this.LoadLookupExportContext(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents the collection of locale list.
        /// </summary>
        [DataMember]
        public Collection<LocaleEnum> LocaleList
        {
            get
            {
                return _localeList;
            }
            set
            {
                _localeList = value;
            }
        }

        /// <summary>
        /// Represents the lookup export group by order.
        /// </summary>
        [DataMember]
        public LookupExportGroupOrder GroupBy
        {
            get
            {
                return _groupBy;
            }
            set
            {
                _groupBy = value;
            }
        }

        /// <summary>
        /// Represents the lookup export file format.
        /// </summary>  
        [DataMember]
        public LookupExportFileFormat FileFormat
        {
            get
            {
                return _fileFormat;
            }
            set
            {
                _fileFormat = value;
            }
        }

        /// <summary>
        /// Represents the maximum number of lookups could be exported per file.
        /// By default value will be 50.
        /// Maximum limit is 255 (Excel work sheet limit per file).
        /// Minimum limit is 1.
        /// </summary>
        [DataMember]
        public Int32 MaxNoOfLookupsPerFile
        {
            get
            {
                return _maxNoOfLookups;
            }
            set
            {
                _maxNoOfLookups = value;
            }
        }

        /// <summary>
        /// Represents the maximum number of lookup records can be exported per lookup.
        /// Maximum limit could be 1 million record per lookup
        /// </summary>
        [DataMember]
        public Int32 MaxRecordsPerLookup
        {
            get
            {
                return _maxRecordsPerLookup;
            }
            set
            {
                _maxRecordsPerLookup = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean  Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is LookupExportContext)
                {
                    LookupExportContext objectToBeCompared = obj as LookupExportContext;

                    if (this.Id != objectToBeCompared.Id)
                        return false;

                    if (this.Name != objectToBeCompared.Name)
                        return false;

                    if (this.LongName != objectToBeCompared.LongName)
                        return false;

                    if (this.LocaleList != objectToBeCompared.LocaleList)
                        return false;

                    if (this.FileFormat != objectToBeCompared.FileFormat)
                        return false;

                    if (this.GroupBy != objectToBeCompared.GroupBy)
                        return false;

                    if (this.MaxNoOfLookupsPerFile != objectToBeCompared.MaxNoOfLookupsPerFile)
                        return false;

                    if (this.MaxRecordsPerLookup != objectToBeCompared.MaxRecordsPerLookup)
                        return false;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.        
        /// </summary>
        /// <param name="subsetLookupExportContext">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(LookupExportContext subsetLookupExportContext, Boolean compareIds = false)
        {
            if (subsetLookupExportContext != null)
            {
                if (base.IsSuperSetOf(subsetLookupExportContext, compareIds))
                {
                    if (this.MaxNoOfLookupsPerFile != subsetLookupExportContext.MaxNoOfLookupsPerFile)
                        return false;

                    if (this.MaxRecordsPerLookup != subsetLookupExportContext.MaxRecordsPerLookup)
                        return false;

                    if (this.LocaleList != subsetLookupExportContext.LocaleList)
                        return false;

                    if (this.GroupBy != subsetLookupExportContext.GroupBy)
                        return false;

                    if (this.FileFormat != subsetLookupExportContext.FileFormat)
                        return false;

                    return true;

                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.FileFormat.GetHashCode() ^ this.GroupBy.GetHashCode() ^ this.LocaleList.GetHashCode() ^ this.MaxRecordsPerLookup.GetHashCode() ^ this.MaxNoOfLookupsPerFile.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Represents Lookup export context in Xml format
        /// </summary>
        /// <returns>String representation of Lookup export context object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Lookup export context type unique identifier node start
            xmlWriter.WriteStartElement("LookupExportContext");

            xmlWriter.WriteAttributeString("GroupBy", this.GroupBy.ToString());
            xmlWriter.WriteAttributeString("FileFormat", this.FileFormat.ToString());
            xmlWriter.WriteAttributeString("MaxNoOfLookupsPerFile", this.MaxNoOfLookupsPerFile.ToString());
            xmlWriter.WriteAttributeString("MaxRecordsPerLookup", this.MaxRecordsPerLookup.ToString());
            xmlWriter.WriteAttributeString("LocaleList",  ValueTypeHelper.JoinCollection<LocaleEnum>(this.LocaleList, ","));

            //Lookup export context type unique identifier end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Initialize Lookup export context object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for LookupExport object
        /// </param>
        private void LoadLookupExportContext(String valuesAsXml)
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupExportContext")
                    {
                        #region Read Lookup Export context properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("GroupBy"))
                            {
                                LookupExportGroupOrder groupBy = _groupBy;
                                Enum.TryParse<LookupExportGroupOrder>(reader.ReadContentAsString(), out groupBy);

                                this.GroupBy = groupBy;
                            }

                            if (reader.MoveToAttribute("FileFormat"))
                            {
                                 LookupExportFileFormat fileFormat = _fileFormat;
                                 Enum.TryParse<LookupExportFileFormat>(reader.ReadContentAsString(), out fileFormat);

                                this.FileFormat = fileFormat;
                            }

                            if (reader.MoveToAttribute("MaxNoOfLookupsPerFile"))
                            {
                                this.MaxNoOfLookupsPerFile = reader.ReadElementContentAsInt();
                            }

                            if (reader.MoveToAttribute("MaxRecordsPerLookup"))
                            {
                                this.MaxRecordsPerLookup = reader.ReadElementContentAsInt();
                            }

                            if (reader.MoveToAttribute("LocaleList"))
                            {
                                this.LocaleList = ValueTypeHelper.SplitStringToLocaleEnumCollection(reader.ReadContentAsString(), ',');
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion
    }
}
