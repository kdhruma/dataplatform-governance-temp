using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Specifies the Attribute Group
    /// </summary>
    [DataContract]
    public class AttributeGroup : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting FK_AttributeParent
        /// </summary>
        private int _parentAttributeGroupId = -1;

        /// <summary>
        /// Field denoting AttributeTypeName 
        /// </summary>
        private string _attributeTypeName = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public AttributeGroup()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes xml as input parameter
        /// </summary>
        /// <param name="valuesAsXml">Indicates values as xml</param>
        public AttributeGroup(String valuesAsXml)
            : base()
        {
            LoadAttributeGroup(valuesAsXml);
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Attribute group instance</param>
        public AttributeGroup(Int32 id)
            : base(id)
        {
            
        }

        /// <summary>
        /// Constructor with Id and LongName as input parameters
        /// </summary>
        /// <param name="Id">Indicates the Identity of an Attribute group instance</param>
        /// <param name="longName">Indicates the Long Name of an Attribute group instance</param>
        public AttributeGroup(Int32 Id, String longName)
            :base(Id)
        {
            this.LongName = longName;
            this.Name = longName;
        }

        /// <summary>
        /// Constructor with Id , ShortName and LongName as input parameters
        /// </summary>
        /// <param name="Id">Indicates the Identity of an Attribute group instance</param>
        /// <param name="shortName">Indicates the Short Name  of an Attribute group instance</param>
        /// <param name="longName">Indicates the Long Name of an Attribute group instance</param>
        public AttributeGroup(Int32 Id, String shortName, String longName)
            : base(Id, shortName, longName)
        {

        }

        #endregion

        #region Property

        /// <summary>
        /// Property denoting AttributeGroupParentId
        /// </summary>
        [DataMember]
        public int ParentAttributeGroupId
        {
            get
            {
                return this._parentAttributeGroupId;
            }
            set
            {
                this._parentAttributeGroupId = value;
            }
        }

        /// <summary>
        /// Property denoting AttributeType
        /// </summary>
        [DataMember]
        public string AttributeTypeName
        {
            get
            {
                return this._attributeTypeName;
            }
            set
            {
                this._attributeTypeName = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if(base.Equals(obj))
            {
                if (obj is AttributeGroup)
                {
                    AttributeGroup objectToBeCompared = obj as AttributeGroup;
                    
                    if (this.Id != objectToBeCompared.Id)
                        return false;

                    if (this.Name != objectToBeCompared.Name)
                        return false;

                    if (this.LongName != objectToBeCompared.LongName)
                        return false;

                    if (this.ParentAttributeGroupId != objectToBeCompared.ParentAttributeGroupId)
                        return false;

                    if (this.AttributeTypeName != objectToBeCompared.AttributeTypeName)
                        return false;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.        
        /// </summary>
        /// <param name="subsetAttributeGroup">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(AttributeGroup subsetAttributeGroup, Boolean compareIds = false)
        {
            if (subsetAttributeGroup != null)
            {
                if (base.IsSuperSetOf(subsetAttributeGroup, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.ParentAttributeGroupId != subsetAttributeGroup.ParentAttributeGroupId)
                            return false;
                    }

                    if (this.AttributeTypeName != subsetAttributeGroup.AttributeTypeName)
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
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this.Id.GetHashCode() ^ this.Name.GetHashCode() ^ this.LongName.GetHashCode() ^ this.ParentAttributeGroupId.GetHashCode() ^ this.AttributeTypeName.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Initialize AttributeGroup object from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for AttributeGroup object</param>
        public void LoadAttributeGroup(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeGroup")
                    {
                        #region Read attribute group prperties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("ParentAttributeGroupId"))
                            {
                                this.ParentAttributeGroupId = reader.ReadContentAsInt();
                            }

                            if (reader.MoveToAttribute("AttributeTypeName"))
                            {
                                this.AttributeTypeName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = reader.ReadContentAsInt();
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
                                String strLocale = reader.ReadContentAsString();
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse<LocaleEnum>(strLocale, true, out locale);
                                this.Locale = locale;
                            }
                        }

                        #endregion
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

        /// <summary>
        /// Represents AttributeGroup in Xml format
        /// </summary>
        /// <returns>String representation of current AttributeGroup object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Entity type unique identifier node start
            xmlWriter.WriteStartElement("AttributeGroup");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
            xmlWriter.WriteAttributeString("ParentAttributeGroupId", this.ParentAttributeGroupId.ToString());
            xmlWriter.WriteAttributeString("AttributeTypeName", this.AttributeTypeName);

            //Entity type unique identifier end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion
    }
}
