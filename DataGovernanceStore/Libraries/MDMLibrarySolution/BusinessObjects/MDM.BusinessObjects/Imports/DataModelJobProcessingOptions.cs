using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies data model JobProcessingOptions which specifies various flags and indications to data model processing logic
    /// </summary>
    [DataContract]
    public class DataModelJobProcessingOptions : ObjectBase
    {
         #region Fields

        /// <summary>
        /// Indicates batch size for the processing.
        /// </summary>
        private Int32 _batchSize = -1;

        /// <summary>
        /// Indicates ProcessingType
        /// </summary>
        private ImportProcessingType _processingType = ImportProcessingType.ValidateAndProcess;

        /// <summary>
        /// Indicates number of threads for data model import
        /// </summary>
        private Int32 _numberofThreads = 1;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataModelJobProcessingOptions()
            : base()
        { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public DataModelJobProcessingOptions(String valuesAsXml)
        {
            LoadDataModelJobProcessingOptions(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates the Procesing type of the Import job
        /// </summary>
        [DataMember]
        public ImportProcessingType DataModelImportProcessingType
        {
            get { return _processingType; }
            set { _processingType = value; }
        }

        /// <summary>
        /// Indicates the batch size for the data model record thread.
        /// </summary>
        [DataMember]
        public Int32 BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        /// <summary>
        /// Indicates thread size for data model import
        /// </summary>
        public Int32 NumberOfThreads
        {
            get { return _numberofThreads; }
            set { _numberofThreads = value; }
        }

        #endregion Properties

        #region Methods
         
        #region Public methods

        /// <summary>
        /// Represents DataModel JobProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current DataModel JobProcessingOptions object instance</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("DataModelJobProcessingOptions");

            xmlWriter.WriteAttributeString("BatchSize", this.BatchSize.ToString());
            xmlWriter.WriteAttributeString("NumberofDataModelThreads", this.NumberOfThreads.ToString());
            xmlWriter.WriteAttributeString("ImportProcessingType", this._processingType.ToString());

            //JobProcessingOptions end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Represents DataModel JobProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current data model JobProcessingOptions object instance</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        #endregion Public methods

        #region Private methods

        private void LoadDataModelJobProcessingOptions(String valuesAsXml)
        { 
            #region Sample Xml
            //<DataModelJobProcessingOptions ImportProcessingType="ValidationOnly" BatchSize="100" />
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelJobProcessingOptions")
                    {
                        #region Read JobProcessingOptions Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("BatchSize"))
                            {
                                this.BatchSize = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0); ;
                            }

                            if (reader.MoveToAttribute("NumberofDataModelThreads"))
                            {
                                this.NumberOfThreads = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 1); 
                            }

                            if (reader.MoveToAttribute("ImportProcessingType"))
                            {
                                ImportProcessingType importProcessingType = ImportProcessingType.ValidateAndProcess;
                                Enum.TryParse(reader.ReadContentAsString(), true, out importProcessingType);
                                this.DataModelImportProcessingType = importProcessingType;
                            }
                        }

                        #endregion
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

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">DataModelJobProcessingOptions Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(DataModelJobProcessingOptions objectToBeCompared)
        {
            if (this.BatchSize != objectToBeCompared.BatchSize)
                return false;

            if (this.NumberOfThreads != objectToBeCompared.NumberOfThreads)
                return false;

            if (this.DataModelImportProcessingType != objectToBeCompared.DataModelImportProcessingType)
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = this.BatchSize.GetHashCode() ^ this.DataModelImportProcessingType.GetHashCode() ^ this.NumberOfThreads.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Determines whether invoking object is superset of parameterised object.
        /// </summary>
        /// <param name="subsetDataModelJobProcessingOptions">Indicate the subset data model job processing option object</param>
        /// <returns>Returns true if it is superset, otherwise false.</returns>
        public Boolean IsSuperSetOf(DataModelJobProcessingOptions subsetDataModelJobProcessingOptions)
        {
            if (this.BatchSize != subsetDataModelJobProcessingOptions.BatchSize)
                return false;

            if (this.NumberOfThreads != subsetDataModelJobProcessingOptions.NumberOfThreads)
                return false;

            if (this.DataModelImportProcessingType != subsetDataModelJobProcessingOptions.DataModelImportProcessingType)
                return false;

            return true;
        }
        #endregion Private methods

        #endregion Methods
    }
}
