using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Contains properties for connector's run time configuration. E.g., Assembly name, class name implementing IConnector methods
    /// </summary>
    [DataContract]
    public class RunTimeSpecifications : IRunTimeSpecifications
    {
        #region Fields

        /// <summary>
        /// Indicates name of assembly containing implementation of IConnector for given connector
        /// </summary>
        private String _assemblyName = String.Empty;

        /// <summary>
        /// Indicates name of class containing implementation of IConnector for given connector
        /// </summary>
        private String _className = String.Empty;

        /// <summary>
        /// Indicates file watcher folder where files will be dropped for this connector
        /// </summary>
        private String _fileWatcherFolderName = String.Empty;

        /// <summary>
        /// Decides whether Orchestration is done by core system or connector implementation team will do it.
        /// True : Core will call the connector methods in sequence.
        /// False : Core will only call ProcessInboundMessage / ProcessOutboundMessage. Calling rest of IConnector methods will be done by external system.
        /// </summary>
        private Boolean _useInplaceOrchestration = true;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RunTimeSpecifications()
            : base()
        {
        }

        /// <summary>
        /// Xml constructor
        /// </summary>
        /// <param name="valuesAsXml">Value in xml format</param>
        public RunTimeSpecifications(String valuesAsXml)
        {
            LoadRunTimeSpecifications(valuesAsXml);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Indicates name of assembly containing implementation of IConnector for given connector
        /// </summary>
        [DataMember]
        public String AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        /// <summary>
        /// Indicates name of class containing implementation of IConnector for given connector
        /// </summary>
        [DataMember]
        public String ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        /// <summary>
        /// Indicates file watcher folder where files will be dropped for this connector
        /// </summary>
        [DataMember]
        public String FileWatcherFolderName
        {
            get { return _fileWatcherFolderName; }
            set { _fileWatcherFolderName = value; }
        }

        /// <summary>
        /// Decides whether Orchestration is done by core system or connector implementation team will do it.
        /// True : Core will call the connector methods in sequence.
        /// False : Core will only call ProcessInboundMessage / ProcessOutboundMessage. Calling rest of IConnector methods will be done by external system.
        /// </summary>
        [DataMember]
        public Boolean UseInplaceOrchestration
        {
            get { return _useInplaceOrchestration; }
            set { _useInplaceOrchestration = value; }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents RunTimeSpecifications in Xml format
        /// </summary>
        public String ToXml()
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            String xml = String.Empty;

            xmlWriter.WriteStartElement("RunTimeSpecifications");

            xmlWriter.WriteAttributeString("AssemblyName", this.AssemblyName);
            xmlWriter.WriteAttributeString("ClassName", this.ClassName);
            xmlWriter.WriteAttributeString("FileWatcherFolderName", this.FileWatcherFolderName);
            xmlWriter.WriteAttributeString("UseInplaceOrchestration", this.UseInplaceOrchestration.ToString().ToLowerInvariant());

            //RunTimeSpecifications end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone the RunTimeSpecifications object and return new instance with the same values.
        /// </summary>
        /// <returns>New cloned IRunTimeSpecifications</returns>
        public IRunTimeSpecifications Clone()
        {
            RunTimeSpecifications clonedRunTimeSpecifications = new RunTimeSpecifications();
            clonedRunTimeSpecifications._assemblyName = this._assemblyName;
            clonedRunTimeSpecifications._className = this._className;
            clonedRunTimeSpecifications._fileWatcherFolderName = this._fileWatcherFolderName;
            clonedRunTimeSpecifications._useInplaceOrchestration = this._useInplaceOrchestration;

            return clonedRunTimeSpecifications;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runTimeSpecifications"></param>
        /// <returns></returns>
        public Boolean IsSuperSetOf(RunTimeSpecifications runTimeSpecifications)
        {
            if (runTimeSpecifications != null)
            {
                if (this.AssemblyName != runTimeSpecifications.AssemblyName)
                {
                    return false;
                }
                if (this.ClassName != runTimeSpecifications.ClassName)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize RunTimeSpecifications from xml.
        /// </summary>
        /// <param name="valuesAsXml">RunTimeSpecifications in xml format</param>
        private void LoadRunTimeSpecifications(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RunTimeSpecifications" && reader.IsStartElement())
                        {
                            #region Read RunTimeSpecifications Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("AssemblyName"))
                                    this.AssemblyName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("ClassName"))
                                    this.ClassName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("FileWatcherFolderName"))
                                    this.FileWatcherFolderName = reader.ReadContentAsString();

                                if (reader.MoveToAttribute("UseInplaceOrchestration"))
                                    this.UseInplaceOrchestration = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), true);

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

        #endregion Methods
    }
}