using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of vendors.
    /// </summary>
    public interface IVendorCollection : IEnumerable<Vendor>
    {
    }
}
