using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get organization related information.
    /// </summary>
    public interface IOrganization : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the Type of an Organization
        /// </summary>
        Int32 OrganizationTypeId { get; set; }

        /// <summary>
        /// Property denoting the Classification of an Organization
        /// </summary>
        Int32 OrganizationClassification { get; set; }

        /// <summary>
        /// Property denoting the Parent of an Organization
        /// </summary>
        Int32 OrganizationParent { get; set; }

        /// <summary>
        /// Property denoting the GLN number of an Organization
        /// </summary>
        String GLN { get; set; }

        /// <summary>
        /// Property denoting ProcessorWeightage
        /// </summary>
        Int32 ProcessorWeightage { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clone Organization object.
        /// </summary>
        /// <returns>cloned Organization object.</returns>
        IOrganization Clone();

        /// <summary>
        /// Get Xml representation of Organization
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Gets the attributes belonging to the Organization
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        IAttributeCollection GetAttributes();

        /// <summary>
        /// Sets the attributes belonging to the Organization
        /// </summary>
        /// <param name="iAttributes">Collection of attributes to be set.</param>
        void SetAttributes(IAttributeCollection iAttributes);

        /// <summary>
        /// Gets attribute with specified attribute Id from current Organization's attributes
        /// </summary>
        /// <param name="attributeId">Id of an attribute to search in current Organization's attributes</param>
        /// <returns>Attribute interface</returns>
        /// <exception cref="ArgumentException">Attribute Id must be greater than 0</exception>
        /// <exception cref="NullReferenceException">Attributes for Organization is null. There are no attributes to search in</exception>
        IAttribute GetAttribute(Int32 attributeId);

        /// <summary>
        /// Delta Merge of organization
        /// </summary>
        /// <param name="deltaOrganization">Organization that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <returns>Merged organization instance</returns>
        IOrganization MergeDelta(IOrganization deltaOrganization, ICallerContext iCallerContext);

        #endregion
    }
}