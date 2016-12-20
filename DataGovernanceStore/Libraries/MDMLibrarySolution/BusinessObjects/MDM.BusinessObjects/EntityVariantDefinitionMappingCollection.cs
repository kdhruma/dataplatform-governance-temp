using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{

    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Interfaces;
    using System.Collections;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Specifies entity variant definition mapping collection
    /// </summary>
    [DataContract]
    public class EntityVariantDefinitionMappingCollection : InterfaceContractCollection<IEntityVariantDefinitionMapping, EntityVariantDefinitionMapping>, IEntityVariantDefinitionMappingCollection, IDataModelObjectCollection, ICloneable 
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.EntityVariantDefinitionMapping;
            }
        }

        #endregion Properties

        #region Methods

        #region Public

        /// <summary>
        /// Adds item in current collection
        /// </summary>
        /// <param name="item">Indicates item to be added in collection</param>
        public void AddDataModelObject(IDataModelObject item)
        {
            this.Add(item as EntityVariantDefinitionMapping);
        }

        /// <summary>
        /// Splits current collection based on batchSize
        /// </summary>
        /// <param name="batchSize">Indicates no of objects to be put in each batch</param>
        /// <returns>IDataModelObjectCollection in Batch</returns>
        public Collection<IDataModelObjectCollection> Split(Int32 batchSize)
        {
            Collection<IDataModelObjectCollection> entityVariantDefinitionMappingsInBatch = null;

            if (this._items != null)
            {
                entityVariantDefinitionMappingsInBatch = Utility.Split(this, batchSize);
            }

            return entityVariantDefinitionMappingsInBatch;
        }

        /// <summary>
        /// Remove dataModelObject based on reference id.
        /// </summary>
        /// <param name="referenceIds">Indicates collection of referenceId of an entity type which is to be fetched.</param>
        /// <returns>true if item was successfully removed from the collection;otherwise, false. This method also returns false if item is not found in the original</returns>
        public bool Remove(Collection<string> referenceIds)
        {
            Boolean result = true;

            EntityVariantDefinitionMappingCollection entityVariantDefinitionMappings = Get(referenceIds);

            if (entityVariantDefinitionMappings != null && entityVariantDefinitionMappings.Count > 0)
            {
                foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in entityVariantDefinitionMappings)
                {
                    result = result && this.Remove(entityVariantDefinitionMapping);
                }
            }

            return result;
        }


        /// <summary>
        /// Gets the entity variant definition mapping for given referenceIds.
        /// </summary>
        /// <param name="referenceIds">referenceIds to be searched in the collection</param>
        /// <returns>Returns entity variant definition mapping having given referenceIds</returns>
        private EntityVariantDefinitionMappingCollection Get(Collection<String> referenceIds)
        {
            EntityVariantDefinitionMappingCollection entityVariantDefinitions = new EntityVariantDefinitionMappingCollection();
            Int32 counter = 0;

            if (this._items != null && this._items.Count > 0 && referenceIds != null && referenceIds.Count > 0)
            {
                foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in this._items)
                {
                    if (referenceIds.Contains(entityVariantDefinitionMapping.ReferenceId))
                    {
                        entityVariantDefinitions.Add(entityVariantDefinitionMapping);
                        counter++;
                    }

                    if (referenceIds.Count.Equals(counter))
                        break;
                }
            }

            return entityVariantDefinitions;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a entity variant definition mappings.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._items.GetEnumerator();
        }

        /// <summary>
        /// Represents entity variant definition mapping collection in Xml format 
        /// </summary>
        /// <returns>Returns entity variant definition mapping collection in Xml format as string.</returns>
        public String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("EntityVariantDefinitionMappings");

            if (this._items != null)
            {
                foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in this._items)
                {
                    xmlWriter.WriteRaw(entityVariantDefinitionMapping.ToXml());
                }
            }

            //EntityVariantDefinitionMappings node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get entity variant definition mapping based on given entity variant definition name, container name and category path.
        /// </summary>
        /// <param name="entityVariantDefinitionName">Indicates name of entity variant definition.</param>
        /// <param name="containerName">Indicates name of container.</param>
        /// <param name="categoryPath">Indicates name of category path.</param>
        /// <returns>Returns entity variant definition mapping based on given entity variant definition name, container name and category path.</returns>
        public EntityVariantDefinitionMapping Get(String entityVariantDefinitionName, String containerName, String categoryPath)
        {
            if (!String.IsNullOrWhiteSpace(entityVariantDefinitionName))
            {
                if (this._items != null && this._items.Count > 0)
                {
                    foreach (var item in this._items)
                    {
                        if (String.Compare(item.EntityVariantDefinitionName, entityVariantDefinitionName) == 0 &&
                            String.Compare(item.ContainerName, containerName) == 0 &&
                            String.Compare(item.CategoryPath, categoryPath) == 0)
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get entity variant definition mapping by given reference Id
        /// </summary>
        /// <param name="referenceId">Indicates reference identifier of entity variant definition mapping.</param>
        /// <returns>Returns entity variant definition mapping by given Id.</returns>
        public EntityVariantDefinitionMapping GetByReferenceId(String referenceId)
        {
            if (!String.IsNullOrWhiteSpace(referenceId))
            {
                if (this._items != null && this._items.Count > 0)
                {
                    foreach (var item in this._items)
                    {
                        if (String.Compare(item.ReferenceId, referenceId) == 0)
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a cloned instance of the current entity variant definition collection object
        /// </summary>
        /// <returns>Returns a cloned instance of the current entity variant definition collection object</returns>
        public IEntityVariantDefinitionMappingCollection Clone()
        {
            EntityVariantDefinitionMappingCollection clonedEntityVariantDefinitionMappings = new EntityVariantDefinitionMappingCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (EntityVariantDefinitionMapping entityVariantDefinitionMapping in this._items)
                {
                    EntityVariantDefinitionMapping clonedIEntityVariantDefinitionMappping = (EntityVariantDefinitionMapping)entityVariantDefinitionMapping.Clone();
                    clonedEntityVariantDefinitionMappings.Add(clonedIEntityVariantDefinitionMappping);
                }
            }

            return clonedEntityVariantDefinitionMappings;
        }

        #endregion Public

        #region Private

        #endregion Private

        #endregion Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current application context mapping collection object
        /// </summary>
        /// <returns>Cloned instance of the current application context mapping collection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

    }
}
