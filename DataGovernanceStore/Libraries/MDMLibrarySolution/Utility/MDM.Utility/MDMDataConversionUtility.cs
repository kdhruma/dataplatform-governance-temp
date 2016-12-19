using System;
using System.IO;
using System.Xml;

namespace MDM.Utility
{
    using MDM.BusinessObjects;

    /// <summary>
    /// A utility class for data conversion
    /// </summary>
    public static class MDMDataConversionUtility
    {
        #region Public Methods

        /// <summary>
        /// Converts given entity object into xml based on passed entity conversion context object
        /// </summary>
        /// <param name="entity">Indicates an entity object to be converted into xml format</param>
        /// <param name="context">Indicates context object which specifies what all data of entity to be converted into Xml</param>
        /// <returns>Returns xml string representing an entity object</returns>
        public static String ConvertEntityToXML(Entity entity, EntityConversionContext context)
        {
            String entityXml = String.Empty;

            if (entity != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                    {
                        entity.ConvertEntityToXml(xmlWriter, context);
                    }

                    entityXml = sw.ToString();
                }
            }
            else
            {
                throw new ArgumentNullException("entity", "Entity to be converted is null.");
            }

            return entityXml;
        }

        /// <summary>
        /// Loads entity object from the xml
        /// </summary>
        /// <param name="valueAsXml">Indicates xml representation of entity object</param>
        /// <param name="context">Indicates context object which specifies what all data to be converted for entity from Xml</param>
        /// <returns>Returns Entity object</returns>
        public static Entity LoadEntityFromXML(String valueAsXml, EntityConversionContext context)
        {
            using (XmlTextReader reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null))
            {
                Entity entity = null;

                reader.Read();

                if (reader.HasAttributes)
                {
                    entity = new Entity();

                    entity.LoadEntityFromXml(reader, context);
                }

                return entity;
            }
        }

        #endregion Public Methods
    }
}
