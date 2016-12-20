using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using MDM.Core;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    /// <summary>
    /// Specifies the Export Profile Data
    /// </summary>
    [DataContract]
    public class EntityExportUIProfileData : EntityExportProfileData, IEntityExportUIProfileData
    {
        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public EntityExportUIProfileData()
        {
        }

        /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public EntityExportUIProfileData(String valuesAsXml)
        {
            LoadExportProfileData(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the common attribute ids.
        /// </summary>
        [DataMember]
        public String CommonAttributeIds { get; set; }

        /// <summary>
        /// Property denoting to whether include all common attributes or not.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllCommonAttributeIds { get; set; }

        /// <summary>
        /// Property denoting to whether include all category attributes or not.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllCategoryAttributeIds { get; set; }

        /// <summary>
        /// Property denoting the category attribute ids.
        /// </summary>
        [DataMember]
        public String CategoryAttributeIds { get; set; }

        /// <summary>
        /// Property denoting the locale id list.
        /// </summary>
        [DataMember]
        public String LocaleIds { get; set; }

        /// <summary>
        /// Property denoting the Relationship type id list.
        /// </summary>
        [DataMember]
        public String RelationshipTypeIds { get; set; }

        /// <summary>
        /// Property denoting to whether include all relationship types or not.
        /// </summary>
        [DataMember]
        public Boolean IncludeAllRelationshipTypeIds { get; set; }

        /// <summary>
        /// Property denoting the RuleId.
        /// </summary>
        [DataMember]
        public Int32 RuleId { get; set; }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// represents ExportProfileData in Xml format 
        /// </summary>
        /// <returns>The result string</returns>
        public override String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("ProfileData");

            xmlWriter.WriteStartElement("CommonAttributeIDs");
            xmlWriter.WriteAttributeString("IncludeAllCommonAttributeIds", this.IncludeAllCommonAttributeIds.ToString().ToLowerInvariant());
            xmlWriter.WriteCData(CommonAttributeIds);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TechnicalAttributeIDs");
            xmlWriter.WriteAttributeString("IncludeAllCategoryAttributeIds", this.IncludeAllCategoryAttributeIds.ToString().ToLowerInvariant());
            xmlWriter.WriteCData(CategoryAttributeIds);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("LocaleIDs");
            xmlWriter.WriteCData(LocaleIds);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RelationshipTypeIDs");
            xmlWriter.WriteAttributeString("IncludeAllRelationshipTypeIds", this.IncludeAllRelationshipTypeIds.ToString().ToLowerInvariant());
            xmlWriter.WriteCData(RelationshipTypeIds);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("RuleId");
            xmlWriter.WriteCData(RuleId.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement(); // End ProfileData

            string profileData = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return profileData;
        }

        /// <summary>
        /// represents ExportProfileData in Xml format 
        /// </summary>
        /// <returns>The result string</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return ToXml();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populates ExportProfileData object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// <para>
        /// Sample Xml:
        /// <ProfileData>
        ///     <CommonAttributeIDs><![CDATA[1,2,3,4,5]]></CommonAttributeIDs>
        ///     <TechnicalAttributeIDs><![CDATA[6,7,8,9]]></TechnicalAttributeIDs>
        ///     <LocaleIDs><![CDATA[10,11]]></LocaleIDs>
        ///     <RelationshipTypeIDs><![CDATA[6,7,8,9]]></RelationshipTypeIDs>
        ///     <RuleId><![CDATA[1]]></RuleId>
        /// </ProfileData>
        /// </para>
        /// </param>
        private void LoadExportProfileData(String valuesAsXml)
        {
            #region Sample XML
            /*
                <ProfileData>
                    <CommonAttributeIDs />
                    <TechnicalAttributeIDs />
                    <LocaleIDs />
                    <RelationshipTypeIDs/>
                </ProfileData>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "CommonAttributeIDs")
                        {
                            #region Read Attributes

                            if (reader.GetAttribute("IncludeAllCommonAttributeIds") != null)
                            {
                                this.IncludeAllCommonAttributeIds = ValueTypeHelper.BooleanTryParse(reader.GetAttribute("IncludeAllCommonAttributeIds"), false);
                            }

                            #endregion

                            this.CommonAttributeIds = reader.ReadElementContentAsString();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "TechnicalAttributeIDs")
                        {
                            #region Read Attributes

                            if (reader.GetAttribute("IncludeAllCategoryAttributeIds") != null)
                            {
                                this.IncludeAllCategoryAttributeIds = ValueTypeHelper.BooleanTryParse(reader.GetAttribute("IncludeAllCategoryAttributeIds"), false);
                            }

                            #endregion

                            this.CategoryAttributeIds = reader.ReadElementContentAsString();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleIDs")
                        {
                            this.LocaleIds = reader.ReadElementContentAsString();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeIDs")
                        {
                            #region Read Attributes

                            if (reader.GetAttribute("IncludeAllRelationshipTypeIds") != null)
                            {
                                this.IncludeAllRelationshipTypeIds = ValueTypeHelper.BooleanTryParse(reader.GetAttribute("IncludeAllRelationshipTypeIds"), false);
                            }

                            #endregion

                            this.RelationshipTypeIds = reader.ReadElementContentAsString();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RuleId")
                        {
                            this.RuleId = ValueTypeHelper.Int32TryParse(reader.ReadElementContentAsString(), 0);
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

        #endregion
    }
}
