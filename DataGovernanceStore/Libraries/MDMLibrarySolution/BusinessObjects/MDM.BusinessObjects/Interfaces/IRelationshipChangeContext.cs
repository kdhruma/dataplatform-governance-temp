using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing relationship change context related information.
    /// </summary>
    public interface IRelationshipChangeContext
    {
        #region Properties

        /// <summary>
        /// Specifies id of relationship.
        /// </summary>
        Int64 RelationshipId { get; set; }

        /// <summary>
        /// Specifies the from entity id of relationship.
        /// </summary>
        Int64 FromEntityId { get; set; }

        /// <summary>
        /// Specifies the related entity id of relationship.
        /// </summary>
        Int64 RelatedEntityId { get; set; }

        /// <summary>
        /// Specifies the relationship type id of relationship.
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Specifies the relationship type name of relationship.
        /// </summary>
        String RelationshipTypeName { get; set; }

        /// <summary>
        /// Specifies the action for based on attribute change context.
        /// </summary>
        ObjectAction Action { get; set; }

        #endregion Properties

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Gets XML representation of relationship change context object
        /// </summary>
        /// <returns>XML representation of relationship change context object</returns>
        String ToXml();

        #endregion ToXml Methods

        #region Attribute Change Contexts related methods

        /// <summary>
        /// Gets the attribute change contexts per locale
        /// </summary>
        /// <returns>Attribute change context for a locale .</returns>
        IAttributeChangeContextCollection GetAttributeChangeContexts();

        /// <summary>
        /// Sets the attribute change context per locale.
        /// </summary>
        /// <param name="iAttributeChangeContexts">Indicates the attribute change contexts to be set</param>
        void SetVariantsChangeContexts(IAttributeChangeContextCollection iAttributeChangeContexts);

        #endregion

        #endregion
    }
}