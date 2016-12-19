using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;

    /// <summary>
    /// Specifies the triggering data specification object
    /// </summary>
    [DataContract]
    public class TriggeringDataSpecification : MDMObject, ITriggeringDataSpecification
    {
        #region Fields

        /// <summary>
        /// Field specifying mdmobject groups collection
        /// </summary>
        private MDMObjectGroupCollection _mdmObjectGroups = new MDMObjectGroupCollection();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies mdmobjectGroups collection
        /// </summary>
        [DataMember]
        public MDMObjectGroupCollection MDMObjectGroups
        {
            get
            {
                return _mdmObjectGroups;
            }
            set
            {
                _mdmObjectGroups = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes triggeringdataspecification object with default parameters
        /// </summary>
        public TriggeringDataSpecification() : base() { }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public TriggeringDataSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadTriggeringDataSpecification(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public IMDMObjectGroup GetMDMObjectGroup(ObjectType objectType)
        {
            return this.MDMObjectGroups.GetMDMObjectGroup(objectType);
        }

        /// <summary>
        /// Represents triggeringdataspecification in Xml format
        /// </summary>
        /// <returns>String representation of current triggeringdataspecification object</returns>
        public override String ToXml()
        {
            String triggeringDataSpecificationXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //TriggeringDataSpecification Item node start
            xmlWriter.WriteStartElement("TriggeringDataSpecification");

            #region Write TriggeringDataSpecification

            if (this.MDMObjectGroups != null)
                xmlWriter.WriteRaw(this.MDMObjectGroups.ToXml());

            #endregion

            //TriggeringDataSpecification Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            triggeringDataSpecificationXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return triggeringDataSpecificationXml;
        }

        /// <summary>
        /// Represents triggeringdataspecification in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current triggeringdataspecification object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String triggeringDataSpecificationXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            if (objectSerialization != ObjectSerialization.Full)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //TriggeringDataSpecification Item node start
                xmlWriter.WriteStartElement("TriggeringDataSpecification");

                #region Write TriggeringDataSpecification

                if (this.MDMObjectGroups != null)
                    xmlWriter.WriteRaw(this.MDMObjectGroups.ToXml(objectSerialization));

                #endregion

                //TriggeringDataSpecification Item node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                triggeringDataSpecificationXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();

            }
            else
            {
                triggeringDataSpecificationXml = this.ToXml();
            }

            return triggeringDataSpecificationXml;
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
                if (obj is TriggeringDataSpecification)
                {
                    TriggeringDataSpecification objectToBeCompared = obj as TriggeringDataSpecification;

                    if (!this.MDMObjectGroups.Equals(objectToBeCompared.MDMObjectGroups))
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
            hashCode = base.GetHashCode() ^ this.MDMObjectGroups.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the triggeringdataspecification with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadTriggeringDataSpecification(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
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
                        #region Read TriggeringDataSpecification

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjectGroups")
                        {
                            // Read MDMObjectGroups
                            #region Read MDMObjectGroups
                            String mdmObjectGroupsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(mdmObjectGroupsXml))
                            {
                                MDMObjectGroupCollection mdmObjectGroupCollection = new MDMObjectGroupCollection(mdmObjectGroupsXml);
                                if (mdmObjectGroupCollection != null)
                                {
                                    this.MDMObjectGroups = mdmObjectGroupCollection;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read TriggeringDataSpecification
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
