using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing entity family queue.
    /// </summary>
    public interface IEntityFamilyQueue : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates unique id of EaaH queue.
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Indicates entity family id for a variants tree
        /// </summary>
        Int64 EntityFamilyId { get; set; }

        /// <summary>
        /// Indicates entity global family id including extended families
        /// </summary>
        Int64 EntityGlobalFamilyId { get; set; }

        /// <summary>
        /// Indicates container id of an entity.
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Specifies family activity list of an entity.
        /// </summary>
        EntityActivityList EntityActivityList { get; set; }

        /// <summary>
        /// Indicates whether loading of entity family queue is in progress
        /// </summary>
        Boolean IsInProgress { get; set; }

        /// <summary>
        /// Indicates whether process is done for entity family queue or not.
        /// </summary>
        Boolean IsProcessed { get; set; }

        #endregion

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of entity family queue
        /// </summary>
        /// <returns>XML representation of entity family queue</returns>
        String ToXml();

        #endregion ToXml Methods

        #region Entity Family Change Contexts related methods

        /// <summary>
        /// Gets the entity family change contexts
        /// </summary>
        /// <returns>Returns entity family change contexts.</returns>
        IEntityFamilyChangeContextCollection GetEntityFamilyChangeContexts();

        /// <summary>
        /// Sets the entity family change contexts
        /// </summary>
        /// <param name="iEntityFamilyChangeContexts">Indicates the entity family change contexts to be set</param>
        void SetEntityFamilyChangeContexts(IEntityFamilyChangeContextCollection iEntityFamilyChangeContexts);

        #endregion

        #endregion
    }
}