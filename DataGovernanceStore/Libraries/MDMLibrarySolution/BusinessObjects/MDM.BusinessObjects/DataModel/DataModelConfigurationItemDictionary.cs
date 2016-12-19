using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.DataModel
{
    using MDM.Core.DataModel;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for dictionary of Data Model Configuration Items
    /// </summary>
    public class DataModelConfigurationItemDictionary : Dictionary<DataModelConfigurationItem, String>, IDataModelConfigurationItemDictionary
    {
    }
}
