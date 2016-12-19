using ProtoBuf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Contains properties and methods to manipulate mdm event handler object
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMEventHandler : MDMObject, IMDMEventHandler
    {
        #region Variables
        /// <summary>
        /// Field for assembly name
        /// </summary>
        [DataMember]
        private String _assemblyName = String.Empty;

        /// <summary>
        /// Field for fully qualified class name
        /// </summary>
        [DataMember]
        private String _fullyQualifiedClassName = String.Empty;

        /// <summary>
        /// Field for event handler method name
        /// </summary>
        [DataMember]
        private String _eventHandlerMethodName = String.Empty;

        /// <summary>
        /// Field for sequence
        /// </summary>
        [DataMember]
        private Int32 _sequence = 0;

        /// <summary>
        /// Field for enabled
        /// </summary>
        [DataMember]
        private Boolean _enabled = false;

        /// <summary>
        /// Field for module
        /// </summary>
        [DataMember]
        private MDMCenterExtensionEnum _module = MDMCenterExtensionEnum.UnKnown;

        /// <summary>
        /// Field for subscribed on service types
        /// </summary>
        [DataMember]
        private Collection<MDMServiceType> _subscribedOnServiceTypes = null;

        /// <summary>
        /// Field for event information identifier
        /// </summary>
        [DataMember]
        private Int32 _eventInfoId = 0;

        /// <summary>
        /// Field for denoting if the handler method is static
        /// </summary>
        [DataMember]
        private Boolean _isHandlerMethodStatic = false;

        /// <summary>
        /// Field for value indicating if event info is internal
        /// </summary>
        [DataMember]
        private Boolean _isInternal = false;

        /// <summary>
        /// Field for MDMEventInfo
        /// </summary>
        [DataMember]
        private MDMEventInfo _mdmEventInfo = null;

        /// <summary>
        /// Field for appconfig key name
        /// </summary>
        [DataMember]
        private String _appConfigKeyName = String.Empty;

        /// <summary>
        /// Field for appconfig key value
        /// </summary>
        [DataMember]
        private String _appConfigKeyValue = String.Empty;

        /// <summary>
        /// Field for featureconfig key name
        /// </summary>
        [DataMember]
        private String _featureConfigKeyName = String.Empty;

        /// <summary>
        /// Field for featureconfig key Value
        /// </summary>
        [DataMember]
        private Boolean _featureConfigKeyValue = false;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Property denoting the name of the appconfig key
        /// </summary>
        /// <value>
        /// The name of the appconfig key
        /// </value>
        public String AppConfigKeyName
        {
            get
            {
                return _appConfigKeyName;
            }
            set
            {
                _appConfigKeyName = value;
            }
        }

        /// <summary>
        /// Property denoting the value of the appconfig key
        /// </summary>
        /// <value>
        /// The value of the appconfig key
        /// </value>
        public String AppConfigKeyValue
        {
            get
            {
                return _appConfigKeyValue;
            }
            set
            {
                _appConfigKeyValue = value;
            }
        }

        /// <summary>
        /// Property denoting the name of the featureconfig key
        /// </summary>
        /// <value>
        /// The name of the featureconfig key
        /// </value>
        public String FeatureConfigKeyName
        {
            get
            {
                return _featureConfigKeyName;
            }
            set
            {
                _featureConfigKeyName = value;
            }
        }

        /// <summary>
        /// Property denoting the value of the featureconfig key
        /// </summary>
        /// <value>
        /// The value of the featureconfig key
        /// </value>
        public Boolean FeatureConfigKeyValue
        {
            get
            {
                return _featureConfigKeyValue;
            }
            set
            {
                _featureConfigKeyValue = value;
            }
        }

        /// <summary>
        /// Property denoting the event information identifier.
        /// </summary>
        /// <value>
        /// The event information identifier.
        /// </value>
        public Int32 EventInfoId
        {
            get
            {
                return _eventInfoId;
            }
            set
            {
                _eventInfoId = value;
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
        /// Property denoting the fully qualified class name.
        /// </summary>
        /// <value>
        /// The fully qualified class name.
        /// </value>
        public String FullyQualifiedClassName
        {
            get
            {
                return _fullyQualifiedClassName;
            }
            set
            {
                _fullyQualifiedClassName = value;
            }
        }

        /// <summary>
        /// Property denoting the name of the event handler method.
        /// </summary>
        /// <value>
        /// The name of the event handler method.
        /// </value>
        public String EventHandlerMethodName
        {
            get
            {
                return _eventHandlerMethodName;
            }
            set
            {
                _eventHandlerMethodName = value;
            }
        }

        /// <summary>
        /// Property denoting the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public Int32 Sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
            }
        }

        /// <summary>
        /// Property denoting a value indicating whether mdm event handler is enabled.
        /// </summary>
        /// <value>
        /// true if enabled; otherwise, false.
        /// </value>
        public Boolean Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        /// <summary>
        /// Property denoting the module where this event handler is subscribed from.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        public MDMCenterExtensionEnum Module
        {
            get
            {
                return _module;
            }
            set
            {
                _module = value;
            }
        }

        /// <summary>
        /// Property denoting the service types where this event handler is subscribed on.
        /// </summary>
        /// <value>
        /// The subscribed on service types.
        /// </value>
        public Collection<MDMServiceType> SubscribedOnServiceTypes
        {
            get
            {
                if (_subscribedOnServiceTypes == null)
                {
                    _subscribedOnServiceTypes = new Collection<MDMServiceType>();
                }
                return _subscribedOnServiceTypes;
            }
            set
            {
                _subscribedOnServiceTypes = value;
            }
        }

        /// <summary>
        /// Property denoting a value indicating whether this handler method is static.
        /// </summary>
        /// <value>
        /// true if this instance is handler method static; otherwise, false.
        /// </value>
        public Boolean IsHandlerMethodStatic
        {
            get
            {
                return _isHandlerMethodStatic;
            }
            set
            {
                _isHandlerMethodStatic = value;
            }
        }

        /// <summary>
        /// Property denoting a value indicating whether the event handler method is internal.
        /// </summary>
        /// <value>
        /// true if this handler is internal; otherwise, false.
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
        /// Property denoting the MDMEventInfo for the current MDMEventHandler
        /// </summary>
        /// <value>
        /// Returns the MDMEventInfo object.
        /// </value>
        public MDMEventInfo MDMEventInfo
        {
            get
            {
                return _mdmEventInfo;
            }
            set
            {
                _mdmEventInfo = value;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMEventHandler() : base() 
        { 
        
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMEventHandler object</param>
        public MDMEventHandler(String valuesAsXml)
        {
            LoadMDMEventHandlerDetail(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get XML representation of mdm event handler object
        /// </summary>
        /// <returns>XML representation of mdm event handler object</returns>
        public override String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMEventHandler");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("EventInfoId", this._eventInfoId.ToString());
                    xmlWriter.WriteAttributeString("AssemblyName", this._assemblyName);
                    xmlWriter.WriteAttributeString("FullyQualifiedClassName", this._fullyQualifiedClassName);
                    xmlWriter.WriteAttributeString("EventHandlerMethodName", this._eventHandlerMethodName);
                    xmlWriter.WriteAttributeString("Sequence", this._sequence.ToString());
                    xmlWriter.WriteAttributeString("Enabled", this._enabled.ToString());
                    xmlWriter.WriteAttributeString("Module", this._module.ToString());
                    xmlWriter.WriteAttributeString("IsHandlerMethodStatic", this._isHandlerMethodStatic.ToString());
                    xmlWriter.WriteAttributeString("IsInternal", this._isInternal.ToString());
                    xmlWriter.WriteAttributeString("AppConfigKeyName", this._appConfigKeyName);
                    xmlWriter.WriteAttributeString("AppConfigKeyValue", this._appConfigKeyValue);
                    xmlWriter.WriteAttributeString("FeatureConfigKeyName", this._featureConfigKeyName);
                    xmlWriter.WriteAttributeString("FeatureConfigKeyValue", this._featureConfigKeyValue.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    if (this._mdmEventInfo != null)
                    {
                        xmlWriter.WriteRaw(this._mdmEventInfo.ToXml());
                    }

                    xmlWriter.WriteStartElement("SubscribedOnServiceTypes");

                    if (this._subscribedOnServiceTypes != null && this._subscribedOnServiceTypes.Count > 0)
                    {
                        foreach (MDMServiceType serviceType in this._subscribedOnServiceTypes)
                        {
                            xmlWriter.WriteElementString("ServiceType", serviceType.ToString());
                        }
                    }
                    xmlWriter.WriteEndElement(); //SubscribedOnServiceTypes
                    xmlWriter.WriteEndElement(); //MDMEventHandler
                    xmlWriter.Flush();
                    
                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Get XML representation of mdm event handler object
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>
        /// XML representation of mdm event handler object
        /// </returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Loads MDMEventHandler object
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMEventHandler object</param>
        public void LoadMDMEventHandlerDetail(String valuesAsXml)
        {
            #region Sample Xml

            /*
                <MDMEventHandler Id="-1" EventInfoId="-1" AssemblyName="RS.MDM.EventIntegrator.Business.dll" FullyQualifiedClassName="MDM.EventIntegrator.Business.RuleFrameworkEventSubscriber" EventHandlerMethodName="EntityValidateEvent" Sequence="0" Enabled="True" Module="MDMCenter" IsHandlerMethodStatic="True" IsInternal="True" AppConfigKeyName="MDMCenter.RuleEngine.Enabled" AppConfigKeyValue="true" FeatureConfigKeyName="" FeatureConfigKeyValue="False">
			        <EventInfo Id="-1" Name="EntityEventManager_EntityValidate" EventManagerClassName="MDM.EntityManager.Business.EntityEventManager" Description="" IsObsolete="False" AlternateEventInfoId="0" HasBusinessRuleSupport="True" IsInternal="False" AssemblyName="EntityValidate" EventName="RS.MDM.EntityManager.Business.dll" />
		            <SubscribedOnServiceTypes>
			            <ServiceType>APIEngine</ServiceType>
			            <ServiceType>JobService</ServiceType>
		            </SubscribedOnServiceTypes>
	            </MDMEventHandler>
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    if (reader != null)
                    {
                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMEventHandler")
                            {
                                //Read MDMEventHandler metadata
                                #region Read EventInfo Attributes

                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                    }

                                    if (reader.MoveToAttribute("EventInfoId"))
                                    {
                                        this._eventInfoId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._eventInfoId);
                                    }

                                    if (reader.MoveToAttribute("AssemblyName"))
                                    {
                                        this._assemblyName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("FullyQualifiedClassName"))
                                    {
                                        this._fullyQualifiedClassName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("EventHandlerMethodName"))
                                    {
                                        this._eventHandlerMethodName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Sequence"))
                                    {
                                        this._sequence = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._sequence);
                                    }

                                    if (reader.MoveToAttribute("Enabled"))
                                    {
                                        this._enabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("Module"))
                                    {
                                        MDMCenterExtensionEnum module = MDMCenterExtensionEnum.UnKnown;
                                        ValueTypeHelper.EnumTryParse<MDMCenterExtensionEnum>(reader.ReadContentAsString(), false, out module);
                                        this._module = module;
                                    }

                                    if (reader.MoveToAttribute("IsHandlerMethodStatic"))
                                    {
                                        this._isHandlerMethodStatic = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("IsInternal"))
                                    {
                                        this._isInternal = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("AppConfigKeyName"))
                                    {
                                        this._appConfigKeyName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("AppConfigKeyValue"))
                                    {
                                        this._appConfigKeyValue = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("FeatureConfigKeyName"))
                                    {
                                        this._featureConfigKeyName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("FeatureConfigKeyValue"))
                                    {
                                        this._featureConfigKeyValue = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                    }

                                    if (reader.MoveToAttribute("Action"))
                                    {
                                        this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                    }

                                    reader.Read();
                                }

                                #endregion Read EventInfo Attributes
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMEventInfo")
                            {
                                String eventInfoXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(eventInfoXml))
                                {
                                    this._mdmEventInfo = new MDMEventInfo(eventInfoXml);
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServiceType")
                            {
                                MDMServiceType serviceTypeEnumValue = MDMServiceType.UnKnown;
                                String serviceTypeStringValue = reader.ReadInnerXml();
                                
                                ValueTypeHelper.EnumTryParse<MDMServiceType>(serviceTypeStringValue, false, out serviceTypeEnumValue);
                                this.SubscribedOnServiceTypes.Add(serviceTypeEnumValue);
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
                        } 
                    }
                } 
            }
        }

        /// <summary>
        /// Compares MDMEventHandler object with current MDMEventHandler object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subSetMDMEventHandler">Indicates MDMEventHandler object to be compared with current MDMEventHandler object</param>
        /// <param name="compareIds">Indicates whether to compare ids for the current object or not</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMEventHandler subSetMDMEventHandler, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subSetMDMEventHandler.Id || this.EventInfoId != subSetMDMEventHandler.EventInfoId)
                {
                    return false;
                }
            }

            if (String.Compare(this.AppConfigKeyName, subSetMDMEventHandler.AppConfigKeyName) != 0 || String.Compare(this.AppConfigKeyValue, subSetMDMEventHandler.AppConfigKeyValue) != 0)
            {
                return false;
            }

            if (String.Compare(this.FeatureConfigKeyName, subSetMDMEventHandler.FeatureConfigKeyName) != 0 || this.FeatureConfigKeyValue != subSetMDMEventHandler.FeatureConfigKeyValue)
            {
                return false;
            }

            if(String.Compare(this.AssemblyName, subSetMDMEventHandler.AssemblyName) != 0)
            {
                return false;
            }

            if(String.Compare(this.EventHandlerMethodName, subSetMDMEventHandler.EventHandlerMethodName) != 0)
            {
                return false;
            }
           
            if(String.Compare(this.FullyQualifiedClassName, subSetMDMEventHandler.FullyQualifiedClassName) != 0)
            {
                return false;
            }

            if(this.Sequence != subSetMDMEventHandler.Sequence)
            {
                return false;
            }

            if (this.IsInternal != subSetMDMEventHandler.IsInternal)
            {
                return false;
            }

            if (this.IsHandlerMethodStatic != subSetMDMEventHandler.IsHandlerMethodStatic)
            {
                return false;
            }

            if (this.Enabled != subSetMDMEventHandler.Enabled)
            {
                return false;
            }

            if (this.Module != subSetMDMEventHandler.Module)
            {
                return false;
            }

            if (!CompareServiceTypes(subSetMDMEventHandler.SubscribedOnServiceTypes))
            {
                return false;
            }

            if(!this.MDMEventInfo.IsSuperSetOf(subSetMDMEventHandler.MDMEventInfo))
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Gets a cloned instance of the current mdm event handler object
        /// </summary>
        /// <returns>Cloned instance of the current mdm event handler object</returns>
        public IMDMEventHandler Clone()
        {
            MDMEventHandler clonedObject = new MDMEventHandler();
            clonedObject.Id = this.Id;
            clonedObject.AssemblyName = this._assemblyName;
            clonedObject.Enabled = this._enabled;
            clonedObject.EventHandlerMethodName = this._eventHandlerMethodName;
            clonedObject.EventInfoId = this._eventInfoId;
            clonedObject.FullyQualifiedClassName = this._fullyQualifiedClassName;
            clonedObject.Module = this._module;
            clonedObject.Sequence = this._sequence;
            clonedObject.SubscribedOnServiceTypes = this._subscribedOnServiceTypes;
            clonedObject.IsHandlerMethodStatic = this._isHandlerMethodStatic;
            clonedObject.IsInternal = this._isInternal;
            clonedObject.MDMEventInfo = this._mdmEventInfo;
            clonedObject.AppConfigKeyName = this._appConfigKeyName;
            clonedObject.AppConfigKeyValue = this._appConfigKeyValue;
            clonedObject.FeatureConfigKeyName = this._featureConfigKeyName;
            clonedObject.FeatureConfigKeyValue = this._featureConfigKeyValue;
            clonedObject.Action = this.Action;

            return (MDMEventHandler)clonedObject;
        }

        /// <summary>
        /// Gets a MDMEventInfo for the current MDMEvent handler
        /// </summary>
        /// <returns>Returns the MDMEventInfo Object</returns>
        public IMDMEventInfo GetMDMEventInfo()
        {
            return _mdmEventInfo;
        }

        #endregion Public Methods

        #region Private  Methods

        #region Utility Methods

        private Boolean CompareServiceTypes(Collection<MDMServiceType> subSetServiceTypes)
        {
            if (subSetServiceTypes != null && subSetServiceTypes.Count > 0)
            {
                foreach (MDMServiceType serviceType in subSetServiceTypes)
                {
                    if (!this.SubscribedOnServiceTypes.Contains(serviceType))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion Utility Methods

        #endregion Private Methods

        #endregion Methods
    }
}
