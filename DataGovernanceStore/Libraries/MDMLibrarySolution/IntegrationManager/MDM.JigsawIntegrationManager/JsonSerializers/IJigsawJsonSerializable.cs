using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager.JsonSerializers
{
    /// <summary>
    /// Represents interface for serializable Jigsaw objects
    /// </summary>
    public interface IJigsawJsonSerializable
    {
        /// <summary>
        /// Converts to json JToken object
        /// </summary>
        /// <returns></returns>
        JToken ToJToken();
    }
}