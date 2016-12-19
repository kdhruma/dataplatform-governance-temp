using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies results for Business rule operations
    /// </summary>
    [DataContract]
    public class BusinessRuleOperationResultCollection : OperationResultCollection, ICollection<BusinessRuleOperationResult>, IEnumerable<BusinessRuleOperationResult>, IBusinessRuleOperationResultCollection
    {
        #region Fields

        /// <summary>
        /// Field denotes the business rule operation result collection
        /// </summary>
        [DataMember]
        private Collection<BusinessRuleOperationResult> _businessRuleOperationResults = new Collection<BusinessRuleOperationResult>();

        /// <summary>
        /// Field denoting errors for operation
        /// </summary>
        private ErrorCollection _errors = new ErrorCollection();

        /// <summary>
        /// Field denoting information about operation
        /// </summary>
        private InformationCollection _informations = new InformationCollection();

        /// <summary>
        /// Field denoting warning about operation
        /// </summary>
        private WarningCollection _warnings = new WarningCollection();

        #endregion Fields

        #region Properties

        #region IBusinessRuleOperationResultCollection

        /// <summary>
        /// Property denoting errors for operation
        /// </summary>
        [DataMember]
        public ErrorCollection Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }

        /// <summary>
        /// Property denoting information about operation
        /// </summary>
        [DataMember]
        public InformationCollection Informations
        {
            get
            {
                return _informations;
            }
            set
            {
                _informations = value;
            }
        }

        /// <summary>
        /// Property denoting error about operation
        /// </summary>
        [DataMember]
        public WarningCollection Warnings
        {
            get
            {
                return _warnings;
            }
            set
            {
                _warnings = value;
            }
        }

        #endregion IBusinessRuleOperationResultCollection

        #region ICollection<BusinessRuleOperationResultCollection> Properties

        /// <summary>
        /// Gets the number of elements contained in Business rule operation result collection
        /// </summary>
        public new Int32 Count
        {
            get
            {
                return this._businessRuleOperationResults.Count;
            }
        }

        #endregion ICollection<BusinessRuleOperationResultCollection> Properties

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public BusinessRuleOperationResultCollection()
            : base()
        {
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of Business rule operation result collection object</param>
        public BusinessRuleOperationResultCollection(String valuesAsXml)
        {
            LoadOperationResult(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current Business rule operationResult collection
        /// </param>
        public void LoadOperationResult(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "BusinessRuleOperationResults")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Status"))
                                {
                                    OperationResultStatusEnum status = OperationResultStatusEnum.None;

                                    ValueTypeHelper.EnumTryParse<OperationResultStatusEnum>(reader.ReadContentAsString(), false, out status);
                                    this.OperationResultStatus = status;
                                }
                                reader.Read();
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Error")
                        {
                            String errorXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(errorXml))
                            {
                                Error error = new Error(errorXml);

                                if (error != null)
                                {
                                    if (this.Errors == null)
                                    {
                                        this.Errors = new ErrorCollection();
                                    }
                                    this.Errors.Add(error);
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Information")
                        {
                            String infoXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(infoXml))
                            {
                                Information info = new Information(infoXml);

                                if (info != null)
                                {
                                    if (this.Informations == null)
                                    {
                                        this.Informations = new InformationCollection();
                                    }
                                    this.Informations.Add(info);
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Warning")
                        {
                            String warningXml = reader.ReadOuterXml();

                            if (!String.IsNullOrEmpty(warningXml))
                            {
                                Warning warning = new Warning(warningXml);

                                if (warning != null)
                                {
                                    if (this.Warnings == null)
                                    {
                                        this.Warnings = new WarningCollection();
                                    }
                                    this.Warnings.Add(warning);
                                }
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "BusinessRuleOperationResult")
                        {
                            String operationResult = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(operationResult))
                            {
                                BusinessRuleOperationResult opResult = new BusinessRuleOperationResult(operationResult);

                                if (opResult != null)
                                {
                                    this._businessRuleOperationResults.Add(opResult);
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
        /// Get Xml representation of Business rule operation result collection
        /// </summary>
        /// <returns>Xml representation of Business rule operation result collection object</returns>
        public new String ToXml()
        {
            String outputXML = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("BusinessRuleOperationResults");
                    xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

                    #region Write Errors

                    //Add Error Nodes
                    xmlWriter.WriteStartElement("Errors");

                    if (this.Errors != null)
                    {
                        foreach (Error error in this.Errors)
                        {
                            xmlWriter.WriteRaw(error.ToXml());
                        }
                    }

                    //Error nodes end
                    xmlWriter.WriteEndElement();

                    #endregion Write Errors

                    #region Write Information

                    //Add Information Nodes
                    xmlWriter.WriteStartElement("Informations");

                    if (this.Informations != null)
                    {
                        foreach (Information information in this.Informations)
                        {
                            xmlWriter.WriteRaw(information.ToXml());
                        }
                    }

                    //Information node end
                    xmlWriter.WriteEndElement();

                    #endregion Write Information

                    #region Write Warning

                    //Add warning Nodes
                    xmlWriter.WriteStartElement("Warnings");

                    if (this.Warnings != null)
                    {
                        foreach (Warning warning in this.Warnings)
                        {
                            xmlWriter.WriteRaw(warning.ToXml());
                        }
                    }

                    //warning node end
                    xmlWriter.WriteEndElement();

                    #endregion Write warning

                    #region Write BusinessRuleOperationResult

                    if (this._businessRuleOperationResults != null)
                    {
                        foreach (BusinessRuleOperationResult result in this._businessRuleOperationResults)
                        {
                            xmlWriter.WriteRaw(result.ToXml());
                        }
                    }

                    #endregion Write BusinessRuleOperationResult

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    outputXML = stringWriter.ToString();
                }
            }
            return outputXML;
        }

        /// <summary>
        /// Add operation result to the Operation result Collection
        /// </summary>
        /// <param name="messageCode">Indicates the locale message code</param>
        /// <param name="message">Indicates the locale message</param>
        /// <param name="parameters">Indicates the list of parameters which needs to be replaced in locale message</param>
        /// <param name="operationResultType">Indicates the operation result type</param>
        /// <param name="callerContext">Indicates name of application and module which is performing the action</param>
        public void AddOperationResult(String messageCode, String message, Object[] parameters, OperationResultType operationResultType, CallerContext callerContext)
        {
            // Code should be recorrected to replace parameters in locale message
            BusinessRuleOperationResult businessRuleOperationResult = new BusinessRuleOperationResult();
            businessRuleOperationResult.AddOperationResult(messageCode, message, parameters, operationResultType);
            _businessRuleOperationResults.Add(businessRuleOperationResult);
        }

        /// <summary>
        /// Returns the operation result based on reference id
        /// </summary>
        /// <param name="referenceId">Indicates the reference id</param>
        /// <returns>Returns the Business rule operation result object.</returns>
        public BusinessRuleOperationResult GetBusinessRuleOperationResultByReferenceId(Int64 referenceId)
        {
            BusinessRuleOperationResult businessRuleOperationResult = null;

            if (this._businessRuleOperationResults != null && this._businessRuleOperationResults.Count > 0)
            {
                foreach (BusinessRuleOperationResult item in this._businessRuleOperationResults)
                {
                    if (item.ReferenceId == referenceId)
                    {
                        businessRuleOperationResult = item;
                        break;
                    }
                }
            }

            return businessRuleOperationResult;
        }

        /// <summary>
        /// Returns the businessrule operationresult based on operation result status enum
        /// </summary>
        /// <param name="status">Indicates the Operation result status enum</param>
        /// <returns>Returns the businessrule operationresult collection object.</returns>
        public BusinessRuleOperationResultCollection GetBusinessRuleOperationResultByOperationResultStatus(OperationResultStatusEnum status)
        {
            BusinessRuleOperationResultCollection businessRuleOperationResults = null;

            if (this._businessRuleOperationResults != null && this._businessRuleOperationResults.Count > 0)
            {
                businessRuleOperationResults = new BusinessRuleOperationResultCollection();

                foreach (BusinessRuleOperationResult item in this._businessRuleOperationResults)
                {
                    if (item.OperationResultStatus == status)
                    {
                        businessRuleOperationResults.Add(item);
                    }
                }
            }

            return businessRuleOperationResults;
        }

        /// <summary>
        /// Returns errors
        /// </summary>
        /// <returns>Collection of error</returns>
        public ErrorCollection GetErrors()
        {
            ErrorCollection errors = new ErrorCollection();

            if (this.Errors != null && this.Errors.Count > 0)
            {
                foreach (Error error in this.Errors)
                {
                    errors.Add(error);
                }
            }

            return errors;
        }

        /// <summary>
        /// Returns informations
        /// </summary>
        /// <returns>Collection of information</returns>
        public InformationCollection GetInformations()
        {
            InformationCollection informations = new InformationCollection();

            foreach (Information info in this.Informations)
            {
                informations.Add(info);
            }

            return informations;
        }

        /// <summary>
        /// Returns warnings
        /// </summary>
        /// <returns>Collection of warnings</returns>
        public WarningCollection GetWarnings()
        {
            WarningCollection warnings = new WarningCollection();

            foreach (Warning warning in this.Warnings)
            {
                warnings.Add(warning);
            }

            return warnings;
        }

        /// <summary>
        /// Calculates the status and updates with new status
        /// </summary>
        public void RefreshBusinessRuleOperationResultStatus()
        {
            //Get failed BusinessRule operation results
            Collection<BusinessRuleOperationResult> failedORs = new Collection<BusinessRuleOperationResult>();

            //Get succeeded BusinessRule operation results
            Collection<BusinessRuleOperationResult> succeededORs = new Collection<BusinessRuleOperationResult>();

            //Get warned BusinessRule operation results
            Collection<BusinessRuleOperationResult> warnedORs = new Collection<BusinessRuleOperationResult>();

            foreach(BusinessRuleOperationResult operationResult in  this._businessRuleOperationResults)
            {
                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed || operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors)
                {
                    failedORs.Add(operationResult);
                }

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.None || operationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    succeededORs.Add(operationResult);
                }

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings)
                {
                    warnedORs.Add(operationResult);
                }
            }

            if (failedORs != null && failedORs.Count > 0)
            {
                //if all are failed the overall status will be Failed else completedWithErrors.
                if (failedORs.Count == this.Count)
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                else
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (warnedORs != null && warnedORs.Count > 0)
            {
                //if some are get warned the overall status will be CompletedWithWarnings
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
            else if (succeededORs != null && succeededORs.Count > 0)
            {
                //if there are no errors and warnings the overall status will be successful
                this.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            else
                this.OperationResultStatus = OperationResultStatusEnum.None;
        }

        /// <summary>
        /// Get business rule operation results having errored results
        /// </summary>
        /// <returns>Returns business rule operation results having errored results</returns>
        public BusinessRuleOperationResultCollection GetErroredBusinessRuleOperationResults()
        {
            BusinessRuleOperationResultCollection filteredBusinessRuleOperationResults = new BusinessRuleOperationResultCollection();

            if (this._businessRuleOperationResults != null && this._businessRuleOperationResults.Count > 0)
            {
                foreach (BusinessRuleOperationResult operationResult in this._businessRuleOperationResults)
                {
                    if (operationResult.HasError)
                    {
                        filteredBusinessRuleOperationResults.Add(operationResult);
                    }
                }
            }

            return filteredBusinessRuleOperationResults;
        }

        /// <summary>
        /// Checks whether actual output is a superset of expected output or not
        /// </summary>
        /// <param name="subSetBusinessRuleOperationResultCollection">Indicates BusinessRuleOperationResultCollection to be compared with the current collection</param>
        /// <param name="compareIds">Indicates whether Ids to be compared or not.</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>   
        public Boolean IsSuperSetOf(BusinessRuleOperationResultCollection subSetBusinessRuleOperationResultCollection, Boolean compareIds = false)  
        {
            if (subSetBusinessRuleOperationResultCollection != null)
            {
                foreach (BusinessRuleOperationResult subSetBusinessRuleOperationResult in subSetBusinessRuleOperationResultCollection)
                {
                    BusinessRuleOperationResult sourceBusinessRuleOperationResult = this.GetBusinessRuleOperationResultByReferenceId(subSetBusinessRuleOperationResult.ReferenceId);
                    
                    //If it doesn't return, that means super set doesn't contain that mdmrule.
                    //So return false, else do further comparison
                    if (subSetBusinessRuleOperationResult != null)
                    {
                        if (!sourceBusinessRuleOperationResult.IsSuperSetOf(subSetBusinessRuleOperationResult))
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


        #endregion Public Methods

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An enumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._businessRuleOperationResults.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IEnumerable<BusinessRuleOperationResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a Business rule operation result collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public new IEnumerator<BusinessRuleOperationResult> GetEnumerator()
        {
            return this._businessRuleOperationResults.GetEnumerator();
        }

        #endregion  IEnumerable<BusinessRuleOperationResult> Members

        #region ICollection<BusinessRuleOperationResult> Members

        /// <summary>
        /// Add business rule operation result object in collection
        /// </summary>
        /// <param name="businessRuleOperationResult">Indicates the business rule operation result</param>
        public void Add(BusinessRuleOperationResult businessRuleOperationResult)
        {
            if (businessRuleOperationResult != null)
            {
                this._businessRuleOperationResults.Add(businessRuleOperationResult);
            }
        }

        /// <summary>
        /// Add business rule operation result object in collection
        /// </summary>
        /// <param name="businessRuleOperationResult">Indicates the business rule operation result</param>
        public void Add(IBusinessRuleOperationResult businessRuleOperationResult)
        {
            if (businessRuleOperationResult != null)
            {
                this.Add((BusinessRuleOperationResult)businessRuleOperationResult);
            }
        }

        /// <summary>
        /// Add businessrule operationresult objects in collection
        /// </summary>
        /// <param name="businessRuleOperationResults">Indicates the business rule operation results to be inserted</param>
        public void AddRange(BusinessRuleOperationResultCollection businessRuleOperationResults)
        {
            if (businessRuleOperationResults != null && businessRuleOperationResults.Count > 0)
            {
                foreach (BusinessRuleOperationResult businessRuleOperationResult in businessRuleOperationResults)
                {
                    this._businessRuleOperationResults.Add(businessRuleOperationResult);
                }
            }
        }

        /// <summary>
        /// Add business rule operation result objects in collection
        /// </summary>
        /// <param name="businessRuleOperationResults">Indicates the business rule operation results to be inserted</param>
        public void AddRange(IBusinessRuleOperationResultCollection businessRuleOperationResults)
        {
            this.AddRange((BusinessRuleOperationResultCollection)businessRuleOperationResults);
        }

        /// <summary>
        /// Removes all Business rules results from collection
        /// </summary>
        public new void Clear()
        {
            this._businessRuleOperationResults.Clear();
        }

        /// <summary>
        /// Determines whether the BusinessRuleOperationResultCollection contains a specific Business rule operation result
        /// </summary>
        /// <param name="businessRuleOperationResult">Indicates the Business rule operation result.</param>
        /// <returns>
        /// <para>true : If Business rule operation result found in BusinessRuleOperationResultCollection</para>
        /// <para>false : If Business rule operation result found not in BusinessRuleOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(BusinessRuleOperationResult businessRuleOperationResult)
        {
            return this._businessRuleOperationResults.Contains(businessRuleOperationResult);
        }

        /// <summary>
        /// Copies the elements of the BusinessRuleOperationResultCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from BusinessRuleOperationResultCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(BusinessRuleOperationResult[] array, Int32 arrayIndex)
        {
            this._businessRuleOperationResults.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific business rule operation result from the BusinessRuleOperationResultCollection.
        /// </summary>
        /// <param name="businessRuleOperationResult">The business rule operation result object to remove from the BusinessRuleOperationResultCollection.</param>
        /// <returns>true if business rule operation result is successfully removed; otherwise, false. This method also returns false if business rule operation result was not found in the original collection</returns>
        public Boolean Remove(BusinessRuleOperationResult businessRuleOperationResult)
        {
            return this._businessRuleOperationResults.Remove(businessRuleOperationResult);
        }

        #endregion

        #endregion  Methods
    }
}
