using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contains collection of MDMRuleBusinessConditionRule
    /// </summary>
    [DataContract]
    public class MDMRuleBusinessConditionRuleCollection : InterfaceContractCollection<IMDMRuleBusinessConditionRule, MDMRuleBusinessConditionRule>, IMDMRuleBusinessConditionRuleCollection, IBusinessRuleObjectCollection
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MDMRuleBusinessConditionRuleCollection()
        {
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="valuesAsXml">Indicates the MDMRuleBusinessConditionRule as Xml</param>
        public MDMRuleBusinessConditionRuleCollection(String valuesAsXml)
        {
            LoadMDMRuleBusinessConditionRule(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        #region Override Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object
        /// </summary>
        /// <param name="obj">MDMRuleBusinessConditionRuleCollection object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRuleBusinessConditionRuleCollection)
            {
                MDMRuleBusinessConditionRuleCollection objectToBeCompared = obj as MDMRuleBusinessConditionRuleCollection;
                Int32 mdmRuleBusinessConditionRulesUnion = this._items.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 mdmRuleBusinessConditionRulesIntersect = this._items.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (mdmRuleBusinessConditionRulesUnion != mdmRuleBusinessConditionRulesIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            int hashCode = 0;

            hashCode = base.GetHashCode();

            foreach (MDMRuleBusinessConditionRule mdmRuleBusinessConditionRule in this._items)
            {
                hashCode += mdmRuleBusinessConditionRule.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Gets Xml representation for MDMRuleMap
        /// </summary>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // MDMRuleBusinessConditionRules node start
                    xmlWriter.WriteStartElement("MDMRuleBusinessConditionRules");

                    foreach (MDMRuleBusinessConditionRule mdmRuleBusinessConditionRule in this._items)
                    {
                        xmlWriter.WriteRaw(mdmRuleBusinessConditionRule.ToXml());
                    }

                    // MDMRuleBusinessConditionRules node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        #endregion Override Methods

        #region IBusinessRuleObjectCollection Members

        /// <summary>
        /// Remove businessconditionrule from collection by reference id
        /// </summary>
        /// <param name="referenceId">Indicates the reference id</param>
        /// <returns>Returns true if businessconditionrule is successfully removed from the collection</returns>
        public Boolean RemoveByReferenceId(Int64 referenceId)
        {
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator object that can be used to iterate through the collection.</returns>
        public new IEnumerator GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        #endregion IBusinessRuleObjectCollection Members

        #endregion Public Methods

        #region Private Methods

        private void LoadMDMRuleBusinessConditionRule(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleBusinessConditionRule")
                        {
                            String mdmRuleBusinessConditionRuleXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(mdmRuleBusinessConditionRuleXml))
                            {
                                MDMRuleBusinessConditionRule mdmRuleBusinessConditionRule = new MDMRuleBusinessConditionRule(mdmRuleBusinessConditionRuleXml);

                                if (mdmRuleBusinessConditionRuleXml != null)
                                {
                                    this.Add(mdmRuleBusinessConditionRule);
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

        #endregion Private Methods

        #endregion Methods
    }
}
