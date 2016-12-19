using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Attribute Model Collection
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeModelCollection : ICollection<AttributeModel>, IEnumerable<AttributeModel>, IAttributeModelCollection, IDataModelObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting collection of attribute Model
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Collection<AttributeModel> _attributeModels = new Collection<AttributeModel>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public AttributeModelCollection() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="entityTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="locale"></param>
        /// <param name="attributeModelType"></param>
        /// <param name="valuesAsXml"></param>
        /// <param name="getCompleteDetailsOfAttribute"></param>
        public AttributeModelCollection(Int32 containerId, Int32 entityTypeId, Int64 categoryId, LocaleEnum locale, AttributeModelType attributeModelType, String valuesAsXml, Boolean getCompleteDetailsOfAttribute)
        {
            /*
             * Sample - 1 When 'getCompleteDetailsOfAttribute=false'
             * <AttributeModels>
                    <AttributeModel Id="4003" Name="ProductID" LongName="ProductID" AttributeParentId="4002" AttributeParentName="Core Attributes" AttributeParentLongName="Core Attributes" AttributeTypeId="1" AttributeTypeName="Attribute" AttributeDataTypeName="String" AttributeDisplayTypeId="3" AttributeDisplayTypeName="TextBox" IsCollection="false" IsComplex="false" IsLocalizable="true" ApplyLocaleFormat="false" ApplyTimeZoneConversion="false" AllowNullSearch="false" >
             *          <AttributeModels>
             *              <AttributeModel Id="4004" Name="ProductID" LongName="ProductID" AttributeParentId="4002" AttributeParentName="Core Attributes" AttributeParentLongName="Core Attributes" AttributeTypeId="1" AttributeTypeName="Attribute" AttributeDataTypeName="String" AttributeDisplayTypeId="3" AttributeDisplayTypeName="TextBox" IsCollection="false" IsComplex="false" IsLocalizable="true" ApplyLocaleFormat="false" ApplyTimeZoneConversion="false" AllowNullSearch="false" >
             *                  <AttributeModels/>
             *              </AttributeModel>
             *          </AttributeModels>
             *      </AttributeModel>
                </AttributeModels>
             * 
             * Sample - 2 When 'getCompleteDetailsOfAttribute=true'
             * <AttributeModels>
                    <AttributeModel Id="4003" Name="ProductID" LongName="ProductID" AttributeParentId="4002" AttributeParentName="Core Attributes" AttributeParentLongName="Core Attributes" AttributeTypeId="1" AttributeTypeName="Attribute" AttributeDataTypeName="String" AttributeDisplayTypeId="3" AttributeDisplayTypeName="TextBox" AllowVal="" MaxLength="0" MinLength="0" Required="0" ReadOnlyatNode="1" AllowableUOM="" DefaultUOM="" UOMType="" Precision="0" IsCollection="false" MinInclusive="" MaxInclusive="" MinExclusive="" MaxExclusive="" Label="" Definition="" Example="" BusinessRule="" ReadOnly="1" Extension="" AssemblyName="" AssemblyMethod="" AttributeRegEx="" AssemblyClass="" LookUpTableName="" DefaultValue="" isClassification="0" ComplexTableName="" RuleLookupTable="" RuleSP="" path="Data Attributes#@#Common Attributes#@#Core Attributes#@#ProductID" Searchable="1" EnableHistory="1" ShowAtCreation="1" Persists="0" WebUri="" CustomAction="0" InitialPopulationMethod="" OnClickJavaScript="" OnLoadJavaScript="" LKStorageFormat="" LKDisplayColumns="" LKSortOrder="" LKSearchColumns="" LookUpDisplayColumns="" LkupDuplicateAllowed="0" StoreLookupReference="0" LKDisplayFormat="" SortOrder="" ExportMask="" Inheritable="1" isHidden="0" IsComplex="false" IsLocalizable="true" ApplyLocaleFormat="false" ApplyTimeZoneConversion="false" AllowNullSearch="false" >
             *          <AttributeModels/>
             *      </AttributeModel
             * </AttributeModels>
             */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModel")
                    {
                        String attributeModelXml = reader.ReadOuterXml();
                        if (!String.IsNullOrEmpty(attributeModelXml))
                        {
                            AttributeModel attributeModel = new AttributeModel(containerId, entityTypeId, categoryId, locale, attributeModelType, attributeModelXml, getCompleteDetailsOfAttribute);

                            if (attributeModel != null)
                                this._attributeModels.Add(attributeModel);
                        }
                    }
                    else
                    {
                        reader.Read();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        
        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public AttributeModelCollection(String valuesAsXml)
        {
            LoadAttributeModelCollection(valuesAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelList">List of attribute models</param>
        public AttributeModelCollection(IList<AttributeModel> modelList)
        {
            if (modelList != null)
            {
                this._attributeModels = new Collection<AttributeModel>(modelList);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Find attribute model from AttributeModelCollection based on attributeId
        /// </summary>
        /// <param name="attributeId">Attribute model Id</param>
        /// <param name="locale">Locale for which attributeModel is to be fetched</param>
        /// <returns>Attribute MetaModel object having given attributeID</returns>
        public AttributeModel this[Int32 attributeId, LocaleEnum locale]
        {
            get
            {
                AttributeModel attributeModel = GetAttributeModel(attributeId, null, locale);
                if (attributeModel == null)
                {
                    throw new ArgumentException(String.Format("No attribute model found for given attribute id: {0} and locale: {1}", attributeId, locale.ToString()));
                }
                else
                {
                    return attributeModel;
                }
            }
            set
            {
                AttributeModel attributeModel = GetAttributeModel(attributeId, null, locale);
                if (attributeModel == null)
                {
                    throw new ArgumentException(String.Format("No attribute model found for given attribute id: {0} and locale: {1}", attributeId, locale.ToString()));
                }

                attributeModel = value;
            }
        }

        /// <summary>
        /// Find attribute model from AttributeModelCollection based on attributeUniqueIdentifier
        /// </summary>
        /// <param name="attributeUniqueIdentifier">
        ///     AttributeUniqueIdentifier using which attribute model is to be searched from AttributeModelCollection
        /// </param>
        /// <param name="locale"> Locale </param>
        /// <returns></returns>
        public AttributeModel this[AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale]
        {
            get
            {
                AttributeModel attributeModel = GetAttributeModel(0, attributeUniqueIdentifier, locale);
                if (attributeModel == null)
                    throw new ArgumentException("No attribute model found for given unique identifier");
                else
                    return attributeModel;
            }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.AttributeModel;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if AttributeModelCollection contains attribute model with given attributeId
        /// </summary>
        /// <param name="attributeId">AttributeId to search in AttributeModelCollection</param>
        /// <param name="locale">Locale of an AttributeModel to search in AttributeModelCollection.</param>
        /// <returns>
        /// <para>true : If attribute model found in AttributeModelCollection</para>
        /// <para>false : If attribute model found not in AttributeModelCollection</para>
        /// </returns>
        public bool Contains(Int32 attributeId, LocaleEnum locale)
        {
            if (GetAttributeModel(attributeId, null, locale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if AttributeModelCollection contains attribute model with given AttributeUniqueidentifier
        /// </summary>
        /// <param name="attributeUniqueIdentifier">AttributeUniqueIdentifier to search in AttributeModelCollection</param>
        /// <param name="locale">Locale of an AttributeModel to search in AttributeModelCollection.</param>
        /// <returns>
        /// <para>true : If attribute model found in AttributeModelCollection</para>
        /// <para>false : If attribute model found not in AttributeModelCollection</para>
        /// </returns>
        public bool Contains(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale)
        {
            if (GetAttributeModel(0, attributeUniqueIdentifier, locale) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove attribute model object from AttributeModelCollection
        /// </summary>
        /// <param name="attributeId">Id of attribute model which is to be removed from collection</param>
        /// <param name="locale">Locale of an AttributeModel to remove from AttributeModelCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(Int32 attributeId, LocaleEnum locale)
        {
            AttributeModel attribute = GetAttributeModel(attributeId, null, locale);

            if (attribute == null)
                throw new ArgumentException("No attribute found for given attribute id and locale");
            else
                return this.Remove(attribute);
        }

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an attribute model which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public Boolean Remove(Collection<String> referenceIds)
        {
            Boolean result = true;

            AttributeModelCollection attributeModels  = GetAttributeModels(referenceIds);

            if (attributeModels != null && attributeModels.Count > 0)
            {
                foreach (AttributeModel attributeModel in attributeModels)
                {
                    result = result && this.Remove(attributeModel);
                }
            }

            return result;
        }

        /// <summary>
        /// Get Xml representation of Attribute Model Collection
        /// </summary>
        /// <returns>Xml representation of Attribute Model Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<AttributeModels>";

            if (this._attributeModels != null && this._attributeModels.Count > 0)
            {
                foreach (AttributeModel attrModel in this._attributeModels)
                {
                    returnXml = String.Concat(returnXml, attrModel.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</AttributeModels>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of Attribute Model Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Model Collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            returnXml = "<AttributeModels>";

            if (this._attributeModels != null && this._attributeModels.Count > 0)
            {
                foreach (AttributeModel attrModel in this._attributeModels)
                {
                    returnXml = String.Concat(returnXml, attrModel.ToXml(objectSerialization));
                }
            }

            returnXml = String.Concat(returnXml, "</AttributeModels>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is AttributeModelCollection)
            {
                AttributeModelCollection objectToBeCompared = obj as AttributeModelCollection;
                Int32 attributeModelsUnion = this._attributeModels.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributeModelsIntersect = this._attributeModels.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (attributeModelsUnion != attributeModelsIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (AttributeModel attributeModel in this._attributeModels)
            {
                hashCode += attributeModel.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetAttributeModelCollection">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(AttributeModelCollection subsetAttributeModelCollection, Boolean compareIds = false)
        {
            if (subsetAttributeModelCollection != null && subsetAttributeModelCollection.Count > 0)
            {
                foreach (AttributeModel childAttributeModel in subsetAttributeModelCollection)
                    {
                        //Get sub set attribute from super attribute collection.
                        IAttributeModel iSourceAttributeModel = this.GetAttributeModel(MDMObjectFactory.GetIAttributeUniqueIdentifier(childAttributeModel.Name, childAttributeModel.AttributeParentName), childAttributeModel.Locale);

                        if (iSourceAttributeModel != null)
                        {
                            AttributeModel sourceAttributeModel = (AttributeModel)iSourceAttributeModel;

                        if (!sourceAttributeModel.IsSuperSetOf(childAttributeModel, compareIds))
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }
                }

            return true;
        }

        /// <summary>
        /// compare 2 attribute models object - successful operationresult indicates 2 identical objects, otherwise mismatch(es) is/are stored as error messages
        /// </summary>
        /// <param name="subsetAttributeModelCollection"></param>
        /// <param name="operationResult"></param>
        /// <param name="compareIds"></param>
        /// <returns></returns>
        public Boolean GetSuperSetOperationResult(AttributeModelCollection subsetAttributeModelCollection, OperationResult operationResult, Boolean compareIds = false)
        {
            if (subsetAttributeModelCollection != null && subsetAttributeModelCollection.Count > 0)
            {
                foreach (AttributeModel childAttributeModel in subsetAttributeModelCollection)
                {
                    //Get sub set attribute from super attribute collection.
                    var iSourceAttributeModel = this.GetAttributeModel(MDMObjectFactory.GetIAttributeUniqueIdentifier(childAttributeModel.Name, childAttributeModel.AttributeParentName), childAttributeModel.Locale);

                    if (iSourceAttributeModel != null)
                    {
                        var sourceAttributeModel = (AttributeModel)iSourceAttributeModel;
                        operationResult = sourceAttributeModel.GetSuperSetOfOperationResult(childAttributeModel, compareIds);
                    }
                    else
                    {
                        operationResult.AddOperationResult("-1", String.Format(" Source Attribute Model {0} is null", childAttributeModel.Name), OperationResultType.Error);
                    }
                }
            }
            else
            {
                operationResult.AddOperationResult("-1", String.Format("SubsetAttributeModelCollection is null - nothing to compare"), OperationResultType.Information);
            }

            operationResult.RefreshOperationResultStatus();

            if (operationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Adds attribute models to the current collection
        /// </summary>
        /// <param name="attributeModelCollection">Collection of attribute models which needs to be added</param>
        public void AddRange(AttributeModelCollection attributeModelCollection)
        {
            this.AddRange(attributeModelCollection, false);
        }

        /// <summary>
        /// Adds attribute models to the current collection
        /// </summary>
        /// <param name="attributeModels">Collection of attribute models which needs to be added</param>
        /// <param name="ignoreDuplicateCheck">Specifies flag to ignore duplicates or not</param>
        public void AddRange(AttributeModelCollection attributeModels, Boolean ignoreDuplicateCheck)
        {
            if (attributeModels != null && attributeModels.Count > 0)
            {
                foreach (AttributeModel attributeModel in attributeModels)
            {
                    this.Add(attributeModel, ignoreDuplicateCheck);
                }
            }
        }

        /// <summary>
        /// Get AttributeModel from AttributeModelCollection based on AttributeId and Locale
        /// </summary>
        /// <param name="attributeId">Id of an AttributeModel to search in</param>
        /// <returns>AttributeModelCollection having given AttributeId. Here Model collection will be returned as 1 AttributeModel can be there for multiple locales.</returns>
        public IAttributeModelCollection GetAttributeModel(Int32 attributeId)
        {
            AttributeModelCollection models = new AttributeModelCollection((from attr in this._attributeModels
                                                                            where attr.Id == attributeId
                                                                            select attr).ToList<AttributeModel>());
            return models;
        }

        /// <summary>
        /// Get AttributeModel from AttributeModelCollection based on AttributeId and Locale
        /// </summary>
        /// <param name="attributeId">Id of an AttributeModel to search in</param>
        /// <param name="locale">Locale of an AttributeModel to search in AttributeModelCollection.</param>
        /// <returns>AttributeModel having given AttributeId and Locale</returns>
        public IAttributeModel GetAttributeModel(Int32 attributeId, LocaleEnum locale)
        {
            return GetAttributeModel(attributeId, null, locale);
        }

        /// <summary>
        /// Get AttributeModel from AttributeModelCollection based on AttributeUniqueIdentifier (Attribute Name + attribute Parent Name) and Locale
        /// </summary>
        /// <param name="attributeUniqueIdentifier">AttributeUniqueIdentifier to search in AttributeModelCollection</param>
        /// <param name="locale">Locale of an AttributeUniqueIdentifier to search in AttributeModelCollecton.</param>
        /// <returns>AttributeModel having given AttributeUniqueIdentifier and Locale</returns>
        public AttributeModel GetAttributeModel(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale)
        {
            return GetAttributeModel(0, attributeUniqueIdentifier, locale);
        }

        /// <summary>
        /// Get AttributeModel from AttributeModelCollection based on AttributeUniqueIdentifier (Attribute Name + attribute Parent Name) and Locale
        /// </summary>
        /// <param name="attributeUniqueIdentifier">AttributeUniqueIdentifier to search in AttributeModelCollection</param>
        /// <param name="locale">Locale of an AttributeUniqueIdentifier to search in AttributeModelCollecton.</param>
        /// <returns>AttributeModel having given AttributeUniqueIdentifier and Locale</returns>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        public AttributeModel GetAttributeModel(AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, Boolean ignoreCase)
        {
            return GetAttributeModel(0, attributeUniqueIdentifier, locale, ignoreCase);
        }

        /// <summary>
        /// Get AttributeModel from AttributeModelCollection based on AttributeUniqueIdentifier interface (Attribute Name + attribute Parent Name) and Locale
        /// </summary>
        /// <param name="iAttributeUniqueIdentifier">AttributeUniqueIdentifier interface to search in AttributeModelCollection</param>
        /// <param name="locale">Locale of an AttributeUniqueIdentifier to search in AttributeModelCollecton.</param>
        /// <returns>AttributeModel interface having given AttributeUniqueIdentifier interface and Locale</returns>
        public IAttributeModel GetAttributeModel(IAttributeUniqueIdentifier iAttributeUniqueIdentifier, LocaleEnum locale)
        {
            return GetAttributeModel(0, (AttributeUniqueIdentifier)iAttributeUniqueIdentifier, locale);
        }

        /// <summary>
        /// Get AttributeModel based on AttributeId or AttributeUniqueIdentifier and locale
        /// </summary>
        /// <param name="attributeId">AttributeID to search in AttributeModelCollection</param>
        /// <param name="attributeUniqueIdentifier">AttributeUniqueIdentifier to search in AttributeModelCollection</param>
        /// <param name="locale">Locale of AttributeModel or AttributeUniqueIdentifier to search in AttributeModelCollecton.</param>
        /// <returns>AttributeModel having given AttributeId or AttributeUniqueIdentifier and Locale</returns>
        public AttributeModel GetAttributeModel(Int32 attributeId, AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale)
        {
            return Get(attributeId, attributeUniqueIdentifier, locale, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeUniqueIdentifier"></param>
        /// <param name="locale"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public AttributeModel GetAttributeModel(Int32 attributeId, AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, Boolean ignoreCase)
        {
            return Get(attributeId, attributeUniqueIdentifier, locale, ignoreCase);
        }

        /// <summary>
        /// Gets AttributeModel based on Name/Parent name of the specified attribute
        /// </summary>
        /// <param name="attribute">The attribute for which the model needs to be retrieved</param>
        /// <returns>AttributeModel object based on the input attribute passed</returns>
        public AttributeModel GetAttributeModel(Attribute attribute)
        {
            List<AttributeModel> foundAttributeModels = new List<AttributeModel>();

            AttributeModel attributeModel = null;

            if (this._attributeModels != null && attribute != null)
            {
                String attributeName = attribute.Name;
                String attributeLongName = attribute.LongName;
                String attributeParentName = attribute.AttributeParentName;
                String attributeParentLongName = attribute.AttributeParentLongName;

                StringComparison stringComparisonType = StringComparison.InvariantCultureIgnoreCase;

                foreach (AttributeModel attrModel in this._attributeModels)
                {
                    if (((String.Compare(attrModel.Name, attributeName, stringComparisonType) == 0) || (String.Compare(attrModel.LongName, attributeLongName, stringComparisonType) == 0))
                          && attrModel.Locale == attribute.Locale)
                    {
                        foundAttributeModels.Add(attrModel);
                    }
                }

                if (foundAttributeModels.Count == 1)
                {
                    attributeModel = foundAttributeModels.ElementAt(0);
                }
                else if (foundAttributeModels.Count > 1)
                {
                    foreach (AttributeModel attrModel in foundAttributeModels)
                    {
                        if (String.Compare(attrModel.AttributeParentName, attributeParentName, stringComparisonType) == 0 ||
                            String.Compare(attrModel.AttributeParentLongName, attributeParentLongName, stringComparisonType) == 0)
                        {
                            attributeModel = attrModel;
                            break;
                        }
                    }
                }
            }

            return attributeModel;
        }

        /// <summary>
        /// Get the Dependent Attribute Models
        /// </summary>
        /// <returns>AttributeModelCollection</returns>
        public AttributeModelCollection GetDependentAttributeAttributeModels()
        {
            AttributeModelCollection dependencyAttributeModels = new AttributeModelCollection();

            if (this._attributeModels != null)
            {
                foreach (AttributeModel attrModel in this._attributeModels)
                {
                    if (attrModel.IsDependentAttribute || attrModel.HasDependentAttribute)
                    {
                        dependencyAttributeModels.Add(attrModel);
                    }

                    if (attrModel.IsComplex)
                    {
                        foreach (AttributeModel child in attrModel.GetChildAttributeModels())
                        {
                            if (child.IsDependentAttribute || child.HasDependentAttribute)
                            {
                                dependencyAttributeModels.Add(child);
                            }
                        }
                    }
                }
            }

            return dependencyAttributeModels;
        }

        /// <summary>
        /// Get the Dependent Attribute Models
        /// </summary>
        /// <returns>AttributeModelCollection</returns>
        public AttributeModelCollection GetDependentChildAttributeModels()
        {
            AttributeModelCollection dependencyAttributeModels = new AttributeModelCollection();

            if (this._attributeModels != null)
            {
                foreach (AttributeModel attrModel in this._attributeModels)
                {
                    if (attrModel.IsDependentAttribute)
                    {
                        dependencyAttributeModels.Add(attrModel);
                    }

                    if (attrModel.IsComplex)
                    {
                        foreach (AttributeModel child in attrModel.GetChildAttributeModels())
                        {
                            if (child.IsDependentAttribute)
                            {
                                dependencyAttributeModels.Add(child);
                            }
                        }
                    }
                }
            }

            return dependencyAttributeModels;
        }

        /// <summary>
        /// Get All Common AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>Common AttributeModelCollection</returns>
        public IAttributeModelCollection GetCommonAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Common, false, false, locale);
        }

        /// <summary>
        /// Get All Required Common AttributeMdodels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredCommonAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Common, true, false, locale);
        }

        /// <summary>
        /// Get All Required and ReadOnly Common AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredReadOnlyCommonAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Common, true, true, locale);
        }

        /// <summary>
        /// Get All Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetTechnicalAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Category, false, false, locale);
        }

        /// <summary>
        /// Get All Required Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredTechnicalAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Category, true, false, locale);
        }

        /// <summary>
        /// Get All Required and ReadOnly Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredReadOnlyTechnicalAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Category, true, true, locale);
        }

        /// <summary>
        /// Get All Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRelationshipAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Relationship, false, false, locale);
        }

        /// <summary>
        /// Get All Required Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredRelationshipAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Relationship, true, false, locale);
        }

        /// <summary>
        /// Get All Required and ReadOnly Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetRequiredReadOnlyRelationshipAttributeModels(LocaleEnum locale)
        {
            return this.GetAttributeModelByType(AttributeModelType.Relationship, true, true, locale);
        }

        /// <summary>
        /// Gets Show At Creation Attribute Models
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetShowAtCreationAttributeModels(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                throw new NullReferenceException("There are no attributeModels to search in");
            }

            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (AttributeModel attributeModel in this._attributeModels)
            {
                if (attributeModel.ShowAtCreation && attributeModel.Locale == locale)
                    attributeModels.Add(attributeModel);
            }

            return (IAttributeModelCollection)attributeModels;
        }

        /// <summary>
        /// Gets AttributeModels which have default values configured
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetAttributeModelsWithDefaultValues(LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                throw new NullReferenceException("There are no attributeModels to search in");
            }

            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (AttributeModel attributeModel in this._attributeModels)
            {
                if (!String.IsNullOrWhiteSpace(attributeModel.DefaultValue) && attributeModel.Locale == locale)
                    attributeModels.Add(attributeModel);
            }

            return (IAttributeModelCollection)attributeModels;
        }

        /// <summary>
        /// Get attribute "internal unique key" array from the current attribute collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current attribute model collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute model collection</exception>
        public Collection<Int32> GetAttributeInternalUniqueKeyList()
        {
            #region Parameter validation

            if (this._attributeModels == null)
            {
                throw new Exception("There are no attributes in Collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Collection<Int32> attributeModelInternalUniqueKeyList = new Collection<Int32>();

            foreach (AttributeModel attributeModel in this._attributeModels)
            {
                attributeModelInternalUniqueKeyList.Add(Attribute.GetInternalUniqueKey(attributeModel.Id, attributeModel.Locale));
            }

            return attributeModelInternalUniqueKeyList;
        }

        /// <summary>
        /// Gets AttributeModels by given attribute parent Id
        /// </summary>
        /// <param name="attributeParentId">Represents attribute parent id</param>
        /// <param name="locales">Represents data locales in which attribute models should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetAttributeModelsByParentId(Int32 attributeParentId, Collection<LocaleEnum> locales)
        {
            if (attributeParentId < 1)
            {
                throw new Exception(String.Format("Failed to get attributeModels for AttributeParentId: {0} as Attribute parent Id cannot be less than 1", attributeParentId));
            }

            if (locales == null || locales.Count < 1)
            {
                throw new Exception(String.Format("Failed to get attributeModels for AttributeParentId: {0} as no locales found to return data.", attributeParentId));
            }

            AttributeModelCollection attributeModelCollection = GetAttributeModelByParentId(attributeParentId, locales);

            return attributeModelCollection;
        }

        /// <summary>
        /// Gets AttributeModels by given attribute parent names and locales
        /// </summary>
        /// <param name="attributeParentNames">Represents attribute parent names</param>
        /// <param name="locales">Represents data locales in which attribute models should be returned</param>
        /// <returns>AttributeModels</returns>
        public IAttributeModelCollection GetAttributeModelsByParentNames(Collection<String> attributeParentNames, Collection<LocaleEnum> locales)
        {
            if (attributeParentNames == null || attributeParentNames.Count == 0)
            {
                throw new Exception("At least one attribute parent/group name requires to get attribute models");
            }

            if (locales == null || locales.Count < 1)
            {
                throw new Exception("Failed to get attributeModels as no locales found to return data.");
            }

            AttributeModelCollection attributeModelCollection = GetAttributeModelByParentNames(attributeParentNames, locales);

            return attributeModelCollection;
        }


        /// <summary>
        /// Get attribute model based on attribute model type.
        /// </summary>
        /// <param name="attributeModelType">Indicates type of attribute model</param>
        /// <returns>Returns attribute model collection based on given attribute model type and flag which indicates load inheritable only attribute models or not.</returns>
        public IAttributeModelCollection GetFilteredAttributeModels(AttributeModelType attributeModelType)
        {
            AttributeModelCollection filteredAttributeModels = new AttributeModelCollection();

            if (this != null)
            {
                foreach (AttributeModel attributeModel in this)
                {
                    if (attributeModel.AttributeModelType == attributeModelType)
                    {
                        filteredAttributeModels.Add(attributeModel);
                    }

                }
            }

            return filteredAttributeModels;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load current AttributeModelCollection from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having value for current AttributeModelCollection
        /// </param>
        public void LoadAttributeModelCollection(String valuesAsXml)
        {
            #region Sample Xml
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModel")
                        {
                            String attributeModelsXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(attributeModelsXml))
                            {
                                AttributeModel attributeModel = new AttributeModel(attributeModelsXml);

                                if (attributeModel != null)
                                {
                                    this.Add(attributeModel);
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
        /// Loads AttributeModelCollection object from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadAttributeModelCollectionFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.IsEmptyElement)
                {
                    return;
                }

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeModel")
                    {
                        if (reader.HasAttributes)
                        {
                            AttributeModel attributeModel = new AttributeModel();

                            attributeModel.LoadAttributeModelDetailFromXml(reader);

                            this.Add(attributeModel);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "AttributeModels")
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read AttributeModelCollection object.");
            }
        }

        /// <summary>
        ///  Gets the attribute model collection using the reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an attribute models which is to be fetched.</param>
        /// <returns>Returns filtered attribute model collection</returns>
        private AttributeModelCollection GetAttributeModels(Collection<String> referenceIds)
        {
            AttributeModelCollection attributeModels = new AttributeModelCollection();
            Int32 counter = 0;

            if (this._attributeModels != null && this._attributeModels.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (AttributeModel attributeModel in this._attributeModels)
                {
                    if (referenceIds.Contains(attributeModel.ReferenceId))
                    {
                        attributeModels.Add(attributeModel);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return attributeModels;
        }

        #endregion

        #region ICollection<AttributeModel> Members

        /// <summary>
        /// Add attribute model object in collection
        /// </summary>
        /// <param name="item">attribute model to add in collection</param>
        /// <exception cref="DuplicateObjectException">Thrown when attribute model having same AttributeName, AttributeParentName and Locale is already there in collection and new object is being added.</exception>
        public void Add(AttributeModel item)
        {
            this.Add(item, false);
        }

        /// <summary>
        /// Add attribute model object in collection
        /// </summary>
        /// <param name="item">attribute model to add in collection</param>
        /// <param name="ignoreDuplicateCheck">Flag to indicate if we need to skip duplicate check in the current collection</param>
        /// <exception cref="DuplicateObjectException">Thrown when attribute model having same AttributeName, AttributeParentName and Locale is already there in collection and new object is being added.</exception>
        public void Add(AttributeModel item, Boolean ignoreDuplicateCheck)
        {
            if (item != null)
            {
                Boolean validModel = true;

                if (!ignoreDuplicateCheck)
                {
                    AttributeModel model = this.GetAttributeModel(new AttributeUniqueIdentifier(item.Name, item.AttributeParentName), item.Locale);
                    if (model != null && model.Name == item.Name && model.AttributeParentName == item.AttributeParentName && model.Locale == item.Locale)
                    {
                        validModel = false;
                        //Multiple AttributeModel with same AttributeName, AttributeParentName and Locale found.
                        //throw new DuplicateObjectException("110316");

                        //TODO :: Should it throw exception ?? Need to check why it is getting duplicate models sometime.
                        //throw new DuplicateObjectException(String.Format("Multiple AttributeModel with same AttributeName={0}, AttributeParentName={1} and Locale={2} found.", item.Name, item.AttributeParentName, item.Locale ));
                    }
                }

                if (validModel)
                {
                    this._attributeModels.Add(item);
                }
            }
        }

        /// <summary>
        /// Add attribute model in collection
        /// </summary>
        /// <param name="iAttributeModel">Attribute model to add in collection</param>
        public void Add(IAttributeModel iAttributeModel)
        {
            this.Add((AttributeModel)iAttributeModel);
        }

        /// <summary>
        /// Add attribute models in collection
        /// </summary>
        /// <param name="iAttributeModels">attribute models to add in collection</param>
        public void AddRange(IAttributeModelCollection iAttributeModels)
        {
            if (iAttributeModels == null)
            {
                throw new ArgumentNullException("AttributeModels");
            }

            foreach (AttributeModel attributeModel in iAttributeModels)
            {
                if (!this.Contains(new AttributeUniqueIdentifier(attributeModel.Name, attributeModel.AttributeParentName), attributeModel.Locale))
                {
                    this.Add(attributeModel);
                }
            }
        }

        /// <summary>
        /// Removes all attribute model from collection
        /// </summary>
        public void Clear()
        {
            this._attributeModels.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeModelCollection contains a specific attribute model.
        /// </summary>
        /// <param name="item">The attribute model object to locate in the AttributeMetaModelCollection.</param>
        /// <returns>
        /// <para>true : If attribute found in AttributeMetaModelCollection</para>
        /// <para>false : If attribute found not in AttributeMetaModelCollection</para>
        /// </returns>
        public bool Contains(AttributeModel item)
        {
            return this._attributeModels.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the AttributeModelCollection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from AttributeModelCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(AttributeModel[] array, int arrayIndex)
        {
            this._attributeModels.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of attributes in AttributeModelCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._attributeModels.Count;
            }
        }

        /// <summary>
        /// Check if AttributeMetaModelCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific attribute model from the AttributeMetaModelCollection.
        /// </summary>
        /// <param name="item">The attribute model object to remove from the AttributeMetaModelCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original collection</returns>
        public bool Remove(AttributeModel item)
        {
            return this._attributeModels.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the Attribute model collection
        /// </summary>
        /// <param name="iAttributeModel">The object to remove from the Attribute Model collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        public Boolean Remove(IAttributeModel iAttributeModel)
        {
            return this.Remove((AttributeModel)iAttributeModel);
        }

        #endregion

        #region IEnumerable<AttributeModel> Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeModelCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeModel> GetEnumerator()
        {
            return this._attributeModels.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeModelCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeModels.GetEnumerator();
        }

        #endregion

        #region IAttributeModelCollection Members

        /// <summary>
        /// Get attribute id array from the current attribute model collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current attribute model collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute model collection</exception>
        public Collection<Int32> GetAttributeIdList()
        {
            #region Parameter validation

            if (this._attributeModels == null)
            {
                throw new Exception("There are no attributes in Collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Collection<Int32> attributeIdList = new Collection<Int32>((from attribute in this._attributeModels
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

            if (this._attributeModels == null)
            {
                throw new Exception("There are no attributes in Collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Collection<KeyValuePair<Int32, LocaleEnum>> attributeIdLocaleList = new Collection<KeyValuePair<int, LocaleEnum>>();

            foreach (AttributeModel model in this._attributeModels)
            {
                if (!attributeIdLocaleList.Contains(new KeyValuePair<Int32, LocaleEnum>(model.Id, model.Locale)))
                {
                    attributeIdLocaleList.Add(new KeyValuePair<int, LocaleEnum>(model.Id, model.Locale));
                }
            }

            return attributeIdLocaleList;
        }

        /// <summary>
        /// Get attribute id array from the current attribute model collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current attribute model collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute model collection</exception>
        public Dictionary<String, KeyValuePair<Int32, LocaleEnum>> GetAttributeIdLocaleListDictionary()
        {
            #region Parameter validation

            if (this._attributeModels == null)
            {
                throw new Exception("There are no attributes in Collection. Cannot get AttributeIds");
            }

            #endregion Parameter validation

            Dictionary<String, KeyValuePair<Int32, LocaleEnum>> attributeIdLocaleListDictionary = new Dictionary<String, KeyValuePair<Int32, LocaleEnum>>();

            foreach (AttributeModel attributeModel in this._attributeModels)
            {
                String key = String.Concat(attributeModel.Name, "_", attributeModel.AttributeParentName, "_", attributeModel.Locale);

                if (!attributeIdLocaleListDictionary.ContainsKey(key))
                {
                    attributeIdLocaleListDictionary.Add(key, new KeyValuePair<Int32, LocaleEnum>(attributeModel.Id, attributeModel.Locale));
                }
            }

            return attributeIdLocaleListDictionary;
        }

        /// <summary>
        /// Get all attribute models for given locale.
        /// </summary>
        /// <param name="locale">Locale for which attribute models are required.</param>
        /// <returns>IAttributeModelCollection for given locale</returns>
        public AttributeModelCollection GetAttributeModels(LocaleEnum locale)
        {
            AttributeModelCollection attributeModels = null;
            attributeModels = new AttributeModelCollection((from model in this._attributeModels
                                                            where model.Locale == locale
                                                            select model).ToList<AttributeModel>());
            return attributeModels;
        }

        /// <summary>
        /// Clone attributeModelCollection
        /// </summary>
        /// <returns>Cloned attribute model collection</returns>
        public AttributeModelCollection Clone()
        {
            AttributeModelCollection clonedAttributeModelCollection = new AttributeModelCollection();

            if (this._attributeModels != null)
            {
                foreach (AttributeModel model in this._attributeModels)
                {
                    AttributeModel clonedAttributeModel = (AttributeModel)model.Clone();
                    clonedAttributeModelCollection.Add(clonedAttributeModel, true);
                }
            }

            return (AttributeModelCollection)clonedAttributeModelCollection;
        }

        /// <summary>
        /// Marks all the attribute models as readOnly
        /// </summary>
        public void MarkAsReadOnly()
        {
            if (this._attributeModels != null)
            {
                foreach (AttributeModel attributeModel in this._attributeModels)
                {
                    attributeModel.ReadOnly = true;
                }
            }
        }

        #endregion IAttributeModelCollection Members

        #region IDataModelObjectCollection Members

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> spliteAttributeModels = null;

            if (this._attributeModels != null)
            {
                spliteAttributeModels = Utility.Split(this, batchSize);
            }

            return spliteAttributeModels;
        }

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as AttributeModel);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeUniqueIdentifier"></param>
        /// <param name="locale"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        private AttributeModel Get(Int32 attributeId, AttributeUniqueIdentifier attributeUniqueIdentifier, LocaleEnum locale, Boolean ignoreCase)
        {
            /*
             * Flow: 
             *      - Search for given Id or ShortName + ParentName + Locale in collection. 
             *      - If found, 
             *              return it.
             *      - If not found.
             *              Search for given Id or ShortName + ParentName
             *              If not found 
             *                  stop
             *              if Found
             *                  Check if attributeModel is localizable
             *                  If NOT Localizable
             *                      Take attributeModel, and return it
             *                  If Localizable
             *                      return null and stop.
             */

            String attributeName = String.Empty;
            String attributeParentName = String.Empty;

            if (attributeUniqueIdentifier != null)
            {
                attributeName = attributeUniqueIdentifier.AttributeName;
                attributeParentName = attributeUniqueIdentifier.AttributeGroupName;
            }

            //Filter attributeModel for given Id or ShortName + ParentName + Locale.
            var expectedAttributeModel = GetAttributeModelByParams(attributeId, locale, attributeName, attributeParentName, ignoreCase);

            //IF attributeModel with given Id or ShortName + ParentName + Locale not found, try to search for given Id or ShortName + ParentName.
            if (expectedAttributeModel == null)
            {
                //search for given Id or ShortName + ParentName.
                expectedAttributeModel = GetAttributeModelByParams(attributeId, attributeName, attributeParentName, ignoreCase);

                //If attributeModel with given attributeId is found, and it is not localizable then return it,
                //Else return null.
                if (expectedAttributeModel != null && expectedAttributeModel.IsLocalizable)
                {
                    expectedAttributeModel = null;
            }
            }

            return expectedAttributeModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="localeEnum"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeParentName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        private AttributeModel GetAttributeModelByParams(Int32 attributeId, LocaleEnum localeEnum, String attributeName, String attributeParentName, Boolean ignoreCase)
        {
            if (ignoreCase)
            {
                attributeName = attributeName == null ? String.Empty : attributeName.ToLowerInvariant();
                attributeParentName = attributeParentName == null ? String.Empty : attributeParentName.ToLowerInvariant();
            }

            Int32 attributeModelCount = _attributeModels.Count;

            for (Int32 index = 0; index < attributeModelCount; index++)
            {
                AttributeModel attributeModel = null;

                try
                {
                attributeModel = _attributeModels[index];
                }
                catch
                {
                    //This is done on purpose to make avoid exception in case when collection is being modified in another case between count check and loop iterations
                }


                if (attributeModel != null)
                {
                    if (attributeId > 0 && attributeModel.Id == attributeId && attributeModel.Locale == localeEnum)
                    {
                        return attributeModel;
                    }

                    if (attributeModel != null) //Adding Null check... There can be cases where AM collection can have an AM object as null...
                    {
                    String attrName = (ignoreCase == true) ? attributeModel.NameInLowerCase : attributeModel.Name;
                    String attrParentName = (ignoreCase == true) ? attributeModel.AttributeParentName.ToLowerInvariant() : attributeModel.AttributeParentName;

                        if (attrName.Equals(attributeName) && attributeModel.Locale == localeEnum)
                        {
                            // Considering is AttributeParent name is not there system has unique short name 
                            if (String.IsNullOrWhiteSpace(attributeParentName))
                            {
                            return attributeModel;
                        }
                            else if (attrParentName.Equals(attributeParentName)) 
                            {
                                return attributeModel;
                    }
                }
            }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeParentName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        private AttributeModel GetAttributeModelByParams(Int32 attributeId, String attributeName, String attributeParentName, Boolean ignoreCase)
        {
            if (ignoreCase)
            {
                attributeName = attributeName == null ? String.Empty : attributeName.ToLowerInvariant();
                attributeParentName = attributeParentName == null ? String.Empty : attributeParentName.ToLowerInvariant();
            }

            Int32 attributeModelCount = _attributeModels.Count;

            for (Int32 index = 0; index < attributeModelCount; index++)
            {
                AttributeModel attributeModel = null;

                try
                {
                attributeModel = _attributeModels[index];
                }
                catch
                {
                    //This is done on purpose..to make avoid exception in case when collection is being modified in another case between count check and loop iterations
                }

                if (attributeModel != null)
                {
                    if (attributeId > 0 && attributeModel.Id == attributeId)
                    {
                        return attributeModel;
                    }

                    if (attributeModel != null) //Adding Null check... There can be cases where AM collection can have an AM object as null...
                    {
                        String attrName = (ignoreCase == true) ? attributeModel.NameInLowerCase : attributeModel.Name;
                        String attrParentName = (ignoreCase == true) ? attributeModel.AttributeParentName.ToLowerInvariant() : attributeModel.AttributeParentName;

                        if (attrName.Equals(attributeName))
                        {
                            // Considering is AttributeParent name is not there system has unique short name
                            if (String.IsNullOrWhiteSpace(attributeParentName))
                            {
                                return attributeModel;
                            }
                            else if (attrParentName.Equals(attributeParentName))
                        {
                            return attributeModel;
                        }
                    }
                }
            }
            }

            return null;
        }

        /// <summary>
        /// Get Attribute Models by Requested AttributeModelType
        /// </summary>
        /// <param name="attributeModelType">Specifies AttributeModel Type</param>
        /// <param name="loadRequiredOnly">Specifies whether to load only required attribute models</param>
        /// <param name="loadReadOnly">Specifies whether to load only readOnly attribute models</param>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        private IAttributeModelCollection GetAttributeModelByType(AttributeModelType attributeModelType, Boolean loadRequiredOnly, Boolean loadReadOnly, LocaleEnum locale)
        {
            if (this._attributeModels == null)
            {
                throw new NullReferenceException("There are no attributeModels to search in. AttributeModelCollection is Null.");
            }

            AttributeModelCollection attributeModels = new AttributeModelCollection();

            foreach (AttributeModel attributeModel in this._attributeModels)
            {
                if (attributeModel.AttributeModelType == attributeModelType && attributeModel.Locale == locale)
                {
                    if (loadReadOnly || loadRequiredOnly)
                    {
                        //Required + ReadOnly
                        if (loadReadOnly && loadRequiredOnly)
                        {
                            if (attributeModel.ReadOnly && attributeModel.Required)
                            {
                                attributeModels.Add(attributeModel);
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        //Required
                        if (loadRequiredOnly && attributeModel.Required)
                        {
                            attributeModels.Add(attributeModel);
                        }
                        //ReadOnly
                        if (loadReadOnly && attributeModel.ReadOnly)
                        {
                            attributeModels.Add(attributeModel);
                        }
                    }
                    else
                    {
                        //Add everything if both required readOnly flags are false
                        attributeModels.Add(attributeModel);
                    }
                }
            }

            return (IAttributeModelCollection)attributeModels;
        }

        private AttributeModelCollection GetAttributeModelByParentId(Int32 attributeParentId, Collection<LocaleEnum> locales)
        {
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            Int32 attributeModelCount = _attributeModels.Count;

            for (Int32 index = 0; index < attributeModelCount; index++)
            {
                AttributeModel attributeModel = _attributeModels[index];

                if (attributeModel != null && attributeModel.AttributeParentId == attributeParentId && locales.Contains(attributeModel.Locale))
                {
                    attributeModels.Add(attributeModel);
                }
            }

            return attributeModels;
        }

        private AttributeModelCollection GetAttributeModelByParentNames(Collection<String> attributeParentNames, Collection<LocaleEnum> locales)
        {
            AttributeModelCollection attributeModels = new AttributeModelCollection();

            Int32 attributeModelCount = _attributeModels.Count;

            for (Int32 index = 0; index < attributeModelCount; index++)
            {
                AttributeModel attributeModel = _attributeModels[index];

                if (attributeModel != null && attributeParentNames.Contains(attributeModel.AttributeParentName) && locales.Contains(attributeModel.Locale))
                {
                    attributeModels.Add(attributeModel);
                }
            }

            return attributeModels;
        }

        #endregion
    }
}