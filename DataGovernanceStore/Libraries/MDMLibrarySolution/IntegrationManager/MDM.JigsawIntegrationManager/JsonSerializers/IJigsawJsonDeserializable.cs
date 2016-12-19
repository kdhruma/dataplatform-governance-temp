using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager.JsonSerializers
{
    /// <summary>
    /// Represents interface for deserializable Jigsaw objects
    /// </summary>
    public interface IJigsawJsonDeserializable
    {
        /// <summary>
        /// Loads from JToken
        /// </summary>
        /// <param name="token">The JToken.</param>
        void LoadFromJToken(JToken token);
    }
}