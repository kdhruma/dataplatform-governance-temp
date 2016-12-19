using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.AttributeModelManager.Business
{
    using MDM.Utility;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDMBO = MDM.BusinessObjects;
    using MDM.AttributeModelManager.Data;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeModelOperationsBL : BusinessLogicBase
    {
        #region Fields

        private Collection<MDMBO.AttributeModel> attributeMetaModels = new Collection<MDMBO.AttributeModel>();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        ///Get Attribute Group
        /// </summary>
        /// <param name="attributeGroupId">Indicates the attributeGroupId of an Attribute MetaModel</param>
        /// <param name="locales">collection of locales</param>
        /// <returns>collection of attribute model</returns>
        public Collection<MDMBO.AttributeModel> GetByAttributeGroup(int attributeGroupId, Collection<LocaleEnum> locales)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelManager.AttributeModelOperationBL.GetByAttributeGroup", false);

            #region Step : Initial Setup

            #region Parameter validations

            if (locales == null)
            {
                throw new MDMOperationException("111757", "Locale is not populated.", "AttributeModelManager.AttributeModelOperationBL", String.Empty, "GetByAttributeGroup");
            }

            #endregion

            attributeMetaModels = new Collection<MDMBO.AttributeModel>();
            AttributeModelOperationsDA attributeModelOperationsDA = new AttributeModelOperationsDA();

            #endregion

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Requesting for System Data Locale...", MDMTraceSource.DataModel);

            LocaleEnum systemDataLocale = GlobalizationHelper.GetSystemDataLocale();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Got requested for System Data Locale.", MDMTraceSource.DataModel);

            if (locales != null && !locales.Contains(systemDataLocale))
            {
                locales.Add(systemDataLocale);
            }

            attributeMetaModels = attributeModelOperationsDA.GetByAttributeType(attributeGroupId, locales, systemDataLocale);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeModelManager.AttributeModelOperationBL.GetByAttributeGroup");

            return attributeMetaModels;
        }

        /// <summary>
        /// Get All Attributes
        /// </summary>
        /// <param name="localeId">Indicates the localeId of an Attribute MetaModel</param>
        /// <returns></returns>
        public Collection<MDMBO.AttributeModel> GetAllCommonAttributes(int localeId)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            return this.FindCommonAttributes("ShortName", String.Empty, "ShortName", 0, 2000, localeId, userName);
        }

        /// <summary>
        /// Get All TechnicalAttributes
        /// </summary>
        /// <param name="localeId">Indicates the localeId of an Attribute MetaModel</param>
        /// <returns></returns>
        public Collection<MDMBO.AttributeModel> GetAllTechnicalAttributes(int localeId)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            return this.FindCategoryAttributes("ShortName", String.Empty, "ShortName", 0, 2000, localeId, userName);
        }

        /// <summary>
        /// Get technical attributes based on the taxonomyId and CategoryId
        /// </summary>
        /// <param name="categoryId"> Indicates the categoryID of an Attribute MetaModel</param>
        /// <param name="taxonomyId"> Indicates the taxonomyID of an Attribute MetaModel</param>
        /// <param name="localeId"> Indicates the localeID of an Attribute MetaModel</param>
        /// <returns></returns>
        public Collection<MDMBO.AttributeModel> GetTechAttributesByTaxonomyAndCategory(int categoryId, int taxonomyId, int localeId)
        {
            AttributeModelOperationsDA attributeModelOperationsDA = new AttributeModelOperationsDA();
            Collection<MDMBO.AttributeModel> attributeMetaModellist = new Collection<MDMBO.AttributeModel>();

            attributeMetaModellist = attributeModelOperationsDA.GetTechAttributesByTaxonomyAndCategory(categoryId, taxonomyId, localeId);

            return attributeMetaModellist;
        }

        /// <summary>
        /// Get Common Attributes based on Entity Type Id
        /// </summary>
        /// <param name="catalogId">Indicates the catalogID of an Attribute MetaModel</param>
        /// <param name="entityTypeId">Indicates the entityTypeID of an Attribute MetaModel</param>
        /// <param name="localeId">Indicates the data localeId</param>
        /// <returns></returns>
        public Collection<MDMBO.AttributeModel> GetCommonAttributesByContainerAndEntityType(Int32 catalogId, Int32 entityTypeId, Int32 localeId)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeModelManager.AttributeModelOperationBL.GetCommonAttributesByContainerAndEntityType", false);

            AttributeModelOperationsDA attributeModelOperationsDA = new AttributeModelOperationsDA();
            Collection<MDMBO.AttributeModel> attributeMetaModellist = new Collection<MDMBO.AttributeModel>();

            attributeMetaModellist = attributeModelOperationsDA.GetCommonAttributesByContainerAndEntityType(catalogId, entityTypeId, localeId);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeModelManager.AttributeModelOperationBL.GetCommonAttributesByContainerAndEntityType");

            return attributeMetaModellist;
        }

        /// <summary>
        /// This method can find common attributes based on search values, it can sort the attribute list by given parameter and return only the required number of attributes 
        /// It gives just the basic details of common attributes
        /// </summary>
        /// <param name="searchBy">Search can be done on any of the following parameters: 1. ShortName, 2. LongName, 3. AttributeTypeName, 4. AttributeDataTypeName, 5. AttributeDisplayTypeName</param>
        /// <param name="searchValue">Value to be searched in the SearchField</param>
        /// <param name="sortBy">Sort by this column's value</param>
        /// <param name="countFrom">Attribute starts from</param>
        /// <param name="countTo">Attribute up to</param>
        /// <param name="localeId">Attribute details in this locale</param>
        /// <returns>List of AttributeMetaModels based on criteria</returns>
        public Collection<MDMBO.AttributeModel> FindCommonAttributes(String searchBy, String searchValue, String sortBy, Int32 countFrom, Int32 countTo, Int32 localeId)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            return this.FindCommonAttributes(searchBy, searchValue, sortBy, countFrom, countTo, localeId, userName);
        }

        /// <summary>
        /// This method can find common attributes based on search values, it can sort the attribute list by given parameter and return only the required number of attributes 
        /// It gives just the basic details of common attributes
        /// </summary>
        /// <param name="searchBy">Search can be done on any of the following parameters: 1. ShortName, 2. LongName, 3. AttributeTypeName, 4. AttributeDataTypeName, 5. AttributeDisplayTypeName</param>
        /// <param name="searchValue">Value to be searched in the SearchField</param>
        /// <param name="sortBy">Sort by this column's value</param>
        /// <param name="countFrom">Attribute starts from</param>
        /// <param name="countTo">Attribute up to</param>
        /// <param name="localeId">Attribute details in this locale</param>
        /// <param name="userName">current user</param>
        /// <returns>List of AttributeMetaModels based on criteria</returns>
        public Collection<MDMBO.AttributeModel> FindCommonAttributes(String searchBy, String searchValue, String sortBy, Int32 countFrom, Int32 countTo, Int32 localeId, String userName)
        {
            return this.FindAttributes(AttributeModelType.Common, searchBy, searchValue, sortBy, countFrom, countTo, localeId, userName);
        }

        /// <summary>
        /// This method can find category attributes based on search values, it can sort the attribute list by given parameter and return only the required number of attributes 
        /// It gives just the basic details of category attributes
        /// This method is for current user in context
        /// </summary>
        /// <param name="searchBy">Search can be done on any of the following parameters: 1. ShortName, 2. LongName, 3. AttributeTypeName, 4. AttributeDataTypeName, 5. AttributeDisplayTypeName</param>
        /// <param name="searchValue">Value to be searched in the SearchField</param>
        /// <param name="sortBy">Sort by this column's value</param>
        /// <param name="countFrom">Attribute starts from</param>
        /// <param name="countTo">Attribute up to</param>
        /// <param name="localeId">Attribute details in this locale</param>
        /// <returns>List of AttributeMetaModels based on criteria</returns>
        public Collection<MDMBO.AttributeModel> FindCategoryAttributes(String searchBy, String searchValue, String sortBy, Int32 countFrom, Int32 countTo, Int32 localeId)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            return this.FindCategoryAttributes(searchBy, searchValue, sortBy, countFrom, countTo, localeId, userName);
        }

        /// <summary>
        /// This method can find category attributes based on search values, it can sort the attribute list by given parameter and return only the required number of attributes 
        /// It gives just the basic details of category attributes
        /// </summary>
        /// <param name="searchBy">Search can be done on any of the following parameters: 1. ShortName, 2. LongName, 3. AttributeTypeName, 4. AttributeDataTypeName, 5. AttributeDisplayTypeName</param>
        /// <param name="searchValue">Value to be searched in the SearchField</param>
        /// <param name="sortBy">Sort by this column's value</param>
        /// <param name="countFrom">Attribute starts from</param>
        /// <param name="countTo">Attribute up to</param>
        /// <param name="localeId">Attribute details in this locale</param>
        /// <param name="userName">current user</param>
        /// <returns>List of AttributeMetaModels based on criteria</returns>
        public Collection<MDMBO.AttributeModel> FindCategoryAttributes(String searchBy, String searchValue, String sortBy, Int32 countFrom, Int32 countTo, Int32 localeId, String userName)
        {
            return this.FindAttributes(AttributeModelType.Category, searchBy, searchValue, sortBy, countFrom, countTo, localeId, userName);
        }

        /// <summary>
        /// This method can find attributes based on search values, it can sort the attribute list by given parameter and return only the required number of attributes 
        /// It gives just the basic details of attributes
        /// This method is for current user in context
        /// </summary>
        /// <param name="attributeModelType">Common or Category specific</param>
        /// <param name="searchBy">Search can be done on any of the following parameters: 1. ShortName, 2. LongName, 3. AttributeTypeName, 4. AttributeDataTypeName, 5. AttributeDisplayTypeName</param>
        /// <param name="searchValue">Value to be searched in the SearchField</param>
        /// <param name="sortBy">Sort by this column's value</param>
        /// <param name="countFrom">Attribute starts from</param>
        /// <param name="countTo">Attribute up to</param>
        /// <param name="localeId">Attribute details in this locale</param>
        /// <returns>List of AttributeMetaModels based on criteria</returns>
        public Collection<MDMBO.AttributeModel> FindAttributes(AttributeModelType attributeModelType, String searchBy, String searchValue, String sortBy, Int32 countFrom, Int32 countTo, Int32 localeId)
        {
            String userName = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;
            return this.FindAttributes(attributeModelType, searchBy, searchValue, sortBy, countFrom, countTo, localeId, userName);
        }

        /// <summary>
        /// This method can find attributes based on search values, it can sort the attribute list by given parameter and return only the required number of attributes 
        /// It gives just the basic details of attributes
        /// </summary>
        /// <param name="attributeModelType">Common or Category specific</param>
        /// <param name="searchBy">Search can be done on any of the following parameters: 1. ShortName, 2. LongName, 3. AttributeTypeName, 4. AttributeDataTypeName, 5. AttributeDisplayTypeName</param>
        /// <param name="searchValue">Value to be searched in the SearchField</param>
        /// <param name="sortBy">Sort by this column's value</param>
        /// <param name="countFrom">Attribute starts from</param>
        /// <param name="countTo">Attribute up to</param>
        /// <param name="localeId">Attribute details in this locale</param>
        /// <param name="userName">current user</param>
        /// <returns>List of AttributeMetaModels based on criteria</returns>
        public Collection<MDMBO.AttributeModel> FindAttributes(AttributeModelType attributeModelType, String searchBy, String searchValue, String sortBy, Int32 countFrom, Int32 countTo, Int32 localeId, String userName)
        {
            Int32 attrType = -1;//common
            if (attributeModelType == AttributeModelType.Category)
                attrType = -2;//category

            Collection<MDMBO.AttributeModel> collectionCA = new Collection<MDM.BusinessObjects.AttributeModel>();
            string strXMLData = String.Empty;
            AttributeModelOperationsDA attributeModelOperationsDA = new AttributeModelOperationsDA();
            strXMLData = attributeModelOperationsDA.GetAllAttributes(attrType, 0, 2000, "", "ShortName", "ShortName", localeId, false, userName);
            if (!string.IsNullOrEmpty(strXMLData))
            {
                System.Xml.XmlDocument _xmlDocument = new System.Xml.XmlDocument();
                _xmlDocument.LoadXml(strXMLData);
                System.Xml.XmlNodeList nodelist = _xmlDocument.SelectNodes("Attributes/Attr");
                foreach (System.Xml.XmlNode node in nodelist)
                {
                    MDMBO.AttributeModelBaseProperties attributeModelBaseProperties = new MDMBO.AttributeModelBaseProperties();

                    attributeModelBaseProperties.Id = XmlHelper.GetXmlAttributeIntegerValue(node, "PK_Attribute");
                    attributeModelBaseProperties.Name = XmlHelper.GetXmlAttributeStringValue(node, "ShortName");
                    attributeModelBaseProperties.LongName = XmlHelper.GetXmlAttributeStringValue(node, "LongName");
                    attributeModelBaseProperties.AttributeParentName = XmlHelper.GetXmlAttributeStringValue(node, "AttributeParentName");
                    attributeModelBaseProperties.AttributeDataTypeName = XmlHelper.GetXmlAttributeStringValue(node, "AttributeDataTypeName");
                    attributeModelBaseProperties.AttributeDisplayTypeName = XmlHelper.GetXmlAttributeStringValue(node, "AttributeDispTypeName");

                    MDMBO.AttributeModel attributeModel = new MDM.BusinessObjects.AttributeModel(attributeModelBaseProperties, null, null);

                    collectionCA.Add(attributeModel);
                }
            }
            return collectionCA;
        }

        /// <summary>
        ///  Get Attribute Xml
        /// </summary>
        /// <param name="organizationId">Indicates the organization Id</param>
        /// <param name="catalogId">Indicates the catalogId Id</param>
        /// <param name="nodeType">Indicates the nodeType</param>
        /// <param name="branchLevel">Indicates the</param>
        /// <param name="includeComplexAttrChildren">Indicates the includeComplexAttrChildren</param>
        /// <param name="excludeableSearchable">Indicates the excludeableSearchable</param>
        /// <param name="locales">Indicates the collection locales</param>
        /// <param name="systemDataLocale">Indicates the systemDataLocale</param>
        /// <returns></returns>
        public String GetCatalogNodeTypeAttrbiuteAsXml(Int32 organizationId, Int32 catalogId, String nodeType, Int32 branchLevel, Boolean includeComplexAttrChildren, Boolean excludeableSearchable, Collection<LocaleEnum> locales, LocaleEnum systemDataLocale)
        {
            AttributeModelOperationsDA _attributeModelOperationDA = new AttributeModelOperationsDA();

            return _attributeModelOperationDA.GetCatalogNodeTypeAttrbiuteAsXml(organizationId, catalogId, nodeType, branchLevel, includeComplexAttrChildren, excludeableSearchable, locales, systemDataLocale);
        }

        #endregion

    }
}
