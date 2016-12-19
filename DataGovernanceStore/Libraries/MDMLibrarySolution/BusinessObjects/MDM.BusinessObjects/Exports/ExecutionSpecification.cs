using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the execution specification object
    /// </summary>
    [DataContract]
    public class ExecutionSpecification : MDMObject, IExecutionSpecification
    {
        #region Fields

        /// <summary>
        /// Field specifying executionsetting collection
        /// </summary>
        private ExecutionSettingCollection _executionSettings = new ExecutionSettingCollection();

        /// <summary>
        /// Field specifying triggering dataspecification
        /// </summary>
        private TriggeringDataSpecification _triggeringDataSpecification = new TriggeringDataSpecification();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies executionsettings collection
        /// </summary>
        [DataMember]
        public ExecutionSettingCollection ExecutionSettings
        {
            get
            {
                return _executionSettings;
            }
            set
            {
                _executionSettings = value;
            }
        }

        /// <summary>
        /// Property specifies triggering dataspecification
        /// </summary>
        [DataMember]
        public TriggeringDataSpecification TriggeringDataSpecification
        {
            get
            {
                return _triggeringDataSpecification;
            }
            set
            {
                _triggeringDataSpecification = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes executionspecification object with default parameters
        /// </summary>
        public ExecutionSpecification() : base() { }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public ExecutionSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadExecutionSpecification(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents executionspecification in Xml format
        /// </summary>
        /// <returns>String representation of current executionspecification object</returns>
        public override String ToXml()
        {
            String executionSpecificationXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //executionSpecification node start
            xmlWriter.WriteStartElement("ExecutionSpecification");

            #region Write execution settings

            if (this.ExecutionSettings != null)
                xmlWriter.WriteRaw(this.ExecutionSettings.ToXml());

            #endregion

            #region Write triggering data specification

            if (this.TriggeringDataSpecification != null)
                xmlWriter.WriteRaw(this.TriggeringDataSpecification.ToXml());

            #endregion

            //executionSpecification node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            executionSpecificationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return executionSpecificationXml;
        }

        /// <summary>
        /// Represents executionspecification in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current executionspecification object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String executionSpecificationXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //executionSpecification node start
                xmlWriter.WriteStartElement("ExecutionSpecification");

                #region Write execution settings

                if (this.ExecutionSettings != null)
                    xmlWriter.WriteRaw(this.ExecutionSettings.ToXml(objectSerialization));

                #endregion

                #region Write triggering data specification

                if (this.TriggeringDataSpecification != null)
                    xmlWriter.WriteRaw(this.TriggeringDataSpecification.ToXml(objectSerialization));

                #endregion

                //executionSpecification node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                executionSpecificationXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            else
            {
                executionSpecificationXml = this.ToXml();
            }

            return executionSpecificationXml;
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
                if (obj is ExecutionSpecification)
                {
                    ExecutionSpecification objectToBeCompared = obj as ExecutionSpecification;

                    if (!this.ExecutionSettings.Equals(objectToBeCompared.ExecutionSettings))
                        return false;

                    if (!this.TriggeringDataSpecification.Equals(objectToBeCompared.TriggeringDataSpecification))
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
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode = base.GetHashCode() ^ this.ExecutionSettings.GetHashCode() ^ this.TriggeringDataSpecification.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the executionspecification with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExecutionSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <ExecutionSpecification>
		            <ExecutionSettings>
			            <ExecutionSetting Name="ExecutionType" Value="" /> <!-- Possible values are "Full" or "Delta". Legacy property name is "Type" -->
			            <ExecutionSetting Name="FirstTimeAsFull" Value="" />
			            <ExecutionSetting Name="FromTime" Value="" />
			            <ExecutionSetting Name="Label" Value="" />
			            <ExecutionSetting Name="StartWithAllCommonAttributes" Value="" />
			            <ExecutionSetting Name="StartWithAllCategoryAttributes" Value="" />
			            <ExecutionSetting Name="StartWithAllSystemAttributes" Value="" />
			            <ExecutionSetting Name="StartWithAllWorkflowAttributes" Value="" />
		            </ExecutionSettings>
		            <TriggeringDataSpecification>
			            <MDMObjectGroups>
				            <MDMObjectGroup ObjectType="EntityMetadata" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects>
						            <MDMObject Id="" Locator="" Include="" MappedName="" />
						            <MDMObject Id="" Locator="" Include="" MappedName="" />
					            </MDMObjects>
				            </MDMObjectGroup>
				            <MDMObjectGroup ObjectType="CommonAtttribute" IncludeAll="" IncludeEmpty="" StartWith="">
					            <MDMObjects />
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
		            </TriggeringDataSpecification>
	            </ExecutionSpecification>
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
                        #region Read execution specification

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExecutionSettings")
                        {
                            // Read execution settings
                            #region Read execution settings
                            String executionSettingsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(executionSettingsXml))
                            {
                                ExecutionSettingCollection executionSettingCollection = new ExecutionSettingCollection(executionSettingsXml);
                                if (executionSettingCollection != null)
                                {
                                    this.ExecutionSettings = executionSettingCollection;
                                }
                            }
                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "TriggeringDataSpecification")
                        {
                            // Read triggering data specification
                            #region Read triggering data specification
                            String triggeringDataSpecificationXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(triggeringDataSpecificationXml))
                            {
                                TriggeringDataSpecification triggeringDataSpecification = new TriggeringDataSpecification(triggeringDataSpecificationXml);
                                if (triggeringDataSpecification != null)
                                {
                                    this.TriggeringDataSpecification = triggeringDataSpecification;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read execution specification
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
