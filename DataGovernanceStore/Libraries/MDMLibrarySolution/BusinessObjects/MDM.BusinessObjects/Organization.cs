using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies an Organization
    /// </summary>
    [DataContract]
    public class Organization : MDMObject, IOrganization, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting type of an Organization
        /// </summary>
        private Int32 _organizationTypeId = 0;

        /// <summary>
        /// Field denoting GLN number of an Organization
        /// </summary>
        private String _gln = String.Empty;

        /// <summary>
        /// Field denoting the Classification of an Organization
        /// </summary>
        private Int32 _organizationClassification = 1;

        /// <summary>
        /// Field denoting the parent Organization id.
        /// </summary>
        private Int32 _parentOrganizationId = 0;

        /// <summary>
        /// Field denoting the parent organization name.
        /// </summary>
        private String _parentOrganizationName = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        private Int32 _processorWeightage = 0;

        /// <summary>
        /// Field denoting organization attributes
        /// </summary>
        private AttributeCollection _attributes = new AttributeCollection();

        /// <summary>
        /// Field Denoting the original organization
        /// </summary>
        private Organization _originalOrganization = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting Organization key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Organization()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization </param>
        public Organization(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of an Organization  as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization </param>
        /// <param name="name">Indicates the Name of an Organization </param>
        /// <param name="longName">Indicates the Description of an Organization </param>
        public Organization(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an Organization as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization </param>
        /// <param name="name">Indicates the Name of an Organization </param>
        /// <param name="longName">Indicates the LongName of an Organization </param>
        /// <param name="locale">Indicates the Locale of an Organization </param>
        public Organization(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and organizationTypeId of an Organization as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization</param>
        /// <param name="name">Indicates the name of an Organization</param>
        /// <param name="longName">Indicates the longName of an Organization</param>
        /// <param name="organizationTypeId">Indicates the organizationTypeId of an Organization</param>
        public Organization(Int32 id, String name, String longName, Int32 organizationTypeId)
            : base(id, name, longName)
        {
            this._organizationTypeId = organizationTypeId;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and organizationTypeId of an Organization as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization</param>
        /// <param name="name">Indicates the name of an Organization</param>
        /// <param name="longName">Indicates the longName of an Organization</param>
        /// <param name="organizationTypeId">Indicates the organizationTypeId of an Organization</param>
        /// <param name="gln">Indicates the gln of an Organization</param>
        public Organization(Int32 id, String name, String longName, Int32 organizationTypeId, String gln)
            : base(id, name, longName)
        {
            this._organizationTypeId = organizationTypeId;
            this._gln = gln;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and organizationTypeId of an Organization as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization</param>
        /// <param name="name">Indicates the name of an Organization</param>
        /// <param name="longName">Indicates the longName of an Organization</param>
        /// <param name="organizationTypeId">Indicates the organizationTypeId of an Organization</param>
        /// <param name="gln">Indicates the gln of an Organization</param>
        /// <param name="organizationClassification">Indicates the Classification of an Organization</param>
        public Organization(Int32 id, String name, String longName, Int32 organizationTypeId, String gln, Int32 organizationClassification)
            : base(id, name, longName)
        {
            this._organizationTypeId = organizationTypeId;
            this._gln = gln;
            this._organizationClassification = organizationClassification;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and organizationTypeId of an Organization as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization</param>
        /// <param name="name">Indicates the name of an Organization</param>
        /// <param name="longName">Indicates the longName of an Organization</param>
        /// <param name="organizationTypeId">Indicates the organizationTypeId of an Organization</param>
        /// <param name="gln">Indicates the gln of an Organization</param>
        /// <param name="organizationClassification">Indicates the Classification of an Organization</param>
        /// <param name="parentOrganizationId">Indicates the Parent of an Organization</param>
        public Organization(Int32 id, String name, String longName, Int32 organizationTypeId, String gln, Int32 organizationClassification, Int32 parentOrganizationId)
            : base(id, name, longName)
        {
            this._organizationTypeId = organizationTypeId;
            this._gln = gln;
            this._organizationClassification = organizationClassification;
            this._parentOrganizationId = parentOrganizationId;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and organizationTypeId of an Organization as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization</param>
        /// <param name="name">Indicates the name of an Organization</param>
        /// <param name="longName">Indicates the longName of an Organization</param>
        /// <param name="organizationTypeId">Indicates the organizationTypeId of an Organization</param>
        /// <param name="gln">Indicates the gln of an Organization</param>
        /// <param name="organizationClassification">Indicates the Classification of an Organization</param>
        /// <param name="parentOrganizationId">Indicates the Parent of an Organization</param>
        /// <param name="processorWeightage"></param>
        public Organization(Int32 id, String name, String longName, Int32 organizationTypeId, String gln, Int32 organizationClassification, Int32 parentOrganizationId, Int32 processorWeightage)
            : base(id, name, longName)
        {
            this._organizationTypeId = organizationTypeId;
            this._gln = gln;
            this._organizationClassification = organizationClassification;
            this._parentOrganizationId = parentOrganizationId;
            this._processorWeightage = processorWeightage;
        }

        /// <summary>
        /// Constructor with XML-format String of an Organization as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates an Organization in format of XML String</param>
        public Organization(String valuesAsXml)
        {
            LoadOrganizationFromXml(valuesAsXml);
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
                return "Organization";
            }
        }

        /// <summary>
        /// Property denoting the Type of an Organization
        /// </summary>
        [DataMember]
        public Int32 OrganizationTypeId
        {
            get
            {
                return this._organizationTypeId;
            }
            set
            {
                this._organizationTypeId = value;
            }
        }

        /// <summary>
        /// Property denoting the Classification of an Organization
        /// </summary>
        [DataMember]
        public Int32 OrganizationClassification
        {
            get
            {
                return this._organizationClassification;
            }
            set
            {
                this._organizationClassification = value;
            }
        }

        /// <summary>
        /// Property denoting the Parent of an Organization
        /// </summary>
        [DataMember]
        public Int32 OrganizationParent
        {
            get
            {
                return this._parentOrganizationId;
            }
            set
            {
                this._parentOrganizationId = value;
            }
        }

        /// <summary>
        /// Property denoting the parent organization name
        /// </summary>
        [DataMember]
        public String ParentOrganizationName
        {
            get
            {
                return this._parentOrganizationName;
            }
            set
            {
                this._parentOrganizationName = value;
            }
        }

        /// <summary>
        /// Property denoting the GLN number of an Organization
        /// </summary>
        [DataMember]
        public String GLN
        {
            get
            {
                return this._gln;
            }
            set
            {
                this._gln = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public Int32 ProcessorWeightage
        {
            get
            {
                return this._processorWeightage;
            }
            set
            {
                this._processorWeightage = value;
            }
        }

        /// <summary>
        /// Property denoting the Attributes of an Organization
        /// </summary>
        [DataMember]
        public AttributeCollection Attributes
        {
            get
            {
                return this._attributes;
            }
            set
            {
                this._attributes = value;
            }
        }
        
        /// <summary>
        /// Property denoting the original organization
        /// </summary>
        public Organization OriginalOrganization
        {
            get
            {
                return _originalOrganization;
            }
            set
            {
                this._originalOrganization = value;
            }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.Organization;
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
        /// Clone Organization object
        /// </summary>
        /// <returns>cloned copy of Organization copy.</returns>
        public IOrganization Clone()
        {
            Organization clonedOrganization = new Organization
            {
                Id = this.Id,
                OrganizationTypeId = this.OrganizationTypeId,
                OrganizationParent = this.OrganizationParent,
                ParentOrganizationName = this.ParentOrganizationName,
                OrganizationClassification = this.OrganizationClassification,
                Name = this.Name,
                LongName = this.LongName,
                Locale = this.Locale,
                Action = this.Action,
                AuditRefId = this.AuditRefId,
                ExtendedProperties = this.ExtendedProperties,
                Attributes = new AttributeCollection(this.Attributes.ToList()),
                GLN = this.GLN,
                ProcessorWeightage = this.ProcessorWeightage
            };

            return clonedOrganization;
        }

        /// <summary>
        /// Get Xml representation of Organization
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            CultureInfo culture = CultureInfo.InvariantCulture;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Organization node start
            xmlWriter.WriteStartElement("Organization");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString(culture));
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("GLN", this.GLN);
            xmlWriter.WriteAttributeString("OrganizationTypeId", this.OrganizationTypeId.ToString(culture));
            xmlWriter.WriteAttributeString("OrganizationClassification", this.OrganizationClassification.ToString(culture));
            xmlWriter.WriteAttributeString("OrganizationParent", this.OrganizationParent.ToString(culture));
            xmlWriter.WriteAttributeString("ParentOrganizationName", this.ParentOrganizationName);
            xmlWriter.WriteAttributeString("ProcessorWeightage", this.OrganizationParent.ToString(culture));
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            xmlWriter.WriteStartElement("Attributes");

            foreach (Attribute attribute in this.Attributes)
            {
                xmlWriter.WriteRaw(attribute.ToXml());
            }
            xmlWriter.WriteEndElement();

            //Organization node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Gets the attributes belonging to the Organization
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        public IAttributeCollection GetAttributes()
        {
            return this.Attributes == null ? null : (IAttributeCollection)this.Attributes;
        }

        /// <summary>
        /// Sets the attributes belonging to the Organization
        /// </summary>
        /// <param name="iAttributes">Collection of attributes to be set.</param>
        public void SetAttributes(IAttributeCollection iAttributes)
        {
            if (iAttributes != null && iAttributes.Count() > 0)
            {
                this.Attributes = (AttributeCollection)iAttributes;
            }
        }

        /// <summary>
        /// Gets attribute with specified attribute Id from current Organization's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current Organization's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">Attributes for Organization is null. There are no attributes to search in</exception>
        public IAttribute GetAttribute(Int32 attributeId)
        {
            return this.Attributes == null ? null : this.Attributes.GetAttribute(attributeId);
        }

        /// <summary>
        /// Get Xml representation of Organization
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetOrganization">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Indicates if Ids to be compared or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Organization subsetOrganization, Boolean compareIds = false)
        {
            if (subsetOrganization != null)
            {
                if (base.IsSuperSetOf(subsetOrganization, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.OrganizationTypeId != subsetOrganization.OrganizationTypeId)
                            return false;
                    }

                    if (!this.Attributes.IsSuperSetOf(subsetOrganization.Attributes))
                        return false;


                    if (this.OrganizationClassification != subsetOrganization.OrganizationClassification)
                        return false;

                    if (this.GLN != subsetOrganization.GLN)
                        return false;

                    if (this.ProcessorWeightage != subsetOrganization.ProcessorWeightage)
                        return false;

                    return true;
                }
            }

            return false;
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
                if (obj is Organization)
                {
                    Organization objectToBeCompared = obj as Organization;

                    if (this.ObjectType != objectToBeCompared.ObjectType)
                        return false;

                    if (this.OrganizationTypeId != objectToBeCompared.OrganizationTypeId)
                        return false;

                    if (this.OrganizationClassification != objectToBeCompared.OrganizationClassification)
                        return false;

                    if (this.OrganizationParent != objectToBeCompared.OrganizationParent)
                        return false;

                    if (this.ParentOrganizationName != objectToBeCompared.ParentOrganizationName)
                        return false;

                    if (this.GLN != objectToBeCompared.GLN)
                        return false;

                    if (this.ProcessorWeightage != objectToBeCompared.ProcessorWeightage)
                        return false;
                    
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Delta Merge of organization Values
        /// </summary>
        /// <param name="deltaOrganization">Organization that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged organization instance</returns>
        public IOrganization MergeDelta(IOrganization deltaOrganization, ICallerContext iCallerContext)
        {
            //Create clone copy of organization to start with..
            IOrganization mergedOrganization = deltaOrganization.Clone(); // This would clone all the properties and attributes.

            mergedOrganization.Action = (mergedOrganization.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            mergedOrganization.SetAttributes(deltaOrganization.GetAttributes());
                
            return mergedOrganization;
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
        ///  Serves as a hash function for Organization
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.Attributes.GetHashCode() ^ this.ProcessorWeightage.GetHashCode() ^ this.GLN.GetHashCode()
                ^ this.OrganizationClassification.GetHashCode() ^ this.OrganizationTypeId.GetHashCode() ^ this.OrganizationParent.GetHashCode()
                ^ this.ParentOrganizationName.GetHashCode();
        }
        #endregion

        #region Private Methods

        private void LoadOrganizationFromXml(String valueAsXml)
        {
            if (!String.IsNullOrEmpty(valueAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Organization")
                        {
                            #region Read Organization properties

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

                                if (reader.MoveToAttribute("GLN"))
                                {
                                    this.GLN = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OrganizationTypeId"))
                                {
                                    this.OrganizationTypeId = ValueTypeHelper.Int32TryParse(
                                        reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("OrganizationClassification"))
                                {
                                    this.OrganizationClassification = ValueTypeHelper.Int32TryParse(
                                        reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("OrganizationParent"))
                                {
                                    this.OrganizationParent = ValueTypeHelper.Int32TryParse(
                                        reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ParentOrganizationName"))
                                {
                                    this.ParentOrganizationName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ProcessorWeightage"))
                                {
                                    this.ProcessorWeightage = ValueTypeHelper.Int32TryParse(
                                        reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }
                            }

                            #endregion Read Organization properties
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attributes")
                        {
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
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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