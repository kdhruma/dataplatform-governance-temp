using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies results for Entity operations
    /// </summary>
    [DataContract]
    public class AttributeOperationResult : OperationResult, IAttributeOperationResult
    {
        #region Fields

        /// <summary>
        /// Field denoting the id of the attribute for which results are created
        /// </summary>
        private Int32 _attributeId = 0;

		/// <summary>
        /// Field denoting the short name of the attribute for which results are created
        /// </summary>
        private String _attributeShortName = String.Empty;

        /// <summary>
        /// Field denoting the long name of the attribute for which results are created
        /// </summary>
        private String _attributeLongName = String.Empty;

        /// <summary>
        /// Field denoting Locale of an attribute. There can be an attribute with different locale.
        /// </summary>
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting the model type of an attribute. It indicates whether an attribute is common attribute or technical attribute.
        /// For possible values, see <see cref="AttributeModelType" />
        /// </summary>
        private AttributeModelType _attributeModelType = AttributeModelType.All;
        

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the Operation Result
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "AttributeOperationResult";
            }
        }

        /// <summary>
        /// Property denoting the id of the attribute for which results are created
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get
            {
                return _attributeId;
            }
            set
            {
                _attributeId = value;
            }
        }

        /// <summary>
        /// Property denoting the short name of the attribute for which results are created
        /// </summary>
        [DataMember]
        public String AttributeShortName
        {
            get 
            { 
                return this._attributeShortName; 
            }
            set 
            { 
                this._attributeShortName = value; 
            }
        }

        /// <summary>
        /// Property denoting the long name of the attribute for which results are created
        /// </summary>
        [DataMember]
        public String AttributeLongName
        {
            get
            {
                return _attributeLongName;
            }
            set
            {
                _attributeLongName = value;
            }
        }

        /// <summary>
        /// Property denoting Locale of an attribute. There can be an attribute with different locale.
        /// </summary>
        [DataMember]
        public LocaleEnum Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        /// <summary>
        /// Property denoting the model type of an attribute. It indicates whether an attribute is common attribute or technical attribute.
        /// For possible values, see <see cref="AttributeModelType" />
        /// </summary>
        [DataMember]
        public AttributeModelType AttributeModelType
        {
            get
            {
                return this._attributeModelType;
            }
            set
            {
                this._attributeModelType = value;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new instance of attribute operation result
        /// </summary>
        public AttributeOperationResult()
        {

        }

        /// <summary>
        /// Initializes new instance of attribute operation result with attribute id
        /// </summary>
        /// <param name="attributeId">Id of the attribute</param>
        /// <param name="attributeShortName"> Short name of the attribute </param>
        /// <param name="attributeLongName"> Long name of the attribute </param>
        /// <param name="attributeModelType"> attribute Model Type </param>
        /// <param name="locale"> Locale for the attribute</param>
        public AttributeOperationResult(Int32 attributeId, String attributeShortName, String attributeLongName, AttributeModelType attributeModelType, LocaleEnum locale)
        {
            this.AttributeId = attributeId;
            this.AttributeShortName = attributeShortName;
            this.AttributeLongName = attributeLongName;
            this.AttributeModelType = attributeModelType;
            this.Locale = locale;
        }

        /// <summary>
        /// Initialize new instance of attribute operation result from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for attribute operation result</param>
        public AttributeOperationResult(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current OperationResult
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <AttributeOperationResult Id="9812" Name="description" LongName="Description" Status="Failed">
        ///     <Errors>
        ///         <Error Code="" Message = "Required Attributes are not filled"/>
        ///     </Errors>
        ///     <Informations>
        ///         <Information Code="" Message = ""/>
        ///     </Informations>
        ///     <ReturnValues />
        /// </AttributeOperationResult>
        /// ]]>
        /// </para>
        /// </param>
        public new void LoadOperationResult(String valuesAsXml)
        {
            #region Sample Xml
            /*
            <AttributeOperationResult Id="9812" Status="Failed" Locale="en_WW">
              <Errors>
                 <Error Code="" Message = "Required Attributes are not filled"/>
              </Errors>
              <Informations>
                 <Information Code="" Message = ""/>
              </Informations>
              <ReturnValues />
            </AttributeOperationResult>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeOperationResult")
                        {
                            #region Read AttributeOperationResult attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.AttributeId = reader.ReadContentAsInt();
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.AttributeShortName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.AttributeLongName = reader.ReadContentAsString();
                                }

                                if(reader.MoveToAttribute("Locale"))
                                {
                                    String strLocale = reader.ReadContentAsString();
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(strLocale, out locale);
                                    this.Locale = locale;
                                }

                                if (reader.MoveToAttribute("AttributeModelType"))
                                {
                                    String strAttributeModelType = reader.ReadContentAsString();
                                    AttributeModelType attributeModelType = AttributeModelType.All;

                                    if (!String.IsNullOrWhiteSpace(strAttributeModelType))
                                        Enum.TryParse<AttributeModelType>(strAttributeModelType, true, out attributeModelType);

                                    this.AttributeModelType = attributeModelType;
                                }

                                if (reader.MoveToAttribute("Status"))
                                {
                                    OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                                    String strOperationResultStatus = reader.ReadContentAsString();

                                    if (!String.IsNullOrWhiteSpace(strOperationResultStatus))
                                        Enum.TryParse<OperationResultStatusEnum>(strOperationResultStatus, true, out operationResultStatus);

                                    this.OperationResultStatus = operationResultStatus;
                                }
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                        {
                            //Read error
                            #region Read error

                            String errorXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(errorXml))
                            {
                                Error error = new Error(errorXml);

                                if (error != null)
                                {
                                    if (this.Errors == null)
                                        this.Errors = new ErrorCollection();

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

                            if (!String.IsNullOrWhiteSpace(infoXml))
                            {
                                Information info = new Information(infoXml);

                                if (info != null)
                                {
                                    if (this.Informations == null)
                                        this.Informations = new InformationCollection();

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

                            if (!String.IsNullOrWhiteSpace(warningXml))
                            {
                                Warning warn = new Warning(warningXml);

                                if (warn != null)
                                {
                                    if (this.Warnings == null)
                                        this.Warnings = new WarningCollection();

                                    this.Warnings.Add(warn);
                                }
                            }

                            #endregion Read Warning
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ReturnValue")
                        {
                            //Read ReturnValues
                            #region Read ReturnValues

                            if (this.ReturnValues == null)
                                this.ReturnValues = new Collection<Object>();

                            this.ReturnValues.Add(reader.ReadInnerXml());

                            #endregion Read ReturnValues
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
        /// Get Xml representation of Attribute Operation Result
        /// </summary>
        /// <returns>Xml representation of Attribute Operation Result object</returns>
        public new String ToXml()
        {
            String attributeOperationResultXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("AttributeOperationResult");

            #region Write attribute operation result properties

            xmlWriter.WriteAttributeString("Id", this.AttributeId.ToString());
            xmlWriter.WriteAttributeString("Name", this.AttributeShortName);
            xmlWriter.WriteAttributeString("LongName", this.AttributeLongName);
            xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

            #endregion

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

            #region Write Warning

            //Add Warning Nodes
            xmlWriter.WriteStartElement("Warnings");

            foreach (Warning warning in this.Warnings)
            {
                xmlWriter.WriteRaw(warning.ToXml());
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

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            attributeOperationResultXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return attributeOperationResultXml;
        }

        /// <summary>
        /// Get Xml representation of Attribute operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute operation result</returns>
        public new String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            if (objectSerialization == ObjectSerialization.Full)
            {
                returnXml = this.ToXml();
            }
            else
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //Operation result node start
                xmlWriter.WriteStartElement("AttributeOperationResult");

                #region Write entity operation result properties

                xmlWriter.WriteAttributeString("Id", this.AttributeId.ToString());
                xmlWriter.WriteAttributeString("Name", this.AttributeShortName);
                xmlWriter.WriteAttributeString("LongName", this.AttributeLongName);                
                xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

                #endregion

                #region Write Errors

                //Add Error Nodes
                xmlWriter.WriteStartElement("Errors");

                foreach (Error error in this.Errors)
                {
                    xmlWriter.WriteRaw(error.ToXml(objectSerialization));
                }

                //Error nodes end
                xmlWriter.WriteEndElement();

                #endregion Write Errors

                #region Write Information

                //Add Information Nodes
                xmlWriter.WriteStartElement("Informations");

                foreach (Information information in this.Informations)
                {
                    xmlWriter.WriteRaw(information.ToXml(objectSerialization));
                }

                //Information node end
                xmlWriter.WriteEndElement();

                #endregion Write Information

                #region Write Warning

                //Add Warning Nodes
                xmlWriter.WriteStartElement("Warnings");

                foreach (Warning warning in this.Warnings)
                {
                    xmlWriter.WriteRaw(warning.ToXml(objectSerialization));
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

                //Operation result node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                returnXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }

            return returnXml;
        }

        /// <summary>
        /// Compares expected AttributeOperationResult object with the current instance
        /// </summary>
        /// <param name="subsetAttributeOperationResult">Expected AttributeOperationResult to be compared from</param>
        /// <param name="compareIds">Check whether to compare Id's or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(AttributeOperationResult subsetAttributeOperationResult, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetAttributeOperationResult.Id)
                    return false;

                if (this.AttributeId != subsetAttributeOperationResult.Id)
                    return false;
            }

            if (this.AttributeShortName != subsetAttributeOperationResult.AttributeShortName)
                return false;

            if (this.AttributeLongName != subsetAttributeOperationResult.AttributeLongName)
                return false;

            if (this.AttributeModelType != subsetAttributeOperationResult.AttributeModelType)
                return false;

            if (this.Locale != subsetAttributeOperationResult.Locale)
                return false;

            if (!base.IsSuperSetOf(subsetAttributeOperationResult, compareIds))
                return false;
                    
            return true;
        }

        /// <summary>
        /// Determines whether the collection reason types contains at least specific reason type.
        /// </summary>
        /// <param name="reasonTypes">Collection of reason type to find in the AttributeOperationResult.</param>
        /// <returns>'true' if collection of reason type found in attribute operation result otherwise 'false'.</returns>
        public Boolean Contains(Collection<ReasonType> reasonTypes)
        {
            Boolean result = false;

            if (this.HasError)
            {
                foreach (Error error in this.Errors)
                {
                    if (reasonTypes.Contains(error.ReasonType))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else if (this.HasWarnings)
            {
                foreach (Warning warning in this.Warnings)
                {
                    if (reasonTypes.Contains(warning.ReasonType))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
