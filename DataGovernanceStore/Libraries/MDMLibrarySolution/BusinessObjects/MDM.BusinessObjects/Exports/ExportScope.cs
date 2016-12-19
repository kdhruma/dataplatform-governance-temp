using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the export scope object
    /// </summary>
    [DataContract]
    [KnownType(typeof(SearchAttributeRuleGroupCollection))]
    [KnownType(typeof(ExportScopeCollection))]
    [KnownType(typeof(SearchValidationStatesRuleGroup))]
    [KnownType(typeof(SearchMDMRuleRuleGroup))]
    [KnownType(typeof(CategoryGroup))]
    public class ExportScope : MDMObject, IExportScope
    {
        #region Fields

        /// <summary>
        /// Field specifying export scope object id
        /// </summary>
        private Int64 _objectId = 0;

        /// <summary>
        /// Field specifying export scope object type
        /// </summary>
        private ObjectType _objectType = 0;

        /// <summary>
        /// Field specifying export scope object id
        /// </summary>
        private String _objectName = String.Empty;

        /// <summary>
        /// Field specifying export scope object unique identifier
        /// </summary>
        private String _objectUniqueIdentifier = String.Empty;

        /// <summary>
        /// Field specifying export scope object include or not
        /// </summary>
        private Boolean _include = false;

        /// <summary>
        /// Field specifying export scope object recursive or not
        /// </summary>
        private Boolean _isRecursive = false;

        /// <summary>
        /// Field specifying collection of search attribute rule group
        /// </summary>
        private SearchAttributeRuleGroupCollection _searchAttributeRuleGroups = new SearchAttributeRuleGroupCollection();

        /// <summary>
        /// Field specifying collection of child export scope
        /// </summary>
        private ExportScopeCollection _exportScopes = new ExportScopeCollection();

        /// <summary>
        /// Field specifying collection of validationStates attribute rule group
        /// </summary>
        private SearchValidationStatesRuleGroup _searchValidationStatesRuleGroup = new SearchValidationStatesRuleGroup();

        /// <summary>
        /// Field specifying collection of business conditions rule group
        /// </summary>
        private SearchMDMRuleRuleGroup _mdmRuleGroup = new SearchMDMRuleRuleGroup();

        /// <summary>
        /// Field specifying the categories for the scope rule
        /// </summary>
        private CategoryGroup _categoryGroup = new CategoryGroup();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies export scope object id
        /// </summary>
        [DataMember]
        public Int64 ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                _objectId = value;
            }
        }

        /// <summary>
        /// Property specifies export scope object type
        /// </summary>
        [DataMember]
        public new ObjectType ObjectType
        {
            get
            {
                return _objectType;
            }
            set
            {
                _objectType = value;
            }
        }

        /// <summary>
        /// Property specifies export scope object type
        /// </summary>
        [DataMember]
        public String ObjectName
        {
            get
            {
                return _objectName;
            }
            set
            {
                _objectName = value;
            }
        }

        /// <summary>
        /// Property specifies export scope object unique identifier
        /// </summary>
        [DataMember]
        public String ObjectUniqueIdentifier
        {
            get
            {
                return _objectUniqueIdentifier;
            }
            set
            {
                _objectUniqueIdentifier = value;
            }
        }

        /// <summary>
        /// Property specifies export scope object include or not
        /// </summary>
        [DataMember]
        public Boolean Include
        {
            get
            {
                return _include;
            }
            set
            {
                _include = value;
            }
        }

        /// <summary>
        /// Property specifies export scope object recursive or not
        /// </summary>
        [DataMember]
        public Boolean IsRecursive
        {
            get
            {
                return _isRecursive;
            }
            set
            {
                _isRecursive = value;
            }
        }

        /// <summary>
        /// Property denoting collection of search attribute rule groups.
        /// </summary>
        [DataMember]
        public SearchAttributeRuleGroupCollection SearchAttributeRuleGroups
        {
            get
            {
                return this._searchAttributeRuleGroups;
            }
            set
            {
                this._searchAttributeRuleGroups = value;
            }
        }

        /// <summary>
        /// Property denoting collection of child export scopes.
        /// </summary>
        [DataMember]
        public ExportScopeCollection ExportScopes
        {
            get
            {
                return this._exportScopes;
            }
            set
            {
                this._exportScopes = value;
            }
        }

        /// <summary>
        /// Property collection of state view attribute rule group
        /// </summary>
        [DataMember]
        public SearchValidationStatesRuleGroup SearchValidationStatesRuleGroup
        {
            get
            {
                return this._searchValidationStatesRuleGroup;
            }
            set
            {
                this._searchValidationStatesRuleGroup = value;
            }
        }

        /// <summary>
        /// Property denoting the collection of MDMRule rule groups
        /// </summary>
        [DataMember]
        public SearchMDMRuleRuleGroup MDMRuleGroup
        {
            get
            {
                return this._mdmRuleGroup;
            }
            set
            {
                this._mdmRuleGroup = value;
            }
        }

        /// <summary>
        /// Property denoting the collection fo categories selected
        /// </summary> 
        /// 
       [DataMember]
        public CategoryGroup CategoryGroup
        {
            get
            {
                return this._categoryGroup;
            }
            set
            {   
                this._categoryGroup = value;
            }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes exportscope object with default parameters
        /// </summary>
        public ExportScope()
        {
            
        }

        /// <summary>
        /// Initializes object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type of objectSerialization</param>
        public ExportScope(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadExportScope(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents exportscope in Xml format
        /// </summary>
        /// <returns>String representation of current exportscope object</returns>
        public override String ToXml()
        {
            String exportScopeXml = String.Empty;
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {

                    //ExportScope node start
                    xmlWriter.WriteStartElement("ExportScope");

                    #region write ExportScope properties for full exportscope xml

                    xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());
                    xmlWriter.WriteAttributeString("ObjectName", this.ObjectName.ToString());
                    xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());
                    xmlWriter.WriteAttributeString("ObjectUniqueIdentifier", this.ObjectUniqueIdentifier);
                    xmlWriter.WriteAttributeString("Include", this.Include.ToString());
                    xmlWriter.WriteAttributeString("IsRecursive", this.IsRecursive.ToString());

                    #endregion  write ExportScope properties for full exportscope xml

                    #region write export scope categories for Full categories Xml

                    if ((this.CategoryGroup !=null) && (this.CategoryGroup.Categories != null))
                    {
                        xmlWriter.WriteRaw(this.CategoryGroup.Categories.ToXml());
                    }

                    #endregion write export scope categories for Full categories Xml

                    #region write export scope search attributerules for Full searchsttributerulegroup Xml

                    if (this.SearchAttributeRuleGroups != null)
                    {
                        xmlWriter.WriteRaw(this.SearchAttributeRuleGroups.ToXml());
                    }

                    #endregion write export scope search attributerules for Full searchsttributerule Xml

                    #region write export scope business condition attribute rules for Full business rule condition Xml

                    if (this.MDMRuleGroup != null)
                    {
                        xmlWriter.WriteRaw(this.MDMRuleGroup.ToXml());
                    }

                    #endregion write export scope business condition attribute rules for Full business rule condition Xml

                    #region write export scope state view attribute rules for Full state view rule group Xml

                    if (this.SearchValidationStatesRuleGroup != null)
                    {
                        xmlWriter.WriteRaw(this.SearchValidationStatesRuleGroup.ToXml());
                    }

                    #endregion write export scope state view attribute rules for Full state view rule group Xml

                    #region Write child exportScopes xml

                    if (this.ExportScopes != null && this.ExportScopes.Count > 0)
                    {
                        xmlWriter.WriteStartElement("ExportScopes");

                        foreach (ExportScope childExportScope in this.ExportScopes)
                        {
                            xmlWriter.WriteRaw(childExportScope.ToXml());
                        }

                        //Child Attribute node end
                        xmlWriter.WriteEndElement();
                    }

                    #endregion Write child exportScopes xml

                    //ExportScope node end
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    //get the actual XML
                    exportScopeXml = sw.ToString();
                }
            }

            return exportScopeXml;
        }

        /// <summary>
        /// Represents exportscope in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current exportscope object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String exportScopeXml = String.Empty;

            if (objectSerialization == ObjectSerialization.External)
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(sw);

                //ExportScope node start
                xmlWriter.WriteStartElement("ExportScope");

                #region write ExportScope properties for full exportscope xml

                xmlWriter.WriteAttributeString("ObjectType", this.ObjectType.ToString());
                xmlWriter.WriteAttributeString("ObjectName", this.ObjectName.ToString());
                xmlWriter.WriteAttributeString("ObjectId", this.ObjectId.ToString());
                xmlWriter.WriteAttributeString("ObjectUniqueIdentifier", this.ObjectUniqueIdentifier);
                xmlWriter.WriteAttributeString("Include", this.Include.ToString());
                xmlWriter.WriteAttributeString("IsRecursive", this.IsRecursive.ToString());

                if (!String.IsNullOrWhiteSpace(ExtendedProperties))
                {
                    xmlWriter.WriteStartElement("ExtendedProperties");
                    foreach (String extendedProperty in this.ExtendedProperties.Split('#'))
                    {
                        //ExtendedProperty node start
                        xmlWriter.WriteStartElement("ExtendedProperty");

                        String[] extendedPropertyArray = extendedProperty.Split(',');
                        xmlWriter.WriteAttributeString("Name", extendedPropertyArray[0]);
                        xmlWriter.WriteAttributeString("Value", extendedPropertyArray[1]);

                        //ExtendedProperty node end
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }

                #endregion  write ExportScope properties for full exportscope xml

                #region write export scope categories for Full categories Xml

                if ((this.CategoryGroup != null) && (this.CategoryGroup.Categories != null))
                {
                    xmlWriter.WriteRaw(this.CategoryGroup.Categories.ToXml(objectSerialization));
                }

                #endregion write export scope categories for Full categories Xml

                #region write export scope search attributerules for Full searchsttributerulegroup Xml

                if (this.SearchAttributeRuleGroups != null)
                {
                    xmlWriter.WriteRaw(this.SearchAttributeRuleGroups.ToXml());
                }

                #endregion write export scope search attributerules for Full searchsttributerule Xml

                #region write export scope business condition attribute rules for Full business rule condition Xml

                if (this.MDMRuleGroup != null)
                {
                    xmlWriter.WriteRaw(this.MDMRuleGroup.ToXml());
                }

                #endregion write export scope business condition attribute rules for Full business rule condition Xml

                #region write export scope search validationstates rules 

                if (this.SearchValidationStatesRuleGroup != null)
                    xmlWriter.WriteRaw(this.SearchValidationStatesRuleGroup.ToXml());

                #endregion write export scope search validationstates rules

                #region Write child exportScopes xml

                if (this.ExportScopes != null && this.ExportScopes.Count > 0)
                {
                    xmlWriter.WriteStartElement("ExportScopes");

                    foreach (ExportScope childExportScope in this.ExportScopes)
                    {
                        xmlWriter.WriteRaw(childExportScope.ToXml(objectSerialization));
                    }

                    //Child Attribute node end
                    xmlWriter.WriteEndElement();
                }

                #endregion Write child exportScopes xml

                //ExportScope node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //get the actual XML
                exportScopeXml = sw.ToString();

                xmlWriter.Close();
                sw.Close();
            }
            else
            {
                exportScopeXml = this.ToXml();
            }
            return exportScopeXml;
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
                if (obj is ExportScope)
                {
                    ExportScope objectToBeCompared = obj as ExportScope;

                    if (!this.SearchAttributeRuleGroups.Equals(objectToBeCompared.SearchAttributeRuleGroups))
                        return false;

                    if (!this.ExportScopes.Equals(objectToBeCompared.ExportScopes))
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
            hashCode = base.GetHashCode() ^ this.SearchAttributeRuleGroups.GetHashCode() ^ this.SearchValidationStatesRuleGroup.GetHashCode() ^ this.MDMRuleGroup.GetHashCode() ^ this.ExportScopes.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the exportscope with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadExportScope(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
                <ExportScope ObjectType="Container" ObjectId="" ObjectUniqueIdentifier="Product Master" Include="" IsRecursive="false">
				    <SearchAttributeRuleGroups>
                      <SearchAttributeRuleGroup ObjectId="0" GroupOperator="" RuleOperator="">
                        <SearchAttributeRules />
                      </SearchAttributeRuleGroup>
                    </SearchAttributeRuleGroups>
				    <ExportScopes>
					    <ExportScope ObjectType="Category" ObjectId="" ObjectUniqueIdentifier="Apparel" Include="" IsRecursive="false">
						    <SearchAttributeRuleGroups />
					    </ExportScope>
					    <ExportScope ObjectType="Entity" ObjectId="" ObjectUniqueIdentifier="P1121" Include="" IsRecursive="false">
						    <SearchAttributeRuleGroups />
					    </ExportScope>
				    </ExportScopes>
			    </ExportScope>
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
                        #region Read ExportScope

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportScope")
                        {
                            #region Read export scope Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ObjectType"))
                                {
                                    ObjectType objectType = ObjectType.None;
                                    Enum.TryParse<ObjectType>(reader.ReadContentAsString(), out objectType);
                                    this.ObjectType = objectType;
                                }

                                if (reader.MoveToAttribute("ObjectId"))
                                {
                                    this.ObjectId = ValueTypeHelper.ConvertToInt64(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ObjectName"))
                                {
                                    this.ObjectName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ObjectUniqueIdentifier"))
                                {
                                    this.ObjectUniqueIdentifier = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Include"))
                                {
                                    this.Include = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("IsRecursive"))
                                {
                                    this.IsRecursive = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperties")
                        {
                            String attributeXml = reader.ReadInnerXml();
                            if (!String.IsNullOrWhiteSpace(attributeXml))
                            {
                                this.ExtendedProperties = LoadExtendedProperties(attributeXml);
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Categories")
                        {
                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrWhiteSpace(attributeXml))
                            {
                                this._categoryGroup.Categories = new CategoryCollection(attributeXml);
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchValidationStatesRuleGroup")
                        {
                            #region Read search ValidaitonStates rule groups

                            String searchValidationStatesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(searchValidationStatesXml))
                            {
                                //Get collection of search validation States rules and populate it in SearchValidationStatesRule collection of current exportscope object.
                                SearchValidationStatesRuleGroup searchValidationStatesRuleGroup = new SearchValidationStatesRuleGroup(searchValidationStatesXml);
                                if (searchValidationStatesRuleGroup != null)
                                {
                                    this.SearchValidationStatesRuleGroup = searchValidationStatesRuleGroup;

                                }
                            }

                            #endregion Read search validation States rule groups
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchMDMRuleRuleGroup")
                        {
                            #region Read search Business rule group

                            String searchBusinessRulesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(searchBusinessRulesXml))
                            {
                                //Get search Mdm rule and populate it in SearchMDMRuleRuleGroup of current exportscope object.
                                SearchMDMRuleRuleGroup searchMDMRuleGroup = new SearchMDMRuleRuleGroup(searchBusinessRulesXml);
                                if (searchMDMRuleGroup != null)
                                {
                                    this.MDMRuleGroup = searchMDMRuleGroup;

                                }
                            }

                            #endregion Read search validation States rule groups
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SearchAttributeRuleGroups")
                        {
                            #region Read search attribute rule groups

                            String searchAttributesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(searchAttributesXml))
                            {
                                //Get collection of search attribute rules and populate it in SearchAttributeRule collection of current exportscope object.
                                SearchAttributeRuleGroupCollection searchAttributeRuleGroupCollection = new SearchAttributeRuleGroupCollection(searchAttributesXml);
                                if (searchAttributeRuleGroupCollection != null)
                                {
                                    foreach (SearchAttributeRuleGroup searchAttributeRule in searchAttributeRuleGroupCollection)
                                    {
                                        if (!this.SearchAttributeRuleGroups.Contains(searchAttributeRule))
                                        {
                                            this.SearchAttributeRuleGroups.Add(searchAttributeRule);
                                        }
                                    }
                                }
                            }

                            #endregion Read search attribute rule groups
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExportScopes")
                        {
                            #region Read child exportscopes

                            String exportScopesXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(exportScopesXml))
                            {
                                //Get collection of child exportScopes and populate it in ExportScope collection of current exportscope object.
                                ExportScopeCollection exportScopeCollection = new ExportScopeCollection(exportScopesXml);
                                if (exportScopeCollection != null)
                                {
                                    foreach (ExportScope exportScope in exportScopeCollection)
                                    {
                                        if (!this.ExportScopes.Contains(exportScope))
                                        {
                                            this.ExportScopes.Add(exportScope);
                                        }
                                    }
                                }
                            }

                            #endregion Read child exportscopes
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read ExportScope
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

        private String LoadExtendedProperties(String valuesAsXml)
        {
            StringBuilder extendedProperties = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtendedProperty")
                        {
                            if (reader.HasAttributes)
                            {
                                String name = String.Empty;
                                String value = String.Empty;
                                if (reader.MoveToAttribute("Name"))
                                {
                                    name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Value"))
                                {
                                    value = reader.ReadContentAsString();
                                }
                                if (!String.IsNullOrWhiteSpace(extendedProperties.ToString()))
                                {
                                    extendedProperties.Append("#" + name + "," + value);
                                }
                                else
                                {
                                    extendedProperties.Append(name + "," + value);
                                }
                            }
                            else
                            {
                                reader.Read();
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
            return extendedProperties.ToString();
        }

        #endregion Private Methods
    }
}
