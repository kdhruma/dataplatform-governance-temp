using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Type of a Container
    /// </summary>
    [DataContract]
    [KnownType(typeof(Attribute))]
    [KnownType(typeof(AttributeCollection))]
    public class Container : MDMObject, IContainer, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field for organization id
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Field for hierarchy id
        /// </summary>
        private Int32 _hierarchyId = 0;

        /// <summary>
        /// Field for container attributes
        /// </summary>
        private AttributeCollection _attributes = null;

        /// <summary>
        /// Field for supported locales for a container
        /// </summary>
        private LocaleCollection _supportedLocales = null;

        /// <summary>
        /// Field indicates organization short name
        /// </summary>
        private String _organizationShortName = String.Empty;

        /// <summary>
        /// Field Indicates Organization long name
        /// </summary>
        private String _organizationLongName = String.Empty;

        /// <summary>
        /// Field indicates Hierarchy Short Name
        /// </summary>
        private String _hierarchyShortName = String.Empty;

        /// <summary>
        /// Field indicates Hierarchy Long Name
        /// </summary>
        private String _hierarchyLongName = String.Empty;

        /// <summary>
        /// Field indicates whether the current entity is default or not.
        /// </summary>
        private Boolean _isDefault = false;

        /// <summary>
        /// Field indicates security object type.
        /// </summary>
        private Int32 _securityObjectTypeId = -1;

        /// <summary>
        /// Fields indicates the current container is for staging or not.
        /// </summary>
        private Boolean _isStaging = false;

        /// <summary>
        /// Field indicates processor weightage
        /// </summary>
        private Int32 _processorWeightage = -1;

        /// <summary>
        /// Field Denoting the original container
        /// </summary>
        private Container _originalContainer = null;

        /// <summary>
        ///  Field indicates type of container
        /// </summary>
        private ContainerType _containerType = ContainerType.Unknown;

        /// <summary>
        /// Field indicates id of container qualifier
        /// </summary>
        private Int32 _containerQualifierId = 1;

        /// <summary>
        /// Field indicates name of container qualifier
        /// </summary>
        private String _containerQualifierName = "None";

        /// <summary>
        /// Field indicates list of secondary container qualifier
        /// </summary>
        private Collection<String> _containerSecondaryQualifiers = new Collection<String>();

        /// <summary>
        /// Field indicates parent container identifier
        /// </summary>
        private Int32 _parentContainerId = -1;

        /// <summary>
        /// Field indicates name of parent container
        /// </summary>
        private String _parentContainerName = String.Empty;

        /// <summary>
        /// Field indicates whether the approved copy of the container is required or not
        /// </summary>
        private Boolean _needsApprovedCopy = false;

        /// <summary>
        /// Field indicates type of workflow
        /// </summary>
        private WorkflowType _workflowType = WorkflowType.Unknown;

        /// <summary>
        /// Field indicates cross reference id between approved and collaboration container
        /// </summary>
        private Int32 _crossReferenceId = 0;

        /// <summary>
        /// Indicates level of container
        /// </summary>
        private Int32 _level = 0;

        /// <summary>
        /// Field indicates whether the auto extension is enabled or not.
        /// </summary>
        private Boolean _autoExtensionEnabled = true;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting container key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Container()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Container</param>
        public Container(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a Container as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Container</param>
        /// <param name="name">Indicates the Name of a Container</param>
        /// <param name="longName">Indicates the Description of a Container</param>
        public Container(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of a Container as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Container</param>
        /// <param name="name">Indicates the Name of a Container</param>
        /// <param name="longName">Indicates the LongName of a Container</param>
        /// <param name="locale">Indicates the Locale of a Container</param>
        public Container(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }

        /// <summary>
        /// Create column object from xml
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Container Id="4" Name="Staging" LongName="Staging Master" Locale ="en_WW" OrganizationId="2" OrganizationShortName="Riverworks" OrganizationLongName="Riverworks Corporation" HierarchyId="3" HierarchyShortName="Product" HierarchyLongName="Product Hieararchy" Denormalize="True" DenormalizeTechnicalAttributes="True" IsDefault="False" SecurityObjectTypeId="4" IsStaging="True" DNIComponent="True" DNICategory="False" DNComponent="True" EnableDenormDelta="False" AutoDenormDelta="True" KeepExistingTechnicalAttributes="False" Action="Read" />
        /// ]]>
        /// </para>
        /// </param>
        public Container(String valuesAsXml)
        {
            LoadContainerFromXml(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "Container";
            }
        }

        /// <summary>
        /// Property denoting the Organization Id of a Container
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get
            {
                return this._organizationId;
            }
            set
            {
                this._organizationId = value;
            }
        }

        /// <summary>
        /// Property denoting the Hierarchy Id of a Container
        /// </summary>
        [DataMember]
        public Int32 HierarchyId
        {
            get
            {
                return this._hierarchyId;
            }
            set
            {
                this._hierarchyId = value;
            }
        }

        /// <summary>
        /// Property denoting the Attributes of a Container
        /// </summary>
        [DataMember]
        public AttributeCollection Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = new AttributeCollection();
                }
                return this._attributes;
            }
            set
            {
                this._attributes = value;
            }
        }

        /// <summary>
        /// Property denoting Supported Locales of a Container
        /// </summary>
        [DataMember]
        public LocaleCollection SupportedLocales
        {
            get
            {
                if (_supportedLocales == null)
                {
                    _supportedLocales = new LocaleCollection();
                }
                return this._supportedLocales;
            }
            set
            {
                this._supportedLocales = value;
            }
        }

        /// <summary>
        /// Property indicates the Organization Short Name
        /// </summary>
        [DataMember]
        public String OrganizationShortName
        {
            get
            {
                return _organizationShortName;
            }
            set
            {
                _organizationShortName = value;
            }
        }

        /// <summary>
        /// Property indicates the Organization Long Name
        /// </summary>
        [DataMember]
        public String OrganizationLongName
        {
            get
            {
                return _organizationLongName;
            }
            set
            {
                _organizationLongName = value;
            }
        }

        /// <summary>
        /// Property indicates the Hierarchy ShortName
        /// </summary>
        [DataMember]
        public String HierarchyShortName
        {
            get
            {
                return _hierarchyShortName;
            }
            set
            {
                _hierarchyShortName = value;
            }
        }

        /// <summary>
        /// Property indicates the Hierarchy LongName
        /// </summary>
        [DataMember]
        public String HierarchyLongName
        {
            get
            {
                return _hierarchyLongName;
            }
            set
            {
                _hierarchyLongName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean IsDefault
        {
            get
            {
                return _isDefault;
            }
            set
            {
                _isDefault = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 SecurityObjectTypeId
        {
            get
            {
                return _securityObjectTypeId;
            }
            set
            {
                _securityObjectTypeId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean IsStaging
        {
            get
            {
                return _isStaging;
            }
            set
            {
                _isStaging = value;
            }
        }

        /// <summary>
        /// Gets or sets the processor weightage.
        /// </summary>
        [DataMember]
        public Int32 ProcessorWeightage
        {
            get
            {
                return _processorWeightage;
            }
            set
            {
                _processorWeightage = value;
            }
        }

        /// <summary>
        /// Property denoting the original container
        /// </summary>
        public Container OriginalContainer
        {
            get
            {
                return _originalContainer;
            }
            set
            {
                this._originalContainer = value;
            }
        }

        /// <summary>
        /// Property denoting type of container
        /// </summary>
        [DataMember]
        public ContainerType ContainerType
        {
            get { return _containerType; }
            set { _containerType = value; }
        }

        /// <summary>
        /// Property denoting id of container qualifier
        /// </summary>
        [DataMember]
        public Int32 ContainerQualifierId
        {
            get { return _containerQualifierId; }
            set { _containerQualifierId = value; }
        }

        /// <summary>
        /// Property denoting name of container qualifier
        /// </summary>
        [DataMember]
        public String ContainerQualifierName
        {
            get { return _containerQualifierName; }
            set { _containerQualifierName = value; }
        }

        /// <summary>
        /// Property denoting list of secondary container qualifier
        /// </summary>
        [DataMember]
        public Collection<String> ContainerSecondaryQualifiers
        {
            get { return _containerSecondaryQualifiers; }
            set { _containerSecondaryQualifiers = value; }
        }

        /// <summary>
        /// Property denoting parent container identifier
        /// </summary>
        [DataMember]
        public Int32 ParentContainerId
        {
            get { return _parentContainerId; }
            set { _parentContainerId = value; }
        }

        /// <summary>
        /// Property denoting name of parent container
        /// </summary>
        [DataMember]
        public String ParentContainerName
        {
            get { return _parentContainerName; }
            set { _parentContainerName = value; }
        }

        /// <summary>
        /// Property denoting whether the approved copy of the container is required or not
        /// </summary>
        [DataMember]
        public Boolean NeedsApprovedCopy
        {
            get { return _needsApprovedCopy; }
            set { _needsApprovedCopy = value; }
        }

        /// <summary>
        /// Property denoting type of workflow
        /// </summary>
        [DataMember]
        public WorkflowType WorkflowType
        {
            get { return _workflowType; }
            set { _workflowType = value; }
        }

        /// <summary>
        /// Property denoting whether container is approved or not
        /// </summary>
        public Boolean IsApproved
        {
            get
            {
                if (this.ContainerType == Core.ContainerType.ExtensionApproved || this.ContainerType == Core.ContainerType.MasterApproved)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Property denoting cross reference id between approved and collaboration container
        /// </summary>
        [DataMember]
        public Int32 CrossReferenceId
        {
            get { return _crossReferenceId; }
            set { _crossReferenceId = value; }
        }

        /// <summary>
        /// Specifies level of container
        /// </summary>
        [DataMember]
        public Int32 Level
        {
            get { return this._level; }
            set { this._level = value; }
        }

        /// <summary>
        /// Specifies whether the auto extension is enabled or not.
        /// </summary>
        [DataMember]
        public Boolean AutoExtensionEnabled
        {
            get { return this._autoExtensionEnabled; }
            set { this._autoExtensionEnabled = value; }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.Catalog;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">EntityType object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is Container)
            {
                Container objectToBeCompared = obj as Container;

                if (!base.Equals(objectToBeCompared))
                {
                    return false;
                }

                if (this.HierarchyShortName != objectToBeCompared.HierarchyShortName)
                {
                    return false;
                }

                if (this.OrganizationShortName != objectToBeCompared.OrganizationShortName)
                {
                    return false;
                }

                if (this.IsDefault != objectToBeCompared.IsDefault)
                {
                    return false;
                }

                if (this.ContainerType != objectToBeCompared.ContainerType)
                {
                    return false;
                }

                if (this.ParentContainerName != objectToBeCompared.ParentContainerName)
                {
                    return false;
                }

                if (this.ContainerQualifierId != objectToBeCompared.ContainerQualifierId)
                {
                    return false;
                }

                if (this.ContainerQualifierName != objectToBeCompared.ContainerQualifierName)
                {
                    return false;
                }

                if (this.ContainerSecondaryQualifiers.Count() == objectToBeCompared.ContainerSecondaryQualifiers.Count)
                {
                    foreach (String item in objectToBeCompared.ContainerSecondaryQualifiers)
                    {
                        if (!this.ContainerSecondaryQualifiers.Contains(item))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

                if (this.NeedsApprovedCopy != objectToBeCompared.NeedsApprovedCopy)
                {
                    return false;
                }

                if (this.WorkflowType != objectToBeCompared.WorkflowType)
                {
                    return false;
                }

                if (this.CrossReferenceId != objectToBeCompared.CrossReferenceId)
                {
                    return false;
                }

                if (this.Level != objectToBeCompared.Level)
                {
                    return false;
                }

                if (this.AutoExtensionEnabled != objectToBeCompared.AutoExtensionEnabled)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Represents Container in Xml format for given Serialization option
        /// Note: Attributes and systemLocales are not serialized 
        /// </summary>
        /// <param name="objectSerialization">Options specifying which Xml format is to be generated</param>
        /// <returns>String representing Container in Xml format</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// represents Container in Xml format 
        /// Note: Attributes and systemLocales are not serialized 
        /// </summary>
        /// <returns></returns>
        public override String ToXml()
        {
            String containerXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Parameter node start
            xmlWriter.WriteStartElement("Container");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
            xmlWriter.WriteAttributeString("OrganizationShortName", this.OrganizationShortName);
            xmlWriter.WriteAttributeString("OrganizationLongName", this.OrganizationLongName);
            xmlWriter.WriteAttributeString("HierarchyId", this.HierarchyId.ToString());
            xmlWriter.WriteAttributeString("HierarchyShortName", this.HierarchyShortName);
            xmlWriter.WriteAttributeString("HierarchyLongName", this.HierarchyLongName);
            xmlWriter.WriteAttributeString("IsDefault", this.IsDefault.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("SecurityObjectTypeId", this.SecurityObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("IsStaging", this.IsStaging.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ContainerType", this.ContainerType.ToString());
            xmlWriter.WriteAttributeString("ContainerQualifierId", this.ContainerQualifierId.ToString());
            xmlWriter.WriteAttributeString("ContainerQualifierName", this.ContainerQualifierName);
            xmlWriter.WriteAttributeString("ParentContainerId", this.ParentContainerId.ToString());
            xmlWriter.WriteAttributeString("ParentContainerName", this.ParentContainerName);
            xmlWriter.WriteAttributeString("NeedsApprovedCopy", this.NeedsApprovedCopy.ToString());
            xmlWriter.WriteAttributeString("WorkflowType", this.WorkflowType.ToString());
            xmlWriter.WriteAttributeString("IsApproved", this.IsApproved.ToString());
            xmlWriter.WriteAttributeString("CrossReferenceId", this.CrossReferenceId.ToString());
            xmlWriter.WriteAttributeString("Level", this.Level.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("AutoExtensionEnabled", this.AutoExtensionEnabled.ToString());

            xmlWriter.WriteStartElement("Attributes");

            foreach (Attribute attribute in this.Attributes)
            {
                xmlWriter.WriteRaw(attribute.ToXml());
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("ContainerSecondaryQualifiers");
            foreach (String containerSecondaryQualifierName in this.ContainerSecondaryQualifiers)
            {
                xmlWriter.WriteStartElement("ContainerSecondaryQualifier");
                xmlWriter.WriteAttributeString("Value", containerSecondaryQualifierName);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SupportedLocales");
            foreach (Locale locale in this.SupportedLocales)
            {
                xmlWriter.WriteRaw(locale.ToXml());
            }
            xmlWriter.WriteEndElement();

            //Param node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            containerXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return containerXml;
        }

        /// <summary>
        /// Clone container object
        /// </summary>
        /// <returns>cloned copy of container copy.</returns>
        public IContainer Clone()
        {
            Container clonedContainer = new Container();

            clonedContainer.Id = this.Id;
            clonedContainer.Name = this.Name;
            clonedContainer.LongName = this.LongName;
            clonedContainer.Locale = this.Locale;
            clonedContainer.Action = this.Action;
            clonedContainer.AuditRefId = this.AuditRefId;
            clonedContainer.ExtendedProperties = this.ExtendedProperties;

            clonedContainer.OrganizationId = this.OrganizationId;
            clonedContainer.HierarchyId = this.HierarchyId;
            clonedContainer.Attributes = new AttributeCollection(this.Attributes.ToList());
            clonedContainer.SupportedLocales = (LocaleCollection)this.SupportedLocales.Clone();
            clonedContainer.OrganizationShortName = this.OrganizationShortName;
            clonedContainer.OrganizationLongName = this.OrganizationLongName;
            clonedContainer.HierarchyShortName = this.HierarchyShortName;
            clonedContainer.HierarchyLongName = this.HierarchyLongName;
            clonedContainer.IsDefault = this.IsDefault;
            clonedContainer.SecurityObjectTypeId = this.SecurityObjectTypeId;
            clonedContainer.IsStaging = this.IsStaging;
            clonedContainer.ContainerType = this.ContainerType;
            clonedContainer.ParentContainerId = this.ParentContainerId;
            clonedContainer.ParentContainerName = this.ParentContainerName;
            clonedContainer.ContainerQualifierId = this.ContainerQualifierId;
            clonedContainer.ContainerQualifierName = this.ContainerQualifierName;
            clonedContainer.ContainerSecondaryQualifiers = this.ContainerSecondaryQualifiers;
            clonedContainer.NeedsApprovedCopy = this.NeedsApprovedCopy;
            clonedContainer.WorkflowType = this.WorkflowType;
            clonedContainer.CrossReferenceId = this.CrossReferenceId;
            clonedContainer.Level = this.Level;
            clonedContainer.AutoExtensionEnabled = this.AutoExtensionEnabled;

            return clonedContainer;
        }

        /// <summary>
        /// Gets the attributes belonging to the Container
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        /// <exception cref="NullReferenceException">Attributes for container is null. There are no attributes to search in</exception>
        public IAttributeCollection GetAttributes()
        {
            if (this.Attributes == null)
            {
                throw new NullReferenceException("Attributes for container is null. There are no attributes to search in");
            }
            return (IAttributeCollection)this.Attributes;
        }

        /// <summary>
        /// Gets attribute with specified attribute Id from current container's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current container's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// /// <exception cref="NullReferenceException">Attributes for container is null. There are no attributes to search in</exception>
        public IAttribute GetAttribute(Int32 attributeId)
        {
            if (this.Attributes == null)
            {
                throw new NullReferenceException("Attributes for container is null. There are no attributes to search in");
            }

            return this.Attributes.GetAttribute(attributeId);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetContainer">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Container subsetContainer, Boolean compareIds = false)
        {
            if (subsetContainer != null)
            {
                if (base.IsSuperSetOf(subsetContainer, compareIds))
                {

                    if (compareIds)
                    {
                        if (this.OrganizationId != subsetContainer.OrganizationId)
                        {
                            return false;
                        }

                        if (this.HierarchyId != subsetContainer.HierarchyId)
                        {
                            return false;
                        }

                        if (this.SecurityObjectTypeId != subsetContainer.SecurityObjectTypeId)
                        {
                            return false;
                        }

                        if (this.ParentContainerId != subsetContainer.ParentContainerId)
                        {
                            return false;
                        }

                        if (this.ContainerQualifierId != subsetContainer.ContainerQualifierId)
                        {
                            return false;
                        }
                    }

                    if (!this.Attributes.IsSuperSetOf(subsetContainer.Attributes))
                    {
                        return false;
                    }

                    foreach (Locale locale in subsetContainer.SupportedLocales)
                    {
                        Locale subsetLocale = this.SupportedLocales.Where(l => l.Locale == locale.Locale).FirstOrDefault();

                        if (subsetLocale == null)
                        {
                            return false;
                        }
                    }

                    if (this.OrganizationShortName != subsetContainer.OrganizationShortName)
                    {
                        return false;
                    }

                    if (this.OrganizationLongName != subsetContainer.OrganizationLongName)
                    {
                        return false;
                    }

                    if (this.HierarchyShortName != subsetContainer.HierarchyShortName)
                    {
                        return false;
                    }

                    if (this.HierarchyLongName != subsetContainer.HierarchyLongName)
                    {
                        return false;
                    }

                    if (this.IsStaging != subsetContainer.IsStaging)
                    {
                        return false;
                    }

                    if (this.ContainerType != subsetContainer.ContainerType)
                    {
                        return false;
                    }

                    if (String.Compare(this.ParentContainerName, subsetContainer.ParentContainerName) != 0)
                    {
                        return false;
                    }

                    if (String.Compare(this.ContainerQualifierName, subsetContainer.ContainerQualifierName) != 0)
                    {
                        return false;
                    }

                    if (this.ContainerSecondaryQualifiers.Count() == subsetContainer.ContainerSecondaryQualifiers.Count())
                    {
                        foreach (String item in subsetContainer.ContainerSecondaryQualifiers)
                        {
                            if (!this.ContainerSecondaryQualifiers.Contains(item))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                    if (this.WorkflowType != subsetContainer.WorkflowType)
                    {
                        return false;
                    }

                    if (this.NeedsApprovedCopy != subsetContainer.NeedsApprovedCopy)
                    {
                        return false;
                    }

                    if (this.AutoExtensionEnabled != subsetContainer.AutoExtensionEnabled)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Delta Merge of container
        /// </summary>
        /// <param name="deltaContainer">Container that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged container instance</returns>
        public IContainer MergeDelta(IContainer deltaContainer, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IContainer mergedContainer = (returnClonedObject == true) ? deltaContainer.Clone() : deltaContainer;

            mergedContainer.Action = (mergedContainer.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedContainer;
        }

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        /// <summary>
        ///  Serves as a hash function for Container
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.Attributes.GetHashCode() ^ this.HierarchyId.GetHashCode() ^ this.SupportedLocales.GetHashCode()
                ^ this.ProcessorWeightage.GetHashCode() ^ this.HierarchyLongName.GetHashCode() ^ this.HierarchyShortName.GetHashCode() ^ this.IsDefault.GetHashCode()
                ^ this.IsStaging.GetHashCode() ^ this.OrganizationLongName.GetHashCode() ^ this.OrganizationShortName.GetHashCode() ^ this.SecurityObjectTypeId.GetHashCode()
                ^ this.Level.GetHashCode() ^ this.AutoExtensionEnabled.GetHashCode();
        }

        /// <summary>
        /// Gets the supported locales of the container
        /// </summary>
        /// <returns>Supported locale collection</returns>
        public ILocaleCollection GetSupportedLocales()
        {
            return this.SupportedLocales;
        }

        /// <summary>
        /// Sets the supported locales of the container
        /// </summary>
        /// <param name="supportedLocales">Indicates the supported locale collection</param>
        public void SetSupportedLocales(ILocaleCollection supportedLocales)
        {
            this.SupportedLocales = (LocaleCollection)supportedLocales;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create column object from xml
        /// Note: Attributes and systemLocales are not deserialized 
        /// </summary>
        /// <param name="valuesAsXml">
        /// Xml having values which we want to populate in current object
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        /// <Container Id="4" Name="Staging" LongName="Staging Master" Locale ="en_WW" OrganizationId="2" OrganizationShortName="Riverworks" OrganizationLongName="Riverworks Corporation" HierarchyId="3" HierarchyShortName="Product" HierarchyLongName="Product Hieararchy" IsDefault="False" SecurityObjectTypeId="4" IsStaging="True" Action="Read" />
        /// ]]>
        /// </para>
        /// </param>
        private void LoadContainerFromXml(String valuesAsXml)
        {

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Container")
                        {
                            #region Read Container

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), out locale);
                                    this.Locale = locale;
                                }
                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("OrganizationId"))
                                {
                                    this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("OrganizationShortName"))
                                {
                                    this.OrganizationShortName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("OrganizationLongName"))
                                {
                                    this.OrganizationLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("HierarchyId"))
                                {
                                    this.HierarchyId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("HierarchyShortName"))
                                {
                                    this.HierarchyShortName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("HierarchyLongName"))
                                {
                                    this.HierarchyLongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("IsDefault"))
                                {
                                    this.IsDefault = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("SecurityObjectTypeId"))
                                {
                                    this.SecurityObjectTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("IsStaging"))
                                {
                                    this.IsStaging = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ContainerType"))
                                {
                                    ContainerType containerType = ContainerType.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out containerType);
                                    this.ContainerType = containerType;
                                }
                                if (reader.MoveToAttribute("ContainerQualifierId"))
                                {
                                    this.ContainerQualifierId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("ContainerQualifierName"))
                                {
                                    this.ContainerQualifierName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("ParentContainerId"))
                                {
                                    this.ParentContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("ParentContainerName"))
                                {
                                    this.ParentContainerName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("NeedsApprovedCopy"))
                                {
                                    this.NeedsApprovedCopy = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("WorkflowType"))
                                {
                                    WorkflowType workflowType = WorkflowType.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out workflowType);
                                    this.WorkflowType = workflowType;
                                }
                                if (reader.MoveToAttribute("CrossReferenceId"))
                                {
                                    this.CrossReferenceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Level"))
                                {
                                    this.Level = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._level);
                                }
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }
                                if (reader.MoveToAttribute("AutoExtensionEnabled"))
                                {
                                    this.AutoExtensionEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._autoExtensionEnabled);
                                }
                            }

                            #endregion Read Container
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
                            //Read attributes
                            #region Read attributes

                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                AttributeCollection attributeCollection = new AttributeCollection(attributeXml);
                                if (attributeCollection != null)
                                {
                                    // Based on the serialization type the unique 
                                    foreach (Attribute attr in attributeCollection)
                                    {
                                        if (!this.Attributes.Contains(attr.Name, attr.AttributeParentName, attr.Locale))
                                            this.Attributes.Add(attr);
                                    }
                                }
                            }

                            #endregion Read attributes
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerSecondaryQualifiers")
                        {
                            while (!reader.EOF)
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerSecondaryQualifier")
                                {
                                    if (reader.MoveToAttribute("Value"))
                                    {
                                        this.ContainerSecondaryQualifiers.Add(reader.ReadContentAsString());
                                    }
                                }
                                else
                                {
                                    reader.Read();
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "SupportedLocales")
                        {
                            #region Read SupportedLocales

                            String localeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(localeXml))
                            {
                                _supportedLocales = new LocaleCollection(localeXml);
                            }

                            #endregion Read SupportedLocales
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

        #endregion Methods
    }
}