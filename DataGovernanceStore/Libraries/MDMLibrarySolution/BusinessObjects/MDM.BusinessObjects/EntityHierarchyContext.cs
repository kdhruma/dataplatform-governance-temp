using System;
using System.Runtime.Serialization;
using ProtoBuf;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies Context which indicates what all information needs to be loaded for Entity Hierarchy
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(EntityContext))]
    [KnownType(typeof(EntityContextCollection))]
    public class EntityHierarchyContext : MDMObject, IEntityHierarchyContext
    {
        #region Fields

        /// <summary>
        /// Specifies entity context for entity hierarchy to be loaded
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private EntityContextCollection _entityContexts = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameters less constructor
        /// </summary>
        public EntityHierarchyContext()
        {

        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XMl representation of entity hierarchy context</param>
        public EntityHierarchyContext(String valuesAsXml)
        {
            LoadEntityHierarchyContext(valuesAsXml);
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// Add IEntityDataContext for provided entity type name
        /// </summary>
        /// <param name="entityTypeName">Entity type name</param>
        /// <param name="entityDataContext">IEntityDataContext to be set</param>
        public void AddEntityDataContext(String entityTypeName, IEntityDataContext entityDataContext)
        {
            if (String.IsNullOrWhiteSpace(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }

            if (entityDataContext == null)
            {
                throw new ArgumentNullException("entityDataContext");
            }

            var entityContext = (EntityContext)entityDataContext;

            entityContext.EntityTypeName = entityTypeName;
            
            this.EntityContexts.Add(entityContext);
        }

        /// <summary>
        /// Get IEntityDataContext for provided entity type name
        /// </summary>
        /// <param name="entityTypeName">Enity type name</param>
        public IEntityDataContext GetEntityDataContext(String entityTypeName)
        {
            if (String.IsNullOrWhiteSpace(entityTypeName))
            {
                throw new ArgumentNullException("entityTypeName");
            }

            if (this._entityContexts != null && _entityContexts.Count > 0)
            {
                return _entityContexts.GetByEntityTypeName(entityTypeName);
            }

            return null;
        }

        /// <summary>
        /// Get XMl representation of entity hierarchy context
        /// </summary>
        /// <returns>XMl string representation of entity hierarchy context</returns>
        public override String ToXml()
        {
            String xml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // Start EntityHierarchyContext Node
                    xmlWriter.WriteStartElement("EntityHierarchyContext");

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
        /// Create a new entity hierarchy context object.
        /// </summary>
        /// <returns>New entity hierarchy context instance</returns>
        public IEntityHierarchyContext Clone()
        {
            EntityHierarchyContext clonedEntityHierarchyContext = new EntityHierarchyContext();

            if (this._entityContexts != null)
            {
                clonedEntityHierarchyContext._entityContexts = (EntityContextCollection)this._entityContexts.Clone();
            }

            return clonedEntityHierarchyContext;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadEntityHierarchyContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHierarchyContext")
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

        #endregion Methods
    }
}
