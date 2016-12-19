using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing extension change context related information.
    /// </summary>
    public interface IExtensionChangeContext
    {
        #region Properties

        /// <summary>
        /// Specifies organization id for extension change context.
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Specifies organization name for extension change context.
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Specifies container id for extension change context.
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Specifies container name for extension change context.
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Specifies container type for extension change context.
        /// </summary>
        ContainerType ContainerType { get; set; }

        /// <summary>
        /// Specifies container qualifier name for extension change context.
        /// </summary>
        String ContainerQualifierName { get; set; }

        #endregion Properties

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of attribute change context object
        /// </summary>
        /// <returns>XML representation of attribute change context object</returns>
        String ToXml();

        #endregion ToXml Methods

        #region Variants Change Context related methods

        /// <summary>
        /// Gets the variants change context of ExtensionChangeContext.
        /// </summary>
        /// <returns>Variants change context of the ExtensionChangeContext.</returns>
        IVariantsChangeContext GetVariantsChangeContext();

        /// <summary>
        /// Sets the variants change context of ExtensionChangeContext.
        /// </summary>
        /// <param name="iVariantsChangeContext">Indicates the variants change context to be set</param>
        void SetVariantsChangeContext(IVariantsChangeContext iVariantsChangeContext);

        #endregion

        #endregion Methods
    }
}