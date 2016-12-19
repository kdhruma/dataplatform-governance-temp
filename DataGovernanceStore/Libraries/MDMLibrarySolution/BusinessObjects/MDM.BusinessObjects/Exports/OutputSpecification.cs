using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;

    /// <summary>
    /// Specifies the output specification object
    /// </summary>
    [DataContract]
    public class OutputSpecification : MDMObject, IOutputSpecification
    {
        #region Fields

        /// <summary>
        /// Field specifying data formatter
        /// </summary>
        private DataFormatter _dataFormatter = new DataFormatter();

        /// <summary>
        /// Field specifying collection of data subscribers
        /// </summary>
        private DataSubscriberCollection _dataSubscribers = new DataSubscriberCollection();

        /// <summary>
        /// Field specifying output dataspecification
        /// </summary>
        private OutputDataSpecification _outputDataSpecification = new OutputDataSpecification();

        /// <summary>
        /// Fiel specifying the lookup export masks
        /// </summary>
        private LookupExportScopeCollection _lookupExportScopes = new LookupExportScopeCollection();


        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies collection of data formatters
        /// </summary>
        [DataMember]
        public DataFormatter DataFormatter
        {
            get
            {
                return _dataFormatter;
            }
            set
            {
                _dataFormatter = value;
            }
        }

        /// <summary>
        /// Property specifies collection of data subscribers
        /// </summary>
        [DataMember]
        public DataSubscriberCollection DataSubscribers
        {
            get
            {
                return _dataSubscribers;
            }
            set
            {
                _dataSubscribers = value;
            }
        }

        /// <summary>
        /// Property specifies output dataspecification
        /// </summary>
        [DataMember]
        public OutputDataSpecification OutputDataSpecification
        {
            get
            {
                return _outputDataSpecification;
            }
            set
            {
                _outputDataSpecification = value;
            }
        }

        /// <summary>
        /// Represents the Export Mask for lookup attributes
        /// </summary>
        [DataMember]
        public LookupExportScopeCollection ExportMasks
        {
            get
            {
                return _lookupExportScopes;
            }
            set
            {
                _lookupExportScopes = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes outputspecification object with default parameters
        /// </summary>
        public OutputSpecification() : base() { }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public OutputSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadOutputSpecification(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents outputspecification in Xml format
        /// </summary>
        /// <returns>String representation of current outputspecification object</returns>
        public override String ToXml()
        {
            String outputSpecificationXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //outputSpecification node start
            xmlWriter.WriteStartElement("OutputSpecification");

            #region Write data formatters

            if (this.DataFormatter != null)
                xmlWriter.WriteRaw(this.DataFormatter.ToXml());

            #endregion

            #region Write data subscribers

            if (this.DataSubscribers != null)
                xmlWriter.WriteRaw(this.DataSubscribers.ToXml());

            #endregion

            #region Write outputdata specification

            if (this.OutputDataSpecification != null)
                xmlWriter.WriteRaw(this.OutputDataSpecification.ToXml());

            #endregion

            //outputSpecification node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            outputSpecificationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return outputSpecificationXml;
        }

        /// <summary>
        /// Represents outputspecification in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current outputspecification object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String outputSpecificationXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                    {

                        //outputSpecification node start
                        xmlWriter.WriteStartElement("OutputSpecification");

                        #region Write data formatters

                        if (this.DataFormatter != null)
                        {
                            xmlWriter.WriteRaw(this.DataFormatter.ToXml(objectSerialization));
                        }

                        #endregion

                        #region Write data subscribers

                        if (this.DataSubscribers != null)
                        {
                            xmlWriter.WriteRaw(this.DataSubscribers.ToXml(objectSerialization));
                        }

                        #endregion

                        #region Write outputdata specification

                        if (this.OutputDataSpecification != null)
                        {
                            xmlWriter.WriteRaw(this.OutputDataSpecification.ToXml(objectSerialization));
                        }

                        #endregion

                        #region Write export mask (Lookup scope) specification

                        if (this.ExportMasks != null)
                        {
                            xmlWriter.WriteRaw(this.ExportMasks.ToXml());
                        }

                        #endregion

                        //outputSpecification node end
                        xmlWriter.WriteEndElement();

                        xmlWriter.Flush();

                        //get the actual XML
                        outputSpecificationXml = sw.ToString();
                    }
                }
            }
            else
            {
                outputSpecificationXml = this.ToXml();
            }

            return outputSpecificationXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is OutputSpecification)
                {
                    OutputSpecification objectToBeCompared = obj as OutputSpecification;

                    if (!this.DataFormatter.Equals(objectToBeCompared.DataFormatter))
                    {
                        return false;
                    }

                    if (!this.DataSubscribers.Equals(objectToBeCompared.DataSubscribers))
                    {
                        return false;
                    }

                    if (!this.OutputDataSpecification.Equals(objectToBeCompared.OutputDataSpecification))
                    {
                        return false;
                    }

                    if (!this.ExportMasks.Equals(objectToBeCompared.ExportMasks))
                    {
                        return false;
                    }

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode = base.GetHashCode() ^ this.DataFormatter.GetHashCode() ^ this.DataSubscribers.GetHashCode() ^ this.OutputDataSpecification.GetHashCode() ^ ExportMasks.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the outputspecification with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadOutputSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <OutputSpecification>
			        <DataFormatter Id="" Name="" Type="" AttributeColumnHeaderFormat="" ApplyExportMaskToLookupAttribute="" CategoryPathType=""></DataFormatter>
		            <DataSubscribers>
			            <DataSubscriber Id="" Name="" Location="" FileName=""></DataSubscriber>
		            </DataSubscribers>
		            <OutputDataSpecification>
			            <MDMObjectGroups>
				            <MDMObjectGroup ObjectType="CommonAtttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects>
						            <MDMObject Id="" Locator="" Include="" MappedName="" />
						            <MDMObject Id="" Locator="" Include="" MappedName="" />
					            </MDMObjects>
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="CategoryAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="SystemAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="WorkflowAttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="EntityType" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="RelationshipType" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="RelationshipAttributes" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="Locale" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
				            </MDMObjectGroup>
			            </MDMObjectGroups>
		            </OutputDataSpecification>
	            </OutputSpecification>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        #region Read output specification

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataFormatter")
                        {
                            // Read data formatters
                            #region Read data formatters
                            String dataFormatterXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dataFormatterXml))
                            {
                                DataFormatter dataFormatter = new DataFormatter(dataFormatterXml);
                                if (dataFormatter != null)
                                {
                                    this.DataFormatter = dataFormatter;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataSubscribers")
                        {
                            // Read data subscribers
                            #region Read data subscribers
                            String dataSubscriberXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(dataSubscriberXml))
                            {
                                DataSubscriberCollection dataSubscriberCollection = new DataSubscriberCollection(dataSubscriberXml);
                                if (dataSubscriberCollection.Any())
                                {
                                    this.DataSubscribers = dataSubscriberCollection;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "OutputDataSpecification")
                        {
                            // Read data subscribers
                            #region Read data subscribers
                            String outputDataSpecificationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(outputDataSpecificationXml))
                            {
                                OutputDataSpecification outputDataSpecification = new OutputDataSpecification(outputDataSpecificationXml);
                                if (outputDataSpecification != null)
                                {
                                    this.OutputDataSpecification = outputDataSpecification;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupExportScopes")
                        {
                            // Read Lookup export scopes 
                            #region Read Lookup export scopes
                            String lookupExportScopeXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(lookupExportScopeXml))
                            {
                                LookupExportScopeCollection lookupExportScopeCollection = new LookupExportScopeCollection(lookupExportScopeXml);
                                if (lookupExportScopeCollection.LookupExportScopes.Any())
                                {
                                    this.ExportMasks = lookupExportScopeCollection;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read Notification
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
        }

        #endregion Private Methods
    }
}
