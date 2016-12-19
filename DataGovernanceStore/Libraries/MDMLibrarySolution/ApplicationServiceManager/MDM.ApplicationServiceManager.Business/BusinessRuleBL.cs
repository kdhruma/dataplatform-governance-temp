using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.ApplicationServiceManager.Business
{
    using MDM.Core;
    using MDM.ApplicationServiceManager.Data;

    /// <summary>
    /// Class having BL methods for Core BusinessRule logic
    /// </summary>
    public class BusinessRuleBL : BusinessLogicBase
    {
        #region Methods

        /// <summary>
        /// Get unique id based on configuration
        /// </summary>
        /// <param name="objectType">Object type for which we want Unique ID</param>
        /// <param name="organizationId">Org Id : Used for context</param>
        /// <param name="containerId">Container Id : Used for context</param>
        /// <param name="categoryId">Category Id : Used for context</param>
        /// <param name="entityTypeId">Entity type Id : Used for context</param>
        /// <param name="relationshipTypeId">Relationship type Id : Used for context</param>
        /// <param name="locale">Locale : Used for context</param>
        /// <param name="roleId">Role of current user Id : Used for context</param>
        /// <param name="userId">User Id : Used for context</param>
        /// <param name="noOfUIdsToGenerate">Indicates how many unique ids are to be generated</param>
        /// <returns>collection of string having auto ids generated from DB</returns>
        public Collection<String> GetUniqueId( ObjectType objectType, Int32 organizationId, Int32 containerId, Int64 categoryId, Int32 entityTypeId,
            Int32 relationshipTypeId, String locale, Int32 roleId, Int32 userId, Int32 noOfUIdsToGenerate )
        {
            Collection<String> returnValue = new Collection<string>();
            BusinessRuleDA businessRuleDA = new BusinessRuleDA();
            returnValue = businessRuleDA.GetUniqueId(objectType, organizationId, containerId, categoryId, entityTypeId, relationshipTypeId, locale, 
                roleId, userId, noOfUIdsToGenerate);
            return returnValue;
        }

        #endregion Methods
    }
}
