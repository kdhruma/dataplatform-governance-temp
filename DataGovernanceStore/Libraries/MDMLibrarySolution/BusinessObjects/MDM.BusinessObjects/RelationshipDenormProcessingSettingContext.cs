using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies configurations for Relationship Denorm ProcessingSettingContext
    /// </summary>
    [DataContract]
    public class RelationshipDenormProcessingSettingContext : MDMObject, IRelationshipDenormProcessingSettingContext
    {
        #region Fields

        /// <summary>
        /// Field specifying organizationIds
        /// </summary>
        private Collection<Int32> _organizationIds = new Collection<Int32>();

        /// <summary>
        /// Field specifying containerIds
        /// </summary>
        private Collection<Int32> _containerIds = new Collection<Int32>();

        /// <summary>
        /// Field specifying entityTypeIds 
        /// </summary>
        private Collection<Int32> _entityTypeIds = new Collection<Int32>();

        /// <summary>
        /// Field specifying categoryIds 
        /// </summary>
        private Collection<Int64> _categoryIds = new Collection<Int64>();

        /// <summary>
        /// Field specifying weightage of the current context
        /// </summary>
        private Int32 _weightage = 0;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property specifies organizationIds
        /// </summary>
        [DataMember]
        public Collection<Int32> OrganizationIds
        {
            get
            {
                return _organizationIds;
            }
            set
            {
                _organizationIds = value;
            }
        }

        /// <summary>
        /// Property specifies containerIds
        /// </summary>
        [DataMember]
        public Collection<Int32> ContainerIds
        {
            get
            {
                return _containerIds;
            }
            set
            {
                _containerIds = value;
            }
        }

        /// <summary>
        /// Property specifies entityTypeIds
        /// </summary>
        [DataMember]
        public Collection<Int32> EntityTypeIds
        {
            get
            {
                return _entityTypeIds;
            }
            set
            {
                _entityTypeIds = value;
            }
        }

        /// <summary>
        /// Property specifies categoryIds
        /// </summary>
        [DataMember]
        public Collection<Int64> CategoryIds
        {
            get
            {
                return _categoryIds;
            }
            set
            {
                _categoryIds = value;
            }
        }

        /// <summary>
        /// Property specifying weightage of the current context
        /// </summary>
        [DataMember]
        public Int32 Weightage
        {
            get
            {
                return _weightage;
            }
            set
            {
                _weightage = value;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes RelationshipDenormProcessingSettingContext object with default parameters
        /// </summary>
        public RelationshipDenormProcessingSettingContext() : base() { }

        /// <summary>
        /// Initializes RelationshipDenormProcessingSettingContext object with specified parameters
        /// </summary>
        /// <param name="organizationIds">Collection of organizationIds</param>
        /// <param name="containerIds">Collection of containerIds</param>
        /// <param name="entityTypeIds">Collection of entitytypeIds</param>
        /// <param name="categoryIds">Collection of categoryIds</param>
        /// <param name="weightage">Weightage of the current context</param>
        public RelationshipDenormProcessingSettingContext(Collection<Int32> organizationIds, Collection<Int32> containerIds, Collection<Int32> entityTypeIds, Collection<Int64> categoryIds, Int32 weightage)
        {
            this._organizationIds = organizationIds;
            this._containerIds = containerIds;
            this._entityTypeIds = entityTypeIds;
            this._categoryIds = categoryIds;
            this._weightage = weightage;
        }

        /// <summary>
        /// Initializes RelationshipDenormProcessingSettingContext object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        /// <param name="objectSerialization">Specifies Enum type od objectSerialization</param>
        public RelationshipDenormProcessingSettingContext(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadRelationshipDenormProcessingSettingContext(valuesAsXml, objectSerialization);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Represents RelationshipDenormProcessingSettingContext in Xml format
        /// </summary>
        /// <returns>String representation of current RelationshipDenormProcessingSettingContext object</returns>
        public override String ToXml()
        {
            String relationshipDenormProcessingSettingContextXml = String.Empty;

            String organizationIds = String.Empty;
            String containerIds = String.Empty;
            String entityTypeIds = String.Empty;
            String categoryIds = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            if (this.OrganizationIds != null)
            {
                organizationIds = ValueTypeHelper.JoinCollection(this.OrganizationIds, ",");
            }

            if (this.ContainerIds != null)
            {
                containerIds = ValueTypeHelper.JoinCollection(this.ContainerIds, ",");
            }

            if (this.EntityTypeIds != null)
            {
                entityTypeIds = ValueTypeHelper.JoinCollection(this.EntityTypeIds, ",");
            }

            if (this.CategoryIds != null)
            {
                categoryIds = ValueTypeHelper.JoinCollection(this.CategoryIds, ",");
            }

            //MDM Trace Config Item node start
            xmlWriter.WriteStartElement("SettingContext");

            #region Write MDM Trace Config Item properties

            xmlWriter.WriteAttributeString("OrganizationIds", String.IsNullOrWhiteSpace(organizationIds) ? "[RSAll]" : organizationIds);
            xmlWriter.WriteAttributeString("ContainerIds", String.IsNullOrWhiteSpace(containerIds) ? "[RSAll]" : containerIds);
            xmlWriter.WriteAttributeString("EntityTypeIds", String.IsNullOrWhiteSpace(entityTypeIds) ? "[RSAll]" : entityTypeIds);
            xmlWriter.WriteAttributeString("CategoryIds", String.IsNullOrWhiteSpace(categoryIds) ? "[RSAll]" : categoryIds);
            xmlWriter.WriteAttributeString("Weightage", this.Weightage.ToString());

            #endregion

            //MDM Trace Config Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            relationshipDenormProcessingSettingContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return relationshipDenormProcessingSettingContextXml;
        }

        /// <summary>
        /// Represents RelationshipDenormProcessingSettingContext in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current RelationshipDenormProcessingSettingContext object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String relationshipDenormProcessingSettingContextXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            relationshipDenormProcessingSettingContextXml = this.ToXml();

            return relationshipDenormProcessingSettingContextXml;
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
                if (obj is RelationshipDenormProcessingSettingContext)
                {
                    RelationshipDenormProcessingSettingContext objectToBeCompared = obj as RelationshipDenormProcessingSettingContext;

                    if (!this.OrganizationIds.Equals(objectToBeCompared.OrganizationIds))
                        return false;

                    if (!this.ContainerIds.Equals(objectToBeCompared.ContainerIds))
                        return false;

                    if (!this.EntityTypeIds.Equals(objectToBeCompared.EntityTypeIds))
                        return false;

                    if (!this.CategoryIds.Equals(objectToBeCompared.CategoryIds))
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
            hashCode = base.GetHashCode() ^ this.Locale.GetHashCode() ^ this.Action.GetHashCode() ^ this.OrganizationIds.GetHashCode()
                        ^ this.ContainerIds.GetHashCode() ^ this.EntityTypeIds.GetHashCode() ^ this.CategoryIds.GetHashCode() ^ this.Weightage.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the relationship denorm processing settingcontext with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadRelationshipDenormProcessingSettingContext(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml

            /*
             * <SettingContext OrganizationIds="[RSAll]" ContainerIds="[RSAll]"  EntityTypeIds="[RSAll]" CategoryIds="[RSAll]" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "SettingContext")
                        {
                            #region Read RelationshipDenormProcessingContext

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("OrganizationIds"))
                                {
                                    String strOrgIds = reader.ReadContentAsString();

                                    if (strOrgIds.ToLowerInvariant() != "[rsall]")
                                    {
                                        this.OrganizationIds = ValueTypeHelper.SplitStringToIntCollection(strOrgIds, ',');
                                    }
                                }

                                if (reader.MoveToAttribute("ContainerIds"))
                                {
                                    String strContainerIds = reader.ReadContentAsString();

                                    if (strContainerIds.ToLowerInvariant() != "[rsall]")
                                    {
                                        this.ContainerIds = ValueTypeHelper.SplitStringToIntCollection(strContainerIds, ',');
                                    }
                                }

                                if (reader.MoveToAttribute("EntityTypeIds"))
                                {
                                    String strEntityTypeIds = reader.ReadContentAsString();

                                    if (strEntityTypeIds.ToLowerInvariant() != "[rsall]")
                                    {
                                        this.EntityTypeIds = ValueTypeHelper.SplitStringToIntCollection(strEntityTypeIds, ',');
                                    }
                                }

                                if (reader.MoveToAttribute("CategoryIds"))
                                {
                                    String strCategoryIds = reader.ReadContentAsString();

                                    if (strCategoryIds.ToLowerInvariant() != "[rsall]")
                                    {
                                        this.CategoryIds = ValueTypeHelper.SplitStringToLongCollection(strCategoryIds, ',');
                                    }
                                }

                                if (reader.MoveToAttribute("Weightage"))
                                {
                                    this.Weightage = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
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

        #endregion Private Methods
    }
}
