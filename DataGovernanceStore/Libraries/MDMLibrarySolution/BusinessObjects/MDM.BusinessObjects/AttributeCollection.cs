using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Attribute Value Instance Collection for the Object
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeCollection : ICollection<Attribute>, IEnumerable<Attribute>, IAttributeCollection
    {
        #region Fields

        //This is the most funny part of the WCF which is able to serialize even a private member.. needs to do further RnD to find out the internals
        [DataMember]
        [ProtoMember(1)]
        private Collection<Attribute> _attributes = new Collection<Attribute>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AttributeCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public AttributeCollection(String valueAsXml)
        {
            LoadAttributeCollection(valueAsXml);
        }


        /// <summary>
        /// constructor which takes Xml and boolean for input
        /// </summary>
        /// <param name="valueAsXml"></param>
        /// <param name="duplicateAttributeAllowed"></param>
        public AttributeCollection(String valueAsXml, Boolean duplicateAttributeAllowed)
        {
          LoadAttributeCollection(valueAsXml, duplicateAttributeAllowed);
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public AttributeCollection(String valueAsXml, Boolean duplicateAttributeAllowed, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadAttributeCollection(valueAsXml, duplicateAttributeAllowed, objectSerialization);
        }

        /// <summary>
        /// Initialize AttributeCollection from IList
        /// </summary>
        /// <param name="attributesList">IList of attributes</param>
        public AttributeCollection(IList<Attribute> attributesList)
        {
            this._attributes = new Collection<Attribute>(attributesList);
        }

        /// <summary>
        /// Initialize attribute collection from attribute model collection
        /// </summary>
        /// <param name="attributeModels">Attribute Models</param>
        public AttributeCollection(AttributeModelCollection attributeModels)
        {
            if (attributeModels != null && attributeModels.Count > 0)
            {
                foreach (AttributeModel attrModel in attributeModels)
                {
                    Attribute attr = new Attribute(attrModel, attrModel.Locale);

                    if (attr != null)
                        this.Add(attr);
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find attribute from AttributeCollection based on attributeId
        /// </summary>
        /// <param name="attributeId">AttributeID to search</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute object having given attributeID</returns>
        public Attribute this[Int32 attributeId, LocaleEnum locale]
        {
            get
            {
                Attribute attribute = Get(attributeId, locale);

                if (attribute == null)
                    throw new ArgumentException(String.Format("No attribute found for attribute id: {0} and Locale: {1}", attributeId, locale.ToString()), "attributeId");
                else
                    return attribute;
            }
            set
            {
                Attribute attribute = Get(attributeId, locale);

                if (attribute == null)
                    throw new ArgumentException(String.Format("No attribute found for attribute id: {0} and Locale: {1}", attributeId, locale.ToString()), "attributeId");

                attribute = value;
            }
        }

        /// <summary>
        /// Find attribute from AttributeCollection based on attributeId
        /// </summary>
        /// <param name="attributeId">AttributeID to search</param>
        /// <param name="instanceRefId">Instance ref id of an attribute</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute object having given attributeID</returns>
        public Attribute this[Int32 attributeId, Int32 instanceRefId, LocaleEnum locale]
        {
            get
            {
                Attribute attribute = Get(attributeId, locale, instanceRefId);

                if (attribute == null)
                    throw new ArgumentException(String.Format("No attribute found for attribute id: {0} and Locale:{1}", attributeId, locale.ToString()), "attributeId");
                else
                    return attribute;
            }
            set
            {
                Attribute attribute = Get(attributeId, locale, instanceRefId);

                if (attribute == null)
                    throw new ArgumentException(String.Format("No attribute found for attribute id: {0}", attributeId), "attributeId");

                attribute = value;
            }
        }

        /// <summary>
        /// Find attribute from AttributeCollection based on attributeUniqueIdentifier
        /// </summary>
        /// <param name="attributeUniqueIdentifier">AttributeUniqueIdentifier using which attribute is to be searched from AttributeCollection</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute object having given attributeUniqueIdentifier</returns>
        public Attribute this[AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale]
        {
            get
            {
                Attribute attribute = Get(attributeUniqueIdentifier, locale);
                if (attribute == null)
                    throw new ArgumentException(String.Format("No attribute found for attribute unique identifier [Name:'{0}', GroupName:'{1}'] and Locale: {2}", attributeUniqueIdentifier.AttributeName, attributeUniqueIdentifier.AttributeGroupName, locale.ToString()), "attributeUniqueIdentifier");
                else
                    return attribute;
            }
        }

        /// <summary>
        /// Find attribute from AttributeCollection based on attributeUniqueIdentifier
        /// </summary>
        /// <param name="iAttributeUniqueIdentifier">AttributeUniqueIdentifier using which attribute is to be searched from AttributeCollection</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute object having given attributeUniqueIdentifier</returns>
        public IAttribute this[IAttributeUniqueIdentifier iAttributeUniqueIdentifier, LocaleEnum locale]
        {
            get
            {
                AttributeUniqueIdentifier attributeUniqueIdentifier = null;

                if (iAttributeUniqueIdentifier is AttributeUniqueIdentifier)
                    attributeUniqueIdentifier = iAttributeUniqueIdentifier as AttributeUniqueIdentifier;
                else
                    throw new ArgumentException("Attribute Unique Identifier is not valid.");

                Attribute attribute = Get(attributeUniqueIdentifier, locale);

                if (attribute == null)
                    throw new ArgumentException(String.Format("No attribute found for attribute unique identifier [Name:'{0}', GroupName:'{1}'], Locale: {2}", attributeUniqueIdentifier.AttributeName, attributeUniqueIdentifier.AttributeGroupName, locale.ToString()), "attributeUniqueIdentifier");
                else
                    return attribute;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if AttributeCollection contains attribute with given attributeId
        /// </summary>
        /// <param name="attributeId">AttributeId to search in AttributeCollection</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>
        /// <para>true : If attribute found in attributeCollection</para>
        /// <para>false : If attribute found not in attributeCollection</para>
        /// </returns>
        public Boolean Contains(Int32 attributeId, LocaleEnum locale)
        {
            if (Get(attributeId, locale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if AttributeCollection contains attribute with given attributeId
        /// </summary>
        /// <param name="attributeId">AttributeId to search in AttributeCollection</param>
        /// <returns>
        /// <para>true : If attribute found in attributeCollection</para>
        /// <para>false : If attribute found not in attributeCollection</para>
        /// </returns>
        public Boolean Contains(Int32 attributeId)
        {
            if (Get(attributeId, LocaleEnum.UnKnown) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if AttributeCollection contains attribute with given attributeId
        /// </summary>
        /// <param name="attributeId">AttributeId to search in AttributeCollection</param>
        /// <param name="instanceRefId">InstanceRefId + AttributeId to search in attribute collection. This is optional parameter. Default value is -1</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>
        /// <para>true : If attribute found in attributeCollection</para>
        /// <para>false : If attribute found not in attributeCollection</para>
        /// </returns>
        public Boolean Contains(Int32 attributeId, Int32 instanceRefId, LocaleEnum locale)
        {
            if (Get(attributeId, locale, instanceRefId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if AttributeCollection contains attribute with given name and parent name
        /// </summary>
        /// <param name="name">AttributeName to search in AttributeCollection</param>
        /// <param name="parentName">AttributeParentName to search in AttributeCollection</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns></returns>
        public Boolean Contains(String name, String parentName, LocaleEnum locale)
        {
            if (Get(name, parentName, locale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove attribute object from AttributeCollection
        /// </summary>
        /// <param name="attributeId">Id of attribute which is to be removed from collection</param>
        /// <param name="locale">Locale for which attribute is to be removed from collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 attributeId, LocaleEnum locale)
        {
            Attribute attribute = Get(attributeId, locale);

            if (attribute == null)
                throw new ArgumentException("No attribute found for given attribute id");
            else
                return this.Remove(attribute);
        }

        /// <summary>
        /// Remove attribute object from AttributeCollection
        /// </summary>
        /// <param name="attributeId">Id of attribute which is to be removed from collection</param>
        /// <param name="instanceRefId">InstanceRefId + AttributeId to remove from attribute collection. This is optional parameter. Default value is -1</param>
        /// <param name="locale">Locale for which attribute is to be removed from attribute collection</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public Boolean Remove(Int32 attributeId, Int32 instanceRefId, LocaleEnum locale)
        {
            Attribute attribute = Get(attributeId, locale, instanceRefId);

            if (attribute == null)
                throw new ArgumentException("No attribute found for given attribute id");
            else
                return this.Remove(attribute);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is AttributeCollection)
            {
                AttributeCollection objectToBeCompared = obj as AttributeCollection;
                Int32 attributesUnion = this._attributes.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributesIntersect = this._attributes.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (attributesUnion != attributesIntersect)
                    return false;
                return true;
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
            foreach (Attribute attr in this._attributes)
            {
                hashCode += attr.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Initialize attributeCollection from Xml
        /// </summary>
        /// <param name="valueAsXml">Xml having value for AttributeCollection
        /// <para>Sample Xml : </para>
        /// <![CDATA[
        /// <Attribute Id="1125" Name="CORE_TECH_TXTBX_Small" LongName="CORE_TECH_Textbox Attribute Small" AttributeParentId="1124" AttributeParentName="CORE_TECH_String Attribute Group" AttributeParentLongName="CORE_TECH_String Attribute Group" IsCollection="False" IsComplex="False" isLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" AttributeType="TechAttribute" AttributeDataType="String" AttributeModelType="Category" SourceFlag="O" Action="Create">
        ///     <Values SourceFlag="O">
        ///         <Value ValueRefId="0" Sequence="-1.00" Locale="en_WW">CORE_TECH_Textbox Attribute Small</Value>
        ///     </Values>
        ///     <Values SourceFlag="I" />
        /// </Attribute>
        /// ]]>
        /// </param>
        /// <param name="objectSerialization">Enums of object Serialization</param>
        public void LoadAttributeCollection(String valueAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadAttributeCollection(valueAsXml, false, objectSerialization);
        }

        /// <summary>
        /// Initialize attributeCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for AttributeCollection
        /// <![CDATA[
        /// <Attribute Id="1125" Name="CORE_TECH_TXTBX_Small" LongName="CORE_TECH_Textbox Attribute Small" AttributeParentId="1124" AttributeParentName="CORE_TECH_String Attribute Group" AttributeParentLongName="CORE_TECH_String Attribute Group" IsCollection="False" IsComplex="False" isLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" AttributeType="TechAttribute" AttributeDataType="String" AttributeModelType="Category" SourceFlag="O" Action="Create">
        ///     <Values SourceFlag="O">
        ///         <Value ValueRefId="0" Sequence="-1.00" Locale="en_WW">CORE_TECH_Textbox Attribute Small</Value>
        ///     </Values>
        ///     <Values SourceFlag="I" />
        /// </Attribute>
        /// ]]>
        /// </param>
        /// <param name="duplicateAttributeAllowed">Flag specifies duplicate attribute check logic</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public void LoadAttributeCollection(String valuesAsXml, Boolean duplicateAttributeAllowed, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            #region Sample Xml
            /*
			 * <Attribute Id="1125" Name="CORE_TECH_TXTBX_Small" LongName="CORE_TECH_Textbox Attribute Small" AttributeParentId="1124" AttributeParentName="CORE_TECH_String Attribute Group" AttributeParentLongName="CORE_TECH_String Attribute Group" IsCollection="False" IsComplex="False" isLocalizable="True" ApplyLocaleFormat="True" ApplyTimeZoneConversion="False" AttributeType="TechAttribute" AttributeDataType="String" AttributeModelType="Category" SourceFlag="O" Action="Create">
				<Values SourceFlag="O">
				  <Value ValueRefId="0" Sequence="-1.00" Locale="en_WW">CORE_TECH_Textbox Attribute Small</Value>
				</Values>
				<Values SourceFlag="I" />
			  </Attribute>
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                        {
                            String attributeXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(attributeXml))
                            {
                                Attribute attribute = new Attribute(attributeXml, duplicateAttributeAllowed, objectSerialization);
                                if (attribute != null)
                                {
                                    this.Add(attribute, duplicateAttributeAllowed);
                                }
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

        /// <summary>
        /// Loads AttributeCollection object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadAttributeCollectionFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.IsEmptyElement)
                {
                    return;
                }

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Attribute")
                    {
                        if (reader.HasAttributes)
                        {
                            Attribute attribute = new Attribute();

                            attribute.LoadAttributeFromXml(reader);

                            this.Add(attribute);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Attributes")
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read AttributeCollection object.");
            }
        }

        /// <summary>
        /// Get distinct collection of locales from all the attributes in the collection
        /// </summary>
        /// <returns>Locale collection  from current attribute collection</returns>
        public Collection<LocaleEnum> GetLocaleList()
        {
            var locales = new Collection<LocaleEnum>();

            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (var attr in this._attributes)
                {
                    if (!locales.Contains(attr.Locale))
                    {
                        locales.Add(attr.Locale);
                    }
                }
            }
            else
            {
                locales.Add(LocaleEnum.UnKnown);
            }

            return locales;
        }

        /// <summary>
        /// Compare attributeCollection with current collection.
        /// This method will compare attribute and Values. If current collection has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetAttributeCollection">AttributeCollection to be compared with current collection</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public bool IsSuperSetOf(AttributeCollection subsetAttributeCollection)
        {
            if (subsetAttributeCollection != null)
            {
                foreach (Attribute attr in subsetAttributeCollection)
                {
                    //Get sub set attribute from super attribute collection.
                    IAttribute iAttribute = this.GetAttribute(MDMObjectFactory.GetIAttributeUniqueIdentifier(attr.Name, attr.AttributeParentName), attr.Locale);

                    //If it doesn't return, that means super set doesn't contain that attribute.
                    //So return false, else do further comparison
                    if (iAttribute != null)
                    {
                        Attribute sourceAttribute = (Attribute)iAttribute;

                        if (!sourceAttribute.IsSuperSetOf(attr))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// unit test assertion helper: this will compare 2 attribute collections, mismatch attributes are logged in entityOperationResult as errors
        /// </summary>
        /// <param name="subsetAttributeCollection"></param>
        /// <param name="entityOperationResult"></param>
        /// <returns></returns>
        public AttributeOperationResultCollection GetSuperSetOperationResult(AttributeCollection subsetAttributeCollection, EntityOperationResult entityOperationResult)
        {
            entityOperationResult.AttributeOperationResultCollection.OperationResultStatus = OperationResultStatusEnum.Successful;

            if (subsetAttributeCollection != null)
            {
                foreach (Attribute subsetAttr in subsetAttributeCollection)
                {
                    //Get sub set attribute from super attribute collection.
                    var iAttribute = this.GetAttribute(MDMObjectFactory.GetIAttributeUniqueIdentifier(subsetAttr.Name, subsetAttr.AttributeParentName), subsetAttr.Locale);

                    //If it doesn't return, that means super set doesn't contain that attribute.
                    //So return false, else do further comparison
                    if (iAttribute != null)
                    {
                        var sourceAttribute = (Attribute)iAttribute;

                        var attributeOperationResult = new AttributeOperationResult(sourceAttribute.Id, sourceAttribute.Name, sourceAttribute.LongName, sourceAttribute.AttributeModelType, sourceAttribute.Locale);
                        sourceAttribute.GetSuperSetOperationResult(subsetAttr, attributeOperationResult);

                        if (attributeOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                        {
                            entityOperationResult.AttributeOperationResultCollection.Add(attributeOperationResult);
                            entityOperationResult.AttributeOperationResultCollection.RefreshOperationResultStatus();
                            entityOperationResult.RefreshOperationResultStatus();
                        }
                    }
                    else
                    {
                        entityOperationResult.AttributeOperationResultCollection.AddAttributeOperationResult(0, "-1", String.Format("Attribute {0} is null", subsetAttr.Name), OperationResultType.Error);
                    }
                }
            }

            //if (attributeOperationResults.OperationResultStatus != OperationResultStatusEnum.Successful)
            //{
            //    entityOperationResult.SetAttributeOperationResults(attributeOperationResults);
            //    attributeOperationResults.RefreshOperationResultStatus();
            //}

            return entityOperationResult.AttributeOperationResultCollection;
        }

        /// <summary>
        /// Clone attribute collection object
        /// </summary>
        /// <returns>Returns cloned copy of attribute collection</returns>
        public IAttributeCollection Clone()
        {
            AttributeCollection clonedAttributes = new AttributeCollection();
            if (this._attributes != null)
            {
                foreach (Attribute attr in this._attributes)
                {
                    clonedAttributes.Add(attr.Clone(), true);
                }
            }

            return clonedAttributes;
        }

        /// <summary>
        /// check if attributes have been changed
        /// </summary>
        /// <returns>True : if at one attribute has action is Create OR Update OR Delete. False : otherwise</returns>
        public Boolean HasChanged()
        {
            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (Attribute attribute in this._attributes)
                {
                    if (attribute.CheckHasChanged())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if any attribute is required
        /// </summary>
        /// <returns>True : If one of the attribute is required otherwise result is false.</returns>
        public Boolean HasRequiredAttributes()
        {
            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (Attribute attribute in this._attributes)
                {
                    if (attribute.Required == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get attribute from current attribute collection based on AttributeId
        /// </summary>
        /// <param name="attributeId">Id of attribute which is to be searched</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute having given AttribtueIdand InstanceRefId</returns>
        private Attribute Get(Int32 attributeId, LocaleEnum locale)
        {
            return Get(attributeId, locale, -1);
        }

        /// <summary>
        /// Get attribute from current attribute collection based on AttributeId and InstanceRefId
        /// </summary>
        /// <param name="attributeId">Id of attribute which is to be searched</param>
        /// <param name="instanceRefId">Instance ref id of an attribute. Default value is -1. This parameter is optional</param>
        /// <returns>Attribute having given AttribtueIdand InstanceRefId</returns>
        private Attribute Get(Int32 attributeId, Int32 instanceRefId = -1)
        {
            /*
             * Flow: (condition flow will be same for AttributeId + Locale search and AttributeId + Locale + InstanceRefId search
             *      - Search for given AttributeId in collection. 
             *      - If found, 
             *              return it.
             *      - If not found.
             *              Search for given AttributeId
             *              If not found 
             *                  stop
             *              if Found
             *                  Check if attribute is localizable
             *                  If NOT Localizable
             *                      Take attribute, and return it
             *                  If Localizable
             *                      throw exception and stop.
             *      - If multiple attribute found throw error.
             */

            IEnumerable<Attribute> filteredAttributes = null;
            IEnumerable<Attribute> expectedAttributes = null;
            Attribute expectedAttribute = null;

            if (instanceRefId == -1)
            {
                //Filter attribute for given AttributeId + Locale.
                filteredAttributes = this._attributes.Where(a => a.Id == attributeId);

                //If attribute with given attributeId is found, and it is not localizable then return it,
                //Else throw exception.
                if (filteredAttributes != null && filteredAttributes.Count() == 1)
                {
                    expectedAttribute = filteredAttributes.FirstOrDefault();

                    if (expectedAttribute != null && expectedAttribute.IsLocalizable)
                    {
                        throw new MDMOperationException("111712", "Not able to find appropriate attribute as attribute is localized. Please provide locale details.", "AttributeCollection", String.Empty, "Get");
                    }
                }
                else
                {
                    expectedAttributes = filteredAttributes;
                }
            }
            else
            {
                //Filter attribute for given AttributeId + InstanceRefId.
                filteredAttributes = this._attributes.Where(a => a.Id == attributeId && a.InstanceRefId == instanceRefId);

                //If attribute with given attributeId + InstanceRefId is found, and it is not localizable then return it,
                //Else throw exception.
                if (filteredAttributes != null && filteredAttributes.Count() == 1)
                {
                    expectedAttribute = filteredAttributes.FirstOrDefault();

                    if (expectedAttribute != null && expectedAttribute.IsLocalizable == false)
                    {
                        throw new MDMOperationException("111712", "Not able to find appropriate attribute as attribute is localized. Please provide locale details.", "AttributeCollection", String.Empty, "Get");
                    }
                }
                else
                {
                    expectedAttributes = filteredAttributes;
                }
            }

            if (expectedAttributes != null && expectedAttributes.Count() > 1)
            {
                throw new MDMOperationException("111712", "Not able to find appropriate attribute as attribute is localized. Please provide locale details.", "AttributeCollection", String.Empty, "Get");
            }

            return expectedAttribute;
        }

        /// <summary>
        /// Get attribute from current attribute collection based on AttributeId and InstanceRefId
        /// </summary>
        /// <param name="attributeId">Id of attribute which is to be searched</param>
        /// <param name="instanceRefId">Instance ref id of an attribute. Default value is -1. This parameter is optional</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute having given AttribtueIdand InstanceRefId</returns>
        private Attribute Get(Int32 attributeId, LocaleEnum locale, Int32 instanceRefId = -1)
        {
            /*
             * Flow: (condition flow will be same for AttributeId + Locale search and AttributeId + Locale + InstanceRefId search
             *      - Search for given AttributeId + Locale in collection. 
             *      - If found, 
             *              return it.
             *      - If not found.
             *              Search for given AttributeId
             *              If not found 
             *                  stop
             *              if Found
             *                  Check if attribute is localizable
             *                  If NOT Localizable
             *                      Take attribute, and return it
             *                  If Localizable
             *                      return null and stop.
             */
            Attribute expectedAttribute = null;

            if (instanceRefId == -1)
            {
                //Filter attribute for given AttributeId + Locale.
                expectedAttribute = GetAttributeByParams(attributeId, locale);
                if (expectedAttribute == null)
                {
                    //search for given attributeId.
                    expectedAttribute = GetAttributeByParams(attributeId);

                    //If attribute with given attributeId is found, and it is not localizable then return it,
                    //Else return null.
                    if (expectedAttribute != null && expectedAttribute.IsLocalizable == true)
                        expectedAttribute = null;
                }
            }
            else
            {
                //Filter attribute for given AttributeId + Locale + InstanceRefId.
                expectedAttribute = GetAttributeByParams(attributeId, instanceRefId, locale);
                if (expectedAttribute == null)
                {
                    //search for given attributeId + InstanceRefId.
                    expectedAttribute = GetAttributeByParams(attributeId, instanceRefId);

                    //If attribute with given attributeId + InstanceRefId is found, and it is not localizable then return it,
                    //Else return null.
                    if (expectedAttribute != null && expectedAttribute.IsLocalizable == true)
                        expectedAttribute = null;
                }
            }

            return expectedAttribute;
        }

        /// <summary>
        /// Get attribute from current attribute collection based on AttributeId and InstanceRefId
        /// </summary>
        /// <param name="name">Name of attribute which is to be searched</param>
        /// <param name="parentName">Parent Name of the attribute.</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute having given name parent</returns>
        private Attribute Get(string name, string parentName, LocaleEnum locale)
        {
            Attribute attribute = null;
            Int32 attributeCount = this._attributes.Count;

            for (Int32 index = 0; index < attributeCount; index++)
            {
                attribute = _attributes[index];
                if (attribute.Name == name && attribute.AttributeParentName == parentName && attribute.Locale == locale)
                    return attribute;
            }
            return null;
        }

        private Attribute GetAttributeByParams(Int32 attributeId)
        {
            Attribute attribute = null;
            Int32 attributeCount = this._attributes.Count;

            for (Int32 index = 0; index < attributeCount; index++)
            {
                attribute = _attributes[index];
                if (attribute.Id == attributeId)
                    return attribute;
            }
            return null;
        }

        private Attribute GetAttributeByParams(Int32 attributeId, Int32 instanceRefId)
        {
            Attribute attribute = null;
            Int32 attributeCount = this._attributes.Count;

            for (Int32 index = 0; index < attributeCount; index++)
            {
                attribute = _attributes[index];
                if (attribute.Id == attributeId && attribute.InstanceRefId == instanceRefId)
                    return attribute;
            }
            return null;
        }

        private Attribute GetAttributeByParams(Int32 attributeId, LocaleEnum locale)
        {
            Attribute attribute = null;
            Int32 attributeCount = this._attributes.Count;

            for (Int32 index = 0; index < attributeCount; index++)
            {
                attribute = _attributes[index];
                if (attribute.Id == attributeId && attribute.Locale == locale)
                    return attribute;
            }
            return null;
        }

        private Attribute GetAttributeByParams(Int32 attributeId, Int32 instanceRefId, LocaleEnum locale)
        {
            Attribute attribute = null;
            Int32 attributeCount = this._attributes.Count;

            for (Int32 index = 0; index < attributeCount; index++)
            {
                attribute = _attributes[index];
                if (attribute.Id == attributeId && attribute.InstanceRefId == instanceRefId && attribute.Locale == locale)
                    return attribute;
            }
            return null;
        }

        private Attribute GetAttributeByParams(String attributeName, String attributeParentName, Int32 instanceRefId, LocaleEnum locale)
        {
            Attribute attribute = null;
            Int32 attributeCount = this._attributes.Count;

            for (Int32 index = 0; index < attributeCount; index++)
            {
                attribute = _attributes[index];
                if (attribute.Name == attributeName && attribute.InstanceRefId == instanceRefId && attribute.Locale == locale)
                {
                    if (!String.IsNullOrWhiteSpace(attributeParentName))
                    {
                        if (attribute.AttributeParentName == attributeParentName)
                        {
                            return attribute;
                        }
                    }
                    else
                    {
                        return attribute;
                    }
                }
            }

            return null;
        }

        private Attribute GetAttributeByParams(String attributeName, String attributeParentName, Int32 instanceRefId)
        {
            Attribute attribute = null;
            Int32 attributeCount = this._attributes.Count;

            for (Int32 index = 0; index < attributeCount; index++)
            {
                attribute = _attributes[index];
                if (attribute.Name == attributeName && attribute.InstanceRefId == instanceRefId)
                {
                    if (!String.IsNullOrWhiteSpace(attributeParentName))
                    {
                        if (attribute.AttributeParentName == attributeParentName)
                        {
                            return attribute;
                        }
                    }
                    else
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get attribute from current attribute collection based on AttributeUniqueIdentifier
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Attribute unique identifier of an attribute which we want to search in current collection</param>
        /// <returns>First occurrence of attribute having value same as AttribtueUniqueIdentifier</returns>
        private Attribute Get(AttributeUniqueIdentifier attributeUniqueIdentifier)
        {
            /*
             * Flow: 
             *      - Search for given ShortName + ParentName  in collection. 
             *      - If found, 
             *              return it.
             *      - If not found.
             *              Search for given ShortName + ParentName
             *              If not found 
             *                  stop
             *              if Found
             *                  Check if attribute is localizable
             *                  If NOT Localizable
             *                      Take attribute, and return it
             *                  If Localizable
             *                      return null and stop.
             *      -If more than one attribute found thow Error.
             */
            IEnumerable<Attribute> filteredAttributes = null;
            Attribute expectedAttribute = null;

            String attributeName = String.Empty;
            String attributeGroupName = String.Empty;
            Int32 instanceRefId = -1;

            if (attributeUniqueIdentifier != null)
            {
                attributeName = attributeUniqueIdentifier.AttributeName;
                attributeGroupName = attributeUniqueIdentifier.AttributeGroupName;
                instanceRefId = attributeUniqueIdentifier.InstanceRefId;
            }

            //Filter attribute for given ShortName + ParentName 
            if (!String.IsNullOrWhiteSpace(attributeGroupName))
            {
                filteredAttributes = from attr in this._attributes
                                     where
                                        attr.Name.Equals(attributeName) &&
                                        attr.AttributeParentName.Equals(attributeGroupName) &&
                                        attr.InstanceRefId.Equals(instanceRefId)
                                     select attr;
            }
            else
            {
                filteredAttributes = from attr in this._attributes
                                     where
                                        attr.Name.Equals(attributeName) &&
                                        attr.InstanceRefId.Equals(instanceRefId)
                                     select attr;
            }

            //If attribute with given attributeId is found, and it is not localizable then return it,
            //Else return null.
            if (filteredAttributes != null && filteredAttributes.Count() == 1)
            {
                expectedAttribute = filteredAttributes.FirstOrDefault();

                if (expectedAttribute != null && expectedAttribute.IsLocalizable == true)
                {
                    throw new MDMOperationException("111712", "Not able to find appropriate attribute as attribute is localized. Please provide locale details.", "AttributeCollection", String.Empty, "Get");
                }
            }
            else if (filteredAttributes != null && filteredAttributes.Count() > 1)
            {
                throw new MDMOperationException("111712", "Not able to find appropriate attribute as attribute is localized. Please provide locale details.", "AttributeCollection", String.Empty, "Get");
            }

            return expectedAttribute;
        }

        /// <summary>
        /// Get attribute from current attribute collection based on AttributeUniqueIdentifier
        /// </summary>
        /// <param name="attributeUniqueIdentifier">Attribute unique identifier of an attribute which we want to search in current collection</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>First occurrence of attribute having value same as AttribtueUniqueIdentifier</returns>
        private Attribute Get(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale)
        {
            /*
             * Flow: 
             *      - Search for given ShortName + ParentName + Locale in collection. 
             *      - If found, 
             *              return it.
             *      - If not found.
             *              Search for given ShortName + ParentName
             *              If not found 
             *                  stop
             *              if Found
             *                  Check if attribute is localizable
             *                  If NOT Localizable
             *                      Take attribute, and return it
             *                  If Localizable
             *                      return null and stop.
             */
            Attribute expectedAttribute = null;

            String attributeName = String.Empty;
            String attributeGroupName = String.Empty;
            Int32 instanceRefId = -1;

            if (attributeUniqueIdentifier != null)
            {
                attributeName = attributeUniqueIdentifier.AttributeName;
                attributeGroupName = attributeUniqueIdentifier.AttributeGroupName;
                instanceRefId = attributeUniqueIdentifier.InstanceRefId;
            }

            //Filter attribute for given ShortName + ParentName + Locale.
            expectedAttribute = GetAttributeByParams(attributeName, attributeGroupName, instanceRefId, locale);
            if (expectedAttribute == null)
            {
                //IF attribute with given ShortName + ParentName + Locale not found, try to search for given ShortName + ParentName.
                expectedAttribute = GetAttributeByParams(attributeName, attributeGroupName, instanceRefId);

                //If attribute with given attributeId is found, and it is not localizable then return it,
                //Else return null.
                if (expectedAttribute != null && expectedAttribute.IsLocalizable == true)
                    expectedAttribute = null;
            }
            return expectedAttribute;
        }

        /// <summary>
        /// Get Attribute from current attribute collection based on name
        /// </summary>
        /// <param name="attributeName">Specifies Attribute Short Name</param>
        /// <returns>Attribute</returns>
        private Attribute Get(String attributeName)
        {
            /*
			 * Flow: 
			 *      - Search for given AttributeName
			 *              If not found 
			 *                  stop
			 *              if Found
			 *                  Check if attribute is localizable
			 *                  If NOT Localizable
			 *                      Take attribute, and return it
			 *                  If Localizable
			 *                      throw exception and stop.
			 *      - If multiple attribute found throw error.
			 */

            IEnumerable<Attribute> filteredAttributes = null;
            Attribute expectedAttribute = null;
            Int32 filteredAttributesCount = -1;

            //Filter attribute for given AttributeName
            filteredAttributes = this._attributes.Where(a => a.Name == attributeName);

            if (filteredAttributes != null)
                filteredAttributesCount = filteredAttributes.Count();

            //If attribute with given attributeName is found, and it is not localizable then return it,
            //Else throw exception.
            if (filteredAttributesCount == 1)
            {
                expectedAttribute = filteredAttributes.FirstOrDefault();

                if (expectedAttribute != null && expectedAttribute.IsLocalizable)
                {
                    throw new MDMOperationException("111712", "Not able to find appropriate attribute as attribute is localized. Please provide locale details.", "AttributeCollection", String.Empty, "Get");
                }
            }
            else if (filteredAttributesCount > 1)
            {
                //One or more attributes found with the same name
                //TODO : Get Message Code
                String duplicateAttributeName = filteredAttributes.FirstOrDefault().Name;
                throw new MDMOperationException("", String.Format("Not able to find appropriate '{0}' attribute as duplicate attributes found. Please provide attribute parent and locale details.", duplicateAttributeName), "AttributeCollection", String.Empty, "Get");
            }

            return expectedAttribute;
        }

        /// <summary>
        /// Get Attribute from current attribute collection based on name and locale
        /// </summary>
        /// <param name="attributeName">Specifies Attribute Short Name</param>
        /// <param name="locale">Specifies Attribute Locale</param>
        /// <returns>Attribute</returns>
        private Attribute Get(String attributeName, LocaleEnum locale)
        {
            /*
			 * Flow: 
			 *      - Search for given ShortName in collection. 
			 *      - If found, 
			 *              return it.
			 *      - If not found.
			 *              Search for given ShortName
			 *              If not found 
			 *                  stop
			 *              if Found
			 *                  Check if attribute is localizable
			 *                  If NOT Localizable
			 *                      Take attribute, and return it
			 *                  If Localizable
			 *                      return null and stop.
			 */
            IEnumerable<Attribute> filteredAttributes = null;
            Attribute expectedAttribute = null;
            Int32 filteredAttributesCount = -1;

            //Filter attribute for given ShortName 
            filteredAttributes = from attr in this._attributes where attr.Name.Equals(attributeName) select attr;

            if (filteredAttributes != null)
                filteredAttributesCount = filteredAttributes.Count();

            if (filteredAttributesCount == 1)
            {
                expectedAttribute = filteredAttributes.FirstOrDefault();

                //If attribute with given attribute name is found, and it is not localizable then return it,
                //Else return null.
                if (expectedAttribute.Locale != locale && expectedAttribute.IsLocalizable)
                    expectedAttribute = null;
            }
            else if (filteredAttributesCount > 1)
            {
                Int32 attribueswithLocaleCount = -1;

                //Find the Attribute having name and locale
                IEnumerable<Attribute> attributes = from attr in filteredAttributes
                                                    where
                                                       attr.Locale.Equals(locale)
                                                    select attr;

                if (attributes != null)
                    attribueswithLocaleCount = attributes.Count();

                if (attribueswithLocaleCount > 1)
                {
                    //One or more attributes found with the same name and locale
                    //TODO : Get Message Code
                    String duplicateAttributeName = filteredAttributes.FirstOrDefault().Name;
                    throw new MDMOperationException("", String.Format("Not able to find appropriate '{0}' attribute as duplicate attributes found. Please provide attribute parent and locale details.", duplicateAttributeName), "AttributeCollection", String.Empty, "Get");
         
                }
                else if (attribueswithLocaleCount == 1)
                {
                    //Attribute found with name and locale
                    expectedAttribute = attributes.FirstOrDefault();
                }
            }

            return expectedAttribute;
        }

        #endregion

        #region ICollection<Attribute> Members

        /// <summary>
        /// Add attribute object in collection
        /// </summary>
        /// <param name="item">attribute to add in collection</param>
        public void Add(Attribute item)
        {
            this.Add(item, false);
        }

        /// <summary>
        /// Add attribute object in collection
        /// </summary>
        /// <param name="item">attribute to add in collection</param>
        /// <param name="ignoreDuplicateCheck"></param>
        public void Add(Attribute item, Boolean ignoreDuplicateCheck)
        {
            IAttribute attribute = null;

            if (!ignoreDuplicateCheck)
            {
                if (item.Name.Length > 0)
                    attribute = GetAttribute(new AttributeUniqueIdentifier(item.Name, item.AttributeParentName, item.InstanceRefId), item.Locale);
                else
                    attribute = GetAttribute(item.Id, item.Locale);
            }

            if (attribute != null && attribute.Sequence == item.Sequence)
            {
                //Multiple Attribute with same AttributeName, AttributeParentName and Locale and sequence found.
                //throw new DuplicateObjectException("110316");

                //TODO :: Should it throw exception ?? Need to check why it is getting duplicate models sometime.
                //throw new DuplicateObjectException(String.Format("Multiple Attribute with same AttributeName={0}, AttributeParentName={1} and Locale={2} found.", item.Name, item.AttributeParentName, item.Locale ));
            }
            else
            {
                this._attributes.Add(item);
            }
        }

        /// <summary>
        /// Add attribute Instance in collection
        /// </summary>
        /// <param name="item">Attribute to add in collection</param>
        public void Add(IAttribute item)
        {
            this.Add((Attribute)item);
        }

        /// <summary>
        /// Add attributes object in collection
        /// </summary>
        /// <param name="items">attributes to add in collection</param>
        public void AddRange(IAttributeCollection items)
        {
            this.AddRange(items, false);
        }

        /// <summary>
        /// Add attributes object in collection
        /// </summary>
        /// <param name="items">attributes to add in collection</param>
        /// <param name="ignoreDuplicateCheck">flag to check duplicates or not</param>
        public void AddRange(IAttributeCollection items, Boolean ignoreDuplicateCheck)
        {
            if (items == null)
            {
                throw new ArgumentNullException("Attributes");
            }

            foreach (Attribute attribute in items)
            {
                this.Add(attribute, ignoreDuplicateCheck);
            }
        }

        /// <summary>
        /// Removes all attributes from collection
        /// </summary>
        public void Clear()
        {
            this._attributes.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeCollection contains a specific attribute.
        /// </summary>
        /// <param name="item">The attribute object to locate in the AttributeCollection.</param>
        /// <returns>
        /// <para>true : If attribute found in attributeCollection</para>
        /// <para>false : If attribute found not in attributeCollection</para>
        /// </returns>
        public bool Contains(Attribute item)
        {
            return this._attributes.Contains(item);
        }

        /// <summary>
        /// Check if AttributeCollection contains attribute with given AttributeUniqueidentifier
        /// </summary>
        /// <param name="attributeUniqueIdentifier">AttributeUniqueIdentifier to search in AttributeCollection</param>
        /// <param name="locale">Locale of an Attribute to search in AttributeCollection.</param>
        /// <returns>
        /// <para>true : If attribute found in AttributeCollection</para>
        /// <para>false : If attribute found not in AttributeCollection</para>
        /// </returns>
        public bool Contains(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale)
        {
            if (GetAttribute(attributeUniqueIdentifier, locale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the AttributeCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from AttributeCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Attribute[] array, int arrayIndex)
        {
            this._attributes.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of attributes in AttributeCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributes.Count;
            }
        }

        /// <summary>
        /// Check if AttributeCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the AttributeCollection.
        /// </summary>
        /// <param name="item">The attribute object to remove from the AttributeCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original AttributeCollection</returns>
        public bool Remove(Attribute item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Attribute");
            }

            return this._attributes.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the Attribute collection
        /// </summary>
        /// <param name="item">The object to remove from the Attribute collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        public Boolean Remove(IAttribute item)
        {
            return this.Remove((Attribute)item);
        }

        /// <summary>
        /// Resets the Value id for all attributes
        /// </summary>
        public void ResetValueId()
        {
            if (_attributes != null && _attributes.Count > 0)
            {
                foreach (Attribute attribute in _attributes)
                {
                    attribute.ResetValueId();
                }
            }
        }

        #endregion

        #region IEnumerable<Attribute> Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<Attribute> GetEnumerator()
        {
            return this._attributes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributes.GetEnumerator();
        }

        #endregion

        #region IAttributeCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of AttributeCollection object
        /// </summary>
        /// <returns>Xml string representing the AttributeCollection</returns>
        public String ToXml()
        {
            String attributesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (Attribute attribute in this._attributes)
            {
                builder.Append(attribute.ToXml());
            }

            attributesXml = String.Format("<Attributes>{0}</Attributes>", builder.ToString());
            return attributesXml;
        }

        /// <summary>
        /// Get Xml representation of AttributeCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String attributesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (Attribute attribute in this._attributes)
                {
                    builder.Append(attribute.ToXml(serialization));
                }
            }

            attributesXml = String.Format("<Attributes>{0}</Attributes>", builder.ToString());
            return attributesXml;
        }

        /// <summary>
        /// Get Xml representation of AttributeCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <param name="valueFormatLocale"></param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization serialization, LocaleEnum valueFormatLocale)
        {
            String attributesXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (Attribute attribute in this._attributes)
                {
                    builder.Append(attribute.ToXml(serialization, valueFormatLocale));
                }
            }

            attributesXml = String.Format("<Attributes>{0}</Attributes>", builder.ToString());
            return attributesXml;
        }

        /// <summary>
        /// Converts AttributeCollection object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of AttributeCollection object</param>
        internal void ConvertAttributeCollectionToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                // Attribute collection node start
                xmlWriter.WriteStartElement("Attributes");

                if (this._attributes != null && this._attributes.Count > 0)
                {
                    foreach (Attribute attribute in this._attributes)
                    {
                        attribute.ConvertAttributeToXml(xmlWriter);
                    }
                }

                // Attribute collection node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write AttributeCollection object.");
            }
        }

        #endregion ToXml methods

        #region Attribute Get

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <exception cref="MDMOperationException">Not able to find appropriate attribute as attribute is localized. Please provide locale details.</exception>
        public IAttribute GetAttribute(Int32 attributeId)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            if (attributeId <= 0)
            {
                throw new ArgumentException("Attribute Id must be greater than 0", attributeId.ToString());
            }

            Attribute attribute = Get(attributeId);

            return attribute;
        }

        /// <summary>
        /// Get attribute based on IAttributeUniqueIdentifier from current entity's attributes
        /// </summary>
        /// <param name="attributeUId">
        ///     IAttributeUniqueIdentifier which identifies an attribute uniquely
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeName is attribute short name
        ///     </para>
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeGroupName is attribute group short name
        ///     </para>
        /// </param>
        /// <returns>Attribute Interface</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="ArgumentException">AttributeUniqueIdentifier.AttributeGroupName and AttributeUniqueIdentifier.AttributeName is either null or empty</exception>
        /// <exception cref="MDMOperationException">Not able to find appropriate attribute as attribute is localized. Please provide locale details.</exception>
        public IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            if (attributeUId == null)
            {
                throw new ArgumentNullException("AttributeUniqueIdentifier");
            }

            if (String.IsNullOrWhiteSpace(attributeUId.AttributeName))
            {
                throw new ArgumentException("AttributeUniqueIdentifier.AttributeName is either null or empty. Please check the value");
            }

            AttributeUniqueIdentifier attributeUniqueIdentifier = (AttributeUniqueIdentifier)attributeUId;
            Attribute attribute = Get(attributeUniqueIdentifier);

            return attribute;
        }

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <param name="instanceRefId">InstanceRefId + AttributeId to search in attribute collection. This is optional parameter. Default value is -1</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        /// <exception cref="MDMOperationException">Not able to find appropriate attribute as attribute is localized. Please provide locale details.</exception>
        public IAttribute GetAttribute(Int32 attributeId, Int32 instanceRefId)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            if (attributeId <= 0)
            {
                throw new ArgumentException("Attribute Id must be greater than 0", attributeId.ToString());
            }

            Attribute attribute = Get(attributeId, instanceRefId);

            return attribute;
        }

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(Int32 attributeId, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            if (attributeId <= 0)
            {
                throw new ArgumentException("Attribute Id must be greater than 0", attributeId.ToString());
            }

            Attribute attribute = Get(attributeId, locale);

            return attribute;
        }

        /// <summary>
        /// Gets attribute with specified attribute Id from current entity's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current entity's attributes</param>
        /// <param name="instanceRefId">InstanceRefId + AttributeId to search in attribute collection. This is optional parameter. Default value is -1</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(Int32 attributeId, Int32 instanceRefId, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            if (attributeId <= 0)
            {
                throw new ArgumentException("Attribute Id must be greater than 0", attributeId.ToString());
            }

            Attribute attribute = Get(attributeId, locale, instanceRefId);

            return attribute;
        }

        /// <summary>
        /// Get attribute based on IAttributeUniqueIdentifier from current entity's attributes
        /// </summary>
        /// <param name="attributeUId">
        ///     IAttributeUniqueIdentifier which identifies an attribute uniquely
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeName is attribute short name
        ///     </para>
        ///     <para>
        ///     IAttributeUniqueIdentifier.AttributeGroupName is attribute group short name
        ///     </para>
        /// </param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>Attribute Interface</returns>
        /// <exception cref="ArgumentNullException">AttributeUniqueIdentifier cannot be null</exception>
        /// <exception cref="ArgumentException">AttributeUniqueIdentifier.AttributeGroupName and AttributeUniqueIdentifier.AttributeName is either null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(IAttributeUniqueIdentifier attributeUId, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            if (attributeUId == null)
            {
                throw new ArgumentNullException("AttributeUniqueIdentifier");
            }

            if (String.IsNullOrWhiteSpace(attributeUId.AttributeName))
            {
                throw new ArgumentException("AttributeUniqueIdentifier.AttributeName is either null or empty. Please check the value");
            }

            AttributeUniqueIdentifier attributeUniqueIdentifier = (AttributeUniqueIdentifier)attributeUId;
            Attribute attribute = Get(attributeUniqueIdentifier, locale);

            return attribute;
        }

        /// <summary>
        /// Gets attribute(s) with specified attribute short Name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttributeCollection GetAttributes(String attributeShortName)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            if (String.IsNullOrWhiteSpace(attributeShortName))
            {
                throw new ArgumentException("Attribute Name cannot be null or empty. Please check the value", attributeShortName);
            }

            AttributeCollection attributeCollection = new AttributeCollection();
            IEnumerable<IAttribute> attributes = from attr in this._attributes
                                                 where attr.Name == attributeShortName
                                                 select attr;
            foreach (Attribute attr in attributes)
            {
                attributeCollection.Add(attr);
            }

            return attributeCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeShortName"></param>
        /// <returns></returns>
        public IAttribute GetAttribute(String attributeShortName)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in. AttributeCollection is Null");
            }

            if (String.IsNullOrWhiteSpace(attributeShortName))
            {
                throw new ArgumentNullException("Attribute Name cannot be null or empty. Please check the value", attributeShortName);
            }

            return (IAttribute)this.Get(attributeShortName);
        }

        /// <summary>
        /// Gets attribute(s) with specified attribute short Name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Name of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Specifies Locale in which Attributes should be returned</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(String attributeShortName, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in. AttributeCollection is Null");
            }

            if (String.IsNullOrWhiteSpace(attributeShortName))
            {
                throw new ArgumentNullException("Attribute Name cannot be null or empty. Please check the value", attributeShortName);
            }

            return (IAttribute)this.Get(attributeShortName, locale);
        }

        /// <summary>
        /// Gets attribute(s) with specified attribute short Name from current entity's attributes in all locales
        /// </summary>
        /// <param name="attributeShortName">Specifies Name of an attribute to search in current entity's attributes</param>
        /// <param name="attributeParentName">Specifies Parent Name of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute in all entity locales</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(String attributeShortName, String attributeParentName)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in. AttributeCollection is Null");
            }

            if (String.IsNullOrWhiteSpace(attributeShortName))
            {
                throw new ArgumentNullException("Attribute Name cannot be null or empty. Please check the value", attributeShortName);
            }

            if (String.IsNullOrWhiteSpace(attributeParentName))
            {
                throw new ArgumentNullException("AttributeParent Name cannot be null or empty. Please check the value", attributeParentName);
            }

            AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeShortName, attributeParentName);

            return (IAttribute)Get(attributeUniqueIdentifier);
        }

        /// <summary>
        /// Gets attribute(s) with specified attribute short Name and parent name from current entity's attributes
        /// </summary>
        /// <param name="attributeShortName">Specifies Name of an attribute to search in current entity's attributes</param>
        /// <param name="attributeParentName">Specifies Parent Name of an attribute to search in current entity's attributes</param>
        /// <param name="locale">Specifies Locale of an attribute to search in current entity's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Name cannot be null or empty</exception>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttribute GetAttribute(String attributeShortName, String attributeParentName, LocaleEnum locale)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in. AttributeCollection is Null");
            }

            if (String.IsNullOrWhiteSpace(attributeShortName))
            {
                throw new ArgumentNullException("Attribute Name cannot be null or empty. Please check the value", attributeShortName);
            }

            if (String.IsNullOrWhiteSpace(attributeParentName))
            {
                throw new ArgumentNullException("AttributeParent Name cannot be null or empty. Please check the value", attributeParentName);
            }

            AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeShortName, attributeParentName);

            return (IAttribute)Get(attributeUniqueIdentifier, locale);
        }

        /// <summary>
        /// Gets child attributes of given attribute group (parent) id
        /// </summary>
        /// <param name="attributeGroupId">AttributeParentId of which attributes are to be fetched.</param>
        /// <param name="locale">Locale for which attribute is to be searched</param>
        /// <returns>IAttribtueCollection having given parent id </returns>
        /// <exception cref="ArgumentException">AttribtueGroupId must be greater than 0</exception>
        /// <exception cref="NullReferenceException">There are no attributes to search in</exception>
        public IAttributeCollection GetAttributes(Int32 attributeGroupId, LocaleEnum locale)
        {
            if (attributeGroupId <= 0)
            {
                throw new ArgumentException("AttribtueGroupId must be greater than 0");
            }

            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            AttributeCollection attributeCollection = new AttributeCollection();
            IEnumerable<IAttribute> attributes = from attr in this._attributes
                                                 where attr.AttributeParentId == attributeGroupId && attr.Locale == locale
                                                 select attr;
            foreach (Attribute attr in attributes)
            {
                attributeCollection.Add(attr);
            }

            return attributeCollection;
        }

        /// <summary>
        /// Get attributes having specific AttributeModelType from current entity's attributes
        /// </summary>
        /// <param name="attributeModelType">AttributeModelType of which attributes are to be fetched from current entity's attributes</param>
        /// <returns>Attribute collection interface. Attributes in this collection are having specified AttributeModelType</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttributeCollection GetAttributes(AttributeModelType attributeModelType)
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            AttributeCollection attributeCollection = new AttributeCollection();

            IEnumerable<IAttribute> attributes = from attr in this._attributes
                                                 where attr.AttributeModelType == attributeModelType
                                                 select attr;
            foreach (Attribute attr in attributes)
            {
                attributeCollection.Add(attr);
            }

            return attributeCollection;
        }

        /// <summary>
        /// Get common attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = Common</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttributeCollection GetCommonAttributes()
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            return this.GetAttributes(AttributeModelType.Common);
        }

        /// <summary>
        /// Get Category specific attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = Category</returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttributeCollection GetCategorySpecificAttributes()
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            return this.GetAttributes(AttributeModelType.Category);
        }

        /// <summary>
        /// Get System attributes from current entity's attributes
        /// </summary>
        /// <returns>Attribute collection interface. Attributes in this collection are having AttributeModelType = System </returns>
        /// <exception cref="NullReferenceException">If there are no attributes to search in</exception>
        public IAttributeCollection GetSystemAttributes()
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            return this.GetAttributes(AttributeModelType.System);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAttributeCollection GetLookupAttributes()
        {
            if (this._attributes == null)
            {
                throw new NullReferenceException("There are no attributes to search in");
            }

            AttributeCollection attributeCollection = new AttributeCollection();

            IEnumerable<Attribute> lookupAttributes = from attr in this._attributes
                                                      where attr.IsLookup
                                                      select attr;


            if (lookupAttributes != null && lookupAttributes.Count() > 0)
            {
                foreach (Attribute attr in lookupAttributes)
                {
                    attributeCollection.Add(attr);
                }
            }

            return attributeCollection;
        }

        /// <summary>
        /// Gets attribute(s) with specified Locale from current attributes.
        /// This will also return only localizable attribute
        /// </summary>
        /// <param name="locale">Locale of an attribute to search in current attributes</param>
        /// <returns>AttributeCollection interface</returns>
        public IAttributeCollection GetAttributes(LocaleEnum locale)
        {
            AttributeCollection attributes = new AttributeCollection();

            if (this._attributes != null && this._attributes.Count > 0)
            {
                attributes = new AttributeCollection((from attr in this._attributes
                                                      where attr.Locale == locale && attr.IsLocalizable == true
                                                      select attr).ToList<Attribute>());
            }
            return (IAttributeCollection)attributes;
        }

        /// <summary>
        /// Gets Non-Localizable attribute(s) from current attributes.
        /// </summary>
        /// <returns>AttributeCollection interface</returns>
        public IAttributeCollection GetNonLocalizableAttributes()
        {
            AttributeCollection attributes = new AttributeCollection();

            if (this._attributes != null && this._attributes.Count > 0)
            {
                attributes = new AttributeCollection((from attr in this._attributes
                                                      where attr.IsLocalizable == false
                                                      select attr).ToList<Attribute>());
            }
            return (IAttributeCollection)attributes;
        }

        #endregion Attribute Get

        #region Attribute Value Get

        #region Current value

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName)
        {
            IAttribute iAttribute = Get(attributeName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstance();
            }

            return iValue;
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies locale in which attribute value should returned </param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName, LocaleEnum locale)
        {
            IAttribute iAttribute = Get(attributeName, locale);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstance(locale);
            }

            return iValue;
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName)
        {
            IAttribute iAttribute = Get(attributeName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstanceInvariant();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, LocaleEnum locale)
        {
            IAttribute iAttribute = Get(attributeName, locale);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstanceInvariant();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName)
        {
            IAttribute iAttribute = GetAttribute(attributeName, attributeParentName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstance();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstance(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeName, attributeParentName);

            IAttribute iAttribute = Get(attributeUniqueIdentifier, locale);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstance(locale);
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName)
        {
            IAttribute iAttribute = GetAttribute(attributeName, attributeParentName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstanceInvariant();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves current value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeCurrentValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeName, attributeParentName);

            IAttribute iAttribute = Get(attributeUniqueIdentifier, locale);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetCurrentValueInstanceInvariant();
            }
            return iValue;
        }

        #endregion

        #region Inherited Value

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName)
        {
            IAttribute iAttribute = Get(attributeName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstance();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName, LocaleEnum locale)
        {
            IAttribute iAttribute = Get(attributeName, locale);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstance(locale);
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName)
        {
            IAttribute iAttribute = Get(attributeName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstanceInvariant();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, LocaleEnum locale)
        {
            IAttribute iAttribute = Get(attributeName, locale);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstanceInvariant();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName)
        {
            IAttribute iAttribute = GetAttribute(attributeName, attributeParentName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstance();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves inherited value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="localeFormate">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstance(String attributeName, String attributeParentName, LocaleEnum localeFormate)
        {
            AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeName, attributeParentName);

            IAttribute iAttribute = Get(attributeUniqueIdentifier, localeFormate);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstance(localeFormate);
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName)
        {
            IAttribute iAttribute = GetAttribute(attributeName, attributeParentName);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstanceInvariant();
            }
            return iValue;
        }

        /// <summary>
        /// Retrieves overridden value of an attribute under current entity
        /// </summary>
        /// <param name="attributeName">Specifies Attribute short name</param>
        /// <param name="attributeParentName">Specifies Parent attribute short name</param>
        /// <param name="locale">Specifies Locale in which value should be returned</param>
        /// <returns>Value</returns>
        public IValue GetAttributeInheritedValueInstanceInvariant(String attributeName, String attributeParentName, LocaleEnum locale)
        {
            AttributeUniqueIdentifier attributeUniqueIdentifier = new AttributeUniqueIdentifier(attributeName, attributeParentName);

            IAttribute iAttribute = Get(attributeUniqueIdentifier, locale);
            IValue iValue = null;

            if (iAttribute != null)
            {
                iValue = iAttribute.GetInheritedValueInstanceInvariant();
            }
            return iValue;
        }

        #endregion

        #endregion

        #region Attribute Methods

        /// <summary>
        /// Get attribute id array from the current attribute collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute collection</exception>
        public Collection<Int32> GetAttributeIdList()
        {
            #region Parameter validation

            if (this._attributes == null)
            {
                throw new Exception("There are no attributes in collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Collection<Int32> attributeIdList = new Collection<Int32>((from attribute in this._attributes
                                                                       select attribute.Id).Distinct().ToList<Int32>());

            return attributeIdList;
        }

        /// <summary>
        /// Get attribute id array from the current attribute model collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current attribute model collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute model collection</exception>
        public Collection<KeyValuePair<Int32, LocaleEnum>> GetAttributeIdLocaleList()
        {
            #region Parameter validation

            if (this._attributes == null)
            {
                throw new Exception("There are no attributes in Collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Collection<KeyValuePair<Int32, LocaleEnum>> attributeIdLocaleList = new Collection<KeyValuePair<int, LocaleEnum>>();

            foreach (Attribute attribute in this._attributes)
            {
                if (!attributeIdLocaleList.Contains(new KeyValuePair<Int32, LocaleEnum>(attribute.Id, attribute.Locale)))
                {
                    attributeIdLocaleList.Add(new KeyValuePair<int, LocaleEnum>(attribute.Id, attribute.Locale));
                }
            }

            return attributeIdLocaleList;
        }

        /// <summary>
        /// Get attribute "internal unique key" array from the current attribute collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current attribute model collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute model collection</exception>
        public Collection<Int32> GetAttributeInternalUniqueKeyList()
        {
            #region Parameter validation

            if (this._attributes == null)
            {
                throw new Exception("There are no attributes in Collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Collection<Int32> attributeInternalUniqueKeyList = new Collection<Int32>();

            foreach (Attribute attribute in this._attributes)
            {
                attributeInternalUniqueKeyList.Add(attribute.GetInternalUniqueKey());
            }

            return attributeInternalUniqueKeyList;
        }

        /// <summary>
        /// Get attribute id array from the current attribute model collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current attribute model collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute model collection</exception>
        public Dictionary<String, KeyValuePair<Int32, LocaleEnum>> GetAttributeIdLocaleListDictionary()
        {
            #region Parameter validation

            if (this._attributes == null)
            {
                throw new Exception("There are no attributes in Collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Dictionary<String, KeyValuePair<Int32, LocaleEnum>> attributeIdLocaleDictionary = new Dictionary<String, KeyValuePair<Int32, LocaleEnum>>();

            foreach (Attribute attribute in this._attributes)
            {
                String key = String.Concat(attribute.Name, "_", attribute.AttributeParentName, "_", attribute.Locale);

                if (!attributeIdLocaleDictionary.ContainsKey(key))
                {
                    attributeIdLocaleDictionary.Add(key, new KeyValuePair<Int32, LocaleEnum>(attribute.Id, attribute.Locale));
                }
            }

            return attributeIdLocaleDictionary;
        }

        /// <summary>
        /// Set given locale for all values for all attributes in current collection.
        /// Values of only those attributes which are having overridden values will be set.
        /// </summary>
        /// <param name="localeShortName">Locale name (en_WW) to set for values </param>
        /// <exception cref="ArgumentException">localeShortName cannot be null</exception>
        public void SetLocale(String localeShortName)
        {
            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (Attribute attr in this._attributes)
                {
                    if (attr.SourceFlag == AttributeValueSource.Overridden)
                    {
                        attr.SetLocale(localeShortName);
                    }
                }
            }
        }

        /// <summary>
        /// Set given action for all attributes in current collection.
        /// </summary>
        /// <param name="action">Locale name (en_WW) to set for values </param>
        /// <exception cref="ArgumentException">localeShortName cannot be null</exception>
        public void SetAction(ObjectAction action)
        {
            if (this._attributes != null && this._attributes.Count > 0)
            {
                foreach (Attribute attr in this._attributes)
                {
                    attr.Action = action;
                }
            }
        }

        /// <summary>
        /// Return the Updated Attribute with new values
        /// </summary>
        /// <returns>Attribute collection object</returns>
        public AttributeCollection GetUpdatedAttributes()
        {
            AttributeCollection flatUpdatedAttributes = new AttributeCollection();
            GetUpdatedAttributes(this, ref flatUpdatedAttributes);

            return flatUpdatedAttributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="flatUpdatedAttributes"></param>
        private void GetUpdatedAttributes(AttributeCollection attributes, ref AttributeCollection flatUpdatedAttributes)
        {
            foreach (Attribute attribute in attributes)
            {
                //DO recursive get..
                //Add range don't add attributes
                if (attribute.Action == ObjectAction.Create || attribute.Action == ObjectAction.Update || attribute.Action == ObjectAction.Delete)
                {
                    flatUpdatedAttributes.Add(attribute, true);
                }

                if (attribute.Attributes != null && attribute.Attributes.Count > 0)
                {
                    GetUpdatedAttributes(attribute.Attributes, ref flatUpdatedAttributes);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <param name="newAttributeInstance"></param>
        public void Replace(Int32 attributeId, LocaleEnum locale, IAttribute newAttributeInstance)
        {
            this.Remove(attributeId, locale);
            this._attributes.Add((Attribute)newAttributeInstance);
        }

        #endregion

        #endregion IAttributeCollection Memebers
    }
}