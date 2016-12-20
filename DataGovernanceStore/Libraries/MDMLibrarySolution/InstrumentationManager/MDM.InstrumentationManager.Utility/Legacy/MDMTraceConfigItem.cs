using System;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace MDM.Instrumentation.Utility
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies configurations for MDM Trace Source Item
    /// </summary>
    [DataContract]
    public class MDMTraceConfigItem : IMDMTraceConfigItem
    {
        #region Fields

        /// <summary>
        /// Field specifying trace source
        /// </summary>
        private MDMTraceSource _traceSource = MDMTraceSource.General;

        /// <summary>
        /// Field specifying whether to consider activity tracing 
        /// </summary>
        private Boolean _logActivityTrace = false;

        /// <summary>
        /// Field specifying whether to consider error logging 
        /// </summary>
        private Boolean _logError = false;

        /// <summary>
        /// Field specifying whether to consider warnings logging
        /// </summary>
        private Boolean _logWarning = false;

        /// <summary>
        /// Field specifying whether to consider info logging
        /// </summary>
        private Boolean _logInformation = false;

        /// <summary>
        /// Field specifying whether to consider verbose logging
        /// </summary>
        private Boolean _logVerbose = false;

        #endregion

        #region Properties

        /// <summary>
        /// Property specifying trace source
        /// </summary>
        [DataMember]
        public MDMTraceSource TraceSource
        {
            get
            {
                return _traceSource;
            }
            set
            {
                _traceSource = value;
            }
        }

        /// <summary>
        /// Property specifying whether to consider activity tracing 
        /// </summary>
        [DataMember]
        public Boolean LogActivityTrace
        {
            get
            {
                return _logActivityTrace;
            }
            set
            {
                _logActivityTrace = value;
            }
        }

        /// <summary>
        /// Property specifying whether to consider error logging 
        /// </summary>
        [DataMember]
        public Boolean LogError
        {
            get
            {
                return _logError;
            }
            set
            {
                _logError = value;
            }
        }

        /// <summary>
        /// Property specifying whether to consider warning logging 
        /// </summary>
        [DataMember]
        public Boolean LogWarning
        {
            get
            {
                return _logWarning;
            }
            set
            {
                _logWarning = value;
            }
        }

        /// <summary>
        /// Property specifying whether to consider information logging 
        /// </summary>
        [DataMember]
        public Boolean LogInformation
        {
            get
            {
                return _logInformation;
            }
            set
            {
                _logInformation = value;
            }
        }

        /// <summary>
        /// Property specifying whether to consider verbose logging 
        /// </summary>
        [DataMember]
        public Boolean LogVerbose
        {
            get
            {
                return _logVerbose;
            }
            set
            {
                _logVerbose = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes MDMTraceConfigItem object with default parameters
        /// </summary>
        public MDMTraceConfigItem()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceSource"></param>
        /// <param name="logActivityTrace"></param>
        /// <param name="logError"></param>
        /// <param name="logWarning"></param>
        /// <param name="logInformation"></param>
        /// <param name="logVerbose"></param>
        public MDMTraceConfigItem(MDMTraceSource traceSource, Boolean logActivityTrace, Boolean logError, Boolean logWarning, Boolean logInformation, Boolean logVerbose)
        {
            this._traceSource = traceSource;
            this._logActivityTrace = logActivityTrace;
            this._logError = logError;
            this._logWarning = logWarning;
            this._logInformation = logInformation;
            this._logVerbose = logVerbose;
        }

        /// <summary>
        /// Initializes MDMTraceConfigItem object with the values provided as Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml string having values</param>
        public MDMTraceConfigItem(String valuesAsXml)
        {
            LoadMDMTraceConfigItem(valuesAsXml);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>True if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj != null && obj is MDMTraceConfigItem)
            {
                MDMTraceConfigItem objectToBeCompared = obj as MDMTraceConfigItem;

                if (this.TraceSource != objectToBeCompared.TraceSource)
                    return false;

                if (this.LogActivityTrace != objectToBeCompared.LogActivityTrace)
                    return false;

                if (this.LogError != objectToBeCompared.LogError)
                    return false;

                if (this.LogWarning != objectToBeCompared.LogWarning)
                    return false;

                if (this.LogInformation != objectToBeCompared.LogInformation)
                    return false;

                if (this.LogVerbose != objectToBeCompared.LogVerbose)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = base.GetHashCode() ^ this.TraceSource.GetHashCode() ^ this.LogActivityTrace.GetHashCode() ^ this.LogError.GetHashCode() ^ this.LogInformation.GetHashCode() ^ this.LogVerbose.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Get Xml representation of MDM Trace Config Item
        /// </summary>
        /// <returns>Xml representation of MDM Trace Config Item</returns>
        public String ToXml()
        {
            String mdmTraceConfigItemXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MDM Trace Config Item node start
            xmlWriter.WriteStartElement("MDMTraceConfigItem");

            #region Write MDM Trace Config Item properties

            xmlWriter.WriteAttributeString("TraceSource", this.TraceSource.ToString());
            xmlWriter.WriteAttributeString("LogActivityTrace", this.LogActivityTrace.ToString());
            xmlWriter.WriteAttributeString("LogError", this.LogError.ToString());
            xmlWriter.WriteAttributeString("LogWarning", this.LogWarning.ToString());
            xmlWriter.WriteAttributeString("LogInformation", this.LogInformation.ToString());
            xmlWriter.WriteAttributeString("LogVerbose", this.LogVerbose.ToString());

            #endregion

            //MDM Trace Config Item node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            mdmTraceConfigItemXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return mdmTraceConfigItemXml;
        }

        /// <summary>
        /// Get Xml representation of MDM Trace Config Item based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of MDM Trace Config Item</returns>
        public String ToXml(ObjectSerialization objectSerialization)
        {
            String mdmTraceConfigItemXml = String.Empty;

            //TODO:: Write logic as per the different object serialization type whenever needed..
            mdmTraceConfigItemXml = this.ToXml();
           
            return mdmTraceConfigItemXml;
        }

        #endregion

        #region Private Methods

        private void LoadMDMTraceConfigItem(String valuesAsXml)
        {
            #region Sample Xml

            /*
             * <MDMTraceConfigItem TraceSource="Application" LogActivityTrace="true" LogError="true" LogWarning="true" LogInformation="false" LogVerbose="true">
             * </MDMTraceConfigItem>
             */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMTraceConfigItem")
                        {
                            #region Read MDMTraceConfigItem

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("TraceSource"))
                                {
                                    MDMTraceSource traceSource = MDMTraceSource.General;
                                    Enum.TryParse<MDMTraceSource>(reader.ReadContentAsString(), out traceSource);
                                    this.TraceSource = traceSource;
                                }

                                if (reader.MoveToAttribute("LogActivityTrace"))
                                {
                                    this.LogActivityTrace = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("LogError"))
                                {
                                    this.LogError = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("LogWarning"))
                                {
                                    this.LogWarning = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("LogInformation"))
                                {
                                    this.LogInformation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("LogVerbose"))
                                {
                                    this.LogVerbose = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                            }
                            else
                            {
                                reader.Read();
                            }

                            #endregion
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

        #endregion
    }
}
