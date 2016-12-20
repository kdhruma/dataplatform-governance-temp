using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Class describes result of merging
    /// </summary>
    [DataContract]
    public class MergeOperationResult : OperationResult, IMergeOperationResult
    {
        #region Fields

        private String _workflowName;
        private Int64? _finalEntityId;
        private Int32? _relationshipTypeId;

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the workflow name
        /// </summary>
        [DataMember]
        public String WorkflowName
        {
            get
            {
                return _workflowName;
            }
            set
            {
                _workflowName = value;
            }
        }

        /// <summary>
        /// Property denoting the merged or created entity id
        /// </summary>
        [DataMember]
        public Int64? FinalEntityId
        {
            get
            {
                return _finalEntityId;
            }
            set
            {
                _finalEntityId = value;
            }
        }

        /// <summary>
        /// Property denoting the relationship type id
        /// </summary>
        [DataMember]
        public Int32? RelationshipTypeId
        {
            get
            {
                return _relationshipTypeId;
            }
            set
            {
                _relationshipTypeId = value;
            }
        }

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current OperationResult</param>
        public new void LoadOperationResult(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                        {
                            //Read error
                            #region Read error

                            String errorXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(errorXml))
                            {
                                Error error = new Error(errorXml);
                                if (error != null)
                                {
                                    if (this.Errors == null)
                                    {
                                        this.Errors = new ErrorCollection();
                                    }
                                    this.Errors.Add(error);
                                }
                            }

                            #endregion Read error
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Information")
                        {
                            //Read Information
                            #region Read Information

                            String infoXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(infoXml))
                            {
                                Information info = new Information(infoXml);
                                if (info != null)
                                {
                                    if (this.Informations == null)
                                    {
                                        this.Informations = new InformationCollection();
                                    }
                                    this.Informations.Add(info);
                                }
                            }

                            #endregion Read Information
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Warning")
                        {
                            //Read Warning
                            #region Read Warning

                            String warningXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(warningXml))
                            {
                                Warning warning = new Warning(warningXml);
                                if (warning != null)
                                {
                                    if (this.Warnings == null)
                                    {
                                        this.Warnings = new WarningCollection();
                                    }
                                    this.Warnings.Add(warning);
                                }
                            }

                            #endregion Read Warning
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ReturnValue")
                        {
                            //Read ReturnValues
                            #region Read ReturnValues

                            if (this.ReturnValues == null)
                            {
                                this.ReturnValues = new Collection<object>();
                            }

                            this.ReturnValues.Add(reader.ReadInnerXml());

                            #endregion Read ReturnValues
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "OperationResultStatus")
                        {
                            OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                            Enum.TryParse(reader.ReadElementContentAsString(), out operationResultStatus);
                            this.OperationResultStatus = operationResultStatus;
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowName")
                        {
                            WorkflowName = reader.ReadElementContentAsString();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeId")
                        {
                            RelationshipTypeId = ValueTypeHelper.ConvertToNullableInt32(reader.ReadElementContentAsString());
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "FinalEntityId")
                        {
                            FinalEntityId = ValueTypeHelper.ConvertToNullableInt64(reader.ReadElementContentAsString());
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

        /// <summary>
        /// Get Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        public new String ToXml()
        {
            String operationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("OperationResult");

            #region Write Errors

            //Add Error Nodes
            xmlWriter.WriteStartElement("Errors");

            foreach (Error error in this.Errors)
            {
                xmlWriter.WriteRaw(error.ToXml());
            }

            //Error nodes end
            xmlWriter.WriteEndElement();

            #endregion Write Errors

            #region Write Information

            //Add Information Nodes
            xmlWriter.WriteStartElement("Informations");

            foreach (Information information in this.Informations)
            {
                xmlWriter.WriteRaw(information.ToXml());
            }

            //Information node end
            xmlWriter.WriteEndElement();

            #endregion Write Information

            #region Write Warnings

            //Add Warning Nodes
            xmlWriter.WriteStartElement("Warnings");

            foreach (Warning warnings in this.Warnings)
            {
                xmlWriter.WriteRaw(warnings.ToXml());
            }

            //Warning node end
            xmlWriter.WriteEndElement();

            #endregion Write Information

            #region Write return values

            //Add Return Values Nodes
            xmlWriter.WriteStartElement("ReturnValues");

            foreach (Object returnValue in this.ReturnValues)
            {
                xmlWriter.WriteStartElement("ReturnValue");
                xmlWriter.WriteRaw(returnValue.ToString());
                xmlWriter.WriteEndElement();
            }

            //Return values node end
            xmlWriter.WriteEndElement();

            #endregion Write return values

            #region Write Operation Result Status

            //Add Return Values Nodes
            xmlWriter.WriteStartElement("OperationResultStatus");
            xmlWriter.WriteRaw(OperationResultStatus.ToString());
            //Return values node end
            xmlWriter.WriteEndElement();

            #endregion Write return values

            #region Write WorkflowName

            xmlWriter.WriteStartElement("WorkflowName");
            xmlWriter.WriteRaw(WorkflowName);
            xmlWriter.WriteEndElement();

            #endregion Write WorkflowName

            #region Write RelationshipTypeId

            if (RelationshipTypeId.HasValue)
            {
                xmlWriter.WriteStartElement("RelationshipTypeId");
                xmlWriter.WriteRaw(RelationshipTypeId.ToString());
                xmlWriter.WriteEndElement();
            }

            #endregion Write RelationshipTypeId

            #region Write FinalEntityId

            if (FinalEntityId.HasValue)
            {
                xmlWriter.WriteStartElement("FinalEntityId");
                xmlWriter.WriteRaw(FinalEntityId.ToString());
                xmlWriter.WriteEndElement();
            }

            #endregion Write FinalEntityId

            #region write Extended properties
            xmlWriter.WriteStartElement("ExtendedProperties");

            if (this.ExtendedProperties != null)
            {
                foreach (String key in this.ExtendedProperties.Keys)
                {
                    //ExtendedProperty node start
                    xmlWriter.WriteStartElement("ExtendedProperty");

                    xmlWriter.WriteAttributeString("Key", key);
                    if (this.ExtendedProperties[key] != null)
                    {
                        xmlWriter.WriteCData(this.ExtendedProperties[key].ToString());
                    }
                    //ExtendedProperty node end
                    xmlWriter.WriteEndElement();
                }
            }
            //ExtendedProperties node end
            xmlWriter.WriteEndElement();
            #endregion write Extended properties

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            operationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return operationResultXml;
        }

        /// <summary>
        /// Get Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        public new String ToXml(ObjectSerialization serialization)
        {
            String operationResultXml = String.Empty;

            if (serialization == ObjectSerialization.Full)
            {
                operationResultXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Operation result node start
                xmlWriter.WriteStartElement("OperationResult");

                #region Write Errors

                //Add Error Nodes
                xmlWriter.WriteStartElement("Errors");

                if (this.Errors != null)
                {
                    foreach (Error error in this.Errors)
                    {
                        xmlWriter.WriteRaw(error.ToXml(serialization));
                    }
                }

                //Error nodes end
                xmlWriter.WriteEndElement();

                #endregion Write Errors

                #region Write Information

                //Add Information Nodes
                xmlWriter.WriteStartElement("Informations");

                if (this.Informations != null)
                {
                    foreach (Information information in this.Informations)
                    {
                        xmlWriter.WriteRaw(information.ToXml(serialization));
                    }
                }

                //Information node end
                xmlWriter.WriteEndElement();

                #endregion Write Information

                #region Write Warnings

                //Add Warning Nodes
                xmlWriter.WriteStartElement("Warnings");

                foreach (Warning warnings in this.Warnings)
                {
                    xmlWriter.WriteRaw(warnings.ToXml(serialization));
                }

                //Warning node end
                xmlWriter.WriteEndElement();

                #endregion Write Information

                #region Write return values

                //Add Return Values Nodes
                xmlWriter.WriteStartElement("ReturnValues");

                if (this.ReturnValues != null)
                {
                    foreach (Object returnValue in this.ReturnValues)
                    {
                        xmlWriter.WriteStartElement("ReturnValue");
                        xmlWriter.WriteRaw(returnValue.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }

                //Return values node end
                xmlWriter.WriteEndElement();

                #endregion Write return values

                #region Write WorkflowName

                xmlWriter.WriteStartElement("WorkflowName");
                xmlWriter.WriteRaw(WorkflowName);
                xmlWriter.WriteEndElement();

                #endregion Write WorkflowName

                #region Write RelationshipTypeId

                if (RelationshipTypeId.HasValue)
                {
                    xmlWriter.WriteStartElement("RelationshipTypeId");
                    xmlWriter.WriteRaw(RelationshipTypeId.ToString());
                    xmlWriter.WriteEndElement();
                }

                #endregion Write RelationshipTypeId

                #region Write FinalEntityId

                if (FinalEntityId.HasValue)
                {
                    xmlWriter.WriteStartElement("FinalEntityId");
                    xmlWriter.WriteRaw(FinalEntityId.ToString());
                    xmlWriter.WriteEndElement();
                }

                #endregion Write FinalEntityId

                #region Write Operation Result Status

                //Add OperationResultStatus Nodes
                xmlWriter.WriteStartElement("OperationResultStatus");
                xmlWriter.WriteRaw(OperationResultStatus.ToString());
                //OperationResultStatus node end
                xmlWriter.WriteEndElement();

                #endregion Write return values

                //Operation result node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                operationResultXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            return operationResultXml;
        }

        #endregion
    }
}
