using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    public interface IEntityStateValidationManager
    {
        EntityStateValidationCollection Get(Collection<Int64> entityIds, CallerContext callerContext, Boolean needGlobalFamilyErrors = false, Boolean needVariantFamilyErrors = false);
    }
}
