using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the impacted entity collection related information.
    /// </summary>
    public interface IImpactedEntityCollection : IEnumerable<ImpactedEntity>
    {
    }
}
