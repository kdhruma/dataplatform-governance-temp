
using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityEventArgs
    {
        MDM.BusinessObjects.CallerContext CallerContext { get; }
        MDM.BusinessObjects.EntityCollection EntityInstances { get; }
        MDM.Interfaces.IEntityManager EntityManagerInstance { get; }
        MDM.BusinessObjects.EntityOperationResultCollection EntityOperationResults { get; }
        Int32 UserId { get; }
        MDM.Interfaces.IEntityProcessingOptions EntityProcessingOptions { get; }
    }
}
