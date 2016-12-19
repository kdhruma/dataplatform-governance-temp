using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;
    using System.IO;
    using System.Xml;
    
    /// <summary>
    /// 
    /// </summary>
    public class ContainerContext:  ObjectBase, IContainerContext
    {
        #region Fields

        /// <summary>
        /// Field denoting whether atributes are to be loaded or not
        /// </summary>
        Boolean _loadAttributes = false;

        /// <summary>
        /// Field denoting a boolean value indicating whether to apply security or not.
        /// </summary>
        private Boolean _applySecurity = true;

        /// <summary>
        /// Field denoting a boolean flag indicating whether to include approved container copy or not.
        /// </summary>
        private Boolean _includeApproved = false;

        #endregion Fields

        #region Properties

        ///<summary>
        /// Property denoting whether atributes are to be loaded or not
        ///</summary>
        [DataMember]
        public Boolean LoadAttributes
        {
            get
            {
                return _loadAttributes;
            }
            set
            {
                _loadAttributes = value;
            }
        }

        ///<summary>
        /// Property denoting whether to apply user permissions or not while loading containers
        ///</summary>
        [DataMember]
        public Boolean ApplySecurity
        {
            get
            {
                return _applySecurity;
            }
            set
            {
                _applySecurity = value;
            }
        }

        ///<summary>
        /// Property denoting a boolean flag indicating whether to include approved container copy or not.
        ///</summary>
        [DataMember]
        public Boolean IncludeApproved
        {
            get
            {
                return _includeApproved;
            }
            set
            {
                _includeApproved = value;
            }
        }

        #endregion Properties

        #region Constructor
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ContainerContext()
            : base()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        public ContainerContext(Boolean loadAttributes)
            : base()
        {
            this.LoadAttributes = loadAttributes;
        }

        /// <summary>
        /// 
        /// </summary>
        public ContainerContext(Boolean loadAttributes, Boolean applySecurity, Boolean includeApproved = false)
            : base()
        {
            this.LoadAttributes = loadAttributes;
            this.ApplySecurity = applySecurity;
            this.IncludeApproved = includeApproved;
        }

        
        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public ContainerContext(String valuesAsXml)
        {
            LoadContainerContext(valuesAsXml);
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Represents ContainerContext  in Xml format
        /// </summary>
        /// <returns>String representation of current ContainerContext object</returns>
        public String ToXml()
        {
            String xml = String.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
           
            //Attribute node start
            xmlWriter.WriteStartElement("ContainerContext");

            xmlWriter.WriteAttributeString("LoadAttributes", this.LoadAttributes.ToString());
            xmlWriter.WriteAttributeString("ApplySecurity", this.ApplySecurity.ToString());
            xmlWriter.WriteAttributeString("IncludeApproved", this.IncludeApproved.ToString());

            //ContainerContext end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        #endregion Public methods

        #region Private Methods

        private void LoadContainerContext(String valuesAsXml)
        {
            #region Sample Xml

            #endregion Sample Xml

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerContext")
                    {
                        #region Read EntityContext Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("LoadAttributes"))
                            {
                                this.LoadAttributes = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                            }

                            if (reader.MoveToAttribute("ApplySecurity"))
                            {
                                this.ApplySecurity = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), true);
                            }

                            if (reader.MoveToAttribute("IncludeApproved"))
                            {
                                this.IncludeApproved = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), true);
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
        #endregion Private Methods

    }
}
