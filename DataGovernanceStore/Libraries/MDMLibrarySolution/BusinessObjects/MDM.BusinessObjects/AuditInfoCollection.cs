using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents class for collection of audit information object
    /// </summary>
    [DataContract]
    public class AuditInfoCollection : ICollection<AuditInfo>, IEnumerable<AuditInfo>, IAuditInfoCollection
    {
        #region Fields

        /// <summary>
        /// Field denoting Collection of AuditInfo Object
        /// </summary>
        [DataMember]
        private Collection<AuditInfo> _auditInfos = new Collection<AuditInfo>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AuditInfoCollection() : base() { }

        /// <summary>
        /// Create AuditInfo object with property values xml as input parameter
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// <AuditInfos>
        ///     <AuditInfo Id="001" ProgramName="BusinessRule.100.1" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
        ///     <AuditInfo Id="002" ProgramName="BusinessRule.100.2" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
        ///     <AuditInfo Id="003" ProgramName="BusinessRule.100.3" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
        /// </AuditInfos>
        /// ]]>
        /// </example>
        /// <param name="valuesAsXml">XML representation for AuditInfo from which object is to be created</param>
        public AuditInfoCollection(String valuesAsXml)
        {
            LoadAuditInfoCollection(valuesAsXml);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Create AuditInfo object with property values xml as input parameter
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// <AuditInfos>
        ///     <AuditInfo Id="001" ProgramName="BusinessRule.100.1" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
        ///     <AuditInfo Id="002" ProgramName="BusinessRule.100.2" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
        ///     <AuditInfo Id="003" ProgramName="BusinessRule.100.3" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
        /// </AuditInfos>
        /// ]]>
        /// </example>
        /// <param name="valuesAsXml">XML representation for AuditInfo from which object is to be created</param>
        public void LoadAuditInfoCollection(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <AuditInfos>
               <AuditInfo Id="001" ProgramName="BusinessRule.100.1" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
               <AuditInfo Id="002" ProgramName="BusinessRule.100.2" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
               <AuditInfo Id="003" ProgramName="BusinessRule.100.3" UserLogin="cfadmin" ChangeDateTime="" Action="Read" Locale="en-WW" />
             </AuditInfos>
            */
            #endregion

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AuditInfo")
                        {
                            String auditInfoXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(auditInfoXml))
                            {
                                AuditInfo auditInfo = new AuditInfo(auditInfoXml);
                                if (auditInfo != null)
                                {
                                    this.Add(auditInfo);
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

        #region IAuditInfoCollection

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is AuditInfoCollection)
            {
                AuditInfoCollection objectToBeCompared = obj as AuditInfoCollection;
                Int32 auditInfoUnion = this._auditInfos.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 auditInfoIntersect = this._auditInfos.ToList().Intersect(objectToBeCompared.ToList()).Count();
                if (auditInfoUnion != auditInfoIntersect)
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
            foreach (AuditInfo auditInfo in this._auditInfos)
            {
                hashCode += auditInfo.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of AuditInfoCollection object
        /// </summary>
        /// <returns>Xml string representing the AuditInfoCollection</returns>
        public String ToXml()
        {
            String auditInfoXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (AuditInfo auditInfo in this._auditInfos)
            {
                builder.Append(auditInfo.ToXml());
            }

            auditInfoXml = String.Format("<AuditInfos>{0}</AuditInfos>", builder.ToString());

            return auditInfoXml;
        }

        /// <summary>
        /// Get Xml representation of AuditInfoCollection object
        /// </summary>
        /// <param name="objectSerialization">Indicates the serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String auditInfoXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (AuditInfo auditInfo in this._auditInfos)
            {
                builder.Append(auditInfo.ToXml(objectSerialization));
            }

            auditInfoXml = String.Format("<AuditInfos>{0}</AuditInfos>", builder.ToString());

            return auditInfoXml;
        }

        #endregion

        #region ICollection<AuditInfo>

        /// <summary>
        /// Add AuditInfo object in collection
        /// </summary>
        /// <param name="item">AuditInfo to add in collection</param>
        public void Add(AuditInfo item)
        {
            this._auditInfos.Add(item);
        }

        /// <summary>
        /// Removes all AuditInfo from collection
        /// </summary>
        public void Clear()
        {
            this._auditInfos.Clear();
        }

        /// <summary>
        /// Determines whether the AuditInfoCollection contains a specific AuditInfo.
        /// </summary>
        /// <param name="item">The AuditInfo object to locate in the AuditInfoCollection.</param>
        /// <returns>
        /// <para>true : If AuditInfo found in mappingCollection</para>
        /// <para>false : If AuditInfo found not in mappingCollection</para>
        /// </returns>
        public bool Contains(AuditInfo item)
        {
            return this._auditInfos.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the AuditInfoCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from AuditInfoCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(AuditInfo[] array, int arrayIndex)
        {
            this._auditInfos.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of AuditInfo in AuditInfoCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._auditInfos.Count;
            }
        }

        /// <summary>
        /// Check if AuditInfoCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the AuditInfoCollection.
        /// </summary>
        /// <param name="item">The Locale object to remove from the AuditInfoCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original AuditInfoCollection</returns>
        public bool Remove(AuditInfo item)
        {
            return this._auditInfos.Remove(item);
        }

        #endregion

        #region IEnumerable<AuditInfo>

        /// <summary>
        /// Returns an enumerator that iterates through a AuditInfoCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AuditInfo> GetEnumerator()
        {
            return this._auditInfos.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a AuditInfoCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._auditInfos.GetEnumerator();
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Get audit information based on current user login
        /// </summary>
        /// <param name="userLogin">Indicates the user login based on which the audit information to be returned</param>
        /// <returns>Returns audit information based on current user login</returns>
        public AuditInfoCollection GetByUser(String userLogin)
        {
            //Return AuditInfo which has UserLogin as given userLogin
            AuditInfoCollection filteredAuditInfo = new AuditInfoCollection();

            IEnumerable<AuditInfo> auditInfo = from audit in this._auditInfos
                                               where audit.UserLogin == userLogin
                                               select audit;

            if (auditInfo != null)
            {
                filteredAuditInfo = (AuditInfoCollection)auditInfo;
            }

            return filteredAuditInfo;
        }

        /// <summary>
        /// Get audit information based on action
        /// </summary>
        /// <param name="action">Indicates the action based on which the audit information to be returned</param>
        /// <returns>Returns audit information based on action</returns>
        public AuditInfoCollection GetByAction(ObjectAction action)
        {
            //Return AuditInfo which has action as given action
            AuditInfoCollection filteredAuditInfo = new AuditInfoCollection();

            IEnumerable<AuditInfo> auditInfo = from audit in this._auditInfos
                                               where audit.Action == action
                                               select audit;

            if (auditInfo != null)
            {
                filteredAuditInfo = (AuditInfoCollection)auditInfo;
            }

            return filteredAuditInfo;
        }

        /// <summary>
        /// Get audit information based on locale
        /// </summary>
        /// <param name="locale">Indicates the locale based on which the audit information to be returned</param>
        /// <returns>Returns audit information based on locale</returns>
        public AuditInfoCollection GetByLocale(LocaleEnum locale)
        {
            //Return AuditInfo which has locale as given locale
            AuditInfoCollection filteredAuditInfo = new AuditInfoCollection();

            IEnumerable<AuditInfo> auditInfo = from audit in this._auditInfos
                                               where audit.Locale == locale
                                               select audit;

            if (auditInfo != null)
            {
                filteredAuditInfo = (AuditInfoCollection)auditInfo;
            }

            return filteredAuditInfo;
        }

        /// <summary>
        /// Get the latest audit information from the list after sorting all audit info in the collection
        /// </summary>
        /// <returns>Returns the latest audit information from the list after sorting all audit info in the collection</returns>
        public AuditInfo GetLatest()
        {
            //sort all the audit info in the collection and return the latest audit info from the list
            AuditInfo filteredAuditInfo = new AuditInfo();

            IEnumerable<AuditInfo> auditInfo = from audit in this._auditInfos
                                               orderby audit.ChangeDateTime descending
                                               select audit;

            if (auditInfo != null)
            {
                filteredAuditInfo = auditInfo.FirstOrDefault();
            }

            return filteredAuditInfo;
        }

        /// <summary>
        /// Determine whether the audit info has changed after the specified date and time
        /// </summary>
        /// <param name="dateTime">Indicates the date and time based on which audit information modification to be determined</param>
        /// <returns>Returns true if the audit info has changed after the specified date and time; otherwise false</returns>
        public Boolean HasModifiedAfter(DateTime dateTime)
        {
            //Return true if we have modification after given Datetime
            Boolean hasModified = false;

            IEnumerable<AuditInfo> auditInfo = from audit in this._auditInfos
                                               where audit.ChangeDateTime > dateTime
                                               select audit;
            if (auditInfo != null && auditInfo.Any())
            {
                hasModified = true;
            }

            return hasModified;
        }

        /// <summary>
        /// Determine whether the audit info has changed before the specified date and time
        /// </summary>
        /// <param name="dateTime">Indicates the date and time based on which audit information modification to be determined</param>
        /// <returns>Returns true if the audit info has changed before the specified date and time; otherwise false</returns>
        public Boolean HasModifiedBefore(DateTime dateTime)
        {
            //Return true if we have modification after given Datetime
            Boolean hasModified = false;

            IEnumerable<AuditInfo> auditInfo = from audit in this._auditInfos
                                               where audit.ChangeDateTime < dateTime
                                               select audit;
            if (auditInfo != null && auditInfo.Any())
            {
                hasModified = true;
            }

            return hasModified;
        }

        /// <summary>
        /// Determine whether audit information has been modified based on its action.
        /// </summary>
        /// <returns>Returns true if any audit information object has action as update or delete; otherwise false</returns>
        public Boolean HasModification()
        {
            Boolean hasModified = false;

            IEnumerable<AuditInfo> auditInfo = from audit in this._auditInfos
                                               where (audit.Action == ObjectAction.Update || audit.Action == ObjectAction.Delete)
                                               select audit;

            if (auditInfo != null && auditInfo.Any())
            {
                hasModified = true;
            }

            return hasModified;
        }

        #endregion
    }
}
