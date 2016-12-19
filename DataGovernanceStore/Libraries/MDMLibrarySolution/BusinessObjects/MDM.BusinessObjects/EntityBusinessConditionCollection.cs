using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    
    /// <summary>
    /// Specifies entity business condition collection
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityBusinessConditionCollection : InterfaceContractCollection<IEntityBusinessCondition, EntityBusinessCondition>, IEntityBusinessConditionCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityBusinessConditionCollection()
        { 
        
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">Indicates XMl representation of entity business condition collection</param>
        public EntityBusinessConditionCollection(String valuesAsXml)
        {
            LoadEntityBusinessConditionCollection(valuesAsXml);
        }

        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets Xml representation of the EntityBusinessConditionCollection object
        /// </summary>
        /// <returns>Returns Xml string representing the EntityBusinessConditionCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region Write EntityBusinessConditionCollection

                    //EntityBusinessConditionCollection node start
                    xmlWriter.WriteStartElement("EntityBusinessConditions");

                    if (this._items != null)
                    {
                        foreach (EntityBusinessCondition entityBusinessCondition in this._items)
                        {
                            xmlWriter.WriteRaw(entityBusinessCondition.ToXml());
                        }
                    }

                    //EntityBusinessConditionCollection node end
                    xmlWriter.WriteEndElement();

                    #endregion Write EntityBusinessConditionCollection

                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gets business conditions mapped to the requested entity id.
        /// </summary>
        /// <param name="entityId">Indicates entity id</param>
        /// <returns>Returns EntityBusinessCondiion</returns>
        public IEntityBusinessCondition GetByEntityId(Int64 entityId)
        {
            EntityBusinessCondition entityBusinessCondition = null;
            
            foreach(EntityBusinessCondition item in this._items)
            {
                if(item.EntityId == entityId)
                {
                    entityBusinessCondition = item;
                    break;
                }
            }

            return (IEntityBusinessCondition) entityBusinessCondition;
        }

        /// <summary>
        /// Gets business conditions mapped to the requested entity family id.
        /// </summary>
        /// <param name="entityFamilyId">Indicates entity family id</param>
        /// <returns>Returns EntityBusinessCondiionCollection</returns>
        public IEntityBusinessConditionCollection GetByEntityFamilyId(Int64 entityFamilyId)
        {
            EntityBusinessConditionCollection entityBusinessConditions = new EntityBusinessConditionCollection();

            foreach (EntityBusinessCondition item in this._items)
            {
                if (item.EntityFamilyId == entityFamilyId)
                {
                    entityBusinessConditions.Add(item);
                }
            }

            return (IEntityBusinessConditionCollection)entityBusinessConditions;
        }

        /// <summary>
        /// Gets business conditions mapped to the requested entity global family id.
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates entity global family id</param>
        /// <returns>Returns EntityBusinessCondiionCollection</returns>
        public IEntityBusinessConditionCollection GetByEntityGlobalFamilyId(Int64 entityGlobalFamilyId)
        {
            EntityBusinessConditionCollection entityBusinessConditions = new EntityBusinessConditionCollection();

            foreach (EntityBusinessCondition item in this._items)
            {
                if (item.EntityGlobalFamilyId == entityGlobalFamilyId)
                {
                    entityBusinessConditions.Add(item);
                }
            }

            return (IEntityBusinessConditionCollection)entityBusinessConditions;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityBusinessConditionCollection(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityBusinessConditions")
                        {
                            String entityBusinessConditionXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(entityBusinessConditionXml))
                            {
                                EntityBusinessCondition entityBusinessCondition = new EntityBusinessCondition(entityBusinessConditionXml);
                                this.Add(entityBusinessCondition);
                            }
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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
