using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing entity family change context related information.
    /// </summary>
    public interface IEntityFamilyChangeContext
    {
        #region Properties

        /// <summary>
        /// Specifies entity family id for entity family change context
        /// </summary>
        Int64 EntityFamilyId { get; set; }

        /// <summary>
        /// Specifies entity global family id for entity family change context
        /// </summary>
        Int64 EntityGlobalFamilyId { get; set; }

        /// <summary>
        /// Specifies organization id for entity family change context
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Specifies container id for entity family change context
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Specifies family activity list of an entity.
        /// </summary>
        EntityActivityList EntityActivityList { get; set; }

        #endregion Properties

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of entity family change context object
        /// </summary>
        /// <returns>XML representation of entity family change context object</returns>
        String ToXml();

        #endregion ToXml Methods

        #region Variants Change Context related methods

        /// <summary>
        /// Gets the variants change context of entity family
        /// </summary>
        /// <returns>Variants change context of entity family</returns>
        IVariantsChangeContext GetVariantsChangeContext();

        /// <summary>
        /// Sets the variants change context of entity family
        /// </summary>
        /// <param name="iVariantsChangeContext">Indicates the variants change context to be set</param>
        void SetVariantsChangeContext(IVariantsChangeContext iVariantsChangeContext);

        #endregion

        #region Extension Change Contexts related methods

        /// <summary>
        /// Gets the extension change contexts of entity family
        /// </summary>
        /// <returns>Gets the extension change contexts of entity family</returns>
        IExtensionChangeContextCollection GetExtensionChangeContexts();

        /// <summary>
        /// Sets the extension change context of entity family
        /// </summary>
        /// <param name="iExtensionChangeContexts">Indicates the extension change context to be set</param>
        void SetExtensionChangeContexts(IExtensionChangeContextCollection iExtensionChangeContexts);

        #endregion

        #region Workflow Change Context related methods

        /// <summary>
        /// Gets the workflow change contexts of an entity.
        /// </summary>
        /// <returns>Gets the workflow change contexts of an entity</returns>
        IWorkflowChangeContext GetWorkflowChangeContext();

        /// <summary>
        /// Sets the workflow change context of an entity
        /// </summary>
        /// <param name="iWorkflowChangeContext">Indicates the workflow change context to be set</param>
        void SetWorkflowChangeContext(IWorkflowChangeContext iWorkflowChangeContext);

        #endregion

        #endregion
    }
}