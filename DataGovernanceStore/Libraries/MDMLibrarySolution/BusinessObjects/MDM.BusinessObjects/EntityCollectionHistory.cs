using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies entity level history for multiple entities
    /// </summary>
    [DataContract]
    public class EntityCollectionHistory : InterfaceContractCollection<IEntityHistory, EntityHistory>, IEntityCollectionHistory
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityCollectionHistory()
            : base()
        { }

         /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityCollectionHistory(String valuesAsXml)
        {
            LoadEntityCollectionHistory(valuesAsXml);
        }

        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EntityCollectionHistory object
        /// </summary>
        /// <returns>Xml string representing the EntityCollectionHistory</returns>
        public String ToXml()
        {
            String entityCollectionHistoryXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (EntityHistory entityHistory in this._items)
            {
                builder.Append(entityHistory.ToXml());
            }

            entityCollectionHistoryXml = String.Format("<EntityCollectionHistory>{0}</EntityCollectionHistory>", builder.ToString());
            return entityCollectionHistoryXml;
        }

        /// <summary>
        /// Clone entity collection history object.
        /// </summary>
        /// <returns>cloned entity collection history object.</returns>
        public IEntityCollectionHistory Clone()
        {
            EntityCollectionHistory entityCollectionHistory = new EntityCollectionHistory();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityHistory entityHistory in this._items)
                {
                    EntityHistory clonedEntityHistory = (EntityHistory) entityHistory.Clone();
                    entityCollectionHistory.Add(clonedEntityHistory);
                }
            }
            return entityCollectionHistory;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">EntityHistory Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(EntityCollectionHistory objectToBeCompared)
        {
            if (this._items.Count == objectToBeCompared._items.Count)
            {
                for (Int32 i = 0; i < this._items.Count; i++)
                {
                    if (!this._items[i].Equals(objectToBeCompared._items[i]))
                        return false;
                }
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">EntityHistory Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(EntityCollectionHistory objectToBeCompared, Boolean compareIds = false)
        {
            if (this._items.Count == objectToBeCompared._items.Count)
            {
                for (Int32 i = 0; i < this._items.Count; i++)
                {
                    if (!this._items[i].Equals(objectToBeCompared._items[i], compareIds))
                        return false;
                }
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// Gets entity history based on entity id from 
        /// </summary>
        /// <param name="entityId">id of an entity for which history is needed </param>
        /// <returns>IEntityHistory</returns>
        public IEntityHistory GetEntityHistory(Int64 entityId)
        {
            IEntityHistory entityHistory = null;

            if (_items != null)
            {
                foreach (EntityHistory itemEntityHistory in this._items)
                {
                    if (itemEntityHistory.EntityId == entityId)
                    {
                        entityHistory = itemEntityHistory;
                        break;
                    }
                }
            }

            return entityHistory;
        }

        /// <summary>
        /// Add EntityHistory in collection
        /// </summary>
        /// <param name="entityCollectionHistory">Entities to add in collection</param>
        public void AddRange(EntityCollectionHistory entityCollectionHistory)
        {
            if (entityCollectionHistory == null)
            {
                throw new ArgumentNullException("entityCollectionHistory");
            }

            foreach (EntityHistory entityHistory in entityCollectionHistory)
            {
                this.Add(entityHistory);
            }
        }

        #endregion

        #region Private Methods

        private void LoadEntityCollectionHistory(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistory")
                        {
                            #region Read EntityHistory Properties

                            String entityHistoryXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(entityHistoryXml))
                            {
                                EntityHistory entityHistory = new EntityHistory(entityHistoryXml);
                                if (entityHistory != null)
                                {
                                    this.Add(entityHistory);
                                }
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
