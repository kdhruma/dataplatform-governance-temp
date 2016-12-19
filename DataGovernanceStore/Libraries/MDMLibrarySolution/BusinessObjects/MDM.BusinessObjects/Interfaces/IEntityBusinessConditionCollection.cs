using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    
    /// <summary>
    /// Exposes methods or properties to set or get entity business condition collection instance.
    /// </summary>
    public interface IEntityBusinessConditionCollection : IEnumerable<EntityBusinessCondition>
    {
        #region Properties

        /// <summary>
        /// Presents no. of entity business condition present into the collection
        /// </summary>
        Int32 Count { get; }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets Xml representation of the EntityBusinessConditionCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the EntityBusinessConditionCollection</returns>
        String ToXml();

        /// <summary>
        /// Gets business conditions mapped to the requested entity id.
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <returns>Returns IEntityBusinessCondiion</returns>
        IEntityBusinessCondition GetByEntityId(Int64 entityId);

        /// <summary>
        /// Gets business conditions mapped to the requested entity family id.
        /// </summary>
        /// <param name="entityFamilyId">Indicates entity family id</param>
        /// <returns>Returns IEntityBusinessCondiionCollection</returns>
        IEntityBusinessConditionCollection GetByEntityFamilyId(Int64 entityFamilyId);

        /// <summary>
        /// Gets business conditions mapped to the requested entity global family id.
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates entity global family id</param>
        /// <returns>Returns IEntityBusinessCondiionCollection</returns>
        IEntityBusinessConditionCollection GetByEntityGlobalFamilyId(Int64 entityGlobalFamilyId);
        
        #endregion Methods
    }
}
