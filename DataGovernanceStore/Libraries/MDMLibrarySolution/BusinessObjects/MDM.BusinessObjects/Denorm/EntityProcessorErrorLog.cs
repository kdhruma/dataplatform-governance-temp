using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects.Denorm
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Represent entity processor error log for logging error information in entity processor
    /// </summary>
    [DataContract]
    public class EntityProcessorErrorLog : MDMObject, IEntityProcessorErrorLog
    {
        #region Fields

        /// <summary>
        /// Indicates unique Id of this Object
        /// </summary>
        private Int64 _id;

        /// <summary>
        /// Indicates impacted entity Id
        /// </summary>
        private Int64 _impactedEntityId;

        /// <summary>
        /// Field indicating impacted entity short name
        /// </summary>
        private String _impactedEntityName = String.Empty;

        /// <summary>
        /// Field indicating impacted entity Long name
        /// </summary>
        private String _impactedEntityLongName = String.Empty;

        /// <summary>
        /// Indicates the priority of the imapcted entity
        /// </summary>
        private Int32 _priority;

        /// <summary>
        /// Indicates the error message
        /// </summary>
        private String _errorMessage;

        /// <summary>
        /// Field indictaes	Type of denorm is used to indicate which denorm got error? 
        /// Enttiy Attribute denorm, Metadata denorm or relationship denorm
        /// </summary>
        private String _processorName;

        /// <summary>
        /// 
        /// </summary>
        private DateTime? _modifiedDateTime;

        /// <summary>
        /// 
        /// </summary>
        private String _modifiedUser;

        /// <summary>
        /// 
        /// </summary>
        private String _modifiedProgram;

        /// <summary>
        /// Indicates the catalog Id of the Impacted entity
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// indicates the PK of Impacted Entity Log which is referred to impacted entity
        /// </summary>
        private Int64 _entityActivityLogId = 0;

        /// <summary>
        /// Action performed by the processor
        /// </summary>
        private EntityActivityList _performedAction = EntityActivityList.Any;

        #endregion

        #region Properties

        /// <summary>
        ///  Property denoting the unique Id of the error log table
        /// </summary>
        [DataMember]
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
        ///  Property denoting impacted entity Id
        /// </summary>
        [DataMember]
        public Int64 ImpactedEntityId
        {
            get
            {
                return this._impactedEntityId;
            }
            set
            {
                this._impactedEntityId = value;
            }
        }

        /// <summary>
        /// Property impacted indicating entity short name
        /// </summary>
        [DataMember]
        public String ImpactedEntityName
        {
            get { return _impactedEntityName; }
            set { _impactedEntityName = value; }
        }

        /// <summary>
        /// Property impacted indicating entity Long name
        /// </summary>
        [DataMember]
        public String ImpactedEntityLongName
        {
            get { return _impactedEntityLongName; }
            set { _impactedEntityLongName = value; }
        }

        /// <summary>
        ///  Property denoting priority of the imapcted entity
        /// </summary>
        [DataMember]
        public Int32 Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        /// <summary>
        ///  Property denoting error message
        /// </summary>
        [DataMember]
        public String ErrorMessage
        {
            get
            {
                return this._errorMessage;
            }
            set
            {
                this._errorMessage = value;
            }
        }

        /// <summary>
        /// Property indicates the Type of denorm is used to indicate which denorm got error? 
        /// Enttiy Attribute denorm, Metadata denorm or relationship denorm
        /// </summary>
        [DataMember]
        public String ProcessorName
        {
            get
            {
                return this._processorName;
            }
            set
            {
                this._processorName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? ModifiedDateTime
        {
            get
            {
                return this._modifiedDateTime;
            }
            set
            {
                this._modifiedDateTime = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ModifiedUser
        {
            get
            {
                return this._modifiedUser;
            }
            set
            {
                this._modifiedUser = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ModifiedProgram
        {
            get
            {
                return this._modifiedProgram;
            }
            set
            {
                this._modifiedProgram = value;
            }
        }
        

        /// <summary>
        /// Indicates the catalog Id of the Impacted entity
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get
            {
                return _containerId;
            }
            set
            {
                _containerId = value;
            }
        }

        /// <summary>
        /// indicates the PK of Impacted Entity Log which is referred to impacted entity
        /// </summary>
        [DataMember]
        public Int64 EntityActivityLogId
        {
            get
            {
                return this._entityActivityLogId;
            }
            set
            {
                this._entityActivityLogId = value;
            }
        }


        /// <summary>
        /// Action performed by the processor
        /// </summary>
        [DataMember]
        public EntityActivityList PerformedAction
        {
            get
            {
                return this._performedAction;
            }
            set
            {
                this._performedAction = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntityProcessorErrorLog()
            : base()
        {
        }

        
        /// <summary>
        /// Constructor with XML
        /// </summary>
        /// <param name="valuesAsxml">XML having xml value</param>
        public EntityProcessorErrorLog(String valuesAsxml)
        {
            LoadEntityProcessorErrorLog(valuesAsxml);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public void LoadEntityProcessorErrorLog(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityProcessorErrorLog")
                        {
                            #region Read Denorm Result Properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                    this.Id = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);

                                if (reader.MoveToAttribute("EntityId"))
                                    this.ImpactedEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), 0);

                                if(reader.MoveToAttribute("EntityName"))
                                    this.ImpactedEntityName = reader.ReadContentAsString();

                                if(reader.MoveToAttribute("EntityLongName"))
                                    this.ImpactedEntityLongName = reader.ReadContentAsString();

								if (reader.MoveToAttribute("Priority"))
                                    this.Priority = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);

                                if (reader.MoveToAttribute("ErrorMessage"))
                                    this.ErrorMessage = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ProcessorName"))
                                    this.ProcessorName = reader.ReadContentAsString();
                               
                                if (reader.MoveToAttribute("PerformedAction"))
                                {
                                    EntityActivityList action = EntityActivityList.UnKnown;
                                    Enum.TryParse(reader.ReadContentAsString(), true, out action);
                                    this.PerformedAction = action;
                                }

                                if (reader.MoveToAttribute("ModifiedDateTime"))
                                    this.ModifiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());

                                if (reader.MoveToAttribute("ModifiedUser"))
                                    this.ModifiedUser = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ModifiedProgram"))
                                    this.ModifiedProgram = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ContainerId"))
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);

                                if (reader.MoveToAttribute("EntityActivityLogId"))
                                {
                                    this.EntityActivityLogId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), -1);
                                }
                            }

                            #endregion
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

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of EntityProcessorErrorLog
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml()
        {
            String denormXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //jobXml node start
            xmlWriter.WriteStartElement("EntityProcessorErrorLog");

            #region Write Properties

            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("EntityId", this.ImpactedEntityId.ToString());
            xmlWriter.WriteAttributeString("EntityName", this.ImpactedEntityName);
            xmlWriter.WriteAttributeString("EntityLongName", this.ImpactedEntityLongName);
            xmlWriter.WriteAttributeString("Priority", this.Priority.ToString());
            xmlWriter.WriteAttributeString("ErrorMessage", this.ErrorMessage);
            xmlWriter.WriteAttributeString("ProcessorName", this.ProcessorName);
            xmlWriter.WriteAttributeString("PerformedAction", this.PerformedAction.ToString());

            if (this.ModifiedDateTime == null)
            {
                xmlWriter.WriteAttributeString("ModifiedDateTime", String.Empty);
            }
            else
            {
                xmlWriter.WriteAttributeString("ModifiedDateTime", this.ModifiedDateTime.ToString());
            }
            xmlWriter.WriteAttributeString("ModifiedUser", this.ModifiedUser);
            xmlWriter.WriteAttributeString("ModifiedProgram", this.ModifiedProgram);
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("EntityActivityLogId", this.EntityActivityLogId.ToString());

            #endregion

            //Denorm Result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            denormXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return denormXml;
        }

        /// <summary>
        /// Get Xml representation of Denorm Result
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            // No serialization implemented for now...
            return this.ToXml();
        }

        #endregion ToXml methods

        #endregion Public Methods
    }
}
