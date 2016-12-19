using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent Collection of ScheduleCriteria Object
    /// </summary>
    [DataContract]
    public class ScheduleCriteriaCollection : InterfaceContractCollection<IScheduleCriteria, ScheduleCriteria>, IScheduleCriteriaCollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ScheduleCriteria Collection
        /// </summary>
        public ScheduleCriteriaCollection() { }

        /// <summary>
        /// Initialize subscriber from Xml value
        /// </summary>
        /// <param name="valuesAsXml">Export subscriber in xml format</param>
        public ScheduleCriteriaCollection(String valuesAsXml)
        {
            LoadScheduleCriteria(valuesAsXml);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if ScheduleCriteriaCollection contains ScheduleCriteria with given Id
        /// </summary>
        /// <param name="Id">Id using which ScheduleCriteria is to be searched from collection</param>
        /// <returns>
        /// <para>true : If ScheduleCriteria found in ScheduleCriteriaCollection</para>
        /// <para>false : If ScheduleCriteria found not in ScheduleCriteriaCollection</para>
        /// </returns>
        public bool Contains(Int32 Id)
        {
            if (GetScheduleCriteria(Id) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove echeduleCriteria object from ScheduleCriteriaCollection
        /// </summary>
        /// <param name="echeduleCriteriaId">echeduleCriteriaId of echeduleCriteria which is to be removed from collection</param>
        /// <returns>true if echeduleCriteria is successfully removed; otherwise, false. This method also returns false if echeduleCriteria was not found in the original collection</returns>
        public bool Remove(Int32 echeduleCriteriaId)
        {
            IScheduleCriteria echeduleCriteria = GetScheduleCriteria(echeduleCriteriaId);

            if (echeduleCriteria == null)
                throw new ArgumentException("No ScheduleCriteria found for given Id :" + echeduleCriteriaId);
            else
                return this.Remove(echeduleCriteria);
        }

        /// <summary>
        /// Get schedule criteria based on id
        /// </summary>
        /// <param name="Id">Indicates the identifier based on which schedule criteria is returned</param>
        /// <returns>Returns schedule criteria based on id</returns>
        public IScheduleCriteria GetScheduleCriteria(Int32 Id)
        {
            var filteredScheduleCriteria = from scheduleCriteria in this._items
                                           where scheduleCriteria.Id == Id
                                           select scheduleCriteria;

            if (filteredScheduleCriteria.Any())
                return filteredScheduleCriteria.First();
            else
                return null;
        }

        /// <summary>
        /// Get Xml representation of ScheduleCriteriaCollection
        /// </summary>
        /// <returns>Xml representation of ScheduleCriteriaCollection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<ScheduleCriterias>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ScheduleCriteria criteria in this._items)
                {
                    returnXml = String.Concat(returnXml, criteria.ToXml());
                }
            }

            returnXml = String.Concat(returnXml, "</ScheduleCriterias>");

            return returnXml;
        }

        /// <summary>
        /// Get Xml representation of ScheduleCriteriaCollection
        /// </summary>
        /// <returns>Xml representation of ScheduleCriteriaCollection</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            String returnXml = String.Empty;

            returnXml = "<ScheduleCriterias>";

            if (this._items != null && this._items.Count > 0)
            {
                foreach (ScheduleCriteria criteria in this._items)
                {
                    returnXml = String.Concat(returnXml, criteria .ToXml(serialization));
                }
            }

            returnXml = String.Concat(returnXml, "</ScheduleCriterias>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ScheduleCriteriaCollection)
            {
                ScheduleCriteriaCollection objectToBeCompared = obj as ScheduleCriteriaCollection;

                Int32 criteriasUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 criteriasIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();

                if (criteriasUnion != criteriasIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            Int32 hashCode = 0;

            foreach (ScheduleCriteria scheduleCriteria in this._items)
            {
                hashCode += scheduleCriteria.GetHashCode();
            }

            return hashCode;
        }

        #endregion

        #region Private Methods

        private void LoadScheduleCriteria(String valuesAsXml)
        {
            #region Sample Xml
            /*
			 * <ScheduleCriterias>
                <ScheduleCriteria Id="1" Name="Export Drop" SubscriberType="Unknown">
                  <ConfigurationParameters>
                    <ConfigurationParameter Key="Directory" Value="\\RST1061\Export Drop" />
                  </ConfigurationParameters>
                </ScheduleCriteria>
              </ScheduleCriterias>
			 */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ScheduleCriteria")
                        {
                            String subscriberXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(subscriberXml))
                            {
                                ScheduleCriteria echeduleCriteria = new ScheduleCriteria(subscriberXml);
                                if (echeduleCriteria != null)
                                {
                                    this.Add(echeduleCriteria);
                                }
                            }

                        }
                        else
                        {
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

        #endregion
    }
}
