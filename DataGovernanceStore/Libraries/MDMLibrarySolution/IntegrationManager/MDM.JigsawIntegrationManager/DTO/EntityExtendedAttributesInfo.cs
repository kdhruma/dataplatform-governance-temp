using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Represents class for JigsawAttributesInfo.
    /// </summary>
    internal class EntityExtendedAttributesInfo : IExtendedAttributesInfo
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the js workflow.
        /// </summary>
        public WorkflowInfo JsWorkflow { get; set; }

        /// <summary>
        /// Gets or sets the js business conditions.
        /// </summary>
        public BusinessConditionsSummary JsBusinessConditionsSummary { get; set; }

        /// <summary>
        /// Gets or sets the js business conditions.
        /// </summary>
        public ValidationStatesSummary JsValidationStatesSummary { get; set; }

        /// <summary>
        /// Gets or sets the jigsaw validation states.
        /// </summary>
        public Collection<ValidationState> JsValidationStates { get; set; }

        /// <summary>
        /// Gets or sets the js relationship.
        /// </summary>
        public Relationship JsRelationship { get; set; }

        /// <summary>
        /// Gets or sets the change context.
        /// </summary>
        public ChangeContext JsChangeContext { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="JigsawAttributesInfo" /> class.
        /// </summary>
        public EntityExtendedAttributesInfo()
        {
        }
    }
}