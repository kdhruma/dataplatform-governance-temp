using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;
using System.Transactions;
using System.Diagnostics;

using MDM.BusinessObjects;
using MDM.Core;
using MDM.AdminManager.Data;
using MDM.Utility;
using MDM.Core.Exceptions;

namespace MDM.AdminManager.Business
{
    public class AuditInfoBL
    {
        #region Fields

        #endregion

        #region Constructors

        public AuditInfoBL()
        {
            
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Creates a Audit ref record in the database and returns the primary key.
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="password"></param>
        /// <returns>The primary key of the newly created audit record.</returns>
        public Int64 Create(String userLoginName, String programName, bool isCreateOperation)
        {
            if (userLoginName == null)
            {
                throw new MDMOperationException("111711", "Failed to set AuditInfo. userLoginName is not available.", "AdminManager", String.Empty, "Set");
            }

            if (programName == null)
            {
                programName = "AdminManager.AuditInfo.Create";
            }

            AuditInfoDA auditInfoDA = new AuditInfoDA();
            return auditInfoDA.Create(userLoginName, programName, isCreateOperation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditInfoId"></param>
        /// <returns></returns>
        public AuditInfoCollection Get(Collection<Int64> auditInfoId)
        {
            if (auditInfoId == null && auditInfoId.Count == 0)
            {
                throw new MDMOperationException("111709", "Failed to get AuditInfo. AuditInfoId is not available.", "AdminManager", String.Empty, "Get");
            }

            AuditInfoDA auditInfoDA = new AuditInfoDA();
            return auditInfoDA.Get(auditInfoId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="attributeIds"></param>
        /// <param name="locale"></param>
        /// <param name="sequence"></param>
        /// <param name="returnEntityAudit"></param>
        /// <param name="returnAttirbuteAudit"></param>
        /// <param name="returnOnlyLatestAudit"></param>
        /// <returns></returns>
        public EntityAuditInfoCollection Get(Int64 entityId, Collection<Int32> attributeIds, LocaleEnum locale, Decimal sequence, Boolean returnEntityAudit, Boolean returnAttirbuteAudit, Boolean returnOnlyLatestAudit)
        {
            #region Parameter Validation

            if (returnAttirbuteAudit == true && attributeIds == null && attributeIds.Count < 0)
            {
                throw new MDMOperationException("111710", "Failed to get AuditInfo. AttributeId is not available.", "AdminManager", String.Empty, "Get");
            }

            #endregion

            AuditInfoDA auditInfoDA = new AuditInfoDA();
            Collection<Int64> entityIdList = new Collection<Int64>() { entityId };

            return auditInfoDA.Get(entityIdList, attributeIds, locale, sequence, returnEntityAudit, returnAttirbuteAudit, returnOnlyLatestAudit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="attributeIds"></param>
        /// <param name="locale"></param>
        /// <param name="sequence"></param>
        /// <param name="returnEntityAudit"></param>
        /// <param name="returnAttirbuteAudit"></param>
        /// <param name="returnOnlyLatestAudit"></param>
        /// <returns></returns>
        public EntityAuditInfoCollection Get(Collection<Int64> entityIds, Collection<Int32> attributeIds, LocaleEnum locale, Decimal sequence, Boolean returnEntityAudit, Boolean returnAttirbuteAudit, Boolean returnOnlyLatestAudit)
        {
            #region Parameter Validation

            if (returnAttirbuteAudit == true && attributeIds == null && attributeIds.Count < 0)
            {
                throw new MDMOperationException("111710", "Failed to get AuditInfo. AttributeId is not available.", "AdminManager", String.Empty, "Get");
            }

            #endregion

            AuditInfoDA auditInfoDA = new AuditInfoDA();

            return auditInfoDA.Get(entityIds, attributeIds, locale, sequence, returnEntityAudit, returnAttirbuteAudit, returnOnlyLatestAudit);
        }

        #endregion
    }
}
