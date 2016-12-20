using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies entity cache invalidate context
    /// </summary>
    [DataContract]
    public class EntityCacheInvalidateContext : MDMObject, IEntityCacheInvalidateContext
    {
        #region Fields

        /// <summary>
        /// Field denoting the entity id.
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Field denoting the container id.
        /// </summary>
        private Int32 _containerId = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Specifies entity id for cache invalidation
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        /// Specifies container id for cache invalidation
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                this._containerId = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityCacheInvalidateContext() : base() { }

        /// <summary>
        /// Initializes EntityCacheInvalidateContext object with the values provided as XML
        /// </summary>
        /// <param name="valuesAsXml">XML string having values</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public EntityCacheInvalidateContext(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadEntityCacheInvalidateContext(valuesAsXml, objectSerialization);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents EntityCacheInvalidateContext in XML format
        /// </summary>
        /// <returns>String representation of current EntityCacheInvalidateContext object</returns>
        public override String ToXml()
        {
            String entityCacheInvalidateContextXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //EntityCacheInvalidateContext node start
            xmlWriter.WriteStartElement("EntityCacheInvalidateContext");

            #region Write EntityCacheInvalidateContext Properties

            xmlWriter.WriteAttributeString("EntityId", this._entityId.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this._containerId.ToString());
            xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

            #endregion

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();

            //Get the actual XML
            entityCacheInvalidateContextXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return entityCacheInvalidateContextXml;
        }

        /// <summary>
        /// Represents EntityCacheInvalidateContext in XML format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current EntityCacheInvalidateContext object</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String entityCacheInvalidateContextXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            entityCacheInvalidateContextXml = this.ToXml();

            return entityCacheInvalidateContextXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is EntityCacheInvalidateContext)
                {
                    EntityCacheInvalidateContext objectToBeCompared = obj as EntityCacheInvalidateContext;

                    if (!this._entityId.Equals(objectToBeCompared._entityId))
                        return false;

                    if (!this._containerId.Equals(objectToBeCompared._containerId))
                        return false;

                    if (!this.Locale.Equals(objectToBeCompared.Locale))
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() ^ this.EntityId.GetHashCode() ^ this.ContainerId.GetHashCode();

            return hashCode;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the entity cache invalidate context with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        private void LoadEntityCacheInvalidateContext(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityCacheInvalidateContext")
                        {
                            #region Read EntityCacheInvalidateContext Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("EntityId"))
                                {
                                    this._entityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityId);
                                }

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this._containerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                                }

                                if (reader.MoveToAttribute("Locale"))
                                {
                                    LocaleEnum locale = LocaleEnum.UnKnown;
                                    Enum.TryParse<LocaleEnum>(reader.ReadContentAsString(), true, out locale);
                                    this.Locale = locale;
                                }
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}