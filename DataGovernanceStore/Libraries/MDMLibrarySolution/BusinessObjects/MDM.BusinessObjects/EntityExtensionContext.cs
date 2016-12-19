using System;
using System.Runtime.Serialization;
using ProtoBuf;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;
    using MDM.Core.Exceptions;

    /// <summary>
    /// Specifies Context which indicates what all information needs to be loaded for Entity Extension.
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(EntityContext))]
    [KnownType(typeof(EntityContextCollection))]
    public class EntityExtensionContext : MDMObject, IEntityExtensionContext
    {
        #region Fields

        /// <summary>
        /// Specifies entity context for entity extension to be loaded
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private EntityContextCollection _entityContexts = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameters less constructor
        /// </summary>
        public EntityExtensionContext()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XMl representation of entity extension context</param>
        public EntityExtensionContext(String valuesAsXml)
        {
            LoadEntityExtensionContext(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies child entity context for entity hierarchy to be loaded
        /// </summary>        
        public EntityContextCollection EntityContexts
        {
            get
            {
                if (_entityContexts == null)
                {
                    _entityContexts = new EntityContextCollection();
                }

                return this._entityContexts;
            }
            set
            {
                this._entityContexts = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets the child entity contexts
        /// </summary>
        /// <returns>Child entity contexts</returns>
        public IEntityContextCollection GetEntityContexts()
        {
            return this._entityContexts;
        }

        /// <summary>
        /// Get entity data context collection
        /// </summary>
        /// <returns>Returns Entity Data Context Collection</returns>
        public IEntityDataContextCollection GetEntityDataContexts()
        {
            return _entityContexts;
        }

        /// <summary>
        /// Sets the child entity contexts
        /// </summary>
        /// <param name="entityContextCollection">Specifies the entity context collection to be set to the hierarchy context</param>
        public void SetEntityContexts(IEntityContextCollection entityContextCollection)
        {
            if (entityContextCollection != null)
            {
                _entityContexts = (EntityContextCollection)entityContextCollection;
            }
        }

        /// <summary>
        /// Add IEntityDataContext for provided container name and category path
        /// Category Path is an optional parameter. if not provided, only container name will be honored.
        /// </summary>
        /// <param name="containerName">Indicates Container name</param>
        /// <param name="categoryPath">Indicates Category path.It is an optional parameter.</param>
        /// <param name="iEntityDataContext">IEntityDataContext to be set</param>
        public void AddEntityDataContext(String containerName, String categoryPath, IEntityDataContext iEntityDataContext)
        {
            if (String.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException("containerName");
            }

            if (iEntityDataContext == null)
            {
                throw new ArgumentNullException("iEntityDataContext");
            }

            //Validate duplicate extension context check for a given container and category path 
            var filteredEntityContext = this.EntityContexts.GetByContainerNameAndCategoryPath(containerName, categoryPath);

            if (filteredEntityContext != null)
            {
                throw new InvalidOperationException(String.Format("Duplicate extension context found for container name : {0} and category path : {1}", containerName, categoryPath));
            }

            var entityContext = (EntityContext)iEntityDataContext;

            //For hierarchy get including extensions, user can request based on container name and category path or container qualifier name.
            //Hence resetting container qualifier name as empty over here.
            entityContext.ContainerQualifierName = String.Empty;

            entityContext.ContainerName = containerName;
            entityContext.CategoryPath = categoryPath;

            this.EntityContexts.Add(entityContext);
        }

        /// <summary>
        /// Add IEntityDataContext for provided container qualifier name
        /// </summary>
        /// <param name="containerQualifierName">Indicates container qualifier name</param>
        /// <param name="iEntityDataContext">IEntityDataContext to be set</param>
        public void AddEntityDataContext(String containerQualifierName, IEntityDataContext iEntityDataContext)
        {
            if (String.IsNullOrWhiteSpace(containerQualifierName))
            {
                throw new ArgumentNullException("containerQualifierName");
            }

            if (iEntityDataContext == null)
            {
                throw new ArgumentNullException("iEntityDataContext");
            }

            var entityContext = (EntityContext)iEntityDataContext;

            //Validate duplicate extension context check for a given container and category path 
            var filteredEntityContext = this.EntityContexts.GetByContainerQualifierName(containerQualifierName);

            if (filteredEntityContext != null)
            {
                throw new InvalidOperationException(String.Format("Duplicate extension context found for container qualifier name : {0}", containerQualifierName));
            }

            //For hierarchy get including extensions, user can request based on container name and category path or container qualifier name.
            //Hence resetting container name and category path as empty over here.
            entityContext.ContainerName = String.Empty;
            entityContext.CategoryPath = String.Empty;

            entityContext.ContainerQualifierName = containerQualifierName;

            this.EntityContexts.Add(entityContext);
        }

        /// <summary>
        /// Get IEntityDataContext for provided entity type name
        /// Category Path is an optional parameter. If not provided, it will return IEntityDataContext for specified container Name
        /// </summary>
        /// <param name="containerName">Indicates Container name</param>
        /// <param name="categoryPath">Indicates CategoryPath. It is an optional parameter.</param>
        /// <returns></returns>
        public IEntityDataContext GetEntityDataContext(String containerName, String categoryPath)
        {
            if (String.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException("containerName");
            }

            if (this._entityContexts != null && _entityContexts.Count > 0)
            {
                return _entityContexts.GetByContainerNameAndCategoryPath(containerName, categoryPath);
            }

            return null;
        }

        /// <summary>
        /// Gets IEntityDataContext for the provided entity type name
        /// </summary>
        /// <param name="containerQualifierName">Indicates container qualifier name</param>
        /// <returns>Entity data context from the entity extension context for the provided container qualifier name</returns>
        public IEntityDataContext GetEntityDataContext(String containerQualifierName)
        {
            if (String.IsNullOrWhiteSpace(containerQualifierName))
            {
                throw new ArgumentNullException("containerQualifierName");
            }

            if (this._entityContexts != null && _entityContexts.Count > 0)
            {
                return _entityContexts.GetByContainerQualifierName(containerQualifierName);
            }

            return null;
        }

        /// <summary>
        /// Get XMl representation of entity extension context
        /// </summary>
        /// <returns>XMl string representation of entity extension context</returns>
        public override String ToXml()
        {
            String xml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // Start EntityHierarchyContext Node
                    xmlWriter.WriteStartElement("EntityExtensionContext");

                    if (this.EntityContexts != null)
                    {
                        xmlWriter.WriteRaw(this.EntityContexts.ToXml());
                    }

                    //End EntityHierarchyContext Node
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();
                    xml = sw.ToString();
                }
            }

            return xml;
        }

        /// <summary>
        /// Create a new entity extension context object.
        /// </summary>
        /// <returns>New entity extension context instance</returns>
        public IEntityExtensionContext Clone()
        {
            EntityExtensionContext clonedEntityExtensionContext = new EntityExtensionContext();

            if (this._entityContexts != null)
            {
                clonedEntityExtensionContext._entityContexts = (EntityContextCollection)this._entityContexts.Clone();
            }

            return clonedEntityExtensionContext;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityExtensionContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityExtensionContext")
                    {
                        reader.Read();
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityContexts")
                    {
                        this.EntityContexts = new EntityContextCollection(reader.ReadOuterXml());
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

        #endregion Private Methods

        #endregion
    }
}