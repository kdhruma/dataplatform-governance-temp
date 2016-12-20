using System;
using System.Runtime.Serialization;
using ProtoBuf;
using System.Xml;
using System.IO;

namespace MDM.Core
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ProtoContract]
    [Serializable()]
    public class TraceSettings
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        private Boolean _isTracingEnabled = false;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        private TracingMode _tracingMode = TracingMode.None;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        private TracingLevel _tracingLevel = TracingLevel.None;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public TraceSettings()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        public TraceSettings(String valuesAsXml)
        {
            LoadData(valuesAsXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isTracingEnabled"></param>
        /// <param name="tracingMode"></param>
        /// <param name="tracingLevel"></param>
        public TraceSettings(Boolean isTracingEnabled, TracingMode tracingMode, TracingLevel tracingLevel)
        {
            _isTracingEnabled = isTracingEnabled;
            _tracingMode = tracingMode;
            _tracingLevel = tracingLevel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsTracingEnabled
        {
            get { return _isTracingEnabled; }
            set { _isTracingEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TracingMode TracingMode
        {
            get { return _tracingMode; }
            set { _tracingMode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TracingLevel TracingLevel
        {
            get { return _tracingLevel; }
            set { _tracingLevel = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsBasicTracingEnabled
        {
            get { return _isTracingEnabled; }
            set { _isTracingEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsDetailTracingEnabled
        {
            get { return _isTracingEnabled && _tracingLevel == TracingLevel.Detail; }
            set { _isTracingEnabled = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates trace settings
        /// </summary>
        /// <param name="isTracingEnabled"></param>
        /// <param name="tracingMode"></param>
        /// <param name="tracingLevel"></param>
        public void UpdateSettings(Boolean isTracingEnabled, TracingMode tracingMode, TracingLevel tracingLevel)
        {
            _isTracingEnabled = isTracingEnabled;
            _tracingMode = tracingMode;
            _tracingLevel = tracingLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TraceSettings Clone()
        {
            var clonedTraceSettings = new TraceSettings();

            clonedTraceSettings._isTracingEnabled = _isTracingEnabled;
            clonedTraceSettings._tracingMode = _tracingMode;
            clonedTraceSettings._tracingLevel = _tracingLevel;

            return clonedTraceSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ToXml()
        {
            String traceSettingsXml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //Service Context Map node start
            xmlWriter.WriteStartElement("TraceSettings");

            #region Write caller Context properties

            xmlWriter.WriteAttributeString("IsTracingEnabled", this.IsTracingEnabled.ToString());
            xmlWriter.WriteAttributeString("TracingMode", this.TracingMode.ToString());
            xmlWriter.WriteAttributeString("TracingLevel", this.TracingLevel.ToString());
            
            #endregion

            //node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //Get the actual XML
            traceSettingsXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return traceSettingsXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadData(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "TraceSettings")
                        {
                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("IsTracingEnabled"))
                                {
                                    _isTracingEnabled = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("TracingMode"))
                                {
                                    String tracingModeAsString = reader.ReadContentAsString();
                                    ValueTypeHelper.EnumTryParse<TracingMode>(tracingModeAsString, true, out _tracingMode);
                                }

                                if (reader.MoveToAttribute("TracingLevel"))
                                {
                                    String tracingLevelAsString = reader.ReadContentAsString();
                                    ValueTypeHelper.EnumTryParse<TracingLevel>(tracingLevelAsString, true, out _tracingLevel);
                                }
                            }
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
    }
}