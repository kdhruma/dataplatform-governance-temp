using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    public interface IRelationshipTypeManager
    {
        /// <summary>
        /// Process relationshipTypes
        /// </summary>
        /// <param name="relationshipTypes">RelationshipTypes to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of RelationshipType Creation</returns>
        OperationResultCollection Process(RelationshipTypeCollection relationshipTypes, CallerContext callerContext);

        /// <summary>
        /// Get relationshipType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RelationshipType GetById(Int32 id, CallerContext callerContext);

        /// <summary>
        /// Get relationship based on container and entity type Id.
        /// </summary>
        /// <param name="containerId">This parameter specifying catalog id.</param>
        /// <param name="entityTypeId">This parameter specifying entity type id.</param>
        /// <returns>collection of relationship type</returns>
        /// <exception cref="ArgumentNullException">Raised when container id is null</exception>
        Tuple<RelationshipTypeCollection, Int32> Get(Int32 containerId, Int32 entityTypeId, CallerContext callerContext);

        /// <summary>
        /// Get relationship based on container id and nodeType Id.
        /// </summary>
        /// <param name="containerId">Specifies container id.</param>
        /// <param name="entityTypeId">Specifies entity type id.</param>
        /// <param name="callerContext">Name of application and module which is performing action</param>
        /// <returns>collection of relationship types</returns>
        RelationshipTypeCollection GetRelationshipTypes(Int32 containerId, Int32 entityTypeId, CallerContext callerContext);

        /// <summary>
        /// Get all relationshipTypes
        /// </summary>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>All Relationship types</returns>
        RelationshipTypeCollection GetAll(CallerContext callerContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationshipTypeName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        RelationshipType GetByName(String relationshipTypeName, CallerContext callerContext);
    }
}
