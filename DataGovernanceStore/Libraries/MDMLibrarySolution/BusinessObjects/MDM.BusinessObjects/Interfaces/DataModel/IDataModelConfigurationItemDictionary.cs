using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core.DataModel;

    /// <summary>
    /// Exposes methods for working with Configuration Item Dictionary
    /// </summary>
    public interface IDataModelConfigurationItemDictionary : IDictionary<DataModelConfigurationItem, String>
    {
    }
}