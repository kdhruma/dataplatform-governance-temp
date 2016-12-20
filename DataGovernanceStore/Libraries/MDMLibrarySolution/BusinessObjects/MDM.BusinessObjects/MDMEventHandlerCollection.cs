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
    /// Contains properties and methods to manipulate mdm event handler collection object
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMEventHandlerCollection : IMDMEventHandlerCollection
    {
        #region Variables

        /// <summary>
        /// Field for mdm event handler collection
        /// </summary>
        [DataMember]
        private Collection<MDMEventHandler> _mdmEventHandlerCollection = new Collection<MDMEventHandler>();

        #endregion Variables

        #region Properties

        #region IMDMEventHandlerCollection Properties

        /// <summary>
        /// Gets the number of elements contained in MDMEventHandlerCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._mdmEventHandlerCollection.Count;
            }
        }

        /// <summary>
        /// Check if MDMEventHandlerCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion IMDMEventHandlerCollection Properties

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMEventHandlerCollection()
        { 
        
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMEventHandler object</param>
        public MDMEventHandlerCollection(String valuesAsXml)
        {
            LoadMDMEventHandlerCollectionDetail(valuesAsXml);
        }

        #endregion Constructor

        #region Interface Members

        #region IMDMEventHandlerCollection Members

        /// <summary>
        /// Get XML representation of mdm event handler collection object
        /// </summary>
        /// <returns>XML representation of mdm event handler collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMEventHandlerCollection");

                    if (this._mdmEventHandlerCollection != null)
                    {
                        foreach (IMDMEventHandler mdmEventHandler in this._mdmEventHandlerCollection)
                        {
                            xmlWriter.WriteRaw(mdmEventHandler.ToXml());
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
        /// Gets a cloned instance of the current mdm event handler collection object
        /// </summary>
        /// <returns>Cloned instance of the current mdm event handler collection object</returns>
        public IMDMEventHandlerCollection Clone()
        {
            IMDMEventHandlerCollection clonedCollection = new MDMEventHandlerCollection();
            if (this._mdmEventHandlerCollection != null && this._mdmEventHandlerCollection.Count > 0)
            {
                foreach (IMDMEventHandler mdmEventHandler in this._mdmEventHandlerCollection)
                {
                    clonedCollection.Add(mdmEventHandler.Clone());
                }
            }
            return clonedCollection;
        }

        /// <summary>
        /// Loads MDMEventHandlerCollection object
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMEventHandlerCollection</param>
        public void LoadMDMEventHandlerCollectionDetail(String valuesAsXml)
        {
            #region Sample Xml

            /*
             <MDMEventHandlerCollection>
	             <MDMEventHandler Id="-1" EventInfoId="-1" AssemblyName="RS.MDM.EventIntegrator.Business.dll" FullyQualifiedClassName="MDM.EventIntegrator.Business.RuleFrameworkEventSubscriber" EventHandlerMethodName="EntityValidateEvent" Sequence="0" Enabled="True" Module="MDMCenter" IsHandlerMethodStatic="True" IsInternal="True" AppConfigKeyName="MDMCenter.RuleEngine.Enabled" AppConfigKeyValue="true" FeatureConfigKeyName="" FeatureConfigKeyValue="False">
			        <MDMEventInfo Id="-1" Name="EntityEventManager_EntityValidate" EventManagerClassName="MDM.EntityManager.Business.EntityEventManager" Description="" IsObsolete="False" AlternateEventInfoId="0" HasBusinessRuleSupport="True" IsInternal="False" AssemblyName="EntityValidate" EventName="RS.MDM.EntityManager.Business.dll" />
		            <SubscribedOnServiceTypes>
			            <ServiceType>APIEngine</ServiceType>
			            <ServiceType>JobService</ServiceType>
		            </SubscribedOnServiceTypes>
	            </MDMEventHandler>
             <MDMEventHandlerCollection>
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMEventHandler")
                        {
                            String mdmEventHandlerXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdmEventHandlerXml))
                            {
                                MDMEventHandler mdmEventHandler = new MDMEventHandler(mdmEventHandlerXml);
                                if (mdmEventHandler != null)
                                {
                                    this.Add(mdmEventHandler);
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
        /// Compares MDMEventHandlerCollection with current collection
        /// </summary>
        /// <param name="subSetMDMEventHandlerCollection">Indicates MDMEventHandlerCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMEventHandlerCollection subSetMDMEventHandlerCollection)
        {
            if (subSetMDMEventHandlerCollection != null)
            {
                foreach (MDMEventHandler subSetMDMEventHandler in subSetMDMEventHandlerCollection)
                {

                    MDMEventHandler sourceMDMEventHandler = this.GetMDMEventHandler(subSetMDMEventHandler.AssemblyName,
                                                                                          subSetMDMEventHandler.FullyQualifiedClassName,
                                                                                          subSetMDMEventHandler.EventHandlerMethodName);

                    //If it doesn't return, that means super set doesn't contain that eventHandler.
                    //So return false, else do further comparison
                    if (sourceMDMEventHandler != null)
                    {
                        if (!sourceMDMEventHandler.IsSuperSetOf(subSetMDMEventHandler))
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

        #endregion IMDMEventHandlerCollection Members

        #region ICollection<MDMEventHandler> Members

        /// <summary>
        /// Adds the specified mdm event handler.
        /// </summary>
        /// <param name="mdmEventHandler">The mdm event handler.</param>
        public void Add(IMDMEventHandler mdmEventHandler)
        {
            Add((MDMEventHandler)mdmEventHandler);
        }

        /// <summary>
        /// Adds the specified mdm event handler.
        /// </summary>
        /// <param name="mdmEventHandler">The mdm event handler.</param>
        public void Add(MDMEventHandler mdmEventHandler)
        {
            this._mdmEventHandlerCollection.Add(mdmEventHandler);
        }

        /// <summary>
        /// Removes all items from the current mdm event handler collection object
        /// </summary>
        public void Clear()
        {
            this._mdmEventHandlerCollection.Clear();
        }

        /// <summary>
        /// Determines whether the current mdm event handler collection object contains the specified mdm event handler.
        /// </summary>
        /// <param name="mdmEventHandler">The mdm event handler.</param>
        /// <returns>true if found in collection else false</returns>
        public Boolean Contains(IMDMEventHandler mdmEventHandler)
        {
            return Contains((MDMEventHandler)mdmEventHandler);
        }

        /// <summary>
        /// Determines whether the current mdm event handler collection object contains the specified mdm event handler.
        /// </summary>
        /// <param name="mdmEventHandler">The mdm event handler.</param>
        /// <returns>true if found in collection else false</returns>
        public Boolean Contains(MDMEventHandler mdmEventHandler)
        {
            return this._mdmEventHandlerCollection.Contains(mdmEventHandler);
        }

        /// <summary>
        /// Copies the elements of the MDMEventHandlerCollection to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from MDMEventHandlerCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(IMDMEventHandler[] array, Int32 arrayIndex)
        {
            CopyTo((MDMEventHandler[])array, arrayIndex);
        }

        /// <summary>
        /// Copies the elements of the MDMEventHandlerCollection to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements copied from MDMEventHandlerCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(MDMEventHandler[] array, Int32 arrayIndex)
        {
            this._mdmEventHandlerCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MDMEventHandlerCollection.
        /// </summary>
        /// <param name="mdmEventHandler">The mdm event handler object to remove from the MDMEventHandlerCollection.</param>
        /// <returns>true if mdm event handler is successfully removed; otherwise, false.</returns>
        public Boolean Remove(IMDMEventHandler mdmEventHandler)
        {
            return Remove((MDMEventHandler)mdmEventHandler);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MDMEventHandlerCollection.
        /// </summary>
        /// <param name="mdmEventHandler">The mdm event handler object to remove from the MDMEventHandlerCollection.</param>
        /// <returns>true if mdm event handler is successfully removed; otherwise, false.</returns>
        public Boolean Remove(MDMEventHandler mdmEventHandler)
        {
            return this._mdmEventHandlerCollection.Remove(mdmEventHandler);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IMDMEventHandler> GetEnumerator()
        {
            return this._mdmEventHandlerCollection.GetEnumerator();
        }

        #endregion ICollection<MDMEventHandler> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An enumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._mdmEventHandlerCollection.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current mdm event handler collection object
        /// </summary>
        /// <returns>Cloned instance of the current mdm event handler collection object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #region Utility Methods

        /// <summary>
        /// Gets MDMEventHandler by provided event handler name
        /// </summary>
        /// <param name="assemblyName">Indicates name of the assembly in which event handler is defined</param>
        /// <param name="fullyQualifiedClassName">Indicates name of the class in which the method is defined</param>
        /// <param name="eventHandlerMethodName">Indicates name of the method which will be invoked when the event is fired</param>
        /// <returns>MDMEventHandler</returns>
        public MDMEventHandler GetMDMEventHandler(String assemblyName, String fullyQualifiedClassName, String eventHandlerMethodName)
        {
            MDMEventHandler result = null;

            if (!String.IsNullOrEmpty(assemblyName) &&
                !String.IsNullOrEmpty(fullyQualifiedClassName) && 
                !String.IsNullOrEmpty(eventHandlerMethodName))
            {
                foreach (MDMEventHandler mdmEventHandler in _mdmEventHandlerCollection)
                {
                    if (String.Compare(mdmEventHandler.AssemblyName, assemblyName) == 0 &&
                        String.Compare(mdmEventHandler.FullyQualifiedClassName, fullyQualifiedClassName) == 0 &&
                        String.Compare(mdmEventHandler.EventHandlerMethodName, eventHandlerMethodName) == 0)
                    {
                        result = mdmEventHandler;
                        break;
                    }
                } 
            }

            return result;
        }

        #endregion Utility Methods

        #endregion Interface Members
    }
}
