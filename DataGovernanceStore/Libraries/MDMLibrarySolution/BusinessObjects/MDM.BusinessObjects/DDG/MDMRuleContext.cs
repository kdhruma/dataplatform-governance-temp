using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MdmRuleContext details
    /// </summary>
    [DataContract]
    [KnownType(typeof(MDMRuleAttributeContextCollection))]
    [KnownType(typeof(MDMRuleRelationshipContextCollection))]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleContext : MDMObject, IMDMRuleContext
    {
        #region Fields

        /// <summary>
        /// Field denotes collection of MdmRule attribute context
        /// </summary>
        [DataMember]
        private MDMRuleAttributeContextCollection _attributeContexts = null;

        /// <summary>
        /// Field denotes collection of MdmRule relationship context
        /// </summary>
        [DataMember]
        private MDMRuleRelationshipContextCollection _relationshipContexts = null;

        /// <summary>
        /// Field denotes extension context collection for an entity
        /// </summary>
        [DataMember]
        private MDMRuleEntityExtensionContextCollection _entityExtensionContexts = null;

        /// <summary>
        /// Field denoting whether to load Workflow Information or not
        /// </summary>
        private Boolean _loadWorkflowInformation = false;

        /// <summary>
        /// Field denoting whether to load attribtue model or not
        /// </summary>
        private Boolean _loadAttributeModel = false;

        /// <summary>
        /// Field denoting whether rule has any default qualified keywords or not.
        /// </summary>
        private Boolean _hasDefaultQualifiedKeywords = false;

        /// <summary>
        /// Field denoting whether rule is complex or not.
        /// </summary>
        private Boolean _isComplexRule = false;

        /// <summary>
        /// Field denoting whether to load required attributes or not
        /// </summary>
        private Boolean _loadRequiredAttributes = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes MdmRule Attribute context collection
        /// </summary>
        public MDMRuleAttributeContextCollection AttributeContexts
        {
            get
            {
                if (this._attributeContexts == null)
                {
                    this._attributeContexts = new MDMRuleAttributeContextCollection();
                }

                return this._attributeContexts;
            }
            set
            {
                this._attributeContexts = value;
            }
        }

        /// <summary>
        /// Property denotes MdmRule Relationship context collection
        /// </summary>
        public MDMRuleRelationshipContextCollection RelationshipContexts
        {
            get
            {
                if (this._relationshipContexts == null)
                {
                    this._relationshipContexts = new MDMRuleRelationshipContextCollection();
                }

                return this._relationshipContexts;
            }
            set
            {
                this._relationshipContexts = value;
            }
        }

        /// <summary>
        /// Property denotes extension context collection for an entity
        /// </summary>
        public MDMRuleEntityExtensionContextCollection EntityExtensionContexts
        {
            get
            {
                if (this._entityExtensionContexts == null)
                {
                    this._entityExtensionContexts = new MDMRuleEntityExtensionContextCollection();
                }

                return this._entityExtensionContexts;
            }
            set
            {
                this._entityExtensionContexts = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load workflow Information or not.
        /// </summary>
        [DataMember]
        public Boolean LoadWorkflowInformation
        {
            get
            {
                return _loadWorkflowInformation;
            }
            set
            {
                this._loadWorkflowInformation = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load AttributeModel or not.
        /// </summary>
        [DataMember]
        public Boolean LoadAttributeModel
        {
            get
            {
                return _loadAttributeModel;
            }
            set
            {
                this._loadAttributeModel = value;
            }
        }

        /// <summary>
        /// Property denoting whether to load required attributes or not.
        /// </summary>
        [DataMember]
        public Boolean LoadRequiredAttributes
        {
            get
            {
                return _loadRequiredAttributes;
            }
            set
            {
                this._loadRequiredAttributes = value;
            }
        }

        /// <summary>
        /// Property denoting whether MDMrule has default qualified keywords or not
        /// </summary>
        [DataMember]
        public Boolean HasDefaultQualifiedKeywords
        {
            get
            {
                return _hasDefaultQualifiedKeywords;
            }
            set
            {
                this._hasDefaultQualifiedKeywords = value;
            }
        }

        /// <summary>
        /// Property denoting whether MDMrule is complex or not
        /// </summary>
        [DataMember]
        public Boolean IsComplexRule
        {
            get
            {
                return _isComplexRule;
            }
            set
            {
                this._isComplexRule = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleContext()
            : base()
        {

        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule context</param>
        public MDMRuleContext(String valuesAsXml)
        {
            LoadRuleContext(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MdmRule context
        /// </param>
        public void LoadRuleContext(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleContext")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("LoadWorkflowInformation"))
                                {
                                    this._loadWorkflowInformation = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("LoadAttributeModel"))
                                {
                                    this._loadAttributeModel = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("LoadRequiredAttributes"))
                                {
                                    this._loadRequiredAttributes = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("HasDefaultQualifiedKeywords"))
                                {
                                    this._hasDefaultQualifiedKeywords = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("IsComplexRule"))
                                {
                                    this._isComplexRule = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                reader.Read();
                            }
                        }

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleAttributeContexts")
                        {
                            String contexts = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(contexts))
                            {
                                MDMRuleAttributeContextCollection attributeContexts = new MDMRuleAttributeContextCollection(contexts);

                                this._attributeContexts = attributeContexts;

                            }
                        }


                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleRelationshipContexts")
                        {
                            String contexts = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(contexts))
                            {
                                MDMRuleRelationshipContextCollection relationshipContexts = new MDMRuleRelationshipContextCollection(contexts);

                                this._relationshipContexts = relationshipContexts;
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleEntityExtensionContexts")
                        {
                            String contexts = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(contexts))
                            {
                                MDMRuleEntityExtensionContextCollection extensionContexts = new MDMRuleEntityExtensionContextCollection(contexts);

                                this._entityExtensionContexts = extensionContexts;
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get Xml representation of MDMRule context object
        /// </summary>
        /// <returns>Xml representation of MDMRule context object</returns>
        public new String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleContext");

                    xmlWriter.WriteAttributeString("LoadWorkflowInformation", this._loadWorkflowInformation.ToString());
                    xmlWriter.WriteAttributeString("LoadAttributeModel", this._loadAttributeModel.ToString());
                    xmlWriter.WriteAttributeString("LoadRequiredAttributes", this._loadRequiredAttributes.ToString());
                    xmlWriter.WriteAttributeString("HasDefaultQualifiedKeywords", this.HasDefaultQualifiedKeywords.ToString());
                    xmlWriter.WriteAttributeString("IsComplexRule", this.IsComplexRule.ToString());

                    if (this._attributeContexts != null)
                    {
                        xmlWriter.WriteRaw(this._attributeContexts.ToXml());
                    }

                    if (this._relationshipContexts != null)
                    {
                        xmlWriter.WriteRaw(this._relationshipContexts.ToXml());
                    }

                    if (this._entityExtensionContexts != null)
                    {
                        xmlWriter.WriteRaw(this._entityExtensionContexts.ToXml());
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Add attribute name into attribute context object
        /// </summary>
        /// <param name="attributeName">Indicates the attribute name</param>
        /// <param name="locale">Indicates the data locale</param>
        public void AddAttributeContext(String attributeName, LocaleEnum locale)
        {
            if (String.IsNullOrWhiteSpace(attributeName) == false)
            {
                MDMRuleAttributeContext context = null;

                if (this._attributeContexts != null)
                {
                    context = this._attributeContexts.GetAttributeContextByLocale(locale);
                }
                else
                {
                    this._attributeContexts = new MDMRuleAttributeContextCollection();
                }

                attributeName = attributeName.Trim();

                if (context != null)
                {
                    if (context.Attributes.ContainsAttributeName(attributeName) == false)
                    {
                        context.Attributes.Add(new MDMRuleAttribute() { Name = attributeName });
                    }
                }
                else
                {
                    MDMRuleAttributeCollection attributes = new MDMRuleAttributeCollection();
                    attributes.AddMDMAttribute(attributeName);

                    this._attributeContexts.Add(new MDMRuleAttributeContext() { DataLocale = locale, Attributes = attributes });
                }
            }
        }

        /// <summary>
        /// Add attribute group name into attribute group context object
        /// </summary>
        /// <param name="attributeGroupName">Indicates the attribute group name</param>
        /// <param name="locale">Indicates the data locale</param>
        /// /// <param name="isRequired">Indicates whether the attribute group is needed only required attributes or all</param>
        public void AddAttributeGroupContext(String attributeGroupName, LocaleEnum locale, Boolean isRequired = false)
        {
            if (String.IsNullOrWhiteSpace(attributeGroupName) == false)
            {
                MDMRuleAttributeContext context = null;

                if (this._attributeContexts != null)
                {
                    context = this._attributeContexts.GetAttributeContextByLocale(locale);
                }
                else
                {
                    this._attributeContexts = new MDMRuleAttributeContextCollection();
                }

                attributeGroupName = attributeGroupName.Trim();

                if (context != null)
                {
                    if (context.AttributeGroups.ContainsAttributeGroupName(attributeGroupName) == false)
                    {
                        context.AttributeGroups.Add(new MDMRuleAttribute() { Name = attributeGroupName, IsRequired = isRequired });
                    }
                }
                else
                {
                    MDMRuleAttributeGroupCollection attributeGroups = new MDMRuleAttributeGroupCollection();
                    attributeGroups.AddMDMAttributeGroup(attributeGroupName, isRequired);
                    this.AttributeContexts.Add(new MDMRuleAttributeContext() { DataLocale = locale, AttributeGroups = attributeGroups });
                }
            }
        }

        /// <summary>
        /// Add relationship type name into MDMRuleContext
        /// </summary>
        /// <param name="relationshipTypeName">Indicates the relationship type name</param>
        public void AddRelationshipContext(String relationshipTypeName)
        {
            if (String.IsNullOrWhiteSpace(relationshipTypeName) == false)
            {
                if (this._relationshipContexts != null)
                {
                    MDMRuleRelationshipContext context = this._relationshipContexts.GetRelationshipContextByTypeName(relationshipTypeName);

                    if (context == null)
                    {
                        this._relationshipContexts.Add(new MDMRuleRelationshipContext() { RelationshipTypeName = relationshipTypeName });
                    }
                }
                else
                {
                    this._relationshipContexts = new MDMRuleRelationshipContextCollection();
                    this._relationshipContexts.Add(new MDMRuleRelationshipContext() { RelationshipTypeName = relationshipTypeName });
                }
            }
        }

        /// <summary>
        /// Add relationship type name into MDMRuleContext
        /// </summary>
        /// <param name="relationshipTypeName">Indicates the relationship type name</param>
        /// <param name="attributeName">Indicates the attribute name</param>
        /// <param name="locale">Indicates the data locale</param>
        public void AddRelationshipContext(String relationshipTypeName, String attributeName, LocaleEnum locale)
        {
            if (String.IsNullOrWhiteSpace(relationshipTypeName) == false)
            {
                MDMRuleRelationshipContext context = null;

                if (this._relationshipContexts != null)
                {
                    context = this._relationshipContexts.GetRelationshipContextByTypeName(relationshipTypeName);
                }
                else
                {
                    this._relationshipContexts = new MDMRuleRelationshipContextCollection();
                }

                MDMRuleAttributeCollection attributes = new MDMRuleAttributeCollection();
                attributes.AddMDMAttribute(attributeName);

                if (context == null)
                {
                    MDMRuleRelationshipContext relContext = new MDMRuleRelationshipContext();
                    relContext.RelationshipTypeName = relationshipTypeName;
                    relContext.RelationshipAttributeContexts.Add(new MDMRuleAttributeContext() { DataLocale = locale, Attributes = attributes });
                    this._relationshipContexts.Add(relContext);
                }
                else
                {
                    var relAttributeContext = context.RelationshipAttributeContexts.GetAttributeContextByLocale(locale);

                    if (relAttributeContext == null)
                    {
                        context.RelationshipAttributeContexts.Add(new MDMRuleAttributeContext() { DataLocale = locale, Attributes = attributes });
                    }
                    else
                    {
                        relAttributeContext.Attributes.AddMDMAttribute(attributeName);
                    }
                }
            }
        }

        /// <summary>
        /// Get MDMRuleAttributes based on locale
        /// </summary>
        /// <param name="locale">Indicates the data locale</param>
        /// <returns>Returns the collection of MDMRuleAttribute</returns>
        public MDMRuleAttributeCollection GetAttributeContextByLocale(LocaleEnum locale)
        {
            MDMRuleAttributeCollection attributes = null;
            MDMRuleAttributeContext context = this.AttributeContexts.GetAttributeContextByLocale(locale);

            if (context != null)
            {
                attributes = context.Attributes;
            }

            return attributes;
        }

        /// <summary>
        /// Get MDMRuleAttributes based on locale
        /// </summary>
        /// <param name="locale">Indicates the data locale</param>
        /// <returns>Returns the collection of MDMRuleAttribute</returns>
        public MDMRuleAttributeGroupCollection GetAttributeGroupContextByLocale(LocaleEnum locale)
        {
            MDMRuleAttributeGroupCollection attributeGroups = null;
            MDMRuleAttributeContext context = this.AttributeContexts.GetAttributeContextByLocale(locale);

            if (context != null)
            {
                attributeGroups = context.AttributeGroups;
            }

            return attributeGroups;
        }

        /// <summary>
        /// Get MDMRuleAttributes based on locale.
        /// This will includes if any attributes are there in RS_ALL locale too.
        /// </summary>
        /// <param name="locale">Indicates the data locale</param>
        /// <returns>Returns the collection of MDMRuleAttribute</returns>
        public Collection<String> GetAttributeNamesByLocale(LocaleEnum locale)
        {
            Collection<String> attributeNames = null;
            MDMRuleAttributeCollection attributes = null;
            MDMRuleAttributeCollection requestedLocaleAttributes = GetAttributeContextByLocale(locale);
            MDMRuleAttributeCollection defaultQualifiedAttributes = GetAttributeContextByLocale(LocaleEnum.rs_ALL);

            if (requestedLocaleAttributes != null && requestedLocaleAttributes.Count > 0)
            {
                attributes = requestedLocaleAttributes;
            }

            if (defaultQualifiedAttributes != null && defaultQualifiedAttributes.Count > 0)
            {
                if (attributes == null)
                {
                    attributes = defaultQualifiedAttributes;
                }
                else
                {
                    attributes.AddRange(defaultQualifiedAttributes);
                }
            }

            if (attributes != null)
            {
                attributeNames = attributes.GetAttributeNames();
            }

            return attributeNames;
        }

        /// <summary>
        /// Get MDMRuleAttributes based on locale.
        /// This will includes if any attributes are there in RS_ALL locale too.
        /// </summary>
        /// <param name="locale">Indicates the data locale</param>
        /// <returns>Returns the collection of MDMRuleAttribute</returns>
        public Collection<String> GetAttributeGroupNamesByLocale(LocaleEnum locale)
        {
            Collection<String> attributeGroupNames = null;
            MDMRuleAttributeGroupCollection attributeGroups = null;
            MDMRuleAttributeGroupCollection requestedLocaleAttributeGroups = GetAttributeGroupContextByLocale(locale);
            MDMRuleAttributeGroupCollection defaultQualifiedAttributeGroups = GetAttributeGroupContextByLocale(LocaleEnum.rs_ALL);

            if (requestedLocaleAttributeGroups != null && requestedLocaleAttributeGroups.Count > 0)
            {
                attributeGroups = requestedLocaleAttributeGroups;
            }

            if (defaultQualifiedAttributeGroups != null && defaultQualifiedAttributeGroups.Count > 0)
            {
                if (attributeGroups == null)
                {
                    attributeGroups = defaultQualifiedAttributeGroups;
                }
                else
                {
                    attributeGroups.AddRange(defaultQualifiedAttributeGroups);
                }
            }

            if (attributeGroups != null)
            {
                attributeGroupNames = attributeGroups.GetAttributeGroupNames();
            }

            return attributeGroupNames;
        }

        /// <summary>
        /// Gets all the attribute names
        /// </summary>
        /// <returns>Returns the list of attribute names</returns>
        public Collection<String> GetAttributeNames()
        {
            Collection<String> attributeNames = null;

            if (_attributeContexts != null)
            {
                attributeNames = new Collection<String>();

                foreach (MDMRuleAttributeContext attrContext in _attributeContexts)
                {
                    foreach (MDMRuleAttribute attr in attrContext.Attributes)
                    {
                        if (attributeNames.Contains(attr.Name) == false)
                        {
                            attributeNames.Add(attr.Name);
                        }
                    }
                }
            }

            return attributeNames;
        }

        /// <summary>
        /// Gets all the Relationship type names
        /// </summary>
        /// <returns>Returns the list of relationship type names</returns>
        public Collection<String> GetRelationshipTypeNames()
        {
            Collection<String> relationshipTypeNames = null;

            if (_relationshipContexts != null)
            {
                relationshipTypeNames = new Collection<String>();

                foreach (MDMRuleRelationshipContext relContext in _relationshipContexts)
                {
                    if (relationshipTypeNames.Contains(relContext.RelationshipTypeName) == false)
                    {
                        relationshipTypeNames.Add(relContext.RelationshipTypeName);
                    }
                }
            }

            return relationshipTypeNames;
        }

        /// <summary>
        /// Get all the required attribute group names
        /// </summary>
        /// <returns>Returns the list of requried attribute group names</returns>
        public Collection<String> GetRequiredAttributeGroupNames()
        {
            return GetAttributeGroupNames(isRequired: true);
        }

        /// <summary>
        /// Get all non required attribute group names
        /// </summary>
        /// <returns>Returns the list of non requried attribute group names</returns>
        public Collection<String> GetNonRequiredAttributeGroupNames()
        {
            return GetAttributeGroupNames(isRequired: false);
        }

        private Collection<String> GetAttributeGroupNames(Boolean isRequired)
        {
            Collection<String> attributeGroupNames = null;

            if (_attributeContexts != null)
            {
                attributeGroupNames = new Collection<String>();

                foreach (MDMRuleAttributeContext attrContext in _attributeContexts)
                {
                    foreach (MDMRuleAttribute attributeGroup in attrContext.AttributeGroups)
                    {
                        if (attributeGroup.IsRequired == isRequired && attributeGroupNames.Contains(attributeGroup.Name) == false)
                        {
                            attributeGroupNames.Add(attributeGroup.Name);
                        }
                    }
                }
            }

            return attributeGroupNames;
        }

        /// <summary>
        /// Get all the attribute group names
        /// </summary>
        /// <returns>Returns the list of attribute group names</returns>
        public Collection<String> GetAllAttributeGroupNames()
        {
            Collection<String> attributeGroupNames = null;

            if (_attributeContexts != null)
            {
                attributeGroupNames = new Collection<String>();

                foreach (MDMRuleAttributeContext attrContext in _attributeContexts)
                {
                    foreach (MDMRuleAttribute attributeGroup in attrContext.AttributeGroups)
                    {
                        if (attributeGroupNames.Contains(attributeGroup.Name) == false)
                        {
                            attributeGroupNames.Add(attributeGroup.Name);
                        }
                    }
                }
            }

            return attributeGroupNames;
        }

        #endregion Methods
    }
}
