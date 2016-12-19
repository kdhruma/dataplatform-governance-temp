using System;

namespace MDM.JigsawIntegrationManager.DataPackages
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Core.Extensions;
    using MDM.BusinessObjects.Workflow;
    
    /// <summary>
    /// Represents class for EntityMessageDataPackage
    /// </summary>
    public class EntityMessageDataPackage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Entity
        /// </summary>
        public Entity Entity{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityStateValidationCollection StateValidations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityStateValidationScoreCollection StateValidationScores { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityBusinessCondition BusinessCondition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkflowStateCollection WorkflowStates{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MDMRuleMapDetailCollection RuleMaps { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntityMessageDataPackage()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public EntityMessageDataPackage(Entity entity) : base()
        {
            Entity = entity;
        }

        #endregion
    }
}