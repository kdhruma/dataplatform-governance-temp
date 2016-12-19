using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies entity family queue collection.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class EntityFamilyQueueCollection : InterfaceContractCollection<IEntityFamilyQueue, EntityFamilyQueue>, IEntityFamilyQueueCollection
    {
        #region Fields
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EntityFamilyQueueCollection()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">Indicates XML having xml value</param>
        public EntityFamilyQueueCollection(String valuesAsxml)
        {
            LoadEntityFamilyQueueCollection(valuesAsxml);
        }

        #endregion Constructor

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of EntityFamilyQueueCollection
        /// </summary>
        /// <returns>Returns Xml representation of EntityFamilyQueueCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //EntityFamilyQueueCollection node start
                    xmlWriter.WriteStartElement("EntityFamilyQueues");

                    #region Write EntityFamilyQueueCollection

                    if (_items != null)
                    {
                        foreach (EntityFamilyQueue entityFamilyQueue in this._items)
                        {
                            xmlWriter.WriteRaw(entityFamilyQueue.ToXml());
                        }
                    }

                    #endregion

                    //EntityFamilyQueueCollection node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //Get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Compares entity family queue collection with current collection object.
        /// </summary>
        /// <param name="subSetEntityFamilyQueues">Indicates entity family queue collection to be compared with current object.</param>
        /// <returns>Returns true if comparison is succeeded otherwise, returns false.</returns>
        public Boolean IsSuperSetOf(EntityFamilyQueueCollection subSetEntityFamilyQueues)
        {
            if (subSetEntityFamilyQueues != null)
            {
                foreach (EntityFamilyQueue subSetEntityFamilyQueue in subSetEntityFamilyQueues)
                {
                    //Get sub set object from super entity family queue collection.
                    EntityFamilyQueue superSetEntityFamilyQueue = this.GetByEntityFamilyId(subSetEntityFamilyQueue.EntityFamilyId);

                    if (superSetEntityFamilyQueue != null)
                    {
                        if (!superSetEntityFamilyQueue.IsSuperSetOf(subSetEntityFamilyQueue))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets EntityFamilyQueue based on entity family id(container specific family id)
        /// </summary>
        /// <param name="entityFamilyId">Indicates entity family id for which entity family queue to be retrieved</param>
        /// <returns>EntityFamilyQueue based on given entity family id</returns>
        public EntityFamilyQueue GetByEntityFamilyId(Int64 entityFamilyId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityFamilyQueue entityFamilyQueue in this._items)
                {
                    if (entityFamilyQueue.EntityFamilyId == entityFamilyId)
                    {
                        return entityFamilyQueue;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets EntityFamilyQueue based on entity global family id
        /// </summary>
        /// <param name="entityGlobalFamilyId">Indicates entity global family id for which entity family queue to be retrieved</param>
        /// <returns>EntityFamilyQueue based on given entity family id</returns>
        public EntityFamilyQueue GetByEntityGlobalFamilyId(Int64 entityGlobalFamilyId)
        {
            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityFamilyQueue entityFamilyQueue in this._items)
                {
                    if (entityFamilyQueue.EntityGlobalFamilyId == entityGlobalFamilyId)
                    {
                        return entityFamilyQueue;
                    }
                }
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityFamilyQueueCollection(String valuesAsXml)
        {
            #region Sample Xml

            /*
             <EntityFamilyQueues>
	            <EntityFamilyQueue Id="-1" EntityFamilyId="101" EntityGlobalFamilyId="100" IsInProgress="0" IsProcessed="0" Action="Update">
		            <EntityFamilyChangeContexts>
			            <EntityFamilyChangeContext>
				            <VariantsChangeContext>
					            <EntityChangeContexts>
						            <EntityChangeContext EntityId="52346" EntityTypeId="17" EntityTypeName="Color" VariantLevel="1" Action="Update">
							            <LocaleChangeContexts>
								            <LocaleChangeContext DataLocale="en-WW">
									            <AttributeChangeContexts>
										            <AttributeChangeContext Action="Update" AttributeIdList="" AttributeNameList="price" />
										            <AttributeChangeContext Action="Add" AttributeIdList="" AttributeNameList="colors" />
									            </AttributeChangeContexts>
									            <RelationshipChangeContexts>
										            <RelationshipChangeContext RelationshipId="11" FromEntityId="54656" RelatedEntityId="55675" RelationshipTypeId="6" RelationshipTypeName="Kit Relation" Action="Update">
											            <AttributeChangeContext Action="Update" AttributeIdList="" AttributeNameList="cross-Sell Description" />
											            <AttributeChangeContext Action="Delete" AttributeIdList="" AttributeNameList="cross-Sell Offer Code" />
										            </RelationshipChangeContext>
									            </RelationshipChangeContexts>
								            </LocaleChangeContext>
							            </LocaleChangeContexts>
						            </EntityChangeContext>
					            </EntityChangeContexts>
				            </VariantsChangeContext>
				            <ExtensionChangeContexts>
					            <ExtensionChangeContext ContainerId="-1" ContainerName="Big-Bazaar Product Master" ContainerType="Unknown" Qualifier="">
						            <VariantsChangeContext>
							            <EntityChangeContexts>
								            <EntityChangeContext EntityId="52346" EntityTypeId="17" EntityTypeName="Color" VariantLevel="1" Action="Update">
									            <LocaleChangeContexts>
										            <LocaleChangeContext DataLocale="en-WW">
											            <AttributeChangeContexts>
												            <AttributeChangeContext Action="Update" AttributeIdList="" AttributeNameList="price" />
												            <AttributeChangeContext Action="Add" AttributeIdList="" AttributeNameList="colors" />
											            </AttributeChangeContexts>
											            <RelationshipChangeContexts>
												            <RelationshipChangeContext RelationshipId="11" FromEntityId="54656" RelatedEntityId="55675" RelationshipTypeId="6" RelationshipTypeName="Kit Relation" Action="Update">
													            <AttributeChangeContext Action="Update" AttributeIdList="" AttributeNameList="cross-Sell Description" />
													            <AttributeChangeContext Action="Delete" AttributeIdList="" AttributeNameList="cross-Sell Offer Code" />
												            </RelationshipChangeContext>
											            </RelationshipChangeContexts>
										            </LocaleChangeContext>
									            </LocaleChangeContexts>
								            </EntityChangeContext>
							            </EntityChangeContexts>
						            </VariantsChangeContext>
					            </ExtensionChangeContext>
				            </ExtensionChangeContexts>
			            </EntityFamilyChangeContext>
		            </EntityFamilyChangeContexts>
	            </EntityFamilyQueue>
            </EntityFamilyQueues> 
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityFamilyQueue")
                        {
                            String EntityFamilyQueueXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(EntityFamilyQueueXml))
                            {
                                EntityFamilyQueue EntityFamilyQueue = new EntityFamilyQueue(EntityFamilyQueueXml);
                                if (EntityFamilyQueue != null)
                                {
                                    this.Add(EntityFamilyQueue);
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