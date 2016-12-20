using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies the TrackedActivityInfo Object Collection
    /// </summary>
    [DataContract]
    public class TrackedActivityInfoCollection : ICollection<TrackedActivityInfo>, IEnumerable<TrackedActivityInfo>, ITrackedActivityInfoCollection
    {
        #region Fields

        [DataMember]
        private Collection<TrackedActivityInfo> _trackedActivityInfoCollection = new Collection<TrackedActivityInfo>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public TrackedActivityInfoCollection() : base() { }

        /// <summary>
        /// Initializes a new instance of the Tracked ActivityInfo collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public TrackedActivityInfoCollection(String valuesAsXml)
        {
            /*
              <TrackedActivityInfoCollection>
                  <TrackedActivityInfo Id="-1" WorkflowVersionId="1" WorkflowDefinitionActivityId="" ActivityShortName="a0b77d5c-23b4-4730-905e-701e93bb04c5" ActivityLongName="Content Enrichment" RuntimeInstanceId="777832b5-6a54-4f4e-8029-3166a0ad0848" ExtendedProperties="" Status="Closed" ActingUserId="-1" ActedUserId="48" Comments="" Variables="" Arguments="" CustomData="" AssignedUsers="" AssignedRoles="" AssignementType="0" SortOrder="0" IsHumanActivity="False" WorkflowId="1" WorkflowName="NPI" IsExecuting="False" PerformedAction="Done" WorkflowVersionName="NPI 1.0" ActedUser="michael" EventDate="2/21/2013 7:43:35 AM" ActingUser="" UserMailAddress="vijay@riversand.com">
                    <WorkflowMDMObjects></WorkflowMDMObjects>
                  </TrackedActivityInfo>
                  <TrackedActivityInfo Id="-1" WorkflowVersionId="1" WorkflowDefinitionActivityId="" ActivityShortName="0b07eb27-3077-4447-9c85-55bf4f094ffe" ActivityLongName="Buyer Review" RuntimeInstanceId="777832b5-6a54-4f4e-8029-3166a0ad0848" ExtendedProperties="" Status="Executing" ActingUserId="0" ActedUserId="0" Comments="" Variables="" Arguments="" CustomData="" AssignedUsers="" AssignedRoles="" AssignementType="0" SortOrder="0" IsHumanActivity="False" WorkflowId="1" WorkflowName="NPI" IsExecuting="False" PerformedAction="" WorkflowVersionName="NPI 1.0" ActedUser="" EventDate="2/21/2013 7:43:37 AM" ActingUser="" UserMailAddress="">
                    <WorkflowMDMObjects></WorkflowMDMObjects>
                  </TrackedActivityInfo>
                  <TrackedActivityInfo Id="-1" WorkflowVersionId="1" WorkflowDefinitionActivityId="" ActivityShortName="7f126297-fe45-4853-997f-d8a24425107a" ActivityLongName="Marketing Enrichment" RuntimeInstanceId="777832b5-6a54-4f4e-8029-3166a0ad0848" ExtendedProperties="" Status="Closed" ActingUserId="-1" ActedUserId="48" Comments="" Variables="" Arguments="" CustomData="" AssignedUsers="" AssignedRoles="" AssignementType="0" SortOrder="0" IsHumanActivity="False" WorkflowId="1" WorkflowName="NPI" IsExecuting="False" PerformedAction="Done" WorkflowVersionName="NPI 1.0" ActedUser="michael" EventDate="2/21/2013 7:44:07 AM" ActingUser="" UserMailAddress="vijay@riversand.com">
                    <WorkflowMDMObjects></WorkflowMDMObjects>
                  </TrackedActivityInfo>
                  <TrackedActivityInfo Id="-1" WorkflowVersionId="1" WorkflowDefinitionActivityId="" ActivityShortName="8758fc7b-107f-4354-8cb5-07bed6c68d0a" ActivityLongName="Marketing Review" RuntimeInstanceId="777832b5-6a54-4f4e-8029-3166a0ad0848" ExtendedProperties="" Status="Executing" ActingUserId="48" ActedUserId="0" Comments="" Variables="" Arguments="" CustomData="" AssignedUsers="" AssignedRoles="" AssignementType="0" SortOrder="0" IsHumanActivity="False" WorkflowId="1" WorkflowName="NPI" IsExecuting="False" PerformedAction="" WorkflowVersionName="NPI 1.0" ActedUser="" EventDate="2/21/2013 7:44:09 AM" ActingUser="michael" UserMailAddress="vijay@riversand.com">
                    <WorkflowMDMObjects></WorkflowMDMObjects>
                  </TrackedActivityInfo>
                </TrackedActivityInfoCollection>
             * */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "TrackedActivityInfoCollection")
                    {
                        String TrackedActivityInfoXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(TrackedActivityInfoXml))
                        {
                            TrackedActivityInfo trackedActivityInfo = new TrackedActivityInfo(TrackedActivityInfoXml);

                            if (trackedActivityInfo != null)
                                this._trackedActivityInfoCollection.Add(trackedActivityInfo);
                        }
                    }
                    else
                    {
                        reader.Read();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion

        #region ICollection<TrackedActivityInfo> Members

        /// <summary>
        /// Add TrackedActivityInfo object in collection
        /// </summary>
        /// <param name="item">TrackedActivityInfo to add in collection</param>
        public void Add(TrackedActivityInfo item)
        {
            this._trackedActivityInfoCollection.Add(item);
        }

        /// <summary>
        /// Add ITrackedActivityInfo object in collection
        /// </summary>
        /// <param name="item">ITrackedActivityInfo to add in collection</param>
        public void Add(ITrackedActivityInfo item)
        {
            this._trackedActivityInfoCollection.Add((TrackedActivityInfo)item);
        }

        /// <summary>
        /// Removes all TrackedActivityInfo from collection
        /// </summary>
        public void Clear()
        {
            this._trackedActivityInfoCollection.Clear();
        }

        /// <summary>
        /// Determines whether the TrackedActivityInfoCollection contains a specific TrackedActivityInfo.
        /// </summary>
        /// <param name="item">The TrackedActivityInfo object to locate in the TrackedActivityInfoCollection.</param>
        /// <returns>
        /// <para>true : If TrackedActivityInfo found in TrackedActivityInfoCollection</para>
        /// <para>false : If TrackedActivityInfo found not in TrackedActivityInfoCollection</para>
        /// </returns>
        public bool Contains(TrackedActivityInfo item)
        {
            return this._trackedActivityInfoCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the TrackedActivityInfoCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from TrackedActivityInfoCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(TrackedActivityInfo[] array, int arrayIndex)
        {
            this._trackedActivityInfoCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of TrackedActivityInfos in TrackedActivityInfoCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._trackedActivityInfoCollection.Count;
            }
        }

        /// <summary>
        /// Check if TrackedActivityInfoCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the TrackedActivityInfoCollection.
        /// </summary>
        /// <param name="item">The TrackedActivityInfo object to remove from the TrackedActivityInfoCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original TrackedActivityInfoCollection</returns>
        public bool Remove(TrackedActivityInfo item)
        {
            return this._trackedActivityInfoCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<TrackedActivityInfo> Members

        /// <summary>
        /// Returns an enumerator that iterates through a TrackedActivityInfoCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<TrackedActivityInfo> GetEnumerator()
        {
            return this._trackedActivityInfoCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a TrackedActivityInfoCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._trackedActivityInfoCollection.GetEnumerator();
        }

        #endregion

        #region TrackedActivityInfoCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of TrackedActivityInfoCollection object
        /// </summary>
        /// <returns>Xml string representing the TrackedActivityInfoCollection</returns>
        public String ToXml()
        {
            String trackedActivityInfoXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (TrackedActivityInfo trackedActivityInfo in this._trackedActivityInfoCollection)
            {
                builder.Append(trackedActivityInfo.ToXml());
            }

            trackedActivityInfoXml = String.Format("<TrackedActivityInfoCollection>{0}</TrackedActivityInfoCollection>", builder.ToString());
            return trackedActivityInfoXml;
        }

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <param name="activityDetailOnly">If it is false then return object and collection details else only object detail </param>
        /// <returns>Xml representation of the object</returns>
        public String ToXml(ObjectSerialization objectSerialization, bool activityDetailOnly)
        {
            String trackedActivityInfoXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (TrackedActivityInfo trackedActivityInfo in this._trackedActivityInfoCollection)
            {
                builder.Append(trackedActivityInfo.ToXml(objectSerialization, activityDetailOnly));
            }

            trackedActivityInfoXml = String.Format("<TrackedActivityInfoCollection>{0}</TrackedActivityInfoCollection>", builder.ToString());
            return trackedActivityInfoXml;
        }

        #endregion ToXml methods

        #endregion TrackedActivityInfoCollection Memebers
    }
}
