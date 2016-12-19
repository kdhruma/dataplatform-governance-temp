using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MDM.Core.Extensions
{
    /// <summary>
    /// Extension for Object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        /// <param name="obj">Object to Serialize</param>
        /// <typeparam name="T">Type of the Object</typeparam>
        /// <returns>Serialized Object</returns>
        public static string SerializeToJson<T>(this T obj) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// JSON Deserialization
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="json">Serialized json Object String</param>
        /// <returns>Deserialized Object of given Type</returns>
        public static T DeserializeFromJson<T>(this string json) where T : class
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof (T));
                return serializer.ReadObject(stream) as T;
            }
        }
    }
}
