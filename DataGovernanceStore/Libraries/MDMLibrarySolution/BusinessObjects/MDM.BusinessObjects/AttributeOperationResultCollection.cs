using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Attribute Operation Result Collection
    /// </summary>
    [DataContract]
    public class AttributeOperationResultCollection : ICollection<AttributeOperationResult>, IEnumerable<AttributeOperationResult>, IAttributeOperationResultCollection
    {
        #region Fields

        /// <summary>
        /// Indicates status of Operation.
        /// </summary>
        private OperationResultStatusEnum _operationResultStatus = OperationResultStatusEnum.None;

        /// <summary>
        /// Field denoting collection of Attribute Operation Result
        /// </summary>
        [DataMember]
        private Collection<AttributeOperationResult> _attributeOperationResultCollection = new Collection<AttributeOperationResult>();

        #endregion

        #region Properties

        /// <summary>
        /// Indicates overall status of Operation result
        /// </summary>
        [DataMember]
        public OperationResultStatusEnum OperationResultStatus
        {
            get
            {
                return _operationResultStatus;
            }
            set
            {
                _operationResultStatus = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the attribute operation result collection class
        /// </summary>
        public AttributeOperationResultCollection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the attribute operation result collection class with the XML having values of object
        /// </summary>
        /// <param name="valuesAsXml">XML having values of object</param>
        public AttributeOperationResultCollection(String valuesAsXml)
        {
            /*
             * Sample XML:
                <AttributeOperationResults Status="Failed">
                    <AttributeOperationResult Id="9812" Status="Failed">
                        <Errors>
                            <Error Code="" Message = "Required Attributes are not filled"/>
                        </Errors>
                        <Informations>
                            <Information Code="" Message = ""/>
                        </Informations>
                        <ReturnValues />
                    </AttributeOperationResult>
                </AttributeOperationResults>
             */
            XmlTextReader reader = null;

            if (!String.IsNullOrEmpty(valuesAsXml))
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeOperationResults")
                    {
                        if (reader.MoveToAttribute("Status"))
                        {
                            OperationResultStatusEnum operationResultStatus = OperationResultStatusEnum.None;
                            String strOperationResultStatus = reader.ReadContentAsString();

                            if (!String.IsNullOrWhiteSpace(strOperationResultStatus))
                                Enum.TryParse<OperationResultStatusEnum>(strOperationResultStatus, out operationResultStatus);

                            this.OperationResultStatus = operationResultStatus;
                        }

                        reader.Read();
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeOperationResult")
                    {
                        String attributeOperationResultsXML = reader.ReadOuterXml();

                        if (!String.IsNullOrWhiteSpace(attributeOperationResultsXML))
                        {
                            AttributeOperationResult attributeOperationResult = new AttributeOperationResult(attributeOperationResultsXML);

                            if (attributeOperationResult != null)
                                this._attributeOperationResultCollection.Add(attributeOperationResult);
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

        /// <summary>
        /// Initializes a new instance of the attribute operation result collection for the attributes
        /// </summary>
        /// <param name="attributes">Attributes as per which attribute operation result collection is going to be initialized</param>
        public AttributeOperationResultCollection(AttributeCollection attributes)
        {
            PrepareAttributeOperationResultsSchema(attributes);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if AttributeOperationResultCollection contains attribute operation result with given attribute id
        /// </summary>
        /// <param name="attributeId">Id of the attribute using which attribute operation result is to be searched from collection</param>
        /// <returns>
        /// <para>true : If attribute operation result found in AttributeOperationResultCollection</para>
        /// <para>false : If attribute operation result found not in AttributeOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(Int32 attributeId)
        {
            if (GetAttributeOperationResult(attributeId) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove attributeOperationResult object from AttributeOperationResultCollection
        /// </summary>
        /// <param name="attributeId">Id of the attribute using which Attribute Operation Result is to be removed from collection</param>
        /// <returns>true if attribute operation result is successfully removed; otherwise, false. This method also returns false if attribute operation result was not found in the original collection</returns>
        public Boolean Remove(Int32 attributeId)
        {
            //Commenting remove

            //There should not be any way to remove attribute operation result
            //If any business rule removes any of the attributeOR from the Operation Result schema, operation gives improper results

            //AttributeOperationResult attributeOperationResult = GetAttributeOperationResult(attributeId);

            //if (attributeOperationResult == null)
            //    throw new ArgumentException("No attribute operation result found for given attribute id");
            //else
            //    return this.Remove(attributeOperationResult);

            return false;
        }

        /// <summary>
        /// Get Xml representation of attribute operation result collection
        /// </summary>
        /// <returns>Xml representation of attribute operation result collection</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("AttributeOperationResults");

            #region Write attribute operation result properties

            xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

            #endregion

            #region Write AttributeOperationResults

            foreach (AttributeOperationResult aor in this._attributeOperationResultCollection)
            {
                xmlWriter.WriteRaw(aor.ToXml());
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
        /// Get Xml representation of attribute operation result collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of attribute operation result collection</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Operation result node start
            xmlWriter.WriteStartElement("AttributeOperationResults");

            #region Write attribute operation result properties

            xmlWriter.WriteAttributeString("Status", this.OperationResultStatus.ToString());

            #endregion

            #region Write AttributeOperationResults

            foreach (AttributeOperationResult aor in this._attributeOperationResultCollection)
            {
                //If object serialization says external and the operation result status is success do not add the AOR.. Add only failed ones..
                // For successful attributes, status stays as 'None'. Ignore those also for external serialization.
                if (objectSerialization == ObjectSerialization.External &&
                        (aor.OperationResultStatus == OperationResultStatusEnum.Successful || aor.OperationResultStatus == OperationResultStatusEnum.None))
                    continue;

                xmlWriter.WriteRaw(aor.ToXml(objectSerialization));
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
        /// Adds operation result
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddAttributeOperationResult(Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType)
        {
            Boolean addSuccess = false;

            //Get the requested attribute operation result
            AttributeOperationResult attributeOperationResult = GetAttributeOperationResult(attributeId);

            if (attributeOperationResult != null)
            {
                //Add operation result
                addSuccess = attributeOperationResult.AddOperationResult(resultCode, resultMessage, operationResultType);

                if (addSuccess)
                    RefreshOperationResultStatus();
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="attributeName">Id of the attribute for which operation result needs to be added</param>
        /// <param name="messageCode">message Code</param>
        /// <param name="parameters">message parameters</param>
        /// <param name="reasonType">Result Message</param>
        /// <param name="ruleMapContextId">Business rule map context id</param>
        /// <param name="ruleId">Business rule id</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddAttributeOperationResult(String attributeName, String messageCode, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            //Get the requested attribute operation result
            AttributeOperationResult attributeOperationResult = GetAttributeOperationResult(attributeName);

            if (attributeOperationResult != null)
            {
                //Add operation result
                addSuccess = attributeOperationResult.AddOperationResult(messageCode, "", parameters, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

                if (addSuccess)
                {
                    RefreshOperationResultStatus();
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="attributeName">Name of the attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="reasonType">Reason type</param>
        /// <param name="ruleMapContextId">Rule map context id</param>
        /// <param name="ruleId">Rule id</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        public Boolean AddAttributeOperationResult(String attributeName, String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false)
        {
            Boolean addSuccess = false;

            //Get the requested attribute operation result
            AttributeOperationResult attributeOperationResult = GetAttributeOperationResult(attributeName);

            if (attributeOperationResult != null)
            {
                //Add operation result
                addSuccess = attributeOperationResult.AddOperationResult(resultCode, resultMessage, reasonType, ruleMapContextId, ruleId, operationResultType, ignoreError);

                if (addSuccess)
                {
                    RefreshOperationResultStatus();
                }
            }

            return addSuccess;
        }

        /// <summary>
        /// Calculates the status and updates with new status
        /// </summary>
        public void RefreshOperationResultStatus()
        {
            //Get failed attribute operation results
            IEnumerable<AttributeOperationResult> failedAORs = this._attributeOperationResultCollection.Where(aor => aor.OperationResultStatus == OperationResultStatusEnum.Failed ||
                aor.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors);

            //Get succeeded attribute operation results
            IEnumerable<AttributeOperationResult> succeededAORs = this._attributeOperationResultCollection.Where(aor => aor.OperationResultStatus == OperationResultStatusEnum.None ||
                aor.OperationResultStatus == OperationResultStatusEnum.Successful);

            IEnumerable<AttributeOperationResult> warnedAORs = this._attributeOperationResultCollection.Where(aor => aor.OperationResultStatus == OperationResultStatusEnum.CompletedWithWarnings);

            //if (failedAORs == null || failedAORs.Count() < 1)
            //    this.OperationResultStatus = OperationResultStatusEnum.Successful; //There are no failed AORs. So, the overall operation result is Success
            //else if (succeededAORs == null || succeededAORs.Count() < 1)
            //    this.OperationResultStatus = OperationResultStatusEnum.Failed;  //There are no succeeded AORs. So, the overall operation result is Fail
            //else if (warnedAORs == null || warnedAORs.Count() < 1)
            //    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            //else
            //    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors; //There are failed and succeeded AORs. So, the overall operation result is Completed With Errors

            if (failedAORs != null && failedAORs.Count() > 0)
            {
                //if all are failed the overall status will be Failed else completedWithErrors.
                if (failedAORs.Count() == this.Count)
                    this.OperationResultStatus = OperationResultStatusEnum.Failed;
                else
                    this.OperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
            }
            else if (warnedAORs != null && warnedAORs.Count() > 0)
            {
                //if some are get warned the overall status will be CompletedWithWarnings
                this.OperationResultStatus = OperationResultStatusEnum.CompletedWithWarnings;
            }
            else if (succeededAORs != null && succeededAORs.Count() > 0)
            {
                //if there are no errors and warnings the overall status will be successful
                this.OperationResultStatus = OperationResultStatusEnum.Successful;
            }
            else
                this.OperationResultStatus = OperationResultStatusEnum.None;
        }

        /// <summary>
        /// Find AttributeOperationResult from AttributeOperationResultCollection using AttributeId and locale 
        /// </summary>
        /// <param name="attributeId">AttributeId for which AttributeOperationResult  is to be searched</param>
        /// <param name="locale">Locale for which AttributeOperationResult  is to be searched</param>
        /// <returns>AttributeOperationResult having given attribute id and locale</returns>
        public AttributeOperationResult GetAttributeOperationResult(Int32 attributeId, LocaleEnum locale)
        {
            AttributeOperationResult attributeOperationResult = null;

            if (this._attributeOperationResultCollection != null)
            {
                //attributeOperationResult
                var operationResult = from attrOR in this._attributeOperationResultCollection
                                      where attrOR.AttributeId == attributeId && attrOR.Locale == locale
                                      select attrOR;

                if (operationResult != null && operationResult.ToList<AttributeOperationResult>() != null && operationResult.ToList<AttributeOperationResult>().Count > 1)
                {
                    throw new InvalidOperationException("More than one attribute operation results found for attribute Id:" + attributeId + " and Locale:" + locale.ToString());
                }
                else
                {
                    attributeOperationResult = operationResult.ToList<AttributeOperationResult>().FirstOrDefault();
                }
            }

            return attributeOperationResult;
        }

        /// <summary>
        /// Refresh and update missing attribute operation results for the given entities
        /// </summary>
        /// <param name="attributes">Entities as per which entity operation result collection is going to be initialized</param>
        public void RefreshOperationResultsSchema(AttributeCollection attributes)
        {
            PrepareAttributeOperationResultsSchema(attributes, RefreshType.Delta);
        }

        /// <summary>
        /// Prepares Attribute Operation Results schema as per the attributes 
        /// </summary>
        /// <param name="attributes">Attributes for which operation results schema has to be prepared</param>
        /// <param name="refreshType"></param>
        public void PrepareAttributeOperationResultsSchema(AttributeCollection attributes, RefreshType refreshType = RefreshType.Full)
        {
            if (attributes != null && attributes.Count > 0)
            {
                if (refreshType == RefreshType.Full && this._attributeOperationResultCollection.Count > 0)
                {
                    this._attributeOperationResultCollection.Clear();
                }

                foreach (Attribute attr in attributes)
                {
                    AttributeOperationResult attributeOperationResult = null;

                    if (refreshType == RefreshType.Delta)
                    {
                        attributeOperationResult = _attributeOperationResultCollection.FirstOrDefault(a => a.AttributeId == attr.Id && a.Locale == attr.Locale);

                        if (attributeOperationResult != null)
                        {
                            attributeOperationResult.AttributeShortName = attr.Name;
                            attributeOperationResult.AttributeLongName = attr.LongName;
                        }
                    }

                    if (attributeOperationResult == null)
                    {
                        attributeOperationResult = new AttributeOperationResult(attr.Id, attr.Name, attr.LongName, attr.AttributeModelType, attr.Locale);
                        this._attributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }
            }
        }

        /// <summary>
        /// Compares Expected AttributeOperationResultCollection object with the current instance
        /// </summary>
        /// <param name="subsetAttributeOperationResultCollection">Expected AttributeOperationResultCollection to be compared from</param>
        /// <param name="compareIds">Check whether to compare Id's or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(AttributeOperationResultCollection subsetAttributeOperationResultCollection, Boolean compareIds = false)
        {
            if (this.OperationResultStatus != subsetAttributeOperationResultCollection.OperationResultStatus)
                return false;

            foreach (AttributeOperationResult attributeOperationResult in subsetAttributeOperationResultCollection)
            {
                AttributeOperationResult subsetAttributeOperationResult = this.Where(aor => aor.AttributeShortName == attributeOperationResult.AttributeShortName
                                                                                    && aor.AttributeLongName == attributeOperationResult.AttributeLongName
                                                                                    && aor.AttributeModelType == attributeOperationResult.AttributeModelType
                                                                                    && aor.Locale == attributeOperationResult.Locale).ToList<AttributeOperationResult>().FirstOrDefault();


                if (subsetAttributeOperationResult == null)
                {
                    return false;
                }

                if (!attributeOperationResult.IsSuperSetOf(subsetAttributeOperationResult, compareIds))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// Gets the attribute operation result collection of specific attribute model type
        /// </summary>
        /// <param name="attributeModelType">Indicates the attribute model.</param>
        /// <returns>Returns the attribute operation result collection of specific attribute model type</returns>
        public AttributeOperationResultCollection Get(AttributeModelType attributeModelType)
        {
            AttributeOperationResultCollection attributeOperationResultCollection = null;

            if (this._attributeOperationResultCollection.Count > 0)
            {
                attributeOperationResultCollection = new AttributeOperationResultCollection();

                foreach (AttributeOperationResult attributeOperationResult in this._attributeOperationResultCollection)
                {
                    if (attributeOperationResult.AttributeModelType == attributeModelType)
                    {
                        attributeOperationResultCollection.Add(attributeOperationResult);
                    }
                }
            }

            return attributeOperationResultCollection;
        }

        #endregion

        #region Private Methods

        private AttributeOperationResult GetAttributeOperationResult(Int32 attributeId)
        {
            var filteredAttributeOperationResults = from attributeOperationResult in this._attributeOperationResultCollection
                                                    where attributeOperationResult.AttributeId == attributeId
                                                    select attributeOperationResult;

            if (filteredAttributeOperationResults.Any())
                return filteredAttributeOperationResults.First();
            else
                return null;
        }

        private AttributeOperationResult GetAttributeOperationResult(String attributeShortName)
        {
            var filteredAttributeOperationResults = from attributeOperationResult in this._attributeOperationResultCollection
                                                    where attributeOperationResult.AttributeShortName == attributeShortName
                                                    select attributeOperationResult;

            if (filteredAttributeOperationResults.Any())
            {
                return filteredAttributeOperationResults.First();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ICollection<AttributeOperationResult> Members

        /// <summary>
        /// Add attribute operation result object in collection
        /// </summary>
        /// <param name="attributeOperationResult">Attribute operation result to add in collection</param>
        public void Add(AttributeOperationResult attributeOperationResult)
        {
            this._attributeOperationResultCollection.Add(attributeOperationResult);
        }

        /// <summary>
        /// Removes all relationships from collection
        /// </summary>
        public void Clear()
        {
            this._attributeOperationResultCollection.Clear();
        }

        /// <summary>
        /// Determines whether the AttributeOperationResultCollection contains a specific attribute operation result
        /// </summary>
        /// <param name="attributeOperationResult">The attribute operation result object to locate in the AttributeOperationResultCollection.</param>
        /// <returns>
        /// <para>true : If attribute operation result found in AttributeOperationResultCollection</para>
        /// <para>false : If attribute operation result found not in AttributeOperationResultCollection</para>
        /// </returns>
        public Boolean Contains(AttributeOperationResult attributeOperationResult)
        {
            return this._attributeOperationResultCollection.Contains(attributeOperationResult);
        }

        /// <summary>
        /// Copies the elements of the AttributeOperationResultCollection to a
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array"> 
        ///  The one-dimensional System.Array that is the destination of the elements
        ///  copied from AttributeOperationResultCollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(AttributeOperationResult[] array, Int32 arrayIndex)
        {
            this._attributeOperationResultCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the count of no. of relationship in AttributeOperationResultCollection
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this._attributeOperationResultCollection.Count;
            }
        }

        /// <summary>
        /// Check if AttributeOperationResultCollection is read-only.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific attribute operation result from the AttributeOperationResultCollection.
        /// </summary>
        /// <param name="attributeOperationResult">The attribute operation result object to remove from the AttributeOperationResultCollection.</param>
        /// <returns>true if attribute operation result is successfully removed; otherwise, false. This method also returns false if attribute operation result was not found in the original collection</returns>
        public Boolean Remove(AttributeOperationResult attributeOperationResult)
        {
            //Commenting remove

            //There should not be any way to remove attribute operation result
            //If any business rule removes any of the attributeOR from the Operation Result schema, operation gives improper results

            //return this._attributeOperationResultCollection.Remove(attributeOperationResult);

            return false;
        }

        #endregion

        #region IEnumerable<AttributeOperationResult> Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<AttributeOperationResult> GetEnumerator()
        {
            return this._attributeOperationResultCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a AttributeOperationResultCollection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this._attributeOperationResultCollection.GetEnumerator();
        }

        #endregion
    }
}
