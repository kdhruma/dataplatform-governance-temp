using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies configurations for Relationship Denorm Action
    /// </summary>
    [DataContract]
    public class RelationshipDenormAction : MDMObject, IRelationshipDenormAction
    {
        #region Fields

        /// <summary>
        /// Field specifying entity activity list
        /// </summary>
        private EntityActivityList _action = EntityActivityList.RelationshipCreate;

        /// <summary>
        /// Field specifying processingMode of an extension
        /// </summary>
        private ProcessingMode _extensionProcessingMode = ProcessingMode.Async;

        /// <summary>
        /// Field specifying processingMode of hierarchy 
        /// </summary>
        private ProcessingMode _hierarchyProcessingMode = ProcessingMode.Async;

        /// <summary>
        /// Field specifying processingMode of whereused relationships
        /// </summary>
        private ProcessingMode _whereUsedProcessingMode = ProcessingMode.Async;

        /// <summary>
        /// Field specifying processingMode of relationshiptree
        /// </summary>
        private ProcessingMode _relationshipTreeProcessingMode = ProcessingMode.Async;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Specifies activity performed on Entity like AttributeUpdate etc
        /// </summary>
        [DataMember]
        public new EntityActivityList Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Specifies processing mode of Extension of an entity
        /// </summary>
        [DataMember]
        public ProcessingMode ExtensionProcessingMode
        {
            get
            {
                return _extensionProcessingMode;
            }
            set
            {
                _extensionProcessingMode = value;
            }
        }

        /// <summary>
        /// Specifies processing mode of Hierarchy in Entity 
        /// </summary>
        [DataMember]
        public ProcessingMode HierarchyProcessingMode
        {
            get
            {
                return _hierarchyProcessingMode;
            }
            set
            {
                _hierarchyProcessingMode = value;
            }
        }

        /// <summary>
        /// Specifies processing mode of WhereUsed relationships in Entity 
        /// </summary>
        [DataMember]
        public ProcessingMode WhereUsedProcessingMode
        {
            get
            {
                return _whereUsedProcessingMode;
            }
            set
            {
                _whereUsedProcessingMode = value;
            }
        }

        /// <summary>
        /// Specifies processing mode of RelationshipTree in Entity 
        /// </summary>
        [DataMember]
        public ProcessingMode RelationshipTreeProcessingMode
        {
            get
            {
                return _relationshipTreeProcessingMode;
            }
            set
            {
                _relationshipTreeProcessingMode = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipDenormAction() : base() { }

        /// <summary>
        /// Initializes RelationshipDenormAction object with parameters
        /// </summary>
        /// <param name="action"></param>
        /// <param name="extensionProcessingMode"></param>
        /// <param name="hierarchyProcessingMode"></param>
        /// <param name="whereUsedProcessingMode"></param>
        /// <param name="relationshipTreeProcessingMode"></param>
        public RelationshipDenormAction(EntityActivityList action, ProcessingMode extensionProcessingMode, ProcessingMode hierarchyProcessingMode, ProcessingMode whereUsedProcessingMode, ProcessingMode relationshipTreeProcessingMode)
        {
            this._action = action;
            this._extensionProcessingMode = extensionProcessingMode;
            this._hierarchyProcessingMode = hierarchyProcessingMode;
            this._whereUsedProcessingMode = whereUsedProcessingMode;
            this._relationshipTreeProcessingMode = relationshipTreeProcessingMode;
        }

        /// <summary>
        /// Initializes RelationshipDenormAction object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public RelationshipDenormAction(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadRelationshipDenormAction(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents RelationshipDenormAction in Xml format
        /// </summary>
        /// <returns>String representation of current RelationshipDenormAction object</returns>
        public override String ToXml()
        {
            String relationshipDenormActionXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MDM Trace Config Item node start
            xmlWriter.WriteStartElement("RelationshipDenormAction");

            #region Write MDM Trace Config Item properties

            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ExtensionProcessingMode", this.ExtensionProcessingMode.ToString());
            xmlWriter.WriteAttributeString("HierarchyProcessingMode", this.HierarchyProcessingMode.ToString());
            xmlWriter.WriteAttributeString("WhereUsedProcessingMode", this.WhereUsedProcessingMode.ToString());
            xmlWriter.WriteAttributeString("RelationshipTreeProcessingMode", this.RelationshipTreeProcessingMode.ToString());

            #endregion

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            relationshipDenormActionXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return relationshipDenormActionXml;
        }

        /// <summary>
        /// Represents RelationshipDenormAction in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current RelationshipDenormAction object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String relationshipDenormActionXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            relationshipDenormActionXml = this.ToXml();

            return relationshipDenormActionXml;
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
                if (obj is RelationshipDenormAction)
                {
                    RelationshipDenormAction objectToBeCompared = obj as RelationshipDenormAction;

                    if (!this.Action.Equals(objectToBeCompared.Action))
                        return false;

                    if (!this.ExtensionProcessingMode.Equals(objectToBeCompared.ExtensionProcessingMode))
                        return false;

                    if (!this.HierarchyProcessingMode.Equals(objectToBeCompared.HierarchyProcessingMode))
                        return false;

                    if (!this.WhereUsedProcessingMode.Equals(objectToBeCompared.WhereUsedProcessingMode))
                        return false;

                    if (!this.RelationshipTreeProcessingMode.Equals(objectToBeCompared.RelationshipTreeProcessingMode))
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
            hashCode = base.GetHashCode() ^ this.Locale.GetHashCode() ^ this.Action.GetHashCode() ^ this.ExtensionProcessingMode.GetHashCode()
                        ^ this.HierarchyProcessingMode.GetHashCode() ^ this.WhereUsedProcessingMode.GetHashCode() ^ this.RelationshipTreeProcessingMode.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the relationship denorm action with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadRelationshipDenormAction(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
             * <RelationshipDenormAction Action="RelationshipCreate" ExtensionProcessingMode="ASync" HierarchyProcessingMode="ASync" WhereUsedProcessingMode="ASync" 
             * RelationshipTreeProcessingMode="ASync" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipDenormAction")
                        {
                            #region Read RelationshipDenormAction

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Action"))
                                {
                                    EntityActivityList action = EntityActivityList.Any;
                                    Enum.TryParse<EntityActivityList>(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }

                                if (reader.MoveToAttribute("ExtensionProcessingMode"))
                                {
                                    ProcessingMode processingMode = MDM.Core.ProcessingMode.Async;
                                    Enum.TryParse<ProcessingMode>(reader.ReadContentAsString(), true, out processingMode);
                                    this.ExtensionProcessingMode = processingMode;
                                }

                                if (reader.MoveToAttribute("HierarchyProcessingMode"))
                                {
                                    ProcessingMode processingMode = MDM.Core.ProcessingMode.Async;
                                    Enum.TryParse<ProcessingMode>(reader.ReadContentAsString(), true, out processingMode);
                                    this.HierarchyProcessingMode = processingMode;
                                }

                                if (reader.MoveToAttribute("WhereUsedProcessingMode"))
                                {
                                    ProcessingMode processingMode = MDM.Core.ProcessingMode.Async;
                                    Enum.TryParse<ProcessingMode>(reader.ReadContentAsString(), true, out processingMode);
                                    this.WhereUsedProcessingMode = processingMode;
                                }

                                if (reader.MoveToAttribute("RelationshipTreeProcessingMode"))
                                {
                                    ProcessingMode processingMode = MDM.Core.ProcessingMode.Async;
                                    Enum.TryParse<ProcessingMode>(reader.ReadContentAsString(), true, out processingMode);
                                    this.RelationshipTreeProcessingMode = processingMode;
                                }

                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
                        }
                        else
                        {
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
        }

        #endregion

    }
}
