using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDMRule map details
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleMapDetailCollection : ICollection<MDMRuleMapDetail>, IEnumerable<MDMRuleMapDetail>, IEquatable<MDMRuleMapDetailCollection>, IMDMRuleMapDetailCollection
    {
        #region Fields

        /// <summary>
        /// Field denotes the MDMRuleMapDetail collection
        /// </summary>
        [DataMember]
        private Collection<MDMRuleMapDetail> _mdmRuleMapDetailCollection = new Collection<MDMRuleMapDetail>();

        #endregion

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public MDMRuleMapDetailCollection()
            : base()
        {

        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRuleMapDetailCollection object</param>
        public MDMRuleMapDetailCollection(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in MDMRuleMapDetailCollection
        /// </summary>
        public Int32 Count
        {
            get { return this._mdmRuleMapDetailCollection.Count; }
        }

        /// <summary>
        /// Check if MDMRuleMapDetailCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Methods

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An enumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._mdmRuleMapDetailCollection.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IEnumerable<MDMRuleMapDetail> Members

        /// <summary>
        /// Returns an enumerator that iterates through a MDMRule map detail collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<MDMRuleMapDetail> GetEnumerator()
        {
            return this._mdmRuleMapDetailCollection.GetEnumerator();
        }

        #endregion  IEnumerable<MDMRuleMapDetail> Members

        #region ICollection<MDMRuleMapDetail> Members

        /// <summary>
        /// Add MDMRule map detail object in collection
        /// </summary>
        /// <param name="mdmRuleMapDetail">Indicates the MDMRule map detail</param>
        public void Add(MDMRuleMapDetail mdmRuleMapDetail)
        {
            if (mdmRuleMapDetail != null)
            {
                this._mdmRuleMapDetailCollection.Add(mdmRuleMapDetail);
            }
        }

        /// <summary>
        /// Add MDMRule map details object into existing collection
        /// </summary>
        /// <param name="mdmRuleMapDetails">Indicates the MDMRule map detail</param>
        /// <param name="allowDuplicate">Indicates whether to allow duplicate value or not</param>
        public void AddRange(MDMRuleMapDetailCollection mdmRuleMapDetails, Boolean allowDuplicate = true)
        {
            if (mdmRuleMapDetails != null && mdmRuleMapDetails.Count > 0)
            {
                foreach (MDMRuleMapDetail mdmRuleMapDetail in mdmRuleMapDetails)
                {
                    if (mdmRuleMapDetail != null)
                    {
                        if (allowDuplicate == true)
                        {
                            this._mdmRuleMapDetailCollection.Add(mdmRuleMapDetail);
                        }
                        else
                        {
                            if (this.ContainsByLongName(mdmRuleMapDetail.LongName) == false)
                            {
                                this._mdmRuleMapDetailCollection.Add(mdmRuleMapDetail);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes all MDMRule map details from collection
        /// </summary>
        public void Clear()
        {
            this._mdmRuleMapDetailCollection.Clear();
        }

        /// <summary>
        /// Determines whether the MDMRuleMapDetailCollection contains a specific MDMRule Map Detail
        /// </summary>
        /// <param name="mdmRuleMapDetail">Indicates the MDMRuleMapDetail.</param>
        /// <returns>
        /// <para>true : If MDMRule map detail found in MDMRuleMapDetailCollection</para>
        /// <para>false : If MDMRule map detail not found in MDMRuleMapDetailCollection</para>
        /// </returns>
        public Boolean Contains(MDMRuleMapDetail mdmRuleMapDetail)
        {
            return this._mdmRuleMapDetailCollection.Contains(mdmRuleMapDetail);
        }

        /// <summary>
        /// Copies the elements of the mdmRuleMapDetailCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from mdmRuleMapDetailCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(MDMRuleMapDetail[] array, Int32 arrayIndex)
        {
            this._mdmRuleMapDetailCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific MDMRuleMapDetail from the MDMRuleMapDetailCollection.
        /// </summary>
        /// <param name="mdmRuleMapDetail">The MDMRule map detail object to remove from the MDMRuleMapDetailCollection.</param>
        /// <returns>true if MDMRule map detail is successfully removed; otherwise, false. This method also returns false if MDMRule map detail was not found in the original collection</returns>
        public Boolean Remove(MDMRuleMapDetail mdmRuleMapDetail)
        {
            return this._mdmRuleMapDetailCollection.Remove(mdmRuleMapDetail);
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRuleMapDetailCollection)
            {
                Equals(obj as MDMRuleMapCollection);
            }
            return false;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="other">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(MDMRuleMapDetailCollection other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            else if (ReferenceEquals(this, other))
            {
                return true;
            }

            Int32 ruleMapsUnion = this._mdmRuleMapDetailCollection.ToList().Union(other.ToList()).Count();
            Int32 ruleMapsIntersect = this._mdmRuleMapDetailCollection.ToList().Intersect(other.ToList()).Count();
            if (ruleMapsUnion != ruleMapsIntersect)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;
            foreach (MDMRuleMapDetail mdmRuleMapDetail in this._mdmRuleMapDetailCollection)
            {
                hashCode += mdmRuleMapDetail.GetHashCode();
            }
            return hashCode;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current MDMRule map detail collection
        /// </param>
        public void LoadOperationResult(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMapDetail")
                        {
                            String result = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(result))
                            {
                                MDMRuleMapDetail mdmRuleMapDetail = new MDMRuleMapDetail(result);

                                if (mdmRuleMapDetail != null)
                                {
                                    this._mdmRuleMapDetailCollection.Add(mdmRuleMapDetail);
                                }
                            }
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
        /// Get Xml representation of MDMRule map detail collection
        /// </summary>
        /// <returns>Xml representation of MDMRule map detail collection object</returns>
        public String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("MDMRuleMapDetails");

                    if (this._mdmRuleMapDetailCollection != null)
                    {
                        foreach (MDMRuleMapDetail mdmRuleMapDetail in this._mdmRuleMapDetailCollection)
                        {
                            xmlWriter.WriteRaw(mdmRuleMapDetail.ToXml());
                        }
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Gets MDMRuleMapDetails based on requested event name
        /// </summary>
        /// <param name="eventName">Indicates the event name</param>
        /// <returns>Returns the MDMRuleMapDetailCollection object</returns>
        public IMDMRuleMapDetailCollection GetMDMRuleMapDetailsByEventName(String eventName)
        {
            if (String.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException("eventName", "eventName can not be null or empty");
            }

            MDMRuleMapDetailCollection results = new MDMRuleMapDetailCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                if (String.Compare(ruleMapDetail.EventName, eventName) == 0)
                {
                    results.Add(ruleMapDetail);
                }
            }

            return results;
        }

        /// <summary>
        /// Gets MDMRuleMapDetails based on requested MDMEvents
        /// </summary>
        /// <param name="mdmEvents">Indicates the MDMEvent</param>
        /// <returns>Returns the MDMRuleMapDetailCollection object</returns>
        public MDMRuleMapDetailCollection GetMDMRuleMapDetailsByEvents(Collection<MDMEvent> mdmEvents)
        {
            if (mdmEvents == null || mdmEvents.Count == 0)
            {
                throw new ArgumentNullException("mdmEvents", "mdmEvents can not be null");
            }

            MDMRuleMapDetailCollection results = new MDMRuleMapDetailCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                foreach (MDMEvent mdmEvent in mdmEvents)
                {
                    if (ruleMapDetail.EventId == (Int32)mdmEvent)
                    {
                        results.Add(ruleMapDetail);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets MDMRuleMapDetails based on requested MDMEvent
        /// </summary>
        /// <param name="mdmEvent">Indicates the MDMEvent</param>
        /// <returns>Returns the MDMRuleMapDetailCollection object</returns>
        public IMDMRuleMapDetailCollection GetMDMRuleMapDetailsByEvent(MDMEvent mdmEvent)
        {
            if (mdmEvent == MDMEvent.UnKnown)
            {
                throw new ArgumentNullException("mdmEvent", "mdmEvent can not be Unknown");
            }

            MDMRuleMapDetailCollection results = new MDMRuleMapDetailCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                if (ruleMapDetail.EventId == (Int32)mdmEvent)
                {
                    results.Add(ruleMapDetail);
                }
            }

            return results;
        }

        /// <summary>
        /// Gets MDMRuleMapDetails based on requested MDMRule type
        /// </summary>
        /// <param name="ruleType">Indicates the MDMRule type</param>
        /// <returns>Returns the MDMRuleMapDetailCollection object</returns>
        public IMDMRuleMapDetailCollection GetMDMRuleMapDetailsByRuleType(MDMRuleType ruleType)
        {
            MDMRuleMapDetailCollection results = new MDMRuleMapDetailCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                if (ruleMapDetail != null && ruleMapDetail.RuleType == ruleType)
                {
                    results.Add(ruleMapDetail);
                }
            }

            return results;
        }

        ///// <summary>
        ///// Gets MDMRuleMapDetails based on requested MDMRule type
        ///// </summary>
        ///// <returns>Returns the MDMRuleMapDetailCollection object</returns>
        //public MDMRuleMapDetailCollection GetAllNonDataModelMDMRuleMapDetails()
        //{
        //    MDMRuleMapDetailCollection results = new MDMRuleMapDetailCollection();

        //    foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
        //    {
        //        if (ruleMapDetail != null && ruleMapDetail.RuleType != MDMRuleType.DataModel)
        //        {
        //            results.Add(ruleMapDetail);
        //        }
        //    }

        //    return results;
        //}

        /// <summary>
        /// Gets MDMRules based on requested MDMRule type
        /// </summary>
        /// <param name="ruleType">Indicates the MDMRule type</param>
        /// <returns>Returns the MDMRules collection object</returns>
        public IMDMRuleCollection GetMDMRulesByRuleType(MDMRuleType ruleType)
        {
            MDMRuleCollection rules = new MDMRuleCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                if (ruleMapDetail != null && ruleMapDetail.RuleType == ruleType)
                {
                    rules.Add(ruleMapDetail.MDMRule);
                }
            }

            return rules;
        }

        /// <summary>
        /// Get all the non business condition rules.
        /// This will returns all the rules except the BC and which are the rules belongs to BC
        /// </summary>
        /// <returns>Returns the MDMRuleMapDetailCollection</returns>
        public MDMRuleMapDetailCollection GetAllNonBusinessConditionRules()
        {
            MDMRuleMapDetailCollection results = new MDMRuleMapDetailCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                if (ruleMapDetail != null && ruleMapDetail.RuleType != MDMRuleType.BusinessCondition && String.IsNullOrWhiteSpace(ruleMapDetail.ParentRuleName) == true)
                {
                    results.Add(ruleMapDetail);
                }
            }

            return results;
        }

        /// <summary>
        /// Get all the business condition rules
        /// </summary>
        /// <returns>Returns the MDMRuleMapDetailCollection</returns>
        public MDMRuleMapDetailCollection GetAllBusinessConditionRules()
        {
            MDMRuleMapDetailCollection results = new MDMRuleMapDetailCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                if (ruleMapDetail != null && ruleMapDetail.RuleType == MDMRuleType.BusinessCondition && String.IsNullOrWhiteSpace(ruleMapDetail.ParentRuleName) == true)
                {
                    results.Add(ruleMapDetail);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns sorted RuleMapRuleDetails
        /// </summary>
        /// <returns>Returns RuleMapRule Collection</returns>
        public MDMRuleMapDetailCollection GetSortedMDMRuleMapDetails()
        {
            MDMRuleMapDetailCollection mdmRuleMapDetailsToReturn = new MDMRuleMapDetailCollection();

            IOrderedEnumerable<MDMRuleMapDetail> sortedMDMRuleMapDetails = this._mdmRuleMapDetailCollection.OrderBy(x => x.Sequence);

            if (sortedMDMRuleMapDetails != null)
            {
                foreach (MDMRuleMapDetail mdmRuleMapDetail in sortedMDMRuleMapDetails)
                {
                    if (mdmRuleMapDetail.ParentRuleId < 1)
                    {
                        mdmRuleMapDetailsToReturn.Add(mdmRuleMapDetail);
                    }
                }
            }

            return mdmRuleMapDetailsToReturn;
        }

        /// <summary>
        /// Get all the Business rule map details by Business condition rule name
        /// </summary>
        /// <param name="bcName">Indicates the Business condition rule name</param>
        /// <returns>Returns the MDMRuleMapDetailCollection</returns>
        public MDMRuleMapDetailCollection GetBusinessRulesByBusinessConditionName(String bcName)
        {
            MDMRuleMapDetailCollection result = new MDMRuleMapDetailCollection();

            foreach (MDMRuleMapDetail ruleMapDetail in this._mdmRuleMapDetailCollection)
            {
                if (ruleMapDetail != null && String.Compare(ruleMapDetail.ParentRuleName, bcName, true) == 0)
                {
                    result.Add(ruleMapDetail);
                }
            }

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private Boolean ContainsByLongName(String longName)
        {
            Boolean result = false;

            if (this._mdmRuleMapDetailCollection != null && this._mdmRuleMapDetailCollection.Count > 0 && String.IsNullOrWhiteSpace(longName) == false)
            {
                foreach (var item in this._mdmRuleMapDetailCollection)
                {
                    if (item != null && String.Compare(item.LongName, longName) == 0)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        #endregion Private Methods

        #endregion Methods
    }
}
