using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Contains properties and methods to manipulate event info collection object
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMEventInfoCollection : IMDMEventInfoCollection
    {
        #region Variables

        /// <summary>
        /// Field for event information collection
        /// </summary>
        [DataMember]
        private Collection<MDMEventInfo> _eventInfoCollection = new Collection<MDMEventInfo>();


        #endregion Variables

        #region Properties

        #region IEventInfoCollection Properties

        /// <summary>
        /// Gets the number of elements contained in EventInfoCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._eventInfoCollection.Count;
            }
        }

        /// <summary>
        /// Check if EventInfoCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion IEventInfoCollection Properties

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMEventInfoCollection()
        { 
        
        }
    
        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMEventInfoCollection object</param>
        public MDMEventInfoCollection(String valuesAsXml)
        { 
            LoadMDMEventInfoCollectionDetail(valuesAsXml);
        }
        
        #endregion Constructor

        #region Interface Members

        #region IEventInfoCollection Members

        /// <summary>
        /// Get XML representation of event info collection object
        /// </summary>
        /// <returns>XML representation of event info collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMEventInfoCollection");

                    if (this._eventInfoCollection != null)
                    {
                        foreach (IMDMEventInfo eventInfo in this._eventInfoCollection)
                        {
                            xmlWriter.WriteRaw(eventInfo.ToXml());
                        }
                    }

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
        /// Loads MDMEventInfo collection object from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMEventInfo collection object</param>
        public void LoadMDMEventInfoCollectionDetail(String valuesAsXml)
        {
            #region Sample Xml

            /*
                 <MDMEventInfoCollection>
			            <MDMEventInfo Id="-1" Name="EntityEventManager_EntityValidate" EventManagerClassName="MDM.EntityManager.Business.EntityEventManager" Description="" IsObsolete="False" AlternateEventInfoId="0" HasBusinessRuleSupport="True" IsInternal="False" AssemblyName="EntityValidate" EventName="RS.MDM.EntityManager.Business.dll" />
                 </MDMEventInfoCollection>
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
                            String eventInfoXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(eventInfoXml))
                            {
                                MDMEventInfo eventInfo = new MDMEventInfo(eventInfoXml);
                                if (eventInfo != null)
                                {
                                    this.Add(eventInfo);
                                }
                            }
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

        /// <summary>
        /// Compares MDMEvenInfoCollection with current collection
        /// </summary>
        /// <param name="subSetMDMEventInfoCollection">Indicates MDMEvenInfoCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMEventInfoCollection subSetMDMEventInfoCollection)
        {
            if (subSetMDMEventInfoCollection != null)
            {
                foreach (MDMEventInfo subSetMDMEventInfo in subSetMDMEventInfoCollection)
                {
                    MDMEventInfo sourceMDMEventInfo = this.GetMDMEventInfoByName(subSetMDMEventInfo.Name);

                    //If it doesn't return, that means super set doesn't contain that eventInfo.
                    //So return false, else do further comparison
                    if (sourceMDMEventInfo != null)
                    {
                        if (!sourceMDMEventInfo.IsSuperSetOf(subSetMDMEventInfo))
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
        /// Gets a cloned instance of the current event info collection object
        /// </summary>
        /// <returns>Cloned instance of the current event info collection object</returns>
        public IMDMEventInfoCollection Clone()
        {
            IMDMEventInfoCollection clonedCollection = new MDMEventInfoCollection();
            if (this._eventInfoCollection != null && this._eventInfoCollection.Count > 0)
            {
                foreach (IMDMEventInfo eventInfo in this._eventInfoCollection)
                {
                    clonedCollection.Add(eventInfo.Clone());
                }
            }
            return clonedCollection;
        }

        /// <summary>
        /// Gets a MDMEventInfo for the requested Id
        /// </summary>
        /// <param name="eventInfoId">Indicates the eventInfo Id</param>
        /// <returns>Returns a MDMEventInfo object.</returns>
        IMDMEventInfo IMDMEventInfoCollection.GetEventInfoById(Int32 eventInfoId)
        {
            return GetEventInfoById(eventInfoId);
        }

        #endregion IEventInfoCollection Members

        #region ICollection<EventInfo> Members

        /// <summary>
        /// Adds the specified event information.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        public void Add(IMDMEventInfo eventInfo)
        {
            Add((MDMEventInfo)eventInfo);
        }

        /// <summary>
        /// Adds the specified event information.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        public void Add(MDMEventInfo eventInfo)
        {
            this._eventInfoCollection.Add(eventInfo);
        }

        /// <summary>
        /// Removes all items from the current event info collection object
        /// </summary>
        public void Clear()
        {
            this._eventInfoCollection.Clear();
        }

        /// <summary>
        /// Determines whether the current event info collection object contains the specified event information].
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        /// <returns>true if found in collection else false</returns>
        public Boolean Contains(IMDMEventInfo eventInfo)
        {
            return Contains((MDMEventInfo)eventInfo);
        }

        /// <summary>
        /// Determines whether the current event info collection object contains the specified event information].
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        /// <returns>true if found in collection else false</returns>
        public Boolean Contains(MDMEventInfo eventInfo)
        {
            return this._eventInfoCollection.Contains(eventInfo);
        }

        /// <summary>
        /// Copies the elements of the EventInfoCollection to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from EventInfoCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(IMDMEventInfo[] array, Int32 arrayIndex)
        {
            CopyTo((MDMEventInfo[])array, arrayIndex);
        }

        /// <summary>
        /// Copies the elements of the EventInfoCollection to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from EventInfoCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(MDMEventInfo[] array, Int32 arrayIndex)
        {
            this._eventInfoCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EventInfoCollection.
        /// </summary>
        /// <param name="eventInfo">The event info object to remove from the EventInfoCollection.</param>
        /// <returns>true if event info is successfully removed; otherwise, false.</returns>
        public Boolean Remove(IMDMEventInfo eventInfo)
        {
            return Remove((MDMEventInfo)eventInfo);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the EventInfoCollection.
        /// </summary>
        /// <param name="eventInfo">The event info object to remove from the EventInfoCollection.</param>
        /// <returns>true if event info is successfully removed; otherwise, false.</returns>
        public Boolean Remove(MDMEventInfo eventInfo)
        {
            return this._eventInfoCollection.Remove(eventInfo);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IMDMEventInfo> GetEnumerator()
        {
            return this._eventInfoCollection.GetEnumerator();
        }

        #endregion ICollection<EventInfo> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An enumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._eventInfoCollection.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current event info collection object
        /// </summary>
        /// <returns>Cloned instance of the current event info collection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #endregion Interface Members

        #region Utility Methods

        /// <summary>
        /// Gets a MDMEventInfo for the requested Id
        /// </summary>
        /// <param name="eventInfoId">Indicates the eventInfo Id</param>
        /// <returns>Returns a MDMEventInfo object.</returns>
        public MDMEventInfo GetEventInfoById(Int32 eventInfoId)
        {
            MDMEventInfo result = null;

            foreach (MDMEventInfo mdmEventInfo in _eventInfoCollection)
            {
                if (mdmEventInfo.Id == eventInfoId)
                {
                    result = mdmEventInfo;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a MDMEventInfo for the requested mdm eventinfo name
        /// </summary>
        /// <param name="mdmEventInfoName">Indicates eventInfo name</param>
        /// <returns>Returns a MDMEventInfo object.</returns>
        public MDMEventInfo GetMDMEventInfoByName(String mdmEventInfoName)
        {
            MDMEventInfo result = null;

            if (!String.IsNullOrEmpty(mdmEventInfoName))
            {
                foreach (MDMEventInfo mdmEventInfo in _eventInfoCollection)
                {
                    if (String.Compare(mdmEventInfo.Name, mdmEventInfoName) == 0)
                    {
                        result = mdmEventInfo;
                        break;
                    }
                } 
            }

            return result;
        }

        /// <summary>
        /// Gets the list of MDMEventInfo Ids
        /// </summary>
        /// <returns>Returns the list of MDMEventInfo Ids</returns>
        public Collection<Int32> GetMDMEventIds()
        {
            Collection<Int32> eventIds = null;
            if (_eventInfoCollection != null)
            {
                eventIds = new Collection<Int32>();
                foreach (MDMEventInfo mdmEventInfo in _eventInfoCollection)
                {
                    eventIds.Add(mdmEventInfo.Id);
                }
            }
            return eventIds;
        }

        /// <summary>
        /// Gets a MDMEventInfo for the requested event name
        /// </summary>
        /// <param name="eventName">Indicates event name</param>
        /// <returns>Returns a MDMEventInfo object.</returns>
        public MDMEventInfo GetMDMEventInfoByEventName(String eventName)
        {
            MDMEventInfo result = null;

            if (!String.IsNullOrWhiteSpace(eventName))
            {
                foreach (MDMEventInfo mdmEventInfo in _eventInfoCollection)
                {
                    if (String.Compare(mdmEventInfo.EventName, eventName) == 0)
                    {
                        result = mdmEventInfo;
                        break;
                    }
                }
            }

            return result;
        }

        #endregion Utility Methods
    }
}
