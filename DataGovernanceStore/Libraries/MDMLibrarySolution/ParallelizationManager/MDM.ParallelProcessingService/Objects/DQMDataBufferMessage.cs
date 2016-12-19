using System;

namespace MDM.ParallelProcessingService.Objects
{
    using BusinessObjects;
    using BusinessObjects.DQM;

    public sealed class DQMDataBufferMessage<T> where T: class 
    {
        public DQMDataBufferMessage(Int64 entityQueueId, Entity entity, T resultData)
        {
            EntityQueueId = entityQueueId;
            ProcessedEntity = entity;
            ResultData = resultData;
        }

        public Int64 EntityQueueId { get; set; }
        public Entity ProcessedEntity { get; set; }
        public T ResultData { get; set; }
    }
}