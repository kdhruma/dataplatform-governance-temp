using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using MDM.BusinessObjects.Workflow;

    /// <summary>
    /// Specifies entity family queue.
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(MDMObject))]
    [KnownType(typeof(ObjectAction))]
    [KnownType(typeof(EntityFamilyChangeContextCollection))]
    [KnownType(typeof(EntityFamilyChangeContext))]
    [KnownType(typeof(VariantsChangeContext))]
    [KnownType(typeof(ExtensionChangeContextCollection))]
    [KnownType(typeof(ExtensionChangeContext))]
    [KnownType(typeof(WorkflowChangeContext))]
    [KnownType(typeof(WorkflowActionContext))]
    [KnownType(typeof(EntityChangeContextCollection))]
    [KnownType(typeof(EntityChangeContext))]
    [KnownType(typeof(LocaleChangeContext))]
    [KnownType(typeof(LocaleChangeContextCollection))]
    [KnownType(typeof(AttributeChangeContextCollection))]
    [KnownType(typeof(AttributeChangeContext))]
    [KnownType(typeof(RelationshipChangeContextCollection))]
    [KnownType(typeof(RelationshipChangeContext))]
    [KnownType(typeof(RevalidateContext))]
    public class EntityFamilyQueue : MDMObject, IEntityFamilyQueue
    {
        #region Fields

        /// <summary>
        ///  Indicates unique id of entity family queue.
        /// </summary>
        private Int64 _id = -1;

        /// <summary>
        /// Indicates entity family id for a variants tree
        /// </summary>
        private Int64 _entityFamilyId = -1;

        /// <summary>
        /// Indicates entity global family id including extended families
        /// </summary>
        private Int64 _entityGlobalFamilyId = -1;

        /// <summary>
        /// Indicates container id of an entity.
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Indicates family activity list of an entity.
        /// </summary>
        private EntityActivityList _entityActivityList = EntityActivityList.UnKnown;

        /// <summary>
        /// Indicates whether loading of entity family queue is in progress
        /// </summary>
        private Boolean _isInProgress = false;

        /// <summary>
        /// Indicates whether process is done for entity family queue or not
        /// </summary>
        private Boolean _isProcessed = false;

        /// <summary>
        /// Indicates entity family change contexts
        /// </summary>
        private EntityFamilyChangeContextCollection _entityFamilyChangeContexts = null;

        /// <summary>
        /// Indicates context specific to revalidation of rule
        /// </summary>
        private RevalidateContext _revalidateContext = null;

        /// <summary>
        /// Indicates the caller context
        /// </summary>
        private CallerContext _callerContext = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EntityFamilyQueue()
            : base()
        {
        }

        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public EntityFamilyQueue(String valuesAsxml)
        {
            LoadEntityFamilyQueue(valuesAsxml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates unique id of entity family queue
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public new Int64 Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Indicates entity family id for a variants tree
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 EntityFamilyId
        {
            get
            {
                return this._entityFamilyId;
            }
            set
            {
                this._entityFamilyId = value;
            }
        }

        /// <summary>
        ///  Indicates entity global family id including extended families
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int64 EntityGlobalFamilyId
        {
            get
            {
                return this._entityGlobalFamilyId;
            }
            set
            {
                this._entityGlobalFamilyId = value;
            }
        }

        /// <summary>
        /// Indicates container id of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
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

        /// <summary>
        /// Specifies family activity list of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public EntityActivityList EntityActivityList
        {
            get
            {
                return this._entityActivityList;
            }
            set
            {
                this._entityActivityList = value;
            }
        }

        /// <summary>
        /// Indicates whether loading of entity family queue is in progress
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Boolean IsInProgress
        {
            get
            {
                return this._isInProgress;
            }
            set
            {
                this._isInProgress = value;
            }
        }

        /// <summary>
        /// Indicates whether process is done for entity family queue or not
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Boolean IsProcessed
        {
            get
            {
                return this._isProcessed;
            }
            set
            {
                this._isProcessed = value;
            }
        }

        /// <summary>
        /// Indicates entity family change contexts
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
        public EntityFamilyChangeContextCollection EntityFamilyChangeContexts
        {
            get
            {
                if (this._entityFamilyChangeContexts == null)
                {
                    this._entityFamilyChangeContexts = new EntityFamilyChangeContextCollection();
                }

                return this._entityFamilyChangeContexts;
            }
            set
            {
                this._entityFamilyChangeContexts = value;
            }
        }

        /// <summary>
        /// Property denoting the caller context.
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
        public CallerContext CallerContext
        {
            get
            {
                if (_callerContext == null)
                {
                    _callerContext = new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Entity);
                }

                return _callerContext;
            }
            set
            {
                _callerContext = value;
            }
        }

        /// <summary>
        /// Property denoting the context specific to revalidation of rule
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public RevalidateContext RevalidateContext
        {
            get
            {
                return _revalidateContext;
            }
            set
            {
                _revalidateContext = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //EntityFamilyQueue node start
                    xmlWriter.WriteStartElement("EntityFamilyQueue");

                    #region write EntityFamilyQueue

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("EntityFamilyId", this.EntityFamilyId.ToString());
                    xmlWriter.WriteAttributeString("EntityGlobalFamilyId", this.EntityGlobalFamilyId.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("EntityActivityList", this.EntityActivityList.ToString());
                    xmlWriter.WriteAttributeString("IsInProgress", this.IsInProgress.ToString());
                    xmlWriter.WriteAttributeString("IsProcessed", this.IsProcessed.ToString());

                    #endregion

                    #region write Entity family change contexts xml

                    if (this._entityFamilyChangeContexts != null)
                    {
                        xmlWriter.WriteRaw(this.EntityFamilyChangeContexts.ToXml());
                    }

                    #endregion write Entity family change context xml

                    #region write Entity family revalidation rule contexts xml

                    if (this._revalidateContext != null)
                    {
                        xmlWriter.WriteRaw(this.RevalidateContext.ToXml());
                    }

                    #endregion

                    //EntityFamilyQueue node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is EntityFamilyQueue)
            {
                EntityFamilyQueue objectToBeCompared = obj as EntityFamilyQueue;

                if (this.Id != objectToBeCompared.Id)
                {
                    return false;
                }
                if (this.EntityFamilyId != objectToBeCompared.EntityFamilyId)
                {
                    return false;
                }
                if (this.EntityGlobalFamilyId != objectToBeCompared.EntityGlobalFamilyId)
                {
                    return false;
                }
                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }
                if (this.EntityActivityList != objectToBeCompared.EntityActivityList)
                {
                    return false;
                }
                if (this.IsInProgress != objectToBeCompared.IsInProgress)
                {
                    return false;
                }
                if (this.IsProcessed != objectToBeCompared.IsProcessed)
                {
                    return false;
                }
                if (!this.EntityFamilyChangeContexts.Equals(objectToBeCompared.EntityFamilyChangeContexts))
                {
                    return false;
                }

                return true;
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

            hashCode = this.Id.GetHashCode() ^ this.EntityFamilyId.GetHashCode() ^ this.EntityGlobalFamilyId.GetHashCode() ^ this.ContainerId.GetHashCode() ^
                       this.EntityActivityList.GetHashCode() ^ this.IsInProgress.GetHashCode() ^ this.IsProcessed.GetHashCode() ^
                       this.EntityFamilyChangeContexts.GetHashCode() ^ this.RevalidateContext.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Compare entity family queue with current entity family queue
        /// </summary>
        /// <param name="subSetEntityFamilyQueue">Indicates object to be compared with current object</param>
        /// <returns>Returns true if comparison is succeeded otherwise, returns false.</returns>
        public bool IsSuperSetOf(EntityFamilyQueue subSetEntityFamilyQueue)
        {
            if (this.EntityFamilyId != subSetEntityFamilyQueue.EntityFamilyId)
            {
                return false;
            }
            if (this.EntityGlobalFamilyId != subSetEntityFamilyQueue.EntityGlobalFamilyId)
            {
                return false;
            }
            if (this.ContainerId != subSetEntityFamilyQueue.ContainerId)
            {
                return false;
            }
            if (this.EntityActivityList != subSetEntityFamilyQueue.EntityActivityList)
            {
                return false;
            }
            if (!this.EntityFamilyChangeContexts.ToXml().Equals(this.EntityFamilyChangeContexts.ToXml()))
            {
                return false;
            }

            return true;
        }

        #region Entity Family Change Contexts related methods

        /// <summary>
        /// Gets the entity family change contexts
        /// </summary>
        /// <returns>Returns entity family change contexts.</returns>
        public IEntityFamilyChangeContextCollection GetEntityFamilyChangeContexts()
        {
            if (this._entityFamilyChangeContexts == null)
            {
                return null;
            }

            return (IEntityFamilyChangeContextCollection)this._entityFamilyChangeContexts;
        }

        /// <summary>
        /// Sets the entity family change contexts
        /// </summary>
        /// <param name="iEntityFamilyChangeContexts">Indicates the entity family change contexts to be set</param>
        public void SetEntityFamilyChangeContexts(IEntityFamilyChangeContextCollection iEntityFamilyChangeContexts)
        {
            this.EntityFamilyChangeContexts = (EntityFamilyChangeContextCollection)iEntityFamilyChangeContexts;
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityFamilyQueue(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityFamilyQueue")
                    {
                        #region Read EntityFamilyQueue

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this._id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._id);
                            }
                            if (reader.MoveToAttribute("EntityFamilyId"))
                            {
                                this._entityFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityFamilyId);
                            }
                            if (reader.MoveToAttribute("EntityGlobalFamilyId"))
                            {
                                this._entityGlobalFamilyId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityGlobalFamilyId);
                            }
                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this._containerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }
                            if (reader.MoveToAttribute("EntityActivityList"))
                            {
                                EntityActivityList entityActivityList = Core.EntityActivityList.UnKnown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out entityActivityList);
                                this._entityActivityList = entityActivityList;
                            }
                            if (reader.MoveToAttribute("IsInProgress"))
                            {
                                this._isInProgress = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isInProgress);
                            }
                            if (reader.MoveToAttribute("IsProcessed"))
                            {
                                this._isProcessed = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this._isProcessed);
                            }
                            if (reader.MoveToAttribute("Action"))
                            {
                                this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityFamilyChangeContexts")
                    {
                        #region Read EntityFamilyChangeContexts

                        String EntityFamilyChangeContextsXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(EntityFamilyChangeContextsXml))
                        {
                            this._entityFamilyChangeContexts = new EntityFamilyChangeContextCollection(EntityFamilyChangeContextsXml);
                        }

                        #endregion Read EntityFamilyChangeContexts
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RevalidateContext")
                    {
                        #region Read RevalidateContext

                        String revalidateContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(revalidateContextXml))
                        {
                            this._revalidateContext = new RevalidateContext(revalidateContextXml);
                        }

                        #endregion Read RevalidateContext
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

        #endregion

        #endregion Methods
    }
}