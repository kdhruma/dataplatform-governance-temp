using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace MDM.AttributeManager.Business
{
    using MDM.AttributeManager.Data;
    using MDM.AttributeModelManager.Business;
    using MDM.BusinessObjects;
    using MDM.ConfigurationManager.Business;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;
    using MDM.Utility;
    using MDM.LookupManager.Business;

    public class AttributeBL : BusinessLogicBase
    {
        #region Fields

        private SecurityPrincipal _securityPrincipal = null;

        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public AttributeBL()
        {
            GetSecurityPrincipal();
        }

        public AttributeBL(SecurityPrincipal securityPrincipalInstance)
        {
            _securityPrincipal = securityPrincipalInstance;
            SecurityPrincipalHelper.ValidateSecurityPrincipal(_securityPrincipal);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public String CopyPasteContent(Int32 fromCnodeId, Int32 tocnodeId, Int32 fromCatalogId, Int32 toCatalogId, String comAttributes, String techAttributes, String relationships, String userId, String programName, ref Int32 coreUpdateCount, ref Int32 techUpdateCount, ref Int32 partCount)
        {
            String returnValue = String.Empty;
            try
            {
                AttributeDA attributeDA = new AttributeDA();
                returnValue = attributeDA.CopyPasteContent(fromCnodeId, tocnodeId, fromCatalogId, toCatalogId, comAttributes, techAttributes, relationships, userId, programName, ref coreUpdateCount, ref  techUpdateCount, ref  partCount);
            }
            finally
            {
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the attributes for the requested attribute group ids and attribute group ids based on attribute model context
        /// </summary>
        /// <param name="attributeIds">Ids of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Ids of attribute groups for which models are required</param>
        /// <param name="excludeAttributeIds">Ids to be excluded from the requested models</param>
        /// <param name="attributeModelContext">The data context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute Models object</returns>
        /// <exception cref="ArgumentNullException">Will be raised when the parameter 'attributeModelContext' is null</exception>
        /// <exception cref="DuplicateObjectException">Will be raised when the return collection have duplicate attribute object</exception>
        public AttributeCollection GetAttributesFromModel(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext)
        {
            if (attributeModelContext == null)
                throw new ArgumentNullException("attributeModelContext");

            AttributeCollection attributes = new AttributeCollection();

            // Get attribute models
            AttributeModelBL attributeModelBL = new AttributeModelBL();
            AttributeModelCollection attributeModelCollection = attributeModelBL.Get(attributeIds, attributeGroupIds, excludeAttributeIds, attributeModelContext);

            if (attributeModelCollection != null)
            {
                // loop through each item in attribute model and create related attribute instance object
                foreach (AttributeModel attributeModel in attributeModelCollection)
                {
                    //TODO:: Write switch to create complex and complex collection object rather than attribute object
                    Attribute attribute = new Attribute(attributeModel);

                    if (!attributes.Contains(attribute.Id, attribute.Locale))
                    {
                        attributes.Add(attribute);
                    }
                }
            }

            return attributes;
        }

        /// <summary>
        /// Gets the attributes based on attribute model context
        /// </summary>
        /// <param name="attributeModelContext">The attribute model context for which models needs to be fetched</param>
        /// <returns>Collection of Attribute collection object</returns>
        /// <exception cref="MDMOperationException">Will be raised when the parameter 'attribute model context' is null</exception>
        public AttributeCollection GetAttributesFromModel(AttributeModelContext attributeModelContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeManager.Business.AttributeBL.GetAttributesFromModel", false);

            AttributeCollection attributes = new AttributeCollection();

            try
            {
                if (attributeModelContext == null)
                {
                    throw new MDMOperationException("111801", "Attribute Model Context is not available", "AttributeManager", String.Empty, "Get"); //Attribute Model Context is not available.
                }

                // Get attribute models
                AttributeModelBL attributeModelBL = new AttributeModelBL();
                AttributeModelCollection attributeModelCollection = attributeModelBL.Get(attributeModelContext);

                if (attributeModelCollection != null)
                {
                    // loop through each item in attribute model and create related attribute instance object
                    foreach (AttributeModel attributeModel in attributeModelCollection)
                    {
                        //TODO:: Write switch to create complex and complex collection object rather than attribute object
                        Attribute attribute = new Attribute(attributeModel);

                        if (!attributes.Contains(attribute.Id, attribute.Locale))
                        {
                            attributes.Add(attribute);
                        }
                    }
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeManager.Business.AttributeBL.GetAttributesFromModel");
            }

            return attributes;
        }

        /// <summary>
        /// Gets the attributes and attribute models
        /// </summary>
        /// <param name="attributeIds">Ids of the attributes for which models are required</param>
        /// <param name="attributeGroupIds">Ids of attribute groups for which models are required</param>
        /// <param name="excludeAttributeIds">Ids to be excluded from the requested models</param>
        /// <param name="attributeModelContext">The attribute model context for which models needs to be fetched</param>
        /// <param name="attributeCollection">Resulting attributes collection</param>
        /// <param name="attributeModelCollection">Resulting attributes models collection</param>
        /// <exception cref="MDMOperationException">Will be raised when the parameter 'attribute model context' is null</exception>
        public void GetAttributesAndModels(Collection<Int32> attributeIds, Collection<Int32> attributeGroupIds, Collection<Int32> excludeAttributeIds, AttributeModelContext attributeModelContext, out AttributeCollection attributeCollection, out AttributeModelCollection attributeModelCollection)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeManager.Business.AttributeBL.GetAttributesAndModels", false);

            attributeCollection = new AttributeCollection();
            attributeModelCollection = new AttributeModelCollection();

            try
            {
                if (attributeModelContext == null)
                {
                    throw new MDMOperationException("111801", "Attribute Model Context is not available", "AttributeManager", String.Empty, "Get"); //Attribute Model Context is not available.
                }

                // Get attribute models
                AttributeModelCollection modelCollection = null;
                AttributeModelBL attributeModelBL = new AttributeModelBL();
                if ((attributeIds != null && attributeIds.Count > 0) || (attributeGroupIds != null && attributeGroupIds.Count > 0))
                {
                    modelCollection = attributeModelBL.Get(attributeIds, attributeGroupIds, excludeAttributeIds, attributeModelContext);
                }
                else
                {
                    modelCollection = attributeModelBL.Get(attributeModelContext);
                    if (excludeAttributeIds != null && excludeAttributeIds.Count > 0)
                    {
                        foreach (var id in excludeAttributeIds)
                        {
                            IAttributeModelCollection items = modelCollection.GetAttributeModel(id);
                            foreach (AttributeModel item in items)
                            {
                                modelCollection.Remove(item);
                            }
                        }
                    }
                }

                if (modelCollection != null)
                {
                    // loop through each item in attribute model and create related attribute instance object
                    foreach (AttributeModel attributeModel in modelCollection)
                    {
                        Attribute attribute = new Attribute(attributeModel);

                        if (!attributeCollection.Contains(attribute.Id, attribute.Locale))
                        {
                            attributeCollection.Add(attribute);
                        }
                    }
                    attributeModelCollection = modelCollection;
                }
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeManager.Business.AttributeBL.GetAttributesAndModels");
            }
        }

        /// <summary>
        /// Gives the string representing AttributeName - Value string for Breadcrumb.
        /// Can return datatable representing AttributeName - Value string for multiple UserConfigType like Breadcrumb , Title etc.
        /// </summary>
        /// <param name="breadcrumbConfigXML">
        /// XML representing AttributeName - ID and some additional attributes needed
        /// Sample XML : 
        ///         <UserConfigs>
        ///           <UserConfig UserConfigType="Breadcrumb">
        ///            <Attribute Name="localizable Decimal Apply locale format" ID="110099" SelectedSeqNo="1" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="Product ID" ID="109838" SelectedSeqNo="2" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="FH_BooleanDropdown Attribute" ID="109778" SelectedSeqNo="3" Selected="true" LabelSeperator="&gt;&gt;" />
        ///           </UserConfig>
        ///           <UserConfig UserConfigType="Title">
        ///             <Attribute Name="localizable Decimal Apply locale format" ID="110099" SelectedSeqNo="1" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="Product ID" ID="109838" SelectedSeqNo="2" Selected="true" LabelSeperator="&gt;&gt;" />
        ///             <Attribute Name="FH_BooleanDropdown Attribute" ID="109778" SelectedSeqNo="3" Selected="true" LabelSeperator="&gt;&gt;" />
        ///           </UserConfig>
        ///         </UserConfigs>
        /// </param>
        /// <param name="FK_Org">FK_Org of an item</param>
        /// <param name="FK_Catalog">FK_Catalog of an item</param>
        /// <param name="FK_CNode">CNodeID of an item</param>
        /// <param name="FK_Locale">FK_Locale of an Item</param>
        /// <returns>
        ///     Datatable containing 2 columns 1. UserConfigType 2.AttributeName - value string
        ///     No. of rows in DataTable = No. of UserConfigType in input XML
        /// </returns>
        public DataTable GetBreadcrumbAttributeValueString(String breadcrumbConfigXML, Int32 FK_Org, Int32 FK_Catalog, Int64 FK_CNode, Int32 FK_Locale)
        {
            DataTable result = null;
            Int32 loginUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserId;

            if (FK_Org <= 0)
                throw new ArgumentException(String.Format("Org Id does not have valid value. Value: {0}", FK_Org), "FK_Org");

            if (FK_Catalog <= 0)
                throw new ArgumentException(String.Format("Container Id does not have valid value. Value: {0}", FK_Catalog), "FK_Catalog");

            if (FK_CNode <= 0)
                throw new ArgumentException(String.Format("Entity Id does not have valid value. Value: {0}", FK_CNode), "FK_CNode");

            if (FK_Locale <= 0)
                throw new ArgumentException(String.Format("Locale Id does not have valid value. Value: {0}", FK_Locale), "FK_Locale");

            if (String.IsNullOrEmpty(breadcrumbConfigXML))
                throw new ArgumentException(String.Format("Breadcrumb Config XML does not have valid value. Value: {0}", breadcrumbConfigXML), "breadcrumbConfigXML");

            AttributeDA attributeDA = new AttributeDA();
            result = attributeDA.GetBreadcrumbAttributeValueString(breadcrumbConfigXML, FK_Org, FK_Catalog, FK_CNode, FK_Locale, loginUser);

            return result;
        }

        /// <summary>
        /// Get Complex Data for Complex Attribute's version history for requested attribute id
        /// </summary>
        /// <param name="entityId">EntityId for which attribute history is needed</param>
        /// <param name="containerId">Container Id under which Entity is created</param>
        /// <param name="attributeId">Attribute id for which we needs data</param>
        /// <param name="auditRefId">AuditRefId for which we needs data</param>
        /// <param name="locale">locale details</param>
        /// <param name="callerContext">Indicates the Caller Context</param>
        /// <returns>Attribute object with complex attribute's data</returns>
        public Attribute GetComplexDataByAuditRefId(Int64 entityId, Int32 containerId, Int32 attributeId, Int64 auditRefId, LocaleEnum locale, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("AttributeManager.AttributeBL.GetComplexDataByAuditRefId", MDMTraceSource.AttributeGet, false);
            }

            Attribute attribute = null;

            try
            {
                if (auditRefId > 0)
                {
                    #region Get Attribute Model

                    AttributeModelBL attributeModelManager = new AttributeModelBL();
                    AttributeModelContext attributeModelContext = new AttributeModelContext();
                    attributeModelContext.AttributeModelType = AttributeModelType.AttributeMaster;
                    attributeModelContext.Locales.Add(locale);

                    AttributeModelCollection attributeModels = attributeModelManager.GetById(attributeId, attributeModelContext);

                    AttributeModel attributeModel = null;

                    if (attributeModels != null && attributeModels.Count > 0)
                    {
                        attributeModel = attributeModels.ElementAt(0);
                    }

                    #endregion

                    if (attributeModel != null && attributeModel.AttributeModels != null)
                    {
                        AttributeDA attributeDA = new AttributeDA();
                        DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

                        if (!attributeModel.IsLocalizable)
                            locale = GlobalizationHelper.GetSystemDataLocale();

                        Attribute result = attributeDA.GetComplexDataByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, command, attributeModel);

                        if (result != null)
                        {
                            Dictionary<String, AttributeModel> childAttributeModels = new Dictionary<String, AttributeModel>();
                            foreach (AttributeModel childAttributeModel in attributeModel.GetChildAttributeModels())
                            {
                                childAttributeModels.Add(String.Concat(childAttributeModel.Id.ToString(), locale.ToString()), childAttributeModel);
                            }

                            attribute = new Attribute(attributeModel);
                            Dictionary<Int32, Lookup> lookupDisplayValues = new Dictionary<Int32, Lookup>();
                            Lookup lookup = null;
                            LookupBL lookupManager = new LookupBL();
                            ApplicationContext applicationContext = new ApplicationContext();
                            foreach (Attribute instanceAttribute in result.Attributes)
                            {
                                foreach (Attribute childAttribute in instanceAttribute.Attributes)
                                {
                                    Int32 childAttributeId = childAttribute.Id;
                                    AttributeModel childAttrModel = childAttributeModels[String.Concat(childAttributeId.ToString(), locale.ToString())];
                                    if (childAttrModel != null)
                                    {
                                        if (childAttrModel.IsLookup)
                                        {
                                            #region Getlookup Display Value

                                            ValueCollection valueCollection = (ValueCollection)childAttribute.GetCurrentValues();
                                            if (valueCollection.Count() > 0)
                                            {
                                                foreach (Value value in valueCollection)
                                                {
                                                    if (!value.HasInvalidValue)
                                                    {
                                                        Int32 lookupId =
                                                            ValueTypeHelper.Int32TryParse(Convert.ToString(value.AttrVal), 0);

                                                        if (lookupDisplayValues.ContainsKey(childAttributeId))
                                                        {
                                                            lookup = lookupDisplayValues[childAttributeId];
                                                            if (lookupId > 0)
                                                            {
                                                                value.SetDisplayValue(lookup.GetDisplayFormatById(lookupId));    
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (value.AttrVal != null)
                                                            {
                                                                Collection<Int32> refIds = new Collection<Int32>();
                                                                Int32 valueRefId = ValueTypeHelper.Int32TryParse(value.AttrVal.ToString(), -1);

                                                                if (valueRefId > 0)
                                                                {
                                                                    refIds.Add(valueRefId);

                                                                    if (refIds.Count > 0)
                                                                    {
                                                                        lookup = lookupManager.Get(childAttributeId, locale, -1, refIds, applicationContext, callerContext, false);
                                                                        lookupDisplayValues.Add(childAttributeId, lookup);
                                                                        if (lookupId > 0)
                                                                        {
                                                                            value.SetDisplayValue(lookup.GetDisplayFormatById(lookupId));    
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    childAttribute.IsLookup = childAttrModel.IsLookup;
                                                }
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            if (!childAttribute.HasInvalidValues)
                                            {
                                                String attributeDataTypeName = childAttrModel.AttributeDataTypeName.ToLowerInvariant();

                                                if (attributeDataTypeName.Equals("date"))
                                                {
                                                    childAttribute.AttributeDataType = AttributeDataType.Date;
                                                }

                                                if (attributeDataTypeName.Equals("datetime"))
                                                {
                                                    childAttribute.AttributeDataType = AttributeDataType.DateTime;
                                                }

                                                if (attributeDataTypeName.Equals("decimal"))
                                                {
                                                    childAttribute.AttributeDataType = AttributeDataType.Decimal;
                                                }
                                            }
                                        }

                                        childAttribute.Name = childAttrModel.Name;
                                        childAttribute.AttributeParentName = childAttrModel.AttributeParentName;
                                    }
                                }

                                attribute.AddComplexChildRecord(instanceAttribute.Attributes);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("AttributeManager.AttributeBL.GetComplexDataByAuditRefId", MDMTraceSource.AttributeGet);
                }
            }

            return attribute;
        }


        /// <summary>
        /// Get Hierarchical attribute at specific version of version history for requested auditRefId, attribute id, entity id
        /// </summary>
        /// <param name="entityId">EntityId for which attribute history is needed</param>
        /// <param name="containerId">Container Id under which Entity is created</param>
        /// <param name="attributeId">Attribute id for which we needs data</param>
        /// <param name="auditRefId">AuditRefId for which we needs data</param>
        /// <param name="locale">locale details</param>
        /// <param name="callerContext">Indicates the Caller Context</param>
        /// <returns>Hierarchical attribute at some specific point of history</returns>
        public Attribute GetHierarchicalAttributeByAuditRefId(Int64 entityId, Int32 containerId, Int32 attributeId, Int64 auditRefId, LocaleEnum locale, CallerContext callerContext)
        {
            if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("AttributeManager.AttributeBL.GetHierarchicalAttributeByAuditRefId", MDMTraceSource.AttributeGet, false);
            }

            Attribute hierarchicalAttribute = null;

            try
            {
                if (auditRefId > 0)
                {
                    #region Get Attribute Model

                    AttributeModelBL attributeModelManager = new AttributeModelBL();
                    AttributeModelContext attributeModelContext = new AttributeModelContext
                    {
                        AttributeModelType = AttributeModelType.AttributeMaster
                    };
                    attributeModelContext.Locales.Add(locale);

                    AttributeModelCollection attributeModels = attributeModelManager.GetById(attributeId, attributeModelContext);

                    AttributeModel attributeModel = null;

                    if (attributeModels != null && attributeModels.Count > 0)
                    {
                        attributeModel = attributeModels.ElementAt(0);
                    }

                    #endregion

                    if (attributeModel != null)
                    {
                        AttributeDA attributeDA = new AttributeDA();
                        DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);
                        
                        hierarchicalAttribute = attributeDA.GetHierarchicalAttributeByAuditRefId(entityId, containerId, attributeId, auditRefId, locale, command, attributeModel);
                        
                    }
                }
            }
            finally
            {
                if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.StopTraceActivity("AttributeManager.AttributeBL.GetHierarchicalAttributeByAuditRefId", MDMTraceSource.AttributeGet);
                }
            }

            return hierarchicalAttribute;
        }

        #region Private Methods

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        #endregion

        #endregion
    }
}
