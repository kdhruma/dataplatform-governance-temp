using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Specifies the interface providing Impacted Entity Manager.
    /// </summary>
    public interface IImpactedEntityManager
    {
        ImpactedEntity GetById(Int64 entityId, CallerContext callerContext);

        ImpactedEntityCollection GetByIdList(Collection<Int64> entityIdList, CallerContext callerContext);
    }
}
