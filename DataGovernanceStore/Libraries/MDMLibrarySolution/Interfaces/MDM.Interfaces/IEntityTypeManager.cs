using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;


    public interface IEntityTypeManager
    {

        /// <summary>
        /// Process entityTypes
        /// </summary>
        /// <param name="entityTypes">EntityTypes to process</param>
        /// <param name="callerContext">Context of application making call for this API</param>
        /// <returns>OperationResult of EntityType process</returns>
        OperationResultCollection Process(EntityTypeCollection entityTypes, CallerContext callerContext);

        /// <summary>
        /// Get all entity types in the system
        /// </summary>
        /// <param name="callerContext">Indicates name of application and module.</param>
        /// <param name="getLatest">Boolean flag which says whether to get from DB or cache. True means always get from DB</param>
        /// <returns>All entity types</returns>
        EntityTypeCollection GetAll(CallerContext callerContext, Boolean getLatest = false);

        /// <summary>
        /// Get entity type by id
        /// </summary>
        /// <param name="id">Id using which Entity type is to be fetched</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns>EntityType with Id specified. Otherwise null</returns>
        EntityType GetById(Int32 id, CallerContext callerContext);

        /// <summary>
        /// Get entity type collection based on its unique short name 
        /// </summary>
        /// <param name="entityTypeShortNames">Unique identifier which identifies entity type uniquly in system</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        EntityTypeCollection GetByNames(Collection<String> entityTypeShortNames, CallerContext callerContext);

        /// <summary>
        /// Get entity type based on its name
        /// </summary>
        /// <param name="entityTypeShortName">Unique identifier which identifies entity type uniquly in system</param>
        /// <param name="callerContext">caller context indicating who called the API</param>
        /// <returns></returns>
        EntityType GetByShortName(String entityTypeShortName, CallerContext callerContext);

        /// <summary>
        /// Get all entity types by list of ids
        /// </summary>
        /// <param name="entityTypeIds">Collection of EntityType Ids to search in the system</param>
        /// <returns>Collection of EntityTypes with specified Ids in the Id list</returns>
        EntityTypeCollection GetEntityTypesByIds(Collection<Int32> entityTypeIds);
    }
}
