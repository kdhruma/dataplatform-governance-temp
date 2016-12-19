using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Data of a Imported Word List
    /// </summary>
    public class ImportWordListDTO : MDMObject, IImportWordListDTO, IDataModelObject
    {
        #region Fields

        private const String NodeName = "ImportWordListDTO";

        private const String ImportWordListIdAttributeName = "Id";
        private const String ImportWordListNameNode = "Name";
        private const String ImportWordListLongNameNode = "LongName";
        private const String ImportWordListActionNode = "Action";
        private const String ImportWordListAuditRefIdNode = "AuditRefId";
        private const String ImportWordListReferenceIdNode = "ReferenceId";
        private const String ImportWordListIsFlushAndFillModeNode = "IsFlushAndFillMode";
        private const String ImportWordListExternalIdNode = "ExternalId";

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ImportWordListDTO() { }

        /// <summary>
        /// Constructor with XML-format String of an ImportWordListDTO as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates an ImportWordListDTO in format of XML String</param>
        public ImportWordListDTO(String valuesAsXml)
        {
            LoadFromXml(valuesAsXml);
        }

        #endregion

        /// <summary>
        /// Specifies if Flush and Fill mode is used for Word List Import
        /// </summary>
        public Boolean IsFlushAndFillMode { get; set; }

        /// <summary>
        /// Represents Original Word List
        /// </summary>
        public WordList OriginalWordList { get; set; }

        /// <summary>
        /// Delta Merge of WordList Values
        /// </summary>
        /// <param name="deltaWordList">WordList that needs to be merged</param>
        public void MergeDelta(IWordList deltaWordList)
        {
            Action = deltaWordList.Name.Equals(Name) && deltaWordList.LongName.Equals(LongName) && !IsFlushAndFillMode ?
                ObjectAction.Read : ObjectAction.Update;
        }

        /// <summary>
        /// Property denoting a ObjectType for DataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return Core.ObjectType.WordList;
            }
        }

        /// <summary>
        /// Property denoting the external id for an DataModelObject object
        /// </summary>
        public String ExternalId { get; set; }

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        /// <summary>
        /// Gets Xml representation of object
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ImportWordListDTO node starts
            xmlWriter.WriteStartElement(NodeName);

            xmlWriter.WriteAttributeString(ImportWordListIdAttributeName, Id.ToString(CultureInfo.InvariantCulture));

            // Name
            xmlWriter.WriteStartElement(ImportWordListNameNode);
            xmlWriter.WriteRaw(Name);
            xmlWriter.WriteEndElement();

            // LongName
            xmlWriter.WriteStartElement(ImportWordListLongNameNode);
            xmlWriter.WriteRaw(LongName);
            xmlWriter.WriteEndElement();

            // Action
            xmlWriter.WriteStartElement(ImportWordListActionNode);
            xmlWriter.WriteRaw(Action.ToString());
            xmlWriter.WriteEndElement();

            // AuditRefId
            xmlWriter.WriteStartElement(ImportWordListAuditRefIdNode);
            xmlWriter.WriteRaw(AuditRefId.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteEndElement();

            // ReferenceId
            xmlWriter.WriteStartElement(ImportWordListReferenceIdNode);
            xmlWriter.WriteRaw(ReferenceId);
            xmlWriter.WriteEndElement();

            //Write ImportWordListDTO's properties
            xmlWriter.WriteRaw(PropertiesToXml());

            //ImportWordListDTO node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            String importWordListXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return importWordListXml;
        }

        private String PropertiesToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement(ImportWordListIsFlushAndFillModeNode);
            xmlWriter.WriteRaw(IsFlushAndFillMode.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement(ImportWordListExternalIdNode);
            xmlWriter.WriteRaw(ExternalId);
            xmlWriter.WriteEndElement();

            String result = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return result;
        }

        private void LoadFromXml(String valuesAsXml)
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case NodeName:
                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute(ImportWordListIdAttributeName))
                                    {
                                        Id = ValueTypeHelper.Int32TryParse(reader.Value, -1);
                                    }
                                }
                                break;
                            case ImportWordListNameNode:
                                Name = reader.ReadInnerXml();
                                break;
                            case ImportWordListLongNameNode:
                                LongName = reader.ReadInnerXml();
                                break;
                            case ImportWordListActionNode:
                                ObjectAction action;
                                Enum.TryParse(reader.ReadInnerXml(), out action);
                                Action = action;
                                break;
                            case ImportWordListAuditRefIdNode:
                                AuditRefId = ValueTypeHelper.Int32TryParse(reader.ReadInnerXml(), -1);
                                break;
                            case ImportWordListReferenceIdNode:
                                ReferenceId = reader.ReadInnerXml();
                                break;
                            case ImportWordListIsFlushAndFillModeNode:
                                IsFlushAndFillMode = ValueTypeHelper.BooleanTryParse(reader.ReadInnerXml(), false);
                                break;
                            case ImportWordListExternalIdNode:
                                ExternalId = reader.ReadInnerXml();
                                break;
                            default:
                                reader.Read();
                                break;
                        }
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
}
