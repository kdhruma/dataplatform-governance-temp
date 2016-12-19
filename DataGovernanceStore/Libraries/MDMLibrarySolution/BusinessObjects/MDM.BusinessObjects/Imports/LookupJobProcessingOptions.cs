using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Imports
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies lookup JobProcessingOptions which specifies various flags and indications to lookup processing logic
    /// </summary>
    [DataContract]
    public class LookupJobProcessingOptions : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Indicates the number of lookup threads/tasks that will be used for the import processing.
        /// </summary>
        private Int32 _numberofLookupThreads = -1;

        /// <summary>
        /// Indicates the number of Attribute threads per entity threads that will be used for the initial load processing.
        /// </summary>
        private Int32 _numberofLookupRecordThreadsPerLookupThread = -1;

        /// <summary>
        /// Indicates batch size for the processing.
        /// </summary>
        private Int32 _batchSize = -1;


        /// <summary>
        /// Indicates if we need to validate the schema or not.
        /// </summary>
        private Boolean _validateSchema = false;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LookupJobProcessingOptions()
            : base()
        { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public LookupJobProcessingOptions(String valuesAsXml)
        {
            LoadLookupJobProcessingOptions(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates the number of lookup threads.
        /// </summary>
        [DataMember]
        public Int32 NumberofLookupThreads
        {
            get { return _numberofLookupThreads; }
            set { _numberofLookupThreads = value; }
        }

        /// <summary>
        /// Indicates the number of lookup record threads per lookup thread.
        /// </summary>
        [DataMember]
        public Int32 NumberofLookupRecordThreadsPerLookupThread
        {
            get { return _numberofLookupRecordThreadsPerLookupThread; }
            set { _numberofLookupRecordThreadsPerLookupThread = value; }
        }

        /// <summary>
        /// Indicates the batch size for the lookup record thread.
        /// </summary>
        [DataMember]
        public Int32 BatchSize
        {
            get { return _batchSize; }
            set { _batchSize = value; }
        }

        /// <summary>
        /// Indicates if we need to validate the schema or not.
        /// </summary>
        [DataMember]
        public Boolean ValidateSchema
        {
            get { return _validateSchema; }
            set { _validateSchema = value; }
        }
        #endregion Properties

        #region Methods

        #region Public methods

        /// <summary>
        /// Represents Lookup JobProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current Lookup JobProcessingOptions object instance</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Attribute node start
            xmlWriter.WriteStartElement("LookupJobProcessingOptions");

            xmlWriter.WriteAttributeString("NumberofLookupThreads", this.NumberofLookupThreads.ToString());
            xmlWriter.WriteAttributeString("NumberofLookupRecordThreadsPerLookupThread", this.NumberofLookupRecordThreadsPerLookupThread.ToString());
            xmlWriter.WriteAttributeString("BatchSize", this.BatchSize.ToString());
            xmlWriter.WriteAttributeString("ValidateSchema", this.ValidateSchema.ToString().ToLower());
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
        /// Represents Lookup JobProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current lookup JobProcessingOptions object instance</returns>
        public String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        #endregion Public methods

        #region Private methods

        private void LoadLookupJobProcessingOptions(String valuesAsXml)
        {
            #region Sample Xml
            //<LookupJobProcessingOptions NumberofLookupThreads="2" NumberofRecordThreadsPerLookupThread="1" BatchSize="100" />
            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "LookupJobProcessingOptions")
                    {
                        #region Read JobProcessingOptions Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("NumberofLookupThreads"))
                            {
                                this.NumberofLookupThreads = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0); ;
                            }

                            if (reader.MoveToAttribute("NumberofLookupRecordThreadsPerLookupThread"))
                            {
                                this.NumberofLookupRecordThreadsPerLookupThread = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0); ;
                            }

                            if (reader.MoveToAttribute("BatchSize"))
                            {
                                this.BatchSize = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0); ;
                            }
                            if (reader.MoveToAttribute("ValidateSchema"))
                            {
                                this.ValidateSchema = reader.ReadContentAsBoolean();
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
        /// <param name="objectToBeCompared">LookupJobProcessingOptions Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(LookupJobProcessingOptions objectToBeCompared)
        {
            if (this.NumberofLookupThreads != objectToBeCompared.NumberofLookupThreads)
                return false;

            if (this.NumberofLookupRecordThreadsPerLookupThread != objectToBeCompared.NumberofLookupRecordThreadsPerLookupThread)
                return false;

            if (this.BatchSize != objectToBeCompared.BatchSize)
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = this.NumberofLookupThreads.GetHashCode() ^ this.NumberofLookupRecordThreadsPerLookupThread.GetHashCode() ^ this.BatchSize.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Determines whether invoking object is superset of parameterised object.
        /// </summary>
        /// <param name="subsetLookupJobProcessingOptions">Indicate the subset lookup job processing option object</param>
        /// <returns>Returns true if it is superset, otherwise false.</returns>
        public Boolean IsSuperSetOf(LookupJobProcessingOptions subsetLookupJobProcessingOptions)
        {
            if (this.NumberofLookupThreads != subsetLookupJobProcessingOptions.NumberofLookupThreads)
                return false;

            if (this.NumberofLookupRecordThreadsPerLookupThread != subsetLookupJobProcessingOptions.NumberofLookupRecordThreadsPerLookupThread)
                return false;

            if (this.BatchSize != subsetLookupJobProcessingOptions.BatchSize)
                return false;

            return true;
        }
        #endregion Private methods

        #endregion Methods
    }
}
