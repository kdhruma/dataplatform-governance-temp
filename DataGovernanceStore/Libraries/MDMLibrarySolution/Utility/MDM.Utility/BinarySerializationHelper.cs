using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BinarySerializationHelper
    {
        #region Public Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializableObject"></param>
        /// <returns></returns>
        public static byte[] Serialize(object serializableObject)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, serializableObject);
            byte[] buffer = new byte[System.Convert.ToInt32(stream.Length)];
            stream.Position = 0;
            stream.Read(buffer, 0, System.Convert.ToInt32(stream.Length));
            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedObjectStream"></param>
        /// <returns></returns>
        public static object Deserialize(Stream serializedObjectStream)
        {
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(serializedObjectStream);
        }

        #endregion
    }
}
