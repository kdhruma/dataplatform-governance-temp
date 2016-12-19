using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;

    /// <summary>
    /// Represents the cache status load context item for processing an cache status of an entity. This is used in activity log table to specify the cache reload request.
    /// </summary>
    public class EntityCacheLoadContextItem : MDMObject
    {
        #region Fields

        /// <summary>
        /// Specifies the object type based on which the cache status has to be processed.
        /// </summary>
        private EntityCacheLoadContextTypeEnum _type;

        /// <summary>
        /// Specifies whether to include all the id's for the object type or selective one's.
        /// </summary>
        private Boolean _includeAll;

        /// <summary>
        /// Specifies the list of id's for the object type based on which the entity cache status is processed.
        /// </summary>
        private Collection<Int64> _valueList = new Collection<Int64>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the EntityCacheLoadContextItem.
        /// </summary>
        public EntityCacheLoadContextItem()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the object type based on which the cache status has to be processed.
        /// </summary>
        public EntityCacheLoadContextTypeEnum Type
        {
            get { return this._type; }
            set { this._type = value; }
        }

        /// <summary>
        /// Specifies whether to include all the id's for the object type or selective one's.
        /// </summary>
        public Boolean IncludeAll
        {
            get { return this._includeAll; }
            set { this._includeAll = value; }
        }

        /// <summary>
        /// Specifies the list of id's for the object type based on which the entity cache status is processed.
        /// </summary>
        public Collection<Int64> ValueList
        {
            get { return this._valueList; }
            set { this._valueList = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the object id into the value list.
        /// </summary>
        /// <param name="objectId"></param>
        public void AddValues(Int32 objectId)
        {
            this._valueList.Add(objectId);
        }

        /// <summary>
        /// Adds the object id into the value list.
        /// </summary>
        /// <param name="objectId"></param>
        public void AddValues(Int64 objectId)
        {
            this._valueList.Add(objectId);
        }

        /// <summary>
        /// Adds the object id list into the value list.
        /// </summary>
        /// <param name="objectIdList"></param>
        public void AddValues(Collection<Int32> objectIdList)
        {
            foreach (Int32 objectId in objectIdList)
            {
                this._valueList.Add(objectId);
            }
        }

        /// <summary>
        /// Get XML representation of Entity Cache Load Context Item object
        /// </summary>
        /// <returns>XML representation of Entity Cache Load Context Item object</returns>
        public override String ToXml()
        {
            String entityCacheLoadContextItem = String.Empty;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("EntityCacheLoadContextItem");

                    xmlWriter.WriteAttributeString("Type", this.Type.ToString());
                    xmlWriter.WriteAttributeString("IncludeAll", this.IncludeAll.ToString().ToLowerInvariant());

                    xmlWriter.WriteStartElement("Values");

                    if (this.ValueList != null && this.ValueList.Count > 0)
                    {
                        foreach (Int64 value in this.ValueList)
                        {
                            xmlWriter.WriteStartElement("Value");
                            xmlWriter.WriteValue(value);
                            xmlWriter.WriteEndElement();
                        }
                    }

                    //Values node end
                    xmlWriter.WriteEndElement();

                    //EntityCacheLoadContextItem node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    entityCacheLoadContextItem = stringWriter.ToString();
                }
            }

            return entityCacheLoadContextItem;
        }

        #endregion
    }
}
