using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies collection of operation results
    /// </summary>
    [DataContract]
    public class OperationResultCollection : InterfaceContractCollection<IOperationResult, OperationResult>, IOperationResultCollection
    {
        #region Fields

        /// <summary>
        /// Indicates status of Operation.
        /// </summary>
        private OperationResultStatusEnum _operationResultStatus = OperationResultStatusEnum.None;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates status of Operation.
        /// </summary>
        [DataMember]
        public OperationResultStatusEnum OperationResultStatus
        {
            get { return _operationResultStatus; }
            set { _operationResultStatus = value; }
        }

        /// <summary>
        /// Returns Operation Result object for a given index from the collection
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Operation Result object for a given index from the collection</returns>
        public OperationResult this[Int32 index]
        {
            get { return _items[index]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public OperationResultCollection() : base() { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        public OperationResultCollection(String valueAsXml)
        {
            LoadOperationResultCollection(valueAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        /// <summary>
        /// Adds an item to the operation result collection
        /// </summary>
        /// <param name="item">Indicates the operation result to add to operation result collection</param>
        public new void Add(IOperationResult item)
        {
            if (item.HasError)
            {
                //Update operation result status
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (item.HasWarnings)
            {
                //Update operation result status
                if (this.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }

            base.Add(item);
        }

        /// <summary>
        /// Adds an item to the operation result collection
        /// </summary>
        /// <param name="items">Indicates the operation result collection to add to another operation result collection</param>
        public void AddRange(IOperationResultCollection items)
        {
            foreach (IOperationResult item in items)
            {
                if (item.HasError)
                {
                    //Update operation result status
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
                }
                else if (item.HasWarnings)
                {
                    //Update operation result status
                    if (this.OperationResultStatus != OperationResultStatusEnum.CompletedWithErrors)
                        this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
                }

                base.Add(item);
            }
        }

        /// <summary>
        /// Verify whether or not sequence contain required item
        /// </summary>
        /// <param name="id">Id of the item</param>
        /// <returns>[True] if item exists, [False] otherwise</returns>
        public Boolean Contains(Int32 id)
        {
            return this.Get(id) != null;
        }

        /// <summary>
        /// Removes item by id
        /// </summary>
        /// <param name="id">Id of item to be removed</param>
        /// <returns>[True] if removal was successful, [False] otherwise</returns>
        public Boolean Remove(Int32 id)
        {
            OperationResult operationResult = this.Get(id) as OperationResult;

            return operationResult != null && _items.Remove(operationResult);
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            return this._items.Sum(attr => attr.GetHashCode());
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is OperationResultCollection)
            {
                OperationResultCollection objectToBeCompared = obj as OperationResultCollection;
                Int32 operationsResultsUnion = this._items.ToList().Union(objectToBeCompared._items.ToList()).Count();
                Int32 operationsIntersect = this._items.ToList().Intersect(objectToBeCompared._items.ToList()).Count();
                if (operationsResultsUnion != operationsIntersect)
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Convert item to XML
        /// </summary>
        /// <returns>XML string</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("OperationResults");

            #region Write AttributeOperationResults

            foreach (OperationResult operationResult in this._items)
            {
                xmlWriter.WriteRaw(operationResult.ToXml());
            }

            #endregion

            //Operation result node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            returnXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return returnXml;
        }

        /// <summary>
        /// Convert item to XML with specific rule
        /// </summary>
        /// <param name="serialization">Rules of serialization</param>
        /// <returns></returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Gets item by Id
        /// </summary>
        /// <param name="objectId">Object Id</param>
        /// <returns>Cloned item</returns>
        public IOperationResult Get(int objectId)
        {
            return this._items.FirstOrDefault(item => item.Id == objectId);
        }

        /// <summary>
        /// Returns the operation result based on reference id
        /// </summary>
        /// <param name="referenceId">Indicates the reference id</param>
        /// <returns>Returns the Operation result object.</returns>
        public OperationResult GetOperationResultByReferenceId(String referenceId)
        {
            OperationResult operationResult = null;

            if (this._items != null && this._items.Count > 0)
            {
                foreach (OperationResult item in this._items)
                {
                    if (String.Compare(item.ReferenceId, referenceId) == 0)
                    {
                        operationResult = item;
                        break;
                    }
                }
            }

            return operationResult;
        }
            
        /// <summary>
        /// Returns the operation result based on operation result status enum
        /// </summary>
        /// <param name="status">Indicates the Operation result status enum</param>
        /// <returns>Returns the operation result collection object.</returns>
        public OperationResultCollection GetByOperationResultStatus(OperationResultStatusEnum status)
        {
            OperationResultCollection operationResults = null;

            if (this._items != null && this._items.Count > 0)
            {
                operationResults = new OperationResultCollection();

                foreach (BusinessRuleOperationResult item in this._items)
                {
                    if (item.OperationResultStatus == status)
                    {
                        operationResults.Add(item);
                    }
                }
            }

            return operationResults;
        }

        /// <summary>
        /// Calculates the status and updates with new status
        /// </summary>
        public void RefreshOperationResultStatus()
        {
            //Get failed attribute operation results
            IEnumerable<OperationResult> failedORs = this._items.Where(or => or.OperationResultStatus == OperationResultStatusEnum.Failed ||
                or.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors);

            //Get succeeded attribute operation results
            IEnumerable<OperationResult> succeededORs = this._items.Where(or => or.OperationResultStatus == OperationResultStatusEnum.None ||
                or.OperationResultStatus == OperationResultStatusEnum.Successful);

            IEnumerable<OperationResult> warnedORs = this._items.Where(or => or.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings);

            if (failedORs != null && failedORs.Count() > 0)
            {
                //if all are failed the overall status will be Failed else completedWithErrors.
                if (failedORs.Count() == this.Count)
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                else
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (warnedORs != null && warnedORs.Count() > 0)
            {
                //if some are get warned the overall status will be CompletedWithWarnings
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
            else if (succeededORs != null && succeededORs.Count() > 0)
            {
                //if there are no errors and warnings the overall status will be successful
                this.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            else
                this.OperationResultStatus = OperationResultStatusEnum.None;
        }

        #endregion Public Methods

        /// <summary>
        /// Checks whether expected output is a subset of actual output or not.
        /// </summary>
        /// <param name="subsetOperationResults">The expected output result which is compared against actual output result.</param>
        /// <param name="compareIds">Indicates whether to compare operation result ids also or not</param>
        public Boolean IsSuperSetOf(OperationResultCollection subsetOperationResults, Boolean compareIds = false)
        {
            if (subsetOperationResults != null && subsetOperationResults.Count > 0)
            {
                foreach (OperationResult subsetOperationResult in subsetOperationResults)
                {
                    if (this._items != null && this._items.Count > 0)
                    {
                        foreach (OperationResult sourceOperationResult in this._items)
                        {
                            if (sourceOperationResult.IsSuperSetOf(subsetOperationResult, compareIds))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        #region Private Methods

        private void LoadOperationResultCollection(string valueAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valueAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "OperationResult")
                        {
                            String operationResultXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(operationResultXml))
                            {
                                OperationResult operationResult = new OperationResult(operationResultXml);
                                this.Add(operationResult);
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

        #endregion Private Methods

        #endregion
    }
}