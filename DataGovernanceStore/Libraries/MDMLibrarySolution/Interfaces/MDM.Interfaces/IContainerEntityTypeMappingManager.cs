using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Specifies an interface to exposes ContainerEntityTypeMappingBL methods
    /// </summary>
    public interface IContainerEntityTypeMappingManager
    {
        /// <summary>
        /// Gets Container EntityType mappings from the system based on containerId
        /// </summary>
        /// <param name="entityTypeId">Indicates the EntityType Id for which Container EntityType mappings needs to be fetched</param>
        /// <param name="callerContext">Context which called the application</param>
        /// <returns>Container EntityType mappings for a specified container Id</returns>
        ContainerEntityTypeMappingCollection GetMappingsByEntityTypeId(Int32 entityTypeId, CallerContext callerContext);
    }
}
