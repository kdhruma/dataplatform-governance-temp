using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Diagnostics;

namespace MDM.AttributeModelManager.Business
{
    using MDM.AttributeModelManager.Data;
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using MDM.MessageManager.Business;
    using MDM.Core.Exceptions;
    
    /// <summary>
    /// 
    /// </summary>
    public class AttributeGroupBL : BusinessLogicBase
    {
        #region Fields

        private Collection<AttributeGroup> attributeGroups = null;

        #endregion

        #region Private Enum

        enum AttributeGroupType
        {
            Technical,
            Common,
            Relationship
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Common Attribute Group Details
        /// </summary>
        /// <param name="catalogID">Indicates the catalogID of an Attribute Group</param>
        /// <param name="nodeType">Indicates the nodeType of an Attribute Group</param>
        /// <returns></returns>
        public static Collection<AttributeGroup> GetCommonAttributeGroups(String catalogID, String nodeType,String currentDataLocaleName)
        {
            XmlDocument attGroupXML = new XmlDocument();
            int iCatalogID = 0;
            int iNodeTypeID = 0;

            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            LocaleEnum currentDataLocale = LocaleEnum.UnKnown;
            LocaleEnum.TryParse(currentDataLocaleName, out currentDataLocale);          
            locales.Add(currentDataLocale);

            Int32.TryParse(catalogID, out iCatalogID);
            Int32.TryParse(nodeType, out iNodeTypeID);
            Collection<AttributeGroup> attributeGroupList = new Collection<AttributeGroup>();
            AttributeModelBL attributeModelManager = new AttributeModelBL();

            if (iCatalogID > 0 && iNodeTypeID > 0)
            {
                AttributeModelContext attributeModelContext = new AttributeModelContext();
                attributeModelContext.AttributeModelType = AttributeModelType.Common;
                attributeModelContext.ContainerId = iCatalogID;
                attributeModelContext.EntityTypeId = iNodeTypeID;
                attributeModelContext.Locales = locales;

                //get all mapped attributes
                AttributeModelCollection attributeModelCollection = attributeModelManager.Get(attributeModelContext);
                
                //Create attribute groups
                if (attributeModelCollection != null)
                {
                    foreach (AttributeModel attributeModel in attributeModelCollection)
                    {
                        if (attributeModel != null)
                        {
                            AttributeGroup attributeGroup = new AttributeGroup();

                            attributeGroup.Id = attributeModel.AttributeParentId;
                            attributeGroup.Name = attributeModel.AttributeParentName;
                            attributeGroup.LongName = attributeModel.AttributeParentLongName;
                            if (!attributeGroupList.Contains(attributeGroup))
                            {
                                attributeGroupList.Add(attributeGroup);
                            }
                        }
                    }
                }

                //sort the list by long name, and fill back to collection
                IList<AttributeGroup> ilistAttributeGroups = attributeGroupList.OrderBy(attributeGroup => attributeGroup.LongName).ToList<AttributeGroup>();
                attributeGroupList = new Collection<AttributeGroup>(ilistAttributeGroups);

                //Remove the duplicate values from the list
                IEnumerable<AttributeGroup> iEnumerableAttributeGroup = attributeGroupList.Distinct().ToList<AttributeGroup>();
                attributeGroupList = new Collection<AttributeGroup>(iEnumerableAttributeGroup.ToList<AttributeGroup>());              
            }

            return attributeGroupList;
        }

