using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Xml;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the Workflow Object Collection
    /// </summary>
    [DataContract]
    public class WorkflowMDMObjectCollection : ICollection<WorkflowMDMObject>, IEnumerable<WorkflowMDMObject>, IWorkflowMDMObjectCollection
    {
        #region Private fields

        /// <summary>
        /// Represents the collection of workflow MDM object type and mdm object id
        /// </summary>
        private Collection<WorkflowMDMObject> _workflowMDMObjects = new Collection<WorkflowMDMObject>();

        #endregion Private fields

        #region Public Properties

        /// <summary>
        /// Property denoting the collection of workflow MDM object type and MDM object id
        /// </summary>
        [DataMember]
        public Collection<WorkflowMDMObject> WorkflowMDMObjects
        {
            get
            {
                return this._workflowMDMObjects;
            }
            set
            {
                this._workflowMDMObjects = value;
            }
        }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public WorkflowMDMObjectCollection()
            : base()
        {
        }

        /// <summary>
        /// Constructor with MDM Object Collection as Input parameter
        /// </summary>
        /// <param name="valuesAsXml">XML having value for WorkflowMDMObjectCollection object</param>
        /// <example>
        /// Sample XML:
        /// <para>
        ///     &lt;WorkflowMDMObjects&gt;
        ///         &lt;WorkflowMDMObject Id="5" MDMObjectId="2" MDMObjectType="Entity" /&gt;
        ///         &lt;WorkflowMDMObject Id="6" MDMObjectId="3" MDMObjectType="Entity" /&gt;
        ///     &lt;/WorkflowMDMObjects&gt;
        /// </para>
        /// </example>
        public WorkflowMDMObjectCollection(String valuesAsXml)
        {
            /*
             * Sample:
             *  <WorkflowMDMObjects>
                  <WorkflowMDMObject Id="5" MDMObjectId="2" MDMObjectType="Entity" />
                  <WorkflowMDMObject Id="6" MDMObjectId="3" MDMObjectType="Entity" />  
                </WorkflowMDMObjects>
             */
            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "WorkflowMDMObject")
                        {
                            String mdmObjXml = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdmObjXml))
                            {
                                WorkflowMDMObject mdmObj = new WorkflowMDMObject(mdmObjXml);
                                if (mdmObj != null)
                                {
                                    this.Add(mdmObj);
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

        #region Public Methods

        /// <summary>
        /// Get Xml representation of Workflow MDM Object Collection
        /// </summary>
        /// <returns>Xml representation of Workflow MDM Object Collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            returnXml = "<WorkflowMDMObjects>";

            if (this._workflowMDMObjects != null && this._workflowMDMObjects.Count > 0)
            {
                foreach (WorkflowMDMObject mdmObj in this._workflowMDMObjects)
                {
                    returnXml = String.Concat(returnXml, mdmObj.ToXml());
                }
            }
            returnXml = String.Concat(returnXml, "</WorkflowMDMObjects>");

            return returnXml;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ActivityTracking object which needs to be compared.</param>
        ///<returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is WorkflowMDMObjectCollection)
            {
                WorkflowMDMObjectCollection objectToBeCompared = obj as WorkflowMDMObjectCollection;

                Int32 mdmObjectsUnion = this._workflowMDMObjects.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 mdmObjectsIntersect = this._workflowMDMObjects.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (mdmObjectsUnion != mdmObjectsIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type 
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.WorkflowMDMObjects.GetHashCode();
        }

        /// <summary>
        /// Gets MDM Object Ids
        /// </summary>
        /// <returns>Collection of MDM Object Ids</returns>
        public Collection<Int64> GetMDMObjectIds()
        {
            Collection<Int64> mdmObjectIds = new Collection<Int64>();

            if (this._workflowMDMObjects != null && this._workflowMDMObjects.Count > 0)
            {
                foreach (WorkflowMDMObject mdmObject in this._workflowMDMObjects)
                {
                    mdmObjectIds.Add(mdmObject.MDMObjectId);
                }
            }

            return mdmObjectIds;
        }

        #endregion Public Methods

        #region ICollection<WorkflowMDMObject> Members
        /// <summary>
        /// Add the item to MDM Object collection
        /// </summary>
        /// <param name="item">The object to be added to the end of the Workflow MDM Object </param>
        public void Add(WorkflowMDMObject item)
        {
            this._workflowMDMObjects.Add(item);
        }

        /// <summary>
        /// Add the item to MDM Object collection
        /// </summary>
        /// <param name="item">The object to be added to the end of the Workflow MDM Object</param>
        public void Add(IWorkflowMDMObject item)
        {
            if (item != null)
            {
                this.Add((WorkflowMDMObject)item);
            }
        }

        /// <summary>
        /// Clear the Workflow MDM Object collection
        /// </summary>
        public void Clear()
        {
            this._workflowMDMObjects.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the Workflow MDM Object collection
        /// </summary>
        /// <param name="item">The object to locate in the MDM Object collection</param>
        /// <returns>true if item is found in the Workflow MDM Object collection;otherwise, false.</returns>
        public bool Contains(WorkflowMDMObject item)
        {
            return this._workflowMDMObjects.Contains(item);
        }

        /// <summary>
        /// Copies the entire Workflow MDM Object collection to a compatible Array, Also it's starting at the specified index of the target array
        /// </summary>
        /// <param name="array">The Array that is the destination of the elements copied from MDM Object collection</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(WorkflowMDMObject[] array, int arrayIndex)
        {
            this._workflowMDMObjects.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the number of elements actually contained in the Workflow MDM object Collection
        /// </summary>
        public int Count
        {
            get
            {
                return this._workflowMDMObjects.Count;
            }
        }

        /// <summary>
        /// Property denoting whether the object is read only or not
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the Workflow MDM Object collection
        /// </summary>
        /// <param name="item">The object to remove from the Workflow MDM object collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        public bool Remove(WorkflowMDMObject item)
        {
            return this._workflowMDMObjects.Remove(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the Workflow MDM Object collection
        /// </summary>
        /// <param name="item">The object to remove from the Workflow MDM object collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        public Boolean Remove(IWorkflowMDMObject item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("WorkflowMDMObject");
            }

            return this.Remove((WorkflowMDMObject)item);
        }

        #endregion  ICollection<WorkflowMDMObject> Members

        #region IEnumerable<WorkflowMDMObject> Members

        /// <summary>
        /// Returns an enumerator that iterates through the Workflow MDM Object collection
        /// </summary>
        /// <returns>An Enumerator for the WorkflowMDMObject</returns>
        public IEnumerator<WorkflowMDMObject> GetEnumerator()
        {
            return this._workflowMDMObjects.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._workflowMDMObjects.GetEnumerator();
        }

        #endregion
    }
}
