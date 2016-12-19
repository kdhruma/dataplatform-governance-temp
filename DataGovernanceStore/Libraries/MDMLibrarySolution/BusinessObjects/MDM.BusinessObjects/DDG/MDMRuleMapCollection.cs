using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class which contains collection of MDMRuleMaps
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class MDMRuleMapCollection : IMDMRuleMapCollection, ICloneable, IBusinessRuleObjectCollection
    {
        #region Fields

        /// <summary>
        /// Field for MDMRuleMap collection
        /// </summary>
        [DataMember]
        private Collection<MDMRuleMap> _mdmRuleMapCollection = new Collection<MDMRuleMap>();

        #endregion

        #region Properties

        #region IMDMRuleMapCollection Properties

        /// <summary>
        /// Gets the number of elements contained in MDMRuleMapCollection
        /// </summary>
        public Int32 Count
        {
            get { return this._mdmRuleMapCollection.Count; }
        }

        /// <summary>
        /// Check if MDMRuleMapCollection is read-only
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMRuleMapCollection()
        { }

        /// <summary>
        /// Initialize MDMRuleCollection from MDMRuleMapList
        /// </summary>
        /// <param name="mdmRuleMapList">Source items</param>
        public MDMRuleMapCollection(Collection<MDMRuleMap> mdmRuleMapList)
        {
            if (mdmRuleMapList != null)
            {
                this._mdmRuleMapCollection = new Collection<MDMRuleMap>(mdmRuleMapList);
            }
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format
        /// Populates current object using XML
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRuleMapCollection</param>
        public MDMRuleMapCollection(String valuesAsXml)
        {
            LoadMDMRuleMapFromXml(valuesAsXml);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Xml representation of MDMRuleMap Collection
        /// </summary>
        /// <returns>Returns xml representation of MDMRuleMapCollection</returns>
        public String ToXml()
        {
            #region Sample Xml

            // Todo..

            #endregion

            String output = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //MDMRuleMaps node start
                    xmlWriter.WriteStartElement("MDMRuleMaps");

                    foreach (MDMRuleMap mdmRuleMap in _mdmRuleMapCollection)
                    {
                        xmlWriter.WriteRaw(mdmRuleMap.ToXml());
                    }

                    //MDMRuleMaps node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                output = sw.ToString();
            }

            return output;
        }

        /// <summary>
        /// Checks whether actual output is a superset of expected output or not
        /// </summary>
        /// <param name="subsetMDMRuleMaps">Indicates MDMRuleMapCollection to be compare with the current collection</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMRuleMapCollection subsetMDMRuleMaps)
        {
            if (subsetMDMRuleMaps != null)
            {
                foreach (MDMRuleMap subSetMDMRuleMap in subsetMDMRuleMaps)
                {
                    MDMRuleMap sourceMDMRuleMap = this.GetMDMRuleMapByName(subSetMDMRuleMap.Name);

                    //If it doesn't return, that means super set doesn't contain that mdmrule.
                    //So return false, else do further comparison
                    if (sourceMDMRuleMap != null)
                    {
                        if (!sourceMDMRuleMap.IsSuperSetOf(subSetMDMRuleMap))
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
        /// Returns MDMRuleMap based on the referenceId from MDMRuleMapCollection
        /// </summary>
        /// <param name="referenceId">Indicates the referenceId of rulemap</param>
        /// <returns>Returns MDMRuleMap if exists, else null</returns>
        public MDMRuleMap GetMDMRuleMapByReferenceId(Int32 referenceId)
        {
            MDMRuleMap result = null;

            foreach (MDMRuleMap mdmRuleMap in _mdmRuleMapCollection)
            {
                if (mdmRuleMap.ReferenceId == referenceId)
                {
                    result = mdmRuleMap;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Remove MDMRuleMap by Reference Id
        /// </summary>
        /// <param name="referenceId">Indicates the MDMRuleMap Reference Id</param>
        /// <returns>True if MDMRule map is successfully removed; otherwise, false.</returns>
        public Boolean RemoveByReferenceId(Int64 referenceId)
        {
            Boolean result = false;
            foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
            {
                if (ruleMap.ReferenceId == referenceId)
                {
                    result = this.Remove(ruleMap);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get all the application context from the current object
        /// </summary>
        /// <returns>Returns the application context collection</returns>
        public ApplicationContextCollection GetAllApplicationContexts()
        {
            ApplicationContextCollection contexts = new ApplicationContextCollection();

            foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
            {
                if (ruleMap != null && ruleMap.ApplicationContext != null)
                {
                    contexts.Add(ruleMap.ApplicationContext);
                }
            }

            return contexts;
        }

        /// <summary>
        /// Get the MDMRule map by map id
        /// </summary>
        /// <param name="mapId">Indicates the rulemap id</param>
        /// <returns>Returns the MDMRule map object</returns>
        public MDMRuleMap GetMDMRuleMapById(Int32 mapId)
        {
            MDMRuleMap ruleMap = null;

            foreach (MDMRuleMap map in this._mdmRuleMapCollection)
            {
                if (map.Id == mapId)
                {
                    ruleMap = map;
                    break;
                }
            }

            return ruleMap;
        }

        /// <summary>
        /// Get the MDMRule map by mapname
        /// </summary>
        /// <param name="mapName">Indicates the rulemap name</param>
        /// <returns>Returns the MDMRule map object</returns>
        public MDMRuleMap GetMDMRuleMapByName(String mapName)
        {
            MDMRuleMap ruleMap = null;

            foreach (MDMRuleMap map in this._mdmRuleMapCollection)
            {
                if (String.Compare(map.Name, mapName, true) == 0)
                {
                    ruleMap = map;
                    break;
                }
            }

            return ruleMap;
        }

        /// <summary>
        /// Get the MDMRule map by RuleName
        /// </summary>
        /// <param name="ruleName">Indicates the rule name</param>
        /// <returns>Returns the MDMRule map object</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByRuleName(String ruleName)
        {
            MDMRuleMapCollection ruleMaps = new MDMRuleMapCollection();

            foreach (MDMRuleMap map in this._mdmRuleMapCollection)
            {
                Collection<String> mdmRuleNames = map.GetAllBusinessRulesAndBusinessConditionsNames();

                if (mdmRuleNames.Contains(ruleName))
                {
                    ruleMaps.Add(map);
                }
            }

            return ruleMaps;
        }

        /// <summary>
        /// Gets the MDMRuleMaps based on workflow name
        /// </summary>
        /// <param name="workflowName">Indicates the workflow name</param>
        /// <returns>Returns the MDMRuleMapCollection.</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByWorkflowName(String workflowName)
        {
            MDMRuleMapCollection ruleMaps = null;

            if (!String.IsNullOrWhiteSpace(workflowName))
            {
                ruleMaps = new MDMRuleMapCollection();

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    if (String.Compare(ruleMap.WorkflowInfo.WorkflowName, workflowName, true) == 0)
                    {
                        ruleMaps.Add(ruleMap.Clone());
                    }
                }
            }

            return ruleMaps;
        }

        /// <summary>
        /// Gets the MDMRuleMaps based on workflow activity name
        /// </summary>
        /// <param name="workflowActivityName">Indicates the workflow activity name</param>
        /// <returns>Returns the MDMRuleMapCollection.</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByWorkflowActivityName(String workflowActivityName)
        {
            MDMRuleMapCollection ruleMaps = null;

            if (!String.IsNullOrWhiteSpace(workflowActivityName))
            {
                ruleMaps = new MDMRuleMapCollection();

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    if (String.Compare(ruleMap.WorkflowInfo.WorkflowActivityShortName, workflowActivityName, true) == 0)
                    {
                        ruleMaps.Add(ruleMap.Clone());
                    }
                }
            }

            return ruleMaps;
        }

        /// <summary>
        /// Gets the MDMRuleMaps based on workflow activity long name
        /// </summary>
        /// <param name="workflowActivityLongName">Indicates the workflow activity long name</param>
        /// <returns>Returns the MDMRuleMapCollection.</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByWorkflowActivityLongName(String workflowActivityLongName)
        {
            MDMRuleMapCollection ruleMaps = null;

            if (!String.IsNullOrWhiteSpace(workflowActivityLongName))
            {
                ruleMaps = new MDMRuleMapCollection();

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    if (String.Compare(ruleMap.WorkflowInfo.WorkflowActivityShortName, workflowActivityLongName, true) == 0)
                    {
                        ruleMaps.Add(ruleMap.Clone());
                    }
                }
            }

            return ruleMaps;
        }

        /// <summary>
        /// Gets the MDMRuleMaps based on workflow action
        /// </summary>
        /// <param name="workflowAction">Indicates the workflow action</param>
        /// <returns>Returns the MDMRuleMapCollection.</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByWorkflowAction(String workflowAction)
        {
            MDMRuleMapCollection ruleMaps = null;

            if (!String.IsNullOrWhiteSpace(workflowAction))
            {
                ruleMaps = new MDMRuleMapCollection();

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    if (String.Compare(ruleMap.WorkflowInfo.WorkflowActivityAction, workflowAction, true) == 0)
                    {
                        ruleMaps.Add(ruleMap.Clone());
                    }
                }
            }

            return ruleMaps;
        }

        /// <summary>
        /// Returns MDMRuleMaps based on the rule id from MDMRuleMapCollection
        /// </summary>
        /// <param name="mdmRuleIds">Indicates the MDMRule Id</param>
        /// <returns>Returns MDMRuleMaps if exists, else null</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByRuleIds(Collection<Int32> mdmRuleIds)
        {
            MDMRuleMapCollection filteredRuleMaps = null;

            if (mdmRuleIds != null && mdmRuleIds.Count > 0 && this._mdmRuleMapCollection != null)
            {
                filteredRuleMaps = new MDMRuleMapCollection();
                Collection<Int32> ruleIds = new Collection<Int32>(mdmRuleIds);

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    MDMRuleMapRule requestedRuleMapRule = null;
                    MDMRuleMapRuleCollection mdmRuleMapRuleCollection = new MDMRuleMapRuleCollection();

                    foreach (Int32 ruleId in mdmRuleIds)
                    {
                        requestedRuleMapRule = ruleMap.MDMRuleMapRules.GetMDMRuleMapRuleByRuleMapIdAndRuleId(ruleMap.Id, ruleId) as MDMRuleMapRule;

                        if (requestedRuleMapRule != null)
                        {
                            mdmRuleMapRuleCollection.Add(requestedRuleMapRule);
                        }
                    }

                    if (mdmRuleMapRuleCollection.Count > 0)
                    {
                        filteredRuleMaps.Add(ruleMap);
                    }
                }
            }

            return filteredRuleMaps;

        }

        /// <summary>
        /// Returns MDMRuleMaps based on the event Ids from MDMRuleMapCollection
        /// </summary>
        /// <param name="eventIds">Indicates list of event Ids</param>
        /// <returns>Returns MDMRuleMaps if exists, else null</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByEventIds(Collection<Int32> eventIds)
        {
            MDMRuleMapCollection filteredRuleMaps = null;

            if (eventIds != null && eventIds.Count > 0 && this._mdmRuleMapCollection != null)
            {
                filteredRuleMaps = new MDMRuleMapCollection();
                Collection<Int32> mdmEventIds = new Collection<Int32>(eventIds);

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    if (eventIds.Contains(ruleMap.EventId))
                    {
                        filteredRuleMaps.Add(ruleMap);
                        eventIds.Remove(ruleMap.EventId);
                    }

                    if (mdmEventIds.Count == 0)
                    {
                        break;
                    }
                }
            }

            return filteredRuleMaps;
        }

        /// <summary>
        /// Returns MDMRuleMaps based on the rule map ids from MDMRuleMapCollection
        /// </summary>
        /// <param name="mdmRuleMapIds">Indicates the list of MDMRule map Ids</param>
        /// <returns>Returns MDMRuleMaps if exists, else null</returns>
        public MDMRuleMapCollection GetMDMRuleMapsByRuleMapIds(Collection<Int32> mdmRuleMapIds)
        {
            MDMRuleMapCollection filteredRuleMaps = null;

            if (mdmRuleMapIds != null && mdmRuleMapIds.Count > 0 && this._mdmRuleMapCollection != null)
            {
                filteredRuleMaps = new MDMRuleMapCollection();
                Collection<Int32> ruleMapIds = new Collection<Int32>(mdmRuleMapIds);

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    if (ruleMapIds.Contains(ruleMap.Id))
                    {
                        filteredRuleMaps.Add(ruleMap);
                        ruleMapIds.Remove(ruleMap.Id);
                    }

                    if (ruleMapIds.Count == 0)
                    {
                        break;
                    }
                }
            }

            return filteredRuleMaps;
        }

        /// <summary>
        /// Gets all the MDMRule Ids 
        /// </summary>
        /// <returns>Returns the MDMRule Ids</returns>
        public Collection<Int32> GetMDMRuleIds()
        {
            Collection<Int32> ruleIds = null;

            if (this._mdmRuleMapCollection != null)
            {
                ruleIds = new Collection<Int32>();

                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    Collection<Int32> mdmRuleIds = ruleMap.GetAllBusinessRulesAndBusinessConditionsIds();

                    if (mdmRuleIds != null && mdmRuleIds.Count > 0)
                    {
                        foreach (Int32 mdmRuleId in mdmRuleIds)
                        {
                            if (!ruleIds.Contains(mdmRuleId))
                            {
                                ruleIds.Add(mdmRuleId);
                            }
                        }
                    }
                }
            }

            return ruleIds;
        }

        #endregion

        #region ICollection<MDMRuleMap> Members

        /// <summary>
        /// Adds the specified mdmRuleMap
        /// </summary>
        /// <param name="mdmRuleMap">mdmrulemap object</param>
        public void Add(MDMRuleMap mdmRuleMap)
        {
            this._mdmRuleMapCollection.Add(mdmRuleMap);
        }

        /// <summary>
        /// Adds the mdmRuleMaps into existing collection
        /// </summary>
        /// <param name="mdmRuleMaps">mdmRuleMaps object</param>
        public void AddRange(MDMRuleMapCollection mdmRuleMaps)
        {
            if (mdmRuleMaps != null && mdmRuleMaps.Count > 0)
            {
                foreach (MDMRuleMap mdmRuleMap in mdmRuleMaps)
                {
                    this._mdmRuleMapCollection.Add(mdmRuleMap);
                }
            }
        }

        /// <summary>
        /// Removes all items from the current MDMRuleMapCollection
        /// </summary>
        public void Clear()
        {
            this._mdmRuleMapCollection.Clear();
        }

        /// <summary>
        /// Determines whether the current MDMRuleMapCollection contains the specified mdmRuleMap.
        /// </summary>
        /// <param name="mdmRuleMap">RuleMap to be verified</param>
        /// <returns>True if found in collection else false</returns>
        public Boolean Contains(MDMRuleMap mdmRuleMap)
        {
            return this._mdmRuleMapCollection.Contains(mdmRuleMap);
        }

        /// <summary>
        /// Determines whether the current MDMRuleMapCollection contains mdmRuleMap with the specified name.
        /// </summary>
        /// <param name="mdmRuleMapName">RuleMapName to be verified</param>
        /// <returns>True if found in collection else false</returns>
        public Boolean Contains(String mdmRuleMapName)
        {
            Boolean containsMapName = false;

            if (!String.IsNullOrWhiteSpace(mdmRuleMapName))
            {
                foreach (MDMRuleMap mdmRuleMap in this._mdmRuleMapCollection)
                {
                    if (String.Compare(mdmRuleMap.Name, mdmRuleMapName, true) == 0)
                    {
                        containsMapName = true;
                        break;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("mdmRuleMapName", "Parameter MDMRuleMap name should not null or empty.");
            }

            return containsMapName;
        }

        /// <summary>
        /// Copies the elements of the MDMRuleMapCollection to an System.Array, starting at a particular System.Array index
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied from MDMRuleMapCollection. The System.Array must have zero-based indexing
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(MDMRuleMap[] array, int arrayIndex)
        {
            this._mdmRuleMapCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the MDMRuleMapCollection.
        /// </summary>
        /// <param name="mdmRuleMap">RuleMap to be removed from the MDMRuleMapCollection</param>
        /// <returns>True if mdmrule is successfully removed; otherwise, false.</returns>
        public bool Remove(MDMRuleMap mdmRuleMap)
        {
            return this._mdmRuleMapCollection.Remove(mdmRuleMap);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<MDMRuleMap> GetEnumerator()
        {
            return this._mdmRuleMapCollection.GetEnumerator();
        }

        #endregion ICollection<MDMRuleMap> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection
        /// </summary>
        /// <returns>An enumerator object that can be used to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._mdmRuleMapCollection.GetEnumerator();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads MDMRuleMap Collection from xml
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRuleCollection</param>
        private void LoadMDMRuleMapFromXml(String valuesAsXml)
        {
            #region Sample Xml

            // Todo..

            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    if (reader != null)
                    {
                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMap")
                            {
                                String mdmRuleMapXml = reader.ReadOuterXml();

                                if (!String.IsNullOrEmpty(mdmRuleMapXml))
                                {
                                    MDMRuleMap mdmRuleMap = new MDMRuleMap(mdmRuleMapXml);

                                    if (mdmRuleMap != null)
                                    {
                                        this.Add(mdmRuleMap);
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
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleMap collection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleMap collection object</returns>
        public MDMRuleMapCollection Clone()
        {
            MDMRuleMapCollection clonedCollection = new MDMRuleMapCollection();
            if (this._mdmRuleMapCollection != null && this._mdmRuleMapCollection.Count > 0)
            {
                foreach (MDMRuleMap ruleMap in this._mdmRuleMapCollection)
                {
                    clonedCollection.Add(ruleMap.Clone());
                }
            }
            return clonedCollection;
        }

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleMap collection object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleMap collection object</returns>

        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #endregion Methods
    }
}