        /// <summary>
        /// Get Technical AttributeGroups
        /// </summary>
        /// <param name="categoryID">Indicates the categoryID of an Attribute Group</param>
        /// <param name="catalogID">Indicates the catalogID of an Attribute Group</param>
        /// <param name="localeID">Indicates the localeID of an Attribute Group</param>
        /// <returns></returns>
        public static Collection<AttributeGroup> GetTechnicalAttributeGroups(String categoryID, String catalogID, String currentDataLocaleName)
        {
            Int64 iCategoryID = 0;
            Int64.TryParse(categoryID, out iCategoryID);

            Int32 iCatalogID = 0;
            Int32.TryParse(catalogID, out iCatalogID);

            //Populate Locales
            Collection<LocaleEnum> locales = new Collection<LocaleEnum>();
            LocaleEnum currentDataLocale = LocaleEnum.UnKnown;            
            LocaleEnum.TryParse(currentDataLocaleName, out currentDataLocale);
            locales.Add(currentDataLocale);

            Collection<AttributeGroup> attributeGroupList = new Collection<AttributeGroup>();

            AttributeModelBL attributeModelManager = new AttributeModelBL();

            if (iCategoryID > 0 && iCatalogID > 0 && (Int32)currentDataLocale > 0)
            {
                //Create AttributeModelContext to get all mapped attributes

                AttributeModelContext attributeModelContext = new AttributeModelContext();
                attributeModelContext.AttributeModelType = AttributeModelType.Category;
                attributeModelContext.ContainerId = iCatalogID;
                attributeModelContext.Locales = locales;
                attributeModelContext.CategoryId = iCategoryID;

                //Get all mapped attributes
                AttributeModelCollection attributeModelCollection = attributeModelManager.Get(attributeModelContext);

                //Create attribute groups out of mapped attributes
                if (attributeModelCollection != null)
                {
                    foreach (AttributeModel attributeModel in attributeModelCollection)
                    {
                        if (attributeModel != null)
                        {
                            AttributeGroup attributeGroup = new AttributeGroup();

                            attributeGroup.Id = attributeModel.AttributeParentId;
                            attributeGroup.Name = attributeModel.AttributeParentName;
                            attributeGroup.LongName = attributeModel.AttributeParentLongName;
                            if (!attributeGroupList.Contains(attributeGroup))
                            {
                                attributeGroupList.Add(attributeGroup);
                            }
                        }
                    }
                }
                //sort the list by long name, and fill back to collection
                IList<AttributeGroup> ilistAttributeGroups = attributeGroupList.OrderBy(attributeGroup => attributeGroup.LongName).ToList<AttributeGroup>();
                attributeGroupList = new Collection<AttributeGroup>(ilistAttributeGroups);

                //Remove the duplicate values from the list
                IEnumerable<AttributeGroup> iEnumerableAttributeGroup = attributeGroupList.Distinct().ToList<AttributeGroup>();
                attributeGroupList = new Collection<AttributeGroup>(iEnumerableAttributeGroup.ToList<AttributeGroup>());  
            }

            return attributeGroupList;
        }

        /// <summary>
        /// GetRelationshipAttributeGroups
        /// </summary>
        /// <param name="catalogID">Indicates the catalogID of an Attribute Group</param>
        /// <param name="nodeTypeID">Indicates the nodeTypeID of an Attribute Group</param>
        /// <returns></returns>
        public static Collection<AttributeGroup> GetRelationshipAttributeGroups(String catalogID, String nodeTypeID)
        {
            Int32 iCatalogID = 0;
            Int32 iNodeTypeId = 0;

            Int32.TryParse(catalogID, out iCatalogID);
            Int32.TryParse(nodeTypeID, out iNodeTypeId);
            Collection<AttributeGroup> attributeGroupList = null;

            AttributeGroupDA attributeGroupDA = new AttributeGroupDA();

            if (iCatalogID > 0 && iNodeTypeId > 0)
            {
                attributeGroupList = new Collection<AttributeGroup>();

                attributeGroupList = attributeGroupDA.GetRelationshipAttributeGroups(iCatalogID, iNodeTypeId);
                IList<AttributeGroup> ilistAttributeGroups = attributeGroupList.OrderBy(attributeGroup => attributeGroup.LongName).ToList<AttributeGroup>();
                attributeGroupList = new Collection<AttributeGroup>(ilistAttributeGroups);
            }

            return attributeGroupList;
        }

