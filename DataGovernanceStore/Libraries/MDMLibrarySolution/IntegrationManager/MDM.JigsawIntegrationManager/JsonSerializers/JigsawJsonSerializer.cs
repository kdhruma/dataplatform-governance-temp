using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MDM.JigsawIntegrationManager.JsonSerializers
{
    /// <summary>
    /// Class JigsawJsonSerializer.
    /// </summary>
    public class JigsawJsonSerializer
    {
        private static JsonSerializer _jsonSerializer = null;

        /// <summary>
        /// Gets the json serializer.
        /// </summary>
        public static JsonSerializer JsonSerializer
        {
            get
            {
                if (_jsonSerializer == null)
                {
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };

                    jsonSerializerSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});

                    _jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
                }

                return _jsonSerializer;
            }
        }
    }
}