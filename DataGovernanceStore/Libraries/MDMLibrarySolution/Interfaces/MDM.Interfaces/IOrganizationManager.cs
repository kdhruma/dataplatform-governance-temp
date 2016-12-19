using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    public interface IOrganizationManager
    {
        /// <summary>
        /// Create - Update or Delete given organizations
        /// </summary>
        /// <param name="organizations">Collection of organizations to process</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Result of the operation</returns>
        OperationResultCollection Process(OrganizationCollection organizations, CallerContext callerContext);

        /// <summary>
        /// Get All Organizations from the system
        /// </summary>
        /// <param name="organizationContext">Indicates context containing flag to load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Collection of Organizations</returns>
        OrganizationCollection GetAll(OrganizationContext organizationContext, CallerContext callerContext);

        /// <summary>
        /// Gets organization by Id
        /// </summary>
        /// <param name="organizationId">Indicates organizationId</param>
        /// <param name="organizationContext">Indicates context containing flag to load attributes or not</param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Organization</returns>
        Organization GetById(Int32 organizationId, OrganizationContext organizationContext, CallerContext callerContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationShortName"></param>
        /// <param name="organizationContext"></param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        Organization GetByName(String organizationShortName, OrganizationContext organizationContext, CallerContext callerContext);

        /// <summary>
        /// Get All Organizations childs
        /// </summary>
        /// <param name="parentOrganizationId"></param>
        /// <param name="callerContext">context indicates the properties of the caller who called the API like program name,Module</param>
        /// <returns>Collection of Organizations</returns>
        MDMObjectInfoCollection GetAllOrganizationDependencies(Int32 parentOrganizationId, CallerContext callerContext);
    }
}