        /// <summary>
        /// Get attribute group name collection based on provided name and context.
        /// </summary>
        /// <param name="attributeGroupShortName">Short Name of the attribute group</param>
        /// <param name="attributeModelContext">Attribute model context defines context from which we need to get attribute group</param>
        /// <param name="callerContext">Caller context to denote application and module information</param>
        /// <returns>Returns collection of attribute group with all details.</returns>
        public Collection<AttributeGroup> GetByName(String attributeGroupShortName, AttributeModelContext attributeModelContext, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeGroupBL.GetByName", MDMTraceSource.DataModel, false);

            #region parameter validation
            ValidateCallerContext(callerContext, "GetByName");

            if(String.IsNullOrEmpty(attributeGroupShortName))
            {
                String errorMessage = this.GetSystemLocaleMessage("112690", callerContext).Message;
                throw new MDMOperationException("112690", errorMessage, "AttributeModelManager.AttributeGroupBL", String.Empty, "GetByName");
            }

            if (attributeModelContext == null)
            {
                String errorMessage = this.GetSystemLocaleMessage("112691", callerContext).Message;
                throw new MDMOperationException("112691", errorMessage, "AttributeModelManager.AttributeGroupBL", String.Empty, "GetByName");
            }
            #endregion

            try
            {
                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Attribute group Name : " + attributeGroupShortName, MDMTraceSource.DataModel);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "CallerContext : Application - " + callerContext.Application + " Module - " + callerContext.Module, MDMTraceSource.DataModel);
                }

                #region Get Attribute group
                attributeGroups = new Collection<AttributeGroup>();

                //get all mapped attributes
                AttributeModelBL attributeModelManager = new AttributeModelBL();
                AttributeModelCollection attributeModelCollection = attributeModelManager.Get(attributeModelContext);

                //Create attribute groups
                if (attributeModelCollection != null)
                {
                    foreach (AttributeModel attributeModel in attributeModelCollection)
                    {
                        if (attributeModel != null && attributeModel.AttributeParentName.Equals(attributeGroupShortName))
                        {
                            AttributeGroup attributeGroup = new AttributeGroup();

                            attributeGroup = new AttributeGroup();
                            attributeGroup.Id = attributeModel.AttributeParentId;
                            attributeGroup.Name = attributeModel.AttributeParentName;
                            attributeGroup.LongName = attributeModel.AttributeParentLongName;
                            if (!attributeGroups.Contains(attributeGroup))
                                attributeGroups.Add(attributeGroup);
                        }
                    }
                }
                #endregion
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("AttributeGroupBL.GetByName", MDMTraceSource.DataModel);
            }

            return attributeGroups;
        }

        /// <summary>
        /// Get AttributeType
        /// </summary>
        /// <param name="common">Indicates the common of an Attribute Group</param>
        /// <param name="technical">Indicates the technical of an Attribute Group</param>
        /// <param name="relationship">Indicates the relationship of an Attribute Group</param>
        /// <param name="locales">Indicates collection of locales.</param>
        /// <param name="systemLocale">Indicates system locale</param>
        /// <returns></returns>
        public Collection<AttributeGroup> GetByAttributeType(Int32 common, Int32 technical, Int32 relationship,Collection<LocaleEnum> locales,LocaleEnum systemLocale)
        {
            attributeGroups = new Collection<AttributeGroup>();
            AttributeGroupDA attributeGroupDA = new AttributeGroupDA();

            attributeGroups=attributeGroupDA.GetByAttributeType(common, technical, relationship,locales,systemLocale); 

            return attributeGroups;
        }
        
        #endregion

        #region Private Methods
        private void ValidateCallerContext(CallerContext callerContext, String methodName)
        {
            String errorMessage = String.Empty;

            if (callerContext == null)
            {
                errorMessage = "CallerContext cannot be null.";
                throw new MDMOperationException("111846", errorMessage, "AttributeModelManager.AttributeGroupBL", String.Empty, methodName, MDMTraceSource.DataModel);
            }

        }

        /// <summary>
        /// Returns Message Object based on message code
        /// </summary>
        /// <param name="messageCode">Message Code</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        private LocaleMessage GetSystemLocaleMessage(String messageCode, CallerContext callerContext)
        {
            LocaleMessageBL localeMessageBL = new LocaleMessageBL();

            return localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, false, callerContext);
        }
        #endregion
    }
}
