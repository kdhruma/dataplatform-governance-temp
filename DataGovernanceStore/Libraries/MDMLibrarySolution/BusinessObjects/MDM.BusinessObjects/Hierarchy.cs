using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents hierarchy information
    /// </summary>
    [DataContract]
    public class Hierarchy : MDMObject, IHierarchy , IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field for container attributes
        /// </summary>
        private AttributeCollection _attributes = new AttributeCollection();
        
        /// <summary>
        /// Field indicates security object type.
        /// </summary>
        private Int32 _securityObjectTypeId = -1;

        /// <summary>
        /// Field indicates processor weightage
        /// </summary>
        private Int32 _processorWeightage = -1;

        /// <summary>
        /// Field indicates if entities can be added only for a leaf category
        /// </summary>
        private Boolean _leafNodeOnly = false;

        /// <summary>
        /// Field Denoting the original hierarchy
        /// </summary>
        private Hierarchy _originalHierarchy = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting hierarchy key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Hierarchy()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an hierarchy</param>
        public Hierarchy(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a Hierarchy as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Hierarchy</param>
        /// <param name="name">Indicates the Name of a Hierarchy</param>
        /// <param name="longName">Indicates the Description of a Hierarchy</param>
        public Hierarchy(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of a Hierarchy as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Hierarchy</param>
        /// <param name="name">Indicates the Name of a Hierarchy</param>
        /// <param name="longName">Indicates the LongName of a Hierarchy</param>
        /// <param name="locale">Indicates the Locale of a Hierarchy</param>
        public Hierarchy(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }

        /// <summary>
        /// Constructor with Xml as input
        /// <param name="valuesAsXml">Value in Xml format</param>
        /// </summary>
        public Hierarchy(String valuesAsXml)
        {
            LoadHierarchyFromXml(valuesAsXml);
        }

        #endregion Constructors

        #region Properties
        
        /// <summary>
        /// Gets or sets the security object type identifier.
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
        /// Property denoting the Attributes of a Container
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
        /// Gets or sets the denorm weightage.
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
        /// Property indicates if entities can be added only for a leaf category
        /// </summary>
        [DataMember]
        public Boolean LeafNodeOnly
        {
            get
            {
                return _leafNodeOnly;
            }
            set
            {
                _leafNodeOnly = value;
            }
        }

        /// <summary>
        /// Property denoting the original hierarchy
        /// </summary>
        public Hierarchy OriginalHierachy
        {
            get
            {
                return _originalHierarchy;
            }
            set
            {
                this._originalHierarchy = value;
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
                return MDM.Core.ObjectType.Taxonomy;
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

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents hierarchy in Xml format
        /// </summary>
        public override String ToXml()
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //hierarchy node start
            xmlWriter.WriteStartElement("Hierarchy");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString(culture));
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("SecurityObjectTypeId", this.SecurityObjectTypeId.ToString());
            xmlWriter.WriteAttributeString("LeafNodeOnly", this.LeafNodeOnly.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            xmlWriter.WriteStartElement("Attributes");

            foreach (Attribute attribute in this.Attributes)
            {
                xmlWriter.WriteRaw(attribute.ToXml());
            }
            xmlWriter.WriteEndElement();

            //ExportSubscriber node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            String xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents hierarchy in Xml format
        /// <param name="serialization">Type of serialization to be done</param>
        /// </summary>
        public override String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Determines whether two Object instances are equal.        
        /// </summary>
        /// <param name="subsetHierarchy">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(Hierarchy subsetHierarchy, Boolean compareIds = false)
        {
            if (subsetHierarchy != null)
            {
                if (base.IsSuperSetOf(subsetHierarchy, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.SecurityObjectTypeId != subsetHierarchy.SecurityObjectTypeId)
                        {
                            return false;
                        }
                    }
                    
                    if (!this.Attributes.IsSuperSetOf(subsetHierarchy.Attributes))
                    {
                        return false;
                    }

                    if (this.ProcessorWeightage != subsetHierarchy.ProcessorWeightage)
                    {
                        return false;
                    }

                    if (this.LeafNodeOnly != subsetHierarchy.LeafNodeOnly)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Clone hierarchy object
        /// </summary>
        /// <returns>cloned copy of hierarchy object.</returns>
        public IHierarchy Clone()
        {
            Hierarchy hierarchy = (Hierarchy)this.MemberwiseClone();
            hierarchy.Attributes = this.Attributes;
            return hierarchy;
        }

        /// <summary>
        /// Gets the attributes belonging to the Hierarchy
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        public IAttributeCollection GetAttributes()
        {
            return this.Attributes;
        }

        /// <summary>
        /// Sets the attributes belonging to the Hierarchy
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
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                Hierarchy objectToBeCompared = obj as Hierarchy;

                if (objectToBeCompared != null)
                {
                    if (this.SecurityObjectTypeId != objectToBeCompared.SecurityObjectTypeId)
                    {
                        return false;
                    }

                    if (this.ProcessorWeightage != objectToBeCompared.ProcessorWeightage)
                    {
                        return false;
                    }

                    if (this.LeafNodeOnly != objectToBeCompared.LeafNodeOnly)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Delta Merge of hierarchy Values
        /// </summary>
        /// <param name="deltaHierarchy">Hierarchy that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged organization instance</returns>
        public IHierarchy MergeDelta(IHierarchy deltaHierarchy, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IHierarchy mergedHierarchy = (returnClonedObject == true) ? deltaHierarchy.Clone() : deltaHierarchy;

            mergedHierarchy.Action = (mergedHierarchy.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            mergedHierarchy.SetAttributes(deltaHierarchy.GetAttributes());

            return mergedHierarchy;
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
        ///  Serves as a hash function for Hierarchy
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            hashCode = base.GetHashCode()
                    ^ this._attributes.GetHashCode()
                    ^ this._securityObjectTypeId.GetHashCode()
                    ^ this._processorWeightage.GetHashCode()
                    ^ this._externalId.GetHashCode()
                    ^ this._leafNodeOnly.GetHashCode();

            if (this._originalHierarchy != null)
            {
                hashCode = hashCode ^ this._originalHierarchy.GetHashCode();
            }            
            return hashCode;
        }
        #endregion PublicMethods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadHierarchyFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Hierarchy")
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
                                if (reader.MoveToAttribute("SecurityObjectTypeId"))
                                {
                                    this.SecurityObjectTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("LeafNodeOnly"))
                                {
                                    this.LeafNodeOnly = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out action);
                                    this.Action = action;
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

        #endregion Private Methods

        #endregion
    }
}