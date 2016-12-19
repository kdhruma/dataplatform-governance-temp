using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of entity family change context of entire entity family.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityFamilyChangeContextCollection : InterfaceContractCollection<IEntityFamilyChangeContext, EntityFamilyChangeContext>, IEntityFamilyChangeContextCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the entity family change context Collection
        /// </summary>
        public EntityFamilyChangeContextCollection()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityFamilyChangeContextCollection(String valuesAsXml)
        {
            LoadEntityFamilyChangeContexts(valuesAsXml);
        }

        /// <summary>
        /// Converts list into current instance
        /// </summary>
        /// <param name="entityFamilyChangeContextList">List of entity family change contexts</param>
        public EntityFamilyChangeContextCollection(IList<EntityFamilyChangeContext> entityFamilyChangeContextList)
        {
            if (entityFamilyChangeContextList != null)
            {
                this._items = new Collection<EntityFamilyChangeContext>(entityFamilyChangeContextList);
            }
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EntityFamilyChangeContext Collection
        /// </summary>
        /// <returns>Xml representation of EntityFamilyChangeContext Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //EntityFamilyChangeContextCollection node start
                    xmlWriter.WriteStartElement("EntityFamilyChangeContexts");

                    #region Write EntityFamilyChangeContextCollection

                    if (_items != null)
                    {
                        foreach (EntityFamilyChangeContext entityFamilyChangeContext in this._items)
                        {
                            xmlWriter.WriteRaw(entityFamilyChangeContext.ToXml());
                        }
                    }

                    #endregion

                    //EntityFamilyChangeContextCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gets EntityFamilyChangeContext based on entity family id(container specific family id)
        /// </summary>
        /// <param name="entityFamilyId">Indicates entity family id for which entity family change context to be retrieved</param>
        /// <returns>EntityFamilyChangeContext based on given entity family id</returns>
        public EntityFamilyChangeContext GetByEntityFamilyId(Int64 entityFamilyId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityFamilyChangeContext entityFamilyChangeContext in this._items)
                {
                    if (entityFamilyChangeContext.EntityFamilyId == entityFamilyId)
                    {
                        return entityFamilyChangeContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets EntityFamilyChangeContext based on entity global family id
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates entity global family id for which entity family change context to be retrieved</param>
        /// <returns>EntityFamilyChangeContext based on given entity family id</returns>
        public EntityFamilyChangeContext GetByEntityGlobalFamilyId(Int64 entityGlobalFamilyId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityFamilyChangeContext entityFamilyChangeContext in this._items)
                {
                    if (entityFamilyChangeContext.EntityGlobalFamilyId == entityGlobalFamilyId)
                    {
                        return entityFamilyChangeContext;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Delta Merge of family change contexts
        /// </summary>
        /// <param name="deltaEntityFamilyChangeContexts">Indicates delta family change contexts needs to be merged</param>
        public void Merge(EntityFamilyChangeContextCollection deltaEntityFamilyChangeContexts)
        {
            //Merge only if we have anything from delta.
            if (deltaEntityFamilyChangeContexts != null && deltaEntityFamilyChangeContexts.Count > 0)
            {
                //We don't have any into current collection. Take all data from delta and update to current collection.
                if (this.Count < 1)
                {
                    this._items = deltaEntityFamilyChangeContexts._items;
                }
                else
                {
                    foreach (EntityFamilyChangeContext deltaEntityFamilyChangeContext in deltaEntityFamilyChangeContexts)
                    {
                        Int64 entityGlobalFamilyId = deltaEntityFamilyChangeContext.EntityGlobalFamilyId;

                        EntityFamilyChangeContext originalEntityFamilyChangeContext = this.GetByEntityGlobalFamilyId(entityGlobalFamilyId);

                        //No original entity family change context found over here.. Add directly to current collection
                        if (originalEntityFamilyChangeContext == null)
                        {
                            this._items.Add(deltaEntityFamilyChangeContext);
                        }
                        else
                        {
                            originalEntityFamilyChangeContext.Merge(deltaEntityFamilyChangeContext);
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityFamilyChangeContexts(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityFamilyChangeContext")
                        {
                            String entityFamilyChangeContextXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(entityFamilyChangeContextXml))
                            {
                                EntityFamilyChangeContext entityFamilyChangeContext = new EntityFamilyChangeContext(entityFamilyChangeContextXml);

                                if (entityFamilyChangeContext != null)
                                {
                                    this.Add(entityFamilyChangeContext);
                                }
                            }
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

        #endregion Private Methods

        #endregion Methods
    }
}