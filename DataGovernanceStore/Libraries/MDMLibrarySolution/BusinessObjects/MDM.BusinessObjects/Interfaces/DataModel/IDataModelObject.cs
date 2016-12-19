using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing data model object related information.
    /// </summary>
    public interface IDataModelObject : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        String ExternalId { get; set; }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        ObjectType DataModelObjectType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        IDataModelObject GetDataModelObject();

        #endregion
    }
}