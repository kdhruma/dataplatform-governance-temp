using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the WorkflowState Object Collection
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(WorkflowState))]
    public class WorkflowStateCollection : ICollection<WorkflowState>, IEnumerable<WorkflowState>, IWorkflowStateCollection
    {
        #region Fields

        [DataMember]
        [ProtoMember(1)]
        private Collection<WorkflowState> _workflowStateCollection = new Collection<WorkflowState>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public WorkflowStateCollection() : base() { }

        /// <summary>
        /// Initializes a new instance of the workflow state collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        public WorkflowStateCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            LoadWorkflowStateCollection(valuesAsXml, objectSerialization);
        }

        /// <summary>
        /// Initialize workflow state collection from tracked activity info collection
        /// </summary>
        /// <param name="trackedActivityInfoCollection">Tracked activity info collection</param>
        public WorkflowStateCollection(ITrackedActivityInfoCollection trackedActivityInfoCollection)
        {
            if (trackedActivityInfoCollection != null)
            {
                foreach (TrackedActivityInfo trackedActivity in trackedActivityInfoCollection)
                {
                    this._workflowStateCollection.Add(new WorkflowState(trackedActivity));
                }
            }
        }

        #endregion

        #region ICollection<WorkflowState> Members

        /// <summary>
        /// Add WorkflowState object in collection
        /// </summary>
        /// <param name="item">WorkflowState to add in collection</param>
        public void Add(WorkflowState item)
        {
            this._workflowStateCollection.Add(item);
        }

        /// <summary>
        /// Add IWorkflowState object in collection
        /// </summary>
        /// <param name="item">IWorkflowState to add in collection</param>
        public void Add(IWorkflowState item)
        {
            this._workflowStateCollection.Add((WorkflowState)item);
        }

        /// <summary>
        /// Removes all WorkflowState from collection
        /// </summary>
        public void Clear()
        {
            this._workflowStateCollection.Clear();
        }

        /// <summary>
        /// Determines whether the WorkflowStateCollection contains a specific WorkflowState.
        /// </summary>
        /// <param name="item">The WorkflowState object to locate in the WorkflowStateCollection.</param>
        /// <returns>
        /// <para>true : If WorkflowState found in WorkflowStateCollection</para>
        /// <para>false : If WorkflowState found not in WorkflowStateCollection</para>
        /// </returns>
        public bool Contains(WorkflowState item)
        {
            return this._workflowStateCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the WorkflowStateCollection to an
        ///  System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from WorkflowStateCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(WorkflowState[] array, int arrayIndex)
        {
            this._workflowStateCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of WorkflowStates in WorkflowStateCollection
        /// </summary>
        public int Count
        {
            get
            {
                return this._workflowStateCollection.Count;
            }
        }

        /// <summary>
        /// Check if WorkflowStateCollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the WorkflowStateCollection.
        /// </summary>
        /// <param name="item">The WorkflowState object to remove from the WorkflowStateCollection.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the original WorkflowStateCollection</returns>
        public bool Remove(WorkflowState item)
        {
            return this._workflowStateCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<WorkflowState> Members

        /// <summary>
        /// Returns an enumerator that iterates through a WorkflowStateCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<WorkflowState> GetEnumerator()
        {
            return this._workflowStateCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a WorkflowStateCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._workflowStateCollection.GetEnumerator();
        }

        #endregion

        #region WorkflowStateCollection Members

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of WorkflowStateCollection object
        /// </summary>
        /// <returns>Xml string representing the WorkflowStateCollection</returns>
        public String ToXml()
        {
            String workflowStateXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (WorkflowState workflowState in this._workflowStateCollection)
            {
                builder.Append(workflowState.ToXml());
            }

            workflowStateXml = String.Format("<WorkflowStates>{0}</WorkflowStates>", builder.ToString());
            return workflowStateXml;
        }

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of the object</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String workflowStateXml = String.Empty;
            StringBuilder builder = new StringBuilder();

            foreach (WorkflowState workflowState in this._workflowStateCollection)
            {
                builder.Append(workflowState.ToXml(objectSerialization));
            }

            workflowStateXml = String.Format("<WorkflowStates>{0}</WorkflowStates>", builder.ToString());
            return workflowStateXml;
        }

        /// <summary>
        /// Converts WorkflowStateCollection object into xml format.
        /// In future, this method can be enhanced to accept EntityConversionContext object
        /// </summary>
        /// <param name="xmlWriter">Indicates a writer to generate xml reperesentation of WorkflowStateCollection object</param>
        internal void ConvertWorkflowStateCollectionToXml(XmlTextWriter xmlWriter)
        {
            if (xmlWriter != null)
            {
                //workflow state collection node start
                xmlWriter.WriteStartElement("WorkflowStates");

                if (this._workflowStateCollection != null && this._workflowStateCollection.Count > 0)
                {
                    foreach (WorkflowState workflowState in this._workflowStateCollection)
                    {
                        workflowState.ConvertWorkflowStateToXml(xmlWriter);
                    }
                }

                //workflow state collection node end
                xmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentNullException("xmlWriter", "XmlTextWriter is null. Can not write WorkflowStateCollection object.");
            }
        }

        #endregion ToXml methods

        /// <summary>
        /// Create a new WorkflowState collection object.
        /// </summary>
        /// <returns>New WorkflowState collection instance.</returns>
        public WorkflowStateCollection Clone()
        {
            WorkflowStateCollection clonedWorkflowStates = new WorkflowStateCollection();

            if (this._workflowStateCollection != null && this._workflowStateCollection.Count > 0)
            {
                foreach (WorkflowState childWorkflowState in this._workflowStateCollection)
                {
                    WorkflowState clonedChildWorkflowState = childWorkflowState.Clone();
                    clonedWorkflowStates.Add(clonedChildWorkflowState);
                }
            }

            return clonedWorkflowStates;
        }

        /// <summary>
        /// Loads WorkflowStateCollection from xml
        /// </summary>
        /// <param name="reader">Indicates an xml reader used to read xml format data</param>
        internal void LoadWorkflowStateCollectionFromXml(XmlTextReader reader)
        {
            if (reader != null)
            {
                if (reader.IsEmptyElement)
                {
                    return;
                }

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowState")
                    {
                        if (reader.HasAttributes)
                        {
                            WorkflowState workflowState = new WorkflowState();

                            workflowState.LoadWorkflowStateMetadataFromXml(reader);

                            this.Add(workflowState);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "WorkflowStates")
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("reader", "XmlTextReader is null. Can not read WorkflowStateCollection object.");
            }
        }

        private void LoadWorkflowStateCollection(String valuesAsXml, ObjectSerialization objectSerialization = ObjectSerialization.Full)
        {
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowStates")
                    {
                        String WorkflowStateXml = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(WorkflowStateXml))
                        {
                            WorkflowState workflowState = new WorkflowState(WorkflowStateXml, objectSerialization);

                            if (workflowState != null)
                                this._workflowStateCollection.Add(workflowState);
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

        #endregion WorkflowStateCollection Memebers
    }
}
