using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class for search criteria for IntegrationItemStatus 
    /// </summary>
    [DataContract]
    public class IntegrationItemStatusSearchCriteria : ObjectBase, IIntegrationItemStatusSearchCriteria
    {
        #region Fields

        /// <summary>
        /// Indicates search status for which connector?
        /// </summary>
        private Int16 _connectorId = -1;

        /// <summary>
        /// Indicates which view will be used
        /// </summary>
        private Boolean _includeHistoryData;

        /// <summary>
        /// Indicates the values of status types which needs to be searched.
        /// </summary>
        [DataMember]
        private Collection<OperationResultType> _statusTypes = new Collection<OperationResultType>();

        /// <summary>
        /// List of MDMObjectType and list of values to search
        /// </summary>
        [DataMember]
        private IntegrationItemStatusSearchParameterCollection _mdmObjectValues = new IntegrationItemStatusSearchParameterCollection();

        /// <summary>
        /// List of ExternalObject and list of values to search
        /// </summary>
        [DataMember]
        private IntegrationItemStatusSearchParameterCollection _externalObjectValues = new IntegrationItemStatusSearchParameterCollection();

        /// <summary>
        /// List of DimensionType and list of DimensionValue to search
        /// </summary>
        [DataMember]
        private IntegrationItemStatusSearchParameterCollection _dimensionValues = new IntegrationItemStatusSearchParameterCollection();

        /// <summary>
        /// List of StatusValues to search
        /// </summary>
        [DataMember]
        private IntegrationItemStatusSearchParameterCollection _statusValues = new IntegrationItemStatusSearchParameterCollection();

        /// <summary>
        /// ItemStatus Comments to search
        /// </summary>
        private IntegrationItemStatusSearchParameter _itemStatusComments = new IntegrationItemStatusSearchParameter();
        
        #endregion Fields

        #region Constructors


        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public IntegrationItemStatusSearchCriteria()
        {
        }

        /// <summary>
        /// Constructor with XML-format String of an IntegrationItemStatusSearchCriteria as input parameter
        /// </summary>
        /// <param name="valueAsXml"></param>
        public IntegrationItemStatusSearchCriteria(String valueAsXml)
        {
            LoadIntegrationItemStatusSearchCriteriaFromXml(valueAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates search status for which connector?
        /// </summary>
        [DataMember]
        public Int16 ConnectorId
        {
            get { return _connectorId; }
            set { _connectorId = value; }
        }

        /// <summary>
        /// Indicates which view will be used
        /// </summary>
        [DataMember]
        public Boolean IncludeHistoryData
        {
            get { return _includeHistoryData; }
            set { _includeHistoryData = value; }
        }

        /// <summary>
        /// Indicates the values of status types which needs to be searched.
        /// </summary>
        [DataMember]
        public Collection<OperationResultType> StatusTypes
        {
            get { return _statusTypes; }
            set { _statusTypes = value; }
        }

        /// <summary>
        /// List of MDMObjectType and list of values to search
        /// </summary>
        [DataMember]
        public IntegrationItemStatusSearchParameterCollection MDMObjectValues
        {
            get { return _mdmObjectValues; }
            set { _mdmObjectValues = value; }
        }

        /// <summary>
        /// List of ExternalObject and list of values to search
        /// </summary>
        [DataMember]
        public IntegrationItemStatusSearchParameterCollection ExternalObjectValues
        {
            get { return _externalObjectValues; }
            set { _externalObjectValues = value; }
        }

        /// <summary>
        /// List of DimensionType and list of DimensionValue to search
        /// </summary>
        [DataMember]
        public IntegrationItemStatusSearchParameterCollection DimensionValues
        {
            get { return _dimensionValues; }
            set { _dimensionValues = value; }
        }

        /// <summary>
        /// List of StatusValues to search
        /// </summary>
        [DataMember]
        public IntegrationItemStatusSearchParameterCollection StatusValues
        {
            get { return _statusValues; }
            set { _statusValues = value; }
        }

        /// <summary>
        /// Comments to search
        /// </summary>
        [DataMember]
        public IntegrationItemStatusSearchParameter ItemStatusComments
        {
            get { return _itemStatusComments; }
            set { _itemStatusComments = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add MDMObjectTypeId and MDMObjectId for search criteria
        /// </summary>
        /// <param name="mdmObjectTypeId">MDMObjectTypeId to search</param>
        /// <param name="commaSeparatedMDMObjectIds">Comma separated list of MDMObjectIds to search</param>
        public void AddMDMObjectTypeIdAndValues(Int16 mdmObjectTypeId, String commaSeparatedMDMObjectIds)
        {
            if (mdmObjectTypeId > 0 && !String.IsNullOrWhiteSpace(commaSeparatedMDMObjectIds))
            {
                IntegrationItemStatusSearchParameter searchParam = new IntegrationItemStatusSearchParameter();
                searchParam.SearchKey = mdmObjectTypeId;
                searchParam.SearchValues = ValueTypeHelper.SplitStringToStringCollection(commaSeparatedMDMObjectIds, ',');
                this._mdmObjectValues.Add(searchParam);
            }
        }

        /// <summary>
        /// Add ExternalObjectTypeId and ExternalId for search criteria
        /// </summary>
        /// <param name="externalObjectTypeId">ExternalObjectTypeId to search</param>
        /// <param name="commaSeparatedExternalMDMObjectIds">Comma separated list of ExternalIds to search</param>
        public void AddExternalObjectTypeIdAndValues(Int16 externalObjectTypeId, String commaSeparatedExternalMDMObjectIds)
        {
            if (externalObjectTypeId > 0 && !String.IsNullOrWhiteSpace(commaSeparatedExternalMDMObjectIds))
            {
                IntegrationItemStatusSearchParameter searchParam = new IntegrationItemStatusSearchParameter();
                searchParam.SearchKey = externalObjectTypeId;
                searchParam.SearchValues = ValueTypeHelper.SplitStringToStringCollection(commaSeparatedExternalMDMObjectIds, ',');
                this._externalObjectValues.Add(searchParam);
            }
        }

        /// <summary>
        /// Add DimensionValueId and CommaSeparatedStatusToSearch for search criteria
        /// </summary>
        /// <param name="dimensionValueId">DimensionValueId to search</param>
        /// <param name="commaSeparatedStatusToSearch">Comma separated list of SeparatedStatusIds to search</param>
        public void AddDimensionValuesAndStatus(Int32 dimensionValueId, String commaSeparatedStatusToSearch)
        {
            if (dimensionValueId > 0 && !String.IsNullOrWhiteSpace(commaSeparatedStatusToSearch))
            {
                IntegrationItemStatusSearchParameter searchParam = new IntegrationItemStatusSearchParameter();
                searchParam.SearchKey = dimensionValueId;
                searchParam.SearchValues = ValueTypeHelper.SplitStringToStringCollection(commaSeparatedStatusToSearch, ',');
                this._dimensionValues.Add(searchParam);
            }
        }

        /// <summary>
        /// Add comments of ItemStatus for search criteria
        /// </summary>
        /// <param name="comments">ItemStatus comments to search</param>
        /// <param name="searchOperator">Operator for search for comments</param>
        public void AddItemStatusComments(String comments, SearchOperator searchOperator)
        {
            this.ItemStatusComments.SearchKey = -1;
            this.ItemStatusComments.SearchValues.Add(comments);
            this.ItemStatusComments.Operator = searchOperator;
        }

        /// <summary>
        /// Add StatusValues for search criteria
        /// </summary>
        /// <param name="commaSeparatedStatusValuesToSearch">Comma separated status values to search</param>
        public void AddStatusValue(String commaSeparatedStatusValuesToSearch)
        {
            if (!String.IsNullOrWhiteSpace(commaSeparatedStatusValuesToSearch))
            {
                IntegrationItemStatusSearchParameter searchParam = new IntegrationItemStatusSearchParameter();
                searchParam.SearchKey = -1;
                searchParam.SearchValues = ValueTypeHelper.SplitStringToStringCollection(commaSeparatedStatusValuesToSearch, ',');
                this._statusValues.Add(searchParam);
            }
        }

        /// <summary>
        /// Get Xml representation of IntegrationItemStatusSearchCriteria
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXml()
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //IntegrationItemStatusSearchCriteria node start
            xmlWriter.WriteStartElement("IntegrationItemStatusSearchCriteria");

            xmlWriter.WriteAttributeString("ConnectorId", ConnectorId.ToString(culture));
            xmlWriter.WriteAttributeString("IncludeHistoryData", this.IncludeHistoryData.ToString().ToLowerInvariant());

            #region StatusTypes part

            if (this.StatusTypes != null)
            {
                var statusTypes = new StringBuilder();
                foreach (var statusType in this.StatusTypes)
                {
                    statusTypes.Append(statusType.ToString("G"));
                }
                if (this.StatusTypes.Any())
                {
                    statusTypes.Length = statusTypes.Length - 1;
                }

                xmlWriter.WriteAttributeString("StatusTypes", statusTypes.ToString());
            }

            #endregion

            #region MDMObjectValues part

            if (this.MDMObjectValues != null)
            {
                xmlWriter.WriteStartElement("MDMObjectValues");

                foreach (IntegrationItemStatusSearchParameter mdmObjectValue in this.MDMObjectValues)
                {
                    xmlWriter.WriteRaw(mdmObjectValue.ToXml());
                }

                xmlWriter.WriteEndElement();
            }

            #endregion

            #region ExternalObjectValues part

            if (this.ExternalObjectValues != null)
            {
                xmlWriter.WriteStartElement("ExternalObjectValues");

                foreach (IntegrationItemStatusSearchParameter externalObjectValue in this.ExternalObjectValues)
                {
                    xmlWriter.WriteRaw(externalObjectValue.ToXml());
                }

                xmlWriter.WriteEndElement();
            }

            #endregion

            #region DimensionValues part

            if (this.DimensionValues != null)
            {
                xmlWriter.WriteStartElement("DimensionValues");

                foreach (IntegrationItemStatusSearchParameter dimensionValue in this.DimensionValues)
                {
                    xmlWriter.WriteRaw(dimensionValue.ToXml());
                }

                xmlWriter.WriteEndElement();
            }

            #endregion

            #region StatusValues part

            if (this.StatusValues != null)
            {
                xmlWriter.WriteStartElement("StatusValues");

                foreach (IntegrationItemStatusSearchParameter statusValue in this.StatusValues)
                {
                    xmlWriter.WriteRaw(statusValue.ToXml());
                }

                xmlWriter.WriteEndElement();
            }

            #endregion

            #region ItemStatusComments part

            if (this.ItemStatusComments != null)
            {
                xmlWriter.WriteStartElement("ItemStatusComments");

                xmlWriter.WriteRaw(this.ItemStatusComments.ToXml());

                xmlWriter.WriteEndElement();
            }

            #endregion

            //IntegrationItemStatusSearchCriteria node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            String xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #region Private Methods

        private void LoadIntegrationItemStatusSearchCriteriaFromXml(String valueAsXml)
        {
            if (!String.IsNullOrEmpty(valueAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valueAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "IntegrationItemStatusSearchCriteria")
                        {
                            #region Read IntegrationItemStatusSearchCriteria properties

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("ConnectorId"))
                                {
                                    this.ConnectorId = ValueTypeHelper.Int16TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("IncludeHistoryData"))
                                {
                                    this.IncludeHistoryData = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("StatusTypes"))
                                {
                                    foreach (var statusType in reader.ReadContentAsString().Split(','))
                                    {
                                        this.StatusTypes.Add((OperationResultType)Enum.Parse(typeof(OperationResultType), statusType));
                                    }
                                }
                            }

                            #endregion Read IntegrationItemStatusSearchCriteria properties
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMObjectValues")
                        {
                            #region Read MDMObjectValues

                            String mdmObjectValues = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(mdmObjectValues))
                            {
                                this.MDMObjectValues = new IntegrationItemStatusSearchParameterCollection(mdmObjectValues);
                            }

                            #endregion Read MDMObjectValues
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExternalObjectValues")
                        {
                            #region Read ExternalObjectValues

                            String externalObjectValues = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(externalObjectValues))
                            {
                                this.ExternalObjectValues = new IntegrationItemStatusSearchParameterCollection(externalObjectValues);
                            }

                            #endregion Read ExternalObjectValues
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DimensionValues")
                        {
                            #region Read DimensionValues

                            String dimensionValues = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(dimensionValues))
                            {
                                this.DimensionValues = new IntegrationItemStatusSearchParameterCollection(dimensionValues);
                            }

                            #endregion Read DimensionValues
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "StatusValues")
                        {
                            #region Read StatusValues

                            String statusValues = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(statusValues))
                            {
                                this.StatusValues = new IntegrationItemStatusSearchParameterCollection(statusValues);
                            }

                            #endregion Read StatusValues
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ItemStatusComments")
                        {
                            #region Read ItemStatusComments

                            String stemStatusComments = reader.ReadOuterXml();
                            if (!String.IsNullOrEmpty(stemStatusComments))
                            {
                                this.ItemStatusComments = new IntegrationItemStatusSearchParameter(stemStatusComments);
                            }

                            #endregion Read ItemStatusComments
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
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

        #endregion Methods
    }
}
