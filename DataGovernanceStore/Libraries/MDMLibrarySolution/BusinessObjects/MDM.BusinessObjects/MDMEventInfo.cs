using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Contains properties and methods to manipulate event info object
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMEventInfo : MDMObject, IMDMEventInfo
    {
        #region Variables
        /// <summary>
        /// Field for event manager class name
        /// </summary>
        [DataMember]
        private String _eventManagerClassName = String.Empty;

        /// <summary>
        /// Field for description
        /// </summary>
        [DataMember]
        private String _description = String.Empty;

        /// <summary>
        /// Field for value indicating if event info is obsolete
        /// </summary>
        [DataMember]
        private Boolean _isObsolete = false;

        /// <summary>
        /// Field for alternate event information identifier
        /// </summary>
        [DataMember]
        private Int32 _alternateEventInfoId = 0;

        /// <summary>
        /// Field for value indicating if event info has business rule support
        /// </summary>
        [DataMember]
        private Boolean _hasBusinessRuleSupport = false;

        /// <summary>
        /// Field for value indicating if event info is internal
        /// </summary>
        [DataMember]
        private Boolean _isInternal = false;

        /// <summary>
        /// Field for assembly name
        /// </summary>
        [DataMember]
        private String _assemblyName = String.Empty;

        /// <summary>
        /// Field for Event name
        /// </summary>
        [DataMember]
        private String _eventName = String.Empty;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Indicates the name of the event manager class.
        /// </summary>
        /// <value>
        /// The name of the event manager class.
        /// </value>
        public String EventManagerClassName
        {
            get
            {
                return _eventManagerClassName;
            }
            set
            {
                _eventManagerClassName = value;
            }
        }

        /// <summary>
        /// Indicates the description of the event.
        /// </summary>
        /// <value>
        /// The description of the event.
        /// </value>
        public String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Indicates a value indicating whether this event info is obsolete.
        /// </summary>
        /// <value>
        /// True if this event info is obsolete; otherwise, False.
        /// </value>
        public Boolean IsObsolete
        {
            get
            {
                return _isObsolete;
            }
            set
            {
                _isObsolete = value;
            }
        }

        /// <summary>
        /// Indicates the alternate event information identifier.
        /// </summary>
        /// <value>
        /// The alternate event information identifier.
        /// </value>
        public Int32 AlternateEventInfoId
        {
            get
            {
                return _alternateEventInfoId;
            }
            set
            {
                _alternateEventInfoId = value;
            }
        }

        /// <summary>
        /// Indicates whether this event info has business rule support.
        /// </summary>
        /// <value>
        /// True if this event info has business rule support; otherwise, False.
        /// </value>
        public Boolean HasBusinessRuleSupport
        {
            get
            {
                return _hasBusinessRuleSupport;
            }
            set
            {
                _hasBusinessRuleSupport = value;
            }
        }

        /// <summary>
        /// Property denoting a value indicating whether this event info is internal.
        /// </summary>
        /// <value>
        /// true if this instance is internal; otherwise, false.
        /// </value>
        public Boolean IsInternal
        {
            get
            {
                return _isInternal;
            }
            set
            {
                _isInternal = value;
            }
        }

        /// <summary>
        /// Property denoting the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        public String AssemblyName
        {
            get
            {
                return _assemblyName;
            }
            set
            {
                _assemblyName = value;
            }
        }

        /// <summary>
        /// Property denoting the name of the Event.
        /// </summary>
        /// <value>
        /// The name of the Event.
        /// </value>
        public String EventName
        {
            get
            {
                return _eventName;
            }
            set
            {
                _eventName = value;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMEventInfo() : base()
        { 
        
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation for MDMEventInfo object</param>
        public MDMEventInfo(String valuesAsXml)
        {
            LoadMDMEventInfoDetail(valuesAsXml);
        }
        
        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get XML representation of event info object
        /// </summary>
        /// <returns>
        /// XML representation of current event info object
        /// </returns>
        public override String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMEventInfo");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("EventManagerClassName", this._eventManagerClassName);
                    xmlWriter.WriteAttributeString("Description", this._description);
                    xmlWriter.WriteAttributeString("IsObsolete", this._isObsolete.ToString());
                    xmlWriter.WriteAttributeString("AlternateEventInfoId", this._alternateEventInfoId.ToString());
                    xmlWriter.WriteAttributeString("HasBusinessRuleSupport", this._hasBusinessRuleSupport.ToString());
                    xmlWriter.WriteAttributeString("IsInternal", this._isInternal.ToString());
                    xmlWriter.WriteAttributeString("AssemblyName", this._assemblyName);
                    xmlWriter.WriteAttributeString("EventName", this._eventName);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    //Table node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Get XML representation of event info object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>
        /// XML representation of current event info object
        /// </returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml();
        }
        
        /// <summary>
        /// Gets a cloned instance of the current event info object
        /// </summary>
        /// <returns>
        /// Cloned instance of the current event info object
        /// </returns>
        public IMDMEventInfo Clone()
        {
            IMDMEventInfo clonedObject = new MDMEventInfo();
            clonedObject.Id = this.Id;
            clonedObject.AlternateEventInfoId = this._alternateEventInfoId;
            clonedObject.Description = this._description;
            clonedObject.EventManagerClassName = this._eventManagerClassName;
            clonedObject.HasBusinessRuleSupport = this._hasBusinessRuleSupport;
            clonedObject.IsInternal = this._isInternal;
            clonedObject.IsObsolete = this._isObsolete;
            clonedObject.Name = this.Name;
            clonedObject.AssemblyName = this._assemblyName;
            clonedObject.EventName = this._eventName;
            clonedObject.Action = this.Action;

            return clonedObject;
        }

        /// <summary>
        /// Loads MDMEventInfo object from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMEventInfo object</param>
        public void LoadMDMEventInfoDetail(String valuesAsXml)
        {
            #region Sample Xml

            /*
			    <MDMEventInfo Id="-1" Name="EntityEventManager_EntityValidate" EventManagerClassName="MDM.EntityManager.Business.EntityEventManager" Description="" IsObsolete="False" AlternateEventInfoId="0" HasBusinessRuleSupport="True" IsInternal="False" AssemblyName="EntityValidate" EventName="RS.MDM.EntityManager.Business.dll" />
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMEventInfo")
                        {
                            //Read EventInfo metadata
                            #region Read EventInfo Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EventManagerClassName"))
                                {
                                    this._eventManagerClassName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Description"))
                                {
                                    this._description = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("IsObsolete"))
                                {
                                    this._isObsolete = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("AlternateEventInfoId"))
                                {
                                    this._alternateEventInfoId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._alternateEventInfoId);
                                }

                                if (reader.MoveToAttribute("HasBusinessRuleSupport"))
                                {
                                    this._hasBusinessRuleSupport = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("IsInternal"))
                                {
                                    this._isInternal = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("AssemblyName"))
                                {
                                    this._assemblyName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EventName"))
                                {
                                    this._eventName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                reader.Read();
                            }

                            #endregion Read EventInfo Attributes
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Compares MDMEvenInfo object with current MDMEvenInfo object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subSetMDMEvenInfo">Indicates MDMEvenInfo object to be compared with current MDMEvenInfo object</param>
        /// <param name="compareIds">Indicates whether to compare ids for the current object or not</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMEventInfo subSetMDMEvenInfo, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subSetMDMEvenInfo.Id || this.AlternateEventInfoId !=  subSetMDMEvenInfo.AlternateEventInfoId)
                {
                    return false;
                }
            }

            if (String.Compare(this.Name, subSetMDMEvenInfo.Name) != 0)
            {
                return false;
            }

            if(String.Compare(this.EventManagerClassName, subSetMDMEvenInfo.EventManagerClassName) != 0)
            {
                return false;
            }

            if(String.Compare(this.EventName, subSetMDMEvenInfo.EventName) != 0)
            {
                return false;
            }

            if(String.Compare(this.Description, subSetMDMEvenInfo.Description) != 0)
            {
                return false;
            }

            if(String.Compare(this.AssemblyName, subSetMDMEvenInfo.AssemblyName) != 0)
            {
                return false;
            }

            if(this.IsObsolete != subSetMDMEvenInfo.IsObsolete)
            {
                return false;
            }

            if( this.HasBusinessRuleSupport != subSetMDMEvenInfo.HasBusinessRuleSupport)
            {
                return false;
            }

            if(this.IsInternal != subSetMDMEvenInfo.IsInternal)
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}
