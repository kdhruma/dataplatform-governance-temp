
namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    public interface IEntityFamilyQueueManager
    {
        OperationResult Process(EntityFamilyQueue entityFamilyQueue, CallerContext callerContext);
    }
}
