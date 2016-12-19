using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    public interface IHierarchyManager
    {
        /// <summary>
        /// Gets hierarchy based on unique short name 
        /// </summary>
        /// <param name="hierarchyShortName">unique short name for hierarchy</param>
        /// <param name="callerContext">caller context to denote application and module which has called an API</param>
        /// <returns>Returns hierarchy based on name</returns>
        Hierarchy GetByName(String hierarchyShortName, CallerContext callerContext);

        /// <summary>
        /// Get hierarchy by id
        /// </summary>
        /// <param name="id">Id using which hierarchy is to be fetched</param>
        /// <param name="callerContext">Context of the caller</param>
        /// <param name="applySecurity"></param>
        /// <returns>hierarchy with Id specified. Otherwise null</returns>
        Hierarchy GetById(Int32 id, CallerContext callerContext, Boolean applySecurity = true);

        /// <summary>
        /// Get all hierarchies in the system
        /// </summary>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <param name="applySecurity"></param>
        /// <returns>All hierarchies</returns>
        HierarchyCollection GetAll(CallerContext callerContext, Boolean applySecurity = true);

    }
}
