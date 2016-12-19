using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.AttributeManager.Business
{
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.BusinessObjects;
    using MDM.Utility;
    using MDM.AttributeManager.Data;
    using MDM.ConfigurationManager.Business;
    using System.Diagnostics;

    /// <summary>
    /// Represent BL Methods for AttributeVersion
    /// </summary>
    public class AttributeVersionBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get Version History of Attribute
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="attributeId">Specifies Id of Attribute</param>
        /// <param name="localeId">Specifies Id of Locale</param>
        /// <param name="catalogId">Specifies Id of Catalog</param>
        /// <param name="entityParentId">Specifies Id of Parent EntityId</param>
        /// <param name="application">Specifies Which Application is Calling this(PIM,VendorPortal,MDMCenter)</param>
        /// <param name="module">Specifies from Which Module it is called(Entity,JobServies....)</param>
        /// <returns>Versions of Attribute</returns>
        /// <exception cref="ArgumentException">Thrown if Entity Id is Less than 0</exception>
        /// <exception cref="ArgumentException">Thrown if AttributeID is Less than 0</exception> 
        /// <exception cref="ArgumentException">Thrown if LocaleID is Less than 0</exception> 
        public AttributeVersionCollection Get(Int64 entityId, Int32 attributeId, Int32 localeId, Int32 catalogId, Int64 entityParentId, CallerContext callerContext)
        {
            #region Parameter Validation

            if (entityId <= 0)
            {
                throw new ArgumentException("Entity Id Must be greater than 0");
            }

            if (attributeId <= 0)
            {
                throw new ArgumentException("Attribute Id must be greater than 0");
            }

            if (localeId <= 0)
            {
                throw new ArgumentException("LocalId must be greater that 0");
            }

            #endregion

            AttributeVersionDA attributeVersionDA = new AttributeVersionDA();

            DBCommandProperties command = DBCommandHelper.Get(callerContext, MDMCenterModuleAction.Read);

            return attributeVersionDA.Get(entityId, attributeId, MDM.Utility.GlobalizationHelper.GetSystemDataLocale(), localeId, catalogId, entityParentId, command);
        }

        /// <summary>
        /// Get Version History of Attribute
        /// </summary>
        /// <param name="entityId">Specifies Id of Entity</param>
        /// <param name="entityParentId">Specifies ParentId of entity</param>
        /// <param name="attributeId">Specifies Id of Attribute</param>
        /// <param name="localeId">Specifies Id of Locale</param>
        /// <param name="catalogId">Specifies Id of Catalog</param>
        /// <param name="locales">Specifies the data Locales</param>
        /// <param name="sequence">Specifies a sequence</param>
        /// <param name="Calllercontext">Specifies Caller context</param>
        /// <returns>AttributeVersionCollection</returns>
        public AttributeVersionCollection GetComplexAttributeVersions(Int64 entityId, Int64 entityParentId, Int32 attributeId, Int32 catalogId, Collection<LocaleEnum> locales, Int32 sequence, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("AttributeManager.GetComplexAttributeVersions", false);
            AttributeVersionCollection attributeVersionCollection = null;

            #region Parameter Validation

            if (entityId <= 0)
            {
                throw new MDMOperationException("111645", "Entity Id Must be greater than 0.", "AttributeManager", String.Empty, "GetComplexAttributeVersions");
            }

            if (attributeId <= 0)
            {
                throw new MDMOperationException("111646", "Attribute Id must be greater than 0", "AttributeManager", String.Empty, "GetComplexAttributeVersions");
            }

            #endregion

            AttributeVersionDA attributeVersionDA = new AttributeVersionDA();

            DBCommandProperties command = DBCommandHelper.Get(callerContext.Application, callerContext.Module, MDMCenterModuleAction.Read);

            attributeVersionCollection = attributeVersionDA.GetComplexAttributeVersions(entityId, entityParentId, attributeId, catalogId, locales, sequence, command);

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("AttributeManager.GetComplexAttributeVersions");

            return attributeVersionCollection;
        }

        #endregion
    }
}